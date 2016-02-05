using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace TestHarness
{
    
    public class TestRunResult
    {
        public int Total { get; set;}
        public int NotRun { get; set;}
        public int SetupFailed { get; set;}
        public int Passed { get; set;}
        public int Failed { get; set;}
    }

    public class TestRunProgress
    {
        public string TestName { get; set; }
        public TestResult TestResult { get; set; }
        public TestRunResult OverallResult { get; set;  }
    }

    public enum RunMode { Async, Sync};

    public class TestSuite
    {
        public RunMode RunMode { get; private set; }
        public int NumberOfThreads { get; private set; }
        public List<UnitTest> UnitTests { get; private set; }

        public TestSuite()
        {
            UnitTests = new List<UnitTest>();
        }

        public TestSuite(string fileName)
            : this()
        {
            var directory = Path.GetDirectoryName(fileName);

            var xml = new XmlDocument();
            xml.Load(fileName);

            RunMode = RunMode.Async;
            NumberOfThreads = 5;
            var runModeAttribute = xml.DocumentElement.Attributes["runmode"];
            if (runModeAttribute != null)
            {
                if (runModeAttribute.Value == "sync")
                {
                    RunMode = RunMode.Sync;
                    NumberOfThreads = 1;
                }
            }

            var unitTests = xml.DocumentElement.SelectNodes("unittest");
            foreach (XmlNode unitTest in unitTests)
                UnitTests.Add(new UnitTest(unitTest, directory));

        }
    

        public async Task<TestResult> RunTestAsync(UnitTest unitTest, Dictionary<string, string> variables, string outputFolder, CancellationToken cancellationToken, IProgress<TestRunProgress> progress)
        {
            if (progress != null)
            {
                progress.Report(new TestRunProgress()
                {
                    TestName = unitTest.Name,
                    TestResult = TestResult.InProgress
                });
            }

            /* Create the output folder */
            var testRunOutputFolder = Path.Combine(outputFolder, unitTest.Name);
            Directory.CreateDirectory(testRunOutputFolder);

            var testVariables = new Dictionary<string, string>(variables);
            var testResult = await unitTest.RunAsync(testVariables, testRunOutputFolder, cancellationToken, null);

            if (progress != null)
            {
                progress.Report(new TestRunProgress()
                {
                    TestName = unitTest.Name,
                    TestResult = testResult
                });
            } 

            return testResult;
        }

        public async Task<TestRunResult> RunAllAsync(Dictionary<string, string> variables, string outputFolder, CancellationToken cancellationToken, IProgress<TestRunProgress> progress)
        {

            var testRunOutputFolder = Path.Combine(outputFolder, "Test Run " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            Directory.CreateDirectory(testRunOutputFolder);

            TestRunResult result = new TestRunResult();

            var unitTestTasks = new List<Task<TestResult>>();
            int nextTask = 0;
            while ((nextTask < NumberOfThreads) && (nextTask < UnitTests.Count))
            {
                var unitTest = UnitTests[nextTask++];
                unitTestTasks.Add(RunTestAsync(unitTest, variables, testRunOutputFolder, cancellationToken, progress));
            }
            
            while (unitTestTasks.Count > 0)
            {
                var unitTestTask = await Task.WhenAny(unitTestTasks);
                unitTestTasks.Remove(unitTestTask);

                var testResult = await unitTestTask;

                if (testResult == TestResult.NotRun)
                    result.NotRun++;
                if (testResult == TestResult.SetupFailed)
                    result.SetupFailed++;
                if (testResult == TestResult.Passed)
                    result.Passed++;
                if (testResult == TestResult.Failed)
                    result.Failed++;

                result.Total++;
                
                if (nextTask < UnitTests.Count)
                {
                    var unitTest = UnitTests[nextTask++];
                    unitTestTasks.Add(RunTestAsync(unitTest, variables, testRunOutputFolder, cancellationToken, progress));
                }
            }

            return result;
        } 
    }
}

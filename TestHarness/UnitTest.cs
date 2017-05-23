using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

using PayrollExchange;

namespace TestHarness
{

    public class UnitTest : ITestItem
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; set; }
        public string Description { get; set; }   
        
        public List<ITask> SetupTasks { get; private set; }
        public List<ITask> TearDownTasks { get; private set; }

        public List<TestCase> TestCases { get; private set; }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TestResult Result { get; private set; }
        public TestSummary Summary { get; private set; }

        public int TestCount
        {
            get
            {
                return TestCases.Count;
            }
        }

        public UnitTest()
        {
            SetupTasks = new List<ITask>();
            TearDownTasks = new List<ITask>();
            TestCases = new List<TestCase>();

            StartTime = new DateTime(0001, 01, 01);
            EndTime = new DateTime(0001, 01, 01);
            Result = TestResult.NotRun;
            Summary = new TestSummary();
        }

        public async Task<TestResult> RunAsync(Dictionary<string, string> variables, string outputFolder, CancellationToken cancellationToken, IProgress<TestProgress> progress)
        {            
            StartTime = DateTime.Now;
            EndTime = new DateTime(0001, 01, 01);
            Result = TestResult.InProgress;
            Summary.Total = TestCount;
            Summary.NotRun = Summary.Total;
            Summary.SetupFailed = 0;
            Summary.Passed = 0;
            Summary.Failed = 0;

            if (progress != null)
                progress.Report(new TestProgress(Id, Result));

            bool testSuccessful;
            var unitTestFolder = Path.Combine(outputFolder, Id.ToString());
            Directory.CreateDirectory(unitTestFolder);

            var fileNameGenerator = new TestOutputFileNameGenerator(unitTestFolder);

            // Run setup tasks
            foreach (ITask task in SetupTasks)
            {
                try
                {
                    testSuccessful = await task.RunAsync(variables, fileNameGenerator, cancellationToken, progress);
                }
                catch (Exception)
                {
                    testSuccessful = false;
                }

                if (!testSuccessful)
                {
                    Result = TestResult.SetupFailed;
                    return Result;
                }
            }

            // Run Test cases
            Result = TestResult.Passed;
            foreach (var testCase in TestCases)
            {
                TestResult testResult;
                try
                {
                    testResult = await testCase.RunAsync(variables, unitTestFolder, cancellationToken, progress);
                }
                catch (Exception)
                {
                    testResult = TestResult.Failed; 
                }

                if (testResult == TestResult.Passed)
                    Summary.Passed++;
                else if (testResult == TestResult.Failed)
                    Summary.Failed++;
                else if (testResult == TestResult.SetupFailed)
                    Summary.SetupFailed++;

                if (testResult != TestResult.NotRun)
                    Summary.NotRun--;                                
            }

            // Run teardown tasks
            foreach (ITask task in TearDownTasks)
            {
                try
                {
                    testSuccessful = await task.RunAsync(variables, fileNameGenerator, cancellationToken, progress);
                }
                catch (Exception)
                {
                    testSuccessful = false;
                }                    
            }

            if (Summary.Passed == Summary.Total)
                Result = TestResult.Passed;
            else
                Result = TestResult.Failed;

            if (progress != null)
                progress.Report(new TestProgress(Id, Result));

            EndTime = DateTime.Now;
            return Result;
            
        }

        public TestCase GetTestCase(Guid id)
        {
            return TestCases.FirstOrDefault(x => x.Id == id);
        }
    }
}

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
    

    public class TestSuite
    {        
        public ITestItem Test { get; set; }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TestResult Result { get; private set; }
        public TestSummary Summary { get; private set; }

        public TestSuite()
        {
            StartTime = new DateTime(0001, 01, 01);
            EndTime = new DateTime(0001, 01, 01);
            Result = TestResult.NotRun;
            Summary = new TestSummary();
        }

/*
        public async Task<TestResult> RunTestAsync(TestCase testCase, Dictionary<string, string> variables, string outputFolder, CancellationToken cancellationToken, IProgress<TestProgress> progress)
        {
            if (progress != null)
            {
                progress.Report(new TestRunProgress()
                {
                    TestName = unitTest.Name,
                    TestResult = TestResult.InProgress
                });
            }

            // Create the output folder 
            var testRunOutputFolder = Path.Combine(outputFolder, unitTest.Name);
            Directory.CreateDirectory(testRunOutputFolder);

            var testVariables = new Dictionary<string, string>(variables);
            var testResult = await unitTest.RunAsync(testVariables, new TestOutputFileNameGenerator(testRunOutputFolder), cancellationToken, null);

            if (progress != null)
            {
                progress.Report(new TestRunProgress()
                {
                    TestName = unitTest.Name,
                    TestResult = testResult
                });
            } 
            
            return testResult; 
        } */

        public async Task<TestResult> RunAllAsync(Dictionary<string, string> variables, string outputFolder, CancellationToken cancellationToken, IProgress<TestProgress> progress)
        {
            StartTime = DateTime.Now;
            EndTime = new DateTime(0001, 01, 01);
            Result = TestResult.InProgress;

            Summary.Total = Test.TestCount;
            Summary.NotRun = Summary.Total;
            Summary.SetupFailed = 0;
            Summary.Passed = 0;
            Summary.Failed = 0;

            Result = await Test.RunAsync(variables, outputFolder, cancellationToken, progress);
        
            if (Summary.Passed == Summary.Total)
                Result = TestResult.Passed;
            else
                Result = TestResult.Failed;

            Summary.Total = Test.Summary.Total;
            Summary.NotRun = Test.Summary.NotRun;
            Summary.SetupFailed = Test.Summary.SetupFailed;
            Summary.Passed = Test.Summary.Passed;
            Summary.Failed = Test.Summary.Failed;

            EndTime = DateTime.Now;

            return Result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace TestHarness
{

    public class TestGroup : ITestItem
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; set; }
        public string Description { get; set; }

        public List<ITestItem> Items { get; private set; }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TestResult Result { get; private set; }
        public TestSummary Summary { get; private set; }

        public int TestCount
        {
            get
            {
                return Items.Sum(x => x.TestCount);
            }
        }

        public TestGroup()
        {
            Items = new List<ITestItem>();

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

            foreach (var testItem in Items)
            {
                var testResult = await testItem.RunAsync(variables, outputFolder, cancellationToken, progress);

                Summary.Passed += testItem.Summary.Passed;
                Summary.Failed += testItem.Summary.Failed;
                Summary.SetupFailed += testItem.Summary.SetupFailed;

                Summary.NotRun -= (testItem.Summary.Total - testItem.Summary.NotRun);               
            }

            EndTime = DateTime.Now;

            if (Summary.Passed == Summary.Total)
                Result = TestResult.Passed;
            else
                Result = TestResult.Failed;

            if (progress != null)
                progress.Report(new TestProgress(Id, Result));

            return Result;
        } 

        public TestCase GetTestCase(Guid id)
        {
            foreach (var testItem in Items)
            {
                var testCase = testItem.GetTestCase(id);
                if (testCase != null)
                    return testCase;
            }

            return null;
        }
    }
}

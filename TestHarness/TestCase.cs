using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;


namespace TestHarness
{
  
    public class TestCase
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; set; }
        public string Description { get; set; }
        public TestType TestType { get; set; }

        public List<ITask> SetupTasks { get; private set; }
        public List<ITask> TestTasks { get; private set; }
        public List<ITask> TearDownTasks { get; private set; }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TestResult Result { get; private set; }

        public TestCase()
        {
            TestType = TestType.Positive;

            StartTime = new DateTime(0001, 01, 01);
            EndTime = new DateTime(0001, 01, 01);
            Result = TestResult.NotRun;

            SetupTasks = new List<ITask>();
            TestTasks = new List<ITask>();
            TearDownTasks = new List<ITask>();
        }

 
        public async Task<TestResult> RunAsync(Dictionary<string, string> variables, string outputFolder, CancellationToken cancellationToken, IProgress<TestProgress> progress)
        {
            StartTime = DateTime.Now;
            EndTime = new DateTime(0001, 01, 01);
            Result = TestResult.InProgress;

            if (progress != null)
                progress.Report(new TestProgress(Id, Result));

            bool testSuccessful;
            outputFolder = Path.Combine(outputFolder, Id.ToString());
            Directory.CreateDirectory(outputFolder);

            var fileNameGenerator = new TestOutputFileNameGenerator(outputFolder);

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
                    EndTime = DateTime.Now;

                    return Result;
                }
            } 

            // Run Test tasks
            Result = TestResult.Passed;
            foreach (ITask task in TestTasks)
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
                    Result = TestResult.Failed;
                    break;
                }
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

            if (progress != null)
                progress.Report(new TestProgress(Id, Result));

            EndTime = DateTime.Now;
            return Result; 
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using TestHarness;

namespace ConsoleRunner
{
    class ConsoleSuiteRunner
    {
        private TestSuite _TestSuite;
        private int _TestsCompleted;
        private int _TestsTotal;

        public void RunTest(string fileName, Dictionary<string, string> variables, string outputFolder)
        {
            _TestSuite = TestHarnessReader.LoadTestSuite(fileName);

            Console.Clear();
            Console.WriteLine("Test started.");

            _TestsCompleted = 0;
            _TestsTotal = _TestSuite.Test.TestCount;

            var progress = new Progress<TestProgress>(OnTestRunProgress);

            var task = _TestSuite.RunAllAsync(variables, outputFolder, CancellationToken.None, progress);
            task.Wait();

            TestHarnessWriter.SaveTestSuite(_TestSuite, variables, Path.Combine(outputFolder, "result.xml"));

            Console.WriteLine("");
            Console.WriteLine("Test complete. Result saved to outputFolder");
        }


        private void OnTestRunProgress(TestProgress progress)
        {
            if (progress.Result != TestResult.InProgress)
            {
                var testCase = _TestSuite.Test.GetTestCase(progress.Id);
                if (testCase != null)
                {
                    _TestsCompleted++;

                    Console.SetCursorPosition(2, Console.CursorTop);

                    var completed = _TestsCompleted.ToString().PadLeft(5);
                    Console.Write("Progess:     {0} of {1}", completed, _TestsTotal);
                }
            }
        }

    }
}

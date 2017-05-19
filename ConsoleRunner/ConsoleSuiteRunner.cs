using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Xml;

using TestHarness;

namespace ConsoleRunner
{
    class ConsoleSuiteRunner
    {

        public void RunTest(string fileName, Dictionary<string, string> variables, string outputFolder)
        {
            var reader = new TestHarnessReader();
            var testSuite = reader.LoadTestSuite(fileName);
            var progress = new Progress<TestProgress>(OnTestRunProgress);

            var task = testSuite.RunAllAsync(variables, outputFolder, CancellationToken.None, progress);
            task.Wait();

            var writer = new TestHarnessWriter();
            writer.SaveTestSuite(testSuite, variables, Path.Combine(outputFolder, "result.xml"));
        }


        private void OnTestRunProgress(TestProgress progress)
        {

            if (progress.Result == TestResult.Passed)
            {
                Console.WriteLine("Test {0} Passed.", progress.Id);
            }
            else if (progress.Result == TestResult.Failed)
            {
                Console.WriteLine("Test {0} Failed.", progress.Id);
            }
            else if (progress.Result == TestResult.SetupFailed)
            {
                Console.WriteLine("Test {0} Setup Failed.", progress.Id);
            }
            else if (progress.Result == TestResult.NotRun)
            {
                Console.WriteLine("Test {0} Not run.", progress.Id);
            }
        }

    }
}

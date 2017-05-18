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
            var xmlResult = new XmlDocument();

            var testRunNode = xmlResult.CreateElement("testrun");
            xmlResult.AppendChild(testRunNode);

            var summaryNode = xmlResult.CreateElement("summary");
            testRunNode.AppendChild(summaryNode);

            OutputValue(summaryNode, "server", variables["SERVER"]);
            OutputValue(summaryNode, "filelibrary", variables["FILELIBRARY"]);
            OutputValue(summaryNode, "user", variables["USER"]);
            OutputValue(summaryNode, "start", DateTime.Now.ToString());

            var resultsNode = xmlResult.CreateElement("results");
            testRunNode.AppendChild(resultsNode);

            var testSuite = new TestSuite(fileName);
            Progress<TestRunProgress> progress = new Progress<TestRunProgress>(OnTestRunProgress);

            var task = testSuite.RunAllAsync(variables, outputFolder, CancellationToken.None, progress);
            task.Wait();

            var testSuiteNode = xmlResult.CreateElement("unittests");
            testSuiteNode.SetAttribute("test", fileName);
            resultsNode.AppendChild(testSuiteNode);

            int testsPassed = 0;
            int testsFailed = 0;
            int testsNotRun = 0;
            int testsTotal = 0;
            foreach (var unitTest in testSuite.UnitTests)
            {
                testsTotal++;

                if (unitTest.Result == TestResult.Passed)
                    testsPassed++;
                else if ((unitTest.Result == TestResult.Failed) || (unitTest.Result == TestResult.SetupFailed))
                    testsFailed++;
                else if (unitTest.Result == TestResult.NotRun)
                    testsFailed++;

                OutputUnitTest(testSuiteNode, unitTest);
            }

            OutputValue(summaryNode, "end", DateTime.Now.ToString());
            OutputValue(summaryNode, "passed", testsPassed.ToString());
            OutputValue(summaryNode, "failed", testsFailed.ToString());
            OutputValue(summaryNode, "notrun", testsNotRun.ToString());
            OutputValue(summaryNode, "total", testsTotal.ToString());

            var resultFile = Path.Combine(outputFolder, "result.xml");
            xmlResult.Save(resultFile);
        }


        private void OnTestRunProgress(TestRunProgress progress)
        {
            if (progress.TestResult == TestResult.InProgress)
            {
                Console.Write("Running test {0} ... ", progress.TestName);
            }
            else if (progress.TestResult == TestResult.Passed)
            {
                Console.WriteLine("Passed.", progress.TestName);
            }
            else if ((progress.TestResult == TestResult.Failed) || (progress.TestResult == TestResult.SetupFailed))
            {
                Console.WriteLine("Failed.", progress.TestName);
            }
            else if (progress.TestResult == TestResult.NotRun)
            {
                Console.WriteLine("Not run.", progress.TestName);
            }
        }

        private void OutputUnitTest(XmlElement xml, UnitTest unitTest)
        {
            var unitTestNode = xml.OwnerDocument.CreateElement("unittest");
            xml.AppendChild(unitTestNode);

            OutputValue(unitTestNode, "name", unitTest.Name);
            OutputValue(unitTestNode, "description", unitTest.Description);
            OutputValue(unitTestNode, "start", unitTest.StartTime.ToString());
            OutputValue(unitTestNode, "end", unitTest.StartTime.ToString());
            OutputValue(unitTestNode, "result", unitTest.Result.ToString());
        }

        private void OutputValue(XmlElement xml, string name, string value)
        {
            var node = xml.OwnerDocument.CreateElement(name);
            node.InnerText = value;
            xml.AppendChild(node);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace TestHarness.IO
{
    public class JUnitTestResultWriter : ITestSuiteResultWriter
    {
        public void WriteResults(TestSuite testSuite, Dictionary<string, string> variables, string fileName)
        {
            var xmlDocument = new XmlDocument();

            var testRunNode = xmlDocument.CreateElement("testsuites");
            xmlDocument.AppendChild(testRunNode);

            WriteTestItem(testRunNode, testSuite.Test, variables); 

            xmlDocument.Save(fileName);
        }

        private void WriteTestItem(XmlElement xml, ITestItem testItem, Dictionary<string, string> variables)
        {
            if (testItem is TestGroup)
                WriteTestGroup(xml, testItem as TestGroup, variables);
            else if (testItem is UnitTest)
                WriteUnitTest(xml, testItem as UnitTest, variables);
            else
                throw new NotSupportedException();
        }

        private void WriteTestGroup(XmlElement xml, TestGroup testGroup, Dictionary<string, string> variables)
        {
            foreach (var testItem in testGroup.Items)
            {
                WriteTestItem(xml, testItem, variables);
            }
        }

        private void WriteUnitTest(XmlElement xml, UnitTest unitTest, Dictionary<string, string> variables)
        {
            var unitTestNode = xml.OwnerDocument.CreateElement("testsuite");
            xml.AppendChild(unitTestNode);

            unitTestNode.SetAttribute("id", unitTest.Id.ToString());
            unitTestNode.SetAttribute("name", unitTest.Name);

            unitTestNode.SetAttribute("tests", unitTest.TestCases.Count.ToString());
            unitTestNode.SetAttribute("failures", (unitTest.Summary.Failed + unitTest.Summary.SetupFailed).ToString());
            unitTestNode.SetAttribute("skipped", unitTest.Summary.NotRun.ToString());
            unitTestNode.SetAttribute("time", unitTest.EndTime.Subtract(unitTest.StartTime).TotalSeconds.ToString("F0"));
            unitTestNode.SetAttribute("timestamp", unitTest.StartTime.ToString("yyyy-MM-ddThh:mm:ss"));

            AddProperty(unitTestNode, "description", unitTest.Description);
            AddProperty(unitTestNode, "server", variables["SERVER"]);
            AddProperty(unitTestNode, "filelibrary", variables["FILELIBRARY"]);
            AddProperty(unitTestNode, "user", variables["USER"]);

            foreach (var testCase in unitTest.TestCases)
            {
                WriteTestCase(unitTestNode, testCase);
            }
        }

        private void WriteTestCase(XmlElement xml, TestCase testCase)
        {
            var testCaseNode = xml.OwnerDocument.CreateElement("testcase");
            xml.AppendChild(testCaseNode);

            testCaseNode.SetAttribute("name", testCase.Name);
            testCaseNode.SetAttribute("classname", "");
            testCaseNode.SetAttribute("time", testCase.EndTime.Subtract(testCase.StartTime).TotalSeconds.ToString("F0"));

            if ((testCase.Result == TestResult.Failed) || (testCase.Result == TestResult.SetupFailed))
            {
                var failureNode = xml.OwnerDocument.CreateElement("failure");
                testCaseNode.AppendChild(failureNode);

                foreach (var task in testCase.TestTasks)
                {
                    if (task.Result == TaskResult.Failed)
                        failureNode.SetAttribute("message", task.Message);
                }
            }
        }

        private void AddProperty(XmlElement xml, string name, string value)
        {
            var propertiesNode = xml.SelectSingleNode("properties");
            if (propertiesNode == null)
            {
                propertiesNode = xml.OwnerDocument.CreateElement("properties");
                xml.AppendChild(propertiesNode);
            }
            
            var propertyNode = xml.OwnerDocument.CreateElement("property");
            propertiesNode.AppendChild(propertyNode);
            propertyNode.SetAttribute("name", name);
            propertyNode.SetAttribute("value", value);
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace TestHarness
{
    public static class TestHarnessWriter
    {
        public static void SaveTestSuite(TestSuite testSuite, Dictionary<string, string> variables, string fileName)
        {
            var xmlDocument = SaveTestSuite(testSuite, variables);
            xmlDocument.Save(fileName);
        }

        public static XmlDocument SaveTestSuite(TestSuite testSuite, Dictionary<string, string> variables)
        {
            var xmlDocument = new XmlDocument();

            var testRunNode = xmlDocument.CreateElement("testrun");
            xmlDocument.AppendChild(testRunNode);

            var summaryNode = xmlDocument.CreateElement("summary");
            testRunNode.AppendChild(summaryNode);

            OutputValue(summaryNode, "server", variables["SERVER"]);
            OutputValue(summaryNode, "filelibrary", variables["FILELIBRARY"]);
            OutputValue(summaryNode, "user", variables["USER"]);

            OutputValue(summaryNode, "start", testSuite.StartTime.ToString());
            OutputValue(summaryNode, "end", testSuite.EndTime.ToString());
            OutputValue(summaryNode, "passed", testSuite.Summary.Passed.ToString());
            OutputValue(summaryNode, "failed", testSuite.Summary.Failed.ToString());
            OutputValue(summaryNode, "notrun", testSuite.Summary.NotRun.ToString());
            OutputValue(summaryNode, "total", testSuite.Summary.Total.ToString());

            var resultsNode = xmlDocument.CreateElement("results");
            testRunNode.AppendChild(resultsNode);

            SaveTestItem(resultsNode, testSuite.Test);

            return xmlDocument;
        }

        private static void SaveTestItem(XmlElement xml, ITestItem testItem)
        {
            if (testItem is TestGroup)
                SaveTestGroup(xml, testItem as TestGroup);
            else if (testItem is UnitTest)
                SaveUnitTest(xml, testItem as UnitTest);
            else
                throw new NotSupportedException();
        }

        private static void SaveTestGroup(XmlElement xml, TestGroup testGroup)
        {            
            var testGroupNode = xml.OwnerDocument.CreateElement("testgroup");
            xml.AppendChild(testGroupNode);

            OutputValue(testGroupNode, "id", testGroup.Id.ToString());
            OutputValue(testGroupNode, "name", testGroup.Name);
            OutputValue(testGroupNode, "description", testGroup.Description);

            OutputValue(testGroupNode, "start", testGroup.StartTime.ToString());
            OutputValue(testGroupNode, "end", testGroup.EndTime.ToString());
            OutputValue(testGroupNode, "passed", testGroup.Summary.Passed.ToString());
            OutputValue(testGroupNode, "failed", testGroup.Summary.Failed.ToString());
            OutputValue(testGroupNode, "notrun", testGroup.Summary.NotRun.ToString());
            OutputValue(testGroupNode, "total", testGroup.Summary.Total.ToString());

            var resultsNode = xml.OwnerDocument.CreateElement("results");
            testGroupNode.AppendChild(resultsNode);

            foreach (var testItem in testGroup.Items)
            {
                SaveTestItem(resultsNode, testItem);
            }
        }

        private static void SaveUnitTest(XmlElement xml, UnitTest unitTest)
        {
            var unitTestNode = xml.OwnerDocument.CreateElement("unittest");
            xml.AppendChild(unitTestNode);

            OutputValue(unitTestNode, "id", unitTest.Id.ToString());
            OutputValue(unitTestNode, "name", unitTest.Name);
            OutputValue(unitTestNode, "description", unitTest.Description);

            OutputValue(unitTestNode, "start", unitTest.StartTime.ToString());
            OutputValue(unitTestNode, "end", unitTest.EndTime.ToString());
            OutputValue(unitTestNode, "passed", unitTest.Summary.Passed.ToString());
            OutputValue(unitTestNode, "failed", unitTest.Summary.Failed.ToString());
            OutputValue(unitTestNode, "notrun", unitTest.Summary.NotRun.ToString());
            OutputValue(unitTestNode, "total", unitTest.Summary.Total.ToString());

            var resultsNode = xml.OwnerDocument.CreateElement("results");
            unitTestNode.AppendChild(resultsNode);

            foreach (var testCase in unitTest.TestCases)
            {
                SaveTestCase(resultsNode, testCase);
            }
        }

        private static void SaveTestCase(XmlElement xml, TestCase testCase)
        {
            var testCaseNode = xml.OwnerDocument.CreateElement("testcase");
            xml.AppendChild(testCaseNode);

            OutputValue(testCaseNode, "id", testCase.Id.ToString());
            OutputValue(testCaseNode, "name", testCase.Name);
            OutputValue(testCaseNode, "description", testCase.Description);

            OutputValue(testCaseNode, "start", testCase.StartTime.ToString());
            OutputValue(testCaseNode, "end", testCase.StartTime.ToString());
            OutputValue(testCaseNode, "result", testCase.Result.ToString());
        } 

        private static void OutputValue(XmlElement xml, string name, string value)
        {
            var node = xml.OwnerDocument.CreateElement(name);
            node.InnerText = value;
            xml.AppendChild(node);
        }
    }
}

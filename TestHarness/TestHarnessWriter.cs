using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace TestHarness
{
    public class TestHarnessWriter
    {
        private XmlDocument XmlDocument;

        public void SaveTestSuite(TestSuite testSuite, Dictionary<string, string> variables, string fileName)
        {
            SaveTestSuite(testSuite, variables);
            XmlDocument.Save(fileName);
        }

        public XmlDocument SaveTestSuite(TestSuite testSuite, Dictionary<string, string> variables)
        {
            XmlDocument = new XmlDocument();

            var testRunNode = XmlDocument.CreateElement("testrun");
            XmlDocument.AppendChild(testRunNode);

            var summaryNode = XmlDocument.CreateElement("summary");
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

            var resultsNode = XmlDocument.CreateElement("results");
            testRunNode.AppendChild(resultsNode);

            resultsNode.AppendChild(SaveTestItem(testSuite.Test));

            return XmlDocument;
        }

        private XmlElement SaveTestItem(ITestItem testItem)
        {
            if (testItem is TestGroup)
                return SaveTestGroup(testItem as TestGroup);
            else if (testItem is UnitTest)
                return SaveUnitTest(testItem as UnitTest);
            else
                throw new NotSupportedException();
        }

        private XmlElement SaveTestGroup(TestGroup testGroup)
        {            
            var testGroupNode = XmlDocument.CreateElement("testgroup");

            OutputValue(testGroupNode, "id", testGroup.Id.ToString());
            OutputValue(testGroupNode, "name", testGroup.Name);
            OutputValue(testGroupNode, "description", testGroup.Description);

            OutputValue(testGroupNode, "start", testGroup.StartTime.ToString());
            OutputValue(testGroupNode, "end", testGroup.EndTime.ToString());
            OutputValue(testGroupNode, "passed", testGroup.Summary.Passed.ToString());
            OutputValue(testGroupNode, "failed", testGroup.Summary.Failed.ToString());
            OutputValue(testGroupNode, "notrun", testGroup.Summary.NotRun.ToString());
            OutputValue(testGroupNode, "total", testGroup.Summary.Total.ToString());

            var resultsNode = XmlDocument.CreateElement("results");
            testGroupNode.AppendChild(resultsNode);

            foreach (var testItem in testGroup.Items)
            {
                resultsNode.AppendChild(SaveTestItem(testItem));
            }

            return testGroupNode;
        }

        private XmlElement SaveUnitTest(UnitTest unitTest)
        {
            var unitTestNode = XmlDocument.CreateElement("unittest");

            OutputValue(unitTestNode, "id", unitTest.Id.ToString());
            OutputValue(unitTestNode, "name", unitTest.Name);
            OutputValue(unitTestNode, "description", unitTest.Description);

            OutputValue(unitTestNode, "start", unitTest.StartTime.ToString());
            OutputValue(unitTestNode, "end", unitTest.EndTime.ToString());
            OutputValue(unitTestNode, "passed", unitTest.Summary.Passed.ToString());
            OutputValue(unitTestNode, "failed", unitTest.Summary.Failed.ToString());
            OutputValue(unitTestNode, "notrun", unitTest.Summary.NotRun.ToString());
            OutputValue(unitTestNode, "total", unitTest.Summary.Total.ToString());

            var resultsNode = XmlDocument.CreateElement("results");
            unitTestNode.AppendChild(resultsNode);

            foreach (var testCase in unitTest.TestCases)
            {
                resultsNode.AppendChild(SaveTestCase(testCase));
            }

            return unitTestNode;
        }

        private XmlElement SaveTestCase(TestCase testCase)
        {
            var testCaseNode = XmlDocument.CreateElement("testcase");

            OutputValue(testCaseNode, "id", testCase.Id.ToString());
            OutputValue(testCaseNode, "name", testCase.Name);
            OutputValue(testCaseNode, "description", testCase.Description);

            OutputValue(testCaseNode, "start", testCase.StartTime.ToString());
            OutputValue(testCaseNode, "end", testCase.StartTime.ToString());
            OutputValue(testCaseNode, "result", testCase.Result.ToString());

            return testCaseNode;
        } 

        private void OutputValue(XmlElement xml, string name, string value)
        {
            var node = XmlDocument.CreateElement(name);
            node.InnerText = value;
            xml.AppendChild(node);
        }
    }
}

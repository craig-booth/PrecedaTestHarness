using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace TestHarness.IO
{
    public class XmlTestSuiteLoader : ITestSuiteLoader
    {
        public TestSuite Load(string fileName)
        {
            var testSuite = new TestSuite();

            testSuite.Test = LoadTestItem(fileName);

            return testSuite;
        }

        public void Save(TestSuite testSuite, string fileName)
        {
            throw new NotImplementedException();
        }

        private ITestItem LoadTestItem(string fileName)
        {
            var directory = Path.GetDirectoryName(fileName);

            var xml = new XmlDocument();
            xml.Load(fileName);

            return LoadTestItem(xml.DocumentElement, directory);
        }

        private ITestItem LoadTestItem(XmlNode xml, string directory)
        {
            if (xml.Name == "testgroup")
            {
                return LoadTestGroup(xml, directory);
            }
            else if (xml.Name == "unittest")
            {
                return LoadUnitTest(xml, directory);
            }
            else
            {
                throw new NotSupportedException();
            }
        } 

        private TestGroup LoadTestGroup(XmlNode xml, string directory)
        {
            var testGroup = new TestGroup();

            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node.Name == "name")
                    testGroup.Name = node.InnerText;
                else if (node.Name == "description")
                    testGroup.Description = node.InnerText;
                else if ((node.Name == "testgroup") || (node.Name == "unittest"))
                {
                    if (node.Attributes["path"] != null)
                        testGroup.Items.Add(LoadTestItem(Path.Combine(directory, node.Attributes["path"].InnerText)));
                    else
                        testGroup.Items.Add(LoadTestItem(node, directory));
                }

            }

            return testGroup;
        } 

        private UnitTest LoadUnitTest(XmlNode xml, string directory)
        {
            var unitTest = new UnitTest();

            XmlNode node;

            node = xml.SelectSingleNode("name");
            if (node != null)
                unitTest.Name = node.InnerText;

            node = xml.SelectSingleNode("description");
            if (node != null)
                unitTest.Description = node.InnerText;


            node = xml.SelectSingleNode("setup");
            if (node != null)
            {
                foreach (XmlNode taskNode in node.ChildNodes)
                {
                    unitTest.SetupTasks.Add(LoadTask(taskNode, directory));
                }
            }

            node = xml.SelectSingleNode("testcases");
            if (node != null)
            {
                foreach (XmlNode taskNode in node.ChildNodes)
                {
                    unitTest.TestCases.Add(LoadTestCase(taskNode, directory));
                }
            }

            node = xml.SelectSingleNode("teardown");
            if (node != null)
            {
                foreach (XmlNode taskNode in node.ChildNodes)
                {
                    unitTest.TearDownTasks.Add(LoadTask(taskNode, directory));
                }
            }

            return unitTest;
        } 

        private TestCase LoadTestCase(XmlNode xml, string directory)
        {
            var testCase = new TestCase();

            XmlNode node;

            node = xml.SelectSingleNode("name");
            if (node != null)
                testCase.Name = node.InnerText;

            node = xml.SelectSingleNode("description");
            if (node != null)
                testCase.Description = node.InnerText;

            node = xml.SelectSingleNode("testtype");
            if (node != null)
            {
                if (node.InnerText == "negative")
                    testCase.TestType = TestType.Negative;
            }

            node = xml.SelectSingleNode("setup");
            if (node != null)
            {
                foreach (XmlNode taskNode in node.ChildNodes)
                {
                    testCase.SetupTasks.Add(LoadTask(taskNode, directory));
                }
            }

            node = xml.SelectSingleNode("test");
            if (node != null)
            {
                foreach (XmlNode taskNode in node.ChildNodes)
                {
                    var task = LoadTask(taskNode, directory);
                    if (task != null)
                        testCase.TestTasks.Add(task);
                }
            }

            node = xml.SelectSingleNode("teardown");
            if (node != null)
            {
                foreach (XmlNode taskNode in node.ChildNodes)
                {
                    testCase.TearDownTasks.Add(LoadTask(taskNode, directory));
                }
            }

            return testCase;
        } 

        private ITask LoadTask(XmlNode xml, string directory)
        {
            if (xml.Name == "mapper")
                return LoadMapperTask(xml, directory);
            else if (xml.Name == "sql")
                return LoadSQLTask(xml, directory);
            else if (xml.Name == "uploadbod")
                return LoadPayrollExchangeUploadBodTask(xml, directory);
            else if (xml.Name == "xmltransform")
                return LoadXmlTransformTask(xml, directory);
            else
                return null;
        } 

        private MapperTask LoadMapperTask(XmlNode xml, string directory)
        {
            var mapperTask = new MapperTask();

            mapperTask.ImportName = xml.SelectSingleNode("import").InnerText;
            mapperTask.FileName = Path.Combine(directory, xml.SelectSingleNode("file").InnerText);

            mapperTask.ActualResult = null;

            var recordsAdded = 0;
            var recordsUpdated = 0;
            var recordsDeleted = 0;
            var recordsFailed = 0;
            var recordsTotal = 0;
            var errorFile = "";

            var expectedResultNode = xml.SelectSingleNode("expectedresult");
            if (expectedResultNode != null)
            {
                recordsAdded = int.Parse(expectedResultNode.SelectSingleNode("added").InnerText);
                recordsUpdated = int.Parse(expectedResultNode.SelectSingleNode("updated").InnerText);
                recordsDeleted = int.Parse(expectedResultNode.SelectSingleNode("deleted").InnerText);
                recordsFailed = int.Parse(expectedResultNode.SelectSingleNode("failed").InnerText);
                recordsTotal = int.Parse(expectedResultNode.SelectSingleNode("total").InnerText);
            }
            var expectedErrors = expectedResultNode.SelectSingleNode("errors");
            if (expectedErrors != null)
                errorFile = Path.Combine(directory, expectedErrors.Attributes["file"].InnerText);

            mapperTask.ExpectedResult = new MapperTaskResult(recordsAdded, recordsUpdated, recordsDeleted, recordsFailed, recordsTotal, errorFile);

            return mapperTask;
        } 

        private SQLTask LoadSQLTask(XmlNode xml, string directory)
        {
            var sqlTask = new SQLTask();

            sqlTask.SQLStatement = xml.SelectSingleNode("statement").InnerText;

            var expectedResultNode = xml.SelectSingleNode("expectedresult");
            if (expectedResultNode != null)
            {
                var dataNode = expectedResultNode.SelectSingleNode("data");
                if (dataNode != null)
                {
                    sqlTask.RunMode = SQLRunMode.Query;
                    sqlTask.ExpectedResult.DataFileName = Path.Combine(directory, dataNode.Attributes["file"].Value);
                }
            }

            return sqlTask;
        } 
        
        private PayrollExchangeUploadBodTask LoadPayrollExchangeUploadBodTask(XmlNode xml, string directory)
        {
            throw new NotSupportedException();
        } 

        private XmlTransformTask LoadXmlTransformTask(XmlNode xml, string directory)
        {
            var transformTask = new XmlTransformTask();

            transformTask.TransformName = xml.SelectSingleNode("transform").InnerText;
            transformTask.FileName = Path.Combine(directory, xml.SelectSingleNode("file").InnerText);

            transformTask.ActualResult = null;

            var successfull = true;
            var errorMessage = "";
            var resultFile = "";

            var expectedResultNode = xml.SelectSingleNode("expectedresult");
            if (expectedResultNode != null)
            {
                var expectedError = expectedResultNode.SelectSingleNode("error");
                if (expectedError != null)
                {
                    successfull = false;
                    errorMessage = expectedError.InnerText;
                }
                else
                {
                    var result = expectedResultNode.SelectSingleNode("result");
                    if (result != null)
                    {
                        successfull = true;
                        resultFile = Path.Combine(directory, result.Attributes["file"].InnerText);
                    }
                }

            }

            transformTask.ExpectedResult = new XmlTransformTaskResult()
            {
                Successfull = successfull,
                Error = errorMessage,
                ResultFile = resultFile
            };

            return transformTask;
        }

    }
}

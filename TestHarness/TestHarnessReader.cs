﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace TestHarness
{
    public static class TestHarnessReader
    {

        public static TestSuite LoadTestSuite(string fileName)
        {
            var directory = Path.GetDirectoryName(fileName);

            var xml = new XmlDocument();
            xml.Load(fileName);

            return LoadTestSuite(xml.DocumentElement, directory);        
        }

        public static TestSuite LoadTestSuite(XmlNode xml, string directory)
        {
            var testSuite = new TestSuite();

            if (xml.Name == "testgroup")
            {
                 testSuite.Test = LoadTestGroup(xml, directory);
            }
            else if (xml.Name == "unittest")
            {
                testSuite.Test = LoadUnitTest(xml, directory);
            }

            return testSuite;
        }


        private static ITestItem LoadTestItem(string fileName)
        {
            var directory = Path.GetDirectoryName(fileName);

            var xml = new XmlDocument();
            xml.Load(fileName);

            return LoadTestItem(xml.DocumentElement, directory);
        }

        private static ITestItem LoadTestItem(XmlNode xml, string directory)
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

        private static TestGroup LoadTestGroup(XmlNode xml, string directory)
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

        private static UnitTest LoadUnitTest(XmlNode xml, string directory)
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

        private static TestCase LoadTestCase(XmlNode xml, string directory)
        {
            var testCase = new TestCase();

            XmlNode node;

            node = xml.SelectSingleNode("name");
            if (node != null)
                testCase.Name = node.InnerText;

            node = xml.SelectSingleNode("description");
            if (node != null)
                testCase.Description = node.InnerText;


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

        private static ITask LoadTask(XmlNode xml, string directory)
        {
            if (xml.Name == "mapper")
                return LoadMapperTask(xml, directory);
            else if (xml.Name == "sql")
                return LoadSQLTask(xml, directory);
            else if (xml.Name == "uploadbod")
                return LoadPayrollExchangeUploadBodTask(xml, directory);
            else
                return null;
        }

        private static MapperTask LoadMapperTask(XmlNode xml, string directory)
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

        private static SQLTask LoadSQLTask(XmlNode xml, string directory)
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

        private static PayrollExchangeUploadBodTask LoadPayrollExchangeUploadBodTask(XmlNode xml, string directory)
        {
            throw new NotSupportedException();
        }
        


    }
}
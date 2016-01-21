using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

using PayrollExchange;

namespace TestHarness
{
    public enum TestType { Positive, Negative };
    public enum TestResult { NotRun, InProgress, SetupFailed, Passed, Failed };

    public interface ITask
    {
        Task<bool> RunAsync(Dictionary<string, string> variables, string outputFolder, StringWriter output, CancellationToken cancellationToken);
    }

    public class UnitTestProgress
    {
        public TestResult Result { get; set; }
        public string Output { get; set; }
    }

    public class UnitTest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TestType TestType { get; set; }      
        
        public List<ITask> SetupTasks { get; private set; }
        public List<ITask> TestTasks { get; private set; }
        public List<ITask> TearDownTasks { get; private set; }

        public TestResult Result { get; private set; }
        public string Output { get; private set; }

        public UnitTest()
        {
            TestType = TestType.Positive;
            Result = TestResult.NotRun;
            Output = "";

            SetupTasks = new List<ITask>();
            TestTasks = new List<ITask>();
            TearDownTasks = new List<ITask>();
        }

        public UnitTest(XmlNode xml, string directory)
            : this()
        {
            XmlNode node;

            node = xml.SelectSingleNode("name");
            if (node != null)
                Name = node.InnerText;

            node = xml.SelectSingleNode("description");
            if (node != null)
                Description = node.InnerText;

            node = xml.SelectSingleNode("testtype");
            if (node != null)
            {
                if (node.InnerText == "negative")
                    TestType = TestType.Negative;
                else
                    TestType = TestType.Positive;
            }

            node = xml.SelectSingleNode("setup");
            if (node != null)
            {
                foreach (XmlNode taskNode in node.ChildNodes)
                {
                    SetupTasks.Add(LoadTask(taskNode, directory)); 
                }
            }

            node = xml.SelectSingleNode("test");
            if (node != null)
            {
                foreach (XmlNode taskNode in node.ChildNodes)
                {
                    TestTasks.Add(LoadTask(taskNode, directory));
                }
            }

            node = xml.SelectSingleNode("teardown");
            if (node != null)
            {
                foreach (XmlNode taskNode in node.ChildNodes)
                {
                    TearDownTasks.Add(LoadTask(taskNode, directory));
                }
            }
        }

        private ITask LoadTask(XmlNode xml, string directory)
        {
            if (xml.Name == "uploadbod")
                return new PayrollExchangeUploadBodTask(xml, directory);
            else if (xml.Name == "mapper")
                return new MapperTask(xml, directory);
            else if (xml.Name == "sql")
                return new SQLTask(xml, directory);
            else
                return null;
        }

        public async Task<TestResult> RunAsync(Dictionary<string, string> variables, string outputFolder, CancellationToken cancellationToken, IProgress<UnitTestProgress> progress)
        {
            var stringWriter = new StringWriter();
            bool testSuccessful;

            Result = TestResult.InProgress;
            Output = "";
            
            var unitTestProgress = new UnitTestProgress();
            if (progress != null)
            {                
                unitTestProgress.Result = Result;
                unitTestProgress.Output = Output;

                progress.Report(unitTestProgress);
            }

            // Run setup tasks
            foreach (ITask task in SetupTasks)
            {
                try
                {
                    testSuccessful = await task.RunAsync(variables, outputFolder, stringWriter, cancellationToken);
                }
                catch (Exception e)
                {
                    stringWriter.WriteLine("An exception occurred: " + e.Message);
                    testSuccessful = false;
                }
                if (!testSuccessful)
                {
                    Result = TestResult.SetupFailed;
                    stringWriter.WriteLine("Task Failed");
                    Output = stringWriter.ToString();

                    return Result;
                }

                if (progress != null)
                {
                    unitTestProgress.Result = TestResult.InProgress;
                    unitTestProgress.Output = stringWriter.ToString();

                    progress.Report(unitTestProgress);
                }
            }

            // Run Test tasks
            foreach (ITask task in TestTasks)
            {
                try
                {
                    testSuccessful = await task.RunAsync(variables, outputFolder, stringWriter, cancellationToken);
                }
                catch (Exception e)
                {
                    stringWriter.WriteLine("An exception occurred: " + e.Message);
                    testSuccessful = false;
                }
                if (testSuccessful)
                {
                    Result = TestResult.Passed;
                    stringWriter.WriteLine("Task Passed");
                    Output = stringWriter.ToString();
                }
                else
                {
                    Result = TestResult.Failed;
                    stringWriter.WriteLine("Task Failed");
                    Output = stringWriter.ToString();
                    break;
                }
                             
            }

            // Run teardown tasks
            foreach (ITask task in TearDownTasks)
            {
                try
                {
                    testSuccessful = await task.RunAsync(variables, outputFolder, stringWriter, cancellationToken);
                }
                catch (Exception e)
                {
                    stringWriter.WriteLine("An exception occurred: " + e.Message);
                    testSuccessful = false;
                }
                if (!testSuccessful)
                {
                    stringWriter.WriteLine("Task Failed");
                    Output = stringWriter.ToString();
                }

                if (progress != null)
                {
                    unitTestProgress.Result = TestResult.InProgress;
                    unitTestProgress.Output = stringWriter.ToString();

                    progress.Report(unitTestProgress);
                }
            }

            return Result;

        }
    }
}

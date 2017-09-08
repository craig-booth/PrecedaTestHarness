using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using XmlTransform;

namespace TestHarness
{
    public class XmlTransformTaskResult
    {
        public bool Successfull { get; set; }
        public string Error { get; set; }
        public string ResultFile { get; set; }

        public XmlTransformTaskResult()
        {
            Successfull = true;
            Error = "";
            ResultFile = "";
        }

        public XmlTransformTaskResult(XmlTransformResult result)
        {
            Successfull = result.Successfull;
            Error = result.Error;
            ResultFile = result.ResultFile;
        }

        public bool Equals(XmlTransformTaskResult value)
        {
            if (Successfull)
            {
                return value.Successfull && FileComparer.Compare(ResultFile, value.ResultFile);
            }
            else
            {
                return (!value.Successfull) && (Error == value.Error);
            }
        }
}


    public class XmlTransformTask : ITask
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Description
        {
            get
            {
                return "Transform Name " + TransformName;
            }
        }

        public string Message { get; private set; }
        public TaskResult Result { get; private set; }
        
        public string TransformName { get; set; }
        public string FileName { get; set; }
        public XmlTransformTaskResult ExpectedResult;
        public XmlTransformTaskResult ActualResult;

        public XmlTransformTask()
        {
            Message = "";
            Result = TaskResult.NotRun;
            ActualResult = null;
            ExpectedResult = new XmlTransformTaskResult();
        }

        public async Task<bool> RunAsync(Dictionary<string, string> variables, TestOutputFileNameGenerator fileNameGenerator, CancellationToken cancellationToken, IProgress<TestProgress> progress)
        {
            try
            {
                Result = TaskResult.InProgress;
                Message = "";

                if (progress != null)
                    progress.Report(new TestProgress(Id, TestResult.InProgress));
                
                var transformRequest = new XmlTransformRequest(variables["SERVER"], variables["USER"], variables["PASSWORD"], variables["FILELIBRARY"]);

                var resultFileName = fileNameGenerator.GetOutputFileName(TransformName + "_result", "xml");
                var uploadResult = await transformRequest.UploadFileAsync(TransformName, FileName, resultFileName, cancellationToken); 
                ActualResult = new XmlTransformTaskResult(uploadResult);
                
                if (ExpectedResult.Equals(ActualResult))
                    Result = TaskResult.Passed;
                else
                {
                    Message = "Result did not match expected result";
                    Result = TaskResult.Failed;
                }

                if (progress != null)
                {
                    if (Result == TaskResult.Passed)
                        progress.Report(new TestProgress(Id, TestResult.Passed));
                    else
                        progress.Report(new TestProgress(Id, TestResult.Failed));
                }


                return (Result == TaskResult.Passed); 
            }
            catch (Exception e)
            {
                Result = TaskResult.ExceptionOccurred;
                Message = "An exception occurred: " + e.Message;

                if (progress != null)
                    progress.Report(new TestProgress(Id, TestResult.Failed));

                return false; 
            }

        }

        public void ViewResult()
        {

        }
    }
}

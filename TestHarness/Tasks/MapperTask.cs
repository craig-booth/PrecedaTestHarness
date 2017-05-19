using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

using CsvHelper;

using Mapper;

namespace TestHarness
{

    public class MapperTaskResult
    {
        public int RecordsAdded { get; set; }
        public int RecordsUpdated { get; set; }
        public int RecordsDeleted { get; set; }
        public int RecordsFailed { get; set; }
        public int RecordsTotal { get; set; }        
        public string ErrorFile { get; set; }

        public MapperTaskResult(int added, int updated, int deleted, int failed, int total, string errorFile)
        {
            RecordsAdded = added;
            RecordsUpdated = updated;
            RecordsDeleted = deleted;
            RecordsFailed = failed;
            RecordsTotal = total;
            ErrorFile = errorFile;
        }

        public MapperTaskResult(MapperUploadResult result)
            : this(result.RecordsAdded, result.RecordsUpdated, result.RecordsDeleted, result.RecordsFailed, result.RecordsTotal, result.ErrorFile)
        {

        }

        public bool Equals(MapperTaskResult value)
        {
            if (SummaryEqual(value))
            {
                if ((ErrorFile == "") && (value.ErrorFile == ""))
                    return true;
                else if ((ErrorFile != "") && (value.ErrorFile != ""))
                    return FileComparer.Compare(ErrorFile, value.ErrorFile);
                else
                    return true;
            }
            else
                return false;
        }

        private bool SummaryEqual(MapperTaskResult value)
        {
            if ((RecordsAdded == value.RecordsAdded) &&
                (RecordsUpdated == value.RecordsUpdated) &&
                (RecordsDeleted == value.RecordsDeleted) &&
                (RecordsFailed == value.RecordsFailed) &&
                (RecordsTotal == value.RecordsTotal))
                return true;
            else
                return false;
        }
    }

    public class MapperTask : ITask
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Description
        {
            get
            {
                return "Mapper Definition " + ImportName;
            }
        }

        public string Message { get; private set; }
        public TaskResult Result { get; private set; }

        public string ImportName { get; set; }
        public string FileName { get; set; }
        public MapperTaskResult ExpectedResult;
        public MapperTaskResult ActualResult;

        public MapperTask()
        {
            Message = "";
            Result = TaskResult.NotRun;
            ActualResult = null;
            ExpectedResult = new MapperTaskResult(0, 0, 0, 0, 0, "");         
        }

        public async Task<bool> RunAsync(Dictionary<string, string> variables, TestOutputFileNameGenerator fileNameGenerator, CancellationToken cancellationToken, IProgress<TestProgress> progress)
        {
            try
            {
                Result = TaskResult.InProgress;
                Message = "";

                if (progress != null)
                    progress.Report(new TestProgress(Id, TestResult.InProgress));

                var mapperImport = new MapperImport(variables["SERVER"], variables["USER"], variables["PASSWORD"], variables["FILELIBRARY"]);

                var errorFileName = fileNameGenerator.GetOutputFileName(ImportName + "_errors", "csv");
                var uploadResult = await mapperImport.UploadFileAsync(ImportName, FileName, errorFileName,  cancellationToken);              
                ActualResult = new MapperTaskResult(uploadResult);

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

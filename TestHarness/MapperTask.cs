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
        public string Description
        {
            get
            {
                return "Mapper Definition " + ImportName;
            }
        }

        public string Message { get; private set; }
        public TaskResult Result { get; private set; }

        public string ImportName { get; private set; }
        public string FileName { get; private set; }
        public MapperTaskResult ExpectedResult;
        public MapperTaskResult ActualResult;

        public MapperTask(XmlNode xml, string directory)
        {
            Message = "";
            Result = TaskResult.NotRun;

            ImportName = xml.SelectSingleNode("import").InnerText;
            FileName = Path.Combine(directory, xml.SelectSingleNode("file").InnerText);

            ActualResult = null;

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

            ExpectedResult = new MapperTaskResult(recordsAdded, recordsUpdated, recordsDeleted, recordsFailed, recordsTotal, errorFile);            
        }

        public async Task<bool> RunAsync(Dictionary<string, string> variables, TestOutputFileNameGenerator fileNameGenerator, CancellationToken cancellationToken)
        {
            try
            {
                Result = TaskResult.InProgress;
                Message = "";

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

                return (Result == TaskResult.Passed);
            }
            catch (Exception e)
            {
                Result = TaskResult.ExceptionOccurred;
                Message = "An exception occurred: " + e.Message;
                return false;
            }

        }

        public void ViewResult()
        {

        }
    }

}

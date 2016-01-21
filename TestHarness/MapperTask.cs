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
        public MapperUploadResult Result { get; private set; }

        public MapperTaskResult(MapperUploadResult result)
        {
            Result = result;
        }

        public void WriteValue(StringWriter output)
        {
            output.WriteLine("Added {0}, Updated {1}, Deleted {2}, Failed {3}, Total {4}", Result.RecordsAdded, Result.RecordsUpdated, Result.RecordsDeleted, Result.RecordsFailed, Result.RecordsTotal, Result.RecordsTotal);

            if (Result.Errors.RecordCount > 0)
            {
                output.WriteLine("Mapper errors:");
                foreach (var record in Result.Errors)
                {
                    output.WriteLine("  Record {0}:", record.RecordNumber);

                    foreach (var message in record.Errors)
                        output.WriteLine("     {0}", message);
                }
            }
        }

        public bool Equals(MapperTaskResult value)
        {
            return SummaryEqual(Result, value.Result) && ErrorsEqual(Result.Errors, value.Result.Errors);
        }

        private bool SummaryEqual(MapperUploadResult value1, MapperUploadResult value2)
        {
            if ((value1.RecordsAdded == value2.RecordsAdded) &&
                (value1.RecordsUpdated == value2.RecordsUpdated) &&
                (value1.RecordsDeleted == value2.RecordsDeleted) &&
                (value1.RecordsFailed == value2.RecordsFailed) &&
                (value1.RecordsTotal == value2.RecordsTotal))
                return true;
            else
                return false;
        }

        private bool ErrorsEqual(MapperRecordErrors value1, MapperRecordErrors value2)
        {
            // Do a quick check the the number of records in error is the same in both
            if (value1.RecordCount != value2.RecordCount)
                return false;

            // Make sure each element in value1 exists in value2
            foreach (var recordError1 in value1)
            {
                if (!value2.ContainsRecord(recordError1.RecordNumber))
                    return false;
            }

            // Both contain the same records in error. So now lets check the actual errors
            foreach (var recordError in value1)
            {
                if (!RecordErrorsEqual(recordError, value2[recordError.RecordNumber]))
                    return false;
            }

            return true;
        }

        private bool RecordErrorsEqual(MapperRecordError value1, MapperRecordError value2)
        {
            // Check that both lists contains the same number of errors
            if (value1.Errors.Count != value2.Errors.Count)
                return false;

            //Check that the error messages are the same
            foreach (var error in value2.Errors)
            {
                if (!value2.Errors.Contains(error))
                    return false;
            }

            return true;
        }
    }

    public class MapperTask : ITask
    {
        public string ImportName { get; private set; }
        public string FileName { get; private set; }
        public MapperTaskResult ExpectedResult;
        public MapperTaskResult ActualResult;

        public MapperTask(XmlNode xml, string directory)
        {
            ImportName = xml.SelectSingleNode("import").InnerText;
            FileName = Path.Combine(directory, xml.SelectSingleNode("file").InnerText);

            ActualResult = null;

            var uploadResult = new MapperUploadResult();
            var expectedResultNode = xml.SelectSingleNode("expectedresult");
            if (expectedResultNode != null)
            {
                uploadResult.RecordsAdded = int.Parse(expectedResultNode.SelectSingleNode("added").InnerText);
                uploadResult.RecordsUpdated = int.Parse(expectedResultNode.SelectSingleNode("updated").InnerText);
                uploadResult.RecordsDeleted = int.Parse(expectedResultNode.SelectSingleNode("deleted").InnerText);
                uploadResult.RecordsFailed = int.Parse(expectedResultNode.SelectSingleNode("failed").InnerText);
                uploadResult.RecordsTotal = int.Parse(expectedResultNode.SelectSingleNode("total").InnerText);
            }
            var expectedErrors = expectedResultNode.SelectSingleNode("errors");
            if (expectedErrors != null)
            {
                var errorFile = Path.Combine(directory, expectedErrors.Attributes["file"].InnerText);
                LoadErrorsFromFile(errorFile, uploadResult.Errors);
            }

            ExpectedResult = new MapperTaskResult(uploadResult);            
        }

        public async Task<bool> RunAsync(Dictionary<string, string> variables, string outputFolder, StringWriter output, CancellationToken cancellationToken)
        {

            output.WriteLine("Importing {0} : \"{1}\"", new Object[] { ImportName, FileName });

            var mapperImport = new MapperImport(variables["SERVER"], variables["USER"], variables["PASSWORD"], variables["FILELIBRARY"]);

            var uploadResult = await mapperImport.UploadFileAsync(ImportName, FileName, cancellationToken);

            ActualResult = new MapperTaskResult(uploadResult);

            // Copy validation errors to output folder
            var outputFileName = Path.Combine(outputFolder, ImportName + "_result.csv");
            OutputErrorsToFile(outputFileName, ActualResult.Result.Errors);

            // Output results
            output.WriteLine("Import complete");
            output.WriteLine("");
            output.WriteLine("Expected Result:");
            ExpectedResult.WriteValue(output);
            output.WriteLine("");
            output.WriteLine("Actual Result:");
            ActualResult.WriteValue(output);
            output.WriteLine("");
            output.WriteLine("");

            if (ExpectedResult.Equals(ActualResult))
                return true;
            else
                return false;

        }

        private void OutputErrorsToFile(string fileName, MapperRecordErrors recordErrors)
        {            
            var csvFile = File.CreateText(fileName);

            var csvWriter = new CsvWriter(csvFile);
            csvWriter.WriteHeader<CsvMapperErrorRecord>();
            foreach (var recordError in recordErrors)
            {
                foreach (var message in recordError.Errors)
                {
                    var outputRecord = new CsvMapperErrorRecord()
                    {
                        RecordNumber = recordError.RecordNumber,
                        Message = message
                    };
                    csvWriter.WriteRecord(outputRecord);

                }
            }

            csvFile.Close();   
        }

        private void LoadErrorsFromFile(string fileName, MapperRecordErrors recordErrors)
        {
            var csvFile = new StreamReader(fileName);

            var csvReader = new CsvReader(csvFile);
            
            while (csvReader.Read())
            {
                var inputRecord = csvReader.GetRecord<CsvMapperErrorRecord>();

                recordErrors.AddError(inputRecord.RecordNumber, inputRecord.Message);
            }
                
        }
    }

    public class CsvMapperErrorRecord
    {
        public int RecordNumber { get; set; }
        public string Message { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Xml;
using System.Net.Http;
using System.Net.Http.Headers;

using CsvHelper;

namespace Mapper
{


    public class MapperUploadResult
    {
        public int RecordsAdded { get; set; }
        public int RecordsUpdated { get; set; }
        public int RecordsDeleted { get; set; }
        public int RecordsFailed { get; set; }
        public int RecordsTotal { get; set; }

        public string ErrorFile { get; set; }

        public MapperUploadResult()
        {
            RecordsAdded = 0;
            RecordsUpdated = 0;
            RecordsDeleted = 0;
            RecordsFailed = 0;
            RecordsTotal = 0;
            ErrorFile = "";
        }
    }

    public class MapperImport
    {

        public string Server { get; private set; }
        public string User { get; private set; }
        public string Password { get; private set; }
        public string FileLibrary { get; private set; }

        public MapperImport(string server, string user, string password, string fileLibrary)
        {
            Server = server;
            User = user;
            Password = password;
            FileLibrary = fileLibrary;
        }

        public async Task<MapperUploadResult> UploadFileAsync(string importName, string fileName, string errorFileName, CancellationToken cancellationToken)
        {

            var requestContent = new MultipartFormDataContent();
            requestContent.Add(new StringContent(User), "USER");
            requestContent.Add(new StringContent(Password), "PASSWORD");
            requestContent.Add(new StringContent(FileLibrary), "DATABASE");
            requestContent.Add(new StringContent("UNITTEST"), "SERVICE");
            requestContent.Add(new StringContent("MAPPER"), "TASKTYPE");
            requestContent.Add(new StringContent(importName), "IMPORT");

            var fileStream = File.OpenRead(fileName);
            requestContent.Add(new StreamContent(fileStream), "FILE", fileName);            

            var httpClient = new HttpClient();     
            var httpResponce = await httpClient.PostAsync("http://" + Server + "/cgi-bin/precedawebservice", requestContent, cancellationToken);

            fileStream.Close();

            /* Check the request was successful */
            if (! httpResponce.IsSuccessStatusCode)
            {
                string errorMessage = String.Format("{0:d} ({1})", httpResponce.StatusCode, httpResponce.ReasonPhrase);

                var htmlErrorResponce = await httpResponce.Content.ReadAsStringAsync();

                var messageStart = htmlErrorResponce.IndexOf("<P>");
                var messageEnd = htmlErrorResponce.IndexOf("</P>");
                if ((messageStart >= 0) && (messageEnd > messageStart))
                {
                    errorMessage += " - " + htmlErrorResponce.Substring(messageStart + 3, messageEnd - messageStart - 3);
                }

                throw new Exception(errorMessage);
            }

            /* Load the xml responce */
            var responce = await httpResponce.Content.ReadAsStringAsync();
            var xmlResult = new XmlDocument();
            xmlResult.LoadXml(responce);  


            var result = new MapperUploadResult();

            /* Handle the import summary */
            var summary = xmlResult.SelectSingleNode("import/summary");
            result.RecordsAdded = int.Parse(summary.SelectSingleNode("added").InnerText);
            result.RecordsUpdated = int.Parse(summary.SelectSingleNode("updated").InnerText);
            result.RecordsDeleted = int.Parse(summary.SelectSingleNode("deleted").InnerText);
            result.RecordsFailed = int.Parse(summary.SelectSingleNode("failed").InnerText);
            result.RecordsTotal = int.Parse(summary.SelectSingleNode("total").InnerText);

            /* Handle the import errors */
            var recordErrors = xmlResult.SelectNodes("import/errors/record");
            if (recordErrors.Count > 0)
            {
                result.ErrorFile = errorFileName;  
                var csvFile = File.CreateText(result.ErrorFile);

                var csvWriter = new CsvWriter(csvFile);
                csvWriter.WriteHeader<CsvMapperErrorRecord>();

                foreach (XmlNode recordError in recordErrors)
                {
                    var recordNumber = int.Parse(recordError.Attributes["number"].Value);

                    var messages = recordError.SelectNodes("message");
                    foreach (XmlNode message in messages)
                    {
                        var outputRecord = new CsvMapperErrorRecord()
                        {
                            RecordNumber = recordNumber,
                            Message = message.InnerText 
                        };
                        csvWriter.WriteRecord(outputRecord);
                    }
                }

                csvFile.Close();
            }

            return result;
        }
    }

    public class CsvMapperErrorRecord
    {
        public int RecordNumber { get; set; }
        public string Message { get; set; }
    }


}

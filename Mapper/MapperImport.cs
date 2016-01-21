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


namespace Mapper
{


    public class MapperUploadResult
    {
        public int RecordsAdded { get; set; }
        public int RecordsUpdated { get; set; }
        public int RecordsDeleted { get; set; }
        public int RecordsFailed { get; set; }
        public int RecordsTotal { get; set; }

        public MapperRecordErrors Errors { get; private set; }


        public MapperUploadResult()
        {
            RecordsAdded = 0;
            RecordsUpdated = 0;
            RecordsDeleted = 0;
            RecordsFailed = 0;
            RecordsTotal = 0;

            Errors = new MapperRecordErrors();
        }
    }

    public class MapperRecordErrors : IEnumerable<MapperRecordError>
    {
        private Dictionary<int, MapperRecordError> _Errors;

        public MapperRecordErrors()
        {
            _Errors = new Dictionary<int, MapperRecordError>();
        }

        public MapperRecordError this[int record]
        {
            get
            {
                return _Errors[record];
            }
        }

        public bool ContainsRecord(int record)
        {
            return _Errors.ContainsKey(record);
        }

        public int RecordCount
        {
            get
            {
                return _Errors.Count();
            }
        }

        public IEnumerator<MapperRecordError> GetEnumerator()
        {
            foreach (var recordError in _Errors.Values)
            {
                yield return recordError;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddError(int record, string error)
        {
            MapperRecordError recordError;

            if (_Errors.ContainsKey(record))
                recordError = _Errors[record];
            else
            {
                recordError = new MapperRecordError(record);
                _Errors.Add(record, recordError);
            }
            recordError.AddErrorMessage(error);
        }

        public void AddErrors(int record, IEnumerable<string> errors)
        {
            MapperRecordError recordError;

            if (_Errors.ContainsKey(record))
                recordError = _Errors[record];
            else
            {
                recordError = new MapperRecordError(record);
                _Errors.Add(record, recordError);
            }
            recordError.AddErrorMessages(errors);
        }
    }

    public class MapperRecordError
    {
        public int RecordNumber { get; set; }

        private List<string> _Errors;
        public IReadOnlyCollection<string> Errors
        {
            get { return _Errors.AsReadOnly(); }
        }

        public MapperRecordError(int recordNumber)
        {
            RecordNumber = recordNumber;
            _Errors = new List<string>();
        }

        public void AddErrorMessage(string message)
        {
            _Errors.Add(message);
        }

        public void AddErrorMessages(IEnumerable<string> messages)
        {
            _Errors.AddRange(messages);
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

        public async Task<MapperUploadResult> UploadFileAsync(string importName, string fileName, CancellationToken cancellationToken)
        {

            var requestContent = new MultipartFormDataContent();
            requestContent.Add(new StringContent(User), "USER");
            requestContent.Add(new StringContent(Password), "PASSWORD");
            requestContent.Add(new StringContent(FileLibrary), "DATABASE");
            requestContent.Add(new StringContent("UNITTEST"), "SERVICE");
            requestContent.Add(new StringContent(importName), "IMPORT");

            var fileStream = File.OpenRead(fileName);
            requestContent.Add(new StreamContent(fileStream), "FILE", fileName);            

            var httpClient = new HttpClient();     
            var httpResponce = await httpClient.PostAsync("http://" + Server + "/cgi-bin/precedawebservice", requestContent, cancellationToken);

            fileStream.Close();

            /* Check the request was successful */
            httpResponce.EnsureSuccessStatusCode();

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
            foreach (XmlNode recordError in recordErrors)
            {
                var recordNumber = int.Parse(recordError.Attributes["number"].Value);
              
                var errors = new List<string>();
                var messages = recordError.SelectNodes("message");
                foreach (XmlNode message in messages)
                    errors.Add(message.InnerText);

                result.Errors.AddErrors(recordNumber, errors);
            }

            return result;
        }
    }


}

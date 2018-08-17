using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.IO;
using System.Net;
using System.Xml;
using System.Net.Http;
using System.Net.Http.Headers;

namespace XmlTransform
{
    public class XmlTransformResult
    {
        public bool Successfull { get; set; }
        public string Error { get; set; }
        public string ResultFile { get; set; }
    }


    public class XmlTransformRequest
    { 
        public string Server { get; private set; }
        public string User { get; private set; }
        public string Password { get; private set; }
        public string FileLibrary { get; private set; }

        public XmlTransformRequest(string server, string user, string password, string fileLibrary)
        {
            Server = server;
            User = user;
            Password = password;
            FileLibrary = fileLibrary;
        }

        public async Task<XmlTransformResult> UploadFileAsync(string transformName, string fileName, string resultFileName, CancellationToken cancellationToken)
        {

            var requestContent = new MultipartFormDataContent();
            requestContent.Add(new StringContent(User), "USER");
            requestContent.Add(new StringContent(Password), "PASSWORD");
            requestContent.Add(new StringContent(FileLibrary), "DATABASE");
            requestContent.Add(new StringContent("UNITTEST"), "SERVICE");
            requestContent.Add(new StringContent("XMLTRANSFORM"), "TASKTYPE");
            requestContent.Add(new StringContent(transformName), "TRANSFORM");

            var fileStream = File.OpenRead(fileName);
            requestContent.Add(new StreamContent(fileStream), "FILE", fileName);

            var httpClient = new HttpClient();
            var httpResponce = await httpClient.PostAsync("https://" + Server + "/cgi-bin/precedawebservice", requestContent, cancellationToken);

            fileStream.Close();

            /* Check the request was successful */
            if (!httpResponce.IsSuccessStatusCode)
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


            var result = new XmlTransformResult();

            /* Check for an error */
            var errors = xmlResult.SelectNodes("errors/error");
            if (errors.Count > 0)
            {
                result.Successfull = false;
                result.Error = errors[0].InnerText;
                result.ResultFile = "";
            }
            else
            {
                result.Successfull = true;
                result.Error = "";
                result.ResultFile = resultFileName;
                var resultFile = File.CreateText(result.ResultFile);

                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var xmlWriter = XmlWriter.Create(resultFile, settings);

                xmlResult.WriteTo(xmlWriter);

                xmlWriter.Close();

                resultFile.Close();
            }

            return result;
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;

using PayrollExchange.WebService;

namespace PayrollExchange
{
    public struct CustomerCode
    {
        public string Global;
        public string Local;

        public CustomerCode(string globalCustomerCode, string localCustomerCode)
        {
            Global = globalCustomerCode;
            Local = localCustomerCode;
        }
    }

    public enum BodProcessingStep { Pending, SendRequest, InPreceda, Complete }
    public enum BodStatus { InProgress, OK, Failed }

    public class BodUploadProgress
    {
        public Guid BodId { get; set; }
        public BodProcessingStep ProcessingStep { get; set; }
        public string PrecedaProcessingStage { get; set; }
        public string PrecedaStatus { get; set; }    
    }

    public class BodUploadResult
    {
        public Guid BodId { get; set; }
        public BodProcessingStep ProcessingStep { get; set; }
        public BodStatus Status { get; set; }
        public string Message { get; set; }
        public string PrecedaProcessingStage { get; set; }
        public string PrecedaStatus { get; set; }
        public string IdNumber { get; set; }
        public string MapperId { get; set; }

        public void SetStatus(int precedaStatus)
        {
            if (precedaStatus == 1)
                Status = BodStatus.InProgress;
            else if (precedaStatus == 2)
                Status = BodStatus.Failed;
            else if ((precedaStatus == 3) || (precedaStatus == 4) || (precedaStatus == 5) || (precedaStatus == 6))
                Status = BodStatus.OK;  
        }
    }

    public class BodInjector
    {
        private InboundXMLRequestServicePortTypeClient _WebService;
        private PrecedaQuery _PrecedaQuery;

        public string User { get; private set; }
        public string Password { get; private set; }
        public string Server { get; private set; } 
        public string FileLibrary { get; private set; } 
        public CustomerCode CustomerCode { get; private set; }      

        public BodInjector(string server, string user, string password, string fileLibrary)
        {
            Server = server;
            User = user;
            Password = password;
            FileLibrary = fileLibrary;

            _PrecedaQuery = new PrecedaQuery(server, user, password, fileLibrary);
            CustomerCode = _PrecedaQuery.GetCustomerCode();

            // Determine remote address and binding 
            var url = "http://" + server + ":8081/cgi-bin/jsmdirect?PEIREQUEST";
            var remoteAddress = new EndpointAddress(url);
            WSHttpBinding binding;
            if (remoteAddress.Uri.Scheme == "https")
                binding = new WSHttpBinding(SecurityMode.Transport);
            else
                binding = new WSHttpBinding(SecurityMode.None);

            _WebService = new InboundXMLRequestServicePortTypeClient(binding, remoteAddress);
        }

        public BodUploadResult UploadBod(PayrollExchangeBod bod)
        {
            bod.SetCustomerCode(CustomerCode);
    
            try
            {
                _WebService.request(User, Password, Encoding.Default.GetBytes(bod.XML.OuterXml));
            }
            catch (Exception e)
            {
                 return new BodUploadResult()
                    {
                        BodId = bod.Id,
                        ProcessingStep = BodProcessingStep.SendRequest,
                        Status = BodStatus.Failed,
                        Message = e.Message
                    };
            }

            while (true)
            {
                Thread.Sleep(10000); 

                try
                {
                    var precedaResult = _PrecedaQuery.GetBodStatus(bod);

                    if (precedaResult.Status >= 2)
                    {
                        return new BodUploadResult()
                        {
                            BodId = bod.Id,
                            ProcessingStep = BodProcessingStep.Complete,
                            Status = precedaResult.Status == 2 ? BodStatus.Failed : BodStatus.OK,
                            Message = "",
                            PrecedaProcessingStage = precedaResult.ProcessingStage,
                            PrecedaStatus = precedaResult.StatusDescription,
                            IdNumber = precedaResult.IdNumber,
                            MapperId = precedaResult.MapperId
                        };
                    }
                }
                catch (Exception e)
                {
                    return new BodUploadResult()
                    {
                        BodId = bod.Id,
                        ProcessingStep = BodProcessingStep.InPreceda,
                        Status = BodStatus.Failed,
                        Message = e.Message
                    };
                }
            }
        }

        public async Task<BodUploadResult> UploadBodAsync(PayrollExchangeBod bod)
        {
            return await UploadBodAsync(bod, CancellationToken.None, null);
        }

        public async Task<BodUploadResult> UploadBodAsync(PayrollExchangeBod bod, CancellationToken cancellationToken)
        {
            return await UploadBodAsync(bod, cancellationToken, null);
        }

        public async Task<BodUploadResult> UploadBodAsync(PayrollExchangeBod bod, IProgress<BodUploadProgress> progress)
        {
            return await UploadBodAsync(bod, CancellationToken.None, progress);
        }

        public async Task<BodUploadResult> UploadBodAsync(PayrollExchangeBod bod, CancellationToken cancellationToken, IProgress<BodUploadProgress> progress)
        {
            BodUploadProgress uploadProgess = new BodUploadProgress()
            {
                BodId = bod.Id,
                ProcessingStep = BodProcessingStep.Pending,
                PrecedaProcessingStage = "",
                PrecedaStatus = ""
            };

            bod.SetCustomerCode(CustomerCode);

            try
            {
                await _WebService.requestAsync(User, Password, Encoding.Default.GetBytes(bod.XML.OuterXml));
            }
            catch (Exception e)
            {
                return new BodUploadResult()
                {
                    BodId = bod.Id,
                    ProcessingStep = BodProcessingStep.SendRequest,
                    Status = BodStatus.Failed,
                    Message = e.Message
                };
            }

            uploadProgess.ProcessingStep = BodProcessingStep.InPreceda;
            if (progress != null)
                progress.Report(uploadProgess);
            cancellationToken.ThrowIfCancellationRequested();

            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

                try
                {
                    var precedaResult = _PrecedaQuery.GetBodStatus(bod);

                    if (precedaResult.Status == 1)

                    {
                        uploadProgess.PrecedaProcessingStage = precedaResult.ProcessingStage;
                        uploadProgess.PrecedaStatus = precedaResult.StatusDescription;
                        if (progress != null)
                            progress.Report(uploadProgess);
                    }
                    else if ((precedaResult.Status == 6) && (precedaResult.IdNumber == "*HIRE"))
                    {
                        /* This test is added to handle case were Preceda updates the status during the validation stage */
                        uploadProgess.PrecedaProcessingStage = precedaResult.ProcessingStage;
                        uploadProgess.PrecedaStatus = precedaResult.StatusDescription;
                        if (progress != null)
                            progress.Report(uploadProgess);
                    }
                    else if (precedaResult.Status >= 2)
                    {
                        return new BodUploadResult()
                        {
                            BodId = bod.Id,
                            ProcessingStep = BodProcessingStep.Complete,
                            Status = precedaResult.Status == 2 ? BodStatus.Failed : BodStatus.OK,
                            Message = "",
                            PrecedaProcessingStage = precedaResult.ProcessingStage,
                            PrecedaStatus = precedaResult.StatusDescription,
                            IdNumber = precedaResult.IdNumber,
                            MapperId = precedaResult.MapperId
                        };
                    }
                }
                catch (Exception e)
                {
                    return new BodUploadResult()
                    {
                        BodId = bod.Id,
                        ProcessingStep = BodProcessingStep.InPreceda,
                        Status = BodStatus.Failed,
                        Message = e.Message
                    };
                }
            }


        }

    }
}

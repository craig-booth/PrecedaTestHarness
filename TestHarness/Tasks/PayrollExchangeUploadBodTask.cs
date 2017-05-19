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
    public class PayrollExchangeUploadBodTaskResult
    {
        public string ProcessingStage { get; set; }
        public string Status { get; set; }
        public List<BodError> ValidationErrors { get; set; }

        public PayrollExchangeUploadBodTaskResult()
        {
            ValidationErrors = new List<BodError>();
        }

        public void WriteValue(StringWriter output)
        {
            output.WriteLine("Preceda processing stage {0}, status {1}", ProcessingStage, Status);

            if ((ProcessingStage == "Data Validation") && (Status == "Failed"))
            {
                output.WriteLine("Mapper errors:");            
                foreach (BodError error in ValidationErrors)
                    output.WriteLine("  import {0}: {1}", new object[] {error.ImportNumber, error.Message});
            }
        }

        public bool Equals(PayrollExchangeUploadBodTaskResult value)
        {
            if ((ProcessingStage != value.ProcessingStage) || (Status != value.Status))
                return false;

            // TODO: Check if validation errors match 

            return true;
        }
    }

    public class PayrollExchangeUploadBodTask : ITask
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Description
        {
            get
            {
                return "Payroll Exchange Upload";
            }
        }
        public string Message { get; private set; }
        public TaskResult Result { get; }

        public string FileName { get; private set; }
        public PayrollExchangeUploadBodTaskResult ExpectedResult;
        public PayrollExchangeUploadBodTaskResult ActualResult;
        public Guid BodId;
        public string PrecedaId;

        public PayrollExchangeUploadBodTask(XmlNode xml, string directory)
        {
            FileName = Path.Combine(directory, xml.Attributes["file"].Value);
            ExpectedResult = new PayrollExchangeUploadBodTaskResult()
            {
                ProcessingStage = "Confirmation Sent",
                Status = "Successful Completion"
            };
            ActualResult = null;
            BodId = Guid.Empty;

            var expectedResultNode = xml.SelectSingleNode("expectedresult");
            if (expectedResultNode != null)
            {
                ExpectedResult.ProcessingStage = expectedResultNode.SelectSingleNode("processingstage").InnerText;
                ExpectedResult.Status = expectedResultNode.SelectSingleNode("status").InnerText;
            }
        }


        public async Task<bool> RunAsync(Dictionary<string, string> variables, TestOutputFileNameGenerator fileNameGenerator, CancellationToken cancellationToken, IProgress<TestProgress> progess)
        {
            StringWriter output = new StringWriter();

            var bod = new PayrollExchangeBod(FileName);

            BodId = bod.Id;

            output.WriteLine("Uploading \"{0}\" as {1}", new Object[] { FileName, BodId.ToString() });

            BodInjector injector = new BodInjector(variables["SERVER"], variables["USER"], variables["PASSWORD"], variables["FILELIBRARY"]);

            BodUploadResult uploadResult;
            if (bod.IsHireBod())
            {
                uploadResult = await injector.UploadBodAsync(bod, cancellationToken);

                if (uploadResult.Status == BodStatus.OK)
                {
                    output.WriteLine("Upload successfull. Hired Id Number {0}", uploadResult.IdNumber);
                    variables["IDNUMBER"] = uploadResult.IdNumber;
                }
            }
            else
            {
                if (variables.TryGetValue("IDNUMBER", out PrecedaId))
                {
                    output.WriteLine("Using Id Number {0}", PrecedaId);
                    bod.SetPrecedaId(PrecedaId);
                }

                uploadResult = await injector.UploadBodAsync(bod, cancellationToken);
            }

            ActualResult = new PayrollExchangeUploadBodTaskResult();
            ActualResult.ProcessingStage = uploadResult.PrecedaProcessingStage;
            ActualResult.Status = uploadResult.PrecedaStatus;
            if (uploadResult.Status != BodStatus.OK)
            {
                var precedaQuery = new PrecedaQuery(variables["SERVER"], variables["USER"], variables["PASSWORD"], variables["FILELIBRARY"]);
                var mapperErrors = precedaQuery.GetBodErrors(uploadResult.MapperId);
                ActualResult.ValidationErrors.AddRange(mapperErrors);
            }

            // Output results
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

        public void ViewResult()
        {

        }
    }
}

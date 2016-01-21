using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PayrollExchange
{

    public class PayrollExchangeBod
    {
        private XmlNamespaceManager _NameSpaceManager;

        public Guid Id { get; private set; }
        public XmlDocument XML { get; private set; }

        private PayrollExchangeBod()
        {
            Id = Guid.NewGuid();
        }

        public PayrollExchangeBod(XmlDocument xmlBod)
            : this()
        {
            XML = new XmlDocument();
            var childNode = XML.ImportNode(xmlBod.FirstChild, true);
            XML.AppendChild(childNode);

            // Create an XmlNamespaceManager for resolving namespaces
            _NameSpaceManager = new XmlNamespaceManager(XML.NameTable);
            _NameSpaceManager.AddNamespace("nga", "http://www.ngahr.com/ngapexxml/1");
            _NameSpaceManager.AddNamespace("oa", "http://www.openapplications.org/oagis/9");
            _NameSpaceManager.AddNamespace("hr", "http://www.hr-xml.org/3");

            SetBodId(Id);
        }

        public PayrollExchangeBod(string fileName)
            : this()
        {
            XML = new XmlDocument();
            XML.Load(fileName);

            // Create an XmlNamespaceManager for resolving namespaces
            _NameSpaceManager = new XmlNamespaceManager(XML.NameTable);
            _NameSpaceManager.AddNamespace("nga", "http://www.ngahr.com/ngapexxml/1");
            _NameSpaceManager.AddNamespace("oa", "http://www.openapplications.org/oagis/9");
            _NameSpaceManager.AddNamespace("hr", "http://www.hr-xml.org/3");

            SetBodId(Id);
        }

        private void SetBodId(Guid id)
        {
            // Update BOD Id in BOD            
            var idNode = XML.SelectSingleNode("//oa:ApplicationArea/oa:BODID", _NameSpaceManager);
            idNode.InnerText = id.ToString();
        }

        public void SetCustomerCode(CustomerCode customerCode)
        {
            var logicalIdNode = XML.SelectSingleNode("//oa:Sender/oa:LogicalID", _NameSpaceManager);
            logicalIdNode.InnerText = customerCode.Global  + "-" + customerCode.Local + "-0000";
        }

        public void SetPrecedaId(string precedaId)
        {
            var payServEmp = XML.SelectSingleNode("//nga:DataArea/nga:PayServEmp", _NameSpaceManager);

            var payServEmpExtension = payServEmp.SelectSingleNode("nga:PayServEmpExtension", _NameSpaceManager);
            if (payServEmpExtension == null)
            {
                payServEmpExtension = XML.CreateElement("nga", "PayServEmpExtension", "http://www.ngahr.com/ngapexxml/1");
                payServEmp.AppendChild(payServEmpExtension);
            }
            var localPayrollData = payServEmpExtension.SelectSingleNode("nga:LocalPayrollData", _NameSpaceManager);
            var alternateIdendifiers = payServEmpExtension.SelectSingleNode("nga:AlternateIdentifiers", _NameSpaceManager);
            if (alternateIdendifiers == null)
            {
                alternateIdendifiers = XML.CreateElement("nga", "AlternateIdentifiers", "http://www.ngahr.com/ngapexxml/1");
                if (localPayrollData != null)
                    payServEmpExtension.InsertBefore(alternateIdendifiers, localPayrollData);
                else
                    payServEmpExtension.AppendChild(alternateIdendifiers);

                var payrollExhangeIdNode = XML.CreateElement("nga", "PayrollExchangeID", "http://www.ngahr.com/ngapexxml/1");
                alternateIdendifiers.AppendChild(payrollExhangeIdNode);
                payrollExhangeIdNode.InnerText = "000000001";

                var precedaIdNode = XML.CreateElement("nga", "PayServID", "http://www.ngahr.com/ngapexxml/1");
                alternateIdendifiers.AppendChild(precedaIdNode);
                precedaIdNode.InnerText = precedaId;
            }
            else
            {
                var precedaIdNode = alternateIdendifiers.SelectSingleNode("nga:PayServID", _NameSpaceManager);
                if (precedaIdNode != null)
                    precedaIdNode.InnerText = precedaId;
            }
        }

        public bool IsHireBod()
        {
            var hireNode = XML.SelectSingleNode("//hr:Hire", _NameSpaceManager);
            var precedaIdNode = XML.SelectSingleNode("//nga:PayServID", _NameSpaceManager);

            if ((hireNode != null) && (precedaIdNode == null))
                return true;
            else
                return false;
        }

    }
}

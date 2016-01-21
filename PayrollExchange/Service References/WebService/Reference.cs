﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PayrollExchange.WebService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://soap.service.ngatest.com", ConfigurationName="WebService.InboundXMLRequestServicePortType")]
    public interface InboundXMLRequestServicePortType {
        
        // CODEGEN: Generating message contract since element name username from namespace http://soap.service.ngatest.com is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="request", ReplyAction="*")]
        PayrollExchange.WebService.requestResponse request(PayrollExchange.WebService.requestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="request", ReplyAction="*")]
        System.Threading.Tasks.Task<PayrollExchange.WebService.requestResponse> requestAsync(PayrollExchange.WebService.requestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="noop", ReplyAction="*")]
        void noop();
        
        [System.ServiceModel.OperationContractAttribute(Action="noop", ReplyAction="*")]
        System.Threading.Tasks.Task noopAsync();
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class requestRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="request", Namespace="http://soap.service.ngatest.com", Order=0)]
        public PayrollExchange.WebService.requestRequestBody Body;
        
        public requestRequest() {
        }
        
        public requestRequest(PayrollExchange.WebService.requestRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://soap.service.ngatest.com")]
    public partial class requestRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string username;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string password;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public byte[] requestXML;
        
        public requestRequestBody() {
        }
        
        public requestRequestBody(string username, string password, byte[] requestXML) {
            this.username = username;
            this.password = password;
            this.requestXML = requestXML;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class requestResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="requestResponse", Namespace="http://soap.service.ngatest.com", Order=0)]
        public PayrollExchange.WebService.requestResponseBody Body;
        
        public requestResponse() {
        }
        
        public requestResponse(PayrollExchange.WebService.requestResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://soap.service.ngatest.com")]
    public partial class requestResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public byte[] requestReturn;
        
        public requestResponseBody() {
        }
        
        public requestResponseBody(byte[] requestReturn) {
            this.requestReturn = requestReturn;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface InboundXMLRequestServicePortTypeChannel : PayrollExchange.WebService.InboundXMLRequestServicePortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class InboundXMLRequestServicePortTypeClient : System.ServiceModel.ClientBase<PayrollExchange.WebService.InboundXMLRequestServicePortType>, PayrollExchange.WebService.InboundXMLRequestServicePortType {
        
        public InboundXMLRequestServicePortTypeClient() {
        }
        
        public InboundXMLRequestServicePortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public InboundXMLRequestServicePortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InboundXMLRequestServicePortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InboundXMLRequestServicePortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        PayrollExchange.WebService.requestResponse PayrollExchange.WebService.InboundXMLRequestServicePortType.request(PayrollExchange.WebService.requestRequest request) {
            return base.Channel.request(request);
        }
        
        public byte[] request(string username, string password, byte[] requestXML) {
            PayrollExchange.WebService.requestRequest inValue = new PayrollExchange.WebService.requestRequest();
            inValue.Body = new PayrollExchange.WebService.requestRequestBody();
            inValue.Body.username = username;
            inValue.Body.password = password;
            inValue.Body.requestXML = requestXML;
            PayrollExchange.WebService.requestResponse retVal = ((PayrollExchange.WebService.InboundXMLRequestServicePortType)(this)).request(inValue);
            return retVal.Body.requestReturn;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<PayrollExchange.WebService.requestResponse> PayrollExchange.WebService.InboundXMLRequestServicePortType.requestAsync(PayrollExchange.WebService.requestRequest request) {
            return base.Channel.requestAsync(request);
        }
        
        public System.Threading.Tasks.Task<PayrollExchange.WebService.requestResponse> requestAsync(string username, string password, byte[] requestXML) {
            PayrollExchange.WebService.requestRequest inValue = new PayrollExchange.WebService.requestRequest();
            inValue.Body = new PayrollExchange.WebService.requestRequestBody();
            inValue.Body.username = username;
            inValue.Body.password = password;
            inValue.Body.requestXML = requestXML;
            return ((PayrollExchange.WebService.InboundXMLRequestServicePortType)(this)).requestAsync(inValue);
        }
        
        public void noop() {
            base.Channel.noop();
        }
        
        public System.Threading.Tasks.Task noopAsync() {
            return base.Channel.noopAsync();
        }
    }
}

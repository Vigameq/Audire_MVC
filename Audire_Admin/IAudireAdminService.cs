using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Audire_Admin
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IAudireAdminService
    {
        
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetServiceURL")]
        CompanyDetail GetServiceURL(Paramerters param);

        // TODO: Add your service operations here
    }
    
    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompanyDetail
    {
        [DataMember]
        public string Company_code
        {
            get; set;
        }

        [DataMember]
        public string Company_Name
        {
            get; set;
        }
        [DataMember]
        public string Service_URL { get; set; }
        [DataMember]
        public int Error_Code { get; set; }
        [DataMember]
        public string Error_msg { get; set; }
    }

    public class Paramerters
    {
        public string company_code { get; set; }
    }
}

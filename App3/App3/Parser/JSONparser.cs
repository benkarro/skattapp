using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace App3.Parser
{
    class JSONparser
    {
        private JSONroot root { get; set; }


        // Find objekter
        public String linksFindHref { get; set;}
        public Boolean isTemplated { get; set; }
        public String portalviewHref { get; set; }
        public String selfHref { get; set; }

        // Embedded/messages

		public List<Messages> messages;

//        public String messageId { get; set; }
//        public String subject { get; set; }
//        public String status { get; set; }
//        public String lastChangedTime { get; set; }
//        public String changedBy { get; set; }
//        public String serviceOwner { get; set; }
//        public String type { get; set; }
//        public String serviceCode { get; set; }
//        public int serviceEdition { get; set; }
//        public String messageSelfHref { get; set; }
//        public String metadataHref { get; set; }
//        public String messagePortalviewHref { get; set; }



    
        public JSONparser(String JSON)
        {
            // Parsed JSON string
            Console.WriteLine("JSON: "+JSON);
            root = JsonConvert.DeserializeObject<JSONroot>(JSON);
            linksFindHref = root._links.find.href;
            isTemplated = root._links.find.isTemplated;

            portalviewHref = root._links.find.href;
            messages =root._embedded.messages;

//            for (int i = 0; i < messages.Count; i++)
//            {
//				Console.WriteLine(messages.Count);
//            }

        }

        

    }

    class JSONparserSingle
    {
       // public List<Messages> messages; //Added by Brage

        private JSONroot root { get; set; }
        #region Elements
        public String MessageId { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
        public String Status { get; set; }
        public String LastChangedTime { get; set; }
        public String LastchangedBy { get; set; }
        public String ServiceOwner { get; set; }
        public String Type { get; set; }
        public String ServiceCode { get; set; }
        public String ServiceEdition { get; set; }
       
        #endregion

        public String messageSelfHref { get; set; }
        public String metadataHref { get; set; }
        public String messagePortalviewHref { get; set; }
        public String form { get; set; }


        public JSONparserSingle(String JSON)
        {
            root = JsonConvert.DeserializeObject<JSONroot>(JSON);
            Subject = root.Subject;
            Status = root.Status;
            LastChangedTime = root.LastChangedDateTime;
            LastchangedBy = root.LastChangedBy;
            ServiceOwner = root.ServiceOwner;
            Body = root.Body;
            ServiceEdition = root.ServiceEdition; // Do int or stirng
            

        }


   }
}

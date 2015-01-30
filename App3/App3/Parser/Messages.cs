using System;
using System.Collections.Generic;
using System.Text;

namespace App3.Parser
{
    public class Messages
    {
        public string MessageId { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public string LastChangedDateTime { get; set; }
        public string LastChangedBy { get; set; }
        public string ServiceOwner { get; set; }
        public string Type { get; set; }
        public string ServiceCode { get; set; }
        public int ServiceEdition { get; set; }
        public Links _links { get; set; }
    }

}

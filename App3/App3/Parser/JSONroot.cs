using System;
using System.Collections.Generic;
using System.Text;

namespace App3.Parser
{
    class JSONroot
    {
        public Links _links { get;set; }
        public Embedded _embedded { get; set; }

        #region Single Message
            public String Subject { get; set; }
            public String Status { get; set; }
            public String Body { get; set; }
            public String LastChangedDateTime { get; set; }
            public String LastChangedBy { get; set; }
            public String ServiceOwner { get; set; }
            public String Type { get; set; }
            public String ServiceCode { get; set; }
            public String ServiceEdition { get; set; }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Parser
{
    public class Links
    {
      //  public Links(Self self, Metadata metadata, Portalview portalview, Print print)
      //  {
        //    this.self = self;
       //     this.metadata = metadata;
        //    this.portalview = portalview;
        //    this.print = print;
       // }

        public Self self { get; set; }
        public Metadata metadata { get; set; }
        public Portalview portalview { get; set; }
        public Print print { get; set; }
        public Find find { get; set; }
		public List<Attachment> attachment { get; set; }


        
    }
}

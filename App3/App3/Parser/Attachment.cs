﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Parser
{

    
    public class Attachment
    {
		public string href { get; set; }
		public string name { get; set; }
		public bool encrypted { get; set; }
		public bool signinglocked { get; set; }
		public bool signedbydefault { get; set; }
    }
}

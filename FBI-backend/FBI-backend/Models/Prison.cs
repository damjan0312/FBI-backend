using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBI_backend.Models
{
    public class Prison
    {
        public string area { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public long Id { get; set; }
        public string phone { get; set; }
        public string built { get; set; }
    }
}
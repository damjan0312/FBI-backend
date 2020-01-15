using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBI_backend.Models
{
    public class Crime
    {
        public long Id { get; set; }
        public String name { get; set; }
        public String punishment { get; set; }
    }
}
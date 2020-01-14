using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBI_backend.Models
{
    public class Information
    {
        public Criminal criminal { get; set; }
        public string prisons { get; set; }
        public string crimes { get; set; }
        public string agent { get; set; }

        public Information()
        {
            prisons = string.Empty;
            crimes = string.Empty;
            criminal = new Criminal();
        }
    }
}
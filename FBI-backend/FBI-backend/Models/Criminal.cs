using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBI_backend.Models
{
    public class Criminal
    {
        public string reward { get; set; }
        public string placeOfBirth { get; set; }
        public string occupation { get; set; }
        public string race{get;set;}
        public string dateOfBirth { get; set; }
        public string eyes { get; set; }
        public string sex { get; set; }
        public string hair { get; set; }
        public string nationality { get; set; }
        public string weight { get; set; }
        public string height { get; set; }
        public string name { get; set; }
        public long Id { get; set; }
    }
}
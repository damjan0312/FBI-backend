using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBI_backend.Models
{
    public class CommitedRelationship
    {
        public long criminalId { get; set; }
        public long crimeId { get; set; }
        public string caution { get; set; }
    }
}
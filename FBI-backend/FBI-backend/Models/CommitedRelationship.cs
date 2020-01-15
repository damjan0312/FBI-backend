using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBI_backend.Models
{
    public class CommitedRelationship
    {
        public long criminalId { get; set; }
        public string crimeName { get; set; }
        public string caution { get; set; }
    }
}
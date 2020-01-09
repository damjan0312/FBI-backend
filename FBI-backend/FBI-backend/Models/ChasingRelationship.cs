using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBI_backend.Models
{
    public class ChasingRelationship
    {
        public long agentId { get; set; }
        public long criminalId { get; set; }
        public string period { get; set; }
    }
}
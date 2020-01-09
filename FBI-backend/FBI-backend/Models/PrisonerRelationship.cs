using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBI_backend.Models
{
    public class PrisonerRelationship
    {
        public long prisonId { get; set; }
        public long criminalId { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
    }
}
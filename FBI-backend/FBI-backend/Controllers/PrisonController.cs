using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FBI_backend.Models;
using FBI_backend.Services;

namespace FBI_backend.Controllers
{
    public class PrisonController : ApiController
    {
        [System.Web.Http.Route("api/prison")]
        public IEnumerable<Prison> GetPrisons()
        {
            return PrisonService.GetPrisons();
        }
    }
}

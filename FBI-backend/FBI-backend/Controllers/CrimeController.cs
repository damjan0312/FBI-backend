using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Neo4jClient;
using Neo4jClient.Cypher;
using FBI_backend.Models;
using FBI_backend.Services;

namespace FBI_backend.Controllers
{

    public class CrimeController : ApiController
    {

        [System.Web.Http.Route("api/crime")]
        public IEnumerable<Crime> getCrimes()
        {
            //CrimeService.CreateCrime("Murder", "10 - 40 years");
            //CrimeService.CreateCrime("Terorrism", "3 - 20 years");
            //CrimeService.CreateCrime("Kidnapping", "6 - 25 years");
            //CrimeService.CreateCrime("Robbery", "2 - 15 years");
        //    CrimeService.deleteCrimes();
            return CrimeService.GetCrimes();
        }

    }
}

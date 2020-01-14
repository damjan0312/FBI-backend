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
    public class CriminalController : ApiController
    {
        [System.Web.Http.Route("api/criminal")]
        public IEnumerable<Criminal> GetCriminals()
        {
            return CriminalService.GetCriminals();
        }

        [System.Web.Http.Route("api/criminal/add")]
        public bool AddCriminal(Criminal newCriminal)
        {
            return CriminalService.CreateCriminal(newCriminal);
        }

        [System.Web.Http.Route("api/criminal/delete")]
        public bool DeleteCriminal(long Id)
        {
            return CriminalService.DeleteCriminal(1);
        }

        [System.Web.Http.Route("api/criminal/addCommited")]
        public bool AddCommitedRelationship(CommitedRelationship c)
        {
            return CriminalService.addCommitedRelationship(c);
        }

        [System.Web.Http.Route("api/criminal/addChasing")]
        public bool AddChasingRelationship(ChasingRelationship c)
        {
            return CriminalService.addChasingRelationship(c);
        }

        [System.Web.Http.Route("api/criminal/addPrisoner")]
        public bool AddPrisonerRelationship(PrisonerRelationship p)
        {
            return CriminalService.addPrisonerRelationship(p);
        }

        [System.Web.Http.Route("api/criminal/getInformation")]
        public Information GetInformation(long id)
        {
            return CriminalService.getInfromation(id);
        }

    }
}

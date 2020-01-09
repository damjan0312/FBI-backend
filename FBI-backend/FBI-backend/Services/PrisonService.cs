using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using Neo4jClient;
using Neo4jClient.Cypher;
using FBI_backend.Models;

namespace FBI_backend.Services
{
    public class PrisonService
    {
        public static IEnumerable<Prison> GetPrisons()
        {
            if (ConnectionService.client == null)
                ConnectionService.Connect();
            try
            {
                var query = new Neo4jClient.Cypher.CypherQuery("match (prison:Prison) where exists(prison.name) return Distinct(prison)",
                new Dictionary<string, Object>(), CypherResultMode.Set);

                return ((IRawGraphClient)ConnectionService.client).ExecuteGetCypherResults<Prison>(query);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
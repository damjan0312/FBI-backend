using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Neo4jClient;
using Neo4jClient.Cypher;
using FBI_backend.Models;

namespace FBI_backend.Services
{
    public class CriminalService : ApiController
    {
        public static IEnumerable<Criminal> GetCriminals()
        {
            if (ConnectionService.client == null)
                ConnectionService.Connect();
            try
            {
                var query = new Neo4jClient.Cypher.CypherQuery(" match (criminal:Criminal) where exists(criminal.name) return Distinct(criminal)",
                new Dictionary<string, Object>(), CypherResultMode.Set);
                
                return ((IRawGraphClient)ConnectionService.client).ExecuteGetCypherResults<Criminal>(query);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool CreateCriminal(Criminal newCriminal)
        {
            if (ConnectionService.client == null)
                ConnectionService.Connect();
            try
            {
                newCriminal.Id = Convert.ToInt32( getMaxId())+1;
                ConnectionService.client.Cypher
                    .Create("(criminal:Criminal {newCriminal})")
                    .WithParam("newCriminal", newCriminal)
                    .ExecuteWithoutResults();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool DeleteCriminal(long Id)
        {
            if (ConnectionService.client == null)
                ConnectionService.Connect();
            try
            {
                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("Id", Id);

                var query = new Neo4jClient.Cypher.CypherQuery("match (c:Criminal) where exists(c.Id) and c.Id ={Id} detach delete c",
                                                                queryDict,CypherResultMode.Projection);

                ((IRawGraphClient)ConnectionService.client).ExecuteCypher(query);

                return true;

                
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool addCommitedRelationship(CommitedRelationship newR)
        {
            if (ConnectionService.client == null)
                ConnectionService.Connect();
            try
            {
                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("criminalId", newR.criminalId);
                queryDict.Add("crimeId", newR.crimeId);
                //queryDict.Add("caution", newR.caution);

                var query = new Neo4jClient.Cypher.CypherQuery("Match(criminal:Criminal) Match(crime:Crime) Where criminal.Id={criminalId} and crime.Id={crimeId} CREATE UNIQUE (crime)<-[c:COMMITED{caution:\""+newR.caution+"\"}]-(criminal)",
                                                                queryDict, CypherResultMode.Projection);

                ((IRawGraphClient)ConnectionService.client).ExecuteCypher(query);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool addChasingRelationship(ChasingRelationship newR)
        {
            if (ConnectionService.client == null)
                ConnectionService.Connect();
            try
            {
                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("agentId", newR.agentId);
                queryDict.Add("criminalId", newR.criminalId);
                //queryDict.Add("caution", newR.caution);

                var query = new Neo4jClient.Cypher.CypherQuery("Match(criminal:Criminal) Match(agent:Agent) Where criminal.Id={criminalId} and agent.Id={agentId} CREATE UNIQUE (agent)-[c:CHASING{period:\"" + newR.period + "\"}]->(criminal)",
                                                                queryDict, CypherResultMode.Projection);

                ((IRawGraphClient)ConnectionService.client).ExecuteCypher(query);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool addPrisonerRelationship(PrisonerRelationship newR)
        {
            if (ConnectionService.client == null)
                ConnectionService.Connect();
            try
            {
                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("criminalId", newR.criminalId);
                queryDict.Add("prisonId", newR.prisonId);
                //queryDict.Add("caution", newR.caution);

                var query = new Neo4jClient.Cypher.CypherQuery("Match(criminal:Criminal) Match(prison:Prison) Where criminal.Id={criminalId} and prison.Id={prisonId} CREATE UNIQUE (criminal)-[p:PRISONER{dateFrom:\"" + newR.dateFrom + "\",dateTo:\"" + newR.dateTo + "\"}]->(prison)",
                                                                queryDict, CypherResultMode.Projection);

                ((IRawGraphClient)ConnectionService.client).ExecuteCypher(query);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private static String getMaxId()
        {
            var query = new Neo4jClient.Cypher.CypherQuery("match(criminal:Criminal) where exists(criminal.Id) return max(criminal.Id)",
                                                            new Dictionary<string, object>(), CypherResultMode.Set);

            String maxId = ((IRawGraphClient)ConnectionService.client).ExecuteGetCypherResults<String>(query).ToList().FirstOrDefault();

            return maxId;
        }
    }
}

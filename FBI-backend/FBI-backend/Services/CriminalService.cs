using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Neo4jClient;
using Neo4jClient.Cypher;
using FBI_backend.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public static Information getInfromation(long CriminalId)
        {
            if (ConnectionService.client == null)
                ConnectionService.Connect();
            try
            {
                
                var query = ConnectionService.client.Cypher
                .Match("(criminal:Criminal) Match (criminal)-[commited:COMMITED]->(crime:Crime)")
                 .Where("criminal.Id = {criminalId}")
                 .WithParams(new
                 {
                     criminalId = CriminalId
                 })
                 .Return((criminal, commited, crime) => new {
                     Criminal = criminal.As<Criminal>(),
                     Commited = commited.As<Node<string>>(),
                     Crime = crime.As<Crime>(),
                 });

                

                var results=query.Results.ToList();
              
                Information inf = new Information();
                inf.criminal=results[0].Criminal;
                

                foreach (var result in results)
                {
                    dynamic data = JObject.Parse(result.Commited.Data);

                    inf.crimes += result.Crime.name + " : " +(string ) data.caution  + "\n";
                }

                 var query1 = ConnectionService.client.Cypher
                .Match("(criminal:Criminal)  Match(criminal) -[prisoner: PRISONER]->(prison: Prison) ")
                 .Where("criminal.Id = {criminalId}")
                 .WithParams(new
                 {
                     criminalId = CriminalId
                 })
                 .Return((prisoner, prison) => new {
                     Prisoner = prisoner.As<Node<string>>(),
                     Prison = prison.As<Prison>()
                 });

                var newResults = query1.Results.ToList();
                

                foreach (var result in newResults)
                {
                    dynamic data = JObject.Parse(result.Prisoner.Data);
                    inf.prisons += result.Prison.name + ":" + (string) data.dateFrom +"-"+ (string) data.dateTo  + "\n";
                }
                
                var query2 = ConnectionService.client.Cypher
                .Match("(criminal:Criminal) Match(agent:Agent) -[chasing: CHASING]->(criminal) ")
                 .Where("criminal.Id = {criminalId}")
                 .WithParams(new
                 {
                     criminalId = CriminalId
                 })
                 .Return((agent, chasing) => new {
                     Agent = agent.As<Node<string>>(),
                     Chasing = chasing.As<Node<string>>()
                 });

                var newResults1 = query2.Results.ToList();
                
                foreach (var result in newResults1)
                {
                    dynamic data = JObject.Parse(result.Agent.Data);
                    dynamic data1 = JObject.Parse(result.Chasing.Data);
                    inf.agent += (string)data.name + " -Period of chasing:" + (string)data1.period +"\n";
                }

                return inf;
            }
            catch (Exception ex)
            {

                return null;
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

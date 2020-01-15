using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jClient;
using Neo4jClient.Cypher;
using FBI_backend.Models;

namespace FBI_backend.Services
{
    public class CrimeService
    {

        public static IEnumerable<Crime> GetCrimes()
        {
            if (ConnectionService.client == null)
                ConnectionService.Connect();
            try
            {
                var query = new Neo4jClient.Cypher.CypherQuery("match (crime:Crime) where exists(crime.name) return crime",
                new Dictionary<string, Object>(), CypherResultMode.Set);
                
                return ((IRawGraphClient)ConnectionService.client).ExecuteGetCypherResults<Crime>(query);
            }
            catch(Exception ex)
            {
                return null;
            }

           

        }

        public static void CreateCrime(string name,string punishment)
        {
            if (ConnectionService.client == null)
                ConnectionService.Connect();
            try
            {
                int Id = Convert.ToInt32(getMaxId()) + 1;
                if (Id == null)
                    Id = 1;

                var newCrime = new Crime { Id = Id, name = name, punishment = punishment };
                ConnectionService.client.Cypher
                    .Create("(crime:Crime {newCrime})")
                    .WithParam("newCrime", newCrime)
                    .ExecuteWithoutResults();
                return;
            }
            catch (Exception ex)
            {
                
                return;
            }
        }

        public static void deleteCrimes()
        {
            if (ConnectionService.client == null)
                ConnectionService.Connect();
            try
            {
                ConnectionService.client.Cypher
                .Match("(crime:Crime)")
                .Where("crime.name='Terrorism'")
                .Delete("crime")
                .ExecuteWithoutResults();

                return ;
            }
            catch (Exception ex)
            {
                return ;
            }



        }

        private static String getMaxId()
        {
            var query = new Neo4jClient.Cypher.CypherQuery("match(rel:COMMITED) where exists(rel.Id) return max(rel.Id)",
                                                            new Dictionary<string, object>(), CypherResultMode.Set);

            String maxId = ((IRawGraphClient)ConnectionService.client).ExecuteGetCypherResults<String>(query).ToList().FirstOrDefault();

            return maxId;
        }
    }
}

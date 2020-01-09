using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBI_backend.Services
{
    public class ConnectionService
    {
       
        public static Neo4jClient.GraphClient client;

        public static bool Connect()
        {
            client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "password");
            try
            {
                client.Connect();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
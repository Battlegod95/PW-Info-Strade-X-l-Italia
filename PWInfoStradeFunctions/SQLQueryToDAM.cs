using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Dapper;

namespace PWInfoStradeFunctions
{
    public static class SQLQueryToDAM
    {
        [FunctionName("SQLQueryToDAM")]
        public static async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var str = Environment.GetEnvironmentVariable("sqldb_connection");
            using (SqlConnection conn = new SqlConnection(str))
            {
                var hour = DateTime.Now.Hour;
                conn.Open();
                var text = "INSERT INTO dbo.SimulatedInsert( "+
                    "Id_incrocio, Id_semaforo, Id_strada, Fascia_oraria, Data, Automobile)" + 
                    "SELECT Id_incrocio, Id_semaforo, " +
                    "Id_strada, Fascia_oraria " +
                    "Data AS Data, " +
                    "SUM(Automobile) AS Automobile, " +
                    "FROM dbo.TrafficTable " +
                    "WHERE Fascia_oraria = 13 " +
                    "GROUP BY Fascia_oraria, Id_incrocio, " +
                    "Id_semaforo, Id_strada, Data";

                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    var rows = await cmd.ExecuteNonQueryAsync();
                    log.LogInformation($"{rows}");
                }
            }
        }
    }
}

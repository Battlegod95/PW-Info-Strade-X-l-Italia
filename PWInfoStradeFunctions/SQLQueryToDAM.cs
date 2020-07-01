using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Azure.Amqp.Framing;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace PWInfoStradeFunctions
{
    //public static class SQLQueryToDAM
    //{
    //    [FunctionName("SQLQueryToDAM")]
    //    public static async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)  //0 0 * * * *
    //    {
    //        log.LogInformation($"Function Trigger viene eseguita ogni ora partendo da: {DateTime.Now}");

    //        var config = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory).
    //                AddJsonFile("local.settings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables().Build();

    //        //var str = ConfigurationManager.ConnectionStrings["sqldb_connection"].ConnectionString;

    //        var str = config["sqldb_connection"];

    //        using (SqlConnection conn = new SqlConnection(str))
    //        {
    //            conn.Open();
    //            var query = "";
    //            //{
    //                //INSERT INTO OPENDATASOURCE(

    //                //    'SQLNCLI', 'Server=pwsmartcross.database.windows.net;Database=smartcross;UID=its2020;PWD==Projectwork2020;').smartcross.dbo.Traffico
    //                //SELECT

    //                //    Id_incrocio,
	   //                // Id_semaforo,
	   //                // Id_strada,
	   //                // Fascia_oraria,
	   //                // Data,
	   //                // Tipologia_veicolo,
	   //                // SUM(Conteggio) AS Conteggio
    //                //FROM dbo.TrafficTable
    //                //WHERE Fascia_oraria = 12
    //                //GROUP BY Fascia_oraria, Id_incrocio, 
	   //                // Id_semaforo, Id_strada, Data, 
	   //                // Tipologia_veicolo, Conteggio
    //                //ORDER BY Fascia_oraria asc
    //            //}
                    
    //            using (SqlCommand cmd = new SqlCommand(query, conn))
    //            {
    //                var result = await cmd.ExecuteNonQueryAsync();
    //                log.LogInformation($"{result}");
    //            }
    //        }
    //    }
    //}
}

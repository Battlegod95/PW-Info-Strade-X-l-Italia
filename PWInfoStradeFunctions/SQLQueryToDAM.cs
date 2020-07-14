//using System;
//using Microsoft.Data.SqlClient;
//using System.Threading.Tasks;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Host;
//using Microsoft.Extensions.Logging;
//using Dapper;
//using Microsoft.Azure.Amqp.Framing;
//using System.Configuration;
//using Microsoft.Extensions.Configuration;
//using System.Data;
//using Newtonsoft.Json;
//using System.Collections.Generic;

//namespace PWInfoStradeFunctions
//{
//    public static class SQLQueryToDAM
//    {
//        [FunctionName("SQLQueryToDAM")]
//        public static void Run([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)  //0 0 * * * *
//        {
//            log.LogInformation($"Funzione triggerata per scrivere nel database comune con il DAM a partire da: {DateTime.Now}");

//            var config = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory).
//                    AddJsonFile("local.settings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables().Build();

//            //var str = ConfigurationManager.ConnectionStrings["sqldb_connection"].ConnectionString;

//            var connString = config["sqldb_connection"];
//            string sprocname = "RetrieveSumTipologiaVeicolo";
//            string jsonOutputParam = "@jsonOutputToDAM";

//            using (SqlConnection conn = new SqlConnection(connString))
//            {
//                conn.Open();
//                //var query =
//                //SELECT
//                //     [Id_incrocio]
//                //      ,[Id_semaforo]
//                //      ,[Id_strada]
//                //      ,[Fascia_oraria]
//                //      ,[Data]
//                //      ,[Tipologia_veicolo]
//                //      ,SUM(Conteggio) AS Conteggio
//                //FROM[dbo].[TrafficTable]
//                //WHERE Data = CAST(GETDATE() AS date)
//                //GROUP BY Tipologia_veicolo, Fascia_oraria, Id_incrocio, 
//                // Id_semaforo, Id_strada, Data
//                //ORDER BY Tipologia_veicolo ASC
//                //FOR JSON AUTO

//                using (SqlCommand cmd = new SqlCommand(sprocname, conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    cmd.Parameters.Add(jsonOutputParam, SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

//                    cmd.ExecuteNonQuery();

//                    var json = cmd.Parameters[jsonOutputParam].Value.ToString();

//                    log.LogInformation($"Il JSON ritornato della procedura è: { json }");

//                    List<SQLJsonObjectTipologiaVeicolo> obj = JsonConvert.DeserializeObject<List<SQLJsonObjectTipologiaVeicolo>>(json);

//                    if (obj != null)
//                    {
//                        var date = DateTime.Now;

//                        foreach (var i in obj)
//                        {
//                            if (i.Fascia_oraria == date.Hour)
//                            {
//                                var configuration = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory).
//                                    AddJsonFile("local.settings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables().Build();

//                                var connectionString = config["sqldb_connection_DAM"];

//                                SqlConnection cnn;

//                                cnn = new SqlConnection(connectionString);

//                                cnn.Open();

//                                SqlCommand command;
//                                SqlDataAdapter adapter = new SqlDataAdapter();
//                                string sql = "";

//                                sql = "INSERT INTO[dbo].[Traffico](" +
//                                   "Id_incrocio, " +
//                                   "Id_semaforo, " +
//                                   "Id_strada, " +
//                                   "Fascia_oraria, " +
//                                   "Data, " +
//                                   "Tipologia_veicolo, " +
//                                   "Conteggio) " +
//                                "VALUES (" +
//                                    i.Id_incrocio + ", " +
//                                     i.Id_semaforo + ", " +
//                                    i.Id_strada + ", " +
//                                    i.Fascia_oraria + ", " +
//                                    i.Data + ", " +
//                                    i.Tipologia_veicolo + ", " +
//                                    i.Conteggio + " " +
//                                " )";

//                                command = new SqlCommand(sql, cnn);

//                                adapter.InsertCommand = new SqlCommand(sql, cnn);
//                                adapter.InsertCommand.ExecuteNonQuery();

//                                command.Dispose();
//                                cnn.Close();
//                            }

//                        }
//                    }
//                    else
//                    {
//                        log.LogInformation($"Nessun dato disponibile");
//                    }
//                }
//            }
//        }
//    }
//}

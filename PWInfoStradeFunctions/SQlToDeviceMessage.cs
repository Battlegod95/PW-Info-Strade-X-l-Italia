using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Azure.Devices;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PWInfoStradeFunctions
{
    public static class SQlToDeviceMessage
    {
        [FunctionName("SQlToDeviceMessage")]
        public static async System.Threading.Tasks.Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {

            log.LogInformation($"Function Trigger viene eseguita ogni ora partendo da: {DateTime.Now}");

            var config = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory).
                    AddJsonFile("local.settings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables().Build();

            //var str = ConfigurationManager.ConnectionStrings["sqldb_connection"].ConnectionString;

            var connString = config["sqldb_connection_DAM"];
            string sprocname = "RetrievePerfCounterData";
            string jsonOutputParam = "@jsonOutput";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                //var query =

                //    "SELECT temporizzazione.[Id_temporizzazione] " +
                //    ",temporizzazione.[Id_incrocio] " +
                //    ",temporizzazione.[Id_semaforo] " +
                //    ",temporizzazione.[Fascia_oraria] " +
                //    ",temporizzazione.[Valore_tempo] " +
                //    "FROM [dbo].[Temporizzazione] temporizzazione " +
                //    "WHERE Id_incrocio = 4 AND Fascia_oraria = " + ora + " " +
                //    "FOR JSON AUTO;";

                using (SqlCommand cmd = new SqlCommand(sprocname, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(jsonOutputParam, SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    var json = cmd.Parameters[jsonOutputParam].Value.ToString();

                    log.LogInformation($"Il JSON ritornato della procedura è: { json }");

                    List<SQLJsonObject> obj = JsonConvert.DeserializeObject<List<SQLJsonObject>>(json);

                    var date = DateTime.Now;
                    foreach (var i in obj)
                    {
                        if (i.Fascia_oraria == date.Hour)
                        {
                            using (var manager = RegistryManager.CreateFromConnectionString("HostName=IoTHubTrafficLight.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=2NwrhbPzHZALsUpPNQWvVdbZyH4spDvGfnOAVw0JSBI="))
                            {
                                var twin = await manager.GetTwinAsync(i.Id_incrocio.ToString());
                                twin.Properties.Desired["Temporizzazione_verde"] = i.Valore_tempo;
                                await manager.UpdateTwinAsync(twin.DeviceId, twin, twin.ETag);
                            }
                        }
                     
                    }

                }
            }
        }
    }
}

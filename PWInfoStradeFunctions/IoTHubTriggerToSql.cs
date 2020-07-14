//using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Host;
//using Microsoft.Azure.EventHubs;
//using System.Text;
//using System.Net.Http;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Configuration;
//using System.Data.SqlClient;

//namespace PWInfoStradeFunctions
//{
//    public static class IoTHubTriggerToSql
//    {
//        private static HttpClient client = new HttpClient();

//        [FunctionName("IoTHubTriggerToSql")]
//        public static void Run([IoTHubTrigger("messages/events", Connection = "IotHubEndpoint")]EventData message, ILogger log, ExecutionContext context)
//        {
//            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");

//            var config = new ConfigurationBuilder()
//                .SetBasePath(context.FunctionAppDirectory)
//                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
//                .AddEnvironmentVariables().Build();

//            var connString = config["SqlCisternaConnectionString"];

//            DeviceData jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DeviceData>(Encoding.UTF8.GetString(message.Body.Array));

//            using (SqlConnection conn = new SqlConnection(connString))
//            {
//                conn.Open();
//                SqlCommand command;
//                SqlDataAdapter adapter = new SqlDataAdapter();
//                string sql = "";

//                sql = "INSERT INTO[dbo].[TrafficTable] " +
//                        "([Description] " +
//                        ",[Id_gateway] " +
//                        ",[Id_incrocio] " +
//                        ",[Id_semaforo] " +
//                        ",[Id_strada] " +
//                        ",[Stato_semaforo] " +
//                        ",[Data] " +
//                        ",[Tipologia_veicolo] " +
//                        ",[Conteggio]) " +
//                        "VALUES " +
//                        "(" + jsonData + " " +
//                        "," + jsonData + "" +
//                        "," + jsonData + ")";

//                command = new SqlCommand(sql, conn);

//                adapter.InsertCommand = new SqlCommand(sql, conn);
//                adapter.InsertCommand.ExecuteNonQuery();

//                command.Dispose();
//                conn.Close();
//            }
//        }
//    }
//}
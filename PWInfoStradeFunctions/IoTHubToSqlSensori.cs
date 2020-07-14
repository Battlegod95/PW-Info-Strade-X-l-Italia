using System;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PWInfoStradeFunctions
{
    public static class IoTHubToSqlSensori
    {
        [FunctionName("IoTHubToSqlSensori")]
        public static void Run([ServiceBusTrigger("iothubtostreamsensori", Connection = "DataTelemetricQueueConnectionString")]string myQueueItem, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables().Build();

            var connString = config["SqlPWConnectionString"];

            SensorData jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<SensorData>(myQueueItem);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                string sql = "";

                sql = "INSERT INTO[dbo].[SensorTable]" +
                        "([Description]" +
                        ", [Id_gateway]" +
                        ", [Id_incrocio]" +
                        ", [Crossroad_id]" +
                        ", [Intersection]" +
                        ", [Location]" +
                        ", [Healthy]" +
                        ", [Temperature]" +
                        ", [Humidity]" +
                        ", [Pressure]" +
                        ", [Coordinate_x]" +
                        ", [Coordinate_y])" +
                        "VALUES " +
                        "(" + jsonData.Description + 
                        ", " + jsonData.idGateway + 
                        ", " + jsonData.idIncrocio +
                        ", " + jsonData.CrossRoad + 
                        ", " + jsonData.Interserction + 
                        ", " + jsonData.Location + 
                        ", " + jsonData.Healthy + 
                        ", " + jsonData.Data.Temperature + 
                        ", " + jsonData.Data.Humidity + 
                        ", " + jsonData.Data.Pressure + 
                        ", " + jsonData.SensorLocation.coordinates.x + 
                        ", " + jsonData.SensorLocation.coordinates.y + ")";

                command = new SqlCommand(sql, conn);

                adapter.InsertCommand = new SqlCommand(sql, conn);
                adapter.InsertCommand.ExecuteNonQuery();
                

                command.Dispose();
                conn.Close();
            }
        }
    }
}

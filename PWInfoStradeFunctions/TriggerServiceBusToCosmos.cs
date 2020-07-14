//using System;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Host;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;

//namespace PWInfoStradeFunctions
//{
//    public static class TriggerServiceBusToCosmos
//    {
//        [FunctionName("TriggerServiceBusToCosmos")]
//        public static void Run(
//            [ServiceBusTrigger("iothubtostreamsensori", Connection = "ConnectionStringQueue")] string myQueueItem,
//            [CosmosDB(
//                databaseName: "PWInfoStrade-Telemetric",
//                collectionName: "SensorData",
//                ConnectionStringSetting = "ConnectionStringCosmos")]out dynamic outputCosmos,
//            ILogger log)
//        {
//            log.LogInformation("#####################################################");

//            log.LogInformation($"Queue Trigger from iothubtostreamsensori with this data: {myQueueItem}");

//            SensorData sensor = (SensorData)JsonConvert.DeserializeObject(myQueueItem, typeof(SensorData));

//            outputCosmos = new
//            {
//                id = Guid.NewGuid().ToString(),
//                Description = sensor.Description,
//                idGateway = sensor.idGateway,
//                idIncrocio = sensor.idIncrocio,
//                CrossRoad = sensor.CrossRoad,
//                Interserction = sensor.Interserction,
//                Location = sensor.Location,
//                Healthy = sensor.Healthy,
//                Date = sensor.Data.Date,
//                Time = sensor.Data.Time,
//                Temperature = sensor.Data.Temperature,
//                Humidity = sensor.Data.Humidity,
//                Pressure = sensor.Data.Pressure,
//                Coordinate_x = sensor.SensorLocation.coordinates.x,
//                Coordinate_y = sensor.SensorLocation.coordinates.y
//            };

//            log.LogInformation($"Json data are processing and store to CosmosDB with this format: \n {outputCosmos}");

//            log.LogInformation("#####################################################");

//        }
//    }
//}

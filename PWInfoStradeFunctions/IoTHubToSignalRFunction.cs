//using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Host;
//using Microsoft.Azure.EventHubs;
//using System.Text;
//using System.Net.Http;
//using Microsoft.Extensions.Logging;
//using Microsoft.Azure.WebJobs.Extensions.SignalRService;
//using Newtonsoft.Json;
//using System.Text.Json;
//using System.Collections.Generic;

//namespace PWInfoStradeFunctions
//{
//    public static class IoTHubToSignalRFunction
//    {
//        private static HttpClient client = new HttpClient();

//        [FunctionName("IoTHubToSignalRFunction")]
//        public static async System.Threading.Tasks.Task RunAsync(
//            [IoTHubTrigger("messages/events", Connection = "IoTHubEndpoint", ConsumerGroup = "signalr")] EventData message,
//            [SignalR(HubName = "signalrwebapp")] IAsyncCollector<SignalRMessage> signalRMessages,
//            ILogger log)
//        {
//            var deviceData = JsonConvert.DeserializeObject<DeviceData>(Encoding.UTF8.GetString(message.Body.Array));

//            log.LogInformation("#####################################################");
//            log.LogInformation($"DATI VEICOLI PER SIGNALR : {Encoding.UTF8.GetString(message.Body.Array)}");

//            log.LogInformation($"Il Messaggio Json proveniente dal Gateway: {deviceData.idGateway} " +
//                $"con riferimento all'incrocio: {deviceData.idIncrocio} " +
//                $"e al semaforo: {deviceData.idSemaforo} " +
//                $"ha come valore dello stato del semaforo: {deviceData.StatoSemaforo} " +
//                $"in data: {deviceData.Data}");
//            log.LogInformation("#####################################################");

//            await signalRMessages.AddAsync(
//            new SignalRMessage
//            {
//                Target = "iotMessage",
//                Arguments = new[] { JsonConvert.SerializeObject(deviceData) }
//            })
//            .ConfigureAwait(false);
//        }
//    }
//}
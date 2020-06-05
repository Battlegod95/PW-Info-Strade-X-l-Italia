using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace PW_Info_Strade_X_l_Italia___Azure_Function
{
    public static class SignalR
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SignalR")]
        public static async Task Run(
        [IoTHubTrigger("messages/events", Connection = "IoTHubEndpoint", ConsumerGroup = "signalr")] EventData message,
        [SignalR(HubName = "broadcast")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            var deviceData = JsonConvert.DeserializeObject<DeviceData>(Encoding.UTF8.GetString(message.Body.Array));
            deviceData.DeviceId = Convert.ToString(message.SystemProperties["iothub-connection-device-id"]);


            log.LogInformation($"C# IoT Hub trigger function processed a message: {JsonConvert.SerializeObject(deviceData)}");
            await signalRMessages.AddAsync(new SignalRMessage()
            {
                Target = "notify",
                Arguments = new[] { JsonConvert.SerializeObject(deviceData) }
            });
        }
    }
}
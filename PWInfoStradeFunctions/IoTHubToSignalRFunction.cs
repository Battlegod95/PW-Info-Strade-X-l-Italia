using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;

namespace PWInfoStradeFunctions
{
    public static class IoTHubToSignalRFunction
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("IoTHubToSignalRFunction")]
        public static async System.Threading.Tasks.Task RunAsync(
            [IoTHubTrigger("messages/events", Connection = "IoTHubEndpoint")]EventData message,
            [SignalR(HubName = "signalrwebapp")] IAsyncCollector<SignalRMessage> signalRMessages, 
            ILogger log)
        {
            var deviceData = JsonConvert.DeserializeObject<DeviceData>(Encoding.UTF8.GetString(message.Body.Array));

            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");

            log.LogInformation($"device Id: {deviceData.DeviceId}");
            log.LogInformation($"message Id: {deviceData.MessageId}");
            log.LogInformation($"Temperature: {deviceData.Temperature}");

            await signalRMessages.AddAsync(
            new SignalRMessage
            {
                Target = "iotMessage",
                Arguments = new[] { JsonConvert.SerializeObject(deviceData) }
            })
            .ConfigureAwait(false);
        }
    }
}
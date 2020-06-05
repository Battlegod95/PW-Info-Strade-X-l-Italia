using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PW_Info_Strade_X_l_Italia___Azure_Function
{
    class DeviceData
    {
        [JsonProperty("messageId")]
        public int MessageId { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("temperature")]
        public float Temperature { get; set; }

        [JsonProperty("humidity")]
        public float Humidity { get; set; }

        [JsonProperty("pressure")]
        public float pressure { get; set; }

        [JsonProperty("pointInfo")]
        public string PointInfo { get; set; }

        [JsonProperty("ioTHub")]
        public string IoTHub { get; set; }

        [JsonProperty("eventEnqueuedUtcTime")]
        public DateTime EventEnqueuedUtcTime { get; set; }

        [JsonProperty("eventProcessedUtcTime")]
        public DateTime EventProcessedUtcTime { get; set; }

        [JsonProperty("partitionId")]
        public string PartitionId { get; set; }
    }
}

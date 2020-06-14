using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWInfoStradeFunctions
{
    public class DeviceData
    {
        [JsonProperty("messageId")]
        public int MessageId { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("temperature")]
        public float Temperature { get; set; }

        [JsonProperty("humidity")]
        public float Humidity { get; set; }

    }
}

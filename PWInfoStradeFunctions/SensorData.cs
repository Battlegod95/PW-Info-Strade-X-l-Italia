using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWInfoStradeFunctions
{
    public class Data
    {

        [JsonProperty("Date")]
        public DateTime DateMessaggio { get; set; }

        [JsonProperty("Time")]
        public TimeSpan Time { get; set; }

        [JsonProperty("Temperature")]
        public int Temperature { get; set; }

        [JsonProperty("Humidity")]
        public int Humidity { get; set; }

        [JsonProperty("Pressure")]
        public int Pressure { get; set; }
    }


    public class Coordinates
    {

        [JsonProperty("x")]
        public int x { get; set; }

        [JsonProperty("y")]
        public int y { get; set; }
    }


    public class SensorLocation
    {

        [JsonProperty("coordinates")]
        public Coordinates coordinates { get; set; }
    }

    public class SensorData
    {

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("idGateway")]
        public string idGateway { get; set; }

        [JsonProperty("idIncrocio")]
        public int idIncrocio { get; set; }

        [JsonProperty("CrossRoad")]
        public string CrossRoad { get; set; }

        [JsonProperty("Interserction")]
        public string Interserction { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }

        [JsonProperty("Healthy")]
        public int Healthy { get; set; }

        [JsonProperty("Data")]
        public Data Data { get; set; }

        [JsonProperty("SensorLocation")]
        public SensorLocation SensorLocation { get; set; }
    }
}

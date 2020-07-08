using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp_Infostrade.Models
{
    public class TipologiaVeicolo
    {

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("value")]
        public int value { get; set; }
    }

    public class DeviceVeicoli
    {
        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("idIncrocio")]
        public int idIncrocio { get; set; }

        [JsonProperty("idGateway")]
        public string idGateway { get; set; }

        [JsonProperty("idSemaforo")]
        public int idSemaforo { get; set; }

        [JsonProperty("idStrada")]
        public int idStrada { get; set; }

        [JsonProperty("StatoSemaforo")]
        public int StatoSemaforo { get; set; }

        [JsonProperty("FasciaOraria")]
        public int FasciaOraria { get; set; }

        [JsonProperty("Data")]
        public string Data { get; set; }

        [JsonProperty("TipologiaVeicolo")]
        public IList<TipologiaVeicolo> TipologiaVeicolo { get; set; }
    }
}
/*
 public class TipologiaVeicolo
    {

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("value")]
        public int value { get; set; }
    }

    public class DeviceData
    {

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("idIncrocio")]
        public int idIncrocio { get; set; }

        [JsonProperty("idGateway")]
        public string idGateway { get; set; }

        [JsonProperty("idSemaforo")]
        public int idSemaforo { get; set; }

        [JsonProperty("idStrada")]
        public int idStrada { get; set; }

        [JsonProperty("StatoSemaforo")]
        public int StatoSemaforo { get; set; }

        [JsonProperty("FasciaOraria")]
        public int FasciaOraria { get; set; }

        [JsonProperty("Data")]
        public string Data { get; set; }

        [JsonProperty("TipologiaVeicolo")]
        public IList<TipologiaVeicolo> TipologiaVeicolo { get; set; }*/

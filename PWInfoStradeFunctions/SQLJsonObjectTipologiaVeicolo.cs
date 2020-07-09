using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWInfoStradeFunctions
{
    public class SQLJsonObjectTipologiaVeicolo
    {

        [JsonProperty("Id_incrocio")]
        public int Id_incrocio { get; set; }

        [JsonProperty("Id_semaforo")]
        public int Id_semaforo { get; set; }

        [JsonProperty("Id_strada")]
        public int Id_strada { get; set; }

        [JsonProperty("Fascia_oraria")]
        public int Fascia_oraria { get; set; }

        [JsonProperty("Data")]
        public string Data { get; set; }

        [JsonProperty("Tipologia_veicolo")]
        public string Tipologia_veicolo { get; set; }

        [JsonProperty("Conteggio")]
        public int Conteggio { get; set; }
    }
}

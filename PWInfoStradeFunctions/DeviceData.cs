using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWInfoStradeFunctions
{
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
        public IList<TipologiaVeicolo> TipologiaVeicolo { get; set; }
    }

    /*
    public class TipologiaVeicolo
    {

        [JsonProperty("Auto")]
        public int Auto { get; set; }

        [JsonProperty("Motociclo")]
        public int Motociclo { get; set; }

        [JsonProperty("Camion")]
        public int Camion { get; set; }
    }

    public class DeviceData
    {

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("idGateway")]
        public string idGateway { get; set; }

        [JsonProperty("idIncrocio")]
        public int idIncrocio { get; set; }

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
        public TipologiaVeicolo TipologiaVeicolo { get; set; }
    }
    */


    /*
     
     {
			"Description": "Veicoli",
			"idIncrocio": 1,
			"idGateway": "slot00",
			"idSemaforo": 2,
			"idStrada": 3,
			"StatoSemaforo": 1,
			"FasciaOraria": 15,
			"Data": "22/06/2020",
			"TipologiaVeicolo": [
				{
					"type": "Automobile",
					"value": 10
				},
				{
					"type": "Motociclo",
					"value": 7
				},
				{
					"type": "Camion",
					"value": 3
				}
			]

		};

      */

}

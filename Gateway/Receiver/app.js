var Client = require('azure-iot-device').Client;
var Protocol = require('azure-iot-device-mqtt').Mqtt;

var connectionString = 'HostName=IoTHubTrafficLight.azure-devices.net;DeviceId=4;SharedAccessKey=gg/l6hhNaNqQgJaVYHSWa97ElZDCH64/MOiW5fHf7Z4=';

var client_iothub = Client.fromConnectionString(connectionString, Protocol);

const redis = require("redis");
let redisNotReady = true;
let client = redis.createClient({
	host: '127.0.0.1',
	port: 6379
});

client.on("error", (err) => {
	console.log("error", err)
});

client.on("connect", (err) => {
	console.log("connect to Redis");
});

client.on("ready", (err) => {
	redisNotReady = false;

});

const SerialPort = require('serialport')
const ByteLength = require('@serialport/parser-byte-length');
const { compileFunction } = require("vm");
const port = new SerialPort('COM8', { baudRate: 38400 });
//const port = new SerialPort('/dev/ttyS0');

//variabili json Sensori
var temperature = 0;
var humidity = 0;
var pressure = 0;

//variabili json Veicoli
var colore = 0;
var trafficLight = 0;
var nAutomezzi = 0;
var nCiclomotori = 0;
var nCamion = 0;

//con questo parser lavoriamo con 6 byte alla volta
const parser = port.pipe(new ByteLength({ length: 6 }))
parser.on('data', parseMsg)

const slotGateway = 'slot00';

function parseMsg(data) {
	console.log(data);

	//per ogni elemento del buffer ricevuto via seriale, estraiamo il valore in binario
	let msgSize = data.length;

	//creiamo due variabili dove inserire il balore binario dei due byte
	let byte0 = parseInt(data[0], 10).toString(2).padStart(8, '0');
	let byte1 = parseInt(data[1], 10).toString(2).padStart(8, '0');
	let byte2 = parseInt(data[2], 10).toString(2).padStart(8, '0');
	let byte3 = parseInt(data[3], 10).toString(2).padStart(8, '0');
	let byte4 = parseInt(data[4], 10).toString(2).padStart(8, '0');
	let byte5 = parseInt(data[5], 10);

	console.log("byte 1 ", byte0); //Byte di controllo del tipo di trasmissione (usati i primi 2 bit e 6 vuoti)
	console.log("byte 2 ", byte1); //Destinatario
	console.log("byte 3 ", byte2); //Mittente
	console.log("byte 4 ", byte3); //id della strada
	console.log("byte 5 ", byte4); //Tipo di dato
	console.log("byte 6 ", byte5); // valore del dato

	let comunicazione = byte0.substring(6); //i primi due bit servono ad identificare il tipo di comunicazione
	let destinatario = byte1.substring(); //8 bit per identificare il destinatario del messaggio
	let mittente = byte2.substring(); //8 bit per identificare il mittente del messaggio
	let idStrada = byte3.substring(); // 8 bit per l'identificativo della strada
	let tipoDato = byte4.substring(); //8 bit per determinare il tipo di dato che si riceve
	let valoreDato = byte5; //8 bit per determinare il valore del tipo di dato ricevuto


	//gestione data e ora
	var time = new Date();
	var gg = time.getDate();
	var mm = time.getMonth();
	var yyyy = time.getFullYear();
	var hour = time.getHours();
	var min = time.getMinutes();


	//condizione per identificare che json va riempito

	if (comunicazione == 01) {
		switch (tipoDato) {
			case "00000000":
				temperature = valoreDato;
				break;
			case "00000001":
				humidity = valoreDato;
				break;
			case "00000010":
				pressure = valoreDato;
				break;
		};

		//definire i json da inviare
		let json = {

			"Desciption": "Sensori",
			"Location": "Pordenone",
			"CrossRoad:": "Via Prasecco",
			"Interserction": "Via Andrea Mantegna",
			"idGateway": slotGateway,
			"idIncrocio": mittente,


			"Data": {
				"Date": time,
				"Temperature": 0,
				"Humidity": 0,
				"Pressure": 0,

			},
			"SensorLocation": {
				"coordinates": {
					"x": -5,
					"y": 96,
				}
			}
		};

		if (temperature != 0) {
			json.Data.Temperature = temperature;
		}
		if (humidity != 0) {
			json.Data.Humidity = humidity;
		}
		if (pressure != 0) {
			json.Data.Pressure = pressure;
		}

		//controllo completamento json
		if (json.Data.Temperature != 0 && json.Data.Humidity != 0 && json.Data.Pressure != 0) {

			console.log(json);

			/* 			//push dei dati nella coda di redis
						client.on("ready", (err) => {
							redisNotReady = false;
						});
			
						client.rpush("IotData", JSON.stringify(json)); */

			client.llen("IotData", function (err, data) {
				console.log("Lunghezza della lista: " + data);
			});

			//resetto il json
			pressure = 0;
			temperature = 0;
			humidity = 0;

			json = 0;


			/*elimina l'elemento in coda e restituisce l'elemento eliminato
			client.lpop("IotData", function (err, data) {
				console.log(data);
			});*/


		}

	}

	else if (comunicazione == 10) {
		switch (tipoDato) {
			case "00000011":
				if (valoreDato == 00000000) {
					colore = 1;
				}
				else if (valoreDato == 00000001) {
					colore = 2;
				}
				else {
					colore = 3;
				}
				trafficLight = colore;
				break;
			case "00000100":
				nAutomezzi = valoreDato;
				break;
			case "00000101":
				nCiclomotori = valoreDato;
				break;
			case "00000110":
				nCamion = valoreDato;
				break;
		}

		//definire i json da inviare



		valori = [];



		if (nAutomezzi != 0) {
			var obj = {
				type: "Automobile",  //car, heavy, moto
				value: nAutomezzi
			}
			valori.push(obj);
		}
		else {
			var obj = {
				type: "Automobile",  //car, heavy, moto
				value: 0
			}
			valori.push(obj);
		}

		if (nCiclomotori != 0) {
			var obj = {
				type: "Motociclo",  //car, heavy, moto
				value: nCiclomotori
			}
			valori.push(obj);
		}
		else {
			var obj = {
				type: "Motociclo",  //car, heavy, moto
				value: 0
			}
			valori.push(obj);
		}

		if (nCamion != 0) {
			var obj = {
				type: "Camion",  //car, heavy, moto
				value: nCamion
			}
			valori.push(obj);
		}
		else {
			var obj = {
				type: "Camion",  //car, heavy, moto
				value: 0
			}
			valori.push(obj);
		}


		let json2 = {
			"Description": "Veicoli",
			"idIncrocio": 1,
			"idGateway": slotGateway,
			"idSemaforo": mittente,
			"idStrada": idStrada,
			"StatoSemaforo": 0,
			"FasciaOraria": hour,
			"Data": time,
			"TipologiaVeicolo": valori

		};

		if (trafficLight != 0) {
			json2.StatoSemaforo = trafficLight;
		}

		var completato = true;

		json2.TipologiaVeicolo.forEach(element => {

			if (element.value == 0) {
				completato = false;
			}

		});

		//controllo completamento json
		if (json2.StatoSemaforo != 0 && completato) {

			console.log(json2);

			//push dei dati nella coda di redis
			client.on("ready", (err) => {
				redisNotReady = false;
			});

			/*client.rpush("IotData", JSON.stringify(json2)); */

			client.llen("IotData", function (err, data) {
				console.log("Lunghezza della lista: " + data);
			});

			//resetto il json
			colore = 0;
			trafficLight = 0;
			nAutomezzi = 0;
			nCiclomotori = 0;
			nCamion = 0;

			json2 = 0;


			/*elimina l'elemento in coda e restituisce l'elemento eliminato
			client.lpop("IotData", function (err, data) {
				console.log(data);
			});*/


		}

	}
	else {
		console.log("errore");
	}

}

client_iothub.open(function (err) {
	if (err) {
		console.error('error connecting to hub: ' + err);
		process.exit(1);
	}
	console.log('client opened');
	// Create device Twin
	client_iothub.getTwin(function (err, twin) {
		if (err) {
			console.error('error getting twin: ' + err);
			process.exit(1);
		}
		console.log('twin contents:');
		twin.on('properties.desired', function (delta) {
			console.log('new desired properties received:');
			console.log(JSON.stringify(delta.Temporizzazione_verde));

			temporizzazione = delta.Temporizzazione_verde.toString(16);

			console.log(temporizzazione);
			if (temporizzazione < 0) {
				temporizzazione = 0;
			}

			if (temporizzazione.length < 2) {
				port.write('La temporizzazione per il semaforo è' + '0' + temporizzazione, 'hex');
			}
			else {
				port.write(temporizzazione.toString(16), 'hex'); //invio dati al pic
			}
		});
	});
});

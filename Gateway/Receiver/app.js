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
const ByteLength = require('@serialport/parser-byte-length')
const port = new SerialPort('/dev/ttyS0');

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
	let byte5 = parseInt(data[4], 10).toString(2).padStart(8, '0');

	console.log("byte 1 ", byte0); //Byte di controllo del tipo di trasmissione (usati i primi 2 bit e 6 vuoti)
	console.log("byte 2 ", byte1); //Destinatario
	console.log("byte 3 ", byte2); //Mittente
	console.log("byte 4 ", byte3); //id della strada
	console.log("byte 5 ", byte4); //Tipo di dato
	console.log("byte 6 ", byte5); // valore del dato

	let comunicazione = byte0.substring(0,2); //i primi due bit servono ad identificare il tipo di comunicazione
	let destinatario = byte1.substring(); //8 bit per identificare il destinatario del messaggio
	let mittente = byte2.substring(); //8 bit per identificare il mittente del messaggio
	let idStrada = byte3.substring(); // 8 bit per l'identificativo della strada
	let tipoDato = byte4.substring(); //8 bit per determinare il tipo di dato che si riceve
	let valoreDato = byte5.substring(); //8 bit per determinare il valore del tipo di dato ricevuto


	//gestione data e ora
	var time = new Date();
	var gg = time.getDate();
	var mm = time.getMonth();
	var yyyy = time.getFullYear();
	var hour = time.getHour();
	var min = time.getMinutes();
	
	//condizione per identificare che json va riempito

	if (comunicazione == 01) {
		switch (tipoDato) {
			case 00000000:
				temperature = valoreDato;
				break;
			case 00000001:
				humidity: valoreDato;
				break;
			case 00000010:
				pressure = valoreDato;
				break;
		};

		//definire i json da inviare
		let json = {}

		json = {

			"Desciption": "Sensori",
			"Location": "Pordenone",
			"CrossRoad:": "Via Prasecco",
			"Interserction": "Via Andrea Mantegna",
			"idGateway": slotGateway,
			"idIncrocio": mittente,


			"Data": {
				"Date": (gg + '/' + (mm + 1) + '/' + yyyy),
				"Time": (hour + ':' + min),
				"Temperature": temperature + "°C",
				"Humidity": humidity + "%",
				"Pressure": pressure,

			},
			"SensorLocation": {
				"coordinates": {
					"x": -4,
					"y": 3,
				}
			}
		};

		//push dei dati nella coda di redis
		client.on("ready", (err) => {
			redisNotReady = false;
			client.rpush("dati", json);

			client.llen("dati", function (err, data) {
				console.log("Lunghezza della lista: " + data);
			});

			//elimina l'elemento in coda e restituisce l'elemento eliminato
			client.lpop("dati", function (err, data) {
				console.log(data);
			});

		});

	}
	else if (comunicazione == 10)
	{

		switch (tipoDato) {
			case 00000011:
				trafficLight = valoreDato;
				break;
			case 00000100:
				nCiclomotori = valoreDato;
				break;
			case 00000101:
				nAutomezzi = valoreDato;
				break;
			case 00000110:
				nCamion = valoreDato;
				break;
		}

		//definire i json da inviare
		let json2 = {}

		json2 = {
			"Description": "Veicoli",
			"idIncrocio": 1,
			"idGateway": slotGateway,
			"idSemaforo": mittente,
			"idStrada": idStrada,
			"StatoSemaforo": trafficLight,
			"FasciaOraria": (hour + ':' + min),
			"Data": (gg + '/' + (mm + 1) + '/' + yyyy),
			"TipologiaVeicolo": [
				{
					"type": "Automobile",  //car, heavy, moto
					"value": nAutomezzi
				},
				{
					"type": "Motociclo",  //car, heavy, moto
					"value": nCiclomotori
				},
				{
					"type": "Camion",  //car, heavy, moto
					"value": nCamion
				}
			]

		};

		console.log(data);

		//push dei dati nella coda di redis
		client.on("ready", (err) => {
			redisNotReady = false;
			client.rpush("dati", json2);

			client.llen("dati", function (err, data) {
				console.log("Lunghezza della lista: " + data);
			});

			//elimina l'elemento in coda e restituisce l'elemento eliminato
			client.lpop("dati", function (err, data) {
				console.log(data);
			});

		});

	}
	else
	{
		console.log("errore");
	}

}



		
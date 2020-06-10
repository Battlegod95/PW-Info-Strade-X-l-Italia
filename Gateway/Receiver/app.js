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
	console.log("connect");
});

client.on("ready", (err) => {
	redisNotReady = false;

});

const SerialPort = require('serialport')
const ByteLength = require('@serialport/parser-byte-length')

const port = new SerialPort('/dev/ttyS0');

//con questo parser lavoriamo con 2 byte alla volta
const parser = port.pipe(new ByteLength({ length: 2 }))
parser.on('data', parseMsg)

const slotGateway = 'slot00';

function parseMsg(data) {
	//console.log(data);

	//per ogni elemento del buffer ricevuto via seriale, estraiamo il valore in binario
	let msgSize = data.length;

	//creiamo due variabili dove inserire il balore binario dei due byte
	let byte0 = parseInt(data[0], 10).toString(2).padStart(8, '0');
	//let byte1 = parseInt(data[1], 10).toString(2).padStart(8, '0');
	//let byte2 = parseInt(data[2], 10).toString(2).padStart(8, '0');

	console.log("byte 1 ", byte0); //gateway + id sensore
	//console.log("byte 2 ", byte1); //id incrocio
	//console.log("byte 3 ", byte2); //valore

	//ipotizziamo il protocollo indicato nell'esercitazione del  prof. Bortolani
	let gateway = byte0.substring(0, 4); //primi 4 bit del byte : COMANDO
	let sensore = byte0.substring(4);   //ultimi 4 bit del byte : ID Sensore / Attuatore

	var id = data[1];
	var valore = data[2];
	let json = {};

	//definire i dati di andata 
	if (gateway == slotGateway) {

		json = {
			"id_incrocio": id,
			"Sensore": sensore,
			"Valore": valore
		};

	}

	console.log(json);

	//push dei dati nella coda di redis
	client.rpush("dati", json.toString());

	client.llen("dati", function (err, data) {
		console.log("Lunghezza della lista: " + data);
	});

	client.lpop("dati", function (err, data) {
		console.log(data);
	});

}
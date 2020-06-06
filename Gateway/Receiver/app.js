const SerialPort = require('serialport')
const ByteLength = require('@serialport/parser-byte-length')

const port = new SerialPort('/dev/ttyS0');

//con questo parser lavoriamo con 2 byte alla volta
const parser = port.pipe(new ByteLength({ length: 2 }))
parser.on('data', parseMsg)

function parseMsg(data) {
	console.log(data);

	//per ogni elemento del buffer ricevuto via seriale, estraiamo il valore in binario
	let msgSize = data.length;

	//creiamo due variabili dove inserire il balore binario dei due byte
	let byte0 = parseInt(data[0], 10).toString(2).padStart(8, '0');
	let byte1 = parseInt(data[1], 10).toString(2).padStart(8, '0');
	let byte2 = parseInt(data[2], 10).toString(2).padStart(8, '0');

	console.log("byte 1 ", byte0); //incrocio
	console.log("byte 2 ", byte1); //valore
	console.log("byte 3 ", byte2); //gateway + id sensore

	//ipotizziamo il protocollo indicato nell'esercitazione del  prof. Bortolani
	let gateway = byte2.substring(0, 4); //primi 4 bit del byte : COMANDO
	let sensore = byte2.substring(4);   //ultimi 4 bit del byte : ID Sensore / Attuatore

	console.log(gateway);

}
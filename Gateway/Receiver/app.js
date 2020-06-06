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
	for (let i = 0; i < msgSize; i++) {
		let valore = parseInt(data[i], 10).toString(2);
		//aggiungiamo gli 0 omessi perchè non significanti
		console.log(valore.padStart(8, '0'));
	}

	//creiamo due variabili dove inserire il balore binario dei due byte
	let byte0 = parseInt(data[0], 10).toString(2).padStart(8, '0');
	let byte1 = parseInt(data[1], 10).toString(2).padStart(8, '0');

	//ipotizziamo il protocollo indicato nell'esercitazione del  prof. Bortolani
	let comando = byte0.substring(0, 4); //primi 4 bit del byte : COMANDO
	let sensore = byte0.substring(4);   //ultimi 4 bit del byte : ID Sensore / Attuatore

	//tabella dei comandi
	switch (comando) {
		case "0000":
			console.log("Scrivi dato");
			break;
		case "0001":
			console.log("Leggi dato");
			break;
		case "0101":
			console.log("Conferma dato");
			break;
		case "0110":
			console.log("Vis. Menu");
			break;
		case "1100":
			console.log("Risposta");
			break;

	}

	//etc etc


}
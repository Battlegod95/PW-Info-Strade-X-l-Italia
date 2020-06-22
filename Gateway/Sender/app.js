const RedisSMQ = require("../index");
const rsmq = new RedisSMQ({ host: "127.0.0.1", port: 6379, ns: "rsmq" });
var uuid = require('uuid');
var Protocol = require('azure-iot-device-mqtt').Mqtt;
var Client = require('azure-iot-device').Client;
var Message = require('azure-iot-device').Message;

/*
======================================
Make a simple queue and send/receive messages
======================================
*/

function main() {
	const queuename = "dati";

		receiveMessageLoop(queuename);

}
main();

function receiveMessageLoop(queuename) {
	// check for new messages every 2.5 seconds
	setInterval(() => {
		// alternative to receiveMessage would be popMessage => receives the next message from the queue and deletes it.
		rsmq.receiveMessage({ qname: queuename }, (err, resp) => {
			if (err) {
				console.error(err);
				return;
			}

			// checks if a message has been received
			if (resp.id) {
				console.log("received message:", resp.message);

				// we are done with working on our message, we can now safely delete it
				rsmq.deleteMessage({ qname: queuename, id: resp.id }, (err) => {
					if (err) {
						console.error(err);
						return;
					}

					console.log("deleted message with id", resp.id);
				});
			} else {
				console.log("no available message in queue..");
			}
		});
	}, 2500);
}

var deviceConnectionString = "HostName=IoTHubTrafficLight.azure-devices.net;DeviceId=GatewayTestTrafficLight;SharedAccessKey=rZFdx7r32CVnmdUNQkPwJ6QNoVY3EDvr2nSxnpZOrBM="

var client = Client.fromConnectionString(deviceConnectionString, Protocol);

// open connection
client.open(function (err) {
	if (err) {
		console.error('Could not connect: ' + err.message);
	}
	else {
		console.log('Client connected');

		client.on('error', function (err) {
			console.error(err.message);
			process.exit(-1);
		});

		// var data= new Date();
		// var ora= data.getHours.toString()+":"+data.getMinutes.toString()+":"+data.getSeconds.toString();
		// // build message
		var message = new Message(JSON.stringify(
			{
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
			}));

		message.contentEncoding = "utf-8";
		message.contentType = "application/json";

		// A unique identifier 
		message.messageId = uuid.v4();

		//add custom properties
		message.properties.add("Status", "Active");


		console.log('Sending message: ' + message.getData());
		client.sendEvent(message, function (err) {
			if (err) {
				console.error('Could not send: ' + err.toString());
				process.exit(-1);
			} else {
				console.log('Message sent: ');
				process.exit(0);
			}
		});
	}

});


// METODO PER L'INVIO DEI MESSAGGI ALLA CODA DI REDIS DALL'IOT HUB
/*
function sendMessageLoop(queuename) {
	// push a message every 2 seconds into the queue
	setInterval(() => {
		// send the messages with a random delay between 0-5 seconds
		rsmq.sendMessage({ qname: queuename, message: `Hello World at ${new Date().toISOString()}`, delay: Math.floor(Math.random() * 6) }, (err) => {
			if (err) {
				console.error(err);
				return;
			}

			console.log("pushed new message into queue..");
		});
	}, 2000);
}
*/
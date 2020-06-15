const RedisSMQ = require("../index");
const rsmq = new RedisSMQ( {host: "127.0.0.1", port: 6379, ns: "rsmq"} );

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
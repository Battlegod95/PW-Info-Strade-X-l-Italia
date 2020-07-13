const apiBaseUrl = 'https://functionappinfostrade.azurewebsites.net';

const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${apiBaseUrl}/api/SignalRConnection`)
    .configureLogging(signalR.LogLevel.Information)
    .build();

console.log('connecting...');

//bindConnectionMessage(connection);
//connection.start()
//    .then(function () {
//        onConnected(connection);
//    })
//    .catch(function (error) {
//        console.error(error.message);
//    });

connection.start()
    .then((response) => {
        console.log('connection established', response);
    })
    .catch(logError);

//connection.on("iotMessage", iotMessage);

//function logError(err) {
//    console.error('Error establishing connection', err);
//}
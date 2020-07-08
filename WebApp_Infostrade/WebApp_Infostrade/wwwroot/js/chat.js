function bindConnectionMessage(connection) {
    var messageCallback = function (name, message) {
        if (!message) return;
        alert("message received:" + message);
    };
    connection.on("ReceiveMessage", function (message) {
    var msg = JSON.parse(message);
    console.log(msg);
});

    });

var connection = new signalR.HubConnectionBuilder()
    .withUrl('/signalrwebapp')
    .build();

bindConnectionMessage(connection);
connection.start()
    .then(function () {
        onConnected(connection);
    })
    .catch(function (error) {
        console.error(error.message);
    });

/*
"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalrwebapp")
    .build();

connection.on("ReceiveMessage", function (message) {
    var msg = JSON.parse(message);
    console.log(msg);
});* /
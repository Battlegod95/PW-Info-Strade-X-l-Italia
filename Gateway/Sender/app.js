'use strict';


var Protocol = require('azure-iot-device-mqtt').Mqtt;
var Client = require('azure-iot-device').Client;
var Message = require('azure-iot-device').Message;

var deviceConnectionString = "HostName=IoTHubTrafficLight.azure-devices.net;DeviceId=4;SharedAccessKey=gg/l6hhNaNqQgJaVYHSWa97ElZDCH64/MOiW5fHf7Z4="

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

        clientRedis.brpop(['IotData', 0], function (listName, item) {
            if (item[1] != null) {
                console.log(item[1]);
                var message = new Message(item[1]);
                message.contentEncoding = "utf-8";
                message.contentType = "application/json";

                //add custom properties
                message.properties.add("Status", "Active");

                console.log('Sending message: ' + message.getData());
                client.sendEvent(message, function (err) {
                    if (err) {
                        console.error('Could not send: ' + err.toString());
                        process.exit(-1);
                    }
                    else {
                        console.log('Message sent: success');
                        process.exit(0);
                    }
                });
            }
        });

       /* var data = new Date();
        var ora = data.getHours();
        // build message
        var message = new Message(JSON.stringify(

            {
              "Description": "Sensori",
              "idGateway": "PROVA8",
              "idIncrocio": 5,
              "CrossRoad": "Via Prasecco",
              "Interserction": "Via Andrea Mantegna",
              "Location": "Pordenone",
              "Healthy": 1,
              "Data": {
                "Date": "2020-06-23",
                "Time": "11:34:54.1237",
                "Temperature": 50,
                "Humidity": 790,
                "Pressure": 990
              },
              "SensorLocation": {
                "coordinates": {
                  "x": -25,
                  "y": 96
                }
              }
            }


            {
                "Description": "Veicoli",
                "idGateway": "slot00",
                "idIncrocio": 1,
                "idSemaforo": 2,
                "idStrada": 3,
                "StatoSemaforo": 1,
                "FasciaOraria": ora,
                "Data": data,
                "TipologiaVeicolo": {
                    "Automobile": 20,
                    "Motociclo": 5,
                    "Camion": 2
                }
            }

        ));*/

        
    }

});
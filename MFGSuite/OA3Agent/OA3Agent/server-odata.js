﻿var odata = require('node-odata');
var io = require('socket.io-client');
var config = require("nodejs-config")(__dirname);

var url = config.get("app.mongodb-url");//"mongodb://localhost:27017/mfgcloud";
var resourceName = config.get("app.mongodb-collection-name");//"engineering";
var resourceModel = config.get("app.mongo-odata-resource-model");//{ uid: String, transactionId: String, type: String, value: Number, time: Number, systemInfo: Object, smbInfo: Object, monitorInfo: Object, oa3Report: Object, oa3ReportTrace: Object, oa3HwDecode: Object, validationResult: Object };
var servicePortNumber = config.get("app.http-server-port"); //3001;

var socketHost = config.get("app.web-socket-server-host");//"127.0.0.1";
var socketPort = config.get("app.web-socket-server-port");//3000;

var server = odata(url);

var socket = null;

//server.resource(resourceName, resourceModel)
//    .post().after(function (originEntity, newEntity) {
//        console.log(JSON.stringify(originEntity));
//        console.log(JSON.stringify(newEntity));

//        socket.emit('msg:newmsg', originEntity);
//    });

//server.listen(servicePortNumber, function () {
//    console.log("Node-OData Service is running, listening on port \"" + servicePortNumber + "\"...");

//    socket = io.connect("ws://" + socketHost + ":" + socketPort.toString());

//    socket.on('connect', function () {
//        console.log('Connected to socket.io server.');
//    });
//});

var cluster = require('cluster');
var os = require('os');
var cpuCount = os.cpus().length;

if (cluster.isMaster) {

    console.log(`Master ${process.pid} is running`);

    console.log(`Number of CPUs: ${cpuCount}.`);

    for (var i = 0; i < cpuCount; i++) {
        cluster.fork();
    }

    cluster.on('exit', (worker, code, signal) => {
        console.log(`worker ${worker.process.pid} died`);
    });
}
else {

    server.resource(resourceName, resourceModel)
        .post().after(function (originEntity, newEntity) {
            console.log(JSON.stringify(originEntity));
            console.log(JSON.stringify(newEntity));

            socket.emit('msg:newmsg', originEntity);
        });

    server.listen(servicePortNumber, function () {
        console.log("Node-OData Service is running, listening on port \"" + servicePortNumber + "\"...");

        socket = io.connect("ws://" + socketHost + ":" + socketPort.toString());

        socket.on('connect', function () {
            console.log('Connected to socket.io server.');
        });
    });

    console.log(`Worker ${process.pid} started`);
}
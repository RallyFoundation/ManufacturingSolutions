﻿var chokidar = require("chokidar");
var config = require("nodejs-config")(__dirname);
var fs = require("fs");
var csv = require("fast-csv");

var logRepository = config.get("app.log-repository");
var logKeywords = config.get("app.log-keywords");
var webSocketServerHost = config.get("app.web-socket-server-host");
var webSocketServerPort = config.get("app.web-socket-server-port");

var io = require("socket.io-client");

var socket = io.connect(("http://" + webSocketServerHost + ":" + webSocketServerPort), { reconnect: true });

socket.on("connect", function (socket) {
    console.log("Connected!");
});

//var express = require('express');
//var app = express();
//var http = require('http').Server(app);
//var io = require('socket.io')(http);

//http.listen(webSocketServerPort, function () {
//    console.log("Web Socket server is running, listening on port \"" + webSocketServerPort + "\"...");
//});


var logKeywordRegPattern = new RegExp(logKeywords, "g");

// Initialize watcher.
var watcher = chokidar.watch(logRepository, 'file, dir, glob, or array', {
    ignored: /(^|[\/\\])\../,
    persistent: true,
    cwd: logRepository
});

// Something to use when events are received.
var log = console.log.bind(console);

var parseLog = function (path) {

    //log(`File ${path} has been added`);
    //log(`File ${path} has been changed`);

    var stream = fs.createReadStream(path);

    csv.fromStream(stream, { ignoreEmpty: true })
        .on("data", function (data) {
            if (logKeywordRegPattern.test(data[1])) {
                console.log(data);
                console.log(path);
                //io.emit("msg:logm", path);
                socket.emit("msg:logm", path);
            }
            //console.log(data);
        })
        .on("end", function () {
            console.log("done");
        });
};

// Add event listeners.
watcher
    //.on('add', path => parseLog(path))
    .on('change', path => parseLog(path))
    //.on('unlink', path => log(`File ${path} has been removed`));


//watcher.close();
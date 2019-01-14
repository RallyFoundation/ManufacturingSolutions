var express = require('express');
var bodyParser = require('body-parser');
var fs = require("fs");
var readDir = require("readdir");
var shell = require('node-powershell');
var redis = require("redis");
var cors = require("cors");
var config = require("nodejs-config")(__dirname);
var assert = require('assert');
//var bearerToken = require('express-bearer-token');
//var async = require("async");
var path = require("path");
var uuidv1 = require("uuid/v1");
var childProcess = require("child_process");

var cluster = require('cluster');
var os = require('os');
var cpuCount = os.cpus().length;

var app = express();

app.use(bodyParser.json()); // for parsing application/json
app.use(bodyParser.urlencoded({ extended: true })); // for parsing application/x-www-form-urlencoded

//for cross domain access
//app.all('*', function (req, res, next) {
//    res.header("Access-Control-Allow-Origin", "*");
//    res.header("Access-Control-Allow-Headers", "X-Requested-With,Authorization,Content-Type");
//    res.header("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS");
//    res.header("Content-Type", "application/json;charset=utf-8");
//    next();
//});

app.use(cors());

//app.use(bearerToken());
//app.use(function (req, res) {
//    res.send('Token ' + req.token);
//});


var enableCpuClustering = config.get("app.enable-cpu-clustering");
var hqaDataZipRepository = config.get("app.hqa-data-zip-repository");
var hqaHome = config.get("app.hqa-home");
var hqaEntry = config.get("app.hqa-entry");
var httpServerPort = config.get("app.http-server-port"); //8089;
var webSocketServerPort = config.get("app.web-socket-server-port");
var redisAddress = config.get("app.redis-address"); //"127.0.0.1";
var redisPort = config.get("app.redis-port"); //6379;
var redisPassword = config.get("app.redis-password"); //"P@ssword1";

var http = require('http').Server(app);
var io = require('socket.io')(http);
var server;

if (enableCpuClustering == true) {
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

        server = app.listen(httpServerPort, function () {
            var host = server.address().address;
            var port = server.address().port;
            console.log("HQA Microservice listening at http://%s:%s", host, port);
        });

        http.listen(webSocketServerPort, function () {
            console.log("Web Socket server is running, listening on port \"" + webSocketServerPort + "\"...");
        });

        console.log(`Worker ${process.pid} started`);
    }
}
else {
    server = app.listen(httpServerPort, function () {
        var host = server.address().address;
        var port = server.address().port;
        console.log("HQA Microservice listening at http://%s:%s", host, port);
    });

    http.listen(webSocketServerPort, function () {
        console.log("Web Socket server is running, listening on port \"" + webSocketServerPort + "\"...");
    });
}

io.on("connection", function (socket) {
    console.log("A new app connected!");

    socket.on("msg:newdata", function (data) {
        //persistData(data);
        //io.emit("msg:msgrly", data);
        //io.emit("msg:resp", "OK! " + Date.now().toString());
    });

    socket.on("msg:newmsg", function (data) {
        //persistData(data);
        io.emit("msg:msgrly", data);
    });

    socket.on("msg:logm", function (data) {
        //persistData(data);
        console.log(data);
        io.emit("msg:logmfr", data);
    });

    //socket.on("msg:cltstat", function (data) {

    //    var ipAddress = socket.handshake.address;
    //    var clientStatus = { ip: ipAddress, status: data };

    //    updateClientStatus(status);
    //});
});

function getRedisClient() {
    var client = redis.createClient(redisPort, redisAddress);
    client.auth(redisPassword);

    return client;
}

var redisClient = getRedisClient();

redisClient.subscribe("pubsub");
redisClient.on("message", function (channel, message) {
    console.log(channel + ": " + message);

    var reportFileDir = message.ReportFileDir;
    var transactionId = message.TransID;

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    var hqaOfflineScript = hqaEntry;

    ps.addCommand(hqaOfflineScript, [("ReportFileDir " + reportFileDir), ("BatchID " + transactionId), ("RootDir " + hqaHome)]);
    ps.invoke().then(output => {
        console.log(output);
    }).catch(err => {
        console.log(err);
        ps.dispose();
    });

    //readDir.read(reportFileDir, ["**.xml"], readDir.ABSOLUTE_PATHS + readDir.CASELESS_SORT, function (err, reportXmlFiles) {
    //    if (err) { res.end(err.message); }
    //    else {

    //        console.log(reportXmlFiles);

    //        for (var i = 0; i < reportXmlFiles.length; i++) {
    //            ps.addCommand(hqaOfflineScript, [("ReportFilePath " + reportXmlFiles[i]), ("TransactionID " + transactionId + "_" + i), ("RootDir " + hqaHome), ("ByPassUI $true"), ("StayInHost $true"), ("OutResult ([ref]$outResult)")]);
    //            ps.invoke()
    //                .then(output => {
    //                    console.log(output);

    //                    ps.addCommand('$outResult');
    //                    ps.invoke()
    //                        .then(output => {
    //                            console.log(output);
    //                            res.end(output);
    //                        })
    //                        .catch(err => {
    //                            console.log(err);
    //                            ps.dispose();
    //                        });
    //                })
    //                .catch(err => {
    //                    console.log(err);
    //                    ps.dispose();
    //                });
    //        }
    //    }
    //});
});

app.post("/hqa/offline/batch/", function (req, res) {

    var reportFileDir = req.body.ReportFileDir;
    var transactionId = req.body.TransID;

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    var hqaOfflineScript = hqaEntry;

    //childProcess.execFile("PowerShell.exe", ["-ExecutionPolicy ByPass", "-NoExit", ("-File \"" + hqaOfflineScript + "\""), ("-ReportFileDir " + reportFileDir), ("-BatchID " + transactionId), ("-RootDir " + hqaHome)], { detached: true, stdio: ['ignore', 'ignore', 'ignore'] });

    //childProcess.exec("PowerShell.exe", ["-ExecutionPolicy ByPass", ("-File " + hqaOfflineScript), ("-ReportFileDir " + reportFileDir), ("-BatchID " + transactionId), ("-RootDir " + hqaHome)], (error, stdout, stderr) => {
    //    if (error) {
    //        console.log(error);
    //    }

    //    if (stderr) {
    //        console.log(stderr);
    //    }

    //    console.log(stdout);
    //});


    ps.addCommand(hqaOfflineScript, [("RootDir \"" + hqaHome + "\""), ("ReportFileDir \"" + reportFileDir + "\""), ("BatchID \"" + transactionId + "\"")]);
    ps.invoke().then(output => {
            console.log(output);
        }).catch(err => {
            console.log(err);
            ps.dispose();
        });

    //readDir.read(reportFileDir, ["**.xml"], readDir.ABSOLUTE_PATHS + readDir.CASELESS_SORT, function (err, reportXmlFiles) {
    //    if (err) { res.end(err.message); }
    //    else {

    //        console.log(reportXmlFiles);

    //        //ps.addCommand("$outResult");
    //        //ps.invoke().then(output => {
    //        //    console.log(output);
    //        //}).catch(err => {
    //        //    console.log(err);
    //        //    ps.dispose();
    //        //});

    //        //for (var i = 0; i < reportXmlFiles.length; i++) {
    //        //    console.log(reportXmlFiles[i]);

    //        //    //ps.addCommand(hqaOfflineScript, [("ReportFilePath " + reportXmlFiles[i]), ("TransactionID " + transactionId + "_" + i), ("RootDir " + hqaHome), ("ByPassUI $true"), ("StayInHost $true"), ("OutResult ([ref]$outResult)")]);

    //        //    ps.addCommand(hqaOfflineScript, [("ReportFilePath " + reportXmlFiles[i]), ("TransactionID " + transactionId + "_" + i), ("RootDir " + hqaHome), ("ByPassUI $true"), ("StayInHost $true")]);

    //        //    ps.invoke()
    //        //        .then(output => {
    //        //            console.log(output);

    //        //            ps.addCommand("$outResult");
    //        //            ps.invoke()
    //        //                .then(output => {
    //        //                    console.log(output);
    //        //                    res.end(output);
    //        //                })
    //        //                .catch(err => {
    //        //                    console.log(err);
    //        //                    ps.dispose();
    //        //                });
    //        //        })
    //        //        .catch(err => {
    //        //            console.log(err);
    //        //            ps.dispose();
    //        //        });
    //        //}
    //    }
    //});
});
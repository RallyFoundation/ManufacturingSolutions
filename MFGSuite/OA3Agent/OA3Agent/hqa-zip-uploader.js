var express = require('express');
var bodyParser = require('body-parser');
var fs = require("fs");
var multer = require('multer');
var cors = require("cors");
var config = require("nodejs-config")(__dirname);
var assert = require('assert');
//var bearerToken = require('express-bearer-token');
//var async = require("async");
var path = require("path");
var uuidv1 = require("uuid/v1");

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

app.use(express.static(__dirname));
app.use(express.static(__dirname + "/views"));
app.use(express.static(__dirname + "/public"));

var enableCpuClustering = config.get("app.enable-cpu-clustering");
var hqaDataZipRepository = config.get("app.hqa-data-zip-repository");
var httpServerPort = config.get("app.http-server-port"); //8089;
var webSocketServerPort = config.get("app.web-socket-server-port");

var hqaDataZipUploader = multer({
    storage: multer.diskStorage({
        destination: function (req, file, cb) {
            cb(null, hqaDataZipRepository);
        },
        filename: function (req, file, cb) {
            var transId = uuidv1();
            cb(null, transId + "_" + file.originalname);
        }
    })
}).any();

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

app.post("/hqa/offline/zip/upload/", function (req, res) {

    hqaDataZipUploader(req, res, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }

        console.log(req.files[0].path);

        res.end();
    });
});

app.get("/hqa/offline/", function (req, res) {

    res.sendFile(path.join(__dirname + "/upload-hqa.html"));
});
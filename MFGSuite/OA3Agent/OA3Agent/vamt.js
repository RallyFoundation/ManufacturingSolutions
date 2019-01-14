var express = require('express');
var bodyParser = require('body-parser');
var shell = require('node-powershell');
var cors = require("cors");
var config = require("nodejs-config")(__dirname);
var assert = require('assert');

var cluster = require('cluster');
var os = require('os');
var cpuCount = os.cpus().length;

var app = express();

app.use(bodyParser.json()); 
app.use(bodyParser.urlencoded({ extended: true })); 

app.use(cors());

var enableCpuClustering = config.get("app.enable-cpu-clustering");
var httpServerPort = config.get("app.http-server-port"); //8089;
var vamtEntry = config.get("app.vamt-entry");
var vamtModulePath = config.get("app.vamt-module-path");

var http = require('http').Server(app);
var server;

if (enableCpuClustering === true) {
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
            console.log("VAMT.js RESTful API listening at http://%s:%s", host, port);
        });

        console.log(`Worker ${process.pid} started`);
    }
}
else {
    server = app.listen(httpServerPort, function () {
        var host = server.address().address;
        var port = server.address().port;
        console.log("VAMT.js RESTful API listening at http://%s:%s", host, port);
    });
}


app.get('/', function (req, res) {
    res.send('Welcome to VAMT!');
});

app.post("/vamt/register/", function (req, res) {

    var data = req.body;

    console.log(data);

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    var vamtScript = vamtEntry;

    ps.addCommand(vamtScript, [("ClientHostName " + data.Host), ("ClientUserName " + data.User), ("ClientPassword " + data.Password), ("VamtPSModulePath \"" + vamtModulePath + "\"")]);
    ps.invoke().then(output => {
        console.log(output);
    }).catch(err => {
        console.log(err);
        ps.dispose();
    });
});

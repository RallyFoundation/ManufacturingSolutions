var express = require('express');
var bodyParser = require('body-parser');
var shell = require('node-powershell');
var cors = require("cors");
var config = require("nodejs-config")(__dirname);
var assert = require('assert');
var uuidv1 = require("uuid/v1");
var fs = require("fs");

var cluster = require('cluster');
var os = require('os');
var cpuCount = os.cpus().length;

var app = express();

app.use(bodyParser.json()); 
app.use(bodyParser.urlencoded({ extended: true })); 

app.use(cors());

var enableCpuClustering = config.get("app.enable-cpu-clustering");
var httpServerPort = config.get("app.http-server-port"); //8089;
var webSocketServerHost = config.get("app.web-socket-server-host");
var webSocketServerPort = config.get("app.web-socket-server-port");
var vamtEntry = config.get("app.vamt-entry");
var vamtModulePath = config.get("app.vamt-module-path");
var logRepository = config.get("app.log-repository");

var http = require('http').Server(app);
var io = require('socket.io')(http);
var server;

var ioClient = require("socket.io-client");
var socketClient = ioClient.connect(("http://" + webSocketServerHost + ":" + webSocketServerPort), { reconnect: true });

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
        console.log("VAMT.js RESTful API listening at http://%s:%s", host, port);
    });

    http.listen(webSocketServerPort, function () {
        console.log("Web Socket server is running, listening on port \"" + webSocketServerPort + "\"...");
    });
}

io.on("connection", function (socket) {
    console.log("A new client connected!");

    socket.on("msg:newreg", function (data) {

        console.log(data);

        persistData(data);
        //io.emit("msg:msgrly", data);
        //io.emit("msg:resp", "OK! " + Date.now().toString());

        let ps = new shell({
            executionPolicy: 'Bypass',
            noProfile: true
        });

        var vamtScript = vamtEntry;

        ps.addCommand(vamtScript, [("ClientHostName " + data.Host), ("ClientUserName " + data.User), ("ClientPassword " + data.Password), ("VamtPSModulePath \"" + vamtModulePath + "\"")]);
        ps.invoke().then(output => {
            console.log(output);
            updateData(data, JSON.stringify(output));
            //io.emit("msg:resp", output);
            persistData(data);
            io.emit("msg:resp", data);
        }).catch(err => {
            console.log(err);
            ps.dispose();
        });
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
});

function persistData(data)
{
    var dirName = data.Value;
    var logFullPath = logRepository + "\\" + dirName;
    var fileFullPath = logFullPath + "\\" + data.TransID + ".json";

    if (!fs.existsSync(logFullPath)) {
        fs.mkdirSync(logFullPath);
    }

    fs.writeFileSync(fileFullPath, JSON.stringify(data), { encoding: "utf-8", flag: "w+"});
}

function updateData(data, verbose)
{
    var transactionId = uuidv1();

    data.RefID = data.TransID;
    data.TransID = transactionId;

    if (verbose != null) {
        data.Data = verbose;
    }

    data.Time = Date.now();
}


app.get('/', function (req, res) {
    res.send('Welcome to VAMT!');
});

app.post("/vamt/register/", function (req, res) {

    var data = req.body;

    console.log(data);

    persistData(data);

    //var transactionId = uuidv1();

    //data.RefID = data.TransID;
    //data.TransID = transactionId;
    //data.Time = Date.now();

    updateData(data);

    socketClient.emit("msg:newreg", data);

    res.end(JSON.stringify(data));
});

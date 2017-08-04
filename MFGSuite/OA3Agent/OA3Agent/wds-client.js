var express = require('express');
var bodyParser = require('body-parser');
var wmi = require('node-wmi');
var redis = require("redis");
var readDir = require("readdir");
var cors = require("cors");
var config = require("nodejs-config")(__dirname);

var app = express();
app.use(cors());
app.use(bodyParser.json()); // for parsing application/json
app.use(bodyParser.urlencoded({ extended: true })); // for parsing application/x-www-form-urlencoded

var http = require('http').Server(app);
var io = require('socket.io')(http);

var httpclient = require("http");

const execFile = require('child_process').execFile;

var httpServerPort = config.get("app.http-server-port"); //8089;
var httpServerAddress = config.get("app.image-server-address");

var redisAddress = config.get("app.redis-address"); //"127.0.0.1";
var redisPort = config.get("app.redis-port"); //6379;
var redisPassword = config.get("app.redis-password"); //"P@ssword1";
var redisDbIndex = config.get("app.redis-db-index"); //0;

var imageServerHost = "minint-et2evvt";
var imageServrePort = 8089;
var imageServerUserName = "Administrator";
var imageServerPassword = "W@lcome!";

var biosData = { SystemSKUNumber: "", Model: "" };

function getRedisClient() {
    var client = redis.createClient(redisPort, redisAddress);
    client.auth(redisPassword);

    return client;
}

function persistData(data) {
}

function readBIOSData(data)
{
    wmi.Query({
        class: 'Win32_ComputerSystem'
    }, function (err, sysInfo) {
        console.log(sysInfo[0].SystemSKUNumber);
        console.log(JSON.stringify(sysInfo[0]));
        data.SystemSKUNumber = sysInfo[0].SystemSKUNumber;
        data.Model = sysInfo[0].Model;

        console.log(JSON.stringify(data));

        data.SystemSKUNumber = "Model1";

        getImageUrl(data.SystemSKUNumber);
    });
}

function getImageUrl(imageId)
{
    var imageUrl;

    var options = {
        host: imageServerHost,
        port: imageServrePort,
        path: "/wds/lookup/" + imageId,
        method: "GET",
        headers: {
            'Content-Type': 'application/json'
        }
    };

    var result = "";

    callback = function (response) {

        response.on('data', function (chunk) {
            result += chunk;
            console.log(result);
        });

        response.on('end', function () {
            console.log(result);
            imageUrl = result;
            console.log(imageUrl);
        });
    }

    var req = httpclient.request(options, callback).end();

    // These just return undefined and empty
    //console.log(req.data);
    //console.log(imageUrl);
}

function downloadImageWithWDSClient(imageNamespace)
{
    var wdsImageSource = imageNamespace.Substring((imageNamespace.IndexOf("/") + 1));
    wdsImageSource = wdsImageSource.Substring(0, wdsImageSource.LastIndexOf("/"));

    var path = "wdsmcast.exe";
    var params = ["/progress", "/verbose", "/trace:wds_trace.etl", "/Apply-Image", ("/Server:" + imageServerHost), ("/Namespace:" + imageNamespace), ("/Username:" + imageServerHost + "\\" + imageServerUserName), (" / Password:" + imageServerPassword), ("/SourceFile:" + wdsImageSource), "/Index:1", "/DestinationPath:W:\\"];

    execFile(path, params, (error, stdout, stderr) => {
        if (error) {
            throw error;
        }
        console.log(stdout);
    });
}

var server = app.listen(httpServerPort, function () {
    var host = server.address().address;
    var port = server.address().port;

    readBIOSData(biosData);

    console.log("WDS client listening at http://%s:%s", host, port);

    console.log(JSON.stringify(biosData));
})

app.get('/', function (req, res) {
    res.send('Welcome!');
});

io.on("connection", function (socket) {
    console.log("A new device connected!");

    socket.on("msg:newdata", function (data) {
        persistData(data);
        //io.emit("msg:msgrly", data);
        //io.emit("msg:resp", "OK! " + Date.now().toString());
    });

    socket.on("msg:newmsg", function (data) {
        //persistData(data);
        io.emit("msg:msgrly", data);
    });
});
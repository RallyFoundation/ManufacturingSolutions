var chokidar = require("chokidar");
var config = require("nodejs-config")(__dirname);
var fs = require("fs");
const StreamZip = require('node-stream-zip');
var redis = require("redis");
var io = require("socket.io-client");
var uuidv1 = require("uuid/v1");

var hqaDataZipRepository = config.get("app.hqa-data-zip-repository");
var hqaDataRepository = config.get("app.hqa-data-repository");
var webSocketServerHost = config.get("app.web-socket-server-host");
var webSocketServerPort = config.get("app.web-socket-server-port");
var redisAddress = config.get("app.redis-address"); //"127.0.0.1";
var redisPort = config.get("app.redis-port"); //6379;
var redisPassword = config.get("app.redis-password"); //"P@ssword1";
var redisDbIndexHQAData = config.get("app.redis-db-index-hqa-data"); //2;

var socket = io.connect(("http://" + webSocketServerHost + ":" + webSocketServerPort), { reconnect: true });

socket.on("connect", function (socket) {
    console.log("Connected!");
});

function getRedisClient() {
    var client = redis.createClient(redisPort, redisAddress);
    client.auth(redisPassword);

    return client;
}

// Initialize watcher.
var watcher = chokidar.watch(hqaDataZipRepository, 'file, dir, glob, or array', {
    ignored: /(^|[\/\\])\../,
    persistent: true,
    cwd: hqaDataZipRepository
});

// Something to use when events are received.
var log = console.log.bind(console);

var extractNewZip = function (path) {

    console.log(path);

    const zip = new StreamZip({
        file: path,
        storeEntries: true
    });

    zip.on('ready', () => {

        var dirName = path.substring(path.lastIndexOf("\\"));
        var extractedDirFullPath = hqaDataRepository + dirName;

        fs.mkdirSync(extractedDirFullPath);

        zip.extract(null, extractedDirFullPath, (err, count) => {
            console.log(err ? 'Extract error' : `Extracted ${count} entries`);
            zip.close();
        });

        var transactionId = uuidv1();
        var message = { TransID: transactionId, ReportFileDir: extractedDirFullPath };

        var redisClient = getRedisClient();

        redisClient.select(redisDbIndexHQAData, function (err) {
            if (err) {
                console.log(err);
                res.end(err);
            }
            else {
                redisClient.set(transactionId, JSON.stringify(message), function (err, result) {
                    if (err) {
                        console.log(err);
                    }
                    else {
                        console.log(result);
                        redisClient.end(true);
                    }
                });
            }
        });
    })
};

// Add event listeners.
    watcher.on('add', path => extractNewZip(path));
    //.on('change', path => parseLog(path))
    //.on('unlink', path => log(`File ${path} has been removed`));


//watcher.close();
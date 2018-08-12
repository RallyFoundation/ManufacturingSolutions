var chokidar = require("chokidar");
var config = require("nodejs-config")(__dirname);
var fs = require("fs");
const streamZip = require('node-stream-zip');

var hqaDataZipRepository = config.get("app.hqa-data-zip-repository");
var webSocketServerHost = config.get("app.web-socket-server-host");
var webSocketServerPort = config.get("app.web-socket-server-port");

var io = require("socket.io-client");

var socket = io.connect(("http://" + webSocketServerHost + ":" + webSocketServerPort), { reconnect: true });

socket.on("connect", function (socket) {
    console.log("Connected!");
});


var logKeywordRegPattern = new RegExp(logKeywords, "g");

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
        fs.mkdirSync('extracted');
        zip.extract(null, './extracted', (err, count) => {
            console.log(err ? 'Extract error' : `Extracted ${count} entries`);
            zip.close();
        });
    });
};

// Add event listeners.
watcher
    .on('add', path => extractNewZip(path))
    //.on('change', path => parseLog(path))
    //.on('unlink', path => log(`File ${path} has been removed`));


//watcher.close();

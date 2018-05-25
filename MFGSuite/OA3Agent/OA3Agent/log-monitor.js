var chokidar = require('chokidar');
var config = require("nodejs-config")(__dirname);
var fs = require("fs");
var csv = require("fast-csv");

var logRepository = config.get("app.log-repository");


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
            console.log(data);
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
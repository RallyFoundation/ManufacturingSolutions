var app = require('express')();
var bodyParser = require('body-parser');
//var http = require('http').Server(app);
//var io = require('socket.io')(http);
var redis = require("redis");

app.use(bodyParser.json()); // for parsing application/json
app.use(bodyParser.urlencoded({ extended: true })); // for parsing application/x-www-form-urlencoded

var redisAddress = "127.0.0.1";
var redisPort = 6379;
var redisPassword = "P@ssword1";
var redisDbIndex = 0;

var httpServerPort = 8087;

var cluster = require('cluster');
var os = require('os');
var cpuCount = os.cpus().length;

function getRedisClient() {
    var client = redis.createClient(redisPort, redisAddress);
    client.auth(redisPassword);

    return client;
}

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

    var server = app.listen(httpServerPort, function () {
        var host = server.address().address
        var port = server.address().port
        console.log("Redis RESTful API service listening at http://%s:%s", host, port)
    })

    console.log(`Worker ${process.pid} started`);
}

//var server = app.listen(httpServerPort, function () {
//    var host = server.address().address
//    var port = server.address().port
//    console.log("Redis RESTful API service listening at http://%s:%s", host, port)
//})

app.get('/', function (req, res) {
    res.send('Hello!');
});

app.get('/redis/:key', function (req, res)
{
    var redisClient = getRedisClient();
    var key = req.params.key;

    redisClient.select(redisDbIndex, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }
        else {
            redisClient.get(key, function (err, result) {
                if (err) {
                    console.log(err);
                    res.end(err);
                }
                else {
                    console.log(result);
                    redisClient.end(true);
                    res.end(result);
                }
            });
        }
    });
});

app.post('/redis/:key', function (req, res)
{
    var redisClient = getRedisClient();
    var key = req.params.key;
    var data = req.body;

    redisClient.select(redisDbIndex, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }
        else {
            redisClient.set(key, data.value, function (err, result) {
                if (err) {
                    console.log(err);
                    res.end(err);
                }
                else {
                    console.log(result);
                    redisClient.end(true);
                    res.end(result);
                }
            });
        }
    });
});

app.delete('/redis/:key', function (req, res) {
    var redisClient = getRedisClient();
    var key = req.params.key;

    redisClient.select(redisDbIndex, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }
        else {
            redisClient.del(key, function (err, result) {
                if (err) {
                    console.log(err);
                    res.end(err);
                }
                else {
                    var resultString = String(result);
                    console.log(resultString);
                    redisClient.end(true);
                    res.end(resultString);
                }
            });
        }
    });
});
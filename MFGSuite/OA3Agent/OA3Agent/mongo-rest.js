var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var bodyParser = require('body-parser');
var mongoClient = require('mongodb').MongoClient;
var assert = require('assert');

var portNumber = 3000;
var mongoUrl = "mongodb://localhost:27017/mfgcloud";
var mongoCollectionName = "engineering";

app.use(bodyParser.json()); // for parsing application/json
app.use(bodyParser.urlencoded({ extended: true })); // for parsing application/x-www-form-urlencoded

app.get('/', function (req, res) {
    res.send('Hello!');
});

app.post("/engineering/", function (req, res) {
    var data = req.body;
    persistData(data);
    res.send(data.id);
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

http.listen(portNumber, function () {
    console.log("Service is running, listening on port \"" + portNumber + "\"...");
});

var insertDocuments = function (db, data, callback) {
    // Get the documents collection
    var collection = db.collection(mongoCollectionName);
    // Insert some documents
    collection.insertOne(
        //{ a : 1 }, { a : 2 }, { a : 3 }
        data,
        function (err, result) {
            assert.equal(err, null);
            assert.equal(1, result.result.n);
            assert.equal(1, result.ops.length);
            console.log("OK!");
            callback(result);
        });
}

function persistData(data) {
    mongoClient.connect(mongoUrl, function (err, db) {
        assert.equal(null, err);
        console.log("Connected correctly to server");

        insertDocuments(db, data, function (res) {
            db.close();

            console.log(res);

            data.id = res.insertedId;
            console.log(JSON.stringify(data));

            io.emit("msg:msgrly", data);
        });
    });
}
var express = require('express');
var bodyParser = require('body-parser');
var fs = require("fs");
var readDir = require("readdir");
var multer = require('multer');
var shell = require('node-powershell');
var redis = require("redis");
var cors = require("cors");
var config = require("nodejs-config")(__dirname);
var mongoClient = require('mongodb').MongoClient;
var assert = require('assert');
//var bearerToken = require('express-bearer-token');
//var async = require("async");

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

var httpServerPort = config.get("app.http-server-port"); //8089;
var webSocketServerPort = config.get("app.web-socket-server-port");
var redisAddress = config.get("app.redis-address"); //"127.0.0.1";
var redisPort = config.get("app.redis-port"); //6379;
var redisPassword = config.get("app.redis-password"); //"P@ssword1";
var redisDbIndex = config.get("app.redis-db-index"); //0;
var redisDbIndexClientStatus = config.get("app.redis-db-index-client-status"); //0;
var mongoDBUrl = config.get("app.mongodb-url");
var mongoDBCollectionName = config.get("app.mongodb-collection-name");
var installImageRepository = config.get("app.install-image-repository");
var bootImageRepository = config.get("app.boot-image-repository");
var ffuImageRepository = config.get("app.ffu-image-repository");
var logRepository = config.get("app.log-repository");

var installImageUploader = multer({
    storage: multer.diskStorage({
        destination: function (req, file, cb) {

            //fs.stat(installImageRepository, function (err, stat) {
            //    if ((err) && (err.errno == -4058) && (err.code == "ENOENT"))
            //    {
            //        fs.mkdirSync(installImageRepository);
            //    }
            //});

            cb(null, installImageRepository);
        },
        filename: function (req, file, cb) {
            cb(null, Date.now().toString() + "_" + file.originalname);
        }
    })
}).any();

var bootImageUploader = multer({
    storage: multer.diskStorage({
        destination: function (req, file, cb) {
            cb(null, bootImageRepository);
        },
        filename: function (req, file, cb) {
            cb(null, Date.now().toString() + "_" + file.originalname);
        }
    })
}).any();

var ffuImageUploader = multer({
    storage: multer.diskStorage({
        destination: function (req, file, cb) {
            cb(null, ffuImageRepository);
        },
        filename: function (req, file, cb) {
            cb(null, Date.now().toString() + "_" + file.originalname);
        }
    })
}).any();

var logUploader = multer({
    storage: multer.diskStorage({
        destination: function (req, file, cb) {
            cb(null, logRepository);
        },
        filename: function (req, file, cb) {
            cb(null, req.params.trsnid + "_" + file.originalname);
        }
    })
}).any();

var server = app.listen(httpServerPort, function () {
    var host = server.address().address;
    var port = server.address().port;
    console.log("WDS RESTful API service listening at http://%s:%s", host, port);
})

var http = require('http').Server(app);
var io = require('socket.io')(http);

http.listen(webSocketServerPort, function () {
    console.log("Web Socket server is running, listening on port \"" + webSocketServerPort + "\"...");
});

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

var insertDocuments = function (db, data, callback) {
    var collection = db.collection(mongoDBCollectionName);
    collection.insertOne(
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
    mongoClient.connect(mongoDBUrl, function (err, db) {
        assert.equal(null, err);
        console.log("Connected to MongoDB.");

        insertDocuments(db, data, function (res) {
            db.close();

            console.log(res);

            data.id = res.insertedId;
            console.log(JSON.stringify(data));

            //io.emit("msg:msgrly", data);
        });
    });
}

//function updateClientStatus(data)
//{
//    var redisClient = getRedisClient();

//    redisClient.select(redisDbIndexClientStatus, function (err) {
//        if (err) {
//            console.log(err);
//        }
//        else
//        {
//            redisClient.set(data.ip, data.status, function (err, result)
//            {
//                if (err) {
//                    console.log(err);
//                }
//                else
//                {
//                    console.log(result);
//                    redisClient.end(true);
//                }
//            });
//        }
//    });
//}


app.get('/', function (req, res) {
    res.send('Welcome to WDS!');
});

app.get('/wds/lookup/:key', function (req, res) {
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

app.get('/wds/lookup/keys/all', function (req, res) {
    var redisClient = getRedisClient();

    redisClient.select(redisDbIndex, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }
        else {
            redisClient.keys('*', function (err, keys) {
                if (err) {
                    console.log(err);
                    res.end(err);
                }
                else
                {
                    console.log(JSON.stringify(keys));
                    redisClient.end(true);
                    res.end(JSON.stringify(keys));
                }
            });
        }
    });
});

app.post('/wds/lookup/', function (req, res) {
    var redisClient = getRedisClient();
    //var key = req.params.key;
    var data = req.body;

    redisClient.select(redisDbIndex, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }
        else {
            redisClient.set(data.Key, data.Value, function (err, result) {
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

app.delete('/wds/lookup/:key', function (req, res) {
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

app.get('/wds/terminal/status/:id', function (req, res) {
    var redisClient = getRedisClient();
    var key = req.params.id;

    redisClient.select(redisDbIndexClientStatus, function (err) {
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

app.get('/wds/terminal/status/keys/all', function (req, res) {
    var redisClient = getRedisClient();

    redisClient.select(redisDbIndexClientStatus, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }
        else {
            redisClient.keys('*', function (err, keys) {
                if (err) {
                    console.log(err);
                    res.end(err);
                }
                else {
                    console.log(JSON.stringify(keys));
                    redisClient.end(true);
                    res.end(JSON.stringify(keys));
                }
            });
        }
    });
});

app.post('/wds/terminal/status/', function (req, res) {
    var redisClient = getRedisClient();
    //var key = req.params.key;
    var data = req.body;

    persistData(data);

    redisClient.select(redisDbIndexClientStatus, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }
        else {
            if (!(data.Key)) {
                data.Key = req.ip.toString();
            }
            redisClient.set(data.Key, JSON.stringify(data), function (err, result) {
                if (err) {
                    console.log(err);
                    res.end(err);
                }
                else {
                    console.log(result);
                    redisClient.end(true);
                    res.end(result);

                    io.emit("msg:clientStat", data);
                }
            });
        }
    });
});

app.delete('/wds/terminal/status/:id', function (req, res) {
    var redisClient = getRedisClient();
    var key = req.params.id;

    redisClient.select(redisDbIndexClientStatus, function (err) {
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

app.get('/wds/image/boot/:name', function (req, res)
{
    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    var imageName = "";

    if (req.params.name)
    {
        imageName = req.params.name;
    }

    ps.addCommand('Import-Module', ['Name "WDS"']);
    ps.invoke()
        .then(output => {
            console.log(output);
            //res.end(output);
            if (imageName != '' && imageName.toLowerCase() != 'all') {
                ps.addCommand('$image = Get-WdsBootImage', ['Name "' + imageName + '"']);
            } else {
                ps.addCommand('$image = Get-WdsBootImage');
            }
            ps.invoke()
                .then(output => {
                    console.log(output);
                    //res.end(output);
                    ps.addCommand('$image | ConvertTo-Json')
                    ps.invoke()
                        .then(output => {
                            console.log(output);
                            res.end(output);
                        })
                        .catch(err => {
                            console.log(err);
                            ps.dispose();
                        });
                })
                .catch(err => {
                    console.log(err);
                    //ps.dispose();
                    res.end(err);
                });
        })
        .catch(err => {
            console.log(err);
            //ps.dispose();
            res.end(err);
        });
});

app.get('/wds/image/install/:name', function (req, res) {
    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    var imageName = "";

    if (req.params.name) {
        imageName = req.params.name;
    }

    ps.addCommand('Import-Module', ['Name "WDS"']);
    ps.invoke()
        .then(output => {
            console.log(output);
            //res.end(output);
            if (imageName != '' && imageName.toLowerCase() != 'all') {
                ps.addCommand('$image = Get-WdsInstallImage', ['Name "' + imageName + '"']);
            } else {
                ps.addCommand('$image = Get-WdsInstallImage');
            }
            ps.invoke()
                .then(output => {
                    console.log(output);
                    //res.end(output);
                    ps.addCommand('$image | ConvertTo-Json')
                    ps.invoke()
                        .then(output => {
                            console.log(output);
                            res.end(output);
                        })
                        .catch(err => {
                            console.log(err);
                            ps.dispose();
                        });
                })
                .catch(err => {
                    console.log(err);
                    //ps.dispose();
                    res.end(err);
                });
        })
        .catch(err => {
            console.log(err);
            //ps.dispose();
            res.end(err);
        });
});

app.get('/wds/imagegroup/install/:name', function (req, res) {
    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    var imageGroupName = "";

    if (req.params.name) {
        imageGroupName = req.params.name;
    }

    ps.addCommand('Import-Module', ['Name "WDS"']);
    ps.invoke()
        .then(output => {
            console.log(output);
            //res.end(output);
            if (imageGroupName != '' && imageGroupName.toLowerCase() != 'all') {
                ps.addCommand('$group = Get-WdsInstallImageGroup', ['Name "' + imageGroupName + '"']);
            } else {
                ps.addCommand('$group = Get-WdsInstallImageGroup');
            }
            ps.invoke()
                .then(output => {
                    console.log(output);
                    //res.end(output);
                    ps.addCommand('$group | ConvertTo-Json')
                    ps.invoke()
                        .then(output => {
                            console.log(output);
                            res.end(output);
                        })
                        .catch(err => {
                            console.log(err);
                            ps.dispose();
                        });
                })
                .catch(err => {
                    console.log(err);
                    //ps.dispose();
                    res.end(err);
                });
        })
        .catch(err => {
            console.log(err);
            //ps.dispose();
            res.end(err);
        });
});

app.get("/wds/imagefile/install", function (req, res) {

    readDir.read(installImageRepository, ["**.wim"], readDir.ABSOLUTE_PATHS + readDir.CASELESS_SORT, function (err, files)
    {
        if (err) { res.end(err.message); }
        else {
            res.end(JSON.stringify(files));
        }
    });
});

app.get("/wds/imagefile/boot", function (req, res) {

    readDir.read(bootImageRepository, ["**.wim"], readDir.ABSOLUTE_PATHS + readDir.CASELESS_SORT, function (err, files) {
        if (err) { res.end(err.message); }
        else {
            res.end(JSON.stringify(files));
        }
    });
});

app.get("/wds/imagefile/ffu", function (req, res) {

    readDir.read(ffuImageRepository, ["**.ffu"], readDir.ABSOLUTE_PATHS + readDir.CASELESS_SORT, function (err, files) {
        if (err) { res.end(err.message); }
        else {
            res.end(JSON.stringify(files));
        }
    });
});

app.get("/wds/imagefile/install/:fname", function (req, res) {
    var filePath = installImageRepository + "/" + req.params.fname;

    fs.stat(filePath, function (err, stats) {

        console.log(JSON.stringify(stats));

        if (err) {
            console.log(err);
            res.end(err);
        }

        res.setHeader("Content-Length", stats.size);

        var stream = fs.createReadStream(filePath);

        stream.pipe(res);

        stream.on("error", function (err) {
            res.statusCode = 500;
            res.end(err);
        });
    });
}); 

app.get("/wds/imagefile/boot/:fname", function (req, res) {
    var filePath = bootImageRepository + "/" + req.params.fname;

    fs.stat(filePath, function (err, stats) {

        console.log(JSON.stringify(stats));

        if (err) {
            console.log(err);
            res.end(err);
        }

        res.setHeader("Content-Length", stats.size);

        var stream = fs.createReadStream(filePath);

        stream.pipe(res);

        stream.on("error", function (err) {
            res.statusCode = 500;
            res.end(err);
        });
    });
}); 

app.get("/wds/imagefile/ffu/:fname", function (req, res) {
    var filePath = ffuImageRepository + "/" + req.params.fname;

    fs.stat(filePath, function (err, stats) {

        console.log(JSON.stringify(stats));

        if (err) {
            console.log(err);
            res.end(err);
        }

        res.setHeader("Content-Length", stats.size);

        var stream = fs.createReadStream(filePath);

        stream.pipe(res);

        stream.on("error", function (err) {
            res.statusCode = 500;
            res.end(err);
        });
    });
}); 

app.get("/wds/logfile/:fname", function (req, res) {
    var filePath = logRepository + "/" + req.params.fname;

    fs.stat(filePath, function (err, stats) {

        console.log(JSON.stringify(stats));

        if (err) {
            console.log(err);
            res.end(err);
        }

        res.setHeader("Content-Length", stats.size);

        var stream = fs.createReadStream(filePath);

        stream.pipe(res);

        stream.on("error", function (err) {
            res.statusCode = 500;
            res.end(err);
        });
    });
});

app.get("/wds/imagefile/info/install/:fname", function (req, res) {
    var filePath = installImageRepository + "/" + req.params.fname;

    fs.stat(filePath, function (err, stats) {

        var info = JSON.stringify(stats);

        console.log(info);

        res.end(info);

        if (err) {
            console.log(err);
            res.end(err);
        }
    });
}); 

app.get("/wds/imagefile/info/boot/:fname", function (req, res) {
    var filePath = bootImageRepository + "/" + req.params.fname;

    fs.stat(filePath, function (err, stats) {

        var info = JSON.stringify(stats);

        console.log(info);

        res.end(info);

        if (err) {
            console.log(err);
            res.end(err);
        }
    });
});

app.get("/wds/imagefile/info/ffu/:fname", function (req, res) {
    var filePath = ffuImageRepository + "/" + req.params.fname;

    fs.stat(filePath, function (err, stats) {

        var info = JSON.stringify(stats);

        console.log(info);

        res.end(info);

        if (err) {
            console.log(err);
            res.end(err);
        }
    });
});

app.post("/wds/imagefile/install/", function (req, res) {

    installImageUploader(req, res, function (err) {
        if (err){
            console.log(err);
            res.end(err);
        }

        console.log(req.files[0].path);

        //req.file = req.files[0];
        //var tmp_path = req.file.path;
        //console.log(tmp_path);

        //var target_path = installImageRepository + "/" + Date.now().toString() + "_" + req.file.originalname;

        //console.log(target_path);

        //if (!fs.existsSync(installImageRepository)) {
        //    fs.mkdirSync(installImageRepository);
        //}

        //var src = fs.createReadStream(tmp_path);
        //var dest = fs.createWriteStream(target_path);

        //src.pipe(dest);

        //src.on('end', function () {
        //    res.end();
        //});

        //src.on('error', function (err) {
        //    res.end();
        //    console.log(err);
        //});

    });
});

app.post("/wds/imagefile/boot/", function (req, res) {

    bootImageUploader(req, res, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }

        console.log(req.files[0].path);

        //req.file = req.files[0];
        //var tmp_path = req.file.path;
        //console.log(tmp_path);

        //var target_path = bootImageRepository + "/" + Date.now().toString() + "_" + req.file.originalname;

        //console.log(target_path);

        //if (!fs.existsSync(bootImageRepository)) {
        //    fs.mkdirSync(bootImageRepository);
        //}

        //var src = fs.createReadStream(tmp_path);
        //var dest = fs.createWriteStream(target_path);

        //src.pipe(dest);

        //src.on('end', function () {
        //    res.end();
        //});

        //src.on('error', function (err) {
        //    res.end();
        //    console.log(err);
        //});
    });
});

app.post("/wds/imagefile/ffu/", function (req, res) {

    ffuImageUploader(req, res, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }

        console.log(req.files[0].path);

        //req.file = req.files[0];
        //var tmp_path = req.file.path;
        //console.log(tmp_path);

        //var target_path = ffuImageRepository + "/" + Date.now().toString() + "_" + req.file.originalname;

        //console.log(target_path);

        //if (!fs.existsSync(ffuImageRepository)) {
        //    fs.mkdirSync(ffuImageRepository);
        //}

        //var src = fs.createReadStream(tmp_path);
        //var dest = fs.createWriteStream(target_path);

        //src.pipe(dest);

        //src.on('end', function () {
        //    res.end();
        //});

        //src.on('error', function (err) {
        //    res.end();
        //    console.log(err);
        //});

    });
});

app.post("/wds/logfile/:trsnid", function (req, res) {

    logUploader(req, res, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }

        console.log(req.params.trsnid);
        console.log(req.files[0].path);
    });
});

app.delete("/wds/imagefile/install/:fname", function (req, res) {

    var filePath = installImageRepository + "/" + req.params.fname;

    fs.stat(filePath, function (err, stats) {

        console.log(JSON.stringify(stats));

        if (err) {
            res.end(err.message); 
        }

        fs.unlink(filePath, function (err) {
            if (err) {
                res.end(err.message);
            }

            console.log("File\"" + filePath + "\"deleted successfully");

            res.end("OK");
        });
    });
});

app.delete("/wds/imagefile/boot/:fname", function (req, res) {

    var filePath = bootImageRepository + "/" + req.params.fname;

    fs.stat(filePath, function (err, stats) {

        console.log(JSON.stringify(stats));

        if (err) {
            res.end(err.message);
        }

        fs.unlink(filePath, function (err) {
            if (err) {
                res.end(err.message);
            }

            console.log("File\"" + filePath + "\"deleted successfully");

            res.end("OK");
        });
    });
});

app.delete("/wds/imagefile/ffu/:fname", function (req, res) {

    var filePath = ffuImageRepository + "/" + req.params.fname;

    fs.stat(filePath, function (err, stats) {

        console.log(JSON.stringify(stats));

        if (err) {
            res.end(err.message);
        }

        fs.unlink(filePath, function (err) {
            if (err) {
                res.end(err.message);
            }

            console.log("File\"" + filePath + "\"deleted successfully");

            res.end("OK");
        });
    });
});

app.patch("/wds/imagefile/install/rename/", function (req, res) {

    var data = req.body;

    var filePathOld = installImageRepository + "/" + data.OldName;

    var filePathNew = installImageRepository + "/" + data.NewName;

    fs.stat(filePathOld, function (err, stats) {

        console.log(JSON.stringify(stats));

        if (err) {
            res.end(err.message);
        }

        fs.stat(filePathNew, function (err, status)
        {
            if (stats)
            {
                console.log(JSON.stringify(stats));
            }

            if (err) {
                console.log(err);

                if ((err.errno == -4058) && (err.code == "ENOENT")) {
                    fs.rename(filePathOld, filePathNew, function (err) {
                        if (err) {
                            res.end(err.message);
                        }

                        console.log("File\"" + filePathOld + "\" successfully changed its name to \"" + filePathNew + "\".");

                        res.end("OK");
                    });
                }
                else
                {
                    res.end(err);
                }
            }
            else
            {
                res.end("File \"" + filePathNew + "\" already exists!");
            }     
        });    
    });
});

app.patch("/wds/imagefile/boot/rename/", function (req, res) {

    var data = req.body;

    var filePathOld = bootImageRepository + "/" + data.OldName;

    var filePathNew = bootImageRepository + "/" + data.NewName;

    fs.stat(filePathOld, function (err, stats) {

        console.log(JSON.stringify(stats));

        if (err) {
            res.end(err.message);
        }

        fs.stat(filePathNew, function (err, status) {
            if (stats) {
                console.log(JSON.stringify(stats));
            }

            if (err) {
                console.log(err);

                if ((err.errno == -4058) && (err.code == "ENOENT")) {
                    fs.rename(filePathOld, filePathNew, function (err) {
                        if (err) {
                            res.end(err.message);
                        }

                        console.log("File\"" + filePathOld + "\" successfully changed its name to \"" + filePathNew + "\".");

                        res.end("OK");
                    });
                }
                else {
                    res.end(err);
                }
            }
            else {
                res.end("File \"" + filePathNew + "\" already exists!");
            }
        });
    });
});

app.patch("/wds/imagefile/ffu/rename/", function (req, res) {

    var data = req.body;

    var filePathOld = ffuImageRepository + "/" + data.OldName;

    var filePathNew = ffuImageRepository + "/" + data.NewName;

    fs.stat(filePathOld, function (err, stats) {

        console.log(JSON.stringify(stats));

        if (err) {
            res.end(err.message);
        }

        fs.stat(filePathNew, function (err, status) {
            if (stats) {
                console.log(JSON.stringify(stats));
            }

            if (err) {
                console.log(err);

                if ((err.errno == -4058) && (err.code == "ENOENT")) {
                    fs.rename(filePathOld, filePathNew, function (err) {
                        if (err) {
                            res.end(err.message);
                        }

                        console.log("File\"" + filePathOld + "\" successfully changed its name to \"" + filePathNew + "\".");

                        res.end("OK");
                    });
                }
                else {
                    res.end(err);
                }
            }
            else {
                res.end("File \"" + filePathNew + "\" already exists!");
            }
        });
    });
});

app.post('/wds/image/content/', function (req, res) {
    var data = req.body;

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    ps.addCommand('$image = Get-WindowsImage', ['ImagePath "' + data.Path + '"']);
    ps.invoke()
        .then(output => {
            console.log(output);
            //res.end(output);
            ps.addCommand('$image | ConvertTo-Json')
            ps.invoke()
                .then(output => {
                    console.log(output);
                    res.end(output);
                })
                .catch(err => {
                    console.log(err);
                    ps.dispose();
                });
        })
        .catch(err => {
            console.log(err);
            //ps.dispose();
            res.end(err);
        });
});

app.post('/wds/imagegroup/install/', function (req, res) {
    var data = req.body;

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    ps.addCommand('Import-Module', ['Name "WDS"']);
    ps.invoke()
        .then(output => {
            console.log(output);
            //res.end(output);
            ps.addCommand('$group = New-WdsInstallImageGroup', ['Name "' + data.Name + '"']);
            ps.invoke()
                .then(output => {
                    console.log(output);
                    //res.end(output);
                    ps.addCommand('$group | ConvertTo-Json')
                    ps.invoke()
                        .then(output => {
                            console.log(output);
                            res.end(output);
                        })
                        .catch(err => {
                            console.log(err);
                            ps.dispose();
                        });
                })
                .catch(err => {
                    console.log(err);
                    //ps.dispose();
                    res.end(err);
                });
        })
        .catch(err => {
            console.log(err);
            //ps.dispose();
            res.end(err);
        });
});

app.post('/wds/image/install/', function (req, res) {
    var data = req.body;

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    ps.addCommand('Import-Module', ['Name "WDS"']);
    ps.invoke()
        .then(output => {
            console.log(output);
            //res.end(output);
            if (data.EnableMulticastTransmission) {
                ps.addCommand('$image = Import-WdsInstallImage', ['ImageGroup "' + data.ImageGroupName + '"', 'Path "' + data.ImageFilePath + '"', 'NewImageName "' + data.NewImageName + '"', 'NewDescription "' + data.NewDescription + '"', 'NewFileName "' + data.NewFileName + '"', 'ImageName "' + data.RawImageNameInFile + '"', 'Multicast', 'TransmissionName "' + data.MulticastTransmissionName + '"']);
            }
            else {
                ps.addCommand('$image = Import-WdsInstallImage', ['ImageGroup "' + data.ImageGroupName + '"', 'Path "' + data.ImageFilePath + '"', 'NewImageName "' + data.NewImageName + '"', 'NewDescription "' + data.NewDescription + '"', 'NewFileName "' + data.NewFileName + '"', 'ImageName "' + data.RawImageNameInFile + '"']);
            }
            ps.invoke()
                .then(output => {
                    console.log(output);
                    //res.end(output);
                    ps.addCommand('$image | ConvertTo-Json')
                    ps.invoke()
                        .then(output => {
                            console.log(output);
                            res.end(output);
                        })
                        .catch(err => {
                            console.log(err);
                            ps.dispose();
                        });
                })
                .catch(err => {
                    console.log(err);
                    //ps.dispose();
                    res.end(err);
                });
        })
        .catch(err => {
            console.log(err);
            //ps.dispose();
            res.end(err);
        });
});


app.post('/wds/image/boot/', function (req, res) {
    var data = req.body;

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    ps.addCommand('Import-Module', ['Name "WDS"']);
    ps.invoke()
        .then(output => {
            console.log(output);
            //res.end(output);
            if (data.EnableMulticastTransmission) {
                ps.addCommand('$image = Import-WdsBootImage', ['Path "' + data.ImageFilePath + '"', 'NewImageName "' + data.NewImageName + '"', 'NewDescription "' + data.NewDescription + '"', 'NewFileName "' + data.NewFileName + '"', 'DisplayOrder ' + data.DisplayOrder, 'Multicast', 'TransmissionName "' + data.MulticastTransmissionName + '"']);
            }
            else {
                ps.addCommand('$image = Import-WdsBootImage', ['Path "' + data.ImageFilePath + '"', 'NewImageName "' + data.NewImageName + '"', 'NewDescription "' + data.NewDescription + '"', 'NewFileName "' + data.NewFileName + '"', 'DisplayOrder ' + data.DisplayOrder]);
            }
            ps.invoke()
                .then(output => {
                    console.log(output);
                    //res.end(output);
                    ps.addCommand('$image | ConvertTo-Json')
                    ps.invoke()
                        .then(output => {
                            console.log(output);
                            res.end(output);
                        })
                        .catch(err => {
                            console.log(err);
                            ps.dispose();
                        });
                })
                .catch(err => {
                    console.log(err);
                    //ps.dispose();
                    res.end(err);
                });
        })
        .catch(err => {
            console.log(err);
            //ps.dispose();
            res.end(err);
        });
});


app.patch('/wds/image/boot/', function (req, res) {
    var data = req.body;

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    ps.addCommand('Import-Module', ['Name "WDS"']);
    ps.invoke()
        .then(output => {
            console.log(output);
            //res.end(output);
            if (data.EnableMulticastTransmission) {
                ps.addCommand('$image = Set-WdsBootImage', ['Architecture ' + data.Architecture, 'ImageName "' + data.ImageName + '"', 'Multicast', 'DisplayOrder ' + data.DisplayOrder, 'FileName "' + data.FileName + '"', 'NewDescription "' + data.NewDescription + '"', 'NewImageName "' + data.NewImageName + '"', 'TransmissionName "' + data.MulticastTransmissionName + '"']);
            }
            else {
                ps.addCommand('$image = Set-WdsBootImage', ['Architecture ' + data.Architecture, 'ImageName "' + data.ImageName + '"', 'StopMulticastTransmission', 'Force', 'DisplayOrder ' + data.DisplayOrder, 'FileName "' + data.FileName + '"', 'NewDescription "' + data.NewDescription + '"', 'NewImageName "' + data.NewImageName + '"']);
            }
            ps.invoke()
                .then(output => {
                    console.log(output);
                    //res.end(output);
                    ps.addCommand('$image | ConvertTo-Json')
                    ps.invoke()
                        .then(output => {
                            console.log(output);
                            res.end(output);
                        })
                        .catch(err => {
                            console.log(err);
                            ps.dispose();
                        });
                })
                .catch(err => {
                    console.log(err);
                    //ps.dispose();
                    res.end(err);
                });
        })
        .catch(err => {
            console.log(err);
            //ps.dispose();
            res.end(err);
        });
});


app.delete('/wds/image/boot/:arch/:name', function (req, res) {
    //var data = req.body;

    var imageName = req.params.name;
    imageName = String(imageName).replace(/\$/g, " ");

    var architecture = req.params.arch;

    var fileName = req.query.fname != null ? String(req.query.fname).replace(/\$/g, " ") : null;
    //fileName = String(fileName).replace(/\$/g, " ");

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    ps.addCommand('Import-Module', ['Name "WDS"']);
    ps.invoke()
        .then(output => {
            console.log(output);
            //res.end(output);
            if (fileName != null && fileName != '') {
                ps.addCommand('$image = Remove-WdsBootImage', ['Architecture ' + architecture, 'ImageName "' + imageName + '"', 'FileName "' + fileName + '"']);
            } else {
                ps.addCommand('$image = Remove-WdsBootImage', ['Architecture ' + architecture, 'ImageName "' + imageName + '"']);
            }
            ps.invoke()
                .then(output => {
                    console.log(output);
                    //res.end(output);
                    ps.addCommand('$image | ConvertTo-Json')
                    ps.invoke()
                        .then(output => {
                            console.log(output);
                            res.end(output);
                        })
                        .catch(err => {
                            console.log(err);
                            ps.dispose();
                        });
                })
                .catch(err => {
                    console.log(err);
                    //ps.dispose();
                    res.end(err);
                });
        })
        .catch(err => {
            console.log(err);
            //ps.dispose();
            res.end(err);
        });
});


app.delete('/wds/image/install/:group/:name', function (req, res) {
    //var data = req.body;

    var imageName = req.params.name;
    imageName = String(imageName).replace(/\$/g, " ");

    var imageGroup = req.params.group;

    var fileName = req.query.fname != null ? String(req.query.fname).replace(/\$/g, " ") : null;
    //fileName = String(fileName).replace(/\$/g, " ");

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    ps.addCommand('Import-Module', ['Name "WDS"']);
    ps.invoke()
        .then(output => {
            console.log(output);
            //res.end(output);
            if (fileName != null && fileName != '') {
                ps.addCommand('$image = Remove-WdsInstallImage', ['ImageName "' + imageName + '"', 'ImageGroup "' + imageGroup + '"', 'FileName "' + fileName + '"']);
            } else {
                ps.addCommand('$image = Remove-WdsInstallImage', ['ImageName "' + imageName + '"', 'ImageGroup "' + imageGroup + '"']);
            }
            ps.invoke()
                .then(output => {
                    console.log(output);
                    //res.end(output);
                    ps.addCommand('$image | ConvertTo-Json')
                    ps.invoke()
                        .then(output => {
                            console.log(output);
                            res.end(output);
                        })
                        .catch(err => {
                            console.log(err);
                            ps.dispose();
                        });
                })
                .catch(err => {
                    console.log(err);
                    //ps.dispose();
                    res.end(err);
                });
        })
        .catch(err => {
            console.log(err);
            //ps.dispose();
            res.end(err);
        });
});
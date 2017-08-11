var express = require('express');
var bodyParser = require('body-parser');
//var fs = require("fs");
var readDir = require("readdir");
var shell = require('node-powershell');
var redis = require("redis");
var cors = require("cors");
var config = require("nodejs-config")(__dirname);
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

var http = require('http').Server(app);
var io = require('socket.io')(http);

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

    socket.on("msg:cltstat", function (data) {

        var ipAddress = socket.handshake.address;
        var clientStatus = { ip: ipAddress, status: data };

        updateClientStatus(status);
    });
});

var httpServerPort = config.get("app.http-server-port"); //8089;

var redisAddress = config.get("app.redis-address"); //"127.0.0.1";
var redisPort = config.get("app.redis-port"); //6379;
var redisPassword = config.get("app.redis-password"); //"P@ssword1";
var redisDbIndex = config.get("app.redis-db-index"); //0;

var redisDbIndexClientStatus = config.get("app.redis-db-index-client-status"); //0;

var installImageRepository = config.get("app.install-image-repository");
var bootImageRepository = config.get("app.boot-image-repository");

function getRedisClient() {
    var client = redis.createClient(redisPort, redisAddress);
    client.auth(redisPassword);

    return client;
}

function persistData(data)
{
}

function updateClientStatus(data)
{
    var redisClient = getRedisClient();

    redisClient.select(redisDbIndexClientStatus, function (err) {
        if (err) {
            console.log(err);
        }
        else
        {
            redisClient.set(data.ip, data.status, function (err, result)
            {
                if (err) {
                    console.log(err);
                }
                else
                {
                    console.log(result);
                    redisClient.end(true);
                }
            });
        }
    });
}

var server = app.listen(httpServerPort, function () {
    var host = server.address().address;
    var port = server.address().port;
    console.log("WDS RESTful API service listening at http://%s:%s", host, port);
})

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
                    //var lookups = [];
                    //async.map(keys, function (key, callback) {
                    //    redisClient.get(key, function (err, value) {
                    //        if (err)
                    //        {
                    //            callback(err);
                    //        }
                    //        else {
                    //            var lookupItem = { key: "", value: "" };
                    //            lookupItem.key = key;
                    //            lookupItem.value = value;
                    //            callback(null, lookupItem);

                    //            console.log(JSON.stringify(lookups));
                    //            redisClient.end(true);
                    //            res.end(JSON.stringify(lookups));
                    //        }
                    //    });
                    //},
                    //function (err, result) {
                    //    if (err) {
                    //        console.log(err);
                    //        res.end(err);
                    //    }
                    //    else {
                    //        console.log(result);
                    //        //res.json({ data: results });
                    //        //res.send(result);

                    //        lookups.push(result);
                    //    }
                    //});

                    //console.log(lookups);
                    //redisClient.end(true);
                    //res.end(JSON.stringify(lookups));

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

    //fs.readdir(installImageRepository, function (err, files)
    //{
    //    if (err) { res.end(err.message); }
    //    else {
    //        res.end(JSON.stringify(files));
    //    }
    //});

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
})

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
})

app.post('/wds/image/install/', function (req, res)
{
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
            if (data.EnableMulticastTransmission)
            {
                ps.addCommand('$image = Import-WdsInstallImage', ['ImageGroup "' + data.ImageGroupName + '"', 'Path "' + data.ImageFilePath + '"', 'NewImageName "' + data.NewImageName + '"', 'NewDescription "' + data.NewDescription + '"', 'NewFileName "' + data.NewFileName + '"', 'ImageName "' + data.RawImageNameInFile + '"', 'Multicast', 'TransmissionName "' + data.MulticastTransmissionName + '"']);
            }
            else
            {
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
})


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
})


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
})


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
            } else
            {
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
})

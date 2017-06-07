var express = require('express');
var bodyParser = require('body-parser');
var fs = require("fs");
var shell = require('node-powershell');
var redis = require("redis");

var app = express();
app.use(bodyParser.json()); // for parsing application/json
app.use(bodyParser.urlencoded({ extended: true })); // for parsing application/x-www-form-urlencoded

var httpServerPort = 8089;

var redisAddress = "127.0.0.1";
var redisPort = 6379;
var redisPassword = "P@ssword1";
var redisDbIndex = 0;

function getRedisClient() {
    var client = redis.createClient(redisPort, redisAddress);
    client.auth(redisPassword);

    return client;
}

var server = app.listen(httpServerPort, function () {
    var host = server.address().address
    var port = server.address().port
    console.log("WDS RESTful API service listening at http://%s:%s", host, port)
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
            redisClient.set(data.key, data.value, function (err, result) {
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
            if (imageName != '') {
                ps.addCommand('Get-WdsBootImage', ['Name "' + imageName + '"']);
            } else {
                ps.addCommand('Get-WdsBootImage');
            }
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
            ps.dispose();
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
            if (imageName != '') {
                ps.addCommand('Get-WdsInstallImage', ['Name "' + imageName + '"']);
            } else {
                ps.addCommand('Get-WdsInstallImage');
            }
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
            ps.dispose();
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
            if (imageGroupName != '') {
                ps.addCommand('Get-WdsInstallImageGroup', ['Name "' + imageGroupName + '"']);
            } else {
                ps.addCommand('Get-WdsInstallImageGroup');
            }
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
            ps.dispose();
        });
});

app.post('/wds/image/content/', function (req, res) {
    var data = req.body;

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    ps.addCommand('Get-WindowsImage', ['ImagePath "' + data.Path + '"']);
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
            ps.addCommand('New-WdsInstallImageGroup', ['Name "' + data.Name + '"']);
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
            ps.dispose();
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
                ps.addCommand('Import-WdsInstallImage', ['ImageGroup "' + data.ImageGroupName + '"', 'Path "' + data.ImageFilePath + '"', 'NewImageName "' + data.NewImageName + '"', 'NewDescription "' + data.NewDescription + '"', 'NewFileName "' + data.NewFileName + '"', 'ImageName "' + data.RawImageNameInFile + '"', 'Multicast', 'TransmissionName "' + data.MulticastTransmissionName + '"']);
            }
            else
            {
                ps.addCommand('Import-WdsInstallImage', ['ImageGroup "' + data.ImageGroupName + '"', 'Path "' + data.ImageFilePath + '"', 'NewImageName "' + data.NewImageName + '"', 'NewDescription "' + data.NewDescription + '"', 'NewFileName "' + data.NewFileName + '"', 'ImageName "' + data.RawImageNameInFile + '"']);
            }
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
            ps.dispose();
        });
})


var express = require('express');
var bodyParser = require('body-parser');
var fs = require("fs");
var shell = require('node-powershell');
var app = express();

app.use(bodyParser.json()); // for parsing application/json
app.use(bodyParser.urlencoded({ extended: true })); // for parsing application/x-www-form-urlencoded


app.get("/services", function (req, res) {

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    ps.addCommand('$services = get-service')
    ps.invoke()
        .then(output => {
            console.log(output);
            //res.end(JSON.stringify(output));
            //res.end(output);
            ps.addCommand('$services | convertto-json')
            ps.invoke()
                .then(output => {
                    console.log(output);
                    //res.end(JSON.stringify(output));
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

app.get("/value/:key", function (req, res)
{
    var key = req.params.key;

    console.log(key);

    var value = "";

    let ps = new shell({
        executionPolicy: 'Bypass',
        noProfile: true
    });

    ps.addCommand('import-module', ['Name "D:\\WDS\\PowerRedis\\PowerRedis.psd1"'] )
    ps.invoke()
        .then(output => {
            console.log(output);
            //res.end(JSON.stringify(output));
            //res.end(output);

            ps.addCommand('Connect-RedisServer', ['RedisServer 127.0.0.1', 'Database 0'])
            ps.invoke().then(output => {
                console.log(output);
                ps.addCommand('$key = Get-RedisKey', ['Key "' + key + '"']);
                ps.invoke().then(output => {
                    console.log(output);
                    //res.end(output);
                    ps.addCommand('$key | ConvertTo-Json');
                    ps.invoke().then(output =>
                    {
                        console.log(output);
                        res.end(output);
                    }).catch(err => {
                        console.log(err);
                        ps.dispose();
                    });
                }).catch(err => {
                    console.log(err);
                    ps.dispose();
                });
            }).catch(err => {
                console.log(err);
                ps.dispose();});
        })
        .catch(err => {
            console.log(err);
            ps.dispose();
        });

    //res.send(key);
});

app.post("/data", function (req, res) {

    var data = JSON.stringify(req.body);

    fs.appendFile("d:\\node-test.txt", data);

    console.log(data);

    res.set({ 'Content-Type': 'application/json', 'Encodeing': 'utf8' });

    res.send(data);
});

var server = app.listen(8088, function () {
    var host = server.address().address
    var port = server.address().port
    console.log("Example app listening at http://%s:%s", host, port)
})
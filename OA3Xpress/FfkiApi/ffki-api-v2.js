var express = require('express');
var bodyParser = require('body-parser');
var fs = require("fs");
var readDir = require("readdir");
var multer = require('multer');
var cors = require("cors");
var config = require("nodejs-config")(__dirname);
var edge = require("edge-js");

var assert = require('assert');
//var bearerToken = require('express-bearer-token');
var cluster = require('cluster');
var os = require('os');
var cpuCount = os.cpus().length;
var app = express();

var et = require('elementtree');
var XML = et.XML;
var ElementTree = et.ElementTree;
var element = et.Element;
var subElement = et.SubElement;

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true })); 
app.use(cors());

var enableCpuClustering = config.get("app.enable-cpu-clustering");
var httpServerPort = config.get("app.http-server-port"); 
var mssqlConnectionString = config.get("app.mssql-connection-string");
var oa3ReportXmlRepository = config.get("app.report-xml-repository");
var oa3ConfigXmlRepository = config.get("app.config-xml-repository");
var logRepository = config.get("app.log-repository");
var ffkiVersion = config.get("app.ffki-version");

var oa3ReportXmlUploader = multer({
    storage: multer.diskStorage({
        destination: function (req, file, cb) {
            cb(null, oa3ReportXmlRepository);
        },
        filename: function (req, file, cb) {
            cb(null, Date.now().toString() + "_" + file.originalname);
        }
    })
}).any();

var oa3ConfigXmlUploader = multer({
    storage: multer.diskStorage({
        destination: function (req, file, cb) {
            cb(null, oa3ConfigXmlRepository);
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

var sqlGetAllBusiness = edge.func('sql', {
    connectionString: mssqlConnectionString,
    source: "SELECT BusinessID, BusinessName FROM Profile"
});

var sqlGetParamValueByParamNameAndBizID = edge.func('sql', {
    connectionString: mssqlConnectionString,
    source: "SELECT DISTINCT @ParamName FROM ProductKey WHERE ProfileID IN (SELECT ProfileID FROM Profile WHERE BusinessID = @BusinessId) AND KeyTypeId = @KeyType"
});

var sqlGetSKU = edge.func('sql', {
    connectionString: mssqlConnectionString,
    source: "SELECT DISTINCT LicensablePartNumber, LicensableName, SKUID FROM ProductKey"
});

var sqlGetKeysByBizID = edge.func('sql', {
    connectionString: mssqlConnectionString,
    source: "SELECT TOP @KeyCount ProductKeyID FROM ProductKey WHERE ProfileID IN (SELECT ProfileID FROM Profile WHERE BusinessID = @BusinessId) AND KeyTypeId = @KeyType"
});

var sqlSetSNByKeyID = edge.func('sql', {
    connectionString: mssqlConnectionString,
    source: "UPDATE ProductKeyInfo SET SerialNumber = @SerialNumber WHERE ProductKeyID = @ProductKeyID"
});


var http = require('http').Server(app);
var server;

if (enableCpuClustering == true) {

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
            server = app.listen(httpServerPort, function () {
            var host = server.address().address;
            var port = server.address().port;
            console.log("OA3.0 FFKI RESTful API service listening at http://%s:%s", host, port);
        });

        console.log(`Worker ${process.pid} started`);
    }
} else {
        server = app.listen(httpServerPort, function () {
        var host = server.address().address;
        var port = server.address().port;
        console.log("OA3.0 FFKI RESTful API service listening at http://%s:%s", host, port);
    });
}

app.get('/', function (req, res) {
    res.send('Welcome to OA3.0!');
});

app.post("/oa3/report/", function (req, res) {

    oa3ReportXmlUploader(req, res, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }

        console.log(req.files[0].path);

        res.end();
    });
});

app.post("/oa3/config/", function (req, res) {

    oa3ConfigXmlUploader(req, res, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }

        console.log(req.files[0].path);

        res.end();
    });
});


app.post("/oa3/log/:trsnid", function (req, res) {

    logUploader(req, res, function (err) {
        if (err) {
            console.log(err);
            res.end(err);
        }

        console.log(req.params.trsnid);
        console.log(req.files[0].path);

        res.end();
    });
});

app.get('/oa3/business/all', function (req, res) {
    try {
        console.log(mssqlConnectionString);

        sqlGetAllBusiness(null, function (error, result) {
            if (error) { console.log(error); return; }
            if (result) {
                console.log(result);
                //res.end(JSON.stringify(result));

                var bizXmlRoot = element('CloudOAConfiguration');
                bizXmlRoot.set('xmlns:xsi', 'http://www.w3.org/2001/XMLSchema-instance');
                bizXmlRoot.set('xmlns:xsd', 'http://www.w3.org/2001/XMLSchema');
                var bizSettings = subElement(bizXmlRoot, 'BusinessSettings');

                for (var i = 0; i < result.length; i++) {
                    var bizSetting = subElement(bizSettings, 'CloudOABusinessSetting');
                    var bizId = subElement(bizSetting, 'BusinessId');
                    bizId.text = result[i].BusinessID;
                    var bizName = subElement(bizSetting, 'BusinessName');
                    bizName.text = result[i].BusinessName;
                }

                var etree = new ElementTree(bizXmlRoot);
                var resultXml = etree.write({ 'xml_declaration': false });

                console.log(resultXml);
                res.end(resultXml);
            }
            else {
                console.log("No results");
            }
        });

    } catch (err) {
        console.log(err);
    }
});


app.get('/oa3/parameter/:bizid/:name/:keytype', function (req, res) {
    try {
        console.log(mssqlConnectionString);

        var paramName = req.params.name;

        var sqlCommandText = "SELECT DISTINCT " + paramName + " FROM ProductKey WHERE ProfileID IN (SELECT ProfileID FROM Profile WHERE BusinessID = @BusinessId) AND KeyTypeId = @KeyType";

        console.log(sqlCommandText);

        sqlGetParamValueByParamNameAndBizID = edge.func('sql', {
            connectionString: mssqlConnectionString,
            source: sqlCommandText
        });

        sqlGetParamValueByParamNameAndBizID({ BusinessId: req.params.bizid, KeyType: req.params.keytype}, function (error, result) {
            if (error) { console.log(error); return; }
            if (result) {
                console.log(result);

                var items = [];

                for (var i = 0; i < result.length; i++) {
                    items.push(result[i][paramName]);
                }

                console.log(items);
                res.end(JSON.stringify(items));

                //res.end(JSON.stringify(result));
            }
            else {
                console.log("No results");
            }
        });

    } catch (err) {
        console.log(err);
    }
});

app.get('/oa3/sku/', function (req, res) {
    try {
        console.log(mssqlConnectionString);

        sqlGetSKU(null, function (error, result) {
            if (error) { console.log(error); return; }
            if (result) {
                console.log(result);
                res.end(JSON.stringify(result));
            }
            else {
                console.log("No results");
            }
        });

    } catch (err) {
        console.log(err);
    }
});

app.post("/oa3/keys/query/:keycount/:bizid/:keytype", function (req, res) {
    try {
        console.log(mssqlConnectionString);

        var queryItems = req.body;

        var keyCount = req.params.keycount;

        var sqlParams = {
            BusinessId: req.params.bizid,
            KeyType: req.params.keytype
        };

        var numKeyCount = Number(keyCount);

        var sqlCommandText = "SELECT ProductKeyID FROM ProductKey WHERE ProfileID IN (SELECT ProfileID FROM Profile WHERE BusinessID = @BusinessId) AND KeyTypeId = @KeyType";

        if (numKeyCount > 0)
        {
            sqlCommandText = "SELECT TOP " + keyCount + " ProductKeyID FROM ProductKey WHERE ProfileID IN (SELECT ProfileID FROM Profile WHERE BusinessID = @BusinessId) AND KeyTypeId = @KeyType";
        }

        if (queryItems != null)
        {
            console.log(JSON.stringify(queryItems));

            for (var i = 0; i < queryItems.length; i++) {
                sqlCommandText += " AND " + queryItems[i].name + " = @" + queryItems[i].name;
                sqlParams[queryItems[i].name] = queryItems[i].value;
            }
        }

        sqlCommandText += " ORDER BY ProductKeyID DESC";

        console.log(sqlCommandText);

        console.log(sqlParams);

        sqlGetKeysByBizID = edge.func('sql', {
            connectionString: mssqlConnectionString,
            source: sqlCommandText
        });

        sqlGetKeysByBizID(sqlParams, function (error, result) {
            if (error) { console.log(error); return; }
            if (result) {
                console.log(result);

                var items = [];

                for (var i = 0; i < result.length; i++) {
                    items.push(result[i].ProductKeyID);
                }

                console.log(items);
                res.end(JSON.stringify(items));

                //res.end(JSON.stringify(result));
            }
            else {
                console.log("No results");
            }
        });
    } catch (err) {
        console.log(err);
    }
});

app.post("/oa3/sn/", function (req, res) {
    try {
        console.log(mssqlConnectionString);

        var queryItems = req.body;

        var sqlParams = {
            SerialNumber: queryItems.sn,
            ProductKeyID: queryItems.keyid
        };

        console.log(sqlParams);

        sqlSetSNByKeyID(sqlParams, function (error, result) {
            if (error) { console.log(error); return; }
            if (result) {
                console.log(result);

                res.end(JSON.stringify(result));
            }
            else {
                console.log("No results");
            }
        });
    } catch (err) {
        console.log(err);
    }
});
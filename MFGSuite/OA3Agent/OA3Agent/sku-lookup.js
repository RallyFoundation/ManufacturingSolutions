var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var redis = require("redis");
var redisClient = null;

var redisAddress = "127.0.0.1";
var redisPort = 6379;
var redisPassword = "P@ssword1";

app.get('/', function (req, res) {
    res.send('<h1>Welcome!</h1>');
});
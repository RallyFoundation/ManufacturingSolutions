//const spawn = require('child_process').spawn;
//const bat = spawn('D:\\MDOS\\RS2OA3ToolRTM\\amd64\\oa3tool.exe', ['/Validate']);//spawn('cmd.exe', ['D:\Documents\runoa3.cmd']);

//bat.stdout.on('data', (data) => {
//    console.log(data.toString());
//});

//bat.stderr.on('data', (data) => {
//    console.log(data.toString());
//});

//bat.on('exit', (code) => {
//    console.log(`Child exited with code ${code}`);
//});


const execFile = require('child_process').execFile;

var path = "D:\\MDOS\\RS2OA3ToolRTM\\amd64\\oa3tool.exe"; //"notepad";
var params = ["/Validate"]; //["D:\\Documents\\hosts"];

const child = execFile(path, params, (error, stdout, stderr) => {
    if (error) {
        throw error;
    }
    console.log(stdout);
});

//const execFile = require('child_process').execFile;
//const child = execFile('node', ['--version'], (error, stdout, stderr) => {
//    if (error) {
//        throw error;
//    }
//    console.log(stdout);
//});
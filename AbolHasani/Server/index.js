const express = require('express');
const app = express();
var path = require('path');

app.get('/', function(req, res){
    var bodyParser = require('body-parser');
    app.use(bodyParser.json());
    app.use(bodyParser.urlencoded({ extended: true }));  
    var app_dirname = req.query.name;
    console.log("=============================")
    console.log(" request to download file :"+app_dirname);
    console.log("=============================")

   // var path = './SafeDir/'+app_dirname;
   // res.sendFile(path);
    res.sendFile(app_dirname, { root: __dirname+"/UploadedFiles/" });
  });
  app.listen(process.env.PORT || 9001, () => console.log("Listen on port 9001 --["+process.env.PORT+"]"));
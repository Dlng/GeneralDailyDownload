var casper = require('casper').create({
    verbose: true,
    logLevel: "debug",
});

var exitCode = {
    "OK": 0,
    "timeout_fundListTable": 1
};
var errorCode = exitCode.OK;

var utils = require('utils');
var fs = require("fs");

var terms = casper.cli.args;
utils.dump(terms);
if (terms.length === 0) {
    casper
        .echo("Usage: $ casperjs PRUDENTIAL_BSN1.js resultfilename")
        .exit(1)
    ;
}


var resultfilename = terms[0];

casper.start('https://www.prubsn.com.my/xml/fundprices.xml');

casper.then(function(){
	fs.write(resultfilename,this.page.content,'w');
});

casper.run();
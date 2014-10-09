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
        .echo("Usage: $ casperjs aia_unitPrice.js resultfilename")
        .exit(1)
    ;
}

var resultfilename = terms[0];

casper.start("https://www.aiadirect.com.my/agent/unitprice.asp");

casper.waitForSelector(".alignCenter",
	function tableList(){
		fs.write(resultfilename, this.page.content, 'w');
	},
	function timeOut(){
        this.echo("tablePage timeout.");
        errorCode = exitCode.timeout_fundListTable;
	},3000);

casper.run(function end() {
    this.capture("aia_unitPrice.png");
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});
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
        .echo("Usage: $ casperjs cigna.js  resultfilename")
        .exit(1)
    ;
}


var resultfilename = terms[0];

casper.start('http://www.cigna.com.hk/CIGNA/fund_information/index.do?lang=en_US');

casper.waitForSelector("table[class='infoTable formsList']",
	function()
	{
		casper.capture('cigna.png');
		fs.write(resultfilename,this.page.content,'w');
	},
	function tableTimeout(){
		errorCode = exitCode.timeout_fundListTable;
	},
	5000);

casper.run(function end() {
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});
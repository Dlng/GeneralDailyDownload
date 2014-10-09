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
        .echo("Usage: $ casperjs avia_lifetrack.js  resultfilename")
        .exit(1)
    ;
}


var resultfilename = terms[0];

casper.start('http://www.aviva-asia.com/cfmappscf/hk/en/FloatingRate/fundprices_lifetrack_details.cfm');

casper.waitForSelector("table[id='fundprices']",
	function()
	{
		casper.capture('avia_lifetrack.png');
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
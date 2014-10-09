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
        .echo("Usage: $ casperjs prudential_assurance.js  resultfilename")
        .exit(1)
    ;
}


var resultfilename = terms[0];

casper.start('http://www2.prudential.com.my/fundpriceV2/daily.php   ');

// wait for finished in 40s
casper.waitForSelector("table[class='daily']",
	function()
	{
		casper.capture('prudential.png');
		fs.write(resultfilename,this.page.content,'w');
	},
	function tableTimeout(){
		errorCode = exitCode.timeout_fundListTable;
	},
	50000);

casper.run(function end() {
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});
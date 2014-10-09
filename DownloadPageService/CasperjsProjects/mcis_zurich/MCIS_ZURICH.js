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
        .echo("Usage: $ casperjs mcis_zurich.js  resultfilename")
        .exit(1)
    ;
}


var resultfilename = terms[0];

// this one is unstable waitFor()timeout or 
casper.start('http://www.mciszurich.com.my/ilink/bottom.asp');

// loading resource failed =with statusp "fail" (no ; at the wnd of color)

//waitfor finished in 40ms

casper.waitForSelector("table[bordercolor='#336699']",
	function()
	{
		casper.capture('mcis.png');
		fs.write(resultfilename,this.page.content,'w');
	},
	function tableTimeout(){
		errorCode = exitCode.timeout_fundListTable;
	},
	500000);

casper.run(function end() {
	this.capture('mcis2.png');
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);

});


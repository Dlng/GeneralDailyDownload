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
        .echo("Usage: $ casperjs axa.js resultfilename")
        .exit(1)
    ;
}

var resultfilename = terms[0];

casper.start("http://www.axa-affin.com/124/en/fund-updates/fund-prices/introduction");

casper.waitForSelector("div[class = 'divContent']",
	function tableList() {
	    fs.write(resultfilename, this.page.content, 'w');
	},
	function timeOut() {
	    this.echo("tablePage timeout.");
	    errorCode = exitCode.timeout_fundListTable;
	}, 3000);

casper.run(function end() {
    this.capture("axa.png");
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});
var casper = require('casper').create({
    verbose: true,
    logLevel: "debug",
});

var exitCode = {
    "OK": 0,
    "timeout_btnAgree": 1,
    "timeout_fundListTable": 2
};
var errorCode = exitCode.OK;

var utils = require('utils');
var fs = require("fs");

var terms = casper.cli.args;
utils.dump(terms);
if (terms.length === 0) {
    casper
        .echo("Usage: $ casperjs massmutual.js resultfilename")
        .exit(1)
    ;
}

var resultfilename = terms[0];

casper.start("http://corp.massmutualasia.com/en/Invest/FLEXI-Series/Investment-Unit-Prices.aspx");

casper.waitForSelector("a[href='/en/Invest/Disclaimer.aspx?AcceptDisclaimer=true']",
    function agreebtn() {
        this.click("a[href='/en/Invest/Disclaimer.aspx?AcceptDisclaimer=true']");
    },
    function agreebtnTimeout() {
        errorCode = exitCode.timeout_btnAgree;
        this.bypass(9999);
    },
    3000);

casper.waitForSelector("#reportTable",
	function tableList() {
	    fs.write(resultfilename, this.page.content, 'w');
	},
	function timeOut() {
	    this.echo("tablePage timeout.");
	    errorCode = exitCode.timeout_fundListTable;
	}, 3000);

casper.run(function end() {
    this.capture('massmutual.png');
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});
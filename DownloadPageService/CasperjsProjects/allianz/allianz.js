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
        .echo("Usage: $ casperjs allianz.js resultfilename")
        .exit(1)
    ;
}

var resultfilename = terms[0];

casper.start();
casper.userAgent('Mozilla/5.0 (Windows NT 6.1; WOW64; rv:30.0) Gecko/20100101 Firefox/30.0'); // stuck after page is open
casper.thenOpen("https://www.allianz.com.my/web/fund-pricing/10598/10006");


casper.waitForSelector(".cms-table",
	function tableList(){
		fs.write(resultfilename, this.page.content, 'w');
	},
	function timeOut(){
        this.echo("tablePage timeout.");
        errorCode = exitCode.timeout_fundListTable;
	},10000);

casper.run(function end() {
    casper.capture("allianz.png");
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});
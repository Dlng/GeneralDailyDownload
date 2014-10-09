var casper = require('casper').create({
    verbose: true,
    logLevel: "debug",
});

var exitCode = {
    "OK": 0,
    "timeout_agree": 1,
    "timeout_tableList": 2,
    "timeout_fancyboxPopup": 3
};
var errorCode = exitCode.OK;

var utils = require('utils');
var fs = require("fs");
var webPage = require('webpage').create();

var terms = casper.cli.args;
utils.dump(terms);
if (terms.length === 0) {
    casper
        .echo("Usage: $ casperjs BNP.js resultfilename")
        .exit(1)
    ;
}
var resultfilename = terms[0];

casper.start("http://www.bnpparibas-ip.sg/central/fundsearch/index.page?l=eng&p=IP_SG-NSG&autosearch=true#tab=4&page=1&sortOn=FM_COMP_NAME&sortOrder=increasing&results=100&FM_RATING_---=true&FM_RATING_1=true&FM_RATING_2=true&FM_RATING_3=true&FM_RATING_4=true&FM_RATING_5=true&FM_ASSET_CLASSIF1=&FM_ASSET_CLASSIF2=&FM_SHARE_TYPE=&FM_DIVIDEND_POLICY=&FM_BASE_CURRENCY=");

casper.waitForSelector('#result_table',
	function tableList() {
	    this.capture("fund1.png");
	    fs.write(resultfilename, this.page.content, 'w');
	},

	function timeOut() {
	    errorCode = exitCode.timeout_tableList;
	    this.bypass(99999);
	},
	20000);


casper.run(function end() {
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});
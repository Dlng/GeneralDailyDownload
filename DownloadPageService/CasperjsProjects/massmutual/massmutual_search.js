var casper = require('casper').create({
    verbose: true,
    logLevel: "debug",
});

var exitCode = {
    "OK": 0,
    "timeout_btnAgree": 1,
    "timeout_select": 2,
    "timeout_fundListTable": 3
};
var errorCode = exitCode.OK;

var utils = require('utils');
var fs = require("fs");

var terms = casper.cli.args;
utils.dump(terms);
if (terms.length === 0) {
    casper
        .echo("Usage: $ casperjs massmutual_search.js resultfilename")
        .exit(1)
    ;
}

var resultfilename = terms[0];
var prodId = "GF        ";
casper.start("http://corp.massmutualasia.com/en/Invest/Premier-Choice-Series/Search.aspx");

casper.waitForSelector("a[href='/en/Invest/Disclaimer.aspx?AcceptDisclaimer=true']",
    function agreebtn() {
        this.click("a[href='/en/Invest/Disclaimer.aspx?AcceptDisclaimer=true']");
    },
    function agreebtnTimeout() {
        errorCode = exitCode.timeout_btnAgree;
        this.bypass(9999);
    },
    3000);

casper.wait(2000);

casper.waitForSelector("select[id = 'phfundsearchfilter_1_ddlAssetType']",
    function () {

        var sel = this.evaluate(
            function selProd(prodValue) {

                var element = document.querySelector("#phfundsearchfilter_1_ddlAssetType");

                element.value = prodValue;
                var evt = document.createEvent('HTMLEvents');
                evt.initEvent('change', false, true);
                element.dispatchEvent(evt);

            }, prodId);

    },
    function timeout() {
        errorCode = exitCode.timeout_select;
        this.bypass(9999);
    },
    5000);

casper.waitForSelector("#reportTable",
	function tableList() {
	    fs.write(resultfilename, this.page.content, 'w');
	},
	function timeOut() {
	    this.echo("tablePage timeout.");
	    errorCode = exitCode.timeout_fundListTable;
	}, 3000);

casper.run(function end() {
    this.capture('massmutual_search.png');
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});
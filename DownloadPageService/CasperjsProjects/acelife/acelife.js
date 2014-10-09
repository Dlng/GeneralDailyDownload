var casper = require('casper').create({
    verbose: true,
    logLevel: "debug",
});

var exitCode = {
    "OK": 0,
    "timeout_btnAgree": 101,
    "timeout_fundListTable": 102
};
var errorCode = exitCode.OK;

var utils = require('utils');
var fs = require("fs");

var terms = casper.cli.args;
utils.dump(terms);
if (terms.length === 0) {
    casper
        .echo("Usage: $ casperjs acelife.js prodId(41/16/42/38/47) resultfilename")
        .exit(1)
    ;
}

var prodId = terms[0] + "";
var resultfilename = terms[1];

casper.start('http://www.acelife.com.hk/en/ace_fund-prices/index.aspx');

casper.waitForSelector('#btnAgree',
    function agreePage() {
        this.capture("acelife1.png");
        this.click('#btnAgree');
    },
    function timeout() {
        if (this.exists('table[id = "fundListTable"]')) {
            this.echo('accept already, continue...');
        } else {
            this.echo("agree page timeout.");
            this.bypass(99999);

            errorCode = exitCode.timeout_btnAgree;
            //this.exit(errorCode.timeout_btnAgree);
        }
    },
    5000);

casper.wait(5000);

casper.waitForSelector("table[id='fundListTable']",
    function listPage() {
        this.capture("acelife2.png");
        
        var sel = this.evaluate(
            function selProd(prodValue){
                var element = document.querySelector('#ddlInvestmentProduct');
                if (element.value != prodValue) {
                    element.value = prodValue;
                    var evt = document.createEvent('HTMLEvents');
                    evt.initEvent('change', false, true);
                    element.dispatchEvent(evt);
                }
            }, prodId);

        this.wait(5000, function chgProd() { 
            this.capture("acelife4.png");
            fs.write(resultfilename, this.page.content, 'w');
        });
    },
    function timeout() {
        this.echo("listPage timeout.");
        this.capture("acelife3.png");
        errorCode = exitCode.timeout_fundListTable;
    },
    5000);

casper.run(function end() {
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});
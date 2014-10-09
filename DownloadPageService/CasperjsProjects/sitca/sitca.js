var casper = require('casper').create({
    verbose: true,
    logLevel: "debug"
});

var utils = require("utils");
var fs = require('fs');

var exitCode = {
    "OK": 0,
    "pageTimeout": 1,
    "tableTimeOut": 2
};

var errorCode = exitCode.OK;

var terms = casper.cli.args;
utils.dump(terms);

if (terms.length === 0) {
    casper
		.echo('Usage: $ casperjs sitca.js prodId(1/2/3) resultFileName')
		.exit(1)
    ;
}

var prodId = terms[0] + "";
var resultFileName = terms[1];

var targetTableId = "#ctl00_ContentPlaceHolder1_Table" + prodId;

casper.start('http://www.sitca.org.tw/ROC/Industry/IN2105.aspx?pid=IN2213_01');

casper.waitForSelector("select[id = 'ctl00_ContentPlaceHolder1_ddlQ_Column']",
	function () {
	    this.capture('sitca01.png');
	    this.evaluate(function switchProdId(prodId) {
	        var element = document.querySelector('#ctl00_ContentPlaceHolder1_ddlQ_Column');
	        if (element.value != prodId) {
	            element.value = prodId;
	            var evt = document.createEvent('HTMLEvents');
	            evt.initEvent('change', false, true);
	            element.dispatchEvent(evt);
	        }
	    }, prodId);
	},
	function operationTimeOut() {
	    this.echo("Change-prodId request timeout.");
	    errorCode = exitCode.pageTimeout;
	}, 3000);

casper.then(function () {
    casper.click("input[name='ctl00$ContentPlaceHolder1$BtnQuery']");
    casper.wait(2000);
    this.capture('sitca02.png');
});


casper.waitForSelector(targetTableId,
	function () {

	    this.capture('stica03.png');
	    fs.write(resultFileName, this.page.content, 'w');
	},
	function tableTimeOut() {
	    this.capture('sitca04.png');
	    if (this.exists('table[id = targetTableId]')) {
	        this.echo('table found, continue...');
	    }
	    else {
	        this.echo("Table timeout.");
	        errorCode = exitCode.tableTimeOut;
	        this.bypass(99999);
	    }
	}, 5000);



casper.run(function end() {
    this.echo("ErrorCode is :" + errorCode);
    this.exit(errorCode);
});

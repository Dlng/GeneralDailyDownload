var casper = require('casper').create({
    verbose: true,
    logLevel: "debug",
    pageSettings: {
        loadImages: false,
        loadPlugins: true
    }
});

var exitCode = {
    "OK": 0,
    "timeout_btnAgree": 1,
    "timeout_showAllFunds": 2
};
var errorCode = exitCode.OK;

var utils = require('utils');
var fs = require("fs");

var terms = casper.cli.args;
utils.dump(terms);
if (terms.length === 0) {
    casper
        .echo("Usage: $ casperjs --ignore-ssl-errors=yes dws.com.sg.js resultfilename")
        .echo("www.dws.com.sg with SSL cert issue, must add '--ignore-ssl-errors=yes'")
        .exit(1)
    ;
}

var resultfilename = terms[0];

casper.start("https://www.dws.com.sg/Funds");

casper.wait(3000,
    function () {
        //this.capture('dws.com.sg.1.png');
        //Agree button
        this.click("#ctl00_ctl00_ctl07_ctl02_agreebutton");
    }); 

casper.waitForSelector("select[id='ctl00_ctl00_ctl07_SearchGrid_SearchResultGrid_TopPagination_ResultsPerPageDropdown']",
	function () {
	    //this.capture('dws.com.sg.2.png');
        //Show all funds by 150 funds per page
	    this.evaluate(function showAllFunds() {
	        var elementSize = document.querySelector('#ctl00_ctl00_ctl07_SearchGrid_SearchResultGrid_TopPagination_ResultsPerPageDropdown');
	        elementSize.value = 150;
	        var evt = document.createEvent('HTMLEvents');
	        evt.initEvent('change', false, true);
	        elementSize.dispatchEvent(evt);
	    });
	},
	function operationTimeOut() {
	    this.echo("showAllFunds request timeout.");
	    errorCode = exitCode.timeout_showAllFunds;
	}, 3000);

casper.wait(5000,
    function writeContent() {
        //this.capture('dws.com.sg.3.png');
        fs.write(resultfilename, this.page.content, 'w');
    });

casper.run(function end() {
    //this.capture('dws.com.sg.4.png');
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});
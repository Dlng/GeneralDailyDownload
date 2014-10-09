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
        .echo("Usage: $ casperjs --ignore-ssl-errors=yes DocumentPage.js pageUrl resultfilename")
        .echo("www.dws.com.sg with SSL cert issue, must add '--ignore-ssl-errors=yes'")
        .exit(1)
    ;
}

var pageUrl = terms[0];
var resultfilename = terms[1];

casper.start(pageUrl);

casper.wait(3000,
    function () {
        //this.capture('dws.com.sg.1.png');
        //Agree button
        //fs.write("page.html", this.page.content, 'w');
        this.click("#ctl00_ctl00_ctl07_ctl02_agreebutton");
    });

casper.wait(3000,
    function writeContent() {
        //this.capture('dws.com.sg.3.png');
        fs.write(resultfilename, this.page.content, 'w');
    });

casper.run(function end() {
    //this.capture('dws.com.sg.4.png');
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});
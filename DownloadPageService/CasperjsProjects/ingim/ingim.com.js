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
        .echo("Usage: $ casperjs ingim.com.js resultfilename")
        .exit(1)
	;
}

var resultfilename = terms[0];

//Download fund list in three pages
casper.start("https://www.ingim.com/SG/Funds/Fundinfo?reload");

//Disclamer window
casper.withFrame("GB_frame",
//casper.waitForUrl(/iss_loader_frame_html/,
//casper.waitForText("wish to continue to this site.",
//casper.waitForSelector("input[id='ACCEPT']",
    function agreebtn() {
    	this.page.switchToChildFrame(0);
    	this.echo("Frame url: " + this.page.url);
    	//this.capture('schroders.click.png');

    	this.click("#ACCEPT");
    	this.clickLabel("Proceed");

    	this.page.switchToParentFrame();
    	this.page.switchToParentFrame();
    },
    function agreebtnTimeout() {
    	errorCode = exitCode.timeout_btnAgree;
    	this.bypass(9999);
    },
    3000);

//assumble all fund list in 3 pages.
casper.wait(3000,
	function tableList() {
		var retString = "<Pages>";
		retString += "<Page id='1'>";
		retString += this.getHTML("#page", true);
		retString += "</Page>";

		this.clickLabel("Next");
		retString += "<Page id='2'>";
		retString += this.getHTML("#page", true);
		retString += "</Page>";

		this.clickLabel("Next");
		retString += "<Page id='3'>";
		retString += this.getHTML("#page", true);
		retString += "</Page>";
		retString += "</Pages>"
		fs.write(resultfilename, retString, 'w');
		fs.write("page.xml", retString, 'w');
	});

casper.run(function end() {
	this.capture('schroders.png');
	this.echo("ErrorCode: " + errorCode);
	this.exit(errorCode);
});
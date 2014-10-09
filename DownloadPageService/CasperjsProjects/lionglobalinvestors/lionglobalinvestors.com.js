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
        .echo("Usage: $ casperjs lionglobalinvestors.js pageid(0/1/2/3) resultfilename")
        .exit(1)
    ;
}

var pageid = terms[0];
var resultfilename = terms[1];

//var data = fs.read("cookies.txt")
//phantom.cookies = JSON.parse(data)

casper.start("http://www.lionglobalinvestors.com/en/funds/index.html#" + pageid);
//casper.start("http://www.lionglobalinvestors.com/en/funds/index.html");

casper.wait(3000,
    function () {
        this.clickLabel("-Select-");
        this.clickLabel("Singapore");
        this.capture('lionglobalinvestors.1.png');

    });

casper.wait(3000,
	function tableList() {
	    this.capture('lionglobalinvestors.2.png');
	    this.click("input[type=checkbox]");
	    this.click("input.disclaimer-btn-agree");
	});


var table1 = "";
var table2 = "";
var table3 = "";
var table4 = "";

casper.waitForSelector("a[class=next]",
	function agreeBtn() {
	    this.capture('lionglobalinvestors.3.png');

	    //var cookies = JSON.stringify(phantom.cookies);
	    //fs.write("cookies.txt", cookies, 644);
	    table1 = this.getHTML("table#tbl", true);

	},
    function timeout() {
    },
    5000);

 casper.wait(3000,
            function list(){
                this.click("a[class=next]");

                this.capture('lionglobalinvestors.4.png');

                table2 = this.getHTML("table#tbl", true);

            });

 casper.wait(3000,
            function list() {
                this.click("a[class=next]");

                this.capture('lionglobalinvestors.5.png');

                table3 = this.getHTML("table#tbl", true);
            });

casper.wait(5000,
    function selectLanguage() {
        var retString = "<Pages>";
        retString += "<Page id='1'>";
        retString += table1;
        retString += "</Page>";
        retString += "<Page id='2'>";
        retString += table2;
        retString += "</Page>";
        retString += "<Page id='3'>";
        retString += table3;
        retString += "</Page>";
        retString += "<Page id='4'>";
        retString += table4;
        retString += "</Page>";
        retString += "</Pages>"
        fs.write(resultfilename, retString, 'w');
        //fs.write("page.xml", retString, 'w');
    });


        //casper.waitForSelector("table[@id='tb1']",
        //    function agreebtn() {
        //        this.click("a[href='/en/Invest/Disclaimer.aspx?AcceptDisclaimer=true']");
        //    },
        //    function agreebtnTimeout() {
        //        errorCode = exitCode.timeout_btnAgree;
        //        this.bypass(9999);
        //    },
        //    3000);

        //casper.waitForSelector("#reportTable",
        //	function tableList() {
        //	    fs.write(resultfilename, this.page.content, 'w');
        //	},
        //	function timeOut() {
        //	    this.echo("tablePage timeout.");
        //	    errorCode = exitCode.timeout_fundListTable;
        //	}, 3000);

        casper.run(function end() {
            //this.capture('massmutual.png');
            this.echo("ErrorCode: " + errorCode);
            this.exit(errorCode);
        });
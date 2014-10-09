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
        .echo("Usage: $ casperjs manulife.js prodId(1/2/3/5/6/11) resultfilename")
        .exit(1)
    ;
}

var prodId = terms[0] + "";

var resultfilename = terms[1];

var link = '';

if (prodId == 1 || prodId == 2 || prodId == 3 || prodId == 11) {
    link = 'http://www.manulife.com.hk/_layouts/manulifedfp/en/FundList.aspx?fundcatid=' + prodId + '&buid=1categoryId=' + prodId;

}

if (prodId == 5 || prodId == 6) {
    link = 'http://www.manulife.com.hk/_layouts/manulifedfp/en/FundList.aspx?fundcatid=' + prodId + '&buid=2categoryId=' + prodId;
}

casper.start(link);

casper.waitForSelector("table[class = 'dfp4_sort_tbl tablesorter-default']", function () {
    fs.write(resultfilename, this.page.content, 'w');
},
function timeout() {
    this.echo("tablePage timeout.");
    errorCode = exitCode.timeout_fundListTable;
},
3000);

casper.run(function end() {
    this.echo("errorCode" + errorCode);
    this.exit(errorCode);
});



var casper = require('casper').create({
    verbose: true,
    logLevel: "debug",
});

var exitCode = {
    "OK": 0,
    "timeout_fancyboxPopup": 1
};
var errorCode = exitCode.OK;

var utils = require('utils');
var fs = require("fs");


var terms = casper.cli.args;
utils.dump(terms);
if (terms.length === 0) {
    casper
        .echo("Usage: $ casperjs BNP.fancy.js Link-id resultfilename")
        .exit(1)
    ;
}

var id = terms[0];
var str = id.split("-");
var umbrellaId = str[2];
var compartmentId = str[3];
var shareId = str[4];
//var docSubType = terms[3]; judgement done in C#.
var docSubType = str[5];
var resultfilename = terms[1];


var fancyUrl = "http://www.bnpparibas-ip.sg/central/fundsheet/documents.page?l=eng&p=IP_SG-NSG&umbrellaId=" + umbrellaId + "&compartmentId=" + compartmentId + "&shareId=" + shareId + "&docType=&docSubType=" + docSubType + "&docLang=ENG&otherDoc=Y";

casper.start(fancyUrl);

casper.waitForSelector('#fundsheet_docu_list',
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


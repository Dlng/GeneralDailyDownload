var casper = require('casper').create({
    verbose: true,
    logLevel: "debug",
});


var exitCode = {
    "OK": 0,
    "timeout_fundTable": 1,
};
var errorCode = exitCode.OK;

var utils = require('utils');
var fs = require("fs");

var terms = casper.cli.args;
utils.dump(terms);
if (terms.length === 0) {
    casper
        .echo("Usage: $ casperjs sunlife.js planName resultfilename")
        .exit(1)
    ;
}

var planName = terms[0];
var resultfilename = terms[1];
var link = '';
if (planName == 'SunFuture')
{
    link = 'http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/SunFuture?vgnLocale=en_CA';
}
else if(planName == 'Star')
{
    link = 'http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/Star+Select+Investment+Plan?vgnLocale=en_CA';
}
else if(planName == 'Rainbow')
{
    link = 'http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/Rainbow+Saver_Rainbow+Investor?vgnLocale=en_CA';
}
else if(planName == 'FORTUNE')
{
    link = 'http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/FORTUNE?vgnLocale=en_CA';
}
else if (planName == 'ANNUITY_100_Retirement_Plan')
{
    link = 'http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/ANNUITY+100+Retirement+Plan?vgnLocale=en_CA';
}
else if (planName == 'SunWealth') {
    link = 'http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/SunWealth?vgnLocale=en_CA';
}
else if (planName =='FORTUNE_Builder')
{
    link = 'http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/FORTUNE+builder?vgnLocale=en_CA';
}

else
 {
    link ='';
}


casper.start(link);
//'http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/SunFuture?vgnLocale=en_CA');
//http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/SunWealth?vgnLocale=en_CA
//http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/ANNUITY+100+Retirement+Plan?vgnLocale=en_CA
//http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/FORTUNE?vgnLocale=en_CA
//http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/Rainbow+Saver_Rainbow+Investor?vgnLocale=en_CA
//http://www.sunlife.com.hk/hongkong/Information+Centre/Fund+prices+%26+performance/Investment-linked+insurance+plans/Star+Select+Investment+Plan?vgnLocale=en_CA




casper.waitForSelector(".data-sheet", function () {
    fs.write(resultfilename, this.page.content, 'w');
},
	function pageTimeOut() {
	    this.echo("tablePage timeout.");
	    errorCode = exitCode.timeout_fundTable;
	},
	7000);


casper.run(function end() {
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});
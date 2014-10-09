var casper = require('casper').create({
    verbose: true,
    logLevel: "debug",
});

var utils = require('utils');
var fs = require("fs");

var exitCode = {
    "OK": 0,
    "ParameterError": 100,
    "unkown": 200
};
var errorCode = exitCode.OK;

var terms = casper.cli.args;
utils.dump(terms);
if (terms.length < 5) {
    casper
        .echo("Usage: $ casperjs doc.t57sb01.js type(E/D) companyid(A00037) year(103) docfilter(nofilter|file1;file2;...) resultfilename")
        .exit(exitCode.ParameterError);
}

var docType = terms[0];
var companyId = terms[1];
var year = terms[2];
var YEAR = 1911 + parseInt(year);

var spacifiedLinks = [];
var checkLink = false;
if(terms[3] != "nofilter"){
    checkLink = true;
    spacifiedLinks = terms[3].split(";");
    //utils.dump(checkLink);
}

var resultfilename = terms[4];

var url = "http://doc.twse.com.tw/server-java/t57sb01?step=1&colorchg=1&co_id=" + companyId + "&year=" + year + "&month=&mtype=" + docType + "&";

//casper.start('http://doc.twse.com.tw/server-java/t57sb01?step=1&colorchg=1&co_id=A00037&year=103&month=&mtype=E&');
//casper.start('http://doc.twse.com.tw/server-java/t57sb01?step=1&colorchg=1&co_id=A00025&year=103&month=&mtype=E&');
casper.start(url);
casper.then(function () {

    this.capture('doc.t57sb01.01.png');

    var rowsOnPage = this.evaluate(function () {
        var t = document.querySelector("table[bordercolor='#ff6600']");
        var tbody = t.childNodes[1];
        var ret = [];
        for (var i = 1; i < tbody.rows.length; i++) {
            var trCells = tbody.rows[i].cells;

            var fundname = trCells[5].textContent;
            if (fundname.indexOf('ETF') > 0)
                fundname = fundname.substring(0, 23);//ugly hardcode

            var row = {
                "id": trCells[7].textContent,
                "name": fundname,
                "size": trCells[8].textContent,
                "url": ""
            };
            ret.push(row);
        }
        return ret;
    });
    this.echo('rowsOnPage: ' + rowsOnPage.length);

    var links = [];
    var retDocNames = [];
    for (x in rowsOnPage) {
        var link = rowsOnPage[x];
        var download = false;

        //utils.dump(link.id);
        if (checkLink) {
            for (i in spacifiedLinks) {
                if (link.id == spacifiedLinks[i]) {
                    download = true;
                    break;
                }
            }
        }
        else {
            download = true;
        }

        if (download) {
            links.push(link);
            retDocNames.push(link.id + "," + link.name);
            utils.dump(link.id);
        }
    }
    this.echo('download links: ' + links.length);

    this.then(function returnlist() {
        
        if (!checkLink) {
            fs.write(resultfilename, retDocNames.join(";"), 'b');
            this.echo("ErrorCode: " + errorCode);
            this.exit(errorCode);
        }
    });

    casper.each(links, function (self, link) {
        try {


            this.wait(3000, function () {
                this.echo("  ");
                this.clickLabel(link.id, 'a');
                utils.dump(link.id);
                this.echo('popups.length after click: ' + this.popups.length);

                this.waitForPopup(/t57sb01$/,
                    function fine() {
                        //searching in popup page...
                        this.withPopup(/t57sb01$/, function () {
                            this.echo("Popup url: " + this.page.url);
                            this.echo("Popup title: " + this.page.title);

                            var pdfLink = this.getElementsAttribute("a", "href");

                            if (pdfLink.length == 0) {
                                this.echo("Server is busy." + link.id);
                                //utils.dump(this.page);
                            }
                            else {
                                link.url = pdfLink[0];
                                this.echo("PDF Link: " + pdfLink);
                            }

                            //try to close the popup page...
                            //this.echo('popups.length: ' + this.popups.length);
                            this.page.close();
                            this.popups.clean(this.page);
                            this.echo('popups.length: ' + this.popups.length);
                        });
                    },
                    function timeout() {
                        //try to close all popups
                        this.echo('popups.length: ' + this.popups.length);
                        var len = this.popups.length;
                        for (var k = len - 1; k >= 0 ; k--) {
                            var page = this.popups[k];
                            page.close();
                            this.popups.clean(page);
                        }

                        utils.dump(this.popups);
                        this.echo("PDF Link: time-out for clickLabel. popups.length: " + this.popups.length);
                    },
                    10000);

            });
        } catch (err) {
            console.log(err);
            errorCode = exitCode.unkown;
        } finally {
        }
    });
    
    this.then(function returnlink() {
        utils.dump(links);

        retText = [];
        retText.push("<res>");
        for (i in links) {
            link = links[i];
            if (link.url.length > 0) {
                //retText.push("<a href='http://doc.twse.com.tw" + link.url + "'>" + link.id + "</a></br>");
                retText.push("<doc id='" + link.id + "' >http://doc.twse.com.tw"+ link.url + "</doc>");
            }
            else {
                //retText.push(link.id + "</br>");
                retText.push("<doc id='" + link.id + "' />");
            }
        }
        retText.push("</res>");

        var fs = require("fs");
        //fs.write("doc.t57sb01.html", retText.join("\n"), 'b');
        fs.write(resultfilename, retText.join("\n"), 'b');
    });

});

casper.run(function end() {
    this.echo("ErrorCode: " + errorCode);
    this.exit(errorCode);
});

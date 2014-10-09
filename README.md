GeneralDailyDownload
====================

GeneralDailyDownload

Summary:
A Web-Crawling tool with automated report generation and runlog database.

Built in Phantomjs, Casperjs, C# and XSLT-FO.


Functionality:
HTML data fetched by casperjs would be sent back to C# command line program, in which task settings would be loaded from a config 

file, and data would be filtered (invalid xml syntax),

transformed into xml, validated and extracted based on XSLT-FO into a csv file.

The csv file would be emailed to target email list.

The automation is achieved through windows task scheduler running on a server.



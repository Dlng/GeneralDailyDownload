using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Linq;
using WebCrawCommon;
using System.Xml.XPath;

namespace SitcaMonitor
{
    class Program
    {
        static void Main(string[] args)
        {

            //string htmlData = File.ReadAllText("d:\\AXA1.htm");
            string htmlData = File.ReadAllText("d:\\中華民國證券投資信託暨顧問商業同業公會.htm");
            //string testPath = @"C:\Users\wwu\WebCrawlHelper\GeneralDailyDownload\bin\Debug\htmlFile.txt";
            //string htmlData = File.ReadAllText(testPath);


            string xmlData = PageParser.ToXml(htmlData);

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xmlData);
            
            //doc.Save("d:\\AXA2.xml");
            //doc.Load("d:\\AXA2.xml");

            string locator = "//table[@id='ctl00_ContentPlaceHolder1_Table1']/tr";
            //locator = "//div[@class='fundprofile-document-content']/span/text()";
            //locator = "//div[@class='fundprofile-document-content']/a/@href";

            XmlNodeList nodes = doc.SelectNodes(locator);
            foreach (XmlNode node in nodes)
            {
                string _idExpression = "./td[2]";
                XmlNode idnode = node.SelectSingleNode(_idExpression);

                string _attribute = "./td[1]";
                XmlNode attr = node.SelectSingleNode(_attribute);

            }


        }
    }
}

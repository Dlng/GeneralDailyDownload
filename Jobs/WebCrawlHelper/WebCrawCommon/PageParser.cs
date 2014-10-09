using MorningstarAWD.Common;
using MorningstarAWD.Common.Sgml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace WebCrawCommon
{
    public static class PageParser
    {
        public static string Parse(string sourceHtml, string xsltUri=null, XsltArgumentList args = null)
        {
            //Cleanse
            string cSourceHtml = CleanseSourceHtml(sourceHtml);

            string resultData = null;
            if (string.IsNullOrWhiteSpace(xsltUri))
            {
                //Directly return if no xslt for transform
                resultData = cSourceHtml;
            }
            else
            {
                //Wrap the source xml for transform
               string xmlData = ToXml(cSourceHtml);
                XslCompiledTransform xslt = CreateXsltTransform(xsltUri);

                //Transform
                string resultXml = TransformWithXslt(xmlData, xslt, args);

                //Unwrap to get the actual result xml
                XElement xResultData = XElement.Parse(resultXml).Element("html");
                xResultData = xResultData ?? XElement.Parse(resultXml).Element("HTML");
                resultData = xResultData == null ? cSourceHtml : xResultData.ToString();
            }

            return resultData;
        }

        private static string CleanseSourceHtml(string sourceHtml)
        {
            //Directly return if html source is empty
            if (string.IsNullOrWhiteSpace(sourceHtml))
            {
                return sourceHtml;
            }

            //Remove chars in front of the node 'html'
            string result = null;
            int htmlStartPosition = sourceHtml.IndexOf("<html", StringComparison.OrdinalIgnoreCase);
            if (htmlStartPosition > 0)
            {
                result = sourceHtml.Substring(htmlStartPosition);
            }
            else
            {
                result = sourceHtml;
            }

            return result;
        }

        public static string ToXml(string htmlData)
        {
            //Cleanse
            htmlData = CleanseSourceHtml(htmlData);
            //File.WriteAllText("d:\\Sitca.html", htmlData);
            string xmlData = null;
            using (SgmlReader sgmlReader = new SgmlReader())
            {
                //sgmlReader.DocType = "HTML";
                sgmlReader.InputStream = new StringReader(htmlData);
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlWriter xmlWriter = new XmlTextWriter(sw))
                    {
                        while (!sgmlReader.EOF)
                        {
                            xmlWriter.WriteNode(sgmlReader, true);
                        }
                    }

                    xmlData = sw.ToString();
                    xmlData = xmlData.Replace("xmlns=\"http://www.w3.org/1999/xhtml\"", "");
                    
                }
            }



            //return "<?xml version = '1.0' encoding = 'utf-8'?>" + xmlData;
            return xmlData;
        }

        public static string TransformWithXslt(string sourceXml, XslCompiledTransform xslTransform, XsltArgumentList args)
        {
            StringBuilder targetXml = new StringBuilder();

            using (XmlReader sourceXmlReader = new XmlTextReader(new StringReader(sourceXml)))
            {
                XElement ele = XElement.Load(sourceXmlReader);
                using (StringWriter sw = new StringWriter(targetXml))
                {
                    xslTransform.Transform(ele.CreateReader(), args, sw);
                }
            }

            return targetXml.ToString();
        }

        public static XslCompiledTransform CreateXsltTransform(string xsltUri)
        {
            string validUri = AWDEnvironment.GetRawPathInAppDomain((xsltUri));

            XmlDocument xsltDoc = new XmlDocument();
            xsltDoc.Load(validUri);

            XsltSettings xsltSetting = new XsltSettings(true, true);
            XmlResolver resolver = new XmlUrlResolver();
            XslCompiledTransform xslTransform = new XslCompiledTransform(true);

            xslTransform.Load(xsltDoc.CreateNavigator(), xsltSetting, resolver);

            return xslTransform;
        }
    }
}

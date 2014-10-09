using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralDailyDownload.CasperjsWebService;
//using MorningstarAWD.Common.Sgml;

using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Linq;
using WebCrawCommon;
using Handler.Net;
using System.Configuration;


namespace GeneralDailyDownload
{
    public enum DownloadErrorCode
    {
        NoError = 0,
        PartialError = 10,

        //NoValidation = 40 merged into NoError
        
    }
    public class DownloadProject
    {
        XmlNode _projectSettings;
        string _outputBaseFolder;
        string _projectName;
        //string _str;
        public DownloadErrorCode err = DownloadErrorCode.NoError;
        public DownloadProject(XmlNode projectNode, string outputfolder)
        {
            _projectSettings = projectNode;
            _outputBaseFolder = outputfolder;
            _projectName = _projectSettings.Attributes["name"].Value;
            //_taskName = _projectSettings.Attributes{};
        }

        public void Start()
        {
            //email settings
            List<string> attachmentPathList = new List<string>();
            string subject;
            string content=null;
            string errMsg = null;
            byte status = 0;
            DateTime startTime = DateTime.Now;
            try
            {

                XmlNodeList tasksNode = _projectSettings.SelectNodes("./tasks/task");
                foreach (XmlNode node in tasksNode)
                {
                    DownloadTask task = new DownloadTask(node, _outputBaseFolder);
                    if (task.Run())
                    {
                        if (task.LastErrorMsg == "No validation is required.")
                        {
                            errMsg += string.Format("\t\tError Task:\t{0}\r\n\t\t\tError Code is:\t{1}\t\r\n\t\t\tError Message is:\t{2} \r\n\r\n", task.taskName, task.LastErrorCode, task.LastErrorMsg);
                        }
                        //get attachments
                        attachmentPathList.Add(task.OutputPath);
                    }
                    else
                    {
                        //err = task.LastErrorCode;
                        //errMsg = task.LastErrorMsg;
                        err = DownloadErrorCode.PartialError;
                        errMsg += string.Format("\t\tError Task:\t{0}\r\n\t\t\tError Code is:\t{1}\t\r\n\t\t\tError Message is:\t{2} \r\n\r\n", task.taskName, task.LastErrorCode, task.LastErrorMsg);

                        continue;
                    }
                }

                DateTime endTime = DateTime.Now;

                /* this snippet was overrided by WriteWebCrawlRunLog
                               * 
                              //Step: Write run log
                              //Guid id = Guid.NewGuid();
                              //string type = _projectSettings.SelectSingleNode("//runLog/type").InnerText;
                              //string url = "http://www.acelife.com.hk/en/ace_fund-prices/index.aspx/";
                              //WriteACERunLog(id, type, url, startTime, endTime);
                               */

                //Step: send emails here
                if (attachmentPathList.Count != 0)
                {
                    //if (err == DownloadErrorCode.NoError || err == DownloadErrorCode.PartialError)
                    //{
                        //Send email result
                        string mailto = _projectSettings.SelectSingleNode("./email/mailto").InnerText;
                        string mailcc = _projectSettings.SelectSingleNode("./email/mailcc").InnerText;
                        subject = _projectSettings.SelectSingleNode("./email/subject").InnerText;
                        content = "It is sent from GeneralDailyDownload.CAS.\r\n\r\n";
                        content += string.Format("\tProject Name:\t{0}\r\n\tAttachments:\t{1}\r\n",
                                                    _projectName, attachmentPathList.Count);
                        content += string.Format("\tError Status:\t{0}\r\n", err.ToString());
                        content += string.Format("\tError Message Content:\r\n{0}", errMsg);
                        EmailSender.SendEmail(mailto, mailcc, attachmentPathList, subject, content, false);
                    //}
                    // ELSE? No need
                }
                else
                {
                    //status = 1; indicates failure
                    status = 1;
                    //Send error report email here
                    Console.WriteLine("Sending error report..");
                    string mailto = _projectSettings.SelectSingleNode("./errorreport/mailto").InnerText;
                    string mailcc = _projectSettings.SelectSingleNode("./errorreport/mailcc").InnerText;
                    subject = _projectSettings.SelectSingleNode("./errorreport/subject").InnerText;
                    content += string.Format("\tError Status:\t{0}\r\n", err.ToString());
                    content += string.Format("\tError Message Content:\r\n\t\t{0}", "Failure");
                    EmailSender.SendEmail(mailto, mailcc, attachmentPathList, subject, content, false);
                    Console.WriteLine("Error report sent!");
                }
               
            }
            catch (Exception e)
            {
                //status=1 indicates failure.
                status = 1;
                //send error email here
                string mailto = _projectSettings.SelectSingleNode("./errorreport/mailto").InnerText;
                string mailcc = _projectSettings.SelectSingleNode("./errorreport/mailcc").InnerText;
                subject = _projectSettings.SelectSingleNode("./errorreport/subject").InnerText;
                content = e.ToString();
                errMsg = content;

                EmailSender.SendEmail(mailto, mailcc, attachmentPathList, subject, content, false);
                //e.Message;
            }
            finally
            {
                this.WriteWebCrawlRunLog(_projectName, startTime, DateTime.Now, Environment.UserName, status, errMsg);
            }
        }

        public void WriteWebCrawlRunLog(string projectName, DateTime StartTime, DateTime endTime, string requestedBy, byte status, string errorMsg)
        {
            XElement xeLog = new XElement("Log",
                                new XElement("ProjectName", projectName),
                                new XElement("StartTime", StartTime.ToString()),
                                new XElement("EndTime", endTime.ToString()),
                                new XElement("RequestedBy", requestedBy),
                                new XElement("Status", status),
                                string.IsNullOrEmpty(errorMsg)?null:new XElement("ErrorMsg", errorMsg)
                                );

            

            XElement xeLogData = new XElement("LogData", xeLog);

            RunLogWriter.Write("WebCrawl", xeLogData.ToString());
        }

    }   
 
    class DownloadTask // 不写public/private是默认为只可以在当前namespace下调用
    {
 
        string _outputPath;
        public string OutputPath
        {
            get { return _outputPath; }
        }

        //?
        //DownloadErrorCode _errCode;
        //public DownloadErrorCode LastErrorCode
        //{
        //    get { return _errCode; }
        //}


       public enum DownloadTaskErrorCode
        {
            //Task validation result 
            NoError = 0,
            NoNodesSelected = 10,
            ChangeInNumOfColumns = 20,
            ChangeInOrderOfColumns = 30,
            NoValidation = 40,
            ExtractDataFailed = 50,
            BuildOutputFailed = 60
        }

        //public DownloadTaskErrorCode _taskErrorCode = DownloadTaskErrorCode.NoError;

        DownloadTaskErrorCode _taskErrorCode;
        // get set ?
        public DownloadTaskErrorCode LastErrorCode //只设置set是将该变量定位只读变量（对声明这个变量的class以外的class来说）
        {
            get { return _taskErrorCode; }
        }
        string _lastErrorMsg;
        public string LastErrorMsg
        {
            get { return _lastErrorMsg; }
        }

        string _outputBase;
        
        XmlNode _taskSettings;
        XmlDocument _pageContent = new XmlDocument();

        //XElement xPageContent = null;

        public string projectName ="";
        public string scriptName = "";
        public string taskName = "";
        public string paraStr = "";
        public DownloadTask(XmlNode taskNode, string outputFolder)
        {
            _taskSettings = taskNode;
            _outputBase = outputFolder;
            projectName = _taskSettings.SelectSingleNode("./downloadpage/projectname").InnerText;
            scriptName = _taskSettings.SelectSingleNode("./downloadpage/scriptname").InnerText;
            paraStr = _taskSettings.SelectSingleNode("./downloadpage/parastr").InnerText;
            taskName = _taskSettings.Attributes["name"].Value.ToString();
        }

        public bool Run()
        {
            //Step: find 
            Console.WriteLine("Start getting html data");

            string htmlData = DownloadPage(projectName, scriptName, paraStr);
            //
            //string path = @"C:\Users\wwu\Desktop\temp.txt";
            //string path2 = @"C:\Users\wwu\Desktop\temp2.txt";
            //string htmlData = File.ReadAllText(path);
            Console.WriteLine("Html data get.");

            //Step: transform
            Console.WriteLine("Transform html to xml.");


            
            //File.WriteAllText(path, htmlData);


            // xmlData is empty... .Werid, fixed by itself
            string xmlData = PageParser.ToXml(htmlData);
            xmlData = xmlData.Replace("xmlns=\"http://www.w3.org/1999/xhtml\"", string.Empty);

            //File.WriteAllText(path2, xmlData);

            Console.WriteLine("Transformation from html to xml completed.");
            //string xmlData = File.ReadAllText(path2);
            //Step: validate
            Console.WriteLine("Start to validate xmlData.");
            _pageContent.LoadXml(xmlData);
            
            ////testing xslt
            //XmlNodeList testnode = _pageContent.SelectNodes(".//descendant::table[@id='viewns_Z7_01A41A42IGEOE0A0S3EDN81020_:mainContent:datatable1']/descendant::tr[position()>3]");

            //var nnnnn = (from xTable in xPageContent.Descendants("table")
            //             where xTable.Attribute("id")!=null&&xTable.Attribute("id").Value == "viewns_Z7_01A41A42IGEOE0A0S3EDN81020_:mainContent:datatable1"
            //             select xTable).ToArray();

            //XElement[] xtestNodes = (from xTable in xPageContent.Descendants("table")
            //                         from xTr in xTable.Descendants("tr")
            //                         where xTable.Attribute("id") != null && xTable.Attribute("id").Value == "viewns_Z7_01A41A42IGEOE0A0S3EDN81020_:mainContent:datatable1"
            //                select xTr).Skip(3).ToArray();


            //foreach (var xtn in xtestNodes)
            //{
            //    string v = xtn.Value;
            //}

            if (!ValidateDataForm())
            {
                Console.WriteLine("Invalid Data Form");
                return false;
            }

            Console.WriteLine("Valid Data Form.");

            Console.WriteLine("Start to extract xmlData.");
            //Step: Extract data and save as output
            //string csvText = ExtractData(xmlData);  
            _outputPath = _taskSettings.SelectSingleNode("./output").InnerText;
            string result = ExtractData(xmlData);
            Console.WriteLine("Extraction Done.");

            _outputPath = BuildOutput(result);
            if (string.IsNullOrEmpty(_outputPath))
            {
                return false;
            }

            return true;
        }




        virtual protected string DownloadPage(string projectName,string scriptName,string paraStr)
        {
            DownloadPageAPI downloadPageAPI = new DownloadPageAPI();
            string htmlString = downloadPageAPI.GetTextByCasperjs(projectName, scriptName, paraStr);
            return htmlString;
        }

        virtual protected bool ValidateDataForm()
        {
            //
            _taskErrorCode = DownloadTaskErrorCode.NoError;
            _lastErrorMsg = "";
            //
            string configfile = ConfigurationManager.AppSettings["ProjectConfigFile"];
            XmlDocument config = new XmlDocument();
            config.Load(configfile);

            //string str = "dailydownload/project/tasks/task[@name='" + taskName + "']";
            //XmlNode nTask = config.SelectSingleNode("dailydownload/project/tasks/task[@name='"+taskName+"']");

            string xpath = config.SelectSingleNode("dailydownload/project[@name = '"+projectName+"']/tasks/task[@name='"+taskName+"']/validation/table").Attributes["xpath"].Value.ToString();
            string xpathForColumns = "./validation/table[1]/column";

            //string xpath = "//table[@id='fundListTable']/tbody/tr[1]/th";
            //string xpathForColumns = "./validation/table[@id='fundListTable']/column";
            XmlNodeList dataPoints;
            if ("" == xpath)
            {
                dataPoints = null;
            }
            else
            {
                dataPoints = _pageContent.SelectNodes(xpath);
            }
            
            XmlNodeList tableColumns = _taskSettings.SelectNodes(xpathForColumns);
            if (0 == tableColumns.Count)
            {
                _taskErrorCode = DownloadTaskErrorCode.NoValidation;
                _lastErrorMsg = "No validation is required.";
                return true;
            }
            if (null == dataPoints[0])
            {
                _taskErrorCode = DownloadTaskErrorCode.NoNodesSelected;
                _lastErrorMsg = "No Nodes Selected." ;
                return false;
            }
            else if (dataPoints.Count != tableColumns.Count)
            {
                _taskErrorCode = DownloadTaskErrorCode.ChangeInNumOfColumns;
                _lastErrorMsg = "Num of Columns has been changed.";
                return false;
            }
            else
            {
                int i = 0;
                foreach (XmlNode node in dataPoints)
                {
                    if (node.InnerText.Trim().Replace("&nbsp;","").Replace("\n","\\n") == tableColumns[i].InnerXml && i < tableColumns.Count)
                    {
                        i++;
                        continue;
                    }
                    else
                    {
                        _taskErrorCode = DownloadTaskErrorCode.ChangeInOrderOfColumns;
                        _lastErrorMsg = "Order of Columns has been changed.";
                        return false;
                    }
                }
                return true;
            }
 
        }

        virtual protected string ExtractData(string xmlData)
        {
            try
            {
                //transform realtive path to definate path
                string pathForXslt = _taskSettings.SelectSingleNode("./transfertemplate").InnerText;
                string fullPathForXslt = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xslt", projectName, pathForXslt);
                //string fullPathForXslt = @"C:\Users\wwu\WebCrawCommon\WebCrawlHelper\GeneralDailyDownload\xslt\acelife.xslt";

                string resultText = null;
                StringBuilder sb = new StringBuilder();
                using (StringReader strReader = new StringReader(xmlData))
                {
                    XmlReader xmlReader = XmlReader.Create(strReader);
                    StringWriter writer = new StringWriter(sb);

                    XsltSettings xsltSetting = new XsltSettings(true, true);

                    XslCompiledTransform transformToCSV = new XslCompiledTransform();
                    XmlResolver resolver = new XmlUrlResolver();
                    transformToCSV.Load(fullPathForXslt, xsltSetting, resolver);
                    transformToCSV.Transform(xmlReader, null, writer);

                    resultText = sb.ToString();
                }
                

                return resultText;
            }
            catch(Exception ex)
            {
                _taskErrorCode = DownloadTaskErrorCode.ExtractDataFailed;
                _lastErrorMsg = ex.Message + ex.StackTrace;
                return null;
            }
        }

        virtual protected string BuildOutput(string resultText)
        {
            try
            {
                string outputFile = _taskSettings.SelectSingleNode("./output").InnerText;
                string outputpath = Path.Combine(_outputBase, projectName, DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(outputpath))
                {
                    Directory.CreateDirectory(outputpath);
                }

                string filepath = Path.Combine(outputpath, outputFile);
                //string filepath = @"C:\Users\wwu\WebCrawCommon\WebCrawlHelper\GeneralDailyDownload\xslt\xslt\ACE_ WealthMaster Variable Universal Life.csv";

                File.WriteAllText(filepath, resultText, Encoding.UTF8);
                return filepath;
            }
            catch (Exception ex)
            {
                _taskErrorCode = DownloadTaskErrorCode.BuildOutputFailed;
                _lastErrorMsg = ex.Message + ex.StackTrace;
                return null;
            }
        }
    }
}

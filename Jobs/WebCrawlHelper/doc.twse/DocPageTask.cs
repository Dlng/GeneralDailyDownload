using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Net;
using doc.twse.DownloadPageService;


namespace doc.twse
{
    public class DocPageTask
    {
        int _id;
        public int Id
        {
            get { return _id; }
        }

        string _docType;
        public string DocType
        {
            get { return _docType; }
        }

        string _pageURL;
        public string PageURL
        {
            get { return _pageURL; }
        }

        string _companyId;
        public string CompanyId
        {
            get { return _companyId; }
        }

        int _year;
        public int Year
        {
            get { return _year; }
        }

        string _downloadFolder;
        public string DownloadFolder
        {
            get { return _downloadFolder; }
        }

        int _docCountDownloaded;
        public int DocCountDownloaded
        {
            get { return _docCountDownloaded; }
        }

        int _docCountNeedDownload;
        public int DocCountNeedDownload
        {
            get { return _docCountNeedDownload; }
        }

        int _docCountOnPage;
        public int DocCountOnPage
        {
            get { return _docCountOnPage; }
        }

        int _docCountUrlReady;
        public int DocCountUrlReady
        {
            get { return _docCountUrlReady; }
        }

        List<string> _months;
        public List<string> FilterMonth
        {
            get { return _months; }
        }

        Dictionary<string, DocDownloadTask> _docDownloadTasks;
        public Dictionary<string, DocDownloadTask> DownloadTasks
        {
            get { return _docDownloadTasks; }
        }

        DownloadTaskHandler _downloadingHandler;

        public DocPageTask(int id, string docType, string pageURL, string companyId, int year,
                            List<string> filterMonths, string downloadFolder, DownloadTaskHandler handler)
        {
            _id = id;
            _docType = docType;
            _pageURL = pageURL;
            _companyId = companyId;
            _year = year;
            _months = filterMonths;
            _downloadFolder = downloadFolder;
            _downloadingHandler = handler;

            _docCountNeedDownload = -1;
            _docCountDownloaded = -1;
            _docCountOnPage = -1;
            _docCountUrlReady = -1;

            _docDownloadTasks = new Dictionary<string, DocDownloadTask>();
        }

        public int ScanDocument()
        {
            if (_docCountNeedDownload == -1)
            {
                DownloadPageAPI downloadAPI = new DownloadPageAPI();
                string paraStr = string.Format("{0} {1} {2} ", DocType, CompanyId, Year);
                string res = downloadAPI.GetTextByCasperjs("doc.twse", "doc.t57sb01.js", paraStr + "nofilter");
                Debug.WriteLine(res);

                _docCountOnPage = 0;
                _docCountNeedDownload = 0;
                _docCountUrlReady = 0;
                _docCountDownloaded = 0;
                
                string[] pdfs = res.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                _docCountOnPage = pdfs.Length;
                foreach (string documentPdf in pdfs)
                {
                    bool needDownload = false;

                    string[] idname = documentPdf.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (idname.Length == 2)
                    {
                        if (FilterMonth.Count > 0)
                        {
                            foreach (string month in FilterMonth)
                            {
                                string docYearMonth = (Year + 1911).ToString() + month;
                                if (idname[0].StartsWith(docYearMonth))
                                {
                                    needDownload = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            needDownload = true;
                        }

                        if (needDownload)
                        {
                            DocDownloadTask task = new DocDownloadTask(idname[0], idname[1], _docType, _companyId, _downloadFolder, paraStr, _downloadingHandler);
                            _docDownloadTasks.Add(task.DocId, task);
                            _docCountNeedDownload++;

                            _downloadingHandler.AddDownloadTask(task);
                        }
                    }
                }
            }

            return _docCountNeedDownload;
        }
    }

    public class DocDownloadTask
    {
        string _docId;
        public string DocId
        {
            get { return _docId; }
        }

        string _name;
        public string FundName
        {
            get { return _name; }
        }

        string _docUrl;
        public string DocUrl
        {
            get { return _docUrl; }
        }

        DocDownloadStatus _status;
        public DocDownloadStatus Status
        {
            get { return _status; }
        }

        string _docType;
        public string DocType
        {
            get { return _docType; }
        }

        string _companyId;
        public string CompanyId
        {
            get { return _companyId; }
        }

        int _downloadFailedTimes;
        public int DownloadFailedTimes
        {
            get { return _downloadFailedTimes; }
            set { _downloadFailedTimes = value; }
        }

        string _percentage;
        public string Percentage
        {
            get { return _percentage; }
            set { _percentage = value; }
        }

        string _downloadFolder;
        

        string _lastError;
        public string LastError
        {
            get { return _lastError; }
        }

        string _casperPara;
        DownloadTaskHandler _downloadingHandler;

        public DocDownloadTask(string docId, string name, string docType, string companyId, string downloadFolder, string para, DownloadTaskHandler handler)
        {
            _docId = docId;
            _name = name;
            _casperPara = para;
            _downloadingHandler = handler;
            _docType = docType;
            _companyId = companyId;
            _downloadFolder = downloadFolder;


            _status = DocDownloadStatus.New;
            _downloadFailedTimes = 0;
            _percentage = "";
        }

        public DocDownloadStatus RefreshUrl()
        {
            if (IsDownoaded())
            {
                _status = DocDownloadStatus.Downloaded;
                return _status;
            }

            DownloadPageAPI downloadAPI = new DownloadPageAPI();
            string reslink = downloadAPI.GetTextByCasperjs("doc.twse", "doc.t57sb01.js", _casperPara + " " + _docId);
            if (!reslink.StartsWith("<res>"))
            {
                //Todo: why return ""?
                Debug.WriteLine(reslink);
            }

            try
            {
                //Debug.WriteLine(reslink);
                XmlDocument resXml = new XmlDocument();
                resXml.LoadXml(reslink);

                if (resXml != null)
                {
                    string xpath = string.Format("/res/doc[@id='{0}']", _docId);
                    XmlNode node = resXml.SelectSingleNode(xpath);
                    if (node != null)
                    {
                        string url = node.InnerText;
                        if (string.IsNullOrEmpty(url))
                        {
                            _status = DocDownloadStatus.ServerBusy;
                            Debug.WriteLine("ServerBusy : " + _docId);
                        }
                        else
                        {
                            _docUrl = url;
                            _status = DocDownloadStatus.UrlReady;
                            //_downloadingHandler.AddDownloadTask(this);
                            Debug.WriteLine("Document URL: " + url);
                        }
                    }
                }
                else
                {
                    _status = DocDownloadStatus.RefreshUrlFailed;
                }
            }
            catch (Exception e)
            {
                _status = DocDownloadStatus.RefreshUrlFailed;
                _lastError = e.Message;
                Debug.WriteLine(_lastError + reslink);
            }
            return _status;
        }

        private bool IsDownoaded()
        {
            string path = _downloadFolder + "\\" + _docId;
            if (!string.IsNullOrEmpty(_downloadFolder))
            {
                return File.Exists(path);
            }
            return false;
        }

        public DocDownloadStatus Download()
        {
            Debug.WriteLine(string.Format("Downloaded: {0}", _docId));

            if (IsDownoaded())
            {
                _status = DocDownloadStatus.Downloaded;
                return _status;
            }

            if (!string.IsNullOrEmpty(DocUrl))
            {
                try
                {
                    _status = DocDownloadStatus.Downloading;
                    string path = _downloadFolder + "\\" + _docId;

                    //WebClient myWebClient = new WebClient();
                    //myWebClient.DownloadFile(DocUrl, path);

                    CURLWrapper curl = new CURLWrapper(this);
                    if (curl.DownloadStaticURL(DocUrl, path))
                        _status = DocDownloadStatus.Downloaded;
                    else
                        _status = DocDownloadStatus.DownloadFailed;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + ex.StackTrace);
                    _downloadFailedTimes++;
                    _status = DocDownloadStatus.DownloadFailed;
                }
            }
            return _status;
        }

    }

    public enum DocDownloadStatus
    {
        New = 0,
        UrlReady,
        ServerBusy,
        Downloading,
        Downloaded,
        DownloadFailed,
        RefreshUrlFailed
    }
}

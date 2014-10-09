using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;

using System.Configuration;
using System.Net.Cache;
using System.Xml;
using System.Diagnostics;
using System.Security.Cryptography;

namespace DownloadPageService
{
    /// <summary>
    /// Summary description for DownloadPageAPI
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DownloadPageAPI : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetDataText(string url,string encodingName=null)
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            using (WebClient wc = CreateWebClient(url, encodingName))
            {
                wc.DownloadStringAsync(new Uri(url));
                wc.DownloadStringCompleted += (sender, e) =>
                {
                    tcs.SetResult(e.Result);
                };

                tcs.Task.Wait();
            }
            return tcs.Task.Result;
        }

        [WebMethod]
        public byte[] GetDataStream(string url, string encodingName = null)
        {
            TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();

            using (WebClient wc = CreateWebClient(url, encodingName))
            {
                wc.DownloadDataAsync(new Uri(url));
                wc.DownloadDataCompleted += (sender, e) =>
                {
                    tcs.SetResult(e.Result);
                };

                tcs.Task.Wait();
            }

            return tcs.Task.Result;
        }

        private static WebClient CreateWebClient(string url, string encodingName)
        {
            WebClient wc = new WebClient();
            if (!string.IsNullOrEmpty(encodingName))
            {
                wc.Encoding = Encoding.GetEncoding(encodingName);
            }

            return wc;
        }

        [WebMethod]
        public string GetTextByCasperjs(string projectName, string scriptName, string paraStr)
        {
            //The projects folder is default to current App folder
            //Todo: get "CasperjsProjectsFolder" from web.config if override default projects folder
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory + "CasperjsProjects";
            return CasperjsWrapper.RunCaspjerProject(baseFolder, projectName, scriptName, paraStr);
        }

        [WebMethod]
        public string GetHttpHeaderXml(string url, bool AutoRedirect)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            request.AllowAutoRedirect = AutoRedirect;
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StringBuilder sb = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(sb);

                writer.WriteStartDocument();
                writer.WriteStartElement("Header");
                writer.WriteAttributeString("StatusCode", response.StatusCode.ToString());
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Found)
                {
                    foreach (string key in response.Headers.Keys)
                    {
                        writer.WriteElementString(key, response.Headers[key]);
                        Debug.WriteLine(string.Format("{0}:{1}", key, response.Headers[key]));
                    }
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
                return sb.ToString();
            }
            catch (WebException ex)
            {
                WebExceptionStatus status = ex.Status;
                HttpWebResponse response = (HttpWebResponse)ex.Response;

                StringBuilder sb = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(sb);
                writer.WriteStartDocument();
                writer.WriteStartElement("Exception");
                writer.WriteStartElement("Status");
                writer.WriteValue(response.StatusCode.ToString());
                writer.WriteEndElement(); //Status end
                writer.WriteStartElement("Message");
                writer.WriteValue(ex.Message);
                writer.WriteEndElement(); //Message end
                writer.WriteEndElement(); //Exception end
                writer.WriteEndDocument();

                writer.Flush();
                writer.Close();
                return sb.ToString();
            }
        }

        [WebMethod]
        public string SimpleDownloadString(string url)
        {
            WebClient client = new WebClient();
            return client.DownloadString(url);
        }

        [WebMethod]
        public string GetDownloadFileProperties(string url)
        {

            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);

            writer.WriteStartDocument();
            writer.WriteStartElement("FileProperties");
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    string path = Path.GetTempPath();
                    string file = Path.GetTempFileName();
                    string fullpath = Path.Combine(path, file);

                    CURLWrapper curl = new CURLWrapper();
                    if (curl.DownloadStaticURL(url, fullpath))
                    {
                        StringBuilder sbHash = new StringBuilder(32);
                        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                        FileStream fs = new FileStream(fullpath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        md5.ComputeHash(fs);
                        for (int i = 0; i < md5.Hash.Length; i++)
                        {
                            sbHash.Append(md5.Hash[i].ToString("X2"));
                        }
                        md5.Clear();

                        long fileLength = fs.Length;
                        fs.Close();

                        writer.WriteAttributeString("StatusCode", "200");
                        writer.WriteElementString("PinNode", "Element");
                        writer.WriteElementString("Length", fileLength.ToString());
                        writer.WriteElementString("HASH", sbHash.ToString());
                        Debug.WriteLine(string.Format("Length: {0}, HASH:{1}", fileLength, sbHash));
                    }


                }
                else
                {
                    writer.WriteAttributeString("StatusCode", "IsNotWellFormedUriString");
                }
            }
            catch (Exception ex)
            {
                writer.WriteAttributeString("StatusCode", ex.Message);
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return sb.ToString();
        }
    }
}

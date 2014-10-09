using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Diagnostics;
using SeasideResearch.LibCurlNet;

namespace DownloadPageService
{
    public class CURLWrapper
    {
        private FileStream _fileStream;

        public CURLWrapper()
        {
        }

        public bool DownloadStaticURL(string url, string OutputFilename)
        {
            string path = Path.GetDirectoryName(OutputFilename);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            try
            {
                _fileStream = new FileStream(OutputFilename, FileMode.Create);

                //Todo: move Curl.GlobalInit out, just invoke once.
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                Easy easy = new Easy();
                Easy.WriteFunction wf = new Easy.WriteFunction(OnWriteData);
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);

                Easy.ProgressFunction pf = new Easy.ProgressFunction(ProgressFunction);

                easy.SetOpt(CURLoption.CURLOPT_NOPROGRESS, false);
                easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);
                if (!string.IsNullOrEmpty(url))
                { easy.SetOpt(CURLoption.CURLOPT_URL, url); }

                easy.Perform();
                easy.Cleanup();
                Curl.GlobalCleanup();

                _fileStream.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                _fileStream.Close();
                return false;
            }

            return true;
        }

        private Int32 OnWriteData(Byte[] buf, Int32 size, Int32 nmemb, Object extraData)
        {
            //Debug.WriteLine(string.Format("{0} bytes are receieved.", buf.Length));
            if (_fileStream != null)
                _fileStream.Write(buf, 0, buf.Length);
            return size * nmemb;
        }

        private int ProgressFunction(object extraData, double dlTotal, double dlNow, double ulTotal, double ulNow)
        {
            //Console.WriteLine("Progress: {0} {1} {2} {3}", dlTotal, dlNow, ulTotal, ulNow);
            //if (dlTotal > 0)
            //    _task.Percentage = string.Format("{0:#.0%}", dlNow / dlTotal);
            //else
            //    _task.Percentage = "0.0%";

            return (int)ulNow; // standard return from PROGRESSFUNCTION
        }
    }
}
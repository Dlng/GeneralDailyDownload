using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using SeasideResearch.LibCurlNet;

namespace doc.twse
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

            Application.Run(new DocDownloadWnd());

            Curl.GlobalCleanup();

        }
    }
}

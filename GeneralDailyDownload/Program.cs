using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Configuration;

namespace GeneralDailyDownload
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0 || string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Usage: generaldailydownload.exe projectname");
                return;
            }

            string configfile = ConfigurationManager.AppSettings["ProjectConfigFile"];

            string projectName = args[0];
            string xpath = string.Format("//dailydownload/project[@name='{0}']", projectName);
            XmlDocument config = new XmlDocument();
            config.Load(configfile);
            XmlNode projectNode = config.SelectSingleNode(xpath);
            if (projectNode != null)
            {
                string outputBaseFolder = ConfigurationManager.AppSettings["OutputBaseFolder"];
                DownloadProject project = new DownloadProject(projectNode, outputBaseFolder);
                project.Start();
            }
            else
            {
                Console.WriteLine(string.Format("Can't find the project: {0}", projectName));
                return;
            }

            Console.WriteLine("I am the end.");
        }
    }
}

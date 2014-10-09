using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Text;



namespace DownloadPageService
{
    public class CasperjsWrapper
    {
        public static string RunCaspjerProject(string baseFolder, string projectName, string scriptName, string paraStr)
        {
            string projectFolder = string.Format("{0}\\{1}", baseFolder, projectName);
            if (!Directory.Exists(projectFolder))
                return "Can't find project folder: " + projectName;
            
            string scriptPath = string.Format("{0}\\{1}", projectFolder, scriptName);
            if(!File.Exists(scriptPath))
                return string.Format("Can't find script file in project {0}: {1}", projectName, scriptName);  
            
            CasperjsProject project = null;
            switch(projectName.ToLower())
            {
                case "doc.twse":
                case "sitca":
                    project = new CasperjsProject(projectName, scriptPath, paraStr);
                    project.OutputEncoding = "Big5";
                    break;
             
                default:
                    project = new CasperjsProject(projectName, scriptPath, paraStr);
                    break;
            }

            if (project == null)
                return "Invalid project name: " + projectName;

            return project.Run();

        }
    }

    enum CasperjsErrorCode
    {
        NoError = 0,
        acelife_timeout_btnAgree = 101,
        acelife_timeout_fundListTable = 102
    }


    //Base class, override it if BuildArguments or BuildOuput has different implement
    class CasperjsProject
    {
        protected string _projectName;
        protected string _scriptPath;
        protected string _paraStr;
        protected string _outputPath;

        string _outputEncoding;
        public string OutputEncoding
        {
            get { return _outputEncoding; }
            set { _outputEncoding = value; }
        }

        public CasperjsProject(string projectName, string scriptPath, string paraStr)
        {
            _projectName = projectName;
            _scriptPath = scriptPath;
            _paraStr = paraStr;
        }

        public string Run()
        {
            Process p = new Process();

            //p.StartInfo.FileName = "cmd";
            p.StartInfo.FileName = "casperjs.exe";
            //p.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            //p.StartInfo.Arguments = string.Format("{0} {1} {2}", scriptPath, strPara, outputPath);
            p.StartInfo.Arguments = BuildArguments();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            int exitcode = -1;
            if (p.Start())
            {
                //p.StandardInput.WriteLine("ipconfig");
                //p.StandardInput.WriteLine("exit");
                p.WaitForExit();

                exitcode = p.ExitCode;

                string msg = p.StandardOutput.ReadToEnd();

                p.Close();

                Console.Write(msg);

                if (exitcode != 0)
                {
                    string logDirectory = string.Format("{0}\\log", Path.GetDirectoryName(_scriptPath));
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }

                    string logPath = string.Format("{0}\\{1}_{2}.log", logDirectory,
                                                _projectName, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                    File.WriteAllText(logPath, msg);
                }

            }
            
            return BuildOuput(exitcode);
        }        

        virtual protected string BuildArguments()
        {
            string strPara = _scriptPath + " " + _paraStr ;

            _outputPath = Path.GetTempFileName();

            strPara = string.Format("--ignore-ssl-errors=yes \"{0}\" {1} \"{2}\"", _scriptPath, _paraStr, _outputPath);

            return strPara;
        }

        virtual protected string BuildOuput(int exitCode)
        {
            string retText = "";
            if (Enum.IsDefined(typeof(CasperjsErrorCode), exitCode))
            {
                CasperjsErrorCode code = (CasperjsErrorCode)exitCode;
                if (code == CasperjsErrorCode.NoError)
                {
                    if(string.IsNullOrEmpty(_outputEncoding))
                        retText = File.ReadAllText(_outputPath);
                    else
                        retText = File.ReadAllText(_outputPath, Encoding.GetEncoding("Big5"));

                }
                else
                    retText = string.Format("{0} error: {1}", _projectName, code.ToString());
            }
            else
            {
                retText = "Unkown error." + exitCode.ToString();
            }

            return retText;
        }

    }
}
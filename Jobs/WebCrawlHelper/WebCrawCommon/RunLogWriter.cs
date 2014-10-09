using MorningstarAWD.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawCommon
{
    public static class RunLogWriter
    {
        /// <summary>
        /// Write run log into common service by xmldata
        /// </summary>
        /// <param name="logtype">Your log type,it will be used to determines which storedprocedure to be called.</param>
        /// <param name="logXml">The xmldata of log,please make sure its data format is corresponding to your storedprocedure.</param>
        /// <returns>the number of rows affected.</returns>
        public static int Write(string logtype, string logXml)
        {
            if (string.IsNullOrWhiteSpace(logtype))
            {
                return -1;
            }

            if (string.IsNullOrWhiteSpace(logXml))
            {
                return -1;
            }

            CommonServiceDB comServiceDB = new CommonServiceDB();
            return comServiceDB.AddRunlogXml(logtype, logXml);
        }
    }

    class CommonServiceDB : AWDDBBase
    {
        internal CommonServiceDB()
            : base(AWDEnvironment.s_DefaultEnviroment,"CommonService")
        { }

        internal int AddRunlogXml(string logtype, string logXml)
        {
            this.CreateStoredProcCommand(string.Format("addRunLogXml_{0}", logtype));
            this.AddInParameter("@p_LogXml", DbType.Xml, logXml);

            return this.ExecuteNonQuery();
        }
    }
}

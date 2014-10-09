using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration ;
using System.Net.Mail;

namespace Handler.Net
{
    public static class EmailSender
    {
        public static void SendEmail(List<string> toList, List<string> ccList,
                                     List<string> attachmentPathList,
                                     string subject, string content,
                                     bool isBodyHtml)
        {

            EmailParameter ep = CreateEP();
 
            ep.toList = toList;
            ep.ccList = ccList;
            ep.attachmentPathList = attachmentPathList;
            ep.subject = subject;
            ep.content = content;
            ep.isBodyHtml = isBodyHtml;
            ep.encoding = Encoding.UTF8;
            EmailHandler.SendEmail(ep);
        }

        public static void SendEmail(string toList, string ccList, List<string> attachmentPathList,
                                     string subject, string content, bool isBodyHtml)
        {
            string[] to = toList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] cc = ccList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            SendEmail(to.ToList(), cc.ToList(), attachmentPathList, subject, content, isBodyHtml);
        }

        public static void SendEmail(List<string> toList, string subject,
                                     string content, bool isBodyHtml)
        {
            SendEmail(toList, null, null, subject, content, isBodyHtml);
        }

        public static void SendEmail(List<string> toList, List<string> ccList,
                                     string subject, AlternateView htmlView)
        {
            EmailParameter ep = CreateEP();
            ep.toList = toList;
            ep.ccList = ccList;
            ep.subject = subject;
            ep.encoding = Encoding.UTF8;
            ep.htmlView = htmlView;
            EmailHandler.SendEmail(ep);
        }
        private static EmailParameter CreateEP()
        {
            EmailParameter ep = new EmailParameter();
            ep.smtpServer = ConfigurationManager.AppSettings["MailServer"];
            ep.authenAccount = ConfigurationManager.AppSettings["AuthenAccount"];
            ep.authenPassword = ConfigurationManager.AppSettings["AuthenPassword"];
            ep.from = ConfigurationManager.AppSettings["EmailFrom"];
            ep.displayname = ConfigurationManager.AppSettings["DisplayName"];
            return ep;
        }
    }
}

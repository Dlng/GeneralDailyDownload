using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using System.Net;

namespace Handler.Net
{


    public class EmailParameter
    {
        public string smtpServer { set; get; }
        public string authenAccount { set; get; }
        public string authenPassword { set; get; }
        public string from { set; get; }
        public string subject { set; get; }
        public string content { set; get; }
        public string displayname { set; get; }
        public bool isBodyHtml { set; get; }
        public Encoding encoding { set; get; }
        public AlternateView htmlView { set; get; }
        public List<string> toList { set; get; }
        public List<string> ccList { set; get; }
        public List<string> attachmentPathList { set; get; }

    }
    public static class EmailHandler
    {

        public static void SendEmail(EmailParameter ep)
        {
            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress(ep.from);

                AddEmail2MailAddressCollection(ep.toList, mm.To);
                AddEmail2MailAddressCollection(ep.ccList, mm.CC);

                AddAttachments(ep.attachmentPathList, mm.Attachments);

                mm.IsBodyHtml = ep.isBodyHtml;
                mm.Subject = ep.subject;
                if (!string.IsNullOrEmpty(ep.content))
                {
                    mm.Body = ep.content;
                }
                if (ep.htmlView != null)
                {
                    mm.AlternateViews.Add(ep.htmlView);
                }

                mm.SubjectEncoding = ep.encoding;
                mm.BodyEncoding = ep.encoding;
                mm.Sender = new MailAddress(ep.from, ep.displayname, ep.encoding);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                SmtpClient smtpMail = new SmtpClient(ep.smtpServer);
                smtpMail.Credentials = new System.Net.NetworkCredential(ep.authenAccount, ep.authenPassword);
                smtpMail.Send(mm);
            }
        }
        private static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private static void AddEmail2MailAddressCollection(List<string> eList, MailAddressCollection MC)
        {
            if (eList == null) return;

            foreach (string e in eList)
            {
                if (!string.IsNullOrEmpty(e))
                {
                    MC.Add(e);
                }
            }
        }

        private static void AddAttachments(List<string> attachmentPathList, AttachmentCollection Attachments)
        {
            if (attachmentPathList == null) return;

            foreach (string att in attachmentPathList)
            {
                if (File.Exists(att))
                {
                    Attachments.Add(new Attachment(att));
                }
            }
        }
    }
}

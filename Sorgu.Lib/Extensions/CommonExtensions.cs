using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Sorgu.Lib.Extensions
{
    public static class CommonExtensions
    {
        public static string ToNormalize(this string Source)
        {
            if (!string.IsNullOrEmpty(Source))
            {
                Regex rgx = new Regex("[^a-zA-Z0-9/-]");
                Source = rgx.Replace(Source, "");

                Source = Source.Replace("--", "");

                return Source;
            }

            return null;
        }

        public static bool SendMail (IList<string> Recipients, IList<string> Cc, string Subject, string Body, IList<string> Bcc)
        {
           

            bool result = true;
            string recipStr = string.Empty, ccStr = string.Empty, bccStr = string.Empty;
            try
            {
                    MailMessage mail = new MailMessage();
                    mail.Body = Body;
                    mail.BodyEncoding = System.Text.Encoding.GetEncoding(1254);
                    mail.From = new MailAddress(WebConfigurationManager.AppSettings["MailUsername"], "HasarOnline Hasar Yönetim Sistemi");
                    mail.Sender = new MailAddress(WebConfigurationManager.AppSettings["MailUsername"], "HasarOnline Hasar Yönetim Sistemi");
                    mail.Subject = string.Format("[HasarOnline] - {0}", Subject);
                    mail.IsBodyHtml = true;
                    mail.SubjectEncoding = System.Text.Encoding.GetEncoding(1254);
                    foreach (string s in Recipients)
                    {
                        mail.To.Add(new MailAddress(s));
                        recipStr += s + ";";
                    }

                    mail.Bcc.Add(new MailAddress(WebConfigurationManager.AppSettings["MailUsername"], "HasarOnline"));

                    if (Bcc != null)
                    {
                        foreach (string bc in Bcc)
                        {
                            mail.To.Add(new MailAddress(bc));
                            recipStr += bc + ";";
                        }
                    }

                    if (Cc != null && Cc.Count > 0)
                    {
                        foreach (string item in Cc)
                        {
                            mail.CC.Add(new MailAddress(item));
                            ccStr += item + ";";
                        }
                    }



                    SmtpClient sc = new SmtpClient(WebConfigurationManager.AppSettings["MailServer"], Convert.ToInt32(WebConfigurationManager.AppSettings["MailPort"]));
                    if (WebConfigurationManager.AppSettings["MailPassword"] != null)
                    {
                        sc.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["MailUsername"], WebConfigurationManager.AppSettings["MailPassword"]);
                    }
                    else
                    {
                        sc.UseDefaultCredentials = true;
                    }
                    if (WebConfigurationManager.AppSettings["SSL"] == "1") sc.EnableSsl = true;
                    sc.Send(mail);
                    
                
            }
            catch (Exception ex)
            {
                result = false;
            }

            

            return result;
        }
    }
}

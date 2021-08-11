using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AllWork.Common
{
    public class Mail
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">主旨</param>
        /// <param name="body">正文</param>
        public  static  void SendMail(string subject, string body)
        {
            body = string.Format("尊敬的开发工程师: {0}", body);
            var from = new MailAddress("gainstart@126.com", "roy126");
            var to = new MailAddress("gainstart@126.com", "易工");
            
            var message = new MailMessage(from, to);
            //message.To.Add(new MailAddress("278107945@qq.com", "雷湘状"));
            message.Subject = subject + DateTime.Now.ToString("yyyy-MM-dd hh:mm:sss");
            message.Body = body;
            message.IsBodyHtml = false; //这个不经意间的设置给我造成了很大的麻烦，设为true之后，因一些数据是标签性的不会显示，以为没有返回数据，我穷尽了一切办法找原因，结果是这里导致的显示问题
            SmtpClient smtpClient = new SmtpClient { Host = "smtp.126.com", Port = 25 };
            smtpClient.Credentials = new NetworkCredential("gainstart", "wnroy456789Ygx");
             smtpClient.Send(message);
        }
    }
}

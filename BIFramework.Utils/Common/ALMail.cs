using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 收发邮件
    /// </summary>
    public class ALMail
    {
        #region 初始化账户信息

        private ALMailAccount account;

        public ALMail()
        {
            var dict = ALConfig.DllConfigs["BIUtils.Web"]["MailConfig"]
                .ToDictionary(d => d.Attribute("key").Value, d => d.Attribute("value").Value);

            account = new ALMailAccount();
            account.Username = dict["Username"];
            account.Sysemail = dict["Sysemail"];
            account.Signature = dict["Signature"];

            account.Pop = dict["Pop"];
            account.PopPort = ALConvert.ToInt(dict["PopPort"]) ?? 110;
            account.PopAccount = dict["PopAccount"];
            account.PopPassword = dict["PopPassword"];
            account.PopEnableSsl = (ALConvert.ToInt0(dict["PopEnableSsl"]) == 1);

            account.Smtp = dict["Smtp"];
            account.SmtpPort = ALConvert.ToInt(dict["SmtpPort"]) ?? 25;
            account.SmtpAccount = dict["SmtpAccount"];
            account.SmtpPassword = dict["SmtpPassword"];
            account.SmtpEnableSsl = (ALConvert.ToInt0(dict["SmtpEnableSsl"]) == 1);
        }
        public ALMail(ALMailAccount account)
        {
            this.account = account;
        }
        #endregion

        /// <summary>
        /// 发邮件
        /// </summary>
        /// <param name="title">邮件标题</param>
        /// <param name="htmlContent">邮件HTML内容</param>
        /// <param name="recipientEmail">收件人 Email</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string title, string htmlContent, List<string> recipientEmail)
        {
            return Send(title, htmlContent, recipientEmail, new List<Attachment>());
        }
        /// <summary>
        /// 发邮件
        /// </summary>
        /// <param name="title">邮件标题</param>
        /// <param name="htmlContent">邮件HTML内容</param>
        /// <param name="recipient">收件人</param>
        /// <param name="recipientEmail">收件人 Email</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string title, string htmlContent, string recipient, string recipientEmail)
        {
            Dictionary<string, string> recipients = new Dictionary<string, string>();
            recipients.Add(recipientEmail, recipient);
            return Send(title, htmlContent, recipients, null);
        }
        /// <summary>
        /// 发邮件
        /// </summary>
        /// <param name="title">邮件标题</param>
        /// <param name="htmlContent">邮件HTML内容</param>
        /// <param name="recipientEmail">收件人 Email</param>
        /// <param name="attachs">附件</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string title, string htmlContent, List<string> recipientEmail, List<Attachment> attachs)
        {
            if (recipientEmail == null)
                return Send(title, htmlContent, (Dictionary<string, string>)null, attachs);
            else
                return Send(title, htmlContent, recipientEmail.ToDictionary(d => d, d => ""), attachs);
        }
        /// <summary>
        /// 发邮件
        /// </summary>
        /// <param name="title">邮件标题</param>
        /// <param name="htmlContent">邮件HTML内容</param>
        /// <param name="recipients">收件人 Email/姓名</param>
        /// <param name="attachs">附件</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string title, string htmlContent, Dictionary<string, string> recipients, List<Attachment> attachs)
        {
            if (recipients == null || recipients.Count == 0)
                return false;

            MailMessage objMailMessage = new MailMessage();
            objMailMessage.From = new MailAddress(account.Sysemail, account.Username);
            foreach (var mail in recipients)
                objMailMessage.To.Add(new MailAddress(mail.Key, string.IsNullOrEmpty(mail.Value) ? mail.Key : mail.Value));
            objMailMessage.BodyEncoding = Encoding.UTF8;
            objMailMessage.Subject = title;
            objMailMessage.Body = htmlContent + account.Signature;
            objMailMessage.IsBodyHtml = true;
            if (attachs != null)
            {
                foreach (Attachment attach in attachs)
                    objMailMessage.Attachments.Add(attach);
            }

            SmtpClient objSmtpClient = new SmtpClient();
            objSmtpClient.Host = account.Smtp;
            objSmtpClient.Port = account.SmtpPort;
            objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            objSmtpClient.UseDefaultCredentials = false;
            objSmtpClient.Credentials = new System.Net.NetworkCredential(account.SmtpAccount, account.SmtpPassword);
            objSmtpClient.EnableSsl = account.SmtpEnableSsl;


            objSmtpClient.Send(objMailMessage);
            return true;

        }

        /// <summary>
        /// 邮件账户信息
        /// </summary>
        public struct ALMailAccount
        {
            /// <summary>
            /// 昵称
            /// </summary>
            public string Username { get; set; }
            /// <summary>
            /// 邮件地址
            /// </summary>
            public string Sysemail { get; set; }
            /// <summary>
            /// 签名
            /// </summary>
            public string Signature { get; set; }

            /// <summary>
            /// 收信服务器
            /// </summary>
            public string Pop { get; set; }
            /// <summary>
            /// 收信服务器端口
            /// </summary>
            public int PopPort { get; set; }
            /// <summary>
            /// 收信用户名
            /// </summary>
            public string PopAccount { get; set; }
            /// <summary>
            /// 收信密码
            /// </summary>
            public string PopPassword { get; set; }
            /// <summary>
            /// 收信SSL通道
            /// </summary>
            public bool PopEnableSsl { get; set; }

            /// <summary>
            /// 发信服务器
            /// </summary>
            public string Smtp { get; set; }
            /// <summary>
            /// 发信服务器端口
            /// </summary>
            public int SmtpPort { get; set; }
            /// <summary>
            /// 发信用户名
            /// </summary>
            public string SmtpAccount { get; set; }
            /// <summary>
            /// 发信密码
            /// </summary>
            public string SmtpPassword { get; set; }
            /// <summary>
            /// 发信SSL通道
            /// </summary>
            public bool SmtpEnableSsl { get; set; }
        }
    }

}

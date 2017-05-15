using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Einvoice_Customer.Service
{
    public class EmailServices
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var _email = System.Configuration.ConfigurationManager.AppSettings["DefaultMailAccount"];
                var _ePass = System.Configuration.ConfigurationManager.AppSettings["DefaultMailPass"];
                var _displayName = ConfigurationManager.AppSettings["EmailCompany"];// "InterContinental Danang Sun Peninsula Resort";
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress(_email, _displayName);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    //smtp.EnableSsl = true;
                    smtp.Host = System.Configuration.ConfigurationManager.AppSettings["HostMail"];
                    smtp.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["HostMailPort"]);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new NetworkCredential(_email, _ePass);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                    await smtp.SendMailAsync(mail);
                }
                //var _email = ConfigurationManager.AppSettings["DefaultMailAccount"];
                //var _ePass = ConfigurationManager.AppSettings["DefaultMailPass"]; 
                //var _displayName = ConfigurationManager.AppSettings["EmailCompany"];
                //MailMessage mail = new MailMessage();
                //mail.To.Add(email);
                //mail.From = new MailAddress(_email, _displayName);
                //mail.Subject = subject;
                //mail.Body = message;
                //mail.IsBodyHtml = true;
                //using (SmtpClient smtp = new SmtpClient())
                //{
                //    smtp.EnableSsl = true;
                //    smtp.Host = "smtp.gmail.com";
                //    smtp.Port = 25;
                //    smtp.UseDefaultCredentials = true;
                //    smtp.Credentials = new NetworkCredential(_email, _ePass);
                //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //    smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                //    await smtp.SendMailAsync(mail);
                //}
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}

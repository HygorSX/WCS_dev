//classe email
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace WCS.Utilities
{
    public class Email
    {
        public String UsernamePassword { get; set; }
        public String FromEmail { get; set; }
        public String ToEmail { get; set; }
        public String CcEmail { get; set; }
        public String Body { get; set; }
        public String Subject { get; set; }


        public Email()
        {
            FromEmail = "cit.estoque@barueri.sp.gov.br";
            UsernamePassword = "Estok@2023";

        }

        public bool Send(Email email)
        {
            

            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {

                    
                    string[] emailsc = email.CcEmail.ToString().Split(';');
                    

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(FromEmail);
                    mail.To.Add(new MailAddress(email.ToEmail));
                    if(emailsc.Length > 0)
                    {
                        foreach (string e in emailsc)
                        {
                            mail.CC.Add(new MailAddress(e));
                        }
                    }
   
                    mail.Subject = email.Subject;
                    mail.Body = email.Body;
                    mail.IsBodyHtml = true;
                    //mail.Headers.Add("Disposition-Notification-To", FromEmail);
                    //mail.ReplyToList.Add(new MailAddress(FromEmail));
                    //mail.Headers.Add("Disposition-Notification-To", "cit.arjona@barueri.sp.gov.br");
                    //mail.ReplyToList.Add(new MailAddress("cit.arjona@barueri.sp.gov.br"));

                    smtp.Host = "mail.barueri.sp.gov.br";
                    smtp.Port = 25;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(email.FromEmail, email.UsernamePassword);

                    smtp.Send(mail);
                    return true;
                }
            }catch (Exception)
            {
                return false;
            }

        }
       
    }
}


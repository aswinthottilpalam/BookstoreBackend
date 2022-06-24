using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Repository_Layer.Service
{
    public class EmailServices
    {
        public static void SendMail(string Email, string token)
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("aswintesterdemo@gmail.com", "pbmsejjmcchjwyxo");

                MailMessage messageObj = new MailMessage();
                messageObj.To.Add(Email);
                messageObj.From = new MailAddress("aswintesterdemo@gmail.com");
                messageObj.Subject = "Password Reset link";
                messageObj.IsBodyHtml = true;
                messageObj.Body = $"<!DOCTYPE html>" +
                                   "<html>" +
                                   "<body> " +
                                   "<h2>Hello! </h2>" +
                                   "<h4>Please click on the below link to change your password.</h4>" +
                                   "</body>" + $"http://localhost:4200/reset-password/{token}" +

                                    "<html>";
                client.Send(messageObj);
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace JoelScottFitness.Services.Services
{
    public class EmailService : IEmailService
    {
        public bool SendEmail(string subject, string content, IEnumerable<string> receivers)
        {
            bool success = false;
            try
            {
                Parallel.ForEach(receivers, (receiver) => 
                {
                    using (var client = new SmtpClient())
                    {
                        client.Host = "smtp.gmail.com";
                        client.Port = 587;
                        client.Credentials = new NetworkCredential("sb355gts@gmail.com", "458Ferrari!");
                        client.EnableSsl = true;

                        MailMessage message = new MailMessage("Joel@JoelScottFitness.com", receiver, subject, content);
                        message.IsBodyHtml = true;

                        client.Send(message);
                    }
                });
                
                success = true;
            }
            catch(Exception ex)
            {
                var x = ex.Message;
            }

            return success;
        }

        public async Task<bool> SendEmailAsync(string subject, string content, IEnumerable<string> receivers)
        {
            bool success = false;
            try
            {
                var emailTasks = new List<Task>();

                foreach (var receiver in receivers)
                {
                    emailTasks.Add(Task.Run(() =>
                    {
                        var client = new SmtpClient();
                        client.Host = "smtp.gmail.com";
                        client.Port = 587;
                        client.Credentials = new NetworkCredential("sb355gts@gmail.com", "458Ferrari!");
                        client.EnableSsl = true;
                        client.SendCompleted += (s, e) => client.Dispose();

                        MailMessage mail = new MailMessage("Joel@JoelScottFitness.com", receiver, subject, content);
                        mail.IsBodyHtml = true;

                        client.SendMailAsync(mail);
                    }));
                }
                
                await Task.WhenAll(emailTasks);
                success = true;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }

            return await Task.FromResult(success);
        }
    }
}

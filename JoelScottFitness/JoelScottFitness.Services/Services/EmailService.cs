﻿using JoelScottFitness.Common.IO;
using JoelScottFitness.Services.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace JoelScottFitness.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFileHelper fileHelper;

        public EmailService(IFileHelper fileHelper)
        {
            if (fileHelper == null)
                throw new ArgumentNullException(nameof(fileHelper));

            this.fileHelper = fileHelper;
        }

        public bool SendEmail(string subject, string content, IEnumerable<string> receivers)
        {
            return SendEmail(subject, content, receivers, null);
        }

        public bool SendEmail(string subject, string content, IEnumerable<string> receivers, IEnumerable<string> attachmentPaths)
        {
            bool success = false;
            try
            {
                Parallel.ForEach(receivers, (receiver) =>
                {
                    using (var client = new SmtpClient())
                    {
                        client.Host = Settings.Default.SmtpHostName;
                        client.Port = Settings.Default.SmtpPort;
                        client.Credentials = new NetworkCredential(Settings.Default.EmailAccount, Settings.Default.EmailPassword);
                        client.EnableSsl = (Settings.Default.SmtpPort == 465);

                        MailMessage message = new MailMessage(Settings.Default.EmailAccount, receiver, subject, content);
                        message.IsBodyHtml = true;

                        client.Send(message);
                    }
                });

                success = true;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }

            return success;
        }

        public async Task<bool> SendEmailAsync(string subject, string content, IEnumerable<string> receivers)
        {
            return await SendEmailAsync(subject, content, receivers, null);
        }

        public async Task<bool> SendEmailAsync(string subject, string content, IEnumerable<string> receivers, IEnumerable<string> attachmentPaths)
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
                        client.Host = Settings.Default.SmtpHostName;
                        client.Port = Settings.Default.SmtpPort;
                        client.Credentials = new NetworkCredential(Settings.Default.EmailAccount, Settings.Default.EmailPassword);
                        client.EnableSsl = (Settings.Default.SmtpPort == 465);
                        client.SendCompleted += (s, e) => client.Dispose();

                        MailMessage mail = new MailMessage(Settings.Default.EmailAccount, receiver, subject, content);

                        // attach files
                        if (attachmentPaths != null && attachmentPaths.Any())
                        {
                            foreach (var attachmentPath in attachmentPaths)
                            {
                                if (fileHelper.FileExists(attachmentPath))
                                {
                                    mail.Attachments.Add(new Attachment(attachmentPath));
                                }
                            }
                        }

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

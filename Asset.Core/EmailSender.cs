using System;
using MailKit.Net.Smtp;
using MimeKit;
using Asset.Domain;
using Asset.ViewModels.UserVM;
using System.Collections.Generic;

namespace Asset.Core
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfigurationVM _emailConfig;

        public EmailSender(EmailConfigurationVM emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(MessageVM message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        public void SendEmailString(string message)
        {
            throw new NotImplementedException();
        }

        private MimeMessage CreateEmailMessage(MessageVM message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("",_emailConfig.From));
            emailMessage.To.AddRange( message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port,true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch(Exception ex)
                {

                    string strError = ex.Message;
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}

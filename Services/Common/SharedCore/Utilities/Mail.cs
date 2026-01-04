using FluentEmail.Smtp;
using FluentEmail.Core;
using System.Net.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using CoreServiceContracts.MailHub;

namespace SharedCore.Utilities
{
    public static class Mail
    {
        static Mail()
        {
            Connect();
        }
        private const string EmailAddress = "notices@mgginv.com";


        private static void Connect()
        {
            var sender = new SmtpSender(() => new SmtpClient("smtp.office365.com")
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(EmailAddress, "Collected_glance_234"),
                EnableSsl = true,
                Port = 587
            });

            Email.DefaultSender = sender;
        }

        public static void Send(String subject, String body, List<String>? ToList = null,
            List<String>? ToCCList = null, List<String>? ToBccList = null,
            bool Critical = false, List<String>? Attachments = null,
            String? EmailFrom = null, String? EmailName = null, int jobID = 0, 
            string jobDesc = "", bool UseMailHub = true, bool ActionRequired = false)
        {
            // Configure FluentEmail with SMTP sender
            // Send email   

            try
            {
                if (Overrides.Environment != Overrides.EnvironmentType.Production)
                    return;

                if (UseMailHub)
                {
                    MailProxy mailProxy = MailProxy.GetInstance(appName: Overrides.AppName);
                    if (mailProxy != null)
                    {
                        bool result = mailProxy.Send(subject, body, ToList, ToCCList, ToBccList, Critical, Attachments, jobID, jobDesc, ActionRequired);
                        if (!result)
                        {
                            Logger.Info("Failed to Send mail");
                        }
                    }
                    else
                        SendLocalMail(subject, body, ToList, ToCCList, ToBccList, Critical, Attachments, EmailFrom, EmailName, jobID, jobDesc);
                }
                else
                {
                    SendLocalMail(subject, body, ToList, ToCCList, ToBccList, Critical, Attachments, EmailFrom, EmailName, jobID, jobDesc);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception sending email.", ex);
            }

        }

        private static void SendLocalMail(string subject, string body, List<string>? ToList, List<string>? ToCCList, List<string>? ToBccList, bool Critical, List<string>? Attachments, string? EmailFrom, string? EmailName, int jobID, string jobDesc)
        {
            if (EmailName == null)
            {
                EmailName = "Reporting Service";
            }
            if (EmailFrom == null)
            {
                EmailFrom = EmailAddress;
            }

            var recipients = new List<FluentEmail.Core.Models.Address>();
            var ccRecipients = new List<FluentEmail.Core.Models.Address>();
            var bccRecipients = new List<FluentEmail.Core.Models.Address>();

            if (ToList != null)
                foreach (var to in ToList)
                    recipients.Add(new FluentEmail.Core.Models.Address(to));

            if (ToCCList != null)
                foreach (var cc in ToCCList)
                    ccRecipients.Add(new FluentEmail.Core.Models.Address(cc));

            if (ToBccList != null)
                foreach (var bcc in ToBccList)
                    bccRecipients.Add(new FluentEmail.Core.Models.Address(bcc));

            body += $"<BR/><BR/><BR/><BR/><font size='2' color='black'>For Internal Use Only, Not For External Distribution</font><p align='right'><font size='1' color='gray'>JobID={jobID} / {jobDesc.ToSafeString()} / {Environment.MachineName}</font></p>";


            // Create and send an email
            var email = Email
                .From("notices@mgginv.com", "")
                .To(recipients)
                .CC(ccRecipients)
                .BCC(bccRecipients)
                .Subject(subject)
                .Body(body, isHtml: true);


            if (Critical)
                email.HighPriority();

            if (Attachments != null)
                foreach (var att in Attachments)
                    email.AttachFromFilename(att, null, Path.GetFileName(att));

            var response = email.Send();

            if (!response.Successful)
            {
                Logger.Error("Failed to send email.");
            }

            Console.WriteLine(response.Successful ? "Email sent successfully!" : "Failed to send email.");
        }
    }
}

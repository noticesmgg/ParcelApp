using CoreServiceContracts.Authentication;
using CoreServiceContracts.MailHub.DataObjects;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts.MailHub
{
    /// <summary>
    /// Proxy class to interact with the MailHub for invoking or triggering any tasks
    /// </summary>
    public class MailProxy
    {
        private static MailProxy? _instance;
        private static readonly object _lock = new object();
        private JsonServiceClient _client;

        public string URL { get; private set; }
        public string AppName { get; set; } = "Mail Hub";

        private MailProxy(string url, string? appName = null)
        {
            URL = url;
            if (appName != null)
                AppName = appName;
            _client = GetClient(true);
        }

        /// <summary>
        /// Get the client
        /// </summary>
        /// <param name="Reconnect">If we need to reinitialize the connection</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private JsonServiceClient GetClient(bool Reconnect = false)
        {
            try
            {
                if (_client == null || Reconnect)
                {
                    _client = new JsonServiceClient(URL);
                    _client.RequestFilter = req =>
                    {
                        req.Headers[ADAuthProvider.C_UserName] = Environment.UserName;
                        req.Headers[ADAuthProvider.C_MachineName] = Environment.MachineName;
                        req.Headers[ADAuthProvider.C_App] = AppName;
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error connecting to ActionHub Proxy {URL}", ex);
            }

            return _client;
        }

        /// <summary>
        /// Reconfigure the URL
        /// </summary>
        /// <param name="url"></param>
        public void ReConfigure(string? url = null)
        {
            if (url != null)
                URL = url;
            _instance = new MailProxy(URL);
        }

        /// <summary>
        /// Get the instance
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static MailProxy GetInstance(string? URL = null, string? appName= null)
        {
            if (URL == null)
                URL = "http://mgg-pr-app01:9966/";

            try
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new MailProxy(URL, appName);
                        }
                    }
                }
                return _instance;
            }
            catch 
            {
                return null;                
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailItem"></param>
        /// <returns></returns>
        public MailResponseDO SendMail(MailItemDO mailItem)
        {
            try
            {
                if (mailItem == null)
                    return new MailResponseDO { Sent = false, Message = "Mail Item is null" };

                if (mailItem.To == null || mailItem.To.Count == 0)
                    return new MailResponseDO { Sent = false, Message = "To address is null or empty" };

                if (String.IsNullOrEmpty(mailItem.Subject))
                    return new MailResponseDO { Sent = false, Message = "Subject is null" };

                if (mailItem.Attachments?.Count > 0)
                {
                    string sharePath = @"\\corp.mggcap.com\dfs\shared\MGGINV\Apps\Mailer";
                    mailItem.FileAttachmentMap ??= new Dictionary<string, string>();

                    foreach (string attachment in mailItem.Attachments)
                    {
                        if (!File.Exists(attachment)) continue;

                        string actualFileName = Path.GetFileName(attachment);
                        string randomFileName = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(attachment)}";
                        string newAttachmentPath = Path.Combine(sharePath, randomFileName);
                        try
                        {
                            File.Copy(attachment, newAttachmentPath, overwrite: true);
                            mailItem.FileAttachmentMap[actualFileName] = randomFileName;
                        }
                        catch (IOException ioEx)
                        {
                            Console.WriteLine($"Error copying file {actualFileName}: {ioEx.Message}");
                        }
                    }
                }
                return GetClient().Post(mailItem);
            }
            catch (Exception ex)
            {
                return new MailResponseDO { Sent = false, Message = "Error Message : " + ex };
            }
        }

        public bool Send(String subject, String body, List<String>? ToList = null,
            List<String>? ToCCList = null, List<String>? ToBccList = null,
            bool Critical = false, List<String>? Attachments = null, 
            int jobID = 0, string jobDesc = "", bool ActionRequired = false)
        {
            bool isSend = false;
            try
            {                
                MailItemDO mailItem = new()
                {
                    Subject = subject,
                    Body = body,
                    To = ToList,
                    Cc = ToCCList,
                    Bcc = ToBccList,
                    Critical = Critical,
                    Attachments = Attachments,
                    JobId = jobID,
                    JobDesc = jobDesc,
                    MachineName = Environment.MachineName,
                    isActionRequired = ActionRequired,
                };

                MailResponseDO response = SendMail(mailItem);
                if (response != null && response.Sent.Equals(true))
                    isSend = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :" + ex);
            }
            return isSend;
        }
    }
}

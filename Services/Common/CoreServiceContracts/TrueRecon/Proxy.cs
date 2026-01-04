using CoreServiceContracts.Authentication;
using CoreServiceContracts.TrueRecon.DataObjects;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts.TrueRecon
{
    public class Proxy
    {
        private static Proxy? _instance;
        private static readonly object _lock = new object();
        private JsonServiceClient _client;

        public string URL { get; private set; }

        private Proxy(string url)
        {
            URL = url;
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
                        req.Headers[ADAuthProvider.C_App] = "Recon Service";
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
            _instance = new Proxy(URL);
        }

        /// <summary>
        /// Get the instance
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static Proxy GetInstance(string? URL = null)
        {
            if (URL == null)
                URL = "http://mgg-pr-app01:9933/";

            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Proxy(URL);
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Trigger the Recon
        /// </summary>
        /// <param name="ReconName"></param>
        /// <param name="AsOfDate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public (bool Status, long TriggerId, String Message) Trigger(String ReconName, DateTime? AsOfDate)
        {
            bool status = false;

            try
            {
                if (AsOfDate == null)
                    AsOfDate = DateTime.Now;

                var item = new InvokerDO
                {
                    ReconName = ReconName,
                    ReconDate = AsOfDate.Value
                };

                var data = GetClient().Get(item);
                status = (data.Status == ReconStatus.Success) || (data.Status == ReconStatus.Pending) || (data.Status == ReconStatus.Running);

                return (status, data.TriggerId, data.Message);
            }
            catch (Exception ex)
            {
                return (status, 0, $"Error triggering Recon {ReconName}. Details: {ex.Message} {ex.StackTrace}");
            }

        }
     
    }
}

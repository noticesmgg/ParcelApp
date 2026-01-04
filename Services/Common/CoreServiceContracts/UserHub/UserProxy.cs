using CoreServiceContracts.ActionHub.DataObjects;
using CoreServiceContracts.Authentication;
using CoreServiceContracts.UserHub.DataObjects;
using Microsoft.AspNetCore.Mvc.Razor;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts.UserHub
{
    /// <summary>
    /// Proxy class to interact with the UserHub for Logging and Get the Logs of user activity
    /// </summary>
    public class UserProxy
    {
        private static UserProxy? _instance;
        private static readonly object _lock = new object();
        private JsonServiceClient _client;

        public string URL { get; private set; }

        private UserProxy(string url)
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
                        req.Headers[ADAuthProvider.C_App] = "User Hub";
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
            _instance = new UserProxy(URL);
        }

        /// <summary>
        /// Get the instance
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static UserProxy GetInstance(string? URL = null)
        {
            if (URL == null)
                URL = "http://mgg-pr-app01:9988/";

            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new UserProxy(URL);
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Get the user activity 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<HubAnalyticsDO> GetUserActivity()
        {
            try
            {
                var client = GetClient();
                var response = client.Get(new GetUserActivity());
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting logs from UserHub Proxy {URL}", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HubAnalyticsDO SendActivity(HubAnalyticsDO request)
        {
            try
            {
                var client = GetClient();
                var response = client.Post(request);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while write the logs from UserHub Proxy {URL}", ex);
            }
        }
    }
}

using CoreServiceContracts.Authentication;
using CoreServiceContracts.iLevelApi.DataObjects;
using Microsoft.AspNetCore.SignalR;
using ServiceStack;

namespace CoreServiceContracts.iLevelApi
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
                        req.Headers[ADAuthProvider.C_App] = "iLevel Hub";
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error connecting to iLevel Proxy {URL}", ex);
            }

            return _client;
        }

        /// <summary>
        /// Get the instance
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static Proxy GetInstance(string? URL = null)
        {
            if (URL == null)
                URL = "http://mgg-pr-app01:9955/";

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
        /// Gets the Task
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="TriggerId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TaskResponseDO GetData(int? borrowerId = null, DateTime? asOfDate = null,
            bool reloadMetaData = false, long? TriggerId = null)
        {
            try
            {
                var client = GetClient();
                if (TriggerId == null)
                    TriggerId = Utilities.GetUniqueID();
                var response = client.Get(new TriggerItemDO
                {
                    BorrowerId = borrowerId ?? 0
                    ,
                    AsOfDate = asOfDate
                    ,
                    ReloadMetaData = reloadMetaData
                    ,
                    TriggerId = TriggerId ?? 0
                });
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error triggering task from iLevel Proxy {URL}", ex);
            }
        }

        /// <summary>
        /// Gets the Task status
        /// </summary>
        /// <param name="TriggerId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TaskResponseDO GetTaskStatus(long TriggerId)
        {
            try
            {
                var client = GetClient();
                var response = client.Get(new TaskTriggerStatusDO { TriggerId = TriggerId });
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting trigger task status from iLevel Proxy {URL}", ex);
            }
        }

        /// <summary>
        /// Reload assets
        /// </summary>
        /// <param name="TriggerId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ResponseDO ReloadAssets(long? TriggerId = null)
        {
            try
            {
                var client = GetClient();
                if (TriggerId == null)
                    TriggerId = Utilities.GetUniqueID();
                var response = client.Get(new ReloadAssetsDO { TriggerId = TriggerId ?? 0, UserName = Environment.UserName });
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reloading assets from iLevel Proxy {URL}", ex);
            }
        }
    }
}

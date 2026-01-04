using CoreServiceContracts.ActionHub.DataObjects;
using CoreServiceContracts.Authentication;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts.ActionHub
{
    /// <summary>
    /// Proxy class to interact with the ActionHub for invoking or triggering any tasks
    /// </summary>
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
                        req.Headers[ADAuthProvider.C_App] = "Action Hub";
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
            if(url != null)
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
                URL = "http://mgg-pr-app01:9988/";

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
        /// Get the tasks
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<TaskItemDO> GetTasks(int? TaskId = null)
        {
            try
            {
                var client = GetClient();
                var response = client.Get(new TaskItem { Id = TaskId ?? 0 });
                return response.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting tasks from ActionHub Proxy {URL}", ex);
            }
        }

        /// <summary>
        /// Trigger a task
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="TriggerId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TriggerResponseDO TriggerTask(int? TaskId = null, int? TriggerId = null, string email = null)
        {
            try
            {
                var client = GetClient();
                var response = client.Get(new TriggerItemDO { Id = TaskId ?? 0, TriggerId = TriggerId ?? 0 });
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error triggering task from ActionHub Proxy {URL}", ex);
            }
        }

        /// <summary>
        /// Gets the triggered task status
        /// </summary>
        /// <param name="TriggerId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TriggerResponseDO GetTriggerTaskStatus(int TriggerId)
        {
            try
            {
                var client = GetClient();
                var response = client.Get(new TaskTriggerStatusDO { TriggerId = TriggerId });
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting trigger task status from ActionHub Proxy {URL}", ex);
            }
        }


    }
}

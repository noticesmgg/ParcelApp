using CoreServiceContracts.Authentication;
using CoreServiceContracts.HeartbeatApi.DataObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts.HeartbeatApi
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
        private JsonServiceClient GetClient(bool Reconnect = false, string ProcessName = "")
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
                        req.Headers[ADAuthProvider.C_App] = String.IsNullOrEmpty(ProcessName) ? "Hearbeat Service" : ProcessName;
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error connecting to Heartbeat Proxy {URL}", ex);
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
               URL = "http://mgg-pr-app01:9899/";

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
        public (bool Status, String Message) UpdateJobStatus(int JobId, DateTime? AsOfDate, Status taskStatus)
        {
            bool status = false;
            string jobName = "";

            try
            {
                if (AsOfDate == null)
                    AsOfDate = DateTime.Now;

                var item = new JobStatusDO
                {
                    JobId = JobId,
                    AsOfDate = AsOfDate,
                    Status = taskStatus,
                    TriggerId = Utilities.GetUniqueID(),
                };

                var data = GetClient().Get(item);
                status = data.Updated;
                jobName = data.Name;
                return (status, data.Message);
            }
            catch (Exception ex)
            {
                return (status, $"Error updating Heartbeat for {JobId}/{jobName}. Details: {ex.Message} {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Gets the Job details based on the Job Id or all jobs if JobId is not provided
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public JobStatusResponseDO[] GetJobDetails(int JobId = 0)
        {
            try
            {
                var request = new JobRequestDO
                {
                    JobId = JobId
                };
                
                var data = GetClient().Get(request);

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting Heartbeat details for {JobId}. Details: {ex.Message} {ex.StackTrace}");
            }
        }

        /// <summary>
        /// File Meta Data update
        /// </summary>
        /// <param name="ProcessId"></param>
        /// <param name="FileId"></param>
        /// <param name="isComplete"></param>
        /// <param name="ActualFileName"></param>
        /// <param name="AsOfDate"></param>
        /// <exception cref="Exception"></exception>
        public FileStatusResponseDO UpdateFileMetaData(int ProcessId, int FileId, bool isComplete, String ActualFileName = "", DateTime? AsOfDate = null, string? message = null)
        {
            try
            {
                if (AsOfDate == null)
                    AsOfDate = DateTime.Now;

                var item = new FileStatus
                {
                    ProcessId = ProcessId,
                    FileId = FileId,
                    ActualFileName = ActualFileName,
                    AsOfDate = AsOfDate,
                    Status = isComplete ? Status.Succeeded : Status.NA,
                    Message = message ?? string.Empty,
                    TriggerId = Utilities.GetUniqueID(),
                };
                var data = GetClient().Get(item);
                return data;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating Heartbeat for File {FileId} in Process {ProcessId}. Details: {ex.Message} {ex.StackTrace}");
            }

        }

    }
}

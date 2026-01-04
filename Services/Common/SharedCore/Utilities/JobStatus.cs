using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using SharedCore.DB;

namespace SharedCore.Utilities
{
    public class JobStatus
    {
        /// <summary>
        /// Retrives Job Id based on Job Name
        /// </summary>
        /// <param name="JobName">JobName</param>
        /// <returns>ID (JobId)</returns>
        public static int GetId(string JobName)
        {
            int jobID = -1;
            string sql = "Select ID from Job where Name = " + JobName.ToSQ();

            try
            {
                var dt = Database.Instance.DB.GetRecords(sql);

                if (dt.Rows.Count == 1)
                    return dt.Rows[0][0].ToSafeInt();

            }
            catch (Exception ex)
            {
                Logger.Error($"JobStatus: Exception pulling the JobId based on JobName {JobName}. Exception Details: {ex.StackTrace} {ex.Message}");
            }
            
            return jobID;
        }

        /// <summary>
        /// Method to display if the Job ran successfully
        /// </summary>
        /// <param name="jobID"></param>
        /// <param name="asofDate"></param>
        /// <returns></returns>
        public static (bool Status, int AllGoodInt) DidItRun(int jobID, DateTime? asofDate = null)
        {
            if (asofDate == null)
                asofDate = DateTime.Now.Date;


            string sql = $@"Select jobID, AllGoodInt From  jobstatus 
                                 WHERE JOBID = {jobID}
                                 and asofDate = {asofDate?.ToDateForDBFormat().ToSQ()}";

            int allGoodInt = 0;
            try
            {
                var dt = Database.Instance.DB.GetRecords(sql);

                if (dt.Rows.Count == 0)
                {
                    return (false, allGoodInt);
                }

                else
                {
                    allGoodInt = dt.Rows[0]["allGoodInt"].ToSafeInt();
                    return (true, allGoodInt);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"JobStatus: Exception pulling the Job Status based on JobId {jobID}. Exception Details: {ex.StackTrace} {ex.Message}");
                return (false, allGoodInt);
            }

        }

        /// <summary>
        /// Gets the mail Recipients for the Job
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        public static List<string> GetEmailRecipients(int JobId)
        {
            List<string> emailAddresses = new List<string>();

            string sql = "  Select email from JobEmailAddress WHERE JOBID = 3 or JOBID = " + JobId;
            
            try
            {
                var dt = Database.Instance.DB.GetRecords(sql);

                foreach (DataRow dr in dt.Rows)
                    emailAddresses.Add(dr[0].ToSafeString());

            }
            catch (Exception ex)
            {
                Logger.Error($"JobStatus: Exception pulling the Job email addressses based on JobName {JobId}. Exception Details: {ex.StackTrace} {ex.Message}");
            }


            return emailAddresses;

        }

        /// <summary>
        /// Gets the Job Name based on Job ID
        /// </summary>
        /// <param name="jobID"></param>
        /// <returns></returns>
        public static string GetJobName(int jobID)
        {
            string jobName = "";
            string sql = "  select Name from Job where ID = " + jobID;
            try
            { 
            System.Data.DataTable dt = Database.Instance.DB.GetRecords(sql);

            if (dt.Rows.Count == 1)
                return dt.Rows[0][0].ToSafeString();
            }
            catch (Exception ex)
            {
                Logger.Error($"JobStatus: Exception pulling the Job based on JobId {jobID}. Exception Details: {ex.StackTrace} {ex.Message}");
            }
            return jobName;
        }


        /// <summary>
        /// Updates Job Status table (or Mark as Complete)
        /// </summary>
        /// <param name="jobID"></param>
        /// <param name="asofdate"></param>
        /// <param name="allGoodInt"></param>
        /// <returns></returns>
        public static bool MarkCompleted(int jobID, DateTime asofdate, int allGoodInt = 0)
        {
            bool Status = false;

            try
            {
                var i = Database.Instance.DB.Execute($"Exec dbo.UpsertJobStatus {jobID},{asofdate.ToDateForDBFormat().ToSQ()}, {allGoodInt}");

                if (i >= 0)
                    Status = true;

            }
            catch (Exception ex)
            {
                Logger.Error($"JobStatus: Exception updating the Job status based on JobId {jobID}. Exception Details: {ex.StackTrace} {ex.Message}");
            }

            
            return Status;
        }

        /// <summary>
        /// Gets the Job Name based on Job ID
        /// </summary>
        /// <param name="jobID"></param>
        /// <returns></returns>
        public static string GetFundName(int jobID)
        {
            string sql = "  select FundName from Job where ID = " + jobID;
            System.Data.DataTable dt = Database.Instance.DB.GetRecords(sql);

            if (dt.Rows.Count == 1)
                return dt.Rows[0][0].ToSafeString();

            return "";

        }

        public static string GetVPMFundName(int jobID)
        {
            string sql = "  select FundNameVPM from Job where ID = " + jobID;
            System.Data.DataTable dt = Database.Instance.DB.GetRecords(sql);

            if (dt.Rows.Count == 1)
                return dt.Rows[0][0].ToSafeString();

            return "";

        }


        /// <summary>
        /// Retrieves if specific Allocation aspect based on Job ID
        /// </summary>
        /// <param name="jobID"></param>
        /// <returns></returns>
        public static int HasAllocationAspect(int jobID)
        {
            int hasAllocationAspect = -1;
            string sql = "  select HasAllocationAspect from Job where ID = " + jobID;

            System.Data.DataTable dt = Database.Instance.DB.GetRecords(sql);
            if (dt.Rows.Count == 1)
                return dt.Rows[0][0].ToSafeInt();

            return hasAllocationAspect;

        }


        /// <summary>
        /// Get RunsOnDay for the Job
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        public static string GetRunsOnDay(int JobId)
        {
            string sql = "  select RunsOn from Job where ID = " + JobId;

            System.Data.DataTable dt = Database.Instance.DB.GetRecords(sql);
            if (dt.Rows.Count >= 1)
            {
                return dt.Rows[0][0].ToSafeString();
            }

            return "";
        }


        public static Dictionary<String, (int FileId, int ProcessId)> GetFileMetaData(int JobId)
        {
            var fileMetaData = new Dictionary<String, (int FileId, int ProcessId)>(StringComparer.CurrentCultureIgnoreCase);
            string sql = $@"  Select f.Id as FileId, p.Id as ProcessId, f.FileMask from FH_Process p
                            INNER JOIN FH_FileInfo f on p.Id = f.ProcessId
                            Where p.JobId = {JobId} Order by 1";
            System.Data.DataTable dt = Database.Instance.DB.GetRecords(sql);
            if (dt.Rows.Count >= 1)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (fileMetaData.ContainsKey(dr["FileMask"].ToSafeString()))
                        continue;

                    fileMetaData.Add(dr["FileMask"].ToSafeString(), (dr["FileId"].ToSafeInt(), dr["ProcessId"].ToSafeInt()));
                }
            }
            return fileMetaData;
        }

    }
}

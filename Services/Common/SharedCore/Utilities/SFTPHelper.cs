using Renci.SshNet;
using Renci.SshNet.Sftp;
using SharedCore.DB;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SharedCore.Utilities
{
    public class SftpHelper : IDisposable   
    {
        private string host;
        private string username;
        private string password;
        private int port;
        private string privateKeyPath;
        private SftpClient client;
        private AuthMethod AuthMode = AuthMethod.Password;
        private enum AuthMethod
        {
            Password = 0,
            PrivateKey = 1,
            Both = 2
        }

        /// <summary>
        /// .ctor to initialize SFTP connection
        /// </summary>
        /// <param name="host"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="port"></param>
        /// <exception cref="Exception"></exception>
        public SftpHelper(string host, string username, string password, int port = 22)
        {
            this.host = host;
            this.username = username;
            this.password = password;
            this.port = port;

            try
            {
                client = new SftpClient(host, port, username, password);
                client.Connect();

            }
            catch (Exception e)
            {
                Logger.Error($"SftpHelper connection issues. Check details " + e.ToString());
                throw new Exception("SftpHelper connection issues. Check details", e);
            }
        }


        /// <summary>
        /// .ctor that pulls connection info from the DB based on the ConnectionName
        /// </summary>
        /// <param name="ConnectionName"></param>
        public SftpHelper(string ConnectionName)
        {
            String strSql = $"Select * from dbo.vw_FTPConnections Where ConnectionName = '{ConnectionName}'";
            try
            {
                var dt = Database.Instance.DB.GetRecords(strSql);
                if (dt.Rows.Count == 0)
                    throw new Exception($"Connection {ConnectionName} not found in the database");

                this.host = dt.Rows[0]["ServerURL"].ToSafeString();
                this.username = dt.Rows[0]["Username"].ToSafeString();
                this.password = dt.Rows[0]["Pwd"].ToSafeString();
                this.port = dt.Rows[0]["Port"].ToSafeInt();
                this.privateKeyPath = dt.Rows[0]["PrivateKeyFilePath"].ToSafeString();
                string authenticationMethod = dt.Rows[0]["AuthenticationMethod"].ToSafeString();

                Logger.Info($"SftpHelper DB values: host={this.host}, username={this.username}, port={this.port}, privateKeyPath={this.privateKeyPath}, authenticationMethod={authenticationMethod}");

                if (authenticationMethod.EqualsIgnoreCase("privateKey"))
                    AuthMode = AuthMethod.PrivateKey;
                else if (authenticationMethod.EqualsIgnoreCase("passwordAndPrivateKey"))
                    AuthMode = AuthMethod.Both;
                else
                    AuthMode = AuthMethod.Password;
                
                var connectionInfo = new ConnectionInfo(host, port, username, GetAuthenticationMethods());

                client = new SftpClient(connectionInfo);

                client.Connect();
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception pulling FTP Connection info from DB. Error " + ex.ToString());
                throw;
            }
        }
     
        /// <summary>
        /// Property to check if the SFTP connection is open
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return client != null && client.IsConnected;
        }

        /// <summary>
        /// Uploads List of Files to FTP
        /// </summary>
        /// <param name="localFilePaths"></param>
        /// <param name="remoteDirectory"></param>
        /// <returns></returns>
        public List<(string FileName, bool Status)> UploadFiles(List<string> localFilePaths, string remoteDirectory)
        {
            var results = new List<(string, bool)>();
            try
            {
                if (client.IsConnected)
                {
                    // Get list of files already in the remote directory
                    var filesInDir = client.ListDirectory(remoteDirectory)
                                           .Where(f => !f.IsDirectory)
                                           .Select(f => f.Name)
                                           .ToHashSet(StringComparer.OrdinalIgnoreCase);

                    foreach (var localFilePath in localFilePaths)
                    {
                        var fileName = Path.GetFileName(localFilePath);
                        var baseName = Path.GetFileNameWithoutExtension(fileName);
                        var ext = Path.GetExtension(fileName);
                        int count = 0;
                        string newFileName = fileName;

                        // Update revision number if file already exists
                        while (filesInDir.Contains(newFileName))
                        {
                            newFileName = $"{baseName}_R{++count}{ext}";
                            Logger.Info($"File already uploaded, Updated version. FileName: {newFileName}");
                        }

                        string remoteFilePath = Path.Combine(remoteDirectory, newFileName).Replace("\\", "/");
                        try
                        {
                            using (var fileStream = new FileStream(localFilePath, FileMode.Open))
                            {
                                client.UploadFile(fileStream, remoteFilePath);
                            }
                            results.Add((localFilePath, true));
                            filesInDir.Add(newFileName);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Error uploading file '{localFilePath}' to SFTP: {ex.Message}");
                            results.Add((localFilePath, false));
                        }
                    }
                }
                else
                {
                    foreach (var localFilePath in localFilePaths)
                    {
                        results.Add((localFilePath, false));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while Uploading files to SFTP: {ex.Message}");
                foreach (var localFilePath in localFilePaths)
                {
                    results.Add((localFilePath, false));
                }
            }
            return results;
        }

        /// <summary>
        /// Uploads a file to the SFTP server
        /// </summary>
        /// <param name="localFilePath">LocalFileName with the path (C:\Temp\file.txt) </param>
        /// <param name="remoteFilePath">Remote filepath along with the fileName (/remote/folder/file.txt) </param>
        /// <returns></returns>
        public bool UploadFile(string localFilePath, string remoteFilePath)
        {
            if (client.IsConnected)
            {
                using (var fileStream = new FileStream(localFilePath, FileMode.Open))
                {
                    client.UploadFile(fileStream, remoteFilePath);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Downloads a file from the SFTP server
        /// </summary>
        /// <param name="remoteFilePath"></param>
        /// <param name="localFilePath"></param>
        /// <returns></returns>
        public bool DownloadFile(string remoteFilePath, string localFilePath)
        {
            if (client.IsConnected)
            {
                try
                {
                    using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                    {
                        client.DownloadFile(remoteFilePath, fileStream);
                    }
                    return true;
                }
                catch (Renci.SshNet.Common.SftpPathNotFoundException ex)
                {
                    Logger.Error($"SFTP file not found: '{remoteFilePath}'. Skipping. Exception: {ex.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error downloading file '{remoteFilePath}': {ex.Message}");
                    return false;
                }
            }
            else
            {
                Logger.Warning("SFTP Connection is not open. Returning false");
                return false;
            }

        }

        /// <summary>
        /// Downloads all files from the remotePath that match the searchPattern and datePattern
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="Directories"></param>
        /// <param name="searchPattern"></param>
        /// <param name="datePattern"></param>
        /// <param name="retrieveTodaysDatedFiles"></param>
        /// <param name="OverrideFiles"></param>
        /// <param name="cutoffTime"></param>
        /// <returns></returns>
        public List<string> DownloadAllFiles(string remotePath, List<string> Directories, string searchPattern = null, string datePattern = null, bool retrieveTodaysDatedFiles = true, bool OverrideFiles= true, DateTime? cutoffTime = null)
        {
            var results = new List<string>();

            if (client.IsConnected)
            {
                if (string.IsNullOrEmpty(remotePath))
                    remotePath = client.WorkingDirectory;

                var files = client.ListDirectory(remotePath)
                    .Where(f => (string.IsNullOrEmpty(searchPattern) || f.Name.ContainsIgnoreCase(searchPattern))
                        && (string.IsNullOrEmpty(datePattern) || f.Name.ContainsIgnoreCase(datePattern))
                        && !f.IsDirectory
                        && (!retrieveTodaysDatedFiles || f.LastWriteTime.Date == DateTime.Today))
                    .OrderByDescending(f => f.LastWriteTime)
                    .ToList();

                foreach (var file in files)
                {
                    if (cutoffTime.HasValue && file.LastWriteTime <= cutoffTime.Value)
                    {
                        Logger.Info($"Skipping file '{file.Name}' because it was last written on {file.LastWriteTime} which is before the cutoff time {cutoffTime.Value}");
                        continue;
                    }

                    foreach (var localfilePath in Directories)
                    {
                        string localFileName = Path.Combine(localfilePath, file.Name);

                        if (!OverrideFiles && System.IO.File.Exists(localFileName))
                        {
                            Logger.Info($"Skipping file (already exists and OverrideFiles is false): '{file.Name}'");
                            continue;
                        }

                        Logger.Info($"Downloading file: '{file.Name}' {file.LastWriteTime.ToDateTimeForDBFormat()}");
                        bool status = DownloadFile(file.FullName, localFileName);
                        if (status)
                        {
                            results.Add(localFileName);
                        }
                        else
                        {
                            Logger.Warning($"Failed to download file: '{file.FullName}'");
                        }
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Checks if a file is currently open by another process
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsFileOpen(string path)
        {
            try
            {
                using (System.IO.FileStream inputStream = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                {
                    inputStream.Close();
                }
            }
            catch (System.IO.IOException ex)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Lists files in a directory on the SFTP server, optionally filtering by a regex pattern
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="matchPattern"></param>
        /// <returns></returns>
        public List<String> ListDirectory(string remotePath, string? matchPattern)
        {
            var fileNames = new List<String>();
            if (client.IsConnected)
            {
                var files = client.ListDirectory(remotePath);
                foreach (var file in files)
                {
                   if (matchPattern != null && !Regex.IsMatch(file.Name, matchPattern))
                        continue;

                    fileNames.Add(file.Name);
                }
             
            }
            return fileNames;
        }

        /// <summary>
        /// Property to get the current working directory on the SFTP server
        /// </summary>
        public string WorkingDirectory
        {
            get
            {
                if (client == null)
                {
                    throw new InvalidOperationException("SFTP client is not initialized.");
                }

                if (!client.IsConnected)
                {
                    throw new InvalidOperationException("SFTP client is not connected.");
                }

                return client.WorkingDirectory;
            }
        }

        /// <summary>
        /// Searches for files in the specified remotePath that match the given searchPattern
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public List<string>? SearchFiles(string remotePath, string searchPattern)
        {
            if (client.IsConnected)
            {
                var files = client.ListDirectory(remotePath);
                var regexPattern = "^" + Regex.Escape(searchPattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
                var matchedFiles = files.Where(f => Regex.IsMatch(f.Name, regexPattern)).Select(f => f.Name).ToList();                
                return matchedFiles;
            }
            else
                return null;
        }

        /// <summary>
        /// Downloads the latest file from the specified remotePath that matches the given searchPattern and datePattern
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localfilePath"></param>
        /// <param name="searchPattern"></param>
        /// <param name="datePattern"></param>
        /// <param name="retrieveTodaysDatedFiles"></param>
        /// <returns></returns>
        public (bool Status, string FileName) DownloadLatestFile(string remotePath, string localfilePath, string searchPattern = null,string datePattern= null , bool retrieveTodaysDatedFiles = true)
        {
            try
            {
                string fileName = string.Empty;
                bool Status = false;
                if (client.IsConnected)
                {
                    if (String.IsNullOrEmpty(remotePath))
                        remotePath = client.WorkingDirectory;

                    var files = client.ListDirectory(remotePath)
                                .Where(f => (string.IsNullOrEmpty(searchPattern) || f.Name.ContainsIgnoreCase(searchPattern)) && (string.IsNullOrEmpty(datePattern) || f.Name.ContainsIgnoreCase(datePattern)) && !f.IsDirectory &&
                                   (!retrieveTodaysDatedFiles || f.LastWriteTime.Date == DateTime.Today)
                                )
                                .OrderByDescending(f => f.LastWriteTime)
                                .ToList();
                    if (files.Any())
                    {
                        var latestFile = files.First();
                        fileName = Path.Combine(localfilePath, latestFile.Name);
                        Logger.Info($"Downloading latest file: '{latestFile.Name}' {latestFile.LastWriteTime.ToDateTimeForDBFormat()}");
                        Status = DownloadFile(latestFile.FullName, fileName);
                    }
                }
                return (Status, fileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error initializing SFTP client: {ex.Message}");
                return (false, string.Empty);
            }
        }

        /// <summary>
        /// Gets the last modified time of a file on the SFTP server
        /// </summary>
        /// <param name="remoteFilePath"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public DateTime GetLastModifiedTime(string remoteFilePath)
        {
            if (client == null)
                throw new InvalidOperationException("SFTP client is not initialized.");

            if (!client.IsConnected)
                throw new InvalidOperationException("SFTP client is not connected.");

            var file = client.GetAttributes(remoteFilePath);
            return file.LastWriteTime;
        }

        /// <summary>
        /// Disposes the SFTP client and disconnects from the server
        /// </summary>
        public void Dispose()
        {
            if (client != null && client.IsConnected)
                client.Disconnect();
        }


        /// <summary>
        /// Retrieves the appropriate authentication methods based on the AuthMode
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private AuthenticationMethod[] GetAuthenticationMethods()
        {
            switch (AuthMode)
            {
                case AuthMethod.Password:
                    return new AuthenticationMethod[]
                    {
                        new PasswordAuthenticationMethod(username, password)
                    };
                case AuthMethod.PrivateKey:
                    return new AuthenticationMethod[]
                    {
                        new PrivateKeyAuthenticationMethod(username, new PrivateKeyFile(privateKeyPath))
                    };
                case AuthMethod.Both:
                    return new AuthenticationMethod[]
                    {
                        new PasswordAuthenticationMethod(username, password),
                        new PrivateKeyAuthenticationMethod(username, new PrivateKeyFile(privateKeyPath))
                    };
                default:
                    throw new InvalidOperationException("Invalid authentication mode.");
            }
        }

    }

}

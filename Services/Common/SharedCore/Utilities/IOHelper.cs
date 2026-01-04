using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using OfficeOpenXml;
using Microsoft.VisualBasic.FileIO;
using Renci.SshNet;

namespace SharedCore.Utilities
{
    public class IOHelper
    {
        /// <summary>
        /// Creates a directory structure based on the date
        /// </summary>
        /// <param name="baseDirectory"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string CreateDirectory(string baseDirectory, DateTime date)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString("D2"); // Two-digit month
            string day = date.Day.ToString("D2"); // Two-digit day

            string fullPath = Path.Combine(baseDirectory, year, month, day);

            try
            {
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
                
            }
            catch (Exception ex)
            {
                Logger.Info($"Error creating directory: {fullPath}. Error details: {ex.StackTrace} {ex.Message}");
                throw;
            }
            

            return fullPath;
        }

        /// <summary>
        /// Changes the current directory of the provided SFTP client.
        /// </summary>
        /// <param name="client">The SftpClient instance.</param>
        /// <param name="path">The path to change to.</param>
        public static void ChangeDirectory(SftpClient client, string path)
        {
            if (client == null)
            {
                throw new InvalidOperationException("SFTP client is not initialized.");
            }

            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!client.IsConnected)
            {
                throw new InvalidOperationException("SFTP client is not connected.");
            }

            client.ChangeDirectory(path);
        }

        /// <summary>
        /// Copies a file to a destination path with the same file name
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        public static bool CopyFile(string sourceFile, string destinationPath)
        {
            try
            {
                //Get file name from the source file
                string fileName = Path.GetFileName(sourceFile);

                if (!Directory.Exists(destinationPath))
                    Directory.CreateDirectory(destinationPath);

                File.Copy(sourceFile, Path.Combine(destinationPath, fileName), true);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Info($"Error copying the file: {sourceFile}. Error details: {ex.StackTrace} {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Unzips a file to a destination path and returns a list of the extracted files
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        public static List<string> UnzipFiles(string zipFilePath, string destinationPath, bool DeleteZipFile = false)
        {
            List<string> extractedFiles = new List<string>();
            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string destinationFilePath = Path.Combine(destinationPath, entry.FullName);
                        if (!string.IsNullOrEmpty(entry.Name))
                        {
                            entry.ExtractToFile(destinationFilePath, overwrite: true);
                            extractedFiles.Add(destinationFilePath);
                        }
                    }
                }

                if (DeleteZipFile)
                    File.Delete(zipFilePath);
            }
            catch (Exception ex)
            {
                Logger.Info($"Error unzipping the file: {zipFilePath}. Error details: {ex.StackTrace} {ex.Message}");
                throw;
            }
            return extractedFiles;
        }

        /// <summary>
        /// Deletes a list of files
        /// </summary>
        /// <param name="files">List of files to be deleted</param>
        /// <returns>Status</returns>
        public static bool DeleteFiles(List<string> files)
        {
            bool status = true;

            foreach (string file in files)
            {
                if(!DeleteFile(file) && status)
                    status = false;
            }

            return status;
        }

        /// <summary>
        /// Deletes a file from the file system 
        /// </summary>
        /// <param name="filePath">FilePath (Full)</param>
        /// <returns>Status</returns>
        public static bool DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Info($"Error deleting the file: {filePath}. Error details: {ex.StackTrace} {ex.Message}");
                return false;
            }
        }

        /// <summary>
        ///  Converts a CSV file to XLSX format   
        /// </summary>
        /// <param name="csvFilePath"></param>
        /// <param name="xlsxFilePath"></param>
        /// <returns></returns>

        public static string CsvToXLSXFile(string csvFilePath, string xlsxFilePath)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            using var parser = new TextFieldParser(csvFilePath, Encoding.UTF8)
            {
                TextFieldType = FieldType.Delimited,
                Delimiters = new[] { "," },
                HasFieldsEnclosedInQuotes = true
            };

            int row = 1;
            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();
                for (int col = 0; col < fields.Length; col++)
                {
                    var value = fields[col];

                    // Try to infer type: int, double, DateTime, else string
                    if (int.TryParse(value, out int intVal))
                        worksheet.Cells[row, col + 1].Value = intVal;
                    else if (double.TryParse(value, out double doubleVal))
                        worksheet.Cells[row, col + 1].Value = doubleVal;
                    else
                        worksheet.Cells[row, col + 1].Value = value;
                }
                row++;
            }

            package.SaveAs(new FileInfo(xlsxFilePath));
            return xlsxFilePath;
        }

        /// <summary>
        /// Show a Message box if the file is already open or in use
        /// </summary>
        ///  

        public static bool NotifyIfFileInUse(string filePath)
        {
            if (!File.Exists(filePath))
                return true;
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return true;
            }
        }

    }
}

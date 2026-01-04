using CsvHelper.Configuration;
using CsvHelper;
using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using OfficeOpenXml;
using Serilog;
using System.Reflection;
using ClosedXML.Excel;

namespace SharedCore.Utilities
{
    public static class DataHelpers
    {
        public static string EPPlusKey { get; private set; }
           = "g+YBVovoumm+fKaLIlrodcq4xO9OEPKlVSqyNTmr+HU8XV425aDyaDr3dAJv6RxglzJQLRn0B4tF6MCxZQnEvwEGRDZEQjJC6QcFAHIBAgIA";
        /// <summary>
        /// Checks for Case Sensitive List
        /// </summary>
        /// <param name="StringList">Enumerable</param>
        /// <param name="keyword">Search Keyword</param>
        /// <returns>Boolean Flag</returns>
        public static bool ContainsIgnoreCase(this IEnumerable<string> StringList, string keyword)
        {
            return StringList.Any(s => s.Equals(keyword, StringComparison.OrdinalIgnoreCase));
        }


        /// <summary>
        /// Retrieves a DataTable based on the CSV Data with Headers
        /// </summary>
        /// <param name="filePath">CSV FilePath</param>
        /// <param name="delimiter">Is File has Header Row</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DataTable GetDataTable(this String filePath, string delimiter = ",", bool HeaderRecord = true)
        {

            try
            {
                var dataTable = new DataTable();
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = delimiter,
                    HasHeaderRecord = HeaderRecord,
                    IgnoreBlankLines = true,
                    TrimOptions = TrimOptions.Trim,
                    BadDataFound = null,
                    MissingFieldFound = null
                };

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    using (var dr = new CsvDataReader(csv))
                    {
                        dataTable.Load(dr);
                    }
                }

                return dataTable;

            }

            catch (Exception ex)
            {
                throw new Exception(String.Format("GetDataTableFromCSV: Details: {0}", ex.ToString()));
            }

        }
        public static string ConvertXlsmToCsv(string xlsmFilePath, string worksheetName)
        {
            ExcelPackage.License.SetCommercial(EPPlusKey);
            string csvFilePath = Path.ChangeExtension(xlsmFilePath, ".csv");

            using (var package = new ExcelPackage(new FileInfo(xlsmFilePath)))
            {
                var worksheet = package.Workbook.Worksheets[worksheetName];
                if (worksheet == null)
                    throw new Exception($"Sheet '{worksheetName}' not found in {xlsmFilePath}.");

                int headerRow = -1;
                int maxRow = worksheet.Dimension.End.Row;
                int maxCol = worksheet.Dimension.End.Column;

                for (int row = 1; row <= maxRow; row++)
                {
                    var cellValue = worksheet.Cells[row, 1].Text.Trim();
                    if (cellValue.Equals("LoanX ID", StringComparison.OrdinalIgnoreCase))
                    {
                        headerRow = row;
                        break;
                    }
                }

                if (headerRow == -1)
                    throw new Exception($"Header row with 'LoanX ID' not found in sheet '{worksheetName}' of {xlsmFilePath}.");

                using (var writer = new StreamWriter(csvFilePath, false, Encoding.UTF8))
                {
                    // Write header
                    var headerValues = new string[maxCol];
                    for (int col = 1; col <= maxCol; col++)
                    {
                        var cellValue = worksheet.Cells[headerRow, col].Text;
                        headerValues[col - 1] = $"\"{cellValue.Replace("\"", "\"\"")}\"";
                    }
                    writer.WriteLine(string.Join(",", headerValues));

                    // Write all rows after header, including blank rows
                    for (int row = headerRow + 1; row <= maxRow; row++)
                    {
                        var values = new string[maxCol];
                        for (int col = 1; col <= maxCol; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Text;
                            values[col - 1] = $"\"{cellValue.Replace("\"", "\"\"")}\"";
                        }
                        writer.WriteLine(string.Join(",", values));
                    }
                }
            }

            Logger.Info($"Excel .xlsm converted to CSV: {Path.GetFileName(csvFilePath)}");
            return csvFilePath;
        }
        public static string ConvertXlsxToCsv(string excelFilePath)
        {
            ExcelPackage.License.SetCommercial(EPPlusKey);
            string csvFilePath = Path.ChangeExtension(excelFilePath, ".csv");

            using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new Exception("No worksheet found in Excel file.");

                var rowCount = worksheet.Dimension.End.Row;
                var colCount = worksheet.Dimension.End.Column;

                using (var writer = new StreamWriter(csvFilePath, false, Encoding.UTF8))
                {
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var values = new string[colCount];
                        for (int col = 1; col <= colCount; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Text;
                            values[col - 1] = $"\"{cellValue.Replace("\"", "\"\"")}\""; // Escape quotes
                        }
                        writer.WriteLine(string.Join(",", values));
                    }
                }
            }

            Logger.Info($"Excel converted to CSV: {Path.GetFileName(csvFilePath)}");
            return csvFilePath;
        }


        /// <summary>
        /// Normalizes CSV rows to ensure all rows have the same number of columns as the header. 
        /// In some cases we receive more number of header columns than the data rows (Quintet files)
        /// and we couldnt have them fix the issue on their end (period)
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="delimiter"></param>
        public static void NormalizeCsvRows(string inputFilePath, string outputFilePath, string delimiter = ",")
        {
            var lines = File.ReadAllLines(inputFilePath);
            if (lines.Length == 0) return;

            var header = lines[0];
            var headerColumns = header.Split(delimiter);
            int columnCount = headerColumns.Length;

            using (var writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
            {
                writer.WriteLine(header);

                for (int i = 1; i < lines.Length; i++)
                {
                    var fields = lines[i].Split(delimiter);
                    var normalizedFields = fields.ToList();

                    // Pad with empty strings if not enough fields
                    while (normalizedFields.Count < columnCount)
                    {
                        normalizedFields.Add("");
                    }

                    // Optionally, trim extra fields if too many
                    if (normalizedFields.Count > columnCount)
                    {
                        normalizedFields = normalizedFields.Take(columnCount).ToList();
                    }

                    writer.WriteLine(string.Join(delimiter, normalizedFields));
                }
            }
        }

        public static void FormatEuropeanColumns(ref DataTable dt, List<string> columnsToFormat)
        {
            CultureInfo euroCulture = new CultureInfo("de-DE");
            CultureInfo usCulture = CultureInfo.InvariantCulture;

            foreach (DataRow row in dt.Rows)
            {
                foreach (string column in columnsToFormat)
                {
                    if (row[column] != DBNull.Value)
                    {
                        string europeanNumber = row[column].ToString();
                        if (double.TryParse(europeanNumber, NumberStyles.Number, euroCulture, out double usNumber))
                        {
                            row[column] = usNumber.ToString("N2", usCulture);
                        }
                    }
                }
            }
        }
       
        public static DataTable GetDataTableFromCSVBF(string fileName, bool hasHeaderRow = true, string delimiter = ",", int startRowIndex = 0, bool addDoubleQuotes = false, bool trimcolumns = false)
        {
            var dataTable = new DataTable();
            try
            {
                using (var parser = new TextFieldParser(fileName))
                {
                    parser.Delimiters = new[] { delimiter };
                    parser.HasFieldsEnclosedInQuotes = true;

                    // Skip rows up to startRow
                    for (int currentRow = 0; currentRow < startRowIndex; currentRow++)
                    {
                        if (parser.EndOfData)
                        {
                            throw new Exception("File has fewer rows than startRow");
                        }
                        parser.ReadFields();
                    }

                    // set up double quotes
                    string dQuotes = "";
                    if (addDoubleQuotes)
                    {
                        dQuotes = "\"";
                    }

                    int lineCounter = 0;

                    while (true)
                    {
                        lineCounter++;

                        if (lineCounter == 4897)
                        {
                            double ss = 2.0;
                            ss++;
                        }

                        var fields = parser.ReadFields();
                        if (fields == null) break;

                        // Trim each value to 100 characters if necessary
                        if (trimcolumns)
                        {
                            for (int i = 0; i < fields.Length; i++)
                            {
                                if (fields[i] != null && fields[i].Length > 100)
                                    fields[i] = fields[i].Substring(0, 100);
                            }
                        }

                        if (dataTable.Columns.Count == 0)
                        {
                            var columnIndex = 0;

                            foreach (var field in fields)
                            {
                                if (dataTable.Columns.Contains(field.ToLower()))
                                    dataTable.Columns.Add(field.ToLower() + "_" + columnIndex++, typeof(string));
                                else
                                    dataTable.Columns.Add(field.ToLower(), typeof(string));
                            }
                            // Hack as Quintet is not giving right file information
                            // Add an extra column if the field count does not match the column count
                            if (fileName.ContainsIgnoreCase("Global_CshNxc_JRN_"))
                            {
                                dataTable.Columns.Add("extra_column", typeof(string));
                            }

                            if (!hasHeaderRow) // File has no Header Row
                            {
                                var dataRow = dataTable.NewRow();
                                for (var i = 0; i < fields.Length; i++)
                                    dataRow[i] = fields[i];

                                dataTable.Rows.Add(dataRow);
                            }
                        }
                        else
                        {
                            if (fields.Length != dataTable.Columns.Count)
                            {
                                // throw new Exception("Field count does not match column count");
                                Log.Error("Field count does not match column count.Field count = " + fields.Length + " and Columns count = " + dataTable.Columns.Count + " For the row = " + dataTable.Rows.Count);
                                continue;

                            }

                            var dataRow = dataTable.NewRow();

                            for (var i = 0; i < fields.Length; i++)
                                dataRow[i] = dQuotes + fields[i] + dQuotes;


                            dataTable.Rows.Add(dataRow);
                        }
                    }
                }

                return dataTable;
            }

            catch (Exception ex)
            {
                throw new Exception(string.Format("GetDataTableFromCSV: Details: {0}", ex));
            }
        }

        public static string exportDatatableToCSV(DataTable dt, string delimiter = ",", bool includeHeader = true)
        {
            var sb = new StringBuilder();
            var columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            if (includeHeader) sb.AppendLine(string.Join(delimiter, columnNames));
            foreach (DataRow row in dt.Rows)
            {
                var fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                sb.AppendLine(string.Join(delimiter, fields));
            }

            return sb.ToString();
        }


        public static string GetCSVLineFromDataRow(DataRow dr, DataTable dt, bool addAsOfDate = false, string asofdateString = "")
        {
            StringBuilder sb = new StringBuilder();
            string doubleQ = "\"";

            foreach (DataColumn col in dt.Columns)
            {
                string val = dr[col.ColumnName].ToSafeString();
                sb.Append(doubleQ).Append(val).Append(doubleQ).Append(",");
            }

            if (addAsOfDate)
            {
                sb.Append(doubleQ).Append(asofdateString).Append(doubleQ).Append(",");
            }

            // Remove the trailing comma
            if (sb.Length > 0)
            {
                sb.Length--;
            }

            return sb.ToString();
        }

        public static string ToCsv(this DataTable dataTable, string filePath, string delimiter = ",")
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = delimiter,
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true,
                    TrimOptions = TrimOptions.Trim,
                    BadDataFound = null
                };

                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, config))
                {
                    // Write the header
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        csv.WriteField(column.ColumnName);
                    }
                    csv.NextRecord();

                    // Write the rows
                    foreach (DataRow row in dataTable.Rows)
                    {
                        foreach (var field in row.ItemArray)
                        {
                            csv.WriteField(field);
                        }
                        csv.NextRecord();
                    }
                }

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("ToCsv: Details: {0}", ex.ToString()));
            }
        }

        public static void ConvertExcelToCsv(string excelFilePath, string csvOutputPath, string workSheetName = "")
        {
            try
            {
                using (var workbook = new XLWorkbook(excelFilePath))
                {
                    IXLWorksheet worksheet;
                    // if workSheetName is empty, then get the first one, otherwise get the one with the name
                    if (workSheetName == "")
                        worksheet = workbook.Worksheets.First();
                    else
                        worksheet = workbook.Worksheets.Where(x => x.Name == workSheetName).FirstOrDefault();

                    // Check if the worksheet is found
                    if (worksheet == null)
                    {
                        throw new Exception($"The requested worksheet '{workSheetName}' cannot be found in the provided Excel file.");
                    }

                    var csvData = new StringBuilder();

                    bool firstRow = true;
                    foreach (var row in worksheet.RangeUsed().Rows())
                    {
                        if (!firstRow)
                        {
                            csvData.AppendLine();
                        }
                        else
                        {
                            firstRow = false;
                        }

                        bool firstColumn = true;
                        foreach (var cell in row.Cells())
                        {
                            if (!firstColumn)
                            {
                                csvData.Append(",");
                            }
                            else
                            {
                                firstColumn = false;
                            }

                            var cellValue = cell.Value.ToString();

                            // Handle your cell values here and escape necessary characters
                            if (cellValue.Contains(",") || cellValue.Contains("\"") || cellValue.Contains("\n"))
                            {
                                cellValue = $"\"{cellValue}\"";
                            }

                            csvData.Append(cellValue);
                        }
                    }

                    File.WriteAllText(csvOutputPath, csvData.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"ConvertExcelToCsv: Details: {ex}");
            }
        }

        public static bool IsFileValidAndReady(this string filePath, string expectedFileTypeExtension = "", string expectedName = "")
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    Logger.Info($"File not found: {filePath}");
                    return false;
                }
                using (var sftpHelper = new SftpHelper("Alter Domus SFTP"))
                {
                    if (sftpHelper.IsFileOpen(filePath))
                    {
                        Logger.Info($"File is already open: {filePath}");
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(expectedFileTypeExtension) && !BasicExtensions.EndsWithIgnoreCase(filePath, expectedFileTypeExtension))
                {
                    Logger.Info($"File is not a {expectedFileTypeExtension} file: {filePath}");
                    return false;
                }
                if (!string.IsNullOrEmpty(expectedName) && !BasicExtensions.ContainsIgnoreCase(Path.GetFileNameWithoutExtension(filePath), expectedName))
                {
                    Logger.Info($"File name does not contain {expectedName}: {filePath}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Info($"Exception in IsValidAndReadyForProcessing: {ex}");
                return false;
            }
        }

        public static DataTable GetRecords(string sql, int CommandTimeOut)
        {
            CommandTimeOut = 60;
            return GetRecords(sql, CommandTimeOut);
        }

        /// <summary>
        /// Returns fixed width file from DataTable
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="filePath"></param>
        /// <param name="columnWidths"></param>
        /// <returns></returns>
        public static string ToFixedWidth(this DataTable dataTable, string filePath, Dictionary<string, int> columnWidths)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = " ", // Use space as delimiter for fixed-width
                HasHeaderRecord = true, // Include header record
            }))
            {
                // Write the header
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (columnWidths.TryGetValue(column.ColumnName, out int width))
                    {
                        writer.Write(column.ColumnName.PadRight(width));
                    }
                    else
                    {
                        writer.Write(column.ColumnName.PadRight(10)); // Default width if not specified
                    }
                }
                writer.WriteLine();

                csv.Context.RegisterClassMap(new FixedWidthClassMap(columnWidths));

                foreach (DataRow row in dataTable.Rows)
                {
                    csv.WriteRecord(row);
                    csv.NextRecord();
                }
            }

            return filePath;
        }

        public class FixedWidthClassMap : ClassMap<DataRow>
        {
            public FixedWidthClassMap(Dictionary<string, int> columnWidths)
            {
                foreach (var column in columnWidths)
                {
                    Map(m => m[column.Key]).Name(column.Key).TypeConverterOption.Format(new string('0', column.Value));
                }
            }
        }

        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            // Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                // Setting column names as Property names
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    // Inserting property values to DataTable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static DateTime getPrevBusinessDay(DateTime seDate)
        {
            DateTime returnDate = seDate;

            while (true)
            {
                returnDate = returnDate.AddDays(-1);

                if (returnDate.DayOfWeek != DayOfWeek.Sunday && returnDate.DayOfWeek != DayOfWeek.Saturday)
                {
                    return returnDate;
                }
            }

        }

        public static string RemoveLeadingZeros(string input)
        {
            string result = input.TrimStart('0');

            if (string.IsNullOrEmpty(result))
            {
                return "0";
            }

            return result;
        }
        public static string ToHTML(this List<string> inputStrings)
        {
            string html = "<table style=\"border-collapse: collapse \">";

            html += "<tr style=\"background-color: #f2f2f2;\">";
            html += "Files moved";
            html += "</tr>";

            foreach (string s in inputStrings)
            {
                html += "<tr style=\"background-color: #ffffff;\">";
                html += "<td style=\"border: 1px solid black;\">";
                html += s;
                html += "</td>";
                html += "</tr>";
            }

            html += "</table>";

            return html;

        }
        public static string ToHTML(this DataTable dt)
        {
            string html = $"<table style=\"border-collapse: collapse\">";
            html += "<tr style=\"background-color: #7094db;\">";
            foreach (DataColumn dc in dt.Columns)
            {
                html += "<td style=\"border: 1px solid black; color: black; padding-right: 30px;padding-left: 30px;\">";
                html += dc.ColumnName;
                html += "</td>";
            }
            html += "</tr>";

            foreach (DataRow dr in dt.Rows)
            {
                html += "<tr style=\"background-color: #ffffff;\">";
                foreach (DataColumn dc in dt.Columns)
                {
                    html += "<td style=\"border: 1px solid black;padding-right: 30px;padding-left: 30px\">";
                    html += dc.DataType.Equals(typeof(int)) ? dr[dc.ColumnName].ToSafeInt() : dr[dc.ColumnName].ToSafeString();
                    html += "</td>";
                }
                html += "</tr>";
            }
            html += "</table>";
            html += "<br>";

            return html;

        }

        public static string ToHTML(this DataTable dt, Dictionary<string, Type> colTypeMap = null, Dictionary<string, string> colFormatMap = null, int fontSizePt=12)
        {
            string fontSizeStyle = $"font-size: {fontSizePt}pt;";

            string html = $"<table style=\"border-collapse: collapse; border: 3px solid black; padding: 0px; {fontSizeStyle} \">";

            html += "<tr style=\"background-color: #003366; font-family: Calibri; font-size: 12px; font-weight: bold;\">";
            foreach (DataColumn dc in dt.Columns)
            {
                html += $"<td style=\"color: #FFFFFF; padding-right: 10px; padding-left: 10px; border-top: 3px solid black; border-bottom: 1px solid #CCCCCC;  padding-top: 5px; padding-bottom: 5px;\">{dc.ColumnName}</td>";
            }
            html += "</tr>";

            foreach (DataRow dr in dt.Rows)
            {
                html += "<tr style=\"background-color: #ffffff; font-family: Calibri; font-size: 12px;\">";

                foreach (DataColumn dc in dt.Columns)
                {
                    if (dt.Rows.IndexOf(dr) == dt.Rows.Count - 1)
                        html += "<td style=\"padding-right: 10px; padding-left: 10px; border-top: 1px solid #CCCCCC; border-bottom : 3px solid black; padding-top: 5px; padding-bottom: 5px;\">";
                    else
                        html += "<td style=\"padding-right: 10px; padding-left: 10px; border-top: 1px solid #CCCCCC; border-bottom : 1px solid #CCCCCC; padding-top: 5px; padding-bottom: 5px;\">";

                    object cellValue = dr[dc.ColumnName];

                    double doubleValue;
                    if (double.TryParse(cellValue.ToString(), out doubleValue))
                    {
                        if (colTypeMap != null && colTypeMap.TryGetValue(dc.ColumnName, out Type columnType))
                        {
                            if (columnType == typeof(int))
                            {
                                int value = cellValue.ToSafeInt();
                                html += colFormatMap != null && colFormatMap.TryGetValue(dc.ColumnName, out string format)
                                    ? value.ToString(format)
                                    : value.ToString("$#,##0;($#,##0);-");
                            }
                            else if (columnType == typeof(double) || columnType == typeof(decimal))
                            {
                                double value = cellValue.ToSafeDouble();
                                html += colFormatMap != null && colFormatMap.TryGetValue(dc.ColumnName, out string format)
                                    ? value.ToString(format)
                                    : value.ToString("N2");
                            }
                            else
                            {
                                html += cellValue.ToSafeString();
                            }
                        }
                        else
                        {
                            if (dc.DataType == typeof(int))
                            {
                                html += cellValue.ToSafeInt().ToString("$#,##0;($#,##0);-");
                            }
                            else if (dc.DataType == typeof(double) || dc.DataType == typeof(decimal))
                            {
                                html += cellValue.ToSafeDouble().ToString("N2");
                            }
                            else
                            {
                                html += cellValue.ToSafeString();
                            }
                        }
                    }
                    else if (cellValue is DateTime dtValue || DateTime.TryParse(cellValue.ToString(), out dtValue))
                    {
                        if (colFormatMap != null && colFormatMap.TryGetValue(dc.ColumnName, out string format))
                            html += dtValue.ToString(format);
                        else
                            html += dtValue.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        html += cellValue.ToSafeString();
                    }
                    html += "</td>";
                }
                html += "</tr>";
            }

            html += "</table><br>";
            return html;
        }

        public static void ReplaceNullStringsWithEmpty(DataRow dr)
        {
            try
            {

                for (int i = 0; i < dr.ItemArray.Length; i++)
                {
                    if (dr[i].GetType() != typeof(DBNull) && dr[i].ToString().Equals("", StringComparison.OrdinalIgnoreCase))
                    {

                        //Console.WriteLine(dr[i].GetType());

                        dr[i] = "";
                    }



                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static string ToPrintConsole(this DataTable table)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            // Calculate column widths
            int[] columnWidths = new int[table.Columns.Count];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                columnWidths[i] = table.Columns[i].ColumnName.Length;
                foreach (DataRow row in table.Rows)
                {
                    int cellLength = row[i].ToSafeString().Length;
                    if (cellLength > columnWidths[i])
                        columnWidths[i] = cellLength;
                }
            }
            // Print separator line
            sb.AppendLine(new string('-', columnWidths.Sum() + table.Columns.Count * 3 + 1));

            // Print column headers
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sb.Append($"| {table.Columns[i].ColumnName.PadRight(columnWidths[i])} ");
            }
            sb.AppendLine("|");

            // Print separator line
            sb.AppendLine(new string('-', columnWidths.Sum() + table.Columns.Count * 3 + 1));

            // Print rows
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sb.Append($"| {row[i].ToSafeString().PadRight(columnWidths[i])} ");
                }
                sb.AppendLine("|");
            }

            // Print separator line
            sb.AppendLine(new string('-', columnWidths.Sum() + table.Columns.Count * 3 + 1));

            return sb.ToString();
        }


        public static string EmailBody(List<string> downloadedFilePaths, string sourceDirectory, List<string> outputDirectories)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<p>Below {downloadedFilePaths.Count} files were copied from <b>{sourceDirectory}</b> to the following output directories:</p>");
            sb.AppendLine("<ul>");
            foreach (var dir in outputDirectories)
                sb.AppendLine($"<li>{dir}</li>");
            sb.AppendLine("</ul>");
            sb.AppendLine("<table border='1' cellpadding='5' style='border-collapse:collapse;font-family:calibri;'>");
            sb.AppendLine("<tr><th>#</th><th>File Name</th><th>Full Path</th></tr>");
            int i = 1;
            foreach (var path in downloadedFilePaths)
            {
                var fileName = Path.GetFileName(path);
                sb.AppendLine($"<tr><td>{i++}</td><td>{fileName}</td><td>{path}</td></tr>");
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }

        public static string FormatPercentage(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            if (double.TryParse(value.ToString(), out double number))
                return $"{number * 100:0.00}%";
            return string.Empty;
        }
    }
}

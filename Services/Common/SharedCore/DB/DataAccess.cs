using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using ServiceStack;
using SharedCore.Utilities;

namespace SharedCore.DB
{
    public class DataAccess : IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public DataAccess(string connectionString)
        {
            try
            {
                _connectionString = connectionString;
                _connection = new SqlConnection(_connectionString);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in DataAccess constructor: {ex.Message} {ex.StackTrace}. Connection String : {connectionString}");
                throw new Exception($"Error in DataAccess constructor: {ex.Message}", ex);
            }
            
        }

        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            return _connection.Query<T>(sql, param);
        }

        public T QuerySingle<T>(string sql, object param = null)
        {
            return _connection.QuerySingle<T>(sql, param);
        }

        public T QuerySingleOrDefault<T>(string sql, object param = null)
        {
            return _connection.QuerySingleOrDefault<T>(sql, param);
        }

        public int Execute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.Execute(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }

        public T ExecuteScalar<T>(string sql, object param = null)
        {
            return _connection.ExecuteScalar<T>(sql, param);
        }   

        public DataSet GetDataset(string sql, object param = null)
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable();
            dataSet.Tables.Add(dataTable);

            using (var reader = _connection.ExecuteReader(sql, param))
            {
                dataTable.Load(reader);
            }

            return dataSet;
        }

        public int ExecuteNonQuery(string sql, Dictionary<String,object> parameterMap)
        {
            var parameters = new DynamicParameters();

            foreach (var kvp in parameterMap)
            {
                if(kvp.Value != null && kvp.Value != DBNull.Value)
                    parameters.Add(kvp.Key, kvp.Value);
            }

            return Execute(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public DataTable GetDataTable(string sql, Dictionary<String, object> parameterMap)
        {
            var parameters = new DynamicParameters();
            var dataTable = new DataTable();
            foreach (var kvp in parameterMap)
            {
                if (kvp.Value != null && kvp.Value != DBNull.Value)
                    parameters.Add(kvp.Key, kvp.Value);
            }

            try
            {
                using (var reader = _connection.ExecuteReader(sql, parameters, commandTimeout:60, commandType: CommandType.StoredProcedure))
                {
                    dataTable.Load(reader);
                }

            }
            catch (Exception ex)
            {
                Logger.Error($"Error in DB.GetDataTable: {ex.Message}");
                throw new Exception(ex.Message, ex);
            }

            
            if (dataTable != null && dataTable.Columns.Count > 0)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (column.DataType == typeof(string))
                    {
                        column.MaxLength = -1; // Remove the length restriction
                    }

                    if (!column.AllowDBNull)
                        column.AllowDBNull = true;

                    if (column.AutoIncrement)
                        column.AutoIncrement = false;

                }

                dataTable.Constraints.Clear();
            }

            return dataTable;
        }

        public DataTable GetRecords(string sql, object param = null)
        {
            var dataTable = new DataTable();

            using (var reader = _connection.ExecuteReader(sql, param, commandTimeout:60))
            {
                dataTable.Load(reader);
            }

            if (dataTable != null && dataTable.Columns.Count > 0)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (column.DataType == typeof(string))
                    {
                        column.MaxLength = -1; // Remove the length restriction
                    }

                    if(!column.AllowDBNull)
                        column.AllowDBNull = true;

                    if(column.AutoIncrement)
                        column.AutoIncrement = false;

                }

                dataTable.Constraints.Clear();
            }           

            return dataTable;
        }

        public bool SaveBulkRecords(DataTable dt, string destinationTable, Dictionary<string, string> columnMapping, int rowsToPersist = 1000)
        {
            var flag = false;

            try
            {
                //For maintaining the Identity column in destination table
                //SqlBulkCopy sbc = new SqlBulkCopy(Connection, SqlBulkCopyOptions.KeepIdentity)

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlBulkCopy sbc = new SqlBulkCopy(connection))
                    {
                        sbc.DestinationTableName = destinationTable;
                        sbc.BulkCopyTimeout = 60;
                        var columnList = GetRecords($"Select Top 1 * from {destinationTable}");
                        Dictionary<String, String> ColMap = new Dictionary<string, String>(StringComparer.CurrentCultureIgnoreCase);

                        foreach (DataColumn c in columnList.Columns)
                            ColMap[c.ColumnName] = c.ColumnName;


                        // Number of records to be processed in one go
                        sbc.BatchSize = rowsToPersist;

                        if (columnMapping == null || columnMapping.Count == 0)
                        {
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (ColMap.ContainsKey(dc.ColumnName))
                                {
                                    //if(dc.DataType.ToString() != ColMap[dc.ColumnName])
                                    //  s += ($"{dc.ColumnName} = Code: {dc.DataType.ToString()} \t DB: {ColMap[dc.ColumnName]} {Environment.NewLine}");
                                    sbc.ColumnMappings.Add(dc.ColumnName, ColMap[dc.ColumnName]);
                                }
                                else
                                    Console.WriteLine($"Skipped {dc.ColumnName}");
                            }
                        }
                        else
                        {
                            foreach (var c in columnMapping)
                                if (ColMap.ContainsKey(c.Value))
                                    sbc.ColumnMappings.Add(c.Key, ColMap[c.Value]);
                        }

                        // Finally write to server
                        sbc.WriteToServer(dt);

                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SaveBulkRecords:Error persisting records to database: " + ex.Message);
            }
            return flag;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }       
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCore.DB
{
    public sealed class Database : IDisposable
    {
        private static Database instance;

        public  DataAccess DB { get; set; }
        private string connectionString = "Server=MGG-PR-DB01;Database=DataMart;Trusted_Connection=True;TrustServerCertificate=True";
        private string uatConnectionString = "Server=MGG-UA-DB01;Database=DataMart;Trusted_Connection=True;TrustServerCertificate=True";
        private string locConnectionString = "Server=localhost;Database=RAAM;Trusted_Connection=True;TrustServerCertificate=True";

        private Database()
        {
            if (Overrides.Environment == Overrides.EnvironmentType.Local)
                connectionString = locConnectionString;

            if (Overrides.Environment == Overrides.EnvironmentType.UAT)
                connectionString = uatConnectionString;

            if (Overrides.SQLConnectionString != null)
                connectionString = Overrides.SQLConnectionString;
            
            DB = new DataAccess(connectionString);
        }


        /// <summary>
        /// Instance
        /// </summary>
        public static Database Instance => instance ?? (instance = new Database());


        /// <summary>
        /// Dispose Pattern
        /// </summary>
        /// <param name="disposing">Object Variable to maintain State</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DB != null)
                {
                    DB.Dispose();
                }
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Public Method to dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace n01629177Cumulative1.Models
{
    public class SchoolDbContext
    {
        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "school_db"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }

        protected static string ConnectionString
        {
            get
            {
                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password;
            }
        }

        /// <summary>
        /// Creates a connection to the database using the connection string defined
        /// in `SchoolDbContext.cs`
        /// </summary>
        /// <example>
        /// MySqlConnection connection_to_school_db = school_db.AccessDatabase();
        /// connection_to_school_db.Open();
        /// ...
        /// connection_to_school_db.Close();
        /// </example>
        /// <returns>A `MySqlConnection` object to the `school_db` database.</returns>
        public MySqlConnection AccessDatabase()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
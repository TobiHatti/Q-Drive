using System;
using System.Data;
using MySql.Data.MySqlClient;

// Q-Drive Network-Drive Manager
// Copyright(C) 2020 Tobias Hattinger

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<https://www.gnu.org/licenses/>.

namespace QDriveLib
{
    public class WrapMySQLConDat
    {
        public string Hostname { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class WrapMySQL : WrapSQL
    {
        #region Fields and Properties

        private readonly string connectionString = string.Empty;
        private readonly MySqlConnection connection = null;
        private MySqlTransaction transaction = null;
        

        /// <summary>
        /// SQL-Connection object.
        /// </summary>
        public MySqlConnection Connection
        {
            get => connection;
        }

        #endregion

        #region Constructors, Destructors, Interfaces

        /// <summary>
        /// Creates a new SQL-Wrapper object.
        /// </summary>
        /// <param name="connectionString">Connection-string for the database</param>
        public WrapMySQL(string connectionString)
        {
            // Set connection-string
            this.connectionString = connectionString;

            // Create connection
            connection = new MySqlConnection(this.connectionString);
        }

        /// <summary>
        /// Creates a new SQL-Wrapper object.
        /// </summary>
        /// <param name="server">Hostname or IP of the server</param>
        /// <param name="database">Target database</param>
        /// <param name="username">Login username</param>
        /// <param name="password">Login password</param>
        /// <param name="port">Server-port. Default: 3306</param>
        /// <param name="sslMode">SSL encryption mode</param>
        public WrapMySQL(string server, string database, string username, string password, int port = 3306, string sslMode = "none")
        {
            // Assemble connection-string
            this.connectionString = $"SERVER={server};Port={port};SslMode={sslMode};DATABASE={database};USER ID={username};PASSWORD={password}";

            // Create connection
            connection = new MySqlConnection(this.connectionString);
        }

        /// <summary>
        /// Creates a new SQL-Wrapper object.
        /// </summary>
        /// <param name="conDat">WrapMySQL connection data packet</param>
        public WrapMySQL(WrapMySQLConDat conDat)
        {
            // Assemble connection-string
            this.connectionString = $"SERVER={conDat.Hostname};Port=3306;SslMode=none;DATABASE={conDat.Database};USER ID={conDat.Username};PASSWORD={conDat.Password}";

            // Create connection
            connection = new MySqlConnection(this.connectionString);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public override void Dispose()
        {
            // Dispose the connection object
            if (connection != null) connection.Dispose();
        }

        #endregion

        #region Connection Open/Close

        /// <summary>
        /// Opens the SQL-connection, if the connection is closed
        /// </summary>
        public override void Open()
        {
            if (this.connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        /// <summary>
        /// Closes the SQL-Connection, if the connection is open.
        /// </summary>
        public override void Close()
        {
            if (this.connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        #endregion

        #region Transaction Begin/Commit/Rollback

        /// <summary>
        /// Starts a transaction.
        /// </summary>
        public override void TransactionBegin()
        {
            base.TransactionBegin();
            transaction = Connection.BeginTransaction();
        }

        /// <summary>
        /// Commits a transaction.
        /// </summary>
        public override void TransactionCommit()
        {
            transaction.Commit();
            base.TransactionCommit();
        }

        /// <summary>
        /// Terminates a transaction.
        /// </summary>
        public override void TransactionRollback()
        {
            transaction.Rollback();
            base.TransactionRollback();
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Executes a non-query statement. 
        /// </summary>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="aCon">Manage connection states (AutoConnect)</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>NonQuery result</returns>
        protected override int ExecuteNonQuery(string sqlQuery, bool aCon, params object[] parameters)
        {
            if (transactionActive && aCon) throw new Exception("AutoConnect-methods (ACon) are not allowed durring a transaction!");

            using (MySqlCommand command = new MySqlCommand(sqlQuery, Connection))
            {
                if (transactionActive) command.Transaction = transaction;
                int result;
                foreach (object parameter in parameters) command.Parameters.AddWithValue(string.Empty, parameter);
                if (aCon) Open();
                result = command.ExecuteNonQuery();
                if (aCon) Close();
                return result;
            }
        }

        #endregion

        #region ExecuteQuery

        /// <summary>
        /// Executes a query-statement.
        /// </summary>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>DataReader fetching the query-results</returns>
        public MySqlDataReader ExecuteQuery(string sqlQuery, params object[] parameters)
        {
            MySqlCommand command = new MySqlCommand(sqlQuery, Connection);
            foreach (object parameter in parameters) command.Parameters.AddWithValue(string.Empty, parameter);
            return command.ExecuteReader();
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes a execute-scalar statement.
        /// </summary>
        /// <typeparam name="T">Target-datatype of the result</typeparam>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="aCon">Manage connection states (AutoConnect)</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>Result of the scalar-query</returns>
        protected override T ExecuteScalar<T>(string sqlQuery, bool aCon, params object[] parameters)
        {
            if (transactionActive && aCon) throw new Exception("AutoConnect-methods (ACon) are not allowed durring a transaction!");

            using (MySqlCommand command = new MySqlCommand(sqlQuery, Connection))
            {
                if (transactionActive) command.Transaction = transaction;
                foreach (object parameter in parameters) command.Parameters.AddWithValue(string.Empty, parameter);
                if (aCon) Open();
                object retval = command.ExecuteScalar();
                if (aCon) Close();
                return (T)Convert.ChangeType(retval, typeof(T));
            }
        }

        #endregion

        #region DataAdapter

        /// <summary>
        /// Fills a DataTable with the results of a query-statement.
        /// </summary>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>Results of a query-statement</returns>
        public override DataTable FillDataTable(string sqlQuery, params object[] parameters)
        {
            using (MySqlCommand command = new MySqlCommand(sqlQuery, Connection))
            {
                foreach (object parameter in parameters) command.Parameters.AddWithValue(string.Empty, parameter);
                using (MySqlDataAdapter da = new MySqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Creates a DataAdapter on the given query-statement.
        /// </summary>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>DataAdapter of the given query-statement</returns>
        public MySqlDataAdapter GetDataAdapter(string sqlQuery, params object[] parameters)
        {
            using (MySqlCommand command = new MySqlCommand(sqlQuery, Connection))
            {
                foreach (object parameter in parameters) command.Parameters.AddWithValue(string.Empty, parameter);
                return new MySqlDataAdapter(command);
            }
        }

        #endregion
    }
}

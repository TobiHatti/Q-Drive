using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

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
    public class WrapSQLite : WrapSQL
    {
        #region Fields and Properties

        private readonly string connectionString = string.Empty;
        private readonly SQLiteConnection connection = null;
        private SQLiteTransaction transaction = null;

        /// <summary>
        /// SQL-Connection object.
        /// </summary>
        public SQLiteConnection Connection
        {
            get => connection;
        }

        #endregion

        #region Constructors, Destructors, Interfaces

        /// <summary>
        /// Creates a new SQL-Wrapper object.
        /// </summary>
        /// <param name="connectionString">Connection-string for the database</param>
        public WrapSQLite(string connectionString)
        {
            // Set connection-string
            this.connectionString = connectionString;

            // Create connection
            connection = new SQLiteConnection(this.connectionString);
        }


        /// <summary>
        /// Creates a new SQL-Wrapper object.
        /// </summary>
        /// <param name="databaseFilePath">Path to the SQLite database file</param>
        /// <param name="isFilePath">If false, the path gets interpreted as the connection-string</param>
        public WrapSQLite(string databaseFilePath, bool isFilePath = true)
        {
            // Set connection-string
            if (isFilePath)
            {
                this.connectionString = $@"URI=file:{databaseFilePath}";
                if (!Directory.Exists(Path.GetDirectoryName(databaseFilePath))) Directory.CreateDirectory(Path.GetDirectoryName(databaseFilePath));
            }
            else this.connectionString = databaseFilePath;



            // Create connection
            connection = new SQLiteConnection(this.connectionString);
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

            using (SQLiteCommand command = new SQLiteCommand(sqlQuery, Connection))
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
        public SQLiteDataReader ExecuteQuery(string sqlQuery, params object[] parameters)
        {
            SQLiteCommand command = new SQLiteCommand(sqlQuery, Connection);
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

            using (SQLiteCommand command = new SQLiteCommand(sqlQuery, Connection))
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
            using (SQLiteCommand command = new SQLiteCommand(sqlQuery, Connection))
            {
                foreach (object parameter in parameters) command.Parameters.AddWithValue(string.Empty, parameter);
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(command))
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
        public SQLiteDataAdapter GetDataAdapter(string sqlQuery, params object[] parameters)
        {
            using (SQLiteCommand command = new SQLiteCommand(sqlQuery, Connection))
            {
                foreach (object parameter in parameters) command.Parameters.AddWithValue(string.Empty, parameter);
                return new SQLiteDataAdapter(command);
            }
        }

        #endregion
    }
}

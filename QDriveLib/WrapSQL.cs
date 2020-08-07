using System;
using System.Data;

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
    public abstract class WrapSQL : IDisposable
    {
        protected bool transactionActive = false;

        public abstract void Dispose();

        #region Connection Open/Close

        public abstract void Open();
        public abstract void Close();

        #endregion

        #region Transaction Begin/Commit/Rollback

        public virtual void TransactionBegin() => transactionActive = true;

        public virtual void TransactionCommit() => transactionActive = false;

        public virtual void TransactionRollback() => transactionActive = false;

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Executes a non-query statement. 
        /// </summary>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="aCon">Manage connection states (AutoConnect)</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>NonQuery result</returns>
        protected abstract int ExecuteNonQuery(string sqlQuery, bool aCon, params object[] parameters);

        /// <summary>
        /// Executes a non-query statement. 
        /// </summary>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>NonQuery result</returns>
        public int ExecuteNonQuery(string sqlQuery, params object[] parameters)
            => ExecuteNonQuery(sqlQuery, false, parameters);

        /// <summary>
        /// Executes a non-query statement. 
        /// Automatically opens and closes the connection.
        /// </summary>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>NonQuery result</returns>
        public int ExecuteNonQueryACon(string sqlQuery, params object[] parameters)
            => ExecuteNonQuery(sqlQuery, true, parameters);

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
        protected abstract T ExecuteScalar<T>(string sqlQuery, bool aCon, params object[] parameters);

        /// <summary>
        /// Executes a execute-scalar statement. 
        /// </summary>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>Result of the scalar-query</returns>
        public object ExecuteScalar(string sqlQuery, params object[] parameters)
            => ExecuteScalar<object>(sqlQuery, false, parameters);

        /// <summary>
        /// Executes a execute-scalar statement. 
        /// Automatically opens and closes the connection.
        /// </summary>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>Result of the scalar-query</returns>
        public object ExecuteScalarACon(string sqlQuery, params object[] parameters)
            => ExecuteScalar<object>(sqlQuery, true, parameters);

        /// <summary>
        /// Executes a execute-scalar statement. 
        /// </summary>
        /// <typeparam name="T">Target-datatype of the result</typeparam>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>Result of the scalar-query</returns>
        public T ExecuteScalar<T>(string sqlQuery, params object[] parameters)
            => ExecuteScalar<T>(sqlQuery, false, parameters);

        /// <summary>
        /// Executes a execute-scalar statement.
        /// Automatically opens and closes the connection.
        /// </summary>
        /// <typeparam name="T">Target-datatype of the result</typeparam>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>Result of the scalar-query</returns>
        public T ExecuteScalarACon<T>(string sqlQuery, params object[] parameters)
            => ExecuteScalar<T>(sqlQuery, true, parameters);

        #endregion

        #region DataAdapter

        /// <summary>
        /// Fills a DataTable with the results of a query-statement.
        /// </summary>
        /// <param name="sqlQuery">SQL-query</param>
        /// <param name="parameters">Query-parameters</param>
        /// <returns>Results of a query-statement</returns>
        public abstract DataTable FillDataTable(string sqlQuery, params object[] parameters);

        #endregion
    }
}

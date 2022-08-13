using System;
using System.Data;
using ICS.Core.DataAccess.OleDb;
using ICS.Core.DataAccess.SQL;

namespace ICS.Core.DataAccess
{
    #region DataAccess: Transaction Helper
    public abstract class TransactionHelper
    {
        #region Default Helper
        private static TransactionHelper _default;
        public static TransactionHelper Default
        {
            get
            {
                if (_default == null)
                {
                    //_default = new OleDbHelper();
                    //aaa111
                    _default = new SqlHelper();
                }

                return _default;
            }
        }

        #endregion

        #region ExecuteNonQuery
        public abstract int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText);
        public abstract int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        public abstract int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues);
        public abstract int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText);
        public abstract int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        public abstract int ExecuteNonQuery(IDbConnection connection, string spName, params object[] parameterValues);
        public abstract int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText);
        public abstract int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        public abstract int ExecuteNonQuery(IDbTransaction transaction, string spName, params object[] parameterValues);
        #endregion ExecuteNonQuery

        #region ExecuteDataset
        public abstract DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText);
        public abstract DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        public abstract DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues);
        public abstract DataSet ExecuteDataset(IDbConnection connection, CommandType commandType, string commandText);
        public abstract DataSet ExecuteDataset(IDbConnection connection, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        public abstract DataSet ExecuteDataset(IDbConnection connection, string spName, params object[] parameterValues);
        public abstract DataSet ExecuteDataset(IDbTransaction transaction, CommandType commandType, string commandText);
        public abstract DataSet ExecuteDataset(IDbTransaction transaction, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        public abstract DataSet ExecuteDataset(IDbTransaction transaction, string spName, params object[] parameterValues);
        #endregion ExecuteDataset

        #region ExecuteReader
        private enum IDbConnectionOwnership
        {
            /// <summary>Connection is owned and managed by OleDbHelper</summary>
            Internal,
            /// <summary>Connection is owned and managed by the caller</summary>
            External
        }
        public abstract IDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText);
        public abstract IDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        public abstract IDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues);
        public abstract IDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText);
        public abstract IDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        public abstract IDataReader ExecuteReader(IDbConnection connection, string spName, params object[] parameterValues);
        public abstract IDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText);
        public abstract IDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        public abstract IDataReader ExecuteReader(IDbTransaction transaction, string spName, params object[] parameterValues);
        #endregion ExecuteReader

        #region ExecuteScalar
        public abstract object ExecuteScalar(string connectionString, CommandType commandType, string commandText);
        public abstract object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        public abstract object ExecuteScalar(string connectionString, string spName, params object[] parameterValues);
        public abstract object ExecuteScalar(IDbConnection connection, CommandType commandType, string commandText);
        public abstract object ExecuteScalar(IDbConnection connection, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        public abstract object ExecuteScalar(IDbConnection connection, string spName, params object[] parameterValues);
        public abstract object ExecuteScalar(IDbTransaction transaction, CommandType commandType, string commandText);
        public abstract object ExecuteScalar(IDbTransaction transaction, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        public abstract object ExecuteScalar(IDbTransaction transaction, string spName, params object[] parameterValues);
        #endregion ExecuteScalar

        #region FillDataset
        public abstract void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames);
        public abstract void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params IDataParameter[] commandParameters);
        public abstract void FillDataset(string connectionString, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues);
        public abstract void FillDataset(IDbConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames);
        public abstract void FillDataset(IDbConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params IDataParameter[] commandParameters);
        public abstract void FillDataset(IDbConnection connection, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues);
        public abstract void FillDataset(IDbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames);
        public abstract void FillDataset(IDbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params IDataParameter[] commandParameters);
        public abstract void FillDataset(IDbTransaction transaction, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues);
        #endregion

        #region UpdateDataset
        public abstract void UpdateDataset(IDbCommand insertCommand, IDbCommand deleteCommand, IDbCommand updateCommand, DataSet dataSet, string tableName);
        #endregion

        #region CreateCommand
        public abstract IDbCommand CreateCommand(IDbConnection connection, string spName, params string[] sourceColumns);
        #endregion

        #region ExecuteNonQueryTypedParams
        public abstract int ExecuteNonQueryTypedParams(String connectionString, String spName, DataRow dataRow);
        public abstract int ExecuteNonQueryTypedParams(IDbConnection connection, String spName, DataRow dataRow);
        public abstract int ExecuteNonQueryTypedParams(IDbTransaction transaction, String spName, DataRow dataRow);
        #endregion

        #region ExecuteDatasetTypedParams
        public abstract DataSet ExecuteDatasetTypedParams(string connectionString, String spName, DataRow dataRow);
        public abstract DataSet ExecuteDatasetTypedParams(IDbConnection connection, String spName, DataRow dataRow);
        public abstract DataSet ExecuteDatasetTypedParams(IDbTransaction transaction, String spName, DataRow dataRow);
        #endregion

        #region ExecuteReaderTypedParams
        public abstract IDataReader ExecuteReaderTypedParams(String connectionString, String spName, DataRow dataRow);
        public abstract IDataReader ExecuteReaderTypedParams(IDbConnection connection, String spName, DataRow dataRow);
        public abstract IDataReader ExecuteReaderTypedParams(IDbTransaction transaction, String spName, DataRow dataRow);
        #endregion

        #region ExecuteScalarTypedParams
        public abstract object ExecuteScalarTypedParams(string connectionString, string spName, DataRow dataRow);
        public abstract object ExecuteScalarTypedParams(IDbConnection connection, string spName, DataRow dataRow);
        public abstract object ExecuteScalarTypedParams(IDbTransaction transaction, string spName, DataRow dataRow);
        #endregion
    }
    #endregion
}

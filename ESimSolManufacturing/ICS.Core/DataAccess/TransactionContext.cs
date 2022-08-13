using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace ICS.Core.DataAccess
{
    #region DataAccess: Transaction Context
    public class TransactionContext
    {
        #region Declaration & Constructors
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private static bool _createSession = false;
        private TransactionContext() { }
        #endregion

        #region Transaction related functions
        public static TransactionContext Begin()
        {
            return new TransactionContext();
        }
        public static TransactionContext Begin(bool createSession)
        {
            _createSession = true;
            return new TransactionContext();
        }

        public void End()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
            _createSession = false;
            RelICSResources();
        }
        private void RelICSResources()
        {
            try
            {
                if (_connection != null && _connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            catch { }
        }
        #endregion

        #region Command preparation and private functions
        private void PrepareConnection()
        {
            if (_connection == null)
            {
                _connection = ConnectionFactory.Default.CreateConnection();
                _connection.Open();

                if (_createSession)
                {
                    _transaction = _connection.BeginTransaction();
                }
            }
        }

        public void PrepareCommand(IDbCommand command)
        {
            PrepareConnection();
            command.Connection = _connection;
            command.Transaction = _transaction;
        }
        #endregion

        #region Execute NonQuery
        public void ExecuteNonQuery(string sql, params object[] args)
        {
            PrepareConnection();
            
            string sSQL = SQLParser.MakeSQL(sql, args);
            if (_transaction != null)
            {
                TransactionHelper.Default.ExecuteNonQuery(_transaction, CommandType.Text, sSQL);
            }
            else
            {
                TransactionHelper.Default.ExecuteNonQuery(_connection, CommandType.Text, sSQL);
            }
        }

        public void ExecuteNonQuery(CommandType commandType, string commandText, IDataParameter[] commandParametes)
        {
            PrepareConnection();
            if (_transaction != null)
            {
                TransactionHelper.Default.ExecuteNonQuery(_transaction, commandType, commandText, commandParametes);
            }
            else
            {
                TransactionHelper.Default.ExecuteNonQuery(_connection, commandType, commandText, commandParametes);
            }
        }

        public void ExecuteNonQuery(CommandType commandType, string spName, params object[] parameterValues)
        {
            PrepareConnection();
            if (_transaction != null)
            {
                TransactionHelper.Default.ExecuteNonQuery(_transaction, spName, parameterValues);
            }
            else
            {
                TransactionHelper.Default.ExecuteNonQuery(_connection, spName, parameterValues);
            }
        }
        public void ExecuteNonQueryCommText(CommandType commandType, string comText, params  SqlParameter[] parameterValues)
        {
            PrepareConnection();
            if (_transaction != null)
            {
                TransactionHelper.Default.ExecuteNonQuery(_transaction, commandType, comText, parameterValues);
            }
            else
            {
                TransactionHelper.Default.ExecuteNonQuery(_connection, commandType, comText, parameterValues);
            }
        }
        #endregion

        #region Generate ID
        public int GenerateID(string sTableName, string sFieldName, string sSQLClause)
        {
            object nMaxID = ExecuteScalar("SELECT MAX(%q) FROM %q %q", sFieldName, sTableName, sSQLClause);
            if (nMaxID == DBNull.Value)
            {
                nMaxID = 1;
            }
            else
            {
                nMaxID = Convert.ToInt32(nMaxID) + 1;
                if ((int)nMaxID <= 0)
                {
                    nMaxID = 1;
                }
            }
            return (int)nMaxID;
        }
        public int GenerateID(string sTableName, string sFieldName)
        {
            return GenerateID(sTableName, sFieldName, "");
        }
        #endregion

        #region Generate New Code
        public string GenerateCode(string sTableName, string sFormat, string sSQLClause)
        {
            object sCurCode = ExecuteScalar("SELECT Count(*) FROM %q %q", sTableName, sSQLClause);
            if ((int)sCurCode <= 0)
            {
                sCurCode = sFormat.Insert(sFormat.Length - 1, "1");
            }
            else
            {
                sCurCode = (int)sCurCode + 1;
                sCurCode = sFormat.Insert(sFormat.Length - sCurCode.ToString().Length, sCurCode.ToString());                
            }
            sCurCode = sCurCode.ToString().Substring(0, sFormat.Length);
            return sCurCode.ToString();
        }
        public string GenerateCode(string sTableName, string sFormat)
        {
            return GenerateCode(sTableName, sFormat, "");
        }

        public bool IsExist(string sTableName, string sFieldName, string sCode)
        {
            object bIsExist = ExecuteScalar("SELECT %q FROM  %q WHERE %q=%s", sFieldName, sTableName, sFieldName, sCode);
            if (bIsExist == null)
            {
                return false;
            }
            return true;
  
        }
        public bool IsExist(string sTableName, string sFieldName, string sFieldName1, string sCode, string sCode1)
        {
            object bIsExist = ExecuteScalar("SELECT %q,%q FROM  %q WHERE %q=%s AND %q=%s", sFieldName, sFieldName1, sTableName, sFieldName, sCode, sFieldName1, sCode1);
            if (bIsExist == null)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Execute Reader
        public IDataReader ExecuteReader(string sql, params object[] args)
        {
            string sSQL = SQLParser.MakeSQL(sql, args);
            PrepareConnection();
            if (_transaction != null)
            {
                return TransactionHelper.Default.ExecuteReader(_transaction, CommandType.Text, sSQL);
            }
            else
            {
                return TransactionHelper.Default.ExecuteReader(_connection, CommandType.Text, sSQL);
            }
        }
        public IDataReader ExecuteReader(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            PrepareConnection();
            if (_transaction != null)
            {
                return TransactionHelper.Default.ExecuteReader(_transaction, commandType, commandText, commandParameters);
            }
            else
            {
                return TransactionHelper.Default.ExecuteReader(_connection, commandType, commandText, commandParameters);
            }
        }
        public IDataReader ExecuteReader(CommandType commandType, string spName, params object[] parameterValues)
        {
            PrepareConnection();
            if (_transaction != null)
            {
                return TransactionHelper.Default.ExecuteReader(_transaction, spName, parameterValues);
            }
            else
            {
                return TransactionHelper.Default.ExecuteReader(_connection, spName, parameterValues);
            }
        }
        #endregion

        #region Execute Scalar
        public object ExecuteScalar(string sql, params object[] args)
        {
            string sSQL = SQLParser.MakeSQL(sql, args);
            PrepareConnection();
            if (_transaction != null)
            {
                return TransactionHelper.Default.ExecuteScalar(_transaction, CommandType.Text, sSQL);
            }
            else
            {
                return TransactionHelper.Default.ExecuteScalar(_connection, CommandType.Text, sSQL);
            }
        }

        public object ExecuteScalar(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            PrepareConnection();
            if (_transaction != null)
            {
                return TransactionHelper.Default.ExecuteScalar(_transaction, commandType, commandText, commandParameters);
            }
            else
            {
                return TransactionHelper.Default.ExecuteScalar(_connection, commandType, commandText, commandParameters);
            }
        }
        public object ExecuteScalar(CommandType commandType, string spName, params object[] parameterValues)
        {
            PrepareConnection();
            if (_transaction != null)
            {
                return TransactionHelper.Default.ExecuteScalar(_transaction, spName, parameterValues);
            }
            else
            {
                return TransactionHelper.Default.ExecuteScalar(_connection, spName, parameterValues);
            }
        }
        #endregion

        #region Execute Dataset
        public DataSet ExecuteDataSet(string sql, params object[] args)
        {
            string sSQL = SQLParser.MakeSQL(sql, args);
            PrepareConnection();
            if (_transaction != null)
            {
                return TransactionHelper.Default.ExecuteDataset(_transaction, CommandType.Text, sSQL);
            }
            else
            {
                return TransactionHelper.Default.ExecuteDataset(_connection, CommandType.Text, sSQL);
            }
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            PrepareConnection();
            if (_transaction != null)
            {
                return TransactionHelper.Default.ExecuteDataset(_transaction, commandType, commandText, commandParameters);
            }
            else
            {
                return TransactionHelper.Default.ExecuteDataset(_connection, commandType, commandText, commandParameters);
            }
        }
        public DataSet ExecuteDataSet(CommandType commandType, string spName, params object[] parameterValues)
        {
            PrepareConnection();
            if (_transaction != null)
            {
                return TransactionHelper.Default.ExecuteDataset(_transaction, spName, parameterValues);
            }
            else
            {
                return TransactionHelper.Default.ExecuteDataset(_connection, spName, parameterValues);
            }
        }
        #endregion

        #region Fill Dataset
        public void FillDataset(DataSet dataSet, string[] tableNames, string sql, params object[] args)
        {
            string sSQL = SQLParser.MakeSQL(sql, args);
            PrepareConnection();
            if (_transaction != null)
            {
                TransactionHelper.Default.FillDataset(_transaction, CommandType.Text, sSQL, dataSet, tableNames);
            }
            else
            {
                TransactionHelper.Default.FillDataset(_connection, CommandType.Text, sSQL, dataSet, tableNames);
            }
        }

        public void FillDataset(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params IDataParameter[] commandParameters)
        {
            PrepareConnection();
            if (_transaction != null)
            {
                TransactionHelper.Default.FillDataset(_transaction, commandType, commandText, dataSet, tableNames, commandParameters);
            }
            else
            {
                TransactionHelper.Default.FillDataset(_connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }
        public void FillDataset(CommandType commandType, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            PrepareConnection();
            if (_transaction != null)
            {
                TransactionHelper.Default.FillDataset(_transaction, spName, dataSet, tableNames, parameterValues);
            }
            else
            {
                TransactionHelper.Default.FillDataset(_connection, spName, dataSet, tableNames, parameterValues);
            }
        }
        #endregion

        #region Error Handler
        /// <summary>
        /// prevents throwing error while tryin to rollback a transaction
        /// </summary>
        public void HandleError()
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Rollback();
                }
                catch { } 
            }

            RelICSResources();
        }

        #endregion
    }
    #endregion
}

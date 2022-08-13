using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DBConnectionDA
    {
        public DBConnectionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DBConnection oDBConnection, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_COA_IUD_DBConnection]"
                                    + "%n, %s, %s, %s, %s, %s, %s, %n,	%n",
                                    oDBConnection.DBConnectionID, oDBConnection.ProjectName, oDBConnection.Description, oDBConnection.ServerName, oDBConnection.UserID, oDBConnection.Password, oDBConnection.DBName, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, DBConnection oDBConnection, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_COA_IUD_DBConnection]"
                                    + "%n, %s, %s, %s, %s, %s, %s, %n,	%n",
                                    oDBConnection.DBConnectionID, oDBConnection.ProjectName, oDBConnection.Description, oDBConnection.ServerName, oDBConnection.UserID, oDBConnection.Password, oDBConnection.DBName, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DBConnection WHERE DBConnectionID=%n", nID);
        }
       
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DBConnection");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

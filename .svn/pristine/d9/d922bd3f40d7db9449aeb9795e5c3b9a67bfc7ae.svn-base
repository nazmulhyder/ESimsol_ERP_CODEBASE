using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class DBTableReferenceDA
    {
        public DBTableReferenceDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DBTableReference oDBTableReference, EnumDBOperation eEnumDBOperation, int nCurrentUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DBTableReference] %n, %s, %s, %s, %n , %n",
                                    oDBTableReference.DBTableReferenceID, 
                                    oDBTableReference.MainTable, 
                                    oDBTableReference.ReferenceTable, 
                                    oDBTableReference.ReferenceColumn,  
                                    (int)eEnumDBOperation, 
                                    nCurrentUserID);
        }

        public static void Delete(TransactionContext tc, DBTableReference oDBTableReference, EnumDBOperation eEnumDBOperation, int nCurrentUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DBTableReference] %n, %s, %s, %s, %n , %n",
                                    oDBTableReference.DBTableReferenceID, 
                                    oDBTableReference.MainTable, 
                                    oDBTableReference.ReferenceTable, 
                                    oDBTableReference.ReferenceColumn, 
                                    (int)eEnumDBOperation, 
                                    nCurrentUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DBTableReference WHERE DBTableReferenceID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DBTableReference ORDER BY MainTable");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void HasReference(TransactionContext tc, string sObjectName, int nObjectID )
        {
            tc.ExecuteNonQuery("EXEC [SP_HasRefereance]"+ "%s,%n",sObjectName, nObjectID);
        }
        public static IDataReader GetsDBObject(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT name FROM sys.tables Order By name");
        }
        public static IDataReader GetsDBObjectColumn(TransactionContext tc, string sObjectName)
        {
            return tc.ExecuteReader("SELECT COLUMN_NAME ColumnName FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = %s", sObjectName);
        }
        #endregion
    }  
    
    
}

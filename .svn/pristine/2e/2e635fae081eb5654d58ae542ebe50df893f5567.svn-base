using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

//Please don not delete. it is our 2nd phase development
namespace ESimSol.Services.DataAccess
{

    public class TableColumnDA
    {
        public TableColumnDA() { }

        #region Get & Exist Function

        public static IDataReader GetsTable(TransactionContext tc, string sDbName)
        {
            return tc.ExecuteReader("USE " + sDbName + " SELECT Name  FROM sys.tables order by name");
        }
        public static IDataReader GetsColumn(TransactionContext tc, string sDbName, string sTableName)
        {
            return tc.ExecuteReader("USE " + sDbName + " SELECT COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME =%s Order By COLUMN_NAME", sTableName);
        }
        public static IDataReader GetsViews(TransactionContext tc, string sDBName)
        {
            return tc.ExecuteReader("SELECT TABLE_NAME As Name FROM INFORMATION_SCHEMA.VIEWS Order By TABLE_NAME ASC");
        }

        #endregion
    }  
   
}

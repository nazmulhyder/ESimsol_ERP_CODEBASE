using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportPIHistoryDA
    {
        public ExportPIHistoryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportPIHistory oExportPIHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPIHistory]"
                                    + "%n, %s, %s, %s, %s, %n, %n",
                                    oExportPIHistory.ExportPIHistoryID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ExportPIHistory oExportPIHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPIHistory]"
                                    + "%n, %s, %s, %s, %s, %n, %n",
                                    oExportPIHistory.ExportPIHistoryID, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPIHistory WHERE ExportPIHistoryID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPIHistory");
        }
        public static IDataReader GetsByExportId(TransactionContext tc, int nExportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPIHistory WHERE ExportPIID=%n", nExportPIID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

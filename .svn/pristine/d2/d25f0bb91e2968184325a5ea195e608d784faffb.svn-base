using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;


namespace ESimSol.Services.DataAccess
{



    public class ReportLayoutDA
    {
        public ReportLayoutDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ReportLayout oReportLayout, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [IUD_ReportLayout]"
                                    + "%n, %s, %s, %n, %n, %n,%n",
                                    oReportLayout.ReportLayoutID, oReportLayout.ReportNo, oReportLayout.ReportName, (int)oReportLayout.OperationType, (int)oReportLayout.ReportType, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ReportLayout oReportLayout, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [IUD_ReportLayout]"
                                    + "%n, %s, %s, %n, %n, %n,%n",
                                    oReportLayout.ReportLayoutID, oReportLayout.ReportNo, oReportLayout.ReportName, (int)oReportLayout.OperationType, (int)oReportLayout.ReportType, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ReportLayout WHERE ReportLayoutID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ReportLayout");
        }

        public static IDataReader Gets(TransactionContext tc, int eOperationType)
        {
            return tc.ExecuteReader("SELECT * FROM ReportLayout Where OperationType = "+eOperationType);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
       #endregion
    }  

    
}

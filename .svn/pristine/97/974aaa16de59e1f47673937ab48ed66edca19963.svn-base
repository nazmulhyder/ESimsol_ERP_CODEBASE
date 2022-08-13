using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportSCDetailDODA
    {
        public ExportSCDetailDODA() { }

        #region Insert Update Delete Function
     
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCDetail_DO WHERE ExportSCDetailDOID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc,int nExportLCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCDetail_DO WHERE ExportPIID in (Select ExportPIID from [ExportPI] where LCID =%n ) ", nExportLCID);
        }

        public static IDataReader GetsByESCID(TransactionContext tc, int nExportSCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCDetail_DO WHERE ExportSCID=%n", nExportSCID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCDetail_DO");
        }

        public static IDataReader GetsByPI(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCDetail_DO WHERE ExportSCID in (Select ExportSC.ExportSCID from ExportSC where ExportPIID=%n) order by ProductID", nPIID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsLog(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportSCDetailDOsLog] WHERE ExportSCID=%n", nPIID);
        }
        public static IDataReader GetsByLC(TransactionContext tc, int nExportLCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCDetailDOs_LC WHERE PIID in (Select ExportPIID from [ExportPI] where LCID =%n ) ", nExportLCID);
        }
        #endregion
    }
}

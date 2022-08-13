using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportPIRWUDA
    {

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("Select * From View_ExportPIReport_WU");
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            //return tc.ExecuteReader("Select ExportPIDetailID From View_ExportPIReport_WU WHERE ExportPIDetailID = %n", nID);
            return tc.ExecuteReader("Select * From View_ExportPIReport_WU WHERE ExportPIDetailID = %n", nID);

        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetByPINo(TransactionContext tc, ExportPIRWU oExportPIRWU)
        {
            return tc.ExecuteReader("Select * From View_ExportPIReport_WU WHERE PINo = '"+oExportPIRWU.PINo+"'");
        }
        public static IDataReader GetByPONo(TransactionContext tc, ExportPIRWU oExportPIRWU)
        {
            return tc.ExecuteReader("Select * From View_ExportPIReport_WU WHERE PONo = '" + oExportPIRWU.PONo + "'");
        }
        public static IDataReader SetPO(TransactionContext tc, ExportPIRWU oExportPIRWU, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Update ExportPIDetail set OrderSheetDetailID=%n where isnull(OrderSheetDetailID,0)<=0 and ExportPIDetailID=%n", oExportPIRWU.FabricSalesContractDetailID, oExportPIRWU.ExportPIDetailID);
            return tc.ExecuteReader("SELECT * FROM View_ExportPIReport_WU where   ExportPIDetailID=%n", oExportPIRWU.ExportPIDetailID);
        }
        public static IDataReader RemoveDispoNo(TransactionContext tc, ExportPIRWU oExportPIRWU, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Update ExportPIDetail set OrderSheetDetailID=0 where ExportPIDetailID=%n", oExportPIRWU.ExportPIDetailID);
            return tc.ExecuteReader("SELECT * FROM View_ExportPIReport_WU where   ExportPIDetailID=%n", oExportPIRWU.ExportPIDetailID);
        }
        
        #endregion
    }  
}


using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportFundAllocationDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ExportFundAllocation oExportFundAllocation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportFundAllocation]"
                                   + "%n,%n,%n,%n,%n,%s,%n,%n",
                                   oExportFundAllocation.ExportFundAllocationID, oExportFundAllocation.ExportLCID, oExportFundAllocation.ExportFundAllocationHeadID, oExportFundAllocation.Amount,oExportFundAllocation.AmountInPercent,oExportFundAllocation.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ExportFundAllocation oExportFundAllocation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportFundAllocation]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n",
                                   oExportFundAllocation.ExportFundAllocationID, oExportFundAllocation.ExportLCID, oExportFundAllocation.ExportFundAllocationHeadID, oExportFundAllocation.Amount, oExportFundAllocation.AmountInPercent, oExportFundAllocation.Remarks, nUserID, (int)eEnumDBOperation);
        }


        #endregion
        public static void Approved(TransactionContext tc, ExportFundAllocation oExportFundAllocation, long nID)
        {
            tc.ExecuteNonQuery("Update ExportFundAllocation SET ApprovedBy = %n WHERE ExportFundAllocationID = %n", nID, oExportFundAllocation.ExportFundAllocationID);
        }
        public static void UndoApproved(TransactionContext tc, ExportFundAllocation oExportFundAllocation, long nID)
        {
            tc.ExecuteNonQuery("Update ExportFundAllocation SET ApprovedBy = 0 WHERE ExportFundAllocationID = %n", oExportFundAllocation.ExportFundAllocationID);
        }

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportFundAllocation WHERE ExportFundAllocationID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, ExportFundAllocation oExportFundAllocation)
        {
            return tc.ExecuteReader("EXEC [SP_Export_FundAllocation]" + " %s", oExportFundAllocation.SQL);
          
        }
        #endregion
    }
}

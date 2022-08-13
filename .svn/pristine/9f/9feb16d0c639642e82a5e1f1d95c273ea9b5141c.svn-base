using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportFundAllocationHeadDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ExportFundAllocationHead oExportFundAllocationHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportFundAllocationHead]"
                                   + "%n,%s,%s,%n,%s,%n,%n",
                                   oExportFundAllocationHead.ExportFundAllocationHeadID, oExportFundAllocationHead.Code, oExportFundAllocationHead.Name,oExportFundAllocationHead.Sequence, oExportFundAllocationHead.Remarks,nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ExportFundAllocationHead oExportFundAllocationHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportFundAllocationHead]"
                                    + "%n,%s,%s,%n,%s,%n,%n",
                                   oExportFundAllocationHead.ExportFundAllocationHeadID, oExportFundAllocationHead.Code, oExportFundAllocationHead.Name, oExportFundAllocationHead.Sequence,oExportFundAllocationHead.Remarks, nUserID, (int)eEnumDBOperation);
        }
        public static void UpdateSequence(TransactionContext tc, ExportFundAllocationHead oExportFundAllocationHead)
        {
            tc.ExecuteNonQuery("Update ExportFundAllocationHead SET Sequence = %n WHERE ExportFundAllocationHeadID = %n", oExportFundAllocationHead.Sequence, oExportFundAllocationHead.ExportFundAllocationHeadID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportFundAllocationHead WHERE ExportFundAllocationHeadID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}

using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;
namespace ESimSol.Services.DataAccess
{
   public class LoanExportLCDA
    {
        #region Insert Update Delete Function

       public static IDataReader InsertUpdate(TransactionContext tc, LoanExportLC oLoanExportLC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {

            return tc.ExecuteReader("EXEC [SP_IUD_LoanExportLC]"
                                   + "%n,%n,%n,%n,%s,%n,%n",
                                   oLoanExportLC.LoanExportLCID,
                                   oLoanExportLC.LoanID,
                                   oLoanExportLC.ExportLCID,
                                   oLoanExportLC.Amount,
                                   oLoanExportLC.Remarks,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }



        public static void Delete(TransactionContext tc, LoanExportLC oLoanExportLC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_LoanExportLC]"
                                   + "%n,%n,%n,%n,%s,%n,%n",
                                   oLoanExportLC.LoanExportLCID,
                                   oLoanExportLC.LoanID,
                                   oLoanExportLC.ExportLCID,
                                   oLoanExportLC.Amount,
                                   oLoanExportLC.Remarks,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }
        public static void DeleteByIDs(TransactionContext tc,int nLoanID, string sExportLCIDs, Int64 nUserID)
        {
            tc.ExecuteNonQuery("DELETE FROM LoanExportLC WHERE LoanID="+nLoanID+  "AND ExportLCID NOT IN ("+sExportLCIDs+")");
        }




        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LoanExportLC WHERE LoanID=%n", nID);
        }
        public static IDataReader Gets(int buid, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_LoanExportLC WHERE BUID=%n", buid);
        }
     
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ImportLCDetailDA
    {
        public ImportLCDetailDA() { }

        #region Insert, Update, Delete
        public static IDataReader InsertUpdate(TransactionContext tc, ImportLCDetail oImportLCDetail, EnumDBOperation eEnumDBImportLC, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLCDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%s",
                                   oImportLCDetail.ImportLCDetailID, 
                                   oImportLCDetail.ImportLCID,
                                   oImportLCDetail.ImportPIID, 
                                   oImportLCDetail.Amount, 
                                   nUserId, 
                                   (int)eEnumDBImportLC, "");
        }
        public static void Delete(TransactionContext tc, ImportLCDetail oImportLCDetail, EnumDBOperation eEnumDBImportLC, Int64 nUserId, string sImportLCDetailIDS)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportLCDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%s",
                                   oImportLCDetail.ImportLCDetailID,
                                   oImportLCDetail.ImportLCID,
                                   oImportLCDetail.ImportPIID,
                                   oImportLCDetail.Amount,
                                   nUserId,
                                   (int)eEnumDBImportLC, sImportLCDetailIDS);
        }

        #endregion

        #region Insert, Update, Delete Log
        public static IDataReader InsertUpdateLog(TransactionContext tc, ImportLCDetail oImportLCDetail, EnumDBOperation eEnumDBImportLC, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLCDetailLog]"
                                    + "%n,%n,%n,%n,%n,%d,%n,%n,%s",
                                   oImportLCDetail.ImportLCDetailLogID,
                                   oImportLCDetail.ImportLCLogID,
                                   oImportLCDetail.ImportLCID,
                                   oImportLCDetail.ImportPIID,
                                   oImportLCDetail.Amount,
                                   oImportLCDetail.AmendmentDate,
                                   nUserId,
                                   (int)eEnumDBImportLC, "");
        }

        public static void DeleteLog(TransactionContext tc, ImportLCDetail oImportLCDetail, EnumDBOperation eEnumDBImportLC, Int64 nUserId, string sImportLCDetailIDS)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportLCDetailLog]"
                                    + "%n,%n,%n,%n,%n,%d,%n,%n,%s",
                                   oImportLCDetail.ImportLCDetailLogID,
                                   oImportLCDetail.ImportLCLogID,
                                   oImportLCDetail.ImportLCID,
                                   oImportLCDetail.ImportPIID,
                                   oImportLCDetail.Amount,
                                   oImportLCDetail.AmendmentDate,
                                   nUserId,
                                   (int)eEnumDBImportLC, sImportLCDetailIDS);
        }
        #endregion


        #region Get & Exist Function

        public static IDataReader GetByPurhcaseLCID(TransactionContext tc, int PurhcaseLCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportLCDetail WHERE ImportLCID = %n", PurhcaseLCID);
        }
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportLCDetail WHERE ImportLCDetailID=%n", nID);
        }
         public static IDataReader Gets(TransactionContext tc ,int nImportLCID)
        {
            return tc.ExecuteReader("Select * from View_ImportLCDetail where Activity=1 and ImportLCID=%n", nImportLCID);
        }
         public static IDataReader GetsLog(TransactionContext tc, int nImportLCLogID)
         {
             return tc.ExecuteReader("Select * from View_ImportLCDetailLog where Activity=1 and ImportLCLogID=%n", nImportLCLogID);
         }
         public static IDataReader GetsByInvoice(TransactionContext tc, int nInvoiceID)
         {
             return tc.ExecuteReader("SELECT * FROM View_ImportLCDetail WHERE ImportLCId = %n", nInvoiceID);
         }
          
        #endregion
    }
   
}

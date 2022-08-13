using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class ImportClaimDetailDA
    {
        public ImportClaimDetailDA() { }
        
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImportClaimDetail oImportClaimDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ImportClaimDetail]"
                                    + "%n,%n,%n,  %n, %n, %s,%n, %n,%s",
                                    oImportClaimDetail.ImportClaimDetailID, oImportClaimDetail.ImportClaimID, oImportClaimDetail.ProductID, oImportClaimDetail.Qty, oImportClaimDetail.UnitPrice, oImportClaimDetail.Note, nUserId, (int)eEnumDBOperation,"");
        }
        public static void Delete(TransactionContext tc, ImportClaimDetail oImportClaimDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ImportClaimDetail]"
                                    + "%n,%n,%n,  %n, %n, %s,%n, %n, %s",
                                    oImportClaimDetail.ImportClaimDetailID, oImportClaimDetail.ImportClaimID, oImportClaimDetail.ProductID, oImportClaimDetail.Qty, oImportClaimDetail.UnitPrice, oImportClaimDetail.Note, nUserId, (int)eEnumDBOperation,"");
        }

        public static void Delete(TransactionContext tc, ImportClaimDetail oImportClaimDetail, EnumDBOperation eEnumDBImportClaimDetail, Int64 nUserId, string sPIDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportClaimDetail]"
                                    + "%n,%n,%n,  %n, %n, %s,%n, %n, %s",
                                    oImportClaimDetail.ImportClaimDetailID, oImportClaimDetail.ImportClaimID, oImportClaimDetail.ProductID, oImportClaimDetail.Qty, oImportClaimDetail.UnitPrice, oImportClaimDetail.Note, nUserId, (int)eEnumDBImportClaimDetail,   sPIDetailIDs);  
        }
        
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportClaimDetail WHERE ImportClaimDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportClaimDetail");
        }
        public static IDataReader Gets(int ImportClaimID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportClaimDetail WHERE ImportClaimID =" + ImportClaimID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class ImportProductDetailDA
    {
        public ImportProductDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImportProductDetail oImportProductDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId,string sDetailIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ImportProductDetail]"
                                    + "%n,%n, %n, %n, %n,%s",
                                    oImportProductDetail.ImportProductDetailID, oImportProductDetail.ImportProductID, oImportProductDetail.ProductCategoryID, nUserId, (int)eEnumDBOperation, sDetailIDs);
        }
        public static void Delete(TransactionContext tc, ImportProductDetail oImportProductDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ImportProductDetail]"
                                    + "%n,%n, %n, %n, %n,%s",
                                        oImportProductDetail.ImportProductDetailID, oImportProductDetail.ImportProductID, oImportProductDetail.ProductCategoryID, nUserId, (int)eEnumDBOperation, sDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportProductDetail WHERE ImportProductDetailID=%n", nID);
        }


        public static IDataReader Gets(TransactionContext tc,int nIPID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportProductDetail WHERE ImportProductID=%n", nIPID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
     
    
     
    
        #endregion
    }
}
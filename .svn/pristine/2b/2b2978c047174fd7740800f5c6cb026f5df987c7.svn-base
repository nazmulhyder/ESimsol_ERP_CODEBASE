using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportPIDetailDA
    {
        public ImportPIDetailDA() { }
        #region New Version By Mohammad Shahjada Sagor
        public static IDataReader InsertUpdate(TransactionContext tc, ImportPIDetail oImportPIDetail, EnumDBOperation eEnumDBImportPIDetail, Int64 nUserId, string sIPDIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportPIDetail]"
                                    + "%n, %n,%n,%n,%n,%n,%n,%n,%s,%n,%s, %n,%n,%s",
                                   oImportPIDetail.ImportPIDetailID, oImportPIDetail.ImportPIID, oImportPIDetail.ProductID, oImportPIDetail.MUnitID, oImportPIDetail.UnitPrice,  oImportPIDetail.Qty, oImportPIDetail.Amount,oImportPIDetail.FreightRate,  oImportPIDetail.Note, oImportPIDetail.TechnicalSheetID,  oImportPIDetail.Shade,  nUserId, (int)eEnumDBImportPIDetail, sIPDIDs);
        }



        public static void Delete(TransactionContext tc, ImportPIDetail oImportPIDetail, EnumDBOperation eEnumDBImportPIDetail, Int64 nUserId, string sIPDIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportPIDetail]"
                                    + "%n, %n,%n,%n,%n,%n,%n,%n,%s,%n,%s, %n,%n,%s",
                                   oImportPIDetail.ImportPIDetailID, oImportPIDetail.ImportPIID, oImportPIDetail.ProductID, oImportPIDetail.MUnitID, oImportPIDetail.UnitPrice, oImportPIDetail.Qty, oImportPIDetail.Amount, oImportPIDetail.FreightRate, oImportPIDetail.Note, oImportPIDetail.TechnicalSheetID, oImportPIDetail.Shade, nUserId, (int)eEnumDBImportPIDetail, sIPDIDs);
        }

       

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIDetail WHERE ImportPIDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nImportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIDetail WHERE ImportPIID=%n", nImportPIID);
        }
         
        public static IDataReader GetsByImportPIID(TransactionContext tc, int nImportPIId)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIDetail WHERE ImportPIID = %n", nImportPIId);
        }     
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        


        #endregion
    }
}

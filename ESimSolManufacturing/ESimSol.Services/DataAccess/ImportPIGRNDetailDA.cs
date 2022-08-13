using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportPIGRNDetailDA
    {
        public ImportPIGRNDetailDA() { }
        #region New Version By Mohammad  Mhabub Alam
        public static IDataReader InsertUpdate(TransactionContext tc, ImportPIGRNDetail oImportPIGRNDetail, EnumDBOperation eEnumDBImportPIGRNDetail, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportPIGRNDetail]"
                                    + "%n, %n,%n,%n,%n,%n,%s,%n,%n,%s",
                                   oImportPIGRNDetail.ImportPIGRNDetailID, oImportPIGRNDetail.ImportPIID,  oImportPIGRNDetail.ProductID, oImportPIGRNDetail.MUnitID, oImportPIGRNDetail.UnitPrice, oImportPIGRNDetail.Qty, oImportPIGRNDetail.Note, nUserId, (int)eEnumDBImportPIGRNDetail, "");
        }



        public static void Delete(TransactionContext tc, ImportPIGRNDetail oImportPIGRNDetail, EnumDBOperation eEnumDBImportPIGRNDetail, Int64 nUserId, string sDRDIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportPIGRNDetail]"
                                    + "%n, %n,%n,%n,%n,%n,%s,%n,%n,%s",
                                   oImportPIGRNDetail.ImportPIGRNDetailID, oImportPIGRNDetail.ImportPIID, oImportPIGRNDetail.ProductID, oImportPIGRNDetail.MUnitID, oImportPIGRNDetail.UnitPrice, oImportPIGRNDetail.Qty, oImportPIGRNDetail.Note, nUserId, (int)eEnumDBImportPIGRNDetail, sDRDIDs);
        }



        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIGRNDetail WHERE ImportPIGRNDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIGRNDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nImportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIGRNDetail WHERE ImportPIID=%n", nImportPIID);
        }
         
        public static IDataReader GetsByImportPIGRNID(TransactionContext tc, int nImportPIId)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIGRNDetail WHERE ImportPIID = %n", nImportPIId);
        }     
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

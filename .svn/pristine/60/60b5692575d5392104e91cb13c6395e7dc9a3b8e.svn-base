using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ImportPackDetailDA
    {

        #region Written by Mohammad Mahabub Alam, 26 sep 2016

        #region Insert, Update, Delete

        public static IDataReader InsertUpdate(TransactionContext tc, ImportPackDetail oImportPackDetail, EnumDBOperation eEnumDBImportPackDetail, Int64 nUserId, string sPackDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportPackDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                     oImportPackDetail.ImportPackDetailID,
                                     oImportPackDetail.ImportPackID,
                                     oImportPackDetail.ImportInvoiceDetailID,
                                     //oImportPackDetail.ProductID,
                                     //oImportPackDetail.LotNo,
                                     oImportPackDetail.NumberOfPack,
                                     oImportPackDetail.MUnitID,
                                     oImportPackDetail.QtyPerPack,
                                     oImportPackDetail.Qty,
                                      oImportPackDetail.MURate,
                                     oImportPackDetail.Remarks,
                                     nUserId,
                                     (int)eEnumDBImportPackDetail,
                                     sPackDetailIDs
                                     );
        }

        public static void Delete(TransactionContext tc, ImportPackDetail oImportPackDetail, EnumDBOperation eEnumDBImportPackDetail, Int64 nUserId, string sPackDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportPackDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                     oImportPackDetail.ImportPackDetailID,
                                     oImportPackDetail.ImportPackID,
                                     oImportPackDetail.ImportInvoiceDetailID,
                //oImportPackDetail.ProductID,
                //oImportPackDetail.LotNo,
                                     oImportPackDetail.NumberOfPack,
                                     oImportPackDetail.MUnitID,
                                     oImportPackDetail.QtyPerPack,
                                     oImportPackDetail.Qty,
                                       oImportPackDetail.MURate,
                                     oImportPackDetail.Remarks,
                                     nUserId,
                                     (int)eEnumDBImportPackDetail,
                                     sPackDetailIDs
                                     );
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPackDetail WHERE ImportPackDetailID=%n", nID);
        }
        public static IDataReader Gets(int nImportPackID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_ImportPackDetail where ImportPackID=%n order by ProductID, LotNo ", nImportPackID);
        }
        public static IDataReader GetsByInvioce(int nImportInvoiceID, TransactionContext tc)
        {
            return tc.ExecuteReader(" select * from View_ImportPackDetail where ImportPackID in (Select ImportPackID from ImportPack where ImportInvoiceID=%n) order by ProductID, LotNo", nImportInvoiceID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

        #endregion
    }
}

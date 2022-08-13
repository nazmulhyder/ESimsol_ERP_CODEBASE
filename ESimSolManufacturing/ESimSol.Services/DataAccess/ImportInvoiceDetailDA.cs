using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ImportInvoiceDetailDA
    {
        public ImportInvoiceDetailDA() { }


        #region New Version By Mohammad Mahabub Alam, 21 Aug 2016

        #region Insert, Update, Delete

        public static IDataReader InsertUpdate(TransactionContext tc, ImportInvoiceDetail oImportInvoiceDetail, EnumDBOperation eEnumDBImportInvoiceDetail, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportInvoiceDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%s",
                                     oImportInvoiceDetail.ImportInvoiceDetailID,
                                     oImportInvoiceDetail.ImportInvoiceID,
                                     oImportInvoiceDetail.ProductID,
                                     oImportInvoiceDetail.UnitPrice,
                                     oImportInvoiceDetail.Qty,
                                     oImportInvoiceDetail.MUnitID,
                                     oImportInvoiceDetail.ImportPIDetailID,
                                     nUserId,
                                     (int)eEnumDBImportInvoiceDetail,
                                     ""
                                     );
        }

        public static void Delete(TransactionContext tc, ImportInvoiceDetail oImportInvoiceDetail, EnumDBOperation eEnumDBImportInvoiceDetail, Int64 nUserId, string sPIDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportInvoiceDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%s",
                                     oImportInvoiceDetail.ImportInvoiceDetailID,
                                     oImportInvoiceDetail.ImportInvoiceID,
                                     oImportInvoiceDetail.ProductID,
                                     oImportInvoiceDetail.UnitPrice,
                                     oImportInvoiceDetail.Qty,
                                     oImportInvoiceDetail.MUnitID,
                                     oImportInvoiceDetail.ImportPIDetailID,
                                     nUserId,
                                     (int)eEnumDBImportInvoiceDetail,
                                     sPIDetailIDs
                                     );
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportInvoiceDetail WHERE ImportInvoiceDetailID=%n", nID);
        }
        public static IDataReader Gets(int nImportInvoiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportInvoiceDetail WHERE ImportInvoiceID=%n", nImportInvoiceID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

        #endregion
    }
}

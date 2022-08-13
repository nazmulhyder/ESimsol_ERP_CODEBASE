using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class PurchaseInvoiceDetailDA
    {
        public PurchaseInvoiceDetailDA() { }

        #region Insert, Update, Delete
        public static IDataReader InsertUpdate(TransactionContext tc, PurchaseInvoiceDetail oPurchaseInvoiceDetail, EnumDBOperation eEnumDBPurchaseInvoiceDetail, Int64 nUserId, string sPurchaseInvoiceDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PurchaseInvoiceDetail]" + "%n, %n,%n, %n,%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s,%n, %n,%n, %n ,%n, %s",
                                     oPurchaseInvoiceDetail.PurchaseInvoiceDetailID, oPurchaseInvoiceDetail.PurchaseInvoiceID, oPurchaseInvoiceDetail.OrderRecapID, oPurchaseInvoiceDetail.ProductID, oPurchaseInvoiceDetail.RefID, oPurchaseInvoiceDetail.UnitPrice, oPurchaseInvoiceDetail.Qty, oPurchaseInvoiceDetail.ReceiveQty, oPurchaseInvoiceDetail.MUnitID, oPurchaseInvoiceDetail.RefDetailID, oPurchaseInvoiceDetail.GRNID, oPurchaseInvoiceDetail.Amount, oPurchaseInvoiceDetail.AdvanceSettle, oPurchaseInvoiceDetail.LCID, oPurchaseInvoiceDetail.InvoiceID, oPurchaseInvoiceDetail.CostHeadID, oPurchaseInvoiceDetail.Remarks, oPurchaseInvoiceDetail.VehicleModelID, oPurchaseInvoiceDetail.LandingCostTypeInt, oPurchaseInvoiceDetail.InvoiceDetailID, nUserId, (int)eEnumDBPurchaseInvoiceDetail, sPurchaseInvoiceDetailIDs);
        }

        public static void Delete(TransactionContext tc, PurchaseInvoiceDetail oPurchaseInvoiceDetail, EnumDBOperation eEnumDBPurchaseInvoiceDetail, Int64 nUserId, string sPurchaseInvoiceDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PurchaseInvoiceDetail]" + "%n, %n,%n, %n,%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s,%n, %n,%n, %n ,%n, %s",
                                     oPurchaseInvoiceDetail.PurchaseInvoiceDetailID, oPurchaseInvoiceDetail.PurchaseInvoiceID, oPurchaseInvoiceDetail.OrderRecapID, oPurchaseInvoiceDetail.ProductID, oPurchaseInvoiceDetail.RefID, oPurchaseInvoiceDetail.UnitPrice, oPurchaseInvoiceDetail.Qty, oPurchaseInvoiceDetail.ReceiveQty, oPurchaseInvoiceDetail.MUnitID, oPurchaseInvoiceDetail.RefDetailID, oPurchaseInvoiceDetail.GRNID, oPurchaseInvoiceDetail.Amount, oPurchaseInvoiceDetail.AdvanceSettle, oPurchaseInvoiceDetail.LCID, oPurchaseInvoiceDetail.InvoiceID, oPurchaseInvoiceDetail.CostHeadID, oPurchaseInvoiceDetail.Remarks, oPurchaseInvoiceDetail.VehicleModelID, oPurchaseInvoiceDetail.LandingCostTypeInt, oPurchaseInvoiceDetail.InvoiceDetailID, nUserId, (int)eEnumDBPurchaseInvoiceDetail, sPurchaseInvoiceDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseInvoiceDetail WHERE PurchaseInvoiceDetailID=%n", nID);
        }
        public static IDataReader Gets(int nPurchaseInvoiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_PurchaseInvoiceDetail where PurchaseInvoiceID=%n", nPurchaseInvoiceID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByPurchaseInvoiceID(TransactionContext tc, int nPurchaseInvoiceId)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseInvoiceDetail WHERE PurchaseInvoiceID = %n ", nPurchaseInvoiceId);
        }
        #endregion
    }
}

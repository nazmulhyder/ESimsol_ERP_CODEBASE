using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;



namespace ESimSol.Services.DataAccess
{

    public class PurchaseOrderDetailDA
    {
        public PurchaseOrderDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PurchaseOrderDetail oPRDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_PurchaseOrderDetail]" + "%n, %n,%n, %n, %n, %n, %n, %s,%n, %n, %n, %n,%s",
                oPRDetail.PODetailID, oPRDetail.POID,oPRDetail.OrderRecapID, oPRDetail.ProductID, oPRDetail.Qty, oPRDetail.UnitPrice, oPRDetail.MUnitID, oPRDetail.Note, oPRDetail.VehicleModelID,  nUserID, (int)eEnumDBOperation,oPRDetail.RefDetailID, sDetailIDs);
        }

        public static void Delete(TransactionContext tc, PurchaseOrderDetail oPRDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_PurchaseOrderDetail]" + "%n, %n,%n, %n, %n, %n, %n, %s,%n, %n, %n,%n,%s",
                oPRDetail.PODetailID, oPRDetail.POID, oPRDetail.OrderRecapID, oPRDetail.ProductID, oPRDetail.Qty, oPRDetail.UnitPrice, oPRDetail.MUnitID, oPRDetail.Note, oPRDetail.VehicleModelID, nUserID, (int)eEnumDBOperation,oPRDetail.RefDetailID, sDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseOrderDetail WHERE PODetailID=%n", nID);
        }

        public static IDataReader Gets(int nPurchaseOrderID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseOrderDetail AS POD WHERE POD.POID =%n", nPurchaseOrderID);
        }
        public static IDataReader GetsForInvoice(PurchaseInvoice oPurchaseInvoice, TransactionContext tc)
        {
            if ((EnumPInvoiceType)oPurchaseInvoice.InvoiceTypeInt == EnumPInvoiceType.Standard)
            {
                return tc.ExecuteReader("SELECT * FROM View_PurchaseOrderDetail AS POD WHERE POD.POID =%n AND POD.YetToInvoiceQty>0", oPurchaseInvoice.RefID);   //oPurchaseInvoice.RefID (zero given for test, because RefID not in Purchace Invoice)
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_PurchaseOrderDetail AS POD WHERE POD.POID =%n", oPurchaseInvoice.RefID);     //oPurchaseInvoice.RefID (zero given for test, because RefID not in Purchace Invoice)
            }
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    

}

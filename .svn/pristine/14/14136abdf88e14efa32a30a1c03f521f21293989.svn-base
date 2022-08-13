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

    public class PurchaseQuotationDetailDA
    {
        public PurchaseQuotationDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PurchaseQuotationDetail oPurchaseQuotationDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sPurchaseQuotationDetailIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_PurchaseQuotationDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%n,%n,%n,%n,%n,%n,%n,%s", oPurchaseQuotationDetail.PurchaseQuotationDetailID, oPurchaseQuotationDetail.PurchaseQuotationID, oPurchaseQuotationDetail.ProductID, oPurchaseQuotationDetail.MUnitID, oPurchaseQuotationDetail.UnitPrice, oPurchaseQuotationDetail.Quantity, oPurchaseQuotationDetail.ItemDescription, oPurchaseQuotationDetail.ActualPrice, oPurchaseQuotationDetail.Discount, oPurchaseQuotationDetail.Vat, oPurchaseQuotationDetail.TransportCost, oPurchaseQuotationDetail.PRDetailID, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, PurchaseQuotationDetail oPurchaseQuotationDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sPurchaseQuotationDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_PurchaseQuotationDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%n,%n,%n,%n,%n,%n,%n,%s", oPurchaseQuotationDetail.PurchaseQuotationDetailID, oPurchaseQuotationDetail.PurchaseQuotationID, oPurchaseQuotationDetail.ProductID, oPurchaseQuotationDetail.MUnitID, oPurchaseQuotationDetail.UnitPrice, oPurchaseQuotationDetail.Quantity, oPurchaseQuotationDetail.ItemDescription, oPurchaseQuotationDetail.ActualPrice, oPurchaseQuotationDetail.Discount, oPurchaseQuotationDetail.Vat, oPurchaseQuotationDetail.TransportCost, oPurchaseQuotationDetail.PRDetailID, nUserID, (int)eEnumDBOperation, sPurchaseQuotationDetailIDs);
        }


        public static IDataReader Approve(TransactionContext tc, PurchaseQuotationDetail oPurchaseQuotationDetail,  long nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_ApprovePurchaseQuotation]"
                                    + "%n,%b,%n", oPurchaseQuotationDetail.PurchaseQuotationDetailID,oPurchaseQuotationDetail.IsCheck,nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseQuotationDetail WHERE PurchaseQuotationDetailID=%n", nID);
        }

        public static IDataReader Gets(int nPurchaseQuotationID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseQuotationDetail where PurchaseQuotationID =%n ", nPurchaseQuotationID);
        }
        public static IDataReader GetsByLog(int nPurchaseQuotationLogID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseQuotationDetailLog where PurchaseQuotationLogID=%n", nPurchaseQuotationLogID);
        }
        public static IDataReader GetsForNOA(TransactionContext tc, int nProductID, int nMunitID)
        {
            return tc.ExecuteReader("select * from (SELECT *, ROW_NUMBER() OVER (Partition by SupplierID Order by PQ.PurchaseQuotationDetailID Desc)  AS RowNumber FROM View_PurchaseQuotationDetail as PQ where ProductID=%n AND MUnitID = %n) as P where P.RowNumber=1", nProductID, nMunitID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nProductID, int nMunitID)
        {
            return tc.ExecuteReader("select *  FROM View_PurchaseQuotationDetail as PQ where ProductID=%n AND MUnitID=%n order by SupplierID ", nProductID, nMunitID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    

}

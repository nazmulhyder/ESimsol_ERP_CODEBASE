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


    public class PurchaseQuotationDA
    {
        public PurchaseQuotationDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PurchaseQuotation oPurchaseQuotation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PurchaseQuotation]"
                                    + "%n, %s,%n,%s,%d,%d,%n,%n,%n,%n,%n,%s,%n,%b,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                                    oPurchaseQuotation.PurchaseQuotationID, oPurchaseQuotation.PurchaseQuotationNo, oPurchaseQuotation.QuotationStatusInInt, oPurchaseQuotation.SupplierReference, oPurchaseQuotation.RateCollectDate, oPurchaseQuotation.ExpiredDate, oPurchaseQuotation.SCPerson, oPurchaseQuotation.CollectBy, oPurchaseQuotation.CurrencyID, oPurchaseQuotation.SupplierID, oPurchaseQuotation.BuyerID, oPurchaseQuotation.Remarks, (int)oPurchaseQuotation.Source, oPurchaseQuotation.Activity, oPurchaseQuotation.PaymentTerm, oPurchaseQuotation.DiscountInAmount, oPurchaseQuotation.DiscountInPercent, oPurchaseQuotation.VatInPercent, oPurchaseQuotation.VatInAmount, oPurchaseQuotation.TransportCostInPercent,oPurchaseQuotation.TransportCostInAmount, oPurchaseQuotation.BUID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, PurchaseQuotation oPurchaseQuotation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PurchaseQuotation]"
                                    + "%n, %s,%n,%s,%d,%d,%n,%n,%n,%n,%n,%s,%n,%b,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                                    oPurchaseQuotation.PurchaseQuotationID, oPurchaseQuotation.PurchaseQuotationNo, oPurchaseQuotation.QuotationStatusInInt, oPurchaseQuotation.SupplierReference, oPurchaseQuotation.RateCollectDate, oPurchaseQuotation.ExpiredDate, oPurchaseQuotation.SCPerson, oPurchaseQuotation.CollectBy, oPurchaseQuotation.CurrencyID, oPurchaseQuotation.SupplierID, oPurchaseQuotation.BuyerID, oPurchaseQuotation.Remarks, (int)oPurchaseQuotation.Source, oPurchaseQuotation.Activity, oPurchaseQuotation.PaymentTerm, oPurchaseQuotation.DiscountInAmount, oPurchaseQuotation.DiscountInPercent, oPurchaseQuotation.VatInPercent, oPurchaseQuotation.VatInAmount, oPurchaseQuotation.TransportCostInPercent, oPurchaseQuotation.TransportCostInAmount, oPurchaseQuotation.BUID, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        public static IDataReader RequestQuotationRevise(TransactionContext tc, PurchaseQuotation oPQ, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_PurchaseQuotationRevise]" + "%n,%n,%n",
                                    oPQ.PurchaseQuotationID
                                    , oPQ.QuotationStatusInInt
                                    , nUserID);
        }
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseQuotation WHERE PurchaseQuotationID=%n", nID);
        }
        public static IDataReader GetByLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseQuotationLog WHERE PurchaseQuotationLogID=%n", nID);
        }
        public static void SendToMgt(TransactionContext tc, long nID)
        {
            tc.ExecuteNonQuery("Update PurchaseQuotation  SET QuotationStatus=" + (int)EnumQuotationStatus.Approve + " WHERE PurchaseQuotationID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseQuotation  where Isnull(ApprovedBy,0)=0 order by Isnull(PRID,0) RateCollectDate");
        }
        public static IDataReader GetsByBU(int nBUID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseQuotation  WHERE  Isnull(BUID,0)= %n ORDER BY RateCollectDate", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByLog(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }  
    
   
}

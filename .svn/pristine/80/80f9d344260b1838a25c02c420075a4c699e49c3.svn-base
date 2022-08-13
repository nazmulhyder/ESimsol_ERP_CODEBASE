using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class TradingDeliveryChallanDA
    {
        public TradingDeliveryChallanDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TradingDeliveryChallan oTradingDeliveryChallan, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TradingDeliveryChallan]" + "%n, %n, %n, %s, %d, %n, %n, %n, %n, %s, %n, %n, %n",
                        oTradingDeliveryChallan.TradingDeliveryChallanID, oTradingDeliveryChallan.BUID, oTradingDeliveryChallan.TradingSaleInvoiceID, oTradingDeliveryChallan.ChallanNo, oTradingDeliveryChallan.ChallanDate, oTradingDeliveryChallan.BuyerID, oTradingDeliveryChallan.StoreID, oTradingDeliveryChallan.CurrencyID, oTradingDeliveryChallan.NetAmount, oTradingDeliveryChallan.Note, oTradingDeliveryChallan.DeliveryBy, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, TradingDeliveryChallan oTradingDeliveryChallan, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TradingDeliveryChallan]" + "%n, %n, %n, %s, %d, %n, %n, %n, %n, %s, %n, %n, %n",
                        oTradingDeliveryChallan.TradingDeliveryChallanID, oTradingDeliveryChallan.BUID, oTradingDeliveryChallan.TradingSaleInvoiceID, oTradingDeliveryChallan.ChallanNo, oTradingDeliveryChallan.ChallanDate, oTradingDeliveryChallan.BuyerID, oTradingDeliveryChallan.StoreID, oTradingDeliveryChallan.CurrencyID, oTradingDeliveryChallan.NetAmount, oTradingDeliveryChallan.Note, oTradingDeliveryChallan.DeliveryBy, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader Disburse(TransactionContext tc, TradingDeliveryChallan oTradingDeliveryChallan, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CommitTradingDeliveryChallan]" + "%n, %n", oTradingDeliveryChallan.TradingDeliveryChallanID, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingDeliveryChallan WHERE TradingDeliveryChallanID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingDeliveryChallan");
        }

        public static IDataReader GetsInitialTradingDeliveryChallans(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingDeliveryChallan AS HH WHERE ISNULL(HH.DeliveryBy,0)=0 AND HH.BUID=%n ORDER BY HH.TradingDeliveryChallanID ASC", nBUID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

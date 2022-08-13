using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class TradingPaymentDA
    {
        public TradingPaymentDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TradingPayment oTradingPayment, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TradingPayment]" + "%n, %n, %n, %n, %n, %s, %d, %d, %n, %n, %n, %n, %n, %n, %d, %s, %s, %n, %n, %n, %n",
                oTradingPayment.TradingPaymentID, oTradingPayment.BUID, oTradingPayment.ContractorID, oTradingPayment.ContactPersonnelID, oTradingPayment.AccountHeadID, 
                oTradingPayment.RefNo, oTradingPayment.PaymentDate, oTradingPayment.EncashmentDate, oTradingPayment.PaymentMethodInt, oTradingPayment.CurrencyID, 
                oTradingPayment.Amount, oTradingPayment.ReferenceTypeInt, oTradingPayment.TradingPaymentStatusInt, oTradingPayment.ApprovedBy, oTradingPayment.ApprovedDate, 
                oTradingPayment.Note, oTradingPayment.ChequeNo, oTradingPayment.AccountID, oTradingPayment.SalesManID, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, TradingPayment oTradingPayment, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TradingPayment]" + "%n, %n, %n, %n, %n, %s, %d, %d, %n, %n, %n, %n, %n, %n, %d, %s, %s, %n, %n, %n, %n",
                oTradingPayment.TradingPaymentID, oTradingPayment.BUID, oTradingPayment.ContractorID, oTradingPayment.ContactPersonnelID, oTradingPayment.AccountHeadID, 
                oTradingPayment.RefNo, oTradingPayment.PaymentDate, oTradingPayment.EncashmentDate, oTradingPayment.PaymentMethodInt, oTradingPayment.CurrencyID, 
                oTradingPayment.Amount, oTradingPayment.ReferenceTypeInt, oTradingPayment.TradingPaymentStatusInt, oTradingPayment.ApprovedBy, oTradingPayment.ApprovedDate, 
                oTradingPayment.Note, oTradingPayment.ChequeNo, oTradingPayment.AccountID, oTradingPayment.SalesManID, nUserID, (int)eEnumDBOperation);
        }

        public static void Approved(TransactionContext tc, TradingPayment oTradingPayment, int nUserID)
        {
            tc.ExecuteNonQuery("UPDATE TradingPayment SET PaymentStatus=%n, ApprovedBy =%n, ApprovedDate=%d   WHERE TradingPaymentID =%n", (int)EnumPaymentStatus.Encashment, nUserID, DateTime.Now, oTradingPayment.TradingPaymentID);
        }
        public static IDataReader SalesmanTradingPayment(TransactionContext tc, int nTradingPaymentID, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CommitTradingPaymentSaleReturn]" + "%n, %n",nTradingPaymentID, nUserID);
        }
        
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingPayment WHERE TradingPaymentID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingPayment");
        }      

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsInitialTradingPayments(TransactionContext tc, int nBUID, EnumPaymentRefType eTradingPaymentRefType, int nTradingPaymentBy, int nUserID)
        {
            if (nTradingPaymentBy == 2) // SalesMan
            {
                return tc.ExecuteReader("SELECT * FROM View_TradingPayment WHERE BUID = %n  AND ReferenceType=%n AND ISNULL(ApprovedBy,0)=0 And ISNULL(SalesManID,0)>0 And SalesManID IN (Select ISNULL((Select top(1)SalesManID from SalesMan Where UserID = %n),0)) ORDER BY TradingPaymentID ASC", nBUID, (int)eTradingPaymentRefType, nUserID);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_TradingPayment WHERE BUID = %n  AND ReferenceType=%n AND ISNULL(ApprovedBy,0)=0 And ISNULL(SalesManID,0)=0  ORDER BY TradingPaymentID ASC", nBUID, (int)eTradingPaymentRefType);
            }

        }
        #endregion
    }
}


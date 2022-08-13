using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class SaleInvoiceDA
    {
        public SaleInvoiceDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SaleInvoice oSaleInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SaleInvoice]"
                + "%n,%s,%d ,%n,%n,%n,%b,%s ,%s,%s,%s,%s,%n ,%n,%n,%n,%n,%d ,%n,%s,%d,%n,%n  ,%s,%n,%n, %n,%n,%n,%n,     %n,%n,%n,    %n,%n,%n,%n,%n",
                oSaleInvoice.SaleInvoiceID, oSaleInvoice.InvoiceNo, oSaleInvoice.InvoiceDate, oSaleInvoice.ContractorID, oSaleInvoice.ContactPersonID, oSaleInvoice.SalesQuotationID,
                oSaleInvoice.IsNewOrder, oSaleInvoice.VehicleLocation, oSaleInvoice.PRNo, oSaleInvoice.SpecialInstruction, oSaleInvoice.ETAAgreement, oSaleInvoice.ETAWeeks,
                oSaleInvoice.CurrencyID, oSaleInvoice.OTRAmount, oSaleInvoice.DiscountAmount, oSaleInvoice.NetAmount, oSaleInvoice.AdvanceAmount,
                oSaleInvoice.AdvanceDate, oSaleInvoice.PaymentMode, oSaleInvoice.ChequeNo, oSaleInvoice.ChequeDate, oSaleInvoice.BankID, oSaleInvoice.ReceivedBy,
                oSaleInvoice.Remarks, oSaleInvoice.AttachmentDoc, oSaleInvoice.ApprovedBy,
                oSaleInvoice.BUID, oSaleInvoice.InvoiceStatusInt, oSaleInvoice.CRate, oSaleInvoice.ProductID,
                oSaleInvoice.VatAmount, oSaleInvoice.PDIChargeAmount, oSaleInvoice.FreeServiceChargeAmount,
                oSaleInvoice.PIID, oSaleInvoice.RegistrationFee, oSaleInvoice.TDSAmount, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, SaleInvoice oSaleInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SaleInvoice]"
                + "%n,%s,%d ,%n,%n,%n,%b,%s ,%s,%s,%s,%s,%n ,%n,%n,%n,%n,%d ,%n,%s,%d,%n,%n  ,%s,%n,%n, %n,%n,%n,%n,     %n,%n,%n,    %n,%n,%n,%n,%n",
                oSaleInvoice.SaleInvoiceID, oSaleInvoice.InvoiceNo, oSaleInvoice.InvoiceDate, oSaleInvoice.ContractorID, oSaleInvoice.ContactPersonID, oSaleInvoice.SalesQuotationID,
                oSaleInvoice.IsNewOrder, oSaleInvoice.VehicleLocation, oSaleInvoice.PRNo, oSaleInvoice.SpecialInstruction, oSaleInvoice.ETAAgreement, oSaleInvoice.ETAWeeks,
                oSaleInvoice.CurrencyID, oSaleInvoice.OTRAmount, oSaleInvoice.DiscountAmount, oSaleInvoice.NetAmount, oSaleInvoice.AdvanceAmount,
                oSaleInvoice.AdvanceDate, oSaleInvoice.PaymentMode, oSaleInvoice.ChequeNo, oSaleInvoice.ChequeDate, oSaleInvoice.BankID, oSaleInvoice.ReceivedBy,
                oSaleInvoice.Remarks, oSaleInvoice.AttachmentDoc, oSaleInvoice.ApprovedBy,
                oSaleInvoice.BUID, oSaleInvoice.InvoiceStatusInt, oSaleInvoice.CRate, oSaleInvoice.ProductID,
                oSaleInvoice.VatAmount, oSaleInvoice.PDIChargeAmount, oSaleInvoice.FreeServiceChargeAmount,
                oSaleInvoice.PIID, oSaleInvoice.RegistrationFee, oSaleInvoice.TDSAmount, nUserId, (int)eEnumDBOperation);
        }

        public static void Approve(TransactionContext tc, SaleInvoice oSaleInvoice, long nID)
        {
            tc.ExecuteNonQuery("UPDATE SaleInvoice SET ApprovedBy =" + nID + " WHERE SaleInvoiceID=%n", oSaleInvoice.SaleInvoiceID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SaleInvoice WHERE SaleInvoiceID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SaleInvoice");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

        public static void UpdateStatus(TransactionContext tc, SaleInvoice oSaleInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SaleInvoice]"
                + "%n,%s,%d ,%n,%n,%n,%b,%s ,%s,%s,%s,%s,%n ,%n,%n,%n,%n,%d ,%n,%s,%d,%n,%n  ,%s,%n,%n, %n,%n,%n,%n,     %n,%n,%n,    %n,%n,%n,%n,%n",
                oSaleInvoice.SaleInvoiceID, oSaleInvoice.InvoiceNo, oSaleInvoice.InvoiceDate, oSaleInvoice.ContractorID, oSaleInvoice.ContactPersonID, oSaleInvoice.SalesQuotationID,
                oSaleInvoice.IsNewOrder, oSaleInvoice.VehicleLocation, oSaleInvoice.PRNo, oSaleInvoice.SpecialInstruction, oSaleInvoice.ETAAgreement, oSaleInvoice.ETAWeeks,
                oSaleInvoice.CurrencyID, oSaleInvoice.OTRAmount, oSaleInvoice.DiscountAmount, oSaleInvoice.NetAmount, oSaleInvoice.AdvanceAmount,
                oSaleInvoice.AdvanceDate, oSaleInvoice.PaymentMode, oSaleInvoice.ChequeNo, oSaleInvoice.ChequeDate, oSaleInvoice.BankID, oSaleInvoice.ReceivedBy,
                oSaleInvoice.Remarks, oSaleInvoice.AttachmentDoc, oSaleInvoice.ApprovedBy,
                oSaleInvoice.BUID, oSaleInvoice.InvoiceStatusInt, oSaleInvoice.CRate, oSaleInvoice.ProductID,
                oSaleInvoice.VatAmount, oSaleInvoice.PDIChargeAmount, oSaleInvoice.FreeServiceChargeAmount,
                oSaleInvoice.PIID, oSaleInvoice.RegistrationFee, oSaleInvoice.TDSAmount, nUserID, (int)eEnumDBOperation);
        }
    }
}

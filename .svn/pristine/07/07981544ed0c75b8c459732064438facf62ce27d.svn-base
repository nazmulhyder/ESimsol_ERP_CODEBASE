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


    public class ProformaInvoiceDA
    {
        public ProformaInvoiceDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProformaInvoice oProformaInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ProformaInvoice]" + "%n,%n, %s, %d, %n, %n, %n, %n, %n, %n, %n, %s, %n, %s, %s, %s,%s,%n,%n,%n,%n,%n,%s,%s, %n, %n",
                                    oProformaInvoice.ProformaInvoiceID, oProformaInvoice.BUID, oProformaInvoice.PINo, oProformaInvoice.IssueDate, oProformaInvoice.PIStatus, oProformaInvoice.BuyerID, oProformaInvoice.LCFavorOf, oProformaInvoice.TransferBankAccountID, oProformaInvoice.UnitID, oProformaInvoice.CurrencyID, (int)oProformaInvoice.PaymentTerm, oProformaInvoice.Origin, (int)oProformaInvoice.DeliveryTerm, oProformaInvoice.PortOfLoadingAir, oProformaInvoice.PortOfLoadingSea, oProformaInvoice.Note, oProformaInvoice.SessionName,    oProformaInvoice.ApplicantID,  oProformaInvoice.GrossAmount, oProformaInvoice.DiscountAmount, oProformaInvoice.AdditionalAmount, oProformaInvoice.NetAmount, oProformaInvoice.CauseOfAddition, oProformaInvoice.CauseOfDiscount,  nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ProformaInvoice oProformaInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ProformaInvoice]" + "%n,%n, %s, %d, %n, %n, %n, %n, %n, %n, %n, %s, %n, %s, %s, %s,%s,%n,%n,%n,%n,%n,%s,%s, %n, %n",
                                    oProformaInvoice.ProformaInvoiceID, oProformaInvoice.BUID, oProformaInvoice.PINo, oProformaInvoice.IssueDate, oProformaInvoice.PIStatus, oProformaInvoice.BuyerID, oProformaInvoice.LCFavorOf, oProformaInvoice.TransferBankAccountID, oProformaInvoice.UnitID, oProformaInvoice.CurrencyID, (int)oProformaInvoice.PaymentTerm, oProformaInvoice.Origin, (int)oProformaInvoice.DeliveryTerm, oProformaInvoice.PortOfLoadingAir, oProformaInvoice.PortOfLoadingSea, oProformaInvoice.Note, oProformaInvoice.SessionName, oProformaInvoice.ApplicantID, oProformaInvoice.GrossAmount, oProformaInvoice.DiscountAmount, oProformaInvoice.AdditionalAmount, oProformaInvoice.NetAmount, oProformaInvoice.CauseOfAddition, oProformaInvoice.CauseOfDiscount, nUserID, (int)eEnumDBOperation);
        }


        public static IDataReader ChangeStatus(TransactionContext tc, ProformaInvoice oProformaInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_ProformaInvoiceOperation]" + "%n, %n, %n, %s, %n, %n, %n", 
                                   oProformaInvoice.ProformaInvoiceHistoryID, oProformaInvoice.ProformaInvoiceID, (int)oProformaInvoice.PIStatus, oProformaInvoice.sNote, (int)oProformaInvoice.ProformaInvoiceActionType, nUserID, (int)eEnumDBOperation);
        }

        #region Accept PI Revise
        public static IDataReader AcceptProformaInvoiceRevise(TransactionContext tc, ProformaInvoice oProformaInvoice, double nTotalNewDetailQty, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_AcceptProformaInvoiceRevise]" + "%n, %n,%s, %d, %n, %n, %n, %n, %n, %n, %s, %n, %s, %s, %s, %n, %n, %n, %n, %n, %n, %s,%s,%n",
                                    oProformaInvoice.ProformaInvoiceID, oProformaInvoice.BUID, oProformaInvoice.PINo, oProformaInvoice.IssueDate, oProformaInvoice.BuyerID, oProformaInvoice.LCFavorOf, oProformaInvoice.TransferBankAccountID, oProformaInvoice.UnitID, oProformaInvoice.CurrencyID, (int)oProformaInvoice.PaymentTerm, oProformaInvoice.Origin, (int)oProformaInvoice.DeliveryTerm, oProformaInvoice.PortOfLoadingAir, oProformaInvoice.PortOfLoadingSea, oProformaInvoice.Note, nTotalNewDetailQty, oProformaInvoice.ApplicantID, oProformaInvoice.GrossAmount, oProformaInvoice.DiscountAmount, oProformaInvoice.AdditionalAmount, oProformaInvoice.NetAmount, oProformaInvoice.CauseOfAddition, oProformaInvoice.CauseOfDiscount,  nUserID);
        }


        #endregion

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoice WHERE ProformaInvoiceID=%n", nID);
        }

        public static IDataReader GetLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceLog WHERE ProformaInvoiceLogID=%n", nID);
        }
        
        public static IDataReader Gets(TransactionContext tc, int buid)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoice WHERE BUID = %n",buid);
        }

        public static IDataReader GetsPILog(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceLog WHERE ProformaInvoiceID=%n", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    

}

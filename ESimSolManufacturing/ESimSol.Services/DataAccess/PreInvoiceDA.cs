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
    public class PreInvoiceDA
    {
        public PreInvoiceDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PreInvoice oPreInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PreInvoice]"
                               + "%n,%s,%d ,%n,%n,%n,%b,%s ,%s,%s,%s,%s,%n ,%n,%n,%n,%n,%d ,%n,%s,%d,%n,%n  ,%s,%n,%n, %n,%n,%n,%n,%s,%s,%n, %n,%n",
                                     oPreInvoice.PreInvoiceID, oPreInvoice.InvoiceNo, oPreInvoice.InvoiceDate, oPreInvoice.ContractorID, oPreInvoice.ContactPersonID, oPreInvoice.SalesQuotationID,
                                     oPreInvoice.IsNewOrder, oPreInvoice.VehicleLocation, oPreInvoice.PRNo, oPreInvoice.SpecialInstruction, oPreInvoice.ETAAgreement, oPreInvoice.ETAWeeks,
                                     oPreInvoice.CurrencyID, oPreInvoice.OTRAmount, oPreInvoice.DiscountAmount, oPreInvoice.NetAmount, oPreInvoice.AdvanceAmount,
                                     oPreInvoice.AdvanceDate, oPreInvoice.PaymentMode, oPreInvoice.ChequeNo, oPreInvoice.ChequeDate, oPreInvoice.BankID, oPreInvoice.ReceivedBy,
                                     oPreInvoice.Remarks, oPreInvoice.AttachmentDoc, oPreInvoice.ApprovedBy,
                                     oPreInvoice.BUID, oPreInvoice.InvoiceStatusInt, oPreInvoice.CRate, oPreInvoice.ProductID,
                                     oPreInvoice.Phone, oPreInvoice.Email, oPreInvoice.OfferedFreeService, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, PreInvoice oPreInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PreInvoice]"
                               + "%n,%s,%d ,%n,%n,%n,%b,%s ,%s,%s,%s,%s,%n ,%n,%n,%n,%n,%d ,%n,%s,%d,%n,%n  ,%s,%n,%n, %n,%n,%n,%n,%s,%s,%n, %n,%n",
                                     oPreInvoice.PreInvoiceID, oPreInvoice.InvoiceNo, oPreInvoice.InvoiceDate, oPreInvoice.ContractorID, oPreInvoice.ContactPersonID, oPreInvoice.SalesQuotationID,
                                     oPreInvoice.IsNewOrder, oPreInvoice.VehicleLocation, oPreInvoice.PRNo, oPreInvoice.SpecialInstruction, oPreInvoice.ETAAgreement, oPreInvoice.ETAWeeks,
                                     oPreInvoice.CurrencyID, oPreInvoice.OTRAmount, oPreInvoice.DiscountAmount, oPreInvoice.NetAmount, oPreInvoice.AdvanceAmount,
                                     oPreInvoice.AdvanceDate, oPreInvoice.PaymentMode, oPreInvoice.ChequeNo, oPreInvoice.ChequeDate, oPreInvoice.BankID, oPreInvoice.ReceivedBy,
                                     oPreInvoice.Remarks, oPreInvoice.AttachmentDoc, oPreInvoice.ApprovedBy,
                                     oPreInvoice.BUID, oPreInvoice.InvoiceStatusInt, oPreInvoice.CRate, oPreInvoice.ProductID,
                                     oPreInvoice.Phone, oPreInvoice.Email, oPreInvoice.OfferedFreeService, nUserId, (int)eEnumDBOperation);
        }
        public static void Approve(TransactionContext tc, PreInvoice oPreInvoice, long nID)
        {
            tc.ExecuteNonQuery("UPDATE PreInvoice SET ApprovedBy =" + nID + " WHERE PreInvoiceID=%n", oPreInvoice.PreInvoiceID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PreInvoice WHERE PreInvoiceID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PreInvoice");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static void UpdateForHandoverCheckList(TransactionContext tc, PreInvoice oPreInvoice)
        {
            tc.ExecuteNonQuery("UPDATE PreInvoice SET HandoverDate = %D ,  VehicleMileage = %n,  WheelCondition = %s, BodyWorkCondition = %s,  InteriorCondition = %s, DealerPerson = %s, Owner = %s, OwnerNID = %s, Note = %s   WHERE PreInvoiceID = %n", oPreInvoice.HandoverDate, oPreInvoice.VehicleMileage, oPreInvoice.WheelCondition, oPreInvoice.BodyWorkCondition, oPreInvoice.InteriorCondition, oPreInvoice.DealerPerson, oPreInvoice.Owner, oPreInvoice.OwnerNID, oPreInvoice.Note, oPreInvoice.PreInvoiceID);
        }

        #endregion

        public static void UpdateStatus(TransactionContext tc, PreInvoice oPreInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PreInvoice]"
                               + "%n,%s,%d ,%n,%n,%n,%b,%s ,%s,%s,%s,%s,%n ,%n,%n,%n,%n,%d ,%n,%s,%d,%n,%n  ,%s,%n,%n, %n,%n,%n,%n,%s,%s,%n, %n,%n",
                                     oPreInvoice.PreInvoiceID, oPreInvoice.InvoiceNo, oPreInvoice.InvoiceDate, oPreInvoice.ContractorID, oPreInvoice.ContactPersonID, oPreInvoice.SalesQuotationID,
                                     oPreInvoice.IsNewOrder, oPreInvoice.VehicleLocation, oPreInvoice.PRNo, oPreInvoice.SpecialInstruction, oPreInvoice.ETAAgreement, oPreInvoice.ETAWeeks,
                                     oPreInvoice.CurrencyID, oPreInvoice.OTRAmount, oPreInvoice.DiscountAmount, oPreInvoice.NetAmount, oPreInvoice.AdvanceAmount,
                                     oPreInvoice.AdvanceDate, oPreInvoice.PaymentMode, oPreInvoice.ChequeNo, oPreInvoice.ChequeDate, oPreInvoice.BankID, oPreInvoice.ReceivedBy,
                                     oPreInvoice.Remarks, oPreInvoice.AttachmentDoc, oPreInvoice.ApprovedBy,
                                     oPreInvoice.BUID, oPreInvoice.InvoiceStatusInt, oPreInvoice.CRate, oPreInvoice.ProductID,
                                     oPreInvoice.Phone, oPreInvoice.Email, oPreInvoice.OfferedFreeService, nUserID, (int)eEnumDBOperation);
        }
    }
}

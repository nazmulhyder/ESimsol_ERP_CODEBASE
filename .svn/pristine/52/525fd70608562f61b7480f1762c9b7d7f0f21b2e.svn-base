using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;
namespace ESimSol.Services.DataAccess
{
    public class PaymentDA 
    {
        public PaymentDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Payment oPayment, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Payment]" + "%n, %s, %d,  %n, %n, %n, %n, %n, %n, %n, %s, %n, %d, %n, %n, %n, %s, %n, %n, %n, %n",
                                    oPayment.PaymentID, oPayment.MRNo, oPayment.MRDate, oPayment.ContractorID, (int)oPayment.ContactPersonnelID, (int)oPayment.PaymentType, (int)oPayment.PaymentModeInt, oPayment.PaymentStatusInInt,  oPayment.Amount, oPayment.PaymentDocID, oPayment.DocNo, oPayment.DocAmount, oPayment.DocDate, oPayment.BankID, oPayment.CurrencyID, oPayment.CRate,oPayment.Note, oPayment.BUID, oPayment.BankAccountID_Deposit,  nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, Payment oPayment, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Payment]" + "%n, %s, %d,  %n, %n, %n, %n, %n, %n, %n, %s, %n, %d, %n, %n, %n, %s, %n, %n, %n, %n",
                                    oPayment.PaymentID, oPayment.MRNo, oPayment.MRDate, oPayment.ContractorID, (int)oPayment.ContactPersonnelID, (int)oPayment.PaymentType, (int)oPayment.PaymentModeInt, oPayment.PaymentStatusInInt, oPayment.Amount, oPayment.PaymentDocID, oPayment.DocNo, oPayment.DocAmount, oPayment.DocDate, oPayment.BankID, oPayment.CurrencyID, oPayment.CRate, oPayment.Note, oPayment.BUID, oPayment.BankAccountID_Deposit, nUserId, (int)eEnumDBOperation);
        }


       
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Payment WHERE PaymentID=%n", nID);
        }
        public static IDataReader GetBy(TransactionContext tc, int nDebitNoteID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Payment WHERE DebitNoteID=%n", nDebitNoteID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Payment");
        }

        public static IDataReader Gets(TransactionContext tc, EnumPaymentType ePaymentType)
        {
            return tc.ExecuteReader("SELECT * FROM View_Payment WHERE PaymentType=%n", (int)ePaymentType);
        }

        public static IDataReader GetMaxPayment(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Payment WHERE PaymentID=(SELECT MAX(PaymentID) FROM Payment)");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void UpdateVoucherEffect(TransactionContext tc, Payment oPayment)
        {
            tc.ExecuteNonQuery(" Update Payment SET IsWillVoucherEffect = %b WHERE PaymentID  = %n", oPayment.IsWillVoucherEffect, oPayment.PaymentID);
        }  
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class SalesComPaymentDA
    {
        public SalesComPaymentDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, SalesComPayment oSCP, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalesComPayment] "
                + "%n , %d ,%n , %n , %n ,  %s , %n ,%n ,%s, %d , %n , %n , %n , %n ,%n "
                , oSCP.SalesComPaymentID,oSCP.MRDate,(int)oSCP.PaymentMode,(int)oSCP.PaymentType,oSCP.ContactPersonnelID,oSCP.Note,oSCP.CRate,oSCP.CurrencyID,oSCP.DocNo,oSCP.DocDate,oSCP.BankAccountID,oSCP.BUID,oSCP.SampleInvoiceID,nUserID,nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nSalesComPaymentID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesComPayment WHERE SalesComPaymentID=%n", nSalesComPaymentID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

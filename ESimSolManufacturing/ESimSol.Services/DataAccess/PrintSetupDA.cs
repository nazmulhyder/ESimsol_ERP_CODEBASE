using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Base.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Base.Client.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PrintSetupDA
    {
        public PrintSetupDA() { }

        #region Insert Update Delete Function
        public static void InsertUpdate(TransactionContext tc, PrintSetup oPrintSetup, Int64 nUserId)
        {
            tc.ExecuteNonQuery("INSERT INTO PrintSetup(PrintSetupID, BankID, Length, Width, PaymentMethodX, paymentMethodY, paymentMethodL, paymentMethodW, paymentMethodFS, DateX, DateY, DateL, DateW, DateFS, PayToX, PayToY, PayToL, PayToW, PayToFS, AmountWordX, AmountWordY, AmountWordL, AmountWordW, AmountWordFS, AmountX, AmountY, AmountL, AmountW, AmountFS, TBookNoX, TBookNoY, TBookNoL, TBookNoW, TBookNoFS, TPaymentTypeX, TPaymentTypeY, TPaymentTypeL, TPaymentTypeW, TPaymentTypeFS, TDateX, TDateY, TDateL, TDateW, TDateFS, TPayToX, TPayToY, TPayToL, TPayToW, TPayToFS, TForNoteX, TForNoteY, TForNoteL, TForNoteW, TForNoteFS, TAmountX, TAmountY, TAmountL, TAmountW, TAmountFS, TVoucherNoX, TVoucherNoY, TVoucherNoL, TVoucherNoW, TVoucherNoFS, DateFormat, IsSplit, PrinterGraceWidth)"
                + " VALUES(%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %b, %n)",
                oPrintSetup.ObjectID, oPrintSetup.BankID, oPrintSetup.Length, oPrintSetup.Width, oPrintSetup.PaymentMethodX, oPrintSetup.paymentMethodY, oPrintSetup.paymentMethodL, oPrintSetup.paymentMethodW, oPrintSetup.paymentMethodFS, oPrintSetup.DateX, oPrintSetup.DateY, oPrintSetup.DateL, oPrintSetup.DateW, oPrintSetup.DateFS, oPrintSetup.PayToX, oPrintSetup.PayToY, oPrintSetup.PayToL, oPrintSetup.PayToW, oPrintSetup.PayToFS, oPrintSetup.AmountWordX, oPrintSetup.AmountWordY, oPrintSetup.AmountWordL, oPrintSetup.AmountWordW, oPrintSetup.AmountWordFS, oPrintSetup.AmountX, oPrintSetup.AmountY, oPrintSetup.AmountL, oPrintSetup.AmountW, oPrintSetup.AmountFS, oPrintSetup.TBookNoX, oPrintSetup.TBookNoY, oPrintSetup.TBookNoL, oPrintSetup.TBookNoW, oPrintSetup.TBookNoFS, oPrintSetup.TPaymentTypeX, oPrintSetup.TPaymentTypeY, oPrintSetup.TPaymentTypeL, oPrintSetup.TPaymentTypeW, oPrintSetup.TPaymentTypeFS, oPrintSetup.TDateX, oPrintSetup.TDateY, oPrintSetup.TDateL, oPrintSetup.TDateW, oPrintSetup.TDateFS, oPrintSetup.TPayToX, oPrintSetup.TPayToY, oPrintSetup.TPayToL, oPrintSetup.TPayToW, oPrintSetup.TPayToFS, oPrintSetup.TForNoteX, oPrintSetup.TForNoteY, oPrintSetup.TForNoteL, oPrintSetup.TForNoteW, oPrintSetup.TForNoteFS, oPrintSetup.TAmountX, oPrintSetup.TAmountY, oPrintSetup.TAmountL, oPrintSetup.TAmountW, oPrintSetup.TAmountFS, oPrintSetup.TVoucherNoX, oPrintSetup.TVoucherNoY, oPrintSetup.TVoucherNoL, oPrintSetup.TVoucherNoW, oPrintSetup.TVoucherNoFS, oPrintSetup.DateFormat, oPrintSetup.IsSplit, oPrintSetup.PrinterGraceWidth);
        }


        #endregion

        #region Update Function
        public static IDataReader Update(TransactionContext tc, PrintSetup oPrintSetup, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_Process_DocumentManagement]"
                                    + "%n,  %n",
                                    oPrintSetup.BankID, nUserId);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PrintSetup WHERE DLID=%n", nID);
        }
        public static IDataReader GetsByBook(TransactionContext tc, int nBookID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PrintSetup WHERE DocumentBookID=%n AND DocumentStatus = 2 ", nBookID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PrintSetup WHERE DocumentStatus > 2 Order by DocumentStatus DESC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

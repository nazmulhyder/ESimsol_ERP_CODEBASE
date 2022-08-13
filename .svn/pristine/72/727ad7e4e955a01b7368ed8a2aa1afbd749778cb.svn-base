using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ChequeSetupDA
    {
        public ChequeSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ChequeSetup oChequeSetup, EnumDBOperation eEnumDBOperation,int nCurrentUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ChequeSetup]" + "%n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %b, %n, %b, %n, %n, %n",
                                                            oChequeSetup.ChequeSetupID,
                                                            oChequeSetup.ChequeSetupName,
                                                            oChequeSetup.Length,
                                                            oChequeSetup.Width,
                                                            oChequeSetup.PaymentMethodX,
                                                            oChequeSetup.paymentMethodY,
                                                            oChequeSetup.paymentMethodL,
                                                            oChequeSetup.paymentMethodW,
                                                            oChequeSetup.paymentMethodFS,
                                                            oChequeSetup.DateX,
                                                            oChequeSetup.DateY,
                                                            oChequeSetup.DateL,
                                                            oChequeSetup.DateW,
                                                            oChequeSetup.DateFS,
                                                            oChequeSetup.PayToX,
                                                            oChequeSetup.PayToY,
                                                            oChequeSetup.PayToL,
                                                            oChequeSetup.PayToW,
                                                            oChequeSetup.PayToFS,
                                                            oChequeSetup.AmountWordX,
                                                            oChequeSetup.AmountWordY,
                                                            oChequeSetup.AmountWordL,
                                                            oChequeSetup.AmountWordW,
                                                            oChequeSetup.AmountWordFS,
                                                            oChequeSetup.AmountX,
                                                            oChequeSetup.AmountY,
                                                            oChequeSetup.AmountL,
                                                            oChequeSetup.AmountW,
                                                            oChequeSetup.AmountFS,
                                                            oChequeSetup.TBookNoX,
                                                            oChequeSetup.TBookNoY,
                                                            oChequeSetup.TBookNoL,
                                                            oChequeSetup.TBookNoW,
                                                            oChequeSetup.TBookNoFS,
                                                            oChequeSetup.TPaymentTypeX,
                                                            oChequeSetup.TPaymentTypeY,
                                                            oChequeSetup.TPaymentTypeL,
                                                            oChequeSetup.TPaymentTypeW,
                                                            oChequeSetup.TPaymentTypeFS,
                                                            oChequeSetup.TDateX,
                                                            oChequeSetup.TDateY,
                                                            oChequeSetup.TDateL,
                                                            oChequeSetup.TDateW,
                                                            oChequeSetup.TDateFS,
                                                            oChequeSetup.TPayToX,
                                                            oChequeSetup.TPayToY,
                                                            oChequeSetup.TPayToL,
                                                            oChequeSetup.TPayToW,
                                                            oChequeSetup.TPayToFS,
                                                            oChequeSetup.TForNoteX,
                                                            oChequeSetup.TForNoteY,
                                                            oChequeSetup.TForNoteL,
                                                            oChequeSetup.TForNoteW,
                                                            oChequeSetup.TForNoteFS,
                                                            oChequeSetup.TAmountX,
                                                            oChequeSetup.TAmountY,
                                                            oChequeSetup.TAmountL,
                                                            oChequeSetup.TAmountW,
                                                            oChequeSetup.TAmountFS,
                                                            oChequeSetup.TVoucherNoX,
                                                            oChequeSetup.TVoucherNoY,
                                                            oChequeSetup.TVoucherNoL,
                                                            oChequeSetup.TVoucherNoW,
                                                            oChequeSetup.TVoucherNoFS,
                                                            oChequeSetup.DateFormat,
                                                            oChequeSetup.IsSplit,
                                                            oChequeSetup.DateSpace,
                                                            oChequeSetup.Ischecked,
                                                            oChequeSetup.PrinterGraceWidth,                
                                                            (int)eEnumDBOperation,
                                                            nCurrentUserID);
        }

        public static void Delete(TransactionContext tc, ChequeSetup oChequeSetup, EnumDBOperation eEnumDBOperation, int nCurrentUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ChequeSetup]" + "%n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %b, %n, %b, %n, %n, %n",
                                                            oChequeSetup.ChequeSetupID,
                                                            oChequeSetup.ChequeSetupName,
                                                            oChequeSetup.Length,
                                                            oChequeSetup.Width,
                                                            oChequeSetup.PaymentMethodX,
                                                            oChequeSetup.paymentMethodY,
                                                            oChequeSetup.paymentMethodL,
                                                            oChequeSetup.paymentMethodW,
                                                            oChequeSetup.paymentMethodFS,
                                                            oChequeSetup.DateX,
                                                            oChequeSetup.DateY,
                                                            oChequeSetup.DateL,
                                                            oChequeSetup.DateW,
                                                            oChequeSetup.DateFS,
                                                            oChequeSetup.PayToX,
                                                            oChequeSetup.PayToY,
                                                            oChequeSetup.PayToL,
                                                            oChequeSetup.PayToW,
                                                            oChequeSetup.PayToFS,
                                                            oChequeSetup.AmountWordX,
                                                            oChequeSetup.AmountWordY,
                                                            oChequeSetup.AmountWordL,
                                                            oChequeSetup.AmountWordW,
                                                            oChequeSetup.AmountWordFS,
                                                            oChequeSetup.AmountX,
                                                            oChequeSetup.AmountY,
                                                            oChequeSetup.AmountL,
                                                            oChequeSetup.AmountW,
                                                            oChequeSetup.AmountFS,
                                                            oChequeSetup.TBookNoX,
                                                            oChequeSetup.TBookNoY,
                                                            oChequeSetup.TBookNoL,
                                                            oChequeSetup.TBookNoW,
                                                            oChequeSetup.TBookNoFS,
                                                            oChequeSetup.TPaymentTypeX,
                                                            oChequeSetup.TPaymentTypeY,
                                                            oChequeSetup.TPaymentTypeL,
                                                            oChequeSetup.TPaymentTypeW,
                                                            oChequeSetup.TPaymentTypeFS,
                                                            oChequeSetup.TDateX,
                                                            oChequeSetup.TDateY,
                                                            oChequeSetup.TDateL,
                                                            oChequeSetup.TDateW,
                                                            oChequeSetup.TDateFS,
                                                            oChequeSetup.TPayToX,
                                                            oChequeSetup.TPayToY,
                                                            oChequeSetup.TPayToL,
                                                            oChequeSetup.TPayToW,
                                                            oChequeSetup.TPayToFS,
                                                            oChequeSetup.TForNoteX,
                                                            oChequeSetup.TForNoteY,
                                                            oChequeSetup.TForNoteL,
                                                            oChequeSetup.TForNoteW,
                                                            oChequeSetup.TForNoteFS,
                                                            oChequeSetup.TAmountX,
                                                            oChequeSetup.TAmountY,
                                                            oChequeSetup.TAmountL,
                                                            oChequeSetup.TAmountW,
                                                            oChequeSetup.TAmountFS,
                                                            oChequeSetup.TVoucherNoX,
                                                            oChequeSetup.TVoucherNoY,
                                                            oChequeSetup.TVoucherNoL,
                                                            oChequeSetup.TVoucherNoW,
                                                            oChequeSetup.TVoucherNoFS,
                                                            oChequeSetup.DateFormat,
                                                            oChequeSetup.IsSplit,
                                                            oChequeSetup.DateSpace,
                                                            oChequeSetup.Ischecked,
                                                            oChequeSetup.PrinterGraceWidth,
                                                            (int)eEnumDBOperation,
                                                            nCurrentUserID);
        }
        #endregion

        #region Update Photo
        public static void UpdatePhoto(TransactionContext tc, ChequeSetup oChequeSetup)
        {

            SqlParameter Photo = new SqlParameter();
            Photo.SqlDbType = SqlDbType.Image;
            Photo.ParameterName = "Photopic";
            Photo.Value = oChequeSetup.ChequeImageInByte;

            string sSQL = SQLParser.MakeSQL("UPDATE ChequeSetup SET ChequeImageInByte=%q,ChequeImageSize=%n"
                + " WHERE ChequeSetupID=%n", "@Photopic", oChequeSetup.ChequeImageInByte.Length, oChequeSetup.ChequeSetupID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ChequeSetup WHERE ChequeSetupID=%n", nID);
        }
        public static IDataReader GetsWithoutImage(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeSetup");
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ChequeSetup");
        }
        public static IDataReader GetsByName(TransactionContext tc, string sName)
        {
            return tc.ExecuteReader("SELECT * FROM ChequeSetup WHERE ChequeSetupName LIKE '%" + sName + "%' ORDER BY ChequeSetupName ASC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}

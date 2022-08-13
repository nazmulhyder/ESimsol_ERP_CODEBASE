using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PaymentAccountDA
    {
        public PaymentAccountDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PaymentAccount oPaymentAccount, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PaymentAccount]" + "%n, %n, %n, %b, %n, %n",
                                    oPaymentAccount.PaymentAccountID, oPaymentAccount.BUID, oPaymentAccount.AccountHeadID, oPaymentAccount.IsDefault, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, PaymentAccount oPaymentAccount, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PaymentAccount]" + "%n, %n, %n, %b, %n, %n",
                                    oPaymentAccount.PaymentAccountID, oPaymentAccount.BUID, oPaymentAccount.AccountHeadID, oPaymentAccount.IsDefault, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PaymentAccount WHERE PaymentAccountID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PaymentAccount");
        }
        public static IDataReader GetsByBU(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PaymentAccount WHERE BUID=%n", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void SetDefault(TransactionContext tc, PaymentAccount oPaymentAccount)
        {
            tc.ExecuteNonQuery("UPDATE PaymentAccount SET IsDefault=0 WHERE BUID=%n", oPaymentAccount.BUID);
            tc.ExecuteNonQuery("UPDATE PaymentAccount SET IsDefault=1 WHERE PaymentAccountID=%n", oPaymentAccount.PaymentAccountID);
        }
        #endregion
    }
}

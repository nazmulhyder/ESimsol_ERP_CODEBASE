using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class PaymentTermDA
    {
        public PaymentTermDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PaymentTerm oPaymentTerm, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PaymentTerm]"
                                    + "%n, %n, %n, %s, %n, %n, %n,%s,%n,%s,%n,%n",
                                    oPaymentTerm.PaymentTermID,oPaymentTerm.BUID,  oPaymentTerm.Percentage, oPaymentTerm.TermText, oPaymentTerm.DayApplyTypeint,oPaymentTerm.Days,oPaymentTerm.DateDisplayPartint,oPaymentTerm.DateText,oPaymentTerm.PaymentTermTypeInt, oPaymentTerm.EndNote, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, PaymentTerm oPaymentTerm, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PaymentTerm]"
                                    + "%n, %n, %n, %s, %n, %n, %n,%s,%n,%s,%n,%n",
                                    oPaymentTerm.PaymentTermID, oPaymentTerm.BUID, oPaymentTerm.Percentage, oPaymentTerm.TermText, oPaymentTerm.DayApplyTypeint, oPaymentTerm.Days, oPaymentTerm.DateDisplayPartint, oPaymentTerm.DateText, oPaymentTerm.PaymentTermTypeInt, oPaymentTerm.EndNote, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM PaymentTerm WHERE PaymentTermID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PaymentTerm");
        }
        public static IDataReader GetsByBU(int nBUID,  TransactionContext tc)
        { 
            return tc.ExecuteReader("SELECT * FROM PaymentTerm WHERE BUID = %n",nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
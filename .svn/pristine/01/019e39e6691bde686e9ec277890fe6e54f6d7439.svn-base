using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{


    public class VoucherReferenceDA
    {
        public VoucherReferenceDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VoucherReference oVoucherReference, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VoucherReference]" + "%n, %n, %n, %n, %s,%d,%n,%s,%n,%n",
                                    oVoucherReference.VoucherReferenceID, oVoucherReference.VoucherDetailID, oVoucherReference.AccountHeadID, oVoucherReference.Amount, oVoucherReference.Description, oVoucherReference.TransactionDate,oVoucherReference.CurrencyID,oVoucherReference.CurrencyConversionRate, oVoucherReference.UserID, (int)eEnumDBOperation);
        }


        public static void Delete(TransactionContext tc, VoucherReference oVoucherReference, EnumDBOperation eEnumDBOperation)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VoucherReference]" + "%n, %n, %n, %n, %s,%d,%n,%s,%n,%n",
                                    oVoucherReference.VoucherReferenceID, oVoucherReference.VoucherDetailID, oVoucherReference.AccountHeadID, oVoucherReference.Amount, oVoucherReference.Description, oVoucherReference.TransactionDate, oVoucherReference.CurrencyID, oVoucherReference.CurrencyConversionRate, oVoucherReference.UserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherReference WHERE CCID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherReference");
        }

        public static IDataReader Gets(TransactionContext tc, int nVoucherDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherReference WHERE VoucherDetailID=%n", nVoucherDetailID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nVoucherlID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherReference WHERE VoucherDetailID in (Select VoucherDetail.VoucherDetailID from VoucherDetail where VoucherID=%n)", nVoucherlID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
       
        #endregion
    }  

   
}

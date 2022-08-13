using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{


    public class VOReferenceDA
    {
        public VOReferenceDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VOReference oVOReference, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VOReference]" + "%n, %n, %n, %d, %s, %b, %n, %n, %n, %n, %n, %n, %n",
                                    oVOReference.VOReferenceID, oVOReference.VoucherDetailID, oVOReference.OrderID, oVOReference.TransactionDate, oVOReference.Remarks, oVOReference.IsDebit, oVOReference.CurrencyID, oVOReference.ConversionRate, oVOReference.AmountInCurrency, oVOReference.Amount, oVOReference.CCTID, nUserID, (int)eEnumDBOperation);
        }


        public static void Delete(TransactionContext tc, VOReference oVOReference, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VOReference]" + "%n, %n, %n, %d, %s, %b, %n, %n, %n, %n, %n, %n, %n",
                                    oVOReference.VOReferenceID, oVOReference.VoucherDetailID, oVOReference.OrderID, oVOReference.TransactionDate, oVOReference.Remarks, oVOReference.IsDebit, oVOReference.CurrencyID, oVOReference.ConversionRate, oVOReference.AmountInCurrency, oVOReference.Amount, oVOReference.CCTID, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VOReference WHERE VOReferenceID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VOReference");
        }

        public static IDataReader Gets(TransactionContext tc, int nVoucherDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VOReference WHERE VoucherDetailID=%n", nVoucherDetailID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nVoucherlID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VOReference WHERE VoucherID=%n", nVoucherlID);
        }
        public static IDataReader GetsByOrder(TransactionContext tc, int nVOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VOReference WHERE OrderID=%n ORDER BY AccountHeadID ASC", nVOrderID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
       
        #endregion
    }  

   
}

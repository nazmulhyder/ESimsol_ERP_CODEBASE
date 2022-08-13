using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{


    public class VoucherBillTransactionDA
    {
        public VoucherBillTransactionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VoucherBillTransaction oVBT, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VoucherBillTransaction]"
                    + "%n, %n, %n, %n, %n, %n, %b, %d, %n, %n, %n, %n",
                    oVBT.VoucherBillTransactionID, oVBT.VoucherBillID, oVBT.VoucherDetailID, oVBT.CCTID, oVBT.TrType, oVBT.Amount, oVBT.IsDr, oVBT.TransactionDate, oVBT.CurrencyID, oVBT.ConversionRate, nUserId, (int)eEnumDBOperation);
        }


        public static void Delete(TransactionContext tc, VoucherBillTransaction oVBT, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VoucherBillTransaction]"
                    + "%n, %n, %n, %n, %n, %n, %b, %d, %n, %n, %n, %n",
                    oVBT.VoucherBillTransactionID, oVBT.VoucherBillID, oVBT.VoucherDetailID, oVBT.CCTID, oVBT.TrType, oVBT.Amount, oVBT.IsDr, oVBT.TransactionDate, oVBT.CurrencyID, oVBT.ConversionRate, nUserId, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBillTransaction WHERE VoucherBillTransactionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBillTransaction");
        }

        public static IDataReader Gets(TransactionContext tc, int nVoucherDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBillTransaction WHERE VoucherDetailID=%n", nVoucherDetailID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nVoucherID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBillTransaction WHERE VoucherDetailID in (Select VoucherDetail.VoucherDetailID from VoucherDetail where VoucherID=%n)", nVoucherID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  

   
}

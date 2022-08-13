using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VoucherChequeDA
    {
        public VoucherChequeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VoucherCheque oVoucherCheque, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VoucherCheque]"
                                    + "%n, %n, %n, %n, %n, %d, %n, %n, %n",
                                    oVoucherCheque.VoucherChequeID, oVoucherCheque.VoucherDetailID, oVoucherCheque.CCTID, (int)oVoucherCheque.ChequeType, oVoucherCheque.ChequeID, oVoucherCheque.TransactionDate, oVoucherCheque.Amount, nUserId, (int)eEnumDBOperation);
        }


        public static void Delete(TransactionContext tc, VoucherCheque oVoucherCheque, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VoucherCheque]"
                                + "%n, %n, %n, %n, %n, %d, %n, %n, %n",
                                oVoucherCheque.VoucherChequeID, oVoucherCheque.VoucherDetailID, oVoucherCheque.CCTID, (int)oVoucherCheque.ChequeType, oVoucherCheque.ChequeID, oVoucherCheque.TransactionDate, oVoucherCheque.Amount, nUserId, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherCheque WHERE VoucherChequeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherCheque");
        }

        public static IDataReader Gets(TransactionContext tc, int nVoucherDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherCheque WHERE VoucherDetailID=%n", nVoucherDetailID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nVoucherID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherCheque WHERE VoucherDetailID in (Select VoucherDetail.VoucherDetailID from VoucherDetail where VoucherID=%n)", nVoucherID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

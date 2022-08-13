using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{


    public class VPTransactionDA
    {
        public VPTransactionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VPTransaction oVPT, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VPTransaction]" + "%n, %n, %n, %n, %n, %n,%n,%n, %b, %D, %n, %n, %s, %n, %n",
                      oVPT.VPTransactionID, oVPT.VoucherDetailID, oVPT.ProductID, oVPT.WorkingUnitID, oVPT.Qty, oVPT.UnitPrice, oVPT.Amount, oVPT.MUnitID, oVPT.IsDr, oVPT.TransactionDate, oVPT.CurrencyID, oVPT.ConversionRate, oVPT.Description, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, VPTransaction oVPT, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VPTransaction]" + "%n, %n, %n, %n, %n, %n,%n,%n, %b, %D, %n, %n, %s, %n, %n",
                      oVPT.VPTransactionID, oVPT.VoucherDetailID, oVPT.ProductID, oVPT.WorkingUnitID, oVPT.Qty, oVPT.UnitPrice, oVPT.Amount, oVPT.MUnitID, oVPT.IsDr, oVPT.TransactionDate, oVPT.CurrencyID, oVPT.ConversionRate, oVPT.Description, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VPTransaction WHERE CCID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VPTransaction");
        }

        public static IDataReader Gets(TransactionContext tc, int nVoucherDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VPTransaction WHERE VoucherDetailID=%n", nVoucherDetailID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nVoucherID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VPTransaction WHERE VoucherDetailID in (Select VoucherDetail.VoucherDetailID from VoucherDetail where VoucherID=%n)", nVoucherID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


      
        #endregion
    }  

   
}

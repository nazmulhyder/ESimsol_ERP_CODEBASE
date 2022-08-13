using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class VoucherCodeDA
    {
        public VoucherCodeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VoucherCode oVoucherCode, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("SP_IUD_VoucherCode]" + "%n, %n, %n, %s, %n, %n, %n, %n, %n",
                                    oVoucherCode.VoucherCodeID, oVoucherCode.VoucherTypeID, oVoucherCode.VoucherCodeType, oVoucherCode.Value, oVoucherCode.Length, oVoucherCode.Restart, oVoucherCode.Sequence,  (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, VoucherCode oVoucherCode, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("SP_IUD_VoucherCode]" + "%n, %n, %n, %s, %n, %n, %n, %n, %n",
                                    oVoucherCode.VoucherCodeID, oVoucherCode.VoucherTypeID, oVoucherCode.VoucherCodeType, oVoucherCode.Value, oVoucherCode.Length, oVoucherCode.Restart, oVoucherCode.Sequence, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM VoucherCode WHERE VoucherCodeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VoucherCode");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

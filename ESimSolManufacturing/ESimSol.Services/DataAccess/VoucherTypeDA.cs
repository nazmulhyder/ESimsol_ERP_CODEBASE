using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VoucherTypeDA
    {
        public VoucherTypeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VoucherType oVoucherType, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VoucherType]"
                                    + "%n, %s, %n, %n, %n, %b, %b, %b, %b, %b, %s, %n, %n",
                                    oVoucherType.VoucherTypeID, oVoucherType.VoucherName, (int)oVoucherType.VoucherCategory, (int)oVoucherType.NumberMethod, oVoucherType.NumberConfigureID, oVoucherType.MustNarrationEntry, oVoucherType.PrintAfterSave, oVoucherType.IsProductRequired, oVoucherType.IsDepartmentRequired, oVoucherType.IsPaymentCheque, oVoucherType.VoucherCodesInString, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, VoucherType oVoucherType, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VoucherType]"
                                    + "%n, %s, %n, %n, %n, %b, %b, %b, %b, %b, %s, %n, %n",
                                    oVoucherType.VoucherTypeID, oVoucherType.VoucherName, (int)oVoucherType.VoucherCategory, (int)oVoucherType.NumberMethod, oVoucherType.NumberConfigureID, oVoucherType.MustNarrationEntry, oVoucherType.PrintAfterSave, oVoucherType.IsProductRequired, oVoucherType.IsDepartmentRequired, oVoucherType.IsPaymentCheque, oVoucherType.VoucherCodesInString, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherType WHERE VoucherTypeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherType");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

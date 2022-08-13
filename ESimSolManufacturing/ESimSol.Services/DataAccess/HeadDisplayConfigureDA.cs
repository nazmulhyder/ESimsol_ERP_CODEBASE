using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class HeadDisplayConfigureDA
    {
        public HeadDisplayConfigureDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, HeadDisplayConfigure oHeadDisplayConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_HeadDisplayConfigure]"
                                    + "%n, %n, %n,%b, %n, %n",
                                    oHeadDisplayConfigure.HeadDisplayConfigureID, oHeadDisplayConfigure.VoucherTypeID, oHeadDisplayConfigure.SubGroupID, oHeadDisplayConfigure.IsDebit, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, HeadDisplayConfigure oHeadDisplayConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_HeadDisplayConfigure]"
                                    + "%n, %n, %n,%b, %n, %n",
                                    oHeadDisplayConfigure.HeadDisplayConfigureID, oHeadDisplayConfigure.VoucherTypeID, oHeadDisplayConfigure.SubGroupID, oHeadDisplayConfigure.IsDebit, nUserId, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_HeadDisplayConfigure WHERE HeadDisplayConfigureID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_HeadDisplayConfigure");
        }
        public static IDataReader Gets(TransactionContext tc, int nVoucherTypeID)
        {
            return tc.ExecuteReader("SELECT * FROM View_HeadDisplayConfigure WHERE VoucherTypeID=%n",nVoucherTypeID);
        }
        public static IDataReader GetTopConfigure(TransactionContext tc, ChartsOfAccount oChartsOfAccount)
        {
            return tc.ExecuteReader("SELECT TOP 1 HH.HeadDisplayConfigureID FROM View_HeadDisplayConfigure AS HH WHERE VoucherTypeID=%n AND IsDebit=%b", oChartsOfAccount.VoucherTypeID, oChartsOfAccount.IsDebit);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}

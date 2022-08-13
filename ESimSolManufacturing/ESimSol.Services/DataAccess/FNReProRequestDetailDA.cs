using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class FNReProRequestDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FNReProRequestDetail oFNReProRequestDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFNReProRequestDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNReProRequestDetail]"
                                   + "%n, %n, %n, %n, %s, %n,%n,%s",
                                   oFNReProRequestDetail.FNReProRequestDetailID, oFNReProRequestDetail.FNReProRequestID, oFNReProRequestDetail.FNBatchCardID, oFNReProRequestDetail.Qty, oFNReProRequestDetail.Note, nUserID, (int)eEnumDBOperation, sFNReProRequestDetailIDs);
        }

        public static void Delete(TransactionContext tc, FNReProRequestDetail oFNReProRequestDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFNReProRequestDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNReProRequestDetail]"
                                   + "%n, %n, %n, %n, %s, %n,%n,%s",
                                   oFNReProRequestDetail.FNReProRequestDetailID, oFNReProRequestDetail.FNReProRequestID, oFNReProRequestDetail.FNBatchCardID, oFNReProRequestDetail.Qty, oFNReProRequestDetail.Note, nUserID, (int)eEnumDBOperation, sFNReProRequestDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNReProRequestDetail WHERE FNReProRequestDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFNReProRequestID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNReProRequestDetail WHERE FNReProRequestID =%n", nFNReProRequestID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}

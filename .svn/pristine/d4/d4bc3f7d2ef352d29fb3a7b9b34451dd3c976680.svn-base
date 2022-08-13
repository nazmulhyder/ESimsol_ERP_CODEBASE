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
    public class FNReProRequestDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FNReProRequest oFNReProRequest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNReProRequest]"
                                   + "%n, %s, %n, %d, %n,   %d, %n, %s, %s, %n,%n",
                                   oFNReProRequest.FNReProRequestID, oFNReProRequest.ReqNo, oFNReProRequest.RequestByID, oFNReProRequest.RequestDate, oFNReProRequest.ApproveBy, 
                                   oFNReProRequest.ApproveDate, oFNReProRequest.Status, oFNReProRequest.Note, oFNReProRequest.Note_Approve, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FNReProRequest oFNReProRequest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNReProRequest]"
                                   + "%n, %s, %n, %d, %n,   %d, %n, %s, %s, %n,%n",
                                   oFNReProRequest.FNReProRequestID, oFNReProRequest.ReqNo, oFNReProRequest.RequestByID, oFNReProRequest.RequestDate, oFNReProRequest.ApproveBy,
                                   oFNReProRequest.ApproveDate, oFNReProRequest.Status, oFNReProRequest.Note, oFNReProRequest.Note_Approve, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNReProRequest WHERE FNReProRequestID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNReProRequest ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}

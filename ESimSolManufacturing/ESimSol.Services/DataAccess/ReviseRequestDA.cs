using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{

    public class ReviseRequestDA
    {
        public ReviseRequestDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ReviseRequest oReviseRequest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ReviseRequest]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n", oReviseRequest.ReviseRequestID, (int)oReviseRequest.OperationType,oReviseRequest.OperationObjectID,oReviseRequest.RequestBy, oReviseRequest.RequestTo, oReviseRequest.Note, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ReviseRequest oReviseRequest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ReviseRequest]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n", oReviseRequest.ReviseRequestID, (int)oReviseRequest.OperationType, oReviseRequest.OperationObjectID, oReviseRequest.RequestBy, oReviseRequest.RequestTo, oReviseRequest.Note, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ReviseRequest WHERE ReviseRequestID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ReviseRequest");
        }



        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
  
}

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


    public class ApprovalRequestDA
    {
        public ApprovalRequestDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ApprovalRequest oApprovalRequest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ApprovalRequest]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n", oApprovalRequest.ApprovalRequestID, (int)oApprovalRequest.OperationType,oApprovalRequest.OperationObjectID,oApprovalRequest.RequestBy, oApprovalRequest.RequestTo, oApprovalRequest.Note, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ApprovalRequest oApprovalRequest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ApprovalRequest]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n", oApprovalRequest.ApprovalRequestID, (int)oApprovalRequest.OperationType, oApprovalRequest.OperationObjectID, oApprovalRequest.RequestBy, oApprovalRequest.RequestTo, oApprovalRequest.Note, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ApprovalRequest WHERE ApprovalRequestID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ApprovalRequest");
        }



        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
  
}

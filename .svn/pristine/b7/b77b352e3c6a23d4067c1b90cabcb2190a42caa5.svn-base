using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class ApprovalHistoryDA
    {
        public ApprovalHistoryDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ApprovalHistory oApprovalHistory, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_ApprovalHistory]" + "%n, %n, %n, %n, %s, %n, %n"
                , oApprovalHistory.ApprovalHistoryID
                , oApprovalHistory.ObjectRefID
                , oApprovalHistory.ApprovalHeadID
                , oApprovalHistory.SendToPersonID
                , oApprovalHistory.Note
                , nUserID
                , nDBOperation
            );
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}



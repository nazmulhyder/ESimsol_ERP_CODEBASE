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
    public class ApprovalHeadDA
    {
        public ApprovalHeadDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ApprovalHead oApprovalHead, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ApprovalHead]" + "%n, %n, %s, %n,%n, %s, %n, %n"
                ,oApprovalHead.ApprovalHeadID
                ,oApprovalHead.ModuleID
                , oApprovalHead.Name
                , oApprovalHead.Sequence
                , oApprovalHead.BUID
                , oApprovalHead.RefColName
                , nDBOperation
                , nUserID
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



        public static IDataReader UpDown(TransactionContext tc, ApprovalHead oApprovalHead, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_ApprovalHeadUpDown]" + "%n, %n,%n, %n"
                , oApprovalHead.ModuleID
                , oApprovalHead.ApprovalHeadID
                , oApprovalHead.BUID
                , oApprovalHead.IsUp
            );
        }

        #endregion
    }
}


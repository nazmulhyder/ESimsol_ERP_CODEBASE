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
    public class ApprovalHeadPersonDA
    {
        public ApprovalHeadPersonDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ApprovalHeadPerson oApprovalHeadPerson, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ApprovalHeadPerson]" + "%n, %n, %n, %s, %n, %n, %b",
                oApprovalHeadPerson.ApprovalHeadPersonID
                ,oApprovalHeadPerson.ApprovalHeadID
                ,oApprovalHeadPerson.UserID
                ,""
                ,nDBOperation
                ,nUserID
                ,oApprovalHeadPerson.IsActive
             );
        }


        public static void Delete(TransactionContext tc, ApprovalHeadPerson oApprovalHeadPerson, Int64 nUserID, int nDBOperation)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ApprovalHeadPerson]" + "%n, %n, %n, %s, %n, %n, %b",
                oApprovalHeadPerson.ApprovalHeadPersonID
                , oApprovalHeadPerson.ApprovalHeadID
                , oApprovalHeadPerson.UserID
                , ""
                , nDBOperation
                , nUserID
                , oApprovalHeadPerson.IsActive
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
        public static IDataReader Gets(int nApprovalHeadID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ApprovalHeadPerson WHERE ApprovalHeadID=%n", nApprovalHeadID);
        }
        public static IDataReader Activate(TransactionContext tc, ApprovalHeadPerson oApprovalHeadPerson)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ApprovalHeadPerson Set IsActive=~IsActive WHERE ApprovalHeadPersonID=%n", oApprovalHeadPerson.ApprovalHeadPersonID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ApprovalHeadPerson WHERE ApprovalHeadPersonID=%n", oApprovalHeadPerson.ApprovalHeadPersonID);
        }
        #endregion
    }
}



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
    public class CandidateUserDA
    {
        public CandidateUserDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, CandidateUser oCandidateUser, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CandidateUser] %n, %s, %s, %s, %n, %n",
                   oCandidateUser.UserID, oCandidateUser.LogInID,oCandidateUser.UserName,oCandidateUser.Password
                   , oCandidateUser.CandidateID, nDBOperation);
                   
        }
        public static void ChangePassword(TransactionContext tc, CandidateUser oCandidateUser)
        {
            tc.ExecuteNonQuery(" UPDATE CandidateUser SET [Password]=%s WHERE UserID=%n", oCandidateUser.Password, oCandidateUser.UserID);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int UserID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CandidateUser WHERE UserID=%n", UserID);
        }
        public static IDataReader GetForLogIn( TransactionContext tc,string sLoginID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CandidateUser WHERE LogInID=%s", sLoginID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CandidateUser");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}

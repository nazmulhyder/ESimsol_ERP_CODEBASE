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
    public class PerformanceIncentiveMemberDA
    {
        public PerformanceIncentiveMemberDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, PerformanceIncentiveMember oPerformanceIncentiveMember, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PerformanceIncentiveMember] %n,%n,%n,%n,%n,%n",
                   oPerformanceIncentiveMember.PIMemberID, oPerformanceIncentiveMember.PIID,
                   oPerformanceIncentiveMember.EmployeeID, oPerformanceIncentiveMember.Rate,
                   nUserID, nDBOperation);
        }

        public static IDataReader Upload_PIMember(TransactionContext tc, PerformanceIncentiveMember oPerformanceIncentiveMember, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Upload_PerformanceIncentiveMember] %s,%s,%n,%n",
                   oPerformanceIncentiveMember.EmployeeCode,
                   oPerformanceIncentiveMember.PICode,
                   oPerformanceIncentiveMember.Rate,
                   nUserID);
        }

        public static IDataReader InActive(int nPerformanceIncentiveMemberID, Int64 nUserId, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE PerformanceIncentiveMember SET InactiveBy=%n,InactiveByDate=%d WHERE PIMemberID=%n;SELECT * FROM View_PerformanceIncentiveMember WHERE PIMemberID=%n", nUserId, DateTime.Now, nPerformanceIncentiveMemberID, nPerformanceIncentiveMemberID);
        }

        public static IDataReader Approve(string sPMIIDs, Int64 nUserId, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE PerformanceIncentiveMember SET ApproveBy=%n,ApproveByDate=%d WHERE PIMemberID IN(" + sPMIIDs + ");SELECT * FROM View_PerformanceIncentiveMember WHERE PIMemberID IN("+sPMIIDs+")", nUserId, DateTime.Now);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nPerformanceIncentiveMemberID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PerformanceIncentiveMember WHERE PIID=%n", nPerformanceIncentiveMemberID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PerformanceIncentiveMember");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

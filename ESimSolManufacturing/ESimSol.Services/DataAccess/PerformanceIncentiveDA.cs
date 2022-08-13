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
    public class PerformanceIncentiveDA
    {
        public PerformanceIncentiveDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, PerformanceIncentive oPerformanceIncentive, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PerformanceIncentive] %n,%s,%s,%n,%n,%n",
                   oPerformanceIncentive.PIID,
                   oPerformanceIncentive.Name,
                   oPerformanceIncentive.Description,
                   oPerformanceIncentive.SalaryHeadID,
                   nUserID, nDBOperation);
        }

        public static IDataReader InActive(int nPerformanceIncentiveID,Int64 nUserId, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE PerformanceIncentive SET InactiveBy=%n,InActiveDate=%d WHERE PIID=%n;SELECT * FROM View_PerformanceIncentive WHERE PIID=%n", nUserId, DateTime.Now, nPerformanceIncentiveID, nPerformanceIncentiveID);
        }

        public static IDataReader Approve(int nPerformanceIncentiveID, Int64 nUserId, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE PerformanceIncentive SET ApproveBy=%n,ApproveDate=%d WHERE PIID=%n;SELECT * FROM View_PerformanceIncentive WHERE PIID=%n", nUserId, DateTime.Now, nPerformanceIncentiveID, nPerformanceIncentiveID);
        }
        public static IDataReader PerformanceIncentive_Transfer(int PreviousPIID, int PresentPIID, Int64 nUserID,TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Transfer_PIMembers] %n,%n,%n",
                   PreviousPIID,PresentPIID,nUserID);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nPerformanceIncentiveID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PerformanceIncentive WHERE PIID=%n", nPerformanceIncentiveID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PerformanceIncentive");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

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
    public class PerformanceIncentiveEvaluationDA
    {
        public PerformanceIncentiveEvaluationDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, PerformanceIncentiveEvaluation oPerformanceIncentiveEvaluation, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PerformanceIncentiveEvaluation] %n,%s,%s,%n,%n,%n",
                   oPerformanceIncentiveEvaluation.PIEvaluationID,

                   nUserID, nDBOperation);
        }

        public static IDataReader Upload_PIEvaluation(TransactionContext tc, PerformanceIncentiveEvaluation oPerformanceIncentiveEvaluation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Upload_PerformanceIncentiveEvaluation] %s,%n,%n,%n,%n",
                   oPerformanceIncentiveEvaluation.EmployeeCode,
                   oPerformanceIncentiveEvaluation.Year,
                   (int)oPerformanceIncentiveEvaluation.MonthID,
                   oPerformanceIncentiveEvaluation.Point,
                   nUserID);
        }

        public static IDataReader Approve(string sPIEIDs, Int64 nUserId, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE PerformanceIncentiveEvaluation SET ApproveBy=%n,ApproveByDate=%d WHERE PIEvaluationID IN(" + sPIEIDs + ");SELECT * FROM View_PerformanceIncentiveEvaluation WHERE PIEvaluationID IN(" + sPIEIDs + ")", nUserId, DateTime.Now);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nPerformanceIncentiveEvaluationID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PerformanceIncentiveEvaluation WHERE PIEvaluationID=%n", nPerformanceIncentiveEvaluationID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PerformanceIncentiveEvaluation");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

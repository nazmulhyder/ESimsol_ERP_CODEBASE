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
    public class EmployeeSettlementDA
    {
        public EmployeeSettlementDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeSettlement oEmployeeSettlement, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeSettlement] %n,%n,%s,%d,%d,%n,%b,%b,%n,%n",
                   oEmployeeSettlement.EmployeeSettlementID,
                   oEmployeeSettlement.EmployeeID,
                   oEmployeeSettlement.Reason,
                   oEmployeeSettlement.SubmissionDate,
                   oEmployeeSettlement.EffectDate,
                   oEmployeeSettlement.SettlementType,
                   oEmployeeSettlement.IsNoticePayDeduction,
                    oEmployeeSettlement.IsSalaryHold,
                   nUserID, nDBOperation);

        }

        //public static IDataReader Approve(int nEmployeeSettlementID, Int64 nUserId, TransactionContext tc)
        //{
        //    return tc.ExecuteReader("UPDATE EmployeeSettlement SET ApproveBy=%n,ApproveByDate=%d WHERE EmployeeSettlementID=%n;SELECT * FROM View_EmployeeSettlement WHERE EmployeeSettlementID=%n", nUserId, DateTime.Now, nEmployeeSettlementID, nEmployeeSettlementID);
        //}

        public static IDataReader GetHierarchy(TransactionContext tc, string sEmployeeIDs)
        {
            return tc.ExecuteReader("EXEC [SP_GetHierarchyList]" + "%s,%s,%s,%b,%b,%s,%b", "EmployeeReportingPerson", "RPID", "EmployeeID", 1, 1, sEmployeeIDs, 1);
        }
        public static IDataReader PaymentDone(int nEmployeeSettlementID, Int64 nUserId, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE EmployeeSettlement SET PaymentDate=%d WHERE EmployeeSettlementID=%n;SELECT * FROM View_EmployeeSettlement WHERE EmployeeSettlementID=%n", DateTime.Now, nEmployeeSettlementID, nEmployeeSettlementID);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nEmployeeSettlementID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSettlement WHERE EmployeeSettlementID=%n", nEmployeeSettlementID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSettlement");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

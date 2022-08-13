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
    public class LeaveLedgerReportDA
    {
        public LeaveLedgerReportDA() { }

        #region Get

        public static IDataReader Gets(string sEmployeeIDs, string sDepartmentIds, string sDesignationIds, int ACSID, int nLeaveHeadID, double nBalanceAmount, int nBalanceType, bool bReportingPerson, DateTime dtFrom, DateTime dtTo, bool bDate, bool IsActive, bool IsInActive, string sBUnit, string sLocationID, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_LeaveLedger] %s,%s,%s,%n,%n,%n,%n,%b,%d,%d,%b,%b,%b,%s,%s, %n",
                  sEmployeeIDs, sDepartmentIds, sDesignationIds, ACSID, nLeaveHeadID, nBalanceAmount, nBalanceType, bReportingPerson, dtFrom, dtTo, bDate, IsActive, IsInActive, sBUnit, sLocationID, nUserID);
        }
        public static IDataReader GetsComp(string sEmployeeIDs, string sDepartmentIds, string sDesignationIds, int ACSID, int nLeaveHeadID, double nBalanceAmount, int nBalanceType, bool bReportingPerson, DateTime dtFrom, DateTime dtTo, bool bDate, bool IsActive, bool IsInActive, string sBUnit, string sLocationID, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_CompLeaveLedger] %s,%s,%s,%n,%n,%n,%n,%b,%d,%d,%b,%b,%b,%s,%s, %n",
                  sEmployeeIDs, sDepartmentIds, sDesignationIds, ACSID, nLeaveHeadID, nBalanceAmount, nBalanceType, bReportingPerson, dtFrom, dtTo, bDate, IsActive, IsInActive, sBUnit, sLocationID, nUserID);
        }

        #endregion
        public static IDataReader GetLeaveWithEnjoyBalance(TransactionContext tc, string sBUIDs, string sLocIDs, string sDeptIDs, string sDesgIDs, string sEmployeeIDs, DateTime sStartDate, DateTime sEndDate, int nApplicationNature, int nLeaveHeadId, int nLeaveType, int nLeaveStatus, int nIsPaid, int nIsUnPaid, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_LeaveApplication] %s, %s, %s, %s, %s, %s, %s, %n, %n, %n, %n, %n, %n, %n",
                                   sBUIDs, sLocIDs, sDeptIDs, sDesgIDs, sEmployeeIDs, sStartDate.ToString("dd MMM yyyy"), sEndDate.ToString("dd MMM yyyy"),
                                    nApplicationNature, nLeaveHeadId, nLeaveType, nLeaveStatus, nIsPaid, nIsUnPaid, nUserID);
        }
    }
}

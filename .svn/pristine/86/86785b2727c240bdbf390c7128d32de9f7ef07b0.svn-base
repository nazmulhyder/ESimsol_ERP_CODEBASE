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
    public class RosterPlanEmployeeV2DA
    {


       
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static int GetTotalCount(TransactionContext tc, string sSql, Int64 nUserID)
        {
            int nTotalCount = 0;
            object oIndex = tc.ExecuteScalar(sSql);
            if (oIndex != null)
            {
                nTotalCount = Convert.ToInt32(oIndex);
            }
            return nTotalCount;
        }

        public static IDataReader UpdateRosterPlanEmployee(TransactionContext tc, RosterPlanEmployeeV2 oRosterPlanEmployeeV2, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_UpdateRosterPlanEmployee] %n,%n,%D,%n,%n,%n,%n,%b,%b,%n",
                   oRosterPlanEmployeeV2.RPEID,
                   oRosterPlanEmployeeV2.EmployeeID,
                   oRosterPlanEmployeeV2.AttendanceDate,
                   oRosterPlanEmployeeV2.BusinessUnitID,
                   oRosterPlanEmployeeV2.LocationID,
                   oRosterPlanEmployeeV2.DepartmentID,
                   oRosterPlanEmployeeV2.ShiftID,
                   oRosterPlanEmployeeV2.IsDayOff,
                   oRosterPlanEmployeeV2.IsHoliday,
                   nUserID
                   );
        }

        public static IDataReader CommitRosterPlanEmployee(TransactionContext tc, RosterPlanEmployeeV2 oRosterPlanEmployeeV2, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CommitRosterPlanEmployee]"
                                    + "%s,%n,%n,%n,%n,%n,%s,%d,%d,%b,%b,%b,%n",
                                  oRosterPlanEmployeeV2.EmployeeIDs, oRosterPlanEmployeeV2.EmployeeBatchID, oRosterPlanEmployeeV2.BusinessUnitID, oRosterPlanEmployeeV2.LocationID, oRosterPlanEmployeeV2.DepartmentID,oRosterPlanEmployeeV2.ShiftID, oRosterPlanEmployeeV2.Remarks,oRosterPlanEmployeeV2.StartDate,oRosterPlanEmployeeV2.EndDate,oRosterPlanEmployeeV2.IsDayOff,oRosterPlanEmployeeV2.IsHoliday,oRosterPlanEmployeeV2.IsPIMSRoaster, nUserID);
        }
    }
}

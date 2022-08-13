using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class EmployeeLeaveOnAttendanceDA
    {
        public EmployeeLeaveOnAttendanceDA() { }

        #region Get Functions

        public static IDataReader Gets(TransactionContext tc, string employeeIDs, DateTime dtFrom, DateTime dtTo)
        {
            string sSQL = "Select Leave.*, LeaveHead.Name LeaveHeadName from " +
                          " (Select EmployeeID, Count(*) LeaveDays, LeaveHeadID  from AttendanceDaily Where AttendanceDate Between '" + dtFrom.ToString("dd MMM yyyy") + "' And '" + dtTo.ToString("dd MMM yyyy") + "'" +
                          " And LeaveHeadID>0 And EmployeeID In (" + employeeIDs + ")  Group By EmployeeID, LeaveHeadID) Leave " +
                          " Left Join LeaveHead On Leave.LeaveHeadID=LeaveHead.LeaveHeadID";
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsComp(TransactionContext tc, string employeeIDs, DateTime dtFrom, DateTime dtTo)
        {
            string sSQL = "Select Leave.*, LeaveHead.Name LeaveHeadName from " +
                          " (Select EmployeeID, Count(*) LeaveDays, CompLeaveHeadID  AS LeaveHeadID  from AttendanceDaily Where AttendanceDate Between '" + dtFrom.ToString("dd MMM yyyy") + "' And '" + dtTo.ToString("dd MMM yyyy") + "'" +
                          " And CompLeaveHeadID>0 And EmployeeID In (" + employeeIDs + ")  Group By EmployeeID, CompLeaveHeadID) Leave " +
                          " Left Join LeaveHead On Leave.LeaveHeadID=LeaveHead.LeaveHeadID";
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsActulaForComp(TransactionContext tc, string employeeIDs, DateTime dtFrom, DateTime dtTo)
        {
            string sSQL = "Select Leave.*, LeaveHead.Name LeaveHeadName from " +
                          " (Select EmployeeID, Count(*) LeaveDays, LeaveHeadID  AS LeaveHeadID  from AttendanceDaily Where AttendanceDate Between '" + dtFrom.ToString("dd MMM yyyy") + "' And '" + dtTo.ToString("dd MMM yyyy") + "'" +
                          " And LeaveHeadID>0 And EmployeeID In (" + employeeIDs + ")  Group By EmployeeID, LeaveHeadID) Leave " +
                          " Left Join LeaveHead On Leave.LeaveHeadID=LeaveHead.LeaveHeadID";
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

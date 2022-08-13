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
    public class LeaveStatusDA
    {
        public static IDataReader Gets(string sSQL, DateTime SalaryStartDate, DateTime SalaryEndDate, TransactionContext tc)
        {
            string FinalsSQL = "SELECT Leave.EmployeeID, Leave.LeaveHeadID, Leave.LeaveDays, LH.Name AS LeaveHeadName, LH.ShortName AS LeaveHeadShortName  FROM (SELECT ATD.EmployeeID, ATD.LeaveHeadID, COUNT(*) AS LeaveDays FROM AttendanceDaily AS ATD WITH(NOLOCK) "
         + " WHERE ATD.EmployeeID IN (SELECT ES.EmployeeID FROM EmployeeSalary AS ES " + sSQL + ")"
         + " AND ATD.AttendanceDate BETWEEN '" + SalaryStartDate.ToString("dd MMM yyyy") + "' AND '" + SalaryEndDate.ToString("dd MMM yyyy") + "' AND ATD.LeaveHeadID > 0 AND ATD.IsLeave = 1 GROUP BY ATD.EmployeeID, ATD.LeaveHeadID) AS Leave "
         + "LEFT OUTER JOIN LeaveHead AS LH ON Leave.LeaveHeadID = LH.LeaveHeadID "
         + "ORDER BY Leave.EmployeeID, Leave.LeaveHeadID ASC";
            return tc.ExecuteReader(FinalsSQL);
        }
        public static IDataReader CompGets(string sSQL, int nMOCID, DateTime SalaryStartDate, DateTime SalaryEndDate, TransactionContext tc)
        {
            string FinalsSQL = "SELECT Leave.EmployeeID, Leave.LeaveHeadID, Leave.LeaveDays, LH.Name AS LeaveHeadName, LH.ShortName AS LeaveHeadShortName  FROM (SELECT ATD.EmployeeID, ATD.LeaveHeadID AS LeaveHeadID, COUNT(*) AS LeaveDays FROM MaxOTConfigurationAttendance AS ATD WITH(NOLOCK) "
         + " WHERE ATD.MOCID = " + nMOCID.ToString() + " AND ATD.EmployeeID IN (SELECT ES.EmployeeID FROM ComplianceEmployeeSalary AS ES " + sSQL + ")"
         + " AND ATD.AttendanceDate BETWEEN '" + SalaryStartDate.ToString("dd MMM yyyy") + "' AND '" + SalaryEndDate.ToString("dd MMM yyyy") + "' AND ATD.LeaveHeadID > 0 GROUP BY ATD.EmployeeID, ATD.LeaveHeadID) AS Leave "
         + "LEFT OUTER JOIN LeaveHead AS LH ON Leave.LeaveHeadID = LH.LeaveHeadID "
         + "ORDER BY Leave.EmployeeID, Leave.LeaveHeadID ASC";
            return tc.ExecuteReader(FinalsSQL);
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class EmployeeOTonAttendanceDA
    {
        public EmployeeOTonAttendanceDA() { }

        #region Get Functions

        public static IDataReader Gets(TransactionContext tc, bool IsCompliance, string employeeIDs, DateTime dtFrom, DateTime dtTo)
        {
            string sSQL=string.Empty;
            if(IsCompliance)
            {
                //sSQL = "Select EmployeeID, SUM(CASE WHEN (ISNULL(OverTimeInMinute,0)>120) THEN 120 ELSE ISNULL(OverTimeInMinute,0) END) as OTMinute from AttendanceDaily Where AttendanceDate Between '" + dtFrom.ToString("dd MMM yyyy") + "' And '" + dtTo.ToString("dd MMM yyyy") + "'" +
                //     " AND EmployeeID In (" + employeeIDs + ") Group By EmployeeID";

                sSQL = "Select EmployeeID, SUM(ISNULL(CompOverTimeInMinute,0)) as OTMinute from AttendanceDaily Where AttendanceDate Between '" + dtFrom.ToString("dd MMM yyyy") + "' And '" + dtTo.ToString("dd MMM yyyy") + "'" +
                     " AND EmployeeID In (" + employeeIDs + ") Group By EmployeeID";
            }
            else
            {
                sSQL = "Select EmployeeID, SUM(ISNULL(OverTimeInMinute,0)) as OTMinute from AttendanceDaily Where AttendanceDate Between '" + dtFrom.ToString("dd MMM yyyy") + "' And '" + dtTo.ToString("dd MMM yyyy") + "'" +
                     " AND EmployeeID In (" + employeeIDs + ") Group By EmployeeID";
            }
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}

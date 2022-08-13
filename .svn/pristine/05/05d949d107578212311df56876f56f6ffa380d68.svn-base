using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class CompliancePayrollProcessManagementDA
    {
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsRunningEmployeeBatchs(TransactionContext tc, CompliancePayrollProcessManagement oCPPM)
        {
            string sSQL = "SELECT	 EMP.BusinessUnitID," +
                          "          EMP.LocationID," +
                          "          EMP.DepartmentID," +
                          "          EMP.EmpCount," +
                          "          (SELECT BU.ShortName FROM BusinessUnit AS BU WHERE BU.BusinessUnitID = EMP.BusinessUnitID) AS BUName," +
                          "          (SELECT LOC.Name FROM Location AS LOC WHERE LOC.LocationID = EMP.LocationID) AS LocName," +
                          "          (SELECT Dept.Name FROM Department AS Dept WHERE Dept.DepartmentID = EMP.DepartmentID) AS DeptName" +
                          " FROM (" +
                          "     SELECT HH.BusinessUnitID, HH.LocationID, HH.DepartmentID, COUNT(*) AS EmpCount FROM View_EmployeeOfficial AS HH " +
                          "     WHERE HH.IsActive = 1 " +
                          "     AND HH.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (SELECT MaxOT.EmployeeTypeID FROM MaxOTCEmployeeType AS MaxOT WHERE MaxOT.MaxOTConfigurationID = " + oCPPM.MOCID.ToString() + "))" +
                          "     AND HH.BusinessUnitID IN (SELECT MM.items FROM dbo.SplitInToDataSet('" + BusinessUnit.IDInString(oCPPM.BusinessUnits) + "', ',') AS MM)" +
                          "     AND HH.LocationID IN (SELECT MM.items FROM dbo.SplitInToDataSet('" + Location.IDInString(oCPPM.Locations) + "', ',') AS MM)" +
                          "     AND HH.DepartmentID IN (SELECT MM.items FROM dbo.SplitInToDataSet('" + Department.IDInString(oCPPM.Departments) + "', ',') AS MM)" +
                          "     GROUP BY HH.BusinessUnitID, HH.LocationID, HH.DepartmentID " +
                          ") AS EMP ORDER BY EMP.BusinessUnitID, EMP.LocationID, EMP.EmpCount DESC";

            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsArchiveEmployeeBatchs(TransactionContext tc, CompliancePayrollProcessManagement oCPPM)
        {
            string sSQL = "SELECT	 EMP.BusinessUnitID," +
                          "          EMP.LocationID," +
                          "          EMP.DepartmentID," +
                          "          EMP.EmpCount," +
                          "          (SELECT BU.ShortName FROM BusinessUnit AS BU WHERE BU.BusinessUnitID = EMP.BusinessUnitID) AS BUName," +
                          "          (SELECT LOC.Name FROM Location AS LOC WHERE LOC.LocationID = EMP.LocationID) AS LocName," +
                          "          (SELECT Dept.Name FROM Department AS Dept WHERE Dept.DepartmentID = EMP.DepartmentID) AS DeptName" +
                          " FROM (" +
                          "     SELECT HH.BUID AS BusinessUnitID, HH.LocationID, HH.DepartmentID, COUNT(*) AS EmpCount FROM ArchiveSalaryStruc AS HH " +
                          "     WHERE HH.SalaryYearID = " + oCPPM.YearID.ToString() + " AND HH.SalaryMonthID = " + oCPPM.MonthID.ToString() +
                          "     AND HH.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (SELECT MaxOT.EmployeeTypeID FROM MaxOTCEmployeeType AS MaxOT WHERE MaxOT.MaxOTConfigurationID = " + oCPPM.MOCID.ToString() + "))" +
                          "     AND HH.BUID IN (SELECT MM.items FROM dbo.SplitInToDataSet('" + BusinessUnit.IDInString(oCPPM.BusinessUnits) + "', ',') AS MM)" +
                          "     AND HH.LocationID IN (SELECT MM.items FROM dbo.SplitInToDataSet('" + Location.IDInString(oCPPM.Locations) + "', ',') AS MM)" +
                          "     AND HH.DepartmentID IN (SELECT MM.items FROM dbo.SplitInToDataSet('" + Department.IDInString(oCPPM.Departments) + "', ',') AS MM)" +
                          "     GROUP BY HH.BUID, HH.LocationID, HH.DepartmentID " +
                          ") AS EMP ORDER BY EMP.BusinessUnitID, EMP.LocationID, EMP.EmpCount DESC";

            return tc.ExecuteReader(sSQL);
        }

        public static void CompPayRollProcess(TransactionContext tc, CompliancePayrollProcessManagement oCPPM, string sBUIDs, string sLocationIDs, string sDepartmentIDs, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_Process_Comp_Payroll_Process_AMG]" + "%n, %n, %n, %d, %d, %s, %s, %s, %n",
                                    oCPPM.MOCID, oCPPM.YearID, oCPPM.MonthID, oCPPM.SalaryFrom, oCPPM.SalaryTo, sBUIDs, sLocationIDs, sDepartmentIDs, nUserID);
        }

        public static void DeleteCompPayRollProcess(TransactionContext tc, string sCPPMIDs, Int64 nUserID, EnumDBOperation eDBOperation)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CompliancePayrollProcessManagement]" + "%s, %n, %n", sCPPMIDs, nUserID, (int)eDBOperation);
        }
    }
}

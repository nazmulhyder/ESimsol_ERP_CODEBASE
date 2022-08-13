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
    public class EmployeeSalaryDA
    {
        public EmployeeSalaryDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeSalary oEmployeeSalary, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeSalary] ",
                   oEmployeeSalary.EmployeeSalaryID, oEmployeeSalary.EmployeeID,
                   oEmployeeSalary.LocationID, oEmployeeSalary.DepartmentID,
                   oEmployeeSalary.DesignationID, oEmployeeSalary.GrossAmount,
                   oEmployeeSalary.NetAmount, oEmployeeSalary.CurrencyID,
                   oEmployeeSalary.SalaryDate, oEmployeeSalary.SalaryReceiveDate,
                   oEmployeeSalary.PayrollProcessID, oEmployeeSalary.IsManual,
                   oEmployeeSalary.StartDate, oEmployeeSalary.EndDate,
                   oEmployeeSalary.IsLock,oEmployeeSalary.ProductionAmount,
                   oEmployeeSalary.ProductionBonus,oEmployeeSalary.OTHour,
                   oEmployeeSalary.OTRatePerHour, oEmployeeSalary.TotalWorkingDay,
                   oEmployeeSalary.TotalAbsent, oEmployeeSalary.TotalLate,
                   oEmployeeSalary.TotalEarlyLeaving, oEmployeeSalary.TotalDayOff,
                   oEmployeeSalary.TotalUpLeave, oEmployeeSalary.TotalPLeave,
                   oEmployeeSalary.RevenueStemp, nUserID, nDBOperation);
         
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nEmployeeSalaryID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalary WHERE EmployeeSalaryID=%n", nEmployeeSalaryID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalary");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader UpdateOutSheet(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);

        }
        public static IDataReader ProcessSalaryComp(TransactionContext tc, EmployeeSalary oEmployeeSalary, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_Payroll_Corporate_V1_Single]" + "%n, %n", oEmployeeSalary.EmployeeSalaryID, nUserID);

        }
        public static IDataReader ProcessSalary(TransactionContext tc, EmployeeSalary oEmployeeSalary, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_Payroll_Corporate_V1_Single]" + "%n, %n", oEmployeeSalary.EmployeeSalaryID, nUserID);

        }

        public static bool GetSalary(string sSQL, TransactionContext tc)
        {
            object obj = tc.ExecuteScalar(sSQL);
            if (obj == null)
            {
                return false;
            }
            else
            {
                int n = Convert.ToInt32(obj);
                if (n > 0) return true;
                else
                    return false;
            }
        }
        public static IDataReader Gets_ZN(string sEmpIDs, string sDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, TransactionContext tc)
        {
            DateTime dStartDate = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dEndDate = Convert.ToDateTime(sDate.Split(',')[1]);

            return tc.ExecuteReader("EXEC [SP_rpt_EmployeeSalary_ZN] %d,%d,%s,%s,%s,%s",
                   dStartDate, dEndDate, sEmpIDs, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs);
        }
        public static IDataReader GetsComparisonReport(string BUIDs, string LocIDs, string DeptIDs, string DesignationIDs, string SchemeIDs, string EmpIDs, bool isMonthWise, int MonthFrom, int YearFrom, int MonthTo, int YearTo, int ComparisonYearFrom, int ComparisonYearTo, double MinSalary, double MaxSalary, string GroupIDs, string BlockIDs, int GroupBy, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_ComparisonSalarySummary] %s,%s,%s,%s,%s,%s,%b,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n",
                BUIDs,
                LocIDs,
                DeptIDs,
                DesignationIDs,
                SchemeIDs,
                EmpIDs,
                isMonthWise,
                MonthFrom,
                YearFrom,
                MonthTo,
                YearTo,
                ComparisonYearFrom,
                ComparisonYearTo,
                MinSalary,
                MaxSalary,
                GroupIDs,
                BlockIDs,
                GroupBy,
                nUserID);
        }

        public static IDataReader GetPayRollBreakDown(DateTime StartDate, DateTime EndDate, bool IsDateSearch, int nLocationID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_PayRollbreakDown] %d, %d, %b, %n", StartDate, EndDate, IsDateSearch, nLocationID);
        }
        
        #endregion
    }
}

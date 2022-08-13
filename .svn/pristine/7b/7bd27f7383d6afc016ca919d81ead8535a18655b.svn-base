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
    public class MonthlyAttendanceReportDA
    {
        public MonthlyAttendanceReportDA() { }

        #region Get

        public static IDataReader Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_MonthlyAttendance] %s,%s,%s,%s,%s,%s,%d,%d,%s,%n, %s, %s,%n,%n,%s, %s",
                  sEmployeeIDs, sBusinessUnitIds,sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus,nUserID, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks);
        }
        public static IDataReader Gets_F3_Comp(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_Comp_MonthlyAttendance] %s,%s,%s,%s,%s,%s,%d,%d,%s,%n, %s, %s",
                  sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, nUserID, sGroupIDs, sBlockIDs);
        }

        #endregion


    }
}

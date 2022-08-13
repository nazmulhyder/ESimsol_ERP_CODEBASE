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
    public class MonthlyAttendance_LeaveDA
    {
        public MonthlyAttendance_LeaveDA() { }

        #region Get

        public static IDataReader Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_MonthlyAttendance_Leave] %s,%s,%s,%s,%s,%s,%d,%d,%s,%n",
                 sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, nUserID);
        }

        public static IDataReader Gets_Comp(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_Comp_MonthlyAttendance_Leave] %s,%s,%s,%s,%s,%s,%d,%d,%s,%n",
                 sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, nUserID);
        }
        #endregion

    }
}

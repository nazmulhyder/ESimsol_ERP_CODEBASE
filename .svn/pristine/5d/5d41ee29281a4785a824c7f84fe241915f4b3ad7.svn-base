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
    public class SalarySummaryDetail_F2DA
    {
        public SalarySummaryDetail_F2DA() { }

        #region Get & Exist Function
        public static IDataReader Gets(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_SalarySummaryDetail_F2] %s,%s,%s,%s,%s,%s,%n,%n,%n,%b,%n, %s, %s,%n,%n",
                   BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, nUserID, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange);
        }
        public static IDataReader GetsSalarySummaryDetailComp(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, bool bIsOutSheet, string sGroupIDs, string sBlockIDs, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_Comp_SalarySummaryDetail] %s,%s,%s,%s,%s,%s,%n,%n,%n,%b,%b,%n, %s, %s",
                   BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, bIsOutSheet, nUserID, sGroupIDs, sBlockIDs);
        }

        public static IDataReader GetsGroupDetail(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, int EmpGrouping, double nStartSalaryRange, double nEndSalaryRange, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_SalarySummaryDetail_BUWise_Group] %s,%s,%s,%s,%s,%s,%n,%n,%n,%b,%n,%n,%n,%n",
                   BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, nUserID, EmpGrouping, nStartSalaryRange, nEndSalaryRange);
        }
        #endregion
    }
}

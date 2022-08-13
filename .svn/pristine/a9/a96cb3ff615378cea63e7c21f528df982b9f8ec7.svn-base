using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class AMGSalarySheetDA
    {
        public AMGSalarySheetDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, string sGroupIDs, string sBlockIDs, bool IsComp, Int64 nUserId)
        {

            return tc.ExecuteReader("EXEC [SP_RPT_AMG_SalarySheet_Comp]" + " %s, %s, %s, %s, %s, %s, %n, %n, %b, %n, %n, %n, %s, %s, %n, %b", BUIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, nMonthID, nYear, IsNewJoin, IsOutSheet, nMinSalary, nMaxSalary, sGroupIDs, sBlockIDs, nUserId, IsComp);
        }
        public static IDataReader GetsComp(TransactionContext tc, string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, string sGroupIDs, string sBlockIDs, int nTimeCardID, Int64 nUserId)
        {

            return tc.ExecuteReader("EXEC [SP_RPT_AMG_SalarySheet_Comp_AsPerTimeCard]" + " %s, %s, %s, %s, %s, %s, %n, %n, %b, %n, %n, %n, %s, %s, %n, %n", BUIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, nMonthID, nYear, IsNewJoin, IsOutSheet, nMinSalary, nMaxSalary, sGroupIDs, sBlockIDs, nUserId, nTimeCardID);
        }
        public static IDataReader GetsPaySlip(TransactionContext tc, string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, string sGroupIDs, string sBlockIDs, int nMOCID)
        {

            return tc.ExecuteReader("EXEC [SP_RPT_AMG_PaySlip_Comp]" + " %s, %s, %s, %s, %s, %s, %n, %n, %b, %n, %n, %n, %n, %s, %s", BUIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, nMonthID, nYear, IsNewJoin, IsOutSheet, nMinSalary, nMaxSalary, nMOCID, sGroupIDs, sBlockIDs);
        }
     

        #endregion
    }  
    
   
}

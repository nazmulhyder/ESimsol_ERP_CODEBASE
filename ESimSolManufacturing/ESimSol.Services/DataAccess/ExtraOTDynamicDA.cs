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
    public class ExtraOTDynamicDA
    {
        public ExtraOTDynamicDA() { }

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string BlockIds, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, int nMOCID)
        {

            return tc.ExecuteReader("EXEC [SP_RPT_ExtraOTSheet]" + " %s, %s, %s, %s, %s, %s, %s, %n, %n, %b, %n, %n, %n, %n", BUIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, BlockIds, EmployeeIDs, nMonthID, nYear, IsNewJoin, IsOutSheet, nMinSalary, nMaxSalary, nMOCID);
        }
     

        #endregion
    }  
    
   
}


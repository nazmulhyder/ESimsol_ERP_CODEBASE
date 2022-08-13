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
    public class EmpSalarySummeryDA
    {
        public EmpSalarySummeryDA() { }
        #region Get & Exist Function
        public static IDataReader Gets(string sEmpIDs, DateTime StartDate, DateTime EndDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_SalarySummery] %d,%d,%s,%s,%s,%s",
                    StartDate, EndDate,
                    sEmpIDs, sDepartmentIDs,
                    sDesignationIDs, sSalarySchemeIDs);
        }

        public static IDataReader Gets_Compliance(string sEmpIDs, DateTime StartDate, DateTime EndDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_SalarySummery_Compliance] %d,%d,%s,%s,%s,%s",
                    StartDate, EndDate,
                    sEmpIDs, sDepartmentIDs,
                    sDesignationIDs, sSalarySchemeIDs);
        }
        #endregion
    }
}

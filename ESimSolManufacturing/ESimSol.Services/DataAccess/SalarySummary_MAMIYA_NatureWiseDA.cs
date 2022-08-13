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
    public class SalarySummary_MAMIYA_NatureWiseDA
    {
        public SalarySummary_MAMIYA_NatureWiseDA() { }
        #region Get & Exist Function
        public static IDataReader Gets(DateTime StartDate, DateTime EndDate,string sEmpIDs, int LocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, bool bBankPay, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_SalarySummery_MAMIYA_NatureWise] %d,%d,%s,%n,%s,%s,%s,%b",
                    StartDate, EndDate,
                    sEmpIDs,LocationID, sDepartmentIDs,
                    sDesignationIDs, sSalarySchemeIDs, bBankPay);
        }
        #endregion
    }
}

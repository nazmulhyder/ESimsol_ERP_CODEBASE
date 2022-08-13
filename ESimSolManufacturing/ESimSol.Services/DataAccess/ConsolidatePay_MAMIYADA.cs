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
    public class ConsolidatePay_MAMIYADA
    {
        public ConsolidatePay_MAMIYADA() { }

        #region Get
        public static IDataReader Gets(DateTime dtDateFrom, DateTime dtDateTo,string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID, int nPayType, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_MAMIYA_ConsolidatePay] %s,%d,%d,%n,%s,%s,%s,%n,%n",
                 sEmpIDs,dtDateFrom, dtDateTo, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nMonthID, nPayType);
        }
        #endregion

    }
}

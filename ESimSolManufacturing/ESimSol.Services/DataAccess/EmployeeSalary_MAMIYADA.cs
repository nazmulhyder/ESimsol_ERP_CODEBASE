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
    public class EmployeeSalary_MAMIYADA
    {
        public EmployeeSalary_MAMIYADA() { }

        #region Get & Exist Function
        public static IDataReader Gets(DateTime dtDateFrom, DateTime dtDateTo, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nPayType, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_rpt_EmployeeSalary_MAMIYA] %d,%d,%s,%n,%s,%s,%s,%n",
                   dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType);
        }
        public static IDataReader GetForPaySlip(DateTime dtDateFrom, DateTime dtDateTo, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs,int nPayType, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_rpt_MultiplePaySlip_MAMIYA] %d,%d,%s,%n,%s,%s,%s,%n",
                   dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType);
        }

        public static IDataReader Gets_SalarySummery(DateTime dtDateFrom, DateTime dtDateTo, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs,int nPayType, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_SalarySummery_MAMIYA] %d,%d,%s,%n,%s,%s,%s,%n",
                   dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType);
        }

        public static IDataReader Gets_FinalSettlementOfResig(int nEmployeeSettlementID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_rpt_MAMIYA_FinalSettlementOfResig] %n",
                   nEmployeeSettlementID);
        }

        public static IDataReader Gets_OTSheet(DateTime dtStartDate, DateTime dtEndDate, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_MAMIYA_OTSheet] %d,%d",
                    dtStartDate,  dtEndDate);
        }

        public static bool IsExists(string sSQL, TransactionContext tc)
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

        #endregion
    }
}

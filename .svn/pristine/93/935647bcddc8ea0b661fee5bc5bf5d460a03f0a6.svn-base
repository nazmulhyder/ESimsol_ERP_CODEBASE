using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class OTExceptComplianceDA
    {
        public OTExceptComplianceDA() { }


        #region Get Functions

        public static IDataReader Gets(TransactionContext tc, string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sEmpIDs, string sBMMIDs, int nPayType, int nMonthID, int nYear, bool bNewJoin, bool bExceptComp)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_ExceptCompliance] %s, %s, %s, %s, %s, %s,  %s, %n, %n, %n, %b, %b", sBU, sLocationID, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, sEmpIDs, sBMMIDs, nPayType, nMonthID, nYear, bNewJoin, bExceptComp);
        }

        #endregion
    }
}

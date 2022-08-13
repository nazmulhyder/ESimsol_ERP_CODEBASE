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
    public class HRAuditDA
    {
        public HRAuditDA() { }


        #region Get & Exist Function
        public static IDataReader GetAuditReport(TransactionContext tc, string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs
          , string sSalarySchemeIDs, string sBlockIDs, string sGroupIDs, string sEmpIDs, int nMonthID, int nYear, bool bNewJoin, int IsOutSheet
          , double nStartSalaryRange, double nEndSalaryRange, bool IsCompliance, int nPayType)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_HRAudit] " + "%s,%s,%s,%s,%s,%s,%s,%s,%n,%n,%b,%n,%n,%n,%b,%n",
                   sBU
                   , sLocationID
                   , sDepartmentIDs
                   , sDesignationIDs
                   , sSalarySchemeIDs
                   , sBlockIDs
                   , sGroupIDs
                   , sEmpIDs
                   , nMonthID
                   , nYear
                   , bNewJoin
                   , IsOutSheet
                   , nStartSalaryRange
                   , nEndSalaryRange
                   , IsCompliance
                   , nPayType
                   );
        }
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}


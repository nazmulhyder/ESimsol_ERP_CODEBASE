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
    public class ExtraBenefitDA
    {
        public ExtraBenefitDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(DateTime dDateFrom, DateTime dDateTo, string BOAIDs, string sEmployeeIDs, string sLocationID, string sDepartmentIDs, string sBusinessUnitIDs, double nStartSalary, double nEndSalary, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_ExtraBenefit] %d,%d,%s,%s,%s,%s,%s,%n,%n,%n",
                   dDateFrom, dDateTo, BOAIDs, sEmployeeIDs, sLocationID, sDepartmentIDs, sBusinessUnitIDs, nStartSalary, nEndSalary, nUserID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL, bool bIsComp)
        {
            return tc.ExecuteReader("EXEC [SP_BenefitOfAttendance_AMG] %s, %b", sSQL, bIsComp);
        }
        #endregion
    }
}

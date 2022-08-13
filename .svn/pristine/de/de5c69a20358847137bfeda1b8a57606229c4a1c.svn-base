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
    public class BenefitOnAttendanceReportDA
    {
        public BenefitOnAttendanceReportDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(DateTime StartDate, DateTime EndDate, string BOAIDs, string sEmployeeIDs, string  sLocationID, string sDepartmentIDs, string sBusinessUnitIDs, double nStartSalaryRange, double nEndSalaryRange,Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_BenefitOnAttendanceEmployeeLedger] %d,%d,%s,%s,%s,%s,%s,%n,%n,%n",
                   StartDate, EndDate, BOAIDs, sEmployeeIDs, sLocationID, sDepartmentIDs, sBusinessUnitIDs, nStartSalaryRange, nEndSalaryRange, nUserID);
        }

        #endregion
    }
}

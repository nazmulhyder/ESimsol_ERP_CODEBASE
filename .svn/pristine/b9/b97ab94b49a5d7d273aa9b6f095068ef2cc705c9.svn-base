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
    public class EarnLeaveReportDA
    {
        public EarnLeaveReportDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sELEncashIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIDs, string sEmployeeIDs, int nACSID,  bool IsDeclarationDate,DateTime DeclarationDate,double nStartSalary,double nEndSalary ,Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_EncashedEL] %s,%s,%s,%s,%s,%s,%n,%b,%d,%n,%n,%n", sELEncashIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIDs, sEmployeeIDs, nACSID, IsDeclarationDate, DeclarationDate, nStartSalary, nEndSalary, nUserID);
        }
        #endregion
    }
}

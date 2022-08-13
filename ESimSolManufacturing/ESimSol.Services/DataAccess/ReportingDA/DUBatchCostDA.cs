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

    public class DUBatchCostDA
    {
        public DUBatchCostDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sSQL, int nRouteSheetID, int nReportType)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_DUBatchCost]" + " %d,%d,%s,%n,%n", dStartDate, dEndDate, sSQL, nRouteSheetID, nReportType);
        }
        public static IDataReader GetsDetail(TransactionContext tc, int nRouteSheetID, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_DUBatchCostDetail]" + " %n, %s", nRouteSheetID, sSQL);
        }
        #endregion
    }  
    
   
}

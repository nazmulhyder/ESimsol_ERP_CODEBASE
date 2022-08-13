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

    public class FNBatchCostDA
    {
        public FNBatchCostDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sSQL, int nRouteSheetID, int nReportType)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_FN_BatchCost]" + " %d,%d,%n", dStartDate, dEndDate, nReportType);
        }
        public static IDataReader GetsDetail(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_FN_BatchCostDetail]" + "%s", sSQL);
        }
        public static IDataReader GetsSQL(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader( sSQL);
        }
        #endregion
    }  
    
   
}

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



    public class DUDeliverySummaryDA
    {
        public DUDeliverySummaryDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, int OrderType, int nReportType)
        {
            return tc.ExecuteReader("EXEC [sp_DUDeliverySummary]" + " %d,%d,%n,%n", dStartDate, dEndDate, OrderType, nReportType);
        }
        #endregion
    }  
    
   
}

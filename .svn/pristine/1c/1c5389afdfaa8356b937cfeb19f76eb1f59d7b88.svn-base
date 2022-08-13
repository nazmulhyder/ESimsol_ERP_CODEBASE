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
    public class LeaveReport_XLDA
    {
        public LeaveReport_XLDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(DateTime dtStartDate, DateTime dtEndDate, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_MAMIYA_LeaveReport] %d,%d",
            dtStartDate, dtEndDate);
        }
        #endregion
    }
}

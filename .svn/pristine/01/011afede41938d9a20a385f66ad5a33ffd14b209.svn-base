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
    public class SalarySummaryDA
    {
        #region Get & Exist Function
        public static IDataReader Gets(DateTime StartDate, DateTime EndDate, bool IsDateSearch,int nLocationID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_SalarySummary] %d, %d, %b, %n", StartDate, EndDate, IsDateSearch, nLocationID);
        }

        #endregion
    }
}

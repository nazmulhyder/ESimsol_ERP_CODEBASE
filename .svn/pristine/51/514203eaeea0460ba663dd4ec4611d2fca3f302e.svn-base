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
    public class AttendancePerformanceChartDA
    {
        public AttendancePerformanceChartDA() { }

        #region Get

        public static IDataReader Gets(int nYear,TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Process_AttPerformance]%n", nYear);
        }

        #endregion


    }
}

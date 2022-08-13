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
    public class AttendanceSummery_ChartDA
    {
        public AttendanceSummery_ChartDA() { }

        #region Get

        public static IDataReader Get(DateTime DateFrom, DateTime DateTo, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Process_AttendanceSummery_Chart]%d,%d", DateFrom, DateTo);
        }

        #endregion


    }
}

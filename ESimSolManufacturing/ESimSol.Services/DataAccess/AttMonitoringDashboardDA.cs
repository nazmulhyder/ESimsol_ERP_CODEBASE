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
    public class AttMonitoringDashboardDA
    {
        public AttMonitoringDashboardDA() { }

        #region Get Function

        public static IDataReader Get(TransactionContext tc,DateTime StartDate, DateTime EndDate)
        {
            return tc.ExecuteReader("EXEC [SP_Process_AttMonitoringDashboard] %d,%d",

                   StartDate, EndDate);

        }


        #endregion


    }
}

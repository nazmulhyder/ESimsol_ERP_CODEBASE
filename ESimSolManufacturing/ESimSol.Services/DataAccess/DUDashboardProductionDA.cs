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
    public class DUDashboardProductionDA
    {
        #region Get & Exist Function
      

        public static IDataReader Gets(TransactionContext tc, DUDashboardProduction oDUDashboardProduction)
        {
            return tc.ExecuteReader("EXEC [SP_DU_DashBoard_Production] "
                                     + "%n , %b ,%D ,%D", oDUDashboardProduction.LocationID, oDUDashboardProduction.IsDate, oDUDashboardProduction.StartDate.ToString("dd MMM yyyy"), oDUDashboardProduction.EndDate.AddDays(1).ToString("dd MMM yyyy"));
        }
        public static IDataReader Get(TransactionContext tc, DUDashboardProduction oDUDashboardProduction)
        {
            return tc.ExecuteReader("EXEC [SP_DU_DashBoard_Production] "
                                     + "%n , %b ,%D ,%D", oDUDashboardProduction.LocationID, oDUDashboardProduction.IsDate, oDUDashboardProduction.StartDate.ToString("dd MMM yyyy"), oDUDashboardProduction.EndDate.AddDays(1).ToString("dd MMM yyyy"));
        }
        public static IDataReader Gets_Daily(TransactionContext tc, DUDashboardProduction oDUDDP)
        {
            return tc.ExecuteReader("EXEC [SP_DU_DashBoard_DailyProduction] "
                                     + "%b, %D ,%D ,%s", oDUDDP.IsDate, oDUDDP.StartDate.ToString("dd MMM yyyy"), oDUDDP.EndDate.AddDays(1).ToString("dd MMM yyyy"), oDUDDP.Name);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

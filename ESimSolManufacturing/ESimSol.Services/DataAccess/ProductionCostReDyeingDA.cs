using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductionCostReDyeingDA
    {
        public ProductionCostReDyeingDA() { }


        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, DateTime startDate, DateTime EndDate, string sBuyerIDs, string sRouteSheetNos, string sPTUIDs)
        {
            return tc.ExecuteReader("EXEC [SP_GetProductionCostForRedying]"
                       + "%D,%D,%s,%s,%s", startDate, EndDate, sBuyerIDs, sRouteSheetNos, sPTUIDs);

        }

        #endregion
    }
}

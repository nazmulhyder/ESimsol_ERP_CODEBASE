using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductionCostDA
    {
        public ProductionCostDA() { }


        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, DateTime startDate, DateTime EndDate, string sBuyerIDs, string sRouteSheetNos, string sPTUIDs)
        {
            return tc.ExecuteReader("EXEC [SP_GetProductionCost]"
                       + "%D,%D,%s,%s,%s",startDate, EndDate, sBuyerIDs, sRouteSheetNos, sPTUIDs);

        }

        #endregion
    }
}

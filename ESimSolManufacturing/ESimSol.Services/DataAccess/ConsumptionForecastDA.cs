using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ConsumptionForecastDA
    {
        public ConsumptionForecastDA() { }

       #region Get & Exist Function
       public static IDataReader PrepareConsumptionForecast(TransactionContext tc, ConsumptionForecast oConsumptionForecast)
        {
            return tc.ExecuteReader("EXEC [SP_Consumption_Forecast] %n, %d, %d, %n,%b",
                                    oConsumptionForecast.BUID, oConsumptionForecast.StartDate, oConsumptionForecast.EndDate, oConsumptionForecast.ProductNature, oConsumptionForecast.bIsWithPI);
        }
        #endregion
    }
}
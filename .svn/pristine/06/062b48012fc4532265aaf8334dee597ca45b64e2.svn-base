using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DyeingForeCastDA
    {
        public static IDataReader Gets(TransactionContext tc, int nBUID, EnumForecastLayout eForecastLayout, DateTime dStartDate, DateTime dEndDate)
        {
            return tc.ExecuteReader("EXEC [SP_Dyeing_Production_Forecast]" + "%n, %n, %d, %d", nBUID, (int)eForecastLayout, dStartDate, dEndDate);
        }
        public static IDataReader GetsDetails(TransactionContext tc, int nBUID, EumDyeingType eDyeingType, EnumForecastLayout eForecastLayout, DateTime dStartDate, DateTime dEndDate)
        {
            return tc.ExecuteReader("EXEC [SP_Dyeing_Production_Forecast_Detail]" + "%n, %n, %n, %d, %d", nBUID, (int)eDyeingType, (int)eForecastLayout, dStartDate, dEndDate);
        }
    }
}

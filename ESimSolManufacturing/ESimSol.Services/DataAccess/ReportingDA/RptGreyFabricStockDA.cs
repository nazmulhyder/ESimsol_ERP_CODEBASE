using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class RptGreyFabricStockDA
    {
        public RptGreyFabricStockDA() { }

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc,string sSQL, DateTime StartTime, DateTime EndTime,int ReportType,int StoreID)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_GreyFabricStock]" + "%s,%n,%D,%D,%n", sSQL, ReportType, StartTime, EndTime, StoreID);
        }

        #endregion
    }
}

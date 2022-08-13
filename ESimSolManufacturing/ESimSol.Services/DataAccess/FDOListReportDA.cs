using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class FDOListReportDA
    {
        #region Get & Exist Function 
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RPT_FDOListReport AS FDOL WHERE FDOL.FEOID IN (SELECT FDOD.FEOID FROM FabricDeliveryOrderDetail AS FDOD)");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, long nFabricProcessID)
        {
            //Should Modify
            return tc.ExecuteReader("SELECT * FROM View_RPT_FDOListReport AS FDOL WHERE FDOL.FEOID IN (SELECT FDOD.FEOID FROM FabricDeliveryOrderDetail AS FDOD)");
        }
        public static IDataReader GetsBysp(TransactionContext tc, string sParams)
        {
            return tc.ExecuteReader("EXEC SP_RPT_FDOListReport"
                                    + "%s",
                                   sParams);
        }
        #endregion
    }
}

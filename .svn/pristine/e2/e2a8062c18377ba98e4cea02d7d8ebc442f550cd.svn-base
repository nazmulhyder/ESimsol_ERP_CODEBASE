using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class RPT_DispoDA
    {
        public RPT_DispoDA() { }

        #region Insert Update Delete Function
        
        #endregion

        #region Get & Exist Function
        
        public static IDataReader Gets(TransactionContext tc, string sSQL, int nReportType, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_Dispo]" + "%s, %n", sSQL, nReportType);
        }
        public static IDataReader Gets_FYStockDispoWise(TransactionContext tc, string sSQL, int nReportType, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_FabricYarnStockDispoWise]" + "%s, %n", sSQL, nReportType);
        }
        public static IDataReader Gets_Weaving(TransactionContext tc, string sSQL, int nReportType, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_Dispo_Weaving]" + "%s, %n", sSQL, nReportType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }  
}


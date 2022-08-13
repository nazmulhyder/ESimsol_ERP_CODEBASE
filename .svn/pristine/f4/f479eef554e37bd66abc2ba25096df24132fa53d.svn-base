using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class RptFEOSalesSummaryDA
    {
        public RptFEOSalesSummaryDA() { }

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, Int16 nOrderType, DateTime dtFrom, DateTime dtTo, bool bIsBuyerWise,  int nExeType, bool bIsOrderTypeWise)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_FEOSalesSummary]" + "%n, %d, %d, %b, %n, %b", nOrderType, dtFrom, dtTo, bIsBuyerWise, nExeType, bIsOrderTypeWise);
        }


        #endregion
    }
}


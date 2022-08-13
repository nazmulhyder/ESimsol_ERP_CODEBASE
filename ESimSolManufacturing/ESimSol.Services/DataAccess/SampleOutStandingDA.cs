using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SampleOutStandingDA
    {
        public SampleOutStandingDA() { }

        #region Generation Function

        #endregion

        #region Get & Exist Function
     
        public static IDataReader Gets(TransactionContext tc, string sSQL,bool bIsDr)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sContractorIDs)
        {
            return tc.ExecuteReader("EXEC [sp_RPT_SampleOutStanding] '" + dStartDate.ToString("dd MMM yyyy") + "','" + dEndDate.ToString("dd MMM yyyy")+ "','" + sContractorIDs + "'");
        }
        public static IDataReader GetsWithMkt(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sContractorIDs)
        {
            return tc.ExecuteReader("EXEC [sp_RPT_SampleOutStanding_Mkt] '" + dStartDate.ToString("dd MMM yyyy") + "','" + dEndDate.ToString("dd MMM yyyy") + "','" + sContractorIDs + "'");
        }
        public static IDataReader GetsMktDetail(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sContractorIDs)
        {
            return tc.ExecuteReader("EXEC [sp_RPT_SampleOutStanding_Mkt_Detail] '" + dStartDate.ToString("dd MMM yyyy") + "','" + dEndDate.ToString("dd MMM yyyy") + "','" + sContractorIDs + "'");
        }
        #endregion

    }
}
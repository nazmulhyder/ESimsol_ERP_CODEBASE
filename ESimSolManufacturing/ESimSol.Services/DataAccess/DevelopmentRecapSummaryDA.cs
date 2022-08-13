using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class DevelopmentRecapSummaryDA
    {
        public DevelopmentRecapSummaryDA() { }

        #region image Function
       
        #endregion

        #region Get & Exist Function


        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecapSummery");
        }

        public static IDataReader GetsRecapWithOrderRecapSummerys(TransactionContext tc, int nStartRow, int nEndRow, string SQL, string sOrderRecapIDs, bool bIsPrint, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_DevelopmentRecapSummary]" + "%n, %n, %s, %s, %b", nStartRow, nEndRow, SQL, sOrderRecapIDs, bIsPrint);
        }
        #endregion
    }
}

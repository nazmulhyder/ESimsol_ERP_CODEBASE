using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VOSummeryDA
    {
        public VOSummeryDA() { }

        #region Get & Exist Function

        public static IDataReader GetsVOSummerys(TransactionContext tc, VOSummery oVOSummery, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_VOrderSummery]" + " %n, %n, %n, %d, %d, %n, %b, %n", oVOSummery.BUID, oVOSummery.AccountHeadID, oVOSummery.SubledgerID, oVOSummery.StartDate, oVOSummery.EndDate, oVOSummery.CurrencyID, oVOSummery.IsApproved, oVOSummery.BalanceStatusInt);
        }
        #endregion
    }
}

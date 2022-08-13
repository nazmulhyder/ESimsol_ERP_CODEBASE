using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class LedgerSummeryDA
    {
        #region Insert Update Delete Function

        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, LedgerSummery oLedgerSummery)
        {
            return tc.ExecuteReader("EXEC [SP_LedgerSummery]"
                                   + "%n, %n, %n, %d, %d, %b",
                                  oLedgerSummery.BUID, oLedgerSummery.CurrencyID, oLedgerSummery.AccountHeadID, oLedgerSummery.StartDate, oLedgerSummery.EndDate, oLedgerSummery.IsApproved);
        }

        #endregion
    }

}

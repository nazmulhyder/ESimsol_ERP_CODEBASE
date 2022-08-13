using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class DULedgerDA
    {
        public DULedgerDA() { }

        #region Insert Update Delete Function
        public static IDataReader Gets(TransactionContext tc, DULedger oDO, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_DULedger]" + "%d,%d,	%n,%n,%n, %n,%n,%n",
                                    oDO.StartDateSt, oDO.EndDateSt, oDO.Layout, oDO.ViewType, oDO.DyeingOrderType, oDO.PaymentType, oDO.CurrencyID, oDO.MKT_CPID);
        }
     
        #endregion
    }
}

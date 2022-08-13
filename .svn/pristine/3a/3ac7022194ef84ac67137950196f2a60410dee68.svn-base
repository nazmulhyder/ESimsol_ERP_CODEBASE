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
    public class ELProcessEditHistoryDA
    {
        public ELProcessEditHistoryDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ELProcessEditHistory oELProcessEditHistory, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ELProcessEditHistory] %n,%n,%n,%s,%n,%n",
                   oELProcessEditHistory.ELPEHID,
                   oELProcessEditHistory.EmployeeID,
                   oELProcessEditHistory.CurrentpresentBalance,
                   oELProcessEditHistory.Description,
                   nUserID, nDBOperation
                   );
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

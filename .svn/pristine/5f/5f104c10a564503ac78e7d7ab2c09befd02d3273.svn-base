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
    public class FinalSettlementFormDA
    {
        public FinalSettlementFormDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nEmployeeID)
        {
            return tc.ExecuteReader("EXEC [SP_rpt_MAMIYA_FinalSettlement]"
                                     + "%n",nEmployeeID);
        }
   
        #endregion
    }
}

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
    public class FinalSettlementDA
    {

       
        #region Get & Exist Function

        public static IDataReader Get(int nEmployeeSettlementID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_FinalSettlement]" + "%n", nEmployeeSettlementID);
        }


        #endregion
    }
}

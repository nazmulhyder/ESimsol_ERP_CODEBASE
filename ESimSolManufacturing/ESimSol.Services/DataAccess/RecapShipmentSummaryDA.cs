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

    public class RecapShipmentSummaryDA
    {
        public RecapShipmentSummaryDA() { }

        #region Insert Update Delete Function
        public static IDataReader Gets(TransactionContext tc, string sYear,int BUID, int nUserType, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_RecapShipmentSummary]" + " %s,%n,%n,%n", sYear, BUID, nUserType, nUserID);
        }
        #endregion

    }  
    
   
}

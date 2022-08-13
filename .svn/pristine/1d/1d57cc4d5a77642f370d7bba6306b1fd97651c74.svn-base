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
    public class InventoryTrackingWIPDA
    {
        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, InventoryTrackingWIP oITWIWP, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_InventoryTracking_WIP]"
                                   + "%n,%D,%D,%n",
                                   oITWIWP.BUID, oITWIWP.StartDate, oITWIWP.EndDate, nUserID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}

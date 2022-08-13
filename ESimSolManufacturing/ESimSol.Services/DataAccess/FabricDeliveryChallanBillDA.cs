using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricDeliveryChallanBillDA
    {
        public FabricDeliveryChallanBillDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_FabricDeliveryChallanBill]  %s", sSQL);
        }
        #endregion
    }  
}


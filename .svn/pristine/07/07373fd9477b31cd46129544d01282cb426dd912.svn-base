using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PurchaseReturnRegisterDA
    {
        public PurchaseReturnRegisterDA() { }
        public static IDataReader GetsPurchaseReturnRegister(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_PurchaseReturnRegister]" + "%s", sSQL);
        }

    }  
}


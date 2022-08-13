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
    public class CommercialInvoiceRegisterDA
    {
        public CommercialInvoiceRegisterDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, int nCIDetailID)
        {
            return tc.ExecuteReader("SELECT top 1 * FROM View_CommercialInvoiceRegister WHERE CommercialInvoiceDetailID = %n",nCIDetailID);
        }
        
        #endregion
    }
}

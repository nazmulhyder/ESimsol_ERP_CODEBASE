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
    public class CommercialInvoiceHistoryDA
    {
        public CommercialInvoiceHistoryDA() { }

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CommercialInvoiceHistory WHERE CommercialInvoiceHistoryID=%n", nID);
        }
        public static IDataReader Gets(int nCommercialInvoiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CommercialInvoiceHistory where CommercialInvoiceID =%n", nCommercialInvoiceID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

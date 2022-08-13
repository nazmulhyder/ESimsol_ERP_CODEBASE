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
    public class KnittingOrderRegisterDA
    {

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingOrderRegister WHERE KnittingOrderDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nKnittingOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingOrderRegister WHERE KnittingOrderDetailID =%n", nKnittingOrderID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsForOrderStatusWise(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_KnittingOrderStatus]" + "%s", sSQL);
        }

        #endregion 
    }

}

using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class RouteSheetApproveDA
    {
        public RouteSheetApproveDA() { }

        #region Generation Function

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetApprove_RS WHERE RouteSheetID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets_LotWise(TransactionContext tc, int nProductID)
        {
            return tc.ExecuteReader("Select * from View_RouteSheetApprove_Lot where ProductID=%n", nProductID);
        }
        public static IDataReader Gets_RSWise(TransactionContext tc)
        {
            return tc.ExecuteReader("Select * from View_RouteSheetApprove_RS");
        }
        #endregion

    }
}
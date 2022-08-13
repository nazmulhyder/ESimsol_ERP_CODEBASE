using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class FabricInHouseChallanDA
    {

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sFEONo, string sChallanNo, string BuyerIDs, bool bIsDate, DateTime dtFrom, DateTime dtTo, bool IsAll, int nUnit)
        {
            return tc.ExecuteReader("EXEC SP_Get_FabricInHouseChallan %s, %s, %s, %b, %d, %d, %b, %n", sFEONo, sChallanNo, BuyerIDs, bIsDate, dtFrom, dtTo, IsAll, nUnit);
        }
        #endregion
    }
}

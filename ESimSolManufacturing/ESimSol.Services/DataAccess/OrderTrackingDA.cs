using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class OrderTrackingDA
    {
        public OrderTrackingDA() { }      

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL, int nLayoutType, bool bIsYetToDO, bool bIsYetToChallan, bool bIsYetToChallanWithDOEntry, bool bIsPTUTransferQty)
        {
            return tc.ExecuteReader("EXEC [SP_JobTracking]'" + sSQL + "'," + nLayoutType + "," + bIsYetToDO + "," + bIsYetToChallan + "," + bIsYetToChallanWithDOEntry + " ," + bIsPTUTransferQty);
            //return tc.ExecuteReader("EXEC [SP_JobTracking_Temp]'" + sSQL + "'," + nLayoutType + "," + bIsYetToDO + "," + bIsYetToChallan + "," + bIsYetToChallanWithDOEntry + " ," + bIsPTUTransferQty);
        }

        #endregion
    }
}

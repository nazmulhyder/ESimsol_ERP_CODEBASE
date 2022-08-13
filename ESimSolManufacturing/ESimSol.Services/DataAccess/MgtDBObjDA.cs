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
    public class MgtDBObjDA
    {
        #region Get & Exist Function        
        public static IDataReader Gets(TransactionContext tc, MgtDBObj oMgtDBObj)
        {
            if (oMgtDBObj.RefType == EnumMgtDBRefType.Order_Summery)
            {
                return tc.ExecuteReader("EXEC [SP_MgtDBOrderSummery]" + "%n, %d, %d", oMgtDBObj.BUID, oMgtDBObj.StartDate, oMgtDBObj.EndDate);
            }
            else if (oMgtDBObj.RefType == EnumMgtDBRefType.Top_Five_Marketing_Performance || oMgtDBObj.RefType == EnumMgtDBRefType.Top_Ten_Customer || oMgtDBObj.RefType == EnumMgtDBRefType.Top_Five_Over_Due_Customer || oMgtDBObj.RefType == EnumMgtDBRefType.Highest_Selling_Product)
            {
                return tc.ExecuteReader("EXEC [SP_MgtDB_MKT_Customer_Product_Sale_Summery]" + "%n, %d, %d, %n", oMgtDBObj.BUID, oMgtDBObj.StartDate, oMgtDBObj.EndDate, (int)oMgtDBObj.RefType);
            }
            else if (oMgtDBObj.RefType == EnumMgtDBRefType.Highest_Produced_Product)
            {
                //return tc.ExecuteReader("EXEC [SP_MgtDBOrderSummery]" + "%n, %d, %d", oMgtDBObj.BUID, oMgtDBObj.StartDate, oMgtDBObj.EndDate);
                return tc.ExecuteReader("EXEC [SP_MgtDB_MKT_Customer_Product_Sale_Summery]" + "%n, %d, %d, %n", oMgtDBObj.BUID, oMgtDBObj.StartDate, oMgtDBObj.EndDate, (int)oMgtDBObj.RefType);
            }
            else if (oMgtDBObj.RefType == EnumMgtDBRefType.Export_PI_Issued_Vs_LCReceived || oMgtDBObj.RefType == EnumMgtDBRefType.Export_Recevable_Vs_Import_Payable)
            {
                return tc.ExecuteReader("EXEC [SP_MgtDBCommercialSummery]" + "%n, %n", oMgtDBObj.BUID, (int)oMgtDBObj.RefType);
            }
            else if (oMgtDBObj.RefType == EnumMgtDBRefType.Stock_Summery)
            {
                return tc.ExecuteReader("EXEC [SP_MgtDBStockSummery]" + "%n", oMgtDBObj.BUID);
            }
            else
            {
                return tc.ExecuteReader("EXEC [SP_MgtDBOrderSummery]" + "%n, %d, %d", oMgtDBObj.BUID, oMgtDBObj.StartDate, oMgtDBObj.EndDate);
            }
        }
        #endregion
    }

}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;



namespace ESimSol.Services.DataAccess
{
    public class RMClosingStockDetailDA
    {
        public RMClosingStockDetailDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, RMClosingStockDetail oEBillDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RMClosingStockDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%s",
                                    oEBillDetail.RMClosingStockDetailID, oEBillDetail.RMClosingStockID, oEBillDetail.RMAccountHeadID, oEBillDetail.Amount, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }
        public static void Delete(TransactionContext tc, RMClosingStockDetail oEBillDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RMClosingStockDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%s",
                                    oEBillDetail.RMClosingStockDetailID, oEBillDetail.RMClosingStockID, oEBillDetail.RMAccountHeadID, oEBillDetail.Amount, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RMClosingStockDetail WHERE RMClosingStockDetailID=%n", nID);
        }
    
        public static IDataReader Gets(TransactionContext tc, int nRMClosingStockID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RMClosingStockDetail WHERE RMClosingStockID=%n", nRMClosingStockID);
        }

        public static IDataReader GetsBySQL(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion

    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class RSRawLotDA
    {
        public RSRawLotDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, RSRawLot oRSRawLot, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RSRawLot]"
                                    + "%n,%n,%n,%n, %n,%n,%n,%n,%n,%n, %n,%n",
                                    oRSRawLot.RSRawLotID, oRSRawLot.RouteSheetID, oRSRawLot.LotID, oRSRawLot.CurrencyID, oRSRawLot.ProductTypeInt, oRSRawLot.Qty, oRSRawLot.UnitPrice, oRSRawLot.NumOfCone, oRSRawLot.RSShiftID,oRSRawLot.DyeingOrderDetailID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, RSRawLot oRSRawLot, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RSRawLot]"
                                    + "%n,%n,%n,%n, %n,%n,%n,%n,%n,%n, %n,%n",
                                    oRSRawLot.RSRawLotID, oRSRawLot.RouteSheetID, oRSRawLot.LotID, oRSRawLot.CurrencyID, oRSRawLot.ProductTypeInt, oRSRawLot.Qty, oRSRawLot.UnitPrice, oRSRawLot.NumOfCone, oRSRawLot.RSShiftID,oRSRawLot.DyeingOrderDetailID, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RSRawLot WHERE RSRawLotID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RSRawLot");
        }
        public static IDataReader GetsByRSID(TransactionContext tc, int RSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RSRawLot WHERE RouteSheetID=%n", RSID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_RSRawLot
        }
        #endregion
    }
}

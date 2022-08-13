using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricQtyAllowDA
    {
        public FabricQtyAllowDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricQtyAllow oFabricQtyAllow, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricQtyAllow]"
                                    + "%n, %n, %n, %n, %n,%n,%s,%n,%n, %n, %n",
                                    oFabricQtyAllow.FabricQtyAllowID, (int)oFabricQtyAllow.AllowType, oFabricQtyAllow.Qty_From, oFabricQtyAllow.Qty_To, oFabricQtyAllow.Percentage,oFabricQtyAllow.MunitID,oFabricQtyAllow.Note,(int)oFabricQtyAllow.OrderType,(int)oFabricQtyAllow.WarpWeftType, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, FabricQtyAllow oFabricQtyAllow, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricQtyAllow]"
                                    + "%n, %n, %n, %n, %n,%n,%s,%n,%n, %n, %n",
                                    oFabricQtyAllow.FabricQtyAllowID, (int)oFabricQtyAllow.AllowType, oFabricQtyAllow.Qty_From, oFabricQtyAllow.Qty_To, oFabricQtyAllow.Percentage, oFabricQtyAllow.MunitID, oFabricQtyAllow.Note, (int)oFabricQtyAllow.OrderType, (int)oFabricQtyAllow.WarpWeftType, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricQtyAllow WHERE FabricQtyAllowID=%n ORDER BY AllowType, Qty_From", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM FabricQtyAllow ORDER BY AllowType, Qty_From");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static int GetsFabricOrderType(TransactionContext tc, int nFSCD)
        {
            object obj = tc.ExecuteScalar("Select OrderType from FabricSalesContract where FabricSalesContractID=%n", nFSCD);
            if (obj == null) return 0;
            return Convert.ToInt32(obj);
        }
        #endregion
    }  
}


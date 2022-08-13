using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricReturnChallanDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricReturnChallan oFabricReturnChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
             return tc.ExecuteReader("EXEC [SP_IUD_FabricReturnChallan]"
                                    + "%n,%n,%s,%n,%d,%n,%n,%s,%s,%s,%s,%s,%n,%n",
                                    oFabricReturnChallan.FabricReturnChallanID,
                                    oFabricReturnChallan.FabricDeliveryChallanID,
                                    oFabricReturnChallan.ReturnNo,
                                    (int)oFabricReturnChallan.ReturnStatus,
                                    oFabricReturnChallan.ReturnDate,
                                    oFabricReturnChallan.BuyerID,
                                    oFabricReturnChallan.StoreID,
                                    oFabricReturnChallan.Remarks,
                                    oFabricReturnChallan.PartyChallanNo,
                                    oFabricReturnChallan.VehicleInfo,
                                    oFabricReturnChallan.GetInNo,
                                    oFabricReturnChallan.ReturnPerson,
                                    (int)eEnumDBOperation, 
                                    nUserID);
        }
        
        public static void Delete(TransactionContext tc, FabricReturnChallan oFabricReturnChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
             tc.ExecuteNonQuery("EXEC [SP_IUD_FabricReturnChallan]"
                                     + "%n,%n,%s,%n,%d,%n,%n,%s,%s,%s,%s,%s,%n,%n",
                                    oFabricReturnChallan.FabricReturnChallanID,
                                    oFabricReturnChallan.FabricDeliveryChallanID,
                                    oFabricReturnChallan.ReturnNo,
                                    (int)oFabricReturnChallan.ReturnStatus,
                                    oFabricReturnChallan.ReturnDate,
                                    oFabricReturnChallan.BuyerID,
                                    oFabricReturnChallan.StoreID,
                                    oFabricReturnChallan.Remarks,
                                    oFabricReturnChallan.PartyChallanNo,
                                    oFabricReturnChallan.VehicleInfo,
                                    oFabricReturnChallan.GetInNo,
                                    oFabricReturnChallan.ReturnPerson,
                                    (int)eEnumDBOperation,
                                    nUserID);
        }
        public static IDataReader ApproveOrReceive(TransactionContext tc, FabricReturnChallan oFabricReturnChallan, int eEnumDBOperation, Int64 nUserID)
        {
             return tc.ExecuteReader("EXEC [SP_IUD_FabricReturnChallan]"
                                     + "%n,%n,%s,%n,%d,%n,%n,%s,%s,%s,%s,%s,%n,%n",
                                      oFabricReturnChallan.FabricReturnChallanID,
                                    oFabricReturnChallan.FabricDeliveryChallanID,
                                    oFabricReturnChallan.ReturnNo,
                                    (int)oFabricReturnChallan.ReturnStatus,
                                    oFabricReturnChallan.ReturnDate,
                                    oFabricReturnChallan.BuyerID,
                                    oFabricReturnChallan.StoreID,
                                    oFabricReturnChallan.Remarks,
                                    oFabricReturnChallan.PartyChallanNo,
                                    oFabricReturnChallan.VehicleInfo,
                                    oFabricReturnChallan.GetInNo,
                                    oFabricReturnChallan.ReturnPerson,
                                    (int)eEnumDBOperation,
                                    nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricReturnChallan WHERE FabricReturnChallanID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricReturnChallan");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

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
    public class DUDeliveryChallanPackDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, DUDeliveryChallanPack oDUDeliveryChallanPack, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDeliveryChallanPack]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n",
                                   oDUDeliveryChallanPack.DUDeliveryChallanPackID, oDUDeliveryChallanPack.DUDeliveryChallanDetailID, oDUDeliveryChallanPack.DUDeliveryChallanID, oDUDeliveryChallanPack.RouteSheetPackingID, oDUDeliveryChallanPack.QTY, oDUDeliveryChallanPack.BagWeight, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, DUDeliveryChallanPack oDUDeliveryChallanPack, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUDeliveryChallanPack]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n",
                                    oDUDeliveryChallanPack.DUDeliveryChallanPackID, oDUDeliveryChallanPack.DUDeliveryChallanDetailID, oDUDeliveryChallanPack.DUDeliveryChallanID, oDUDeliveryChallanPack.RouteSheetPackingID, oDUDeliveryChallanPack.QTY, oDUDeliveryChallanPack.BagWeight, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DUDeliveryChallanPack WHERE DUDeliveryChallanPackID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DUDeliveryChallanPack");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static void DeletePacks(TransactionContext tc, int DUDeliveryChallanDetailID, string PackIDs)
        {
            tc.ExecuteNonQuery("DELETE FROM DUDeliveryChallanPack WHERE DUDeliveryChallanDetailID = %n AND DUDeliveryChallanPackID NOT IN (SELECT * FROM dbo.SplitInToDataSet(%s,','))", DUDeliveryChallanDetailID, PackIDs);
        }

        #endregion
    }

}

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
    public class KnittingFabricReceiveDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnittingFabricReceiveDetail oKnittingFabricReceiveDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sKnittingFabricReceiveIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnittingFabricReceiveDetail]"
                                   + "%n,%n,%n, %n,%n,%n,%n,%n,%s,%s,%n, %s,%s,%s, %n,%n,%s",
                                   oKnittingFabricReceiveDetail.KnittingFabricReceiveDetailID, oKnittingFabricReceiveDetail.KnittingFabricReceiveID, oKnittingFabricReceiveDetail.KnittingOrderDetailID, oKnittingFabricReceiveDetail.FabricID, oKnittingFabricReceiveDetail.ReceiveStoreID, oKnittingFabricReceiveDetail.LotID, oKnittingFabricReceiveDetail.MUnitID, oKnittingFabricReceiveDetail.Qty, oKnittingFabricReceiveDetail.Remarks, oKnittingFabricReceiveDetail.NewLotNo, oKnittingFabricReceiveDetail.ProcessLossQty, oKnittingFabricReceiveDetail.GSM, oKnittingFabricReceiveDetail.MICDia, oKnittingFabricReceiveDetail.FinishDia, nUserID, (int)eEnumDBOperation, sKnittingFabricReceiveIDs);
        }

        public static void Delete(TransactionContext tc, KnittingFabricReceiveDetail oKnittingFabricReceiveDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sKnittingFabricReceiveIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnittingFabricReceiveDetail]"
                                   + "%n,%n,%n, %n,%n,%n,%n,%n,%s,%s,%n,%s,%s,%s,%n,%n,%s",
                                   oKnittingFabricReceiveDetail.KnittingFabricReceiveDetailID, oKnittingFabricReceiveDetail.KnittingFabricReceiveID, oKnittingFabricReceiveDetail.KnittingOrderDetailID, oKnittingFabricReceiveDetail.FabricID, oKnittingFabricReceiveDetail.ReceiveStoreID, oKnittingFabricReceiveDetail.LotID, oKnittingFabricReceiveDetail.MUnitID, oKnittingFabricReceiveDetail.Qty, oKnittingFabricReceiveDetail.Remarks, oKnittingFabricReceiveDetail.NewLotNo, oKnittingFabricReceiveDetail.ProcessLossQty, oKnittingFabricReceiveDetail.GSM, oKnittingFabricReceiveDetail.MICDia, oKnittingFabricReceiveDetail.FinishDia, nUserID, (int)eEnumDBOperation, sKnittingFabricReceiveIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingFabricReceiveDetail WHERE KnittingFabricReceiveDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM KnittingFabricReceiveDetail");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}

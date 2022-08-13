using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class WUSubContractFabricReceiveDA
    {
        public WUSubContractFabricReceiveDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, WUSubContractFabricReceive oWUSubContractFabricReceive, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_WUSubContractFabricReceive]"
                                    + "%n, %n, %s, %d, %s, %n, %n, %s, %n, %s, %n, %n, %n, %n, %s, %n, %n, %n",
                                    oWUSubContractFabricReceive.WUSubContractFabricReceiveID, oWUSubContractFabricReceive.WUSubContractID, oWUSubContractFabricReceive.ReceiveNo, oWUSubContractFabricReceive.ReceiveDate, oWUSubContractFabricReceive.PartyChallanNo, oWUSubContractFabricReceive.ReceiveStoreID, oWUSubContractFabricReceive.CompositionID, oWUSubContractFabricReceive.Construction, oWUSubContractFabricReceive.LotID, oWUSubContractFabricReceive.NewLotNo, oWUSubContractFabricReceive.MunitID, oWUSubContractFabricReceive.ReceivedQty, oWUSubContractFabricReceive.RollNo, oWUSubContractFabricReceive.ProcessLossQty, oWUSubContractFabricReceive.Remarks, oWUSubContractFabricReceive.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, WUSubContractFabricReceive oWUSubContractFabricReceive, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_WUSubContractFabricReceive]"
                                    + "%n, %n, %s, %d, %s, %n, %n, %s, %n, %s, %n, %n, %n, %n, %s, %n, %n, %n",
                                    oWUSubContractFabricReceive.WUSubContractFabricReceiveID, oWUSubContractFabricReceive.WUSubContractID, oWUSubContractFabricReceive.ReceiveNo, oWUSubContractFabricReceive.ReceiveDate, oWUSubContractFabricReceive.PartyChallanNo, oWUSubContractFabricReceive.ReceiveStoreID, oWUSubContractFabricReceive.CompositionID, oWUSubContractFabricReceive.Construction, oWUSubContractFabricReceive.LotID, oWUSubContractFabricReceive.NewLotNo, oWUSubContractFabricReceive.MunitID, oWUSubContractFabricReceive.ReceivedQty, oWUSubContractFabricReceive.RollNo, oWUSubContractFabricReceive.ProcessLossQty, oWUSubContractFabricReceive.Remarks, oWUSubContractFabricReceive.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader Approve(TransactionContext tc, WUSubContractFabricReceive oWUSubContractFabricReceive, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_WUSubContractFabricReceive]"
                                    + "%n, %n, %s, %d, %s, %n, %n, %s, %n, %s, %n, %n, %n, %n, %s, %n, %n, %n",
                                    oWUSubContractFabricReceive.WUSubContractFabricReceiveID, oWUSubContractFabricReceive.WUSubContractID, oWUSubContractFabricReceive.ReceiveNo, oWUSubContractFabricReceive.ReceiveDate, oWUSubContractFabricReceive.PartyChallanNo, oWUSubContractFabricReceive.ReceiveStoreID, oWUSubContractFabricReceive.CompositionID, oWUSubContractFabricReceive.Construction, oWUSubContractFabricReceive.LotID, oWUSubContractFabricReceive.NewLotNo, oWUSubContractFabricReceive.MunitID, oWUSubContractFabricReceive.ReceivedQty, oWUSubContractFabricReceive.RollNo, oWUSubContractFabricReceive.ProcessLossQty, oWUSubContractFabricReceive.Remarks, oWUSubContractFabricReceive.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WUSubContractFabricReceive WHERE WUSubContractFabricReceiveID=%n", nID);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

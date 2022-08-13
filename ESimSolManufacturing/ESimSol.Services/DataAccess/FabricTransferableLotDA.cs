using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;


namespace ESimSol.Services.DataAccess
{

    public class FabricTransferableLotDA
    {
        public FabricTransferableLotDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricTransferableLot oFabricTransferableLot, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricTransferableLot]"
                                    + "%n,%n,%n,%n,%n,%n",
                                    oFabricTransferableLot.FabricTransferableLotID, oFabricTransferableLot.LotID, oFabricTransferableLot.WorkingUnitID, oFabricTransferableLot.Qty, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricTransferableLot oFabricTransferableLot, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricTransferableLot]"
                                     + "%n,%n,%n,%n,%n,%n",
                                    oFabricTransferableLot.FabricTransferableLotID, oFabricTransferableLot.LotID, oFabricTransferableLot.WorkingUnitID, oFabricTransferableLot.Qty, nUserID, (int)eEnumDBOperation);
        }

        public static void TransferToStore(TransactionContext tc, FabricTransferableLot oFabricTransferableLot, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricTransferRequisitionSlip]" + "%n,%n,%n",
                                    oFabricTransferableLot.WorkingUnitID, oFabricTransferableLot.WorkingUnitID_Recd, nUserID);
        }
        public static void LotAdjustment(TransactionContext tc, FabricTransferableLot oFabricTransferableLot, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AdjustmentRequisitionSlip_Delivery]"
                                     + "%n,%n",
                                    oFabricTransferableLot.WorkingUnitID, nUserID);
        }
        #endregion


        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, Int64 nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricTransferableLot where DBUserID=%n", nUserID);
        }
        #endregion
    }


}

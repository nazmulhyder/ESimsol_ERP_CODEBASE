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

    public class TransferableLotDA
    {
        public TransferableLotDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, TransferableLot oTransferableLot, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TransferableLot]"
                                    + "%n,%n,%n,%n,%n,%n",
                                    oTransferableLot.TransferableLotID, oTransferableLot.LotID, oTransferableLot.WorkingUnitID, oTransferableLot.Qty, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, TransferableLot oTransferableLot, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TransferableLot]"
                                     + "%n,%n,%n,%n,%n,%n",
                                    oTransferableLot.TransferableLotID, oTransferableLot.LotID, oTransferableLot.WorkingUnitID, oTransferableLot.Qty, nUserID, (int)eEnumDBOperation);
        }

        public static void TransferToStore(TransactionContext tc, TransferableLot oTransferableLot, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TransferRequisitionSlip_DeliveryToOther]"
                                     + "%n,%n,%n",
                                    oTransferableLot.WorkingUnitID, oTransferableLot.WorkingUnitID_Recd, nUserID);
        }
        public static void LotAdjustment(TransactionContext tc, TransferableLot oTransferableLot, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AdjustmentRequisitionSlip_Delivery]"
                                     + "%n,%n",
                                    oTransferableLot.WorkingUnitID, nUserID);
        }
        #endregion
      

        #region Get & Exist Function
   
        public static IDataReader Gets(TransactionContext tc, Int64 nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TransferableLot where DBUserID=%n",nUserID);
        }
        #endregion
    }  
    
   
}

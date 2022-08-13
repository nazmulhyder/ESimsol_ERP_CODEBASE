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
    public class DUDeliveryStockDA
    {
        public DUDeliveryStockDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nOrderType, int nWorkingUnitID, string sSQL)
        {
            return tc.ExecuteReader("EXEC [sp_DUDeliveryStock_SubFinishing]" + "%n,%n, %s", nOrderType, nWorkingUnitID, sSQL);
        }
        public static IDataReader GetsAvalnDelivery(TransactionContext tc, int nOrderType, int nWorkingUnitID, string sSQL)
        {
            return tc.ExecuteReader("EXEC [sp_DUDeliveryStock_AvalnDelivery]" + "%n,%n, %s", nOrderType, nWorkingUnitID, sSQL);
        }
        public static void SendToRequsitionToDelivery(TransactionContext tc, DUDeliveryStock oDUDeliveryStock, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TransferRequisitionSlip_DeliveryStoreAutoRed]" + " %n, %n, %n",
                                     oDUDeliveryStock.LotID, oDUDeliveryStock.WorkingUnitID,  nUserID);
        }

        #endregion
    }  
    
   
}

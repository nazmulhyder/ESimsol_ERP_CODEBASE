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
    public class DUDeliveryChallanDetailDA
    {
        public DUDeliveryChallanDetailDA() { }

        #region Insert Update Delete Function


        public static IDataReader InsertUpdate(TransactionContext tc, DUDeliveryChallanDetail oDCD, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDUDeliveryChallanDetaillIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDeliveryChallanDetail]" + "%n, %n, %n, %n,%n, %n, %n,%n,%n,%n, %s,%s,%n,%n",
                     oDCD.DUDeliveryChallanDetailID, oDCD.DUDeliveryChallanID, oDCD.DODetailID, oDCD.OrderID, oDCD.PTUID, oDCD.DyeingOrderDetailID, oDCD.LotID, oDCD.Qty, oDCD.BagQty, oDCD.HanksCone, oDCD.Note, oDCD.GYLotNo, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, DUDeliveryChallanDetail oDCD, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDUDeliveryChallanDetaillIDs)
          {
              tc.ExecuteNonQuery("EXEC [SP_IUD_DUDeliveryChallanDetail]" + "%n, %n, %n,%n, %n, %n, %n,%n,%n,%n, %s,%s,%n,%n",
                                         oDCD.DUDeliveryChallanDetailID, oDCD.DUDeliveryChallanID, oDCD.DODetailID, oDCD.OrderID, oDCD.PTUID,oDCD.DyeingOrderDetailID, oDCD.LotID, oDCD.Qty, oDCD.BagQty, oDCD.HanksCone, oDCD.Note,oDCD.GYLotNo, nUserID, (int)eEnumDBOperation);
          }

        

        #endregion

     

        #region Get & Exist Function
        #region Get Lot
        public static IDataReader Gets_Lot(TransactionContext tc, int nWorkingUnitID, int nDODetailID, int nPTUID,int nDyeingOrderDetailID, int nLotID )
        {
            return tc.ExecuteReader("EXEC [SP_RPT_GetsLotsForChallan] %n,%n,%n,%n,%n", nWorkingUnitID, nDODetailID,nPTUID,nDyeingOrderDetailID, nLotID);
        }
        #endregion
        public static IDataReader Get(int nID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUDeliveryChallanDetail WHERE DUDeliveryChallanDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nDUDeliveryChallan)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUDeliveryChallanDetail WHERE DUDeliveryChallanID=%n", nDUDeliveryChallan);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void UpdateQty(TransactionContext tc, double nTotalQty, double nTotalBagQty, int DUDeliveryChallanDetailID)
        {
            tc.ExecuteNonQuery("UPDATE DUDeliveryChallanDetail SET QTY=%n, BagQty=%n WHERE DUDeliveryChallanDetailID=%n", nTotalQty, nTotalBagQty, DUDeliveryChallanDetailID);
        }
      
        #endregion
    }
}

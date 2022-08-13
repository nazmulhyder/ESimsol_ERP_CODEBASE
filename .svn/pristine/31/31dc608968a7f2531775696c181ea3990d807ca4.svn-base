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
    public class DUClaimOrderDetailDA
    {
        public DUClaimOrderDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUClaimOrderDetail oDUCO, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDUClaimOrderDetaillIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUClaimOrderDetail]" + "%n, %n, %n,%n, %n, %n,%n,%s,%s,%s,%s,%d, %n,%n,%s",
                       oDUCO.DUClaimOrderDetailID, oDUCO.DUClaimOrderID, oDUCO.DyeingOrderDetailID, oDUCO.ParentDODetailID,oDUCO.ClaimReasonID, oDUCO.Qty,  oDUCO.LotID, oDUCO.ChallanNo,oDUCO.BatchNo, oDUCO.Note,oDUCO.NoOfCone,NullHandler.GetNullValue(oDUCO.DeliveryDate),  nUserID, (int)eEnumDBOperation, sDUClaimOrderDetaillIDs);
        }
        public static void Delete(TransactionContext tc, DUClaimOrderDetail oDUCO, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDUClaimOrderDetaillIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUClaimOrderDetail]" + "%n, %n, %n,%n, %n, %n,%n,%s,%s,%s,%s,%d, %n,%n,%s",
                                    oDUCO.DUClaimOrderDetailID, oDUCO.DUClaimOrderID, oDUCO.DyeingOrderDetailID, oDUCO.ParentDODetailID, oDUCO.ClaimReasonID, oDUCO.Qty, oDUCO.LotID, oDUCO.ChallanNo, oDUCO.BatchNo, oDUCO.Note, oDUCO.NoOfCone, NullHandler.GetNullValue(oDUCO.DeliveryDate), nUserID, (int)eEnumDBOperation, sDUClaimOrderDetaillIDs);
        }
        #endregion

        #region Get & Exist Function
        
        public static IDataReader Get( TransactionContext tc,int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUClaimOrderDetail WHERE DUClaimOrderDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nDUClaimOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUClaimOrderDetail WHERE DUClaimOrderID=%n", nDUClaimOrderID);
        }
        public static IDataReader Gets( TransactionContext tc,string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

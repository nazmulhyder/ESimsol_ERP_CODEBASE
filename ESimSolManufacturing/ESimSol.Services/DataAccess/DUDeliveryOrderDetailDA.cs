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
    public class DUDeliveryOrderDetailDA
    {
        public DUDeliveryOrderDetailDA() { }

        #region Insert Update Delete Function


        public static IDataReader InsertUpdate(TransactionContext tc, DUDeliveryOrderDetail oDOD, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDUDeliveryOrderDetaillIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDeliveryOrderDetail]" + "%n, %n, %n, %n, %n, %n, %n, %s,%n,%n,%s",
                     oDOD.DUDeliveryOrderDetailID, oDOD.DUDeliveryOrderID, oDOD.ProductID, oDOD.ExportSCDetailID, oDOD.DyeingOrderDetailID, oDOD.DyeingOrderDetailID, oDOD.Qty, oDOD.Note, nUserID, (int)eEnumDBOperation, sDUDeliveryOrderDetaillIDs);
        }

         public static void Delete(TransactionContext tc, DUDeliveryOrderDetail oDOD, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDUDeliveryOrderDetaillIDs)
          {
              tc.ExecuteNonQuery("EXEC [SP_IUD_DUDeliveryOrderDetail]" + "%n, %n, %n, %n, %n,%n, %n, %s,%n,%n,%s",
                                         oDOD.DUDeliveryOrderDetailID, oDOD.DUDeliveryOrderID, oDOD.ProductID, oDOD.ExportSCDetailID, oDOD.DyeingOrderDetailID, oDOD.DyeingOrderDetailID, oDOD.Qty, oDOD.Note, nUserID, (int)eEnumDBOperation, sDUDeliveryOrderDetaillIDs);
          }

        

        #endregion

       

        #region Get & Exist Function

        public static IDataReader Get(int nDEODID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUDeliveryOrderDetail WHERE DUDeliveryOrderDetailID=%n", nDEODID);
        }
        public static IDataReader Gets(TransactionContext tc, int nDUDeliveryOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUDeliveryOrderDetail WHERE DUDeliveryOrderID=%n", nDUDeliveryOrderID);
        }

      
      
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

      
        #endregion
    }
}

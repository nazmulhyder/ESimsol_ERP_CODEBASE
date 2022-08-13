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
    public class TwistingDetailDA
    {
        public TwistingDetailDA() { }

        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, TwistingDetail oDOD, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sTwistingDetaillIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TwistingDetail]"
                    + "%n,%n,%n,%n,%n, %n,%n,%n,%n, %b, %n,%s, %n, %s, %n,%n,%s",
                     oDOD.TwistingDetailID, oDOD.TwistingID, oDOD.ProductID, oDOD.LotID, oDOD.MUnitID, oDOD.Qty, oDOD.UnitPrice, oDOD.BagCount, oDOD.CurrencyID, oDOD.IsLotMendatory, oDOD.InOutTypeInt, oDOD.Note, oDOD.DyeingOrderDetailID, oDOD.LotNo, nUserID, (int)eEnumDBOperation, sTwistingDetaillIDs);
        }

         public static void Delete(TransactionContext tc, TwistingDetail oDOD, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sTwistingDetaillIDs)
          {
              tc.ExecuteNonQuery("EXEC [SP_IUD_TwistingDetail]"
                    + "%n,%n,%n,%n,%n, %n,%n,%n,%n, %b, %n,%s, %n, %s, %n,%n,%s",
                     oDOD.TwistingDetailID, oDOD.TwistingID, oDOD.ProductID, oDOD.LotID, oDOD.MUnitID, oDOD.Qty, oDOD.UnitPrice, oDOD.BagCount, oDOD.CurrencyID, oDOD.IsLotMendatory, oDOD.InOutTypeInt, oDOD.Note, oDOD.DyeingOrderDetailID, oDOD.LotNo, nUserID, (int)eEnumDBOperation, sTwistingDetaillIDs);
          }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nDEODID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TwistingDetail WHERE TwistingDetailID=%n", nDEODID);
        }
        public static IDataReader Gets(TransactionContext tc, int nTwistingID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TwistingDetail WHERE TwistingID=%n", nTwistingID);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}

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
    public class KnittingOrderDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnittingOrderDetail oKnittingOrderDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sKnittingOrderDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnittingOrderDetail]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%n,%n,%s,%s,%s,%n,%n,%n,%s",
                                   oKnittingOrderDetail.KnittingOrderDetailID, oKnittingOrderDetail.KnittingOrderID, oKnittingOrderDetail.StyleID, oKnittingOrderDetail.OrderQty, oKnittingOrderDetail.OrderUnitID, oKnittingOrderDetail.PAM, oKnittingOrderDetail.FabricID, oKnittingOrderDetail.GSMID, oKnittingOrderDetail.MICDiaID, oKnittingOrderDetail.FinishDiaID, oKnittingOrderDetail.ColorID, oKnittingOrderDetail.StratchLength, oKnittingOrderDetail.MUnitID, oKnittingOrderDetail.Qty, oKnittingOrderDetail.UnitPrice, oKnittingOrderDetail.Amount, oKnittingOrderDetail.Remarks, oKnittingOrderDetail.BrandName, oKnittingOrderDetail.LotNo, oKnittingOrderDetail.KnitDyeingProgramDetailID, nUserID, (int)eEnumDBOperation, sKnittingOrderDetailIDs);
        }

        public static void Delete(TransactionContext tc, KnittingOrderDetail oKnittingOrderDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sKnittingOrderDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnittingOrderDetail]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%n,%n,%s,%s,%s,%n,%n,%n,%s",
                                   oKnittingOrderDetail.KnittingOrderDetailID, oKnittingOrderDetail.KnittingOrderID, oKnittingOrderDetail.StyleID, oKnittingOrderDetail.OrderQty, oKnittingOrderDetail.OrderUnitID, oKnittingOrderDetail.PAM, oKnittingOrderDetail.FabricID, oKnittingOrderDetail.GSMID, oKnittingOrderDetail.MICDiaID, oKnittingOrderDetail.FinishDiaID, oKnittingOrderDetail.ColorID, oKnittingOrderDetail.StratchLength, oKnittingOrderDetail.MUnitID, oKnittingOrderDetail.Qty, oKnittingOrderDetail.UnitPrice, oKnittingOrderDetail.Amount, oKnittingOrderDetail.Remarks, oKnittingOrderDetail.BrandName, oKnittingOrderDetail.LotNo, oKnittingOrderDetail.KnitDyeingProgramDetailID, nUserID, (int)eEnumDBOperation, sKnittingOrderDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingOrderDetail WHERE KnittingOrderDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingOrderDetail");
        }
        public static IDataReader Gets(TransactionContext tc, Int64 id)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingOrderDetail WHERE KnittingOrderID = %n", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}

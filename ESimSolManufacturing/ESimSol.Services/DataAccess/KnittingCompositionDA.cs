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
    public class KnittingCompositionDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnittingComposition oKnittingComposition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnittingComposition]"
                                   + "%n,%n,%n,%n,%n,%n,%s,%s,%s,%n,%n",
                                   oKnittingComposition.KnittingCompositionID, oKnittingComposition.KnittingOrderDetailID, oKnittingComposition.FabricID, oKnittingComposition.YarnID, oKnittingComposition.Qty, oKnittingComposition.RatioInPercent, oKnittingComposition.LotNo, oKnittingComposition.ColorName, oKnittingComposition.BrandName, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, KnittingComposition oKnittingComposition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnittingComposition]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%s,%s,%n,%n",
                                    oKnittingComposition.KnittingCompositionID, oKnittingComposition.KnittingOrderDetailID, oKnittingComposition.FabricID, oKnittingComposition.YarnID, oKnittingComposition.Qty, oKnittingComposition.RatioInPercent, oKnittingComposition.LotNo, oKnittingComposition.ColorName, oKnittingComposition.BrandName, nUserID, (int)eEnumDBOperation);
        }
        public static void DeleteByIDs(TransactionContext tc, int nKnittingOrderDetailID, string sKnittingCompositionIDs, Int64 nUserID)
        {
            tc.ExecuteNonQuery("DELETE FROM KnittingComposition WHERE KnittingOrderDetailID=" + nKnittingOrderDetailID + "AND KnittingCompositionID NOT IN (" + sKnittingCompositionIDs + ")");
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingComposition WHERE KnittingCompositionID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM KnittingComposition");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}

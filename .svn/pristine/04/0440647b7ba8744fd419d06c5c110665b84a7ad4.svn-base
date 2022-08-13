using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricStickerDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricSticker oFabricSticker, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSticker]"
                                    + "%n, %s, %s, %s, %n, %s, %s, %s, %n, %d, %n, %n, %n, %n, %s, %s, %n, %n",
                                    oFabricSticker.FabricStickerID, oFabricSticker.Title, oFabricSticker.FabricMillName, oFabricSticker.FabricArticleNo, oFabricSticker.Composition, oFabricSticker.Construction, oFabricSticker.Width, oFabricSticker.Weight, oFabricSticker.FinishType, oFabricSticker.StickerDate, oFabricSticker.Price, oFabricSticker.PrintCount, oFabricSticker.FabricDesignID, oFabricSticker.FabricWeave, oFabricSticker.Email, oFabricSticker.Phone, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, FabricSticker oFabricSticker, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricSticker]"
                                    + "%n, %s, %s, %s, %n, %s, %s, %s, %n, %d, %n, %n, %n, %n, %s, %s, %n, %n",
                                    oFabricSticker.FabricStickerID, oFabricSticker.Title, oFabricSticker.FabricMillName, oFabricSticker.FabricArticleNo, oFabricSticker.Composition, oFabricSticker.Construction, oFabricSticker.Width, oFabricSticker.Weight, oFabricSticker.FinishType, oFabricSticker.StickerDate, oFabricSticker.Price, oFabricSticker.PrintCount, oFabricSticker.FabricDesignID, oFabricSticker.FabricWeave, oFabricSticker.Email, oFabricSticker.Phone, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSticker WHERE FabricStickerID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSticker");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class HangerStickerDA 
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, HangerSticker oHangerSticker, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_HangerSticker]"
                                    + "%n,%s,%s,%s,%s,%s,%s,%s,%n ,%s,%s,%n,%n,%n",
                                    oHangerSticker.HangerStickerID
                                    ,oHangerSticker.ART
                                    , oHangerSticker.Supplier
                                    , oHangerSticker.Composition
                                    , oHangerSticker.Construction
                                    , oHangerSticker.Finishing
                                    , oHangerSticker.MOQ
                                    , oHangerSticker.Remarks, oHangerSticker.Price, oHangerSticker.Date, oHangerSticker.Width  
                                    , oHangerSticker.FSCDID
                                    ,(int)eEnumDBOperation
                                    ,nUserID);
        }

        public static void Delete(TransactionContext tc, HangerSticker oHangerSticker, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_HangerSticker]"
                                    + "%n,%s,%s,%s,%s,%s,%s,%s,%n ,%s,%s,%n,%n,%n",
                                    oHangerSticker.HangerStickerID
                                    , oHangerSticker.ART
                                    , oHangerSticker.Supplier
                                    , oHangerSticker.Composition
                                    , oHangerSticker.Construction
                                    , oHangerSticker.Finishing
                                    , oHangerSticker.MOQ
                                    , oHangerSticker.Remarks, oHangerSticker.Price, oHangerSticker.Date, oHangerSticker.Width
                                    , oHangerSticker.FSCDID
                                    , (int)eEnumDBOperation
                                    , nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM HangerSticker WHERE HangerStickerID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM HangerSticker  ORDER BY ART ASC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

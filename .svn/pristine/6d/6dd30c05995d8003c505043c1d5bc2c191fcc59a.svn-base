using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{


    public class DevelopmentRecapSizeColorRatioDA
    {
        public DevelopmentRecapSizeColorRatioDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DevelopmentRecapSizeColorRatio oDevelopmentRecapSizeColorRatio, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sDevelopmentRecapSizeColorRatioIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DevelopmentRecapSizeColorRatio]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%s", oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID, oDevelopmentRecapSizeColorRatio.DevelopmentRecapDetailID, oDevelopmentRecapSizeColorRatio.ColorID, oDevelopmentRecapSizeColorRatio.SizeID, oDevelopmentRecapSizeColorRatio.Qty, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, DevelopmentRecapSizeColorRatio oDevelopmentRecapSizeColorRatio, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sDevelopmentRecapSizeColorRatioIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DevelopmentRecapSizeColorRatio]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%s", oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID, oDevelopmentRecapSizeColorRatio.DevelopmentRecapDetailID, oDevelopmentRecapSizeColorRatio.ColorID, oDevelopmentRecapSizeColorRatio.SizeID, oDevelopmentRecapSizeColorRatio.Qty, nUserID, (int)eEnumDBOperation, sDevelopmentRecapSizeColorRatioIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DevelopmentRecapSizeColorRatio WHERE DevelopmentRecapSizeColorRatioID=%n", nID);
        }

        public static IDataReader Gets(int nDevelopmentRecapDetailID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DevelopmentRecapSizeColorRatio WHERE DevelopmentRecapDetailID =%n Order BY ColorID", nDevelopmentRecapDetailID);
        }



        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    

}

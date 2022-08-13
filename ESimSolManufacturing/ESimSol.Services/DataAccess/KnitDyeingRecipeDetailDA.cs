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
    public class KnitDyeingRecipeDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingRecipeDetail oKnitDyeingRecipeDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingRecipeDetail]"
                                   + "%n, %n, %n, %n,%n,%n,%n,%s",
                                  oKnitDyeingRecipeDetail.KnitDyeingRecipeDetailID, oKnitDyeingRecipeDetail.KnitDyeingRecipeID, oKnitDyeingRecipeDetail.ProductID, oKnitDyeingRecipeDetail.MUnitType, oKnitDyeingRecipeDetail.ReqQty, nUserID, (int)eEnumDBOperation, sIDs);
        }

        public static void Delete(TransactionContext tc, KnitDyeingRecipeDetail oKnitDyeingRecipeDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingRecipeDetail]"
                                    + "%n, %n, %n, %n,%n,%n,%n,%s",
                                    oKnitDyeingRecipeDetail.KnitDyeingRecipeDetailID, oKnitDyeingRecipeDetail.KnitDyeingRecipeID, oKnitDyeingRecipeDetail.ProductID, oKnitDyeingRecipeDetail.MUnitType, oKnitDyeingRecipeDetail.ReqQty, nUserID, (int)eEnumDBOperation, sIDs);
        }

        #endregion

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingRecipeDetail WHERE KnitDyeingRecipeID = %n Order By KnitDyeingRecipeDetailID", id);
        } 
    }
}

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
    public class KnitDyeingRecipeDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingRecipe oKnitDyeingRecipe, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingRecipe]"
                                   + "%n,%s,%s,%s,%n,%b,%n,%n",
                                   oKnitDyeingRecipe.KnitDyeingRecipeID, oKnitDyeingRecipe.RecipeCode, oKnitDyeingRecipe.RecipeName, oKnitDyeingRecipe.Note, oKnitDyeingRecipe.BUID, oKnitDyeingRecipe.IsActive, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, KnitDyeingRecipe oKnitDyeingRecipe, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingRecipe]"
                                    + "%n,%s,%s,%s,%n,%b,%n,%n",
                                  oKnitDyeingRecipe.KnitDyeingRecipeID, oKnitDyeingRecipe.RecipeCode, oKnitDyeingRecipe.RecipeName, oKnitDyeingRecipe.Note, oKnitDyeingRecipe.BUID, oKnitDyeingRecipe.IsActive, nUserID, (int)eEnumDBOperation);
        }


        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingRecipe WHERE KnitDyeingRecipeID=%n", nID);
        }
        public static void Activity(TransactionContext tc, KnitDyeingRecipe oKnitDyeingRecipe, Int64 nUserId)
        {
            tc.ExecuteNonQuery("UPDATE KnitDyeingRecipe SET IsActive = %b WHERE KnitDyeingRecipeID = %n", oKnitDyeingRecipe.IsActive, oKnitDyeingRecipe.KnitDyeingRecipeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}

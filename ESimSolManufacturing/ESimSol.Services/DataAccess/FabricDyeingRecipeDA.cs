using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricDyeingRecipeDA
    {
        public FabricDyeingRecipeDA() { }
         
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricDyeingRecipe oFabricDyeingRecipe, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricDyeingRecipe]"
                                    + "%n, %n, %n, %n, %n",
                                    oFabricDyeingRecipe.FabricDyeingRecipeID, oFabricDyeingRecipe.FEOSID, oFabricDyeingRecipe.DyeingSolutionID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, string sSQL)
        {
            tc.ExecuteNonQuery(sSQL);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricDyeingRecipe WHERE FabricDyeingRecipeID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricDyeingRecipe Order By [Name]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_FabricDyeingRecipe
        }
        #endregion
    }  
}


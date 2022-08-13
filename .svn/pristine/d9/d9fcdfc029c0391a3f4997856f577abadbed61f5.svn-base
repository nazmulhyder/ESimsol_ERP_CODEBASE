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
    public class RecipeDA
    {
        public RecipeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Recipe oRecipe, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Recipe]"
                                    + "%n,%s,%s,%n,%b,%s,%n,%s,%n, %n,%n",
                                    oRecipe.RecipeID,oRecipe.RecipeCode,oRecipe.RecipeName,oRecipe.RecipeType, oRecipe.IsActive,oRecipe.Note, oRecipe.BUID,oRecipe.ColorName, oRecipe.ProductNature,nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, Recipe oRecipe, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Recipe]"
                                    + "%n,%s,%s,%n,%b,%s,%n,%s,%n, %n,%n",
                                    oRecipe.RecipeID, oRecipe.RecipeCode, oRecipe.RecipeName, oRecipe.RecipeType, oRecipe.IsActive, oRecipe.Note, oRecipe.BUID, oRecipe.ColorName, oRecipe.ProductNature, nUserId, (int)eEnumDBOperation);
        }

        public static void ActiveInActive(TransactionContext tc, Recipe oRecipe, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update Recipe SEt IsActive = %b WHERE RecipeID = %n", oRecipe.IsActive, oRecipe.RecipeID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM Recipe WHERE RecipeID=%n", nID);
        }

        public static IDataReader GetMaxRecipe(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Recipe WHERE RecipeID=(SELECT MAX(RecipeID) FROM Recipe)");
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Recipe ORDER BY RecipeID");
        }

        public static IDataReader GetsByBUWithProductNature(int nBUID, int nProductNature, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Recipe  WHERE BUID = " + nBUID + " AND ProductNature = " + nProductNature + "  ORDER BY RecipeID");
        }
        public static IDataReader GetsByTypeWithBUAndNature(int nRcipeType,int nBUID, int nProductNature, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Recipe  WHERE RecipeType=" + nRcipeType + "AND  BUID = " + nBUID + " AND ProductNature = " + nProductNature + " AND IsActive=1  ORDER BY RecipeID");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

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
    public class RecipeDetailDA
    {
        public RecipeDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, RecipeDetail oRecipeDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RecipeDetail]"
                                    + "%n, %n, %n, %n, %n, %s, %n, %n, %s",
                                    oRecipeDetail.RecipeDetailID, oRecipeDetail.RecipeID, oRecipeDetail.ProductID, oRecipeDetail.QtyTypeInt, oRecipeDetail.QtyInPercent, oRecipeDetail.Note, nUserId, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, RecipeDetail oRecipeDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RecipeDetail]"
                                    + "%n, %n, %n, %n, %n, %s, %n, %n, %s",
                                    oRecipeDetail.RecipeDetailID, oRecipeDetail.RecipeID, oRecipeDetail.ProductID, oRecipeDetail.QtyTypeInt, oRecipeDetail.QtyInPercent, oRecipeDetail.Note, nUserId, (int)eEnumDBOperation, sDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RecipeDetail WHERE RecipeDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RecipeDetail");
        }

        public static IDataReader Gets(TransactionContext tc, int nRecipeID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RecipeDetail WHERE RecipeID=%n", nRecipeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNLabdipRecipeDA
    {
        public FNLabdipRecipeDA() { }


        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, FNLabdipRecipe oFNLabdipRecipe, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNLabdipRecipe]" + "%n, %n, %n, %n, %s, %b,%b, %n, %n, %n", oFNLabdipRecipe.FNLabdipRecipeID, oFNLabdipRecipe.FNLabdipShadeID, oFNLabdipRecipe.ProductID, oFNLabdipRecipe.Qty, oFNLabdipRecipe.Note, oFNLabdipRecipe.IsDyes, oFNLabdipRecipe.IsGL, oFNLabdipRecipe.FabricOrderTypeInInt, nUserID, nDBOperation);
        }

        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNLabdipRecipe WHERE FNLabdipRecipeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, int nLabdipDetailID, int nShade)
        {
            return tc.ExecuteReader("Select * from View_FNLabdipRecipe where LabdipShadeID in (Select LabdipShade.LabdipShadeID from LabdipShade where ShadeID=%n and LabdipDetailID=%n) order by IsDyes,ProductID",nShade, nLabdipDetailID);
        }

        #endregion
    }
}

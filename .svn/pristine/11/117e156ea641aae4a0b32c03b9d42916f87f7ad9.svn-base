using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LabdipRecipeDA
    {
        public LabdipRecipeDA() { }


        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, LabdipRecipe oLabdipRecipe, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabdipRecipe]" + "%n, %n, %n, %n, %s, %b,%b, %n, %n", oLabdipRecipe.LabdipRecipeID, oLabdipRecipe.LabdipShadeID, oLabdipRecipe.ProductID, oLabdipRecipe.Qty, oLabdipRecipe.Note, oLabdipRecipe.IsDyes,oLabdipRecipe.IsGL, nUserID, nDBOperation);
        }

        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabdipRecipe WHERE LabdipRecipeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, int nLabdipDetailID, int nShade)
        {
            return tc.ExecuteReader("Select * from View_LabdipRecipe where LabdipShadeID in (Select LabdipShade.LabdipShadeID from LabdipShade where ShadeID=%n and LabdipDetailID=%n) order by IsDyes,ProductID",nShade, nLabdipDetailID);
        }

        public static IDataReader UpdateLot(TransactionContext tc, LabdipRecipe oLabdipRecipe, int nUserID)
        {
            string sSQL1 = SQLParser.MakeSQL("Update LabdipRecipe set LotID=%n where LabdipRecipeID=%n", oLabdipRecipe.LotID, oLabdipRecipe.LabdipRecipeID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_LabdipRecipe WHERE LabdipRecipeID=%n", oLabdipRecipe.LabdipRecipeID);

        }

        #endregion
    }
}

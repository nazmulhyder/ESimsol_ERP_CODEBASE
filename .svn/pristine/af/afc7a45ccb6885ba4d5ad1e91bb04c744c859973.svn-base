using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNRecipeDA
    {
        public FNRecipeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FNRecipe oFNRecipe, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNRecipe]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %s, %s, %s, %s, %s, %b, %s, %n, %n, %n ",
                                    oFNRecipe.FNRecipeID,       oFNRecipe.FSCDID,       oFNRecipe.FNTPID,   oFNRecipe.ProductTypeInInt, oFNRecipe.ProductID, 
                                    oFNRecipe.GL,               oFNRecipe.QtyColor,     oFNRecipe.Qty,      oFNRecipe.BathSize,         oFNRecipe.Note,
                                    oFNRecipe.PadderPressure,   oFNRecipe.Temp,         oFNRecipe.Speed,    oFNRecipe.PH,               oFNRecipe.Flem,
                                    oFNRecipe.IsProcess,        oFNRecipe.CausticStrength,                  (int)oFNRecipe.ShadeID,          (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, FNRecipe oFNRecipe, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNRecipe]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %s, %s, %s, %s, %s, %b, %s, %n, %n, %n ",
                                    oFNRecipe.FNRecipeID,      oFNRecipe.FSCDID,        oFNRecipe.FNTPID, oFNRecipe.ProductTypeInInt, oFNRecipe.ProductID,
                                    oFNRecipe.GL,              oFNRecipe.QtyColor,      oFNRecipe.Qty,    oFNRecipe.BathSize,         oFNRecipe.Note,
                                    oFNRecipe.PadderPressure,  oFNRecipe.Temp,          oFNRecipe.Speed,  oFNRecipe.PH,               oFNRecipe.Flem,
                                    oFNRecipe.IsProcess,       oFNRecipe.CausticStrength,                 oFNRecipe.ShadeID,          (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader CopyOrder(TransactionContext tc, int nFNLabDipDetailID, bool IsFromLabDip, int nFSCDID, int nFromFSCDID, int nShadeID, long nUserID, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNRecipe_Copy]"
                                    + "%n, %n, %n, %n, %b, %n, %n",
                                    nFNLabDipDetailID, nFSCDID, nFromFSCDID, nShadeID, IsFromLabDip, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNRecipe WHERE FNRecipeID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNRecipe Order By [Name]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}


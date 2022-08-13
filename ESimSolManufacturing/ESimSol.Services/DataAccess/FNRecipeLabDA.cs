using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNRecipeLabDA
    {
        public FNRecipeLabDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FNRecipeLab oFNRecipeLab, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNRecipeLab]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %s, %s, %s, %s, %s, %b, %s, %n, %n, %n",
                                    oFNRecipeLab.FNRecipeLabID,  oFNRecipeLab.FNLDDID,    oFNRecipeLab.FNTPID,       oFNRecipeLab.ProductTypeInInt, oFNRecipeLab.ProductID, 
                                    oFNRecipeLab.GL,             oFNRecipeLab.QtyColor,   oFNRecipeLab.Qty,          oFNRecipeLab.BathSize,         oFNRecipeLab.Note,
                                    oFNRecipeLab.PadderPressure, oFNRecipeLab.Temp,       oFNRecipeLab.Speed,        oFNRecipeLab.PH,               oFNRecipeLab.Flem,     
                                    oFNRecipeLab.IsProcess,      oFNRecipeLab.CausticStrength,                       (int)oFNRecipeLab.ShadeID,     (int)eEnumDBOperation,         nUserID);
        }

        public static void Delete(TransactionContext tc, FNRecipeLab oFNRecipeLab, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNRecipeLab]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %s, %s, %s, %s, %s, %b, %s, %n, %n, %n",
                                    oFNRecipeLab.FNRecipeLabID,  oFNRecipeLab.FNLDDID,   oFNRecipeLab.FNTPID,         oFNRecipeLab.ProductTypeInInt,  oFNRecipeLab.ProductID,
                                    oFNRecipeLab.GL,             oFNRecipeLab.QtyColor,  oFNRecipeLab.Qty,            oFNRecipeLab.BathSize,          oFNRecipeLab.Note,
                                    oFNRecipeLab.PadderPressure, oFNRecipeLab.Temp,      oFNRecipeLab.Speed,          oFNRecipeLab.PH,                oFNRecipeLab.Flem,
                                    oFNRecipeLab.IsProcess,      oFNRecipeLab.CausticStrength,                        (int)oFNRecipeLab.ShadeID,      (int)eEnumDBOperation,       nUserID);
        }
        public static IDataReader CopyShadeSave(TransactionContext tc, int nFNLDDID, int nShadeID, int nShadeIDCopy, int nFNLabDipDetailID, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNRecipeLabShade_Copy] "
                                    + " %n, %n, %n, %n, %n, %n", nFNLDDID, nShadeID, nShadeIDCopy, nFNLabDipDetailID, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNRecipeLab WHERE FNRecipeLabID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNRecipeLab Order By [Name]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}


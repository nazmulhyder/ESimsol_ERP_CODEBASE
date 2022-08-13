using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class DyeingSolutionTempletDA
    {
        public DyeingSolutionTempletDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, DyeingSolutionTemplet oDyeingSolutionTemplet, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingSolutionTemplet] %n,%n,%n,%n,%b,%s,%n,%n,%s,%n,%n,%n,%n",
                   oDyeingSolutionTemplet.DSTID, oDyeingSolutionTemplet.DyeingSolutionID, oDyeingSolutionTemplet.ProcessID,
                   oDyeingSolutionTemplet.ParentID, oDyeingSolutionTemplet.IsDyesChemical,
                   oDyeingSolutionTemplet.TempTime, oDyeingSolutionTemplet.GL,
                   oDyeingSolutionTemplet.Percentage, oDyeingSolutionTemplet.Note,(int)oDyeingSolutionTemplet.ProductType, (int)oDyeingSolutionTemplet.RecipeCalType,
                   nUserID, nDBOperation);
        }


        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nDyeingSolutionTempletID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingSolutionTemplet WHERE DSTID=%n", nDyeingSolutionTempletID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingSolutionTemplet");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void UpdateSequence(TransactionContext tc, DyeingSolutionTemplet oDyeingSolutionTemplet)
        {
            tc.ExecuteNonQuery("Update DyeingSolutionTemplet SET Sequence = %n WHERE DSTID = %n", oDyeingSolutionTemplet.Sequence, oDyeingSolutionTemplet.DSTID);
        }

        #endregion
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNLabdipShadeDA
    {
        public FNLabdipShadeDA() { }


        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, FNLabdipShade oFNLabdipShade, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNLabdipShade]" + " %n, %n, %n, %n,%n, %s, %n, %n", oFNLabdipShade.FNLabdipShadeID, oFNLabdipShade.FNLabDipDetailID, (int)oFNLabdipShade.ShadeID, oFNLabdipShade.ShadePercentage,oFNLabdipShade.Qty, oFNLabdipShade.Note, nUserID, nDBOperation);
        }

        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNLabdipShade WHERE FNLabdipShadeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}

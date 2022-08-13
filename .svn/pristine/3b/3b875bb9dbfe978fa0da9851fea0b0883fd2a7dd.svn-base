using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LabdipShadeDA
    {
        public LabdipShadeDA() { }


        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, LabdipShade oLabdipShade, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabdipShade]" + " %n, %n, %n, %n,%n, %s, %n, %n", oLabdipShade.LabdipShadeID, oLabdipShade.LabdipDetailID, (int)oLabdipShade.ShadeID, oLabdipShade.ShadePercentage,oLabdipShade.Qty, oLabdipShade.Note, nUserID, nDBOperation);
        }

        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabdipShade WHERE LabdipShadeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}

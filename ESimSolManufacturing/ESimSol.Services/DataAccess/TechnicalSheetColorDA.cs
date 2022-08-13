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
    public class TechnicalSheetColorDA
    {
        public TechnicalSheetColorDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TechnicalSheetColor oTechnicalSheetColor, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TechnicalSheetColor]"
                                    + "%n, %n, %n, %b, %s, %n,%s, %n, %n",
                                    oTechnicalSheetColor.TechnicalSheetColorID, oTechnicalSheetColor.TechnicalSheetID, oTechnicalSheetColor.ColorCategoryID, oTechnicalSheetColor.IsSelected, oTechnicalSheetColor.Note, oTechnicalSheetColor.Sequence,oTechnicalSheetColor.PantonNo,   nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, TechnicalSheetColor oTechnicalSheetColor, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TechnicalSheetColor]"
                                    + "%n, %n, %n, %b, %s, %n,%s, %n, %n",
                                    oTechnicalSheetColor.TechnicalSheetColorID, oTechnicalSheetColor.TechnicalSheetID, oTechnicalSheetColor.ColorCategoryID, oTechnicalSheetColor.IsSelected, oTechnicalSheetColor.Note, oTechnicalSheetColor.Sequence, oTechnicalSheetColor.PantonNo, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheetColor WHERE TechnicalSheetColorID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheetColor");
        }

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheetColor WHERE TechnicalSheetID=%n ORDER BY Sequence", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

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
    public class ColorCategoryDA
    {
        public ColorCategoryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ColorCategory oColorCategory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ColorCategory]"
                                    + "%n, %s, %s, %n, %n",
                                    oColorCategory.ColorCategoryID, oColorCategory.ColorName, oColorCategory.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ColorCategory oColorCategory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ColorCategory]"
                                    + "%n, %s, %s, %n, %n",
                                    oColorCategory.ColorCategoryID, oColorCategory.ColorName, oColorCategory.Note, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ColorCategory WHERE ColorCategoryID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ColorCategory ORDER BY ColorName");
        }
        public static IDataReader GetsTSNotAssignColor(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM ColorCategory WHERE ColorCategoryID NOT IN(SELECT ColorCategoryID FROM TechnicalSheetColor WHERE TechnicalSheetID=%n) ORDER BY ColorName", nTechnicalSheetID);
        }

        public static IDataReader GetsColorPikerForQC(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM ColorCategory WHERE ColorCategoryID IN(SELECT ColorCategoryID FROM TechnicalSheetColor WHERE TechnicalSheetID=%n) ORDER BY ColorName", nTechnicalSheetID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsbyColorName(TransactionContext tc, string sColorName)
        {
            return tc.ExecuteReader("SELECT * FROM ColorCategory WHERE ColorName LIKE ('%" + sColorName + "%')  ORDER BY ColorName");
        }
      
        #endregion
    }
}

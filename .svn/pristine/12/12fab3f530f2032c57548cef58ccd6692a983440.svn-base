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
    public class ModelCategoryDA
    {
        public ModelCategoryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ModelCategory oModelCategory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ModelCategory]"
                                    + "%n, %s, %s, %n, %n",
                                    oModelCategory.ModelCategoryID, oModelCategory.CategoryName, oModelCategory.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ModelCategory oModelCategory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ModelCategory]"
                                    + "%n, %s, %s, %n, %n",
                                    oModelCategory.ModelCategoryID, oModelCategory.CategoryName, oModelCategory.Remarks, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ModelCategory WHERE ModelCategoryID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ModelCategory ORDER BY CategoryName");
        }
        public static IDataReader GetsTSNotAssignColor(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM ModelCategory WHERE ModelCategoryID NOT IN(SELECT ModelCategoryID FROM TechnicalSheetColor WHERE TechnicalSheetID=%n) ORDER BY CategoryName", nTechnicalSheetID);
        }

        public static IDataReader GetsColorPikerForQC(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM ModelCategory WHERE ModelCategoryID IN(SELECT ModelCategoryID FROM TechnicalSheetColor WHERE TechnicalSheetID=%n) ORDER BY CategoryName", nTechnicalSheetID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsbyCategoryName(TransactionContext tc, string sCategoryName)
        {
            return tc.ExecuteReader("SELECT * FROM ModelCategory WHERE CategoryName LIKE ('%" + sCategoryName + "%')  ORDER BY CategoryName");
        }
      
        #endregion
    }
}

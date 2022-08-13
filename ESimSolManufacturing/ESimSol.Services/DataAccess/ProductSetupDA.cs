using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class ProductSetupDA
    {
        public ProductSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductSetup oProductSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ProductSetup]"
                                    + "%n,%n, %b, %b, %b, %b, %b,%b,%b,%b,%b, %s, %n, %n",
                                    oProductSetup.ProductSetupID, oProductSetup.ProductCategoryID, oProductSetup.IsApplyCategory, oProductSetup.IsApplyGroup, oProductSetup.IsApplyProductType, oProductSetup.IsApplyProperty, oProductSetup.IsApplyPlantNo, oProductSetup.IsApplyColor, oProductSetup.IsApplySize,oProductSetup.IsApplyMeasurement,  oProductSetup.IsApplySizer,       oProductSetup.Note, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ProductSetup oProductSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ProductSetup]"
                                    + "%n,%n, %b, %b, %b, %b, %b,%b,%b,%b,%b, %s, %n, %n",
                                    oProductSetup.ProductSetupID, oProductSetup.ProductCategoryID, oProductSetup.IsApplyCategory, oProductSetup.IsApplyGroup, oProductSetup.IsApplyProductType, oProductSetup.IsApplyProperty, oProductSetup.IsApplyPlantNo, oProductSetup.IsApplyColor, oProductSetup.IsApplySize, oProductSetup.IsApplyMeasurement, oProductSetup.IsApplySizer, oProductSetup.Note, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductSetup WHERE ProductSetupID=%n", nID);
        }
        public static IDataReader GetByCategory(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductSetup WHERE ProductCategoryID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductSetup");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
       
    
     
    
        #endregion
    }
}
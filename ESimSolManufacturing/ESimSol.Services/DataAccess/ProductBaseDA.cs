using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductBaseDA
    {
        public ProductBaseDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductBase oProductBase, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductBase]"
                                    + "%n, %n, %s, %s, %s, %s, %s, %b, %n, %n",
                                    oProductBase.ProductBaseID, oProductBase.ProductCategoryID, oProductBase.ProductCode, oProductBase.ProductName, oProductBase.ShortName, oProductBase.ManufacturerModelCode, oProductBase.Note, oProductBase.IsActivate, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ProductBase oProductBase, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductBase]"
                                    + "%n, %n, %s, %s, %s, %s, %s, %b, %n, %n",
                                     oProductBase.ProductBaseID, oProductBase.ProductCategoryID, oProductBase.ProductCode, oProductBase.ProductName, oProductBase.ShortName, oProductBase.ManufacturerModelCode, oProductBase.Note, oProductBase.IsActivate, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductBase WHERE ProductBaseID=%n", nID);
        }
        public static IDataReader GetsByCategory(TransactionContext tc, long nCategoryID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductBase WHERE ProductCategoryID=%n", nCategoryID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductBase Order By ProductBaseID DESC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

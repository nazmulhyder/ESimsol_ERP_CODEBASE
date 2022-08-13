using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductStockDA
    {
        public ProductStockDA() { }

        #region Get & Exist Function
        public static void SetReorderLevel(TransactionContext tc, ProductStock oProductStock, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update Product SET ShortQty = %n  WHERE ProductID = %n", oProductStock.ShortQty, oProductStock.ProductID);
        }
        public static IDataReader Gets(TransactionContext tc, ProductStock oProductStock)
        {
            return tc.ExecuteReader("EXEC [SP_ProductStock] %n, %n, %n, %b", oProductStock.BUID, oProductStock.ProductCategoryID, oProductStock.ProductID, oProductStock.IsReport);
        }        
        #endregion
    } 
   
}

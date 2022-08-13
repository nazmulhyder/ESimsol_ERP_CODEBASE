using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductDA
    {
        public ProductDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Product oProduct, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Product]" + "%n, %s, %u, %n, %s, %b, %b,  %s, %n, %n, %s, %s, %s, %b, %n, %n, %b, %n, %s, %n, %n, %n, %n, %n, %b, %n, %n, %n, %n",
                                    oProduct.ProductID, oProduct.ProductCode, oProduct.ProductName, oProduct.MeasurementUnitID, oProduct.ShortName, oProduct.ApplyInventory, oProduct.ApplyProperty,  oProduct.Brand, oProduct.ProductCategoryID, oProduct.ProductBaseID, oProduct.AddOne,oProduct.AddTwo, oProduct.AddThree, oProduct.IsFixedAsset, (int)oProduct.ProductType, oProduct.AccountHeadID, oProduct.IsSerialNoApply,oProduct.ModelReferenceID, oProduct.ProductDescription, oProduct.FinishGoodsWeight, oProduct.NaliWeight, oProduct.WeigthFor, oProduct.FinishGoodsUnit, oProduct.ReportingUnitID,oProduct.Activity, oProduct.PurchasePrice, oProduct.SalePrice,  nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, Product oProduct, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Product]" + "%n, %s, %u, %n, %s, %b, %b,  %s, %n, %n, %s, %s, %s, %b, %n, %n, %b, %n, %s, %n, %n, %n, %n, %n, %b, %n, %n, %n, %n",
                                    oProduct.ProductID, oProduct.ProductCode, oProduct.ProductName, oProduct.MeasurementUnitID, oProduct.ShortName, oProduct.ApplyInventory, oProduct.ApplyProperty, oProduct.Brand, oProduct.ProductCategoryID, oProduct.ProductBaseID, oProduct.AddOne, oProduct.AddTwo, oProduct.AddThree, oProduct.IsFixedAsset, (int)oProduct.ProductType, oProduct.AccountHeadID, oProduct.IsSerialNoApply, oProduct.ModelReferenceID, oProduct.ProductDescription, oProduct.FinishGoodsWeight, oProduct.NaliWeight, oProduct.WeigthFor, oProduct.FinishGoodsUnit, oProduct.ReportingUnitID, oProduct.Activity, oProduct.PurchasePrice, oProduct.SalePrice, nUserID, (int)eEnumDBOperation);
        }
        public static void CommitActivity(TransactionContext tc, int id, bool Activity, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update Product SET Activity =%b  WHERE ProductID = %n", Activity, id);
        }
        public static void ProductMarge(TransactionContext tc, Product oProduct,Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_Process_Product_Merge]"
                                    + "%s,%n",
                                     oProduct.ProductName,oProduct.ProductID);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Product WHERE ProductID=%n ", nID);
        }
     
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Product" );
        }
        public static IDataReader GetsByBaseProduct(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Product WHERE ProductBaseID = %n ", nID);
        }
        public static IDataReader GetsByPCategory(TransactionContext tc, int nCategoryID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Product WHERE ProductCategoryID=%n", nCategoryID);
        }
        public static double GetStockQuantity(TransactionContext tc, int nBUID, int nProductID, int nUserID)
        {
            object obj = tc.ExecuteScalar("Select ISNULL(SUM(Balance),0) from Lot Where BUID=%n And ProductID=%n", nBUID, nProductID);
            return Convert.ToDouble(obj);
        }
        public static IDataReader GetsByBU(TransactionContext tc, int nBUID, string sProductName)
        {
            string sSQL = "SELECT * FROM View_Product WHERE Activity = 1 ";
            if (nBUID !=0)
            {
               sSQL+=" AND ProductCategoryID IN (SELECT ProductCategoryID FROM  BUWiseProductCategory WHERE BUID = " + nBUID + ")";
            }
            if (sProductName != null && sProductName != "")
            {
                sSQL += " AND ProductName Like '%" + sProductName + "%'";
            }

            sSQL += " order by ProductCategoryID,ProductName";
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader ProductGroupChangeSave(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsbyName(TransactionContext tc, string sProductName, string sProductCategorys)
        {
            string sSQL = "SELECT * FROM View_Product WHERE ProductID<>0";
            if (sProductName.Trim() != "")
            {
                sSQL = sSQL + " And ProductName LIKE ('%" + sProductName + "%')";
            }
            if (sProductCategorys.Trim() != "")
            {
                sSQL = sSQL + " AND ProductCategoryID IN (" + sProductCategorys + ")";
            }
            sSQL = sSQL + " ORDER BY ProductName";
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsbyConfigure(TransactionContext tc, string sProductName, string sParentCategoryIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_Product WHERE  ProductName LIKE ('%" + sProductName + "%')  ORDER BY ProductName");
        }
        public static IDataReader GetsByCodeOrName(TransactionContext tc, Product oProduct)
        {
            return tc.ExecuteReader("SELECT * FROM View_Product WHERE NameCode LIKE '%" + oProduct.NameCode + "%' ORDER BY ProductName");
        }
        public static IDataReader GetsPermittedProduct(TransactionContext tc, int nBUID, EnumModuleName eModuleName, EnumProductUsages eProductUsages, int nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Product AS HH WHERE  HH.Activity = 1 AND HH.ProductCategoryID IN (SELECT PC.ProductCategoryID FROM [dbo].[FN_GetProductCategoryByBUModuleAndUser](%n,%n,%n,%n) AS PC)  ORDER BY HH.ProductName", nUserID, (int)eModuleName, (int)eProductUsages,  nBUID);
        }
        public static IDataReader GetsPermittedProductByNameCode(TransactionContext tc, int nBUID, EnumModuleName eModuleName, EnumProductUsages eProductUsages, string sNameCode, int nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Product AS HH WHERE HH.Activity = 1 AND HH.ProductName LIKE '%" + sNameCode + "%' AND  HH.ProductCategoryID IN (SELECT PC.ProductCategoryID FROM [dbo].[FN_GetProductCategoryByBUModuleAndUser](" + nUserID + "," + (int)eModuleName + "," + (int)eProductUsages + "," + nBUID + ") AS PC) ORDER BY HH.ProductName");
        }
        public static IDataReader Gets_Import(TransactionContext tc,string sNameCode , int nBUID,  int nProductType, int nUserID)
        {
            return tc.ExecuteReader("SELECT top(100)* FROM View_Product AS HH WHERE HH.Activity = 1 AND HH.ProductName LIKE '%" + sNameCode + "%' AND  HH.ProductCategoryID IN (SELECT ProductCategoryID FROM [dbo].[fn_GetProductCategoryBU](" + nBUID + "," + nProductType + ")) ORDER BY HH.ProductName");
        }
        #endregion
    } 
   
}

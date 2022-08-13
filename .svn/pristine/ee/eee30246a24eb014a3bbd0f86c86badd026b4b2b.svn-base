using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class ProductService : MarshalByRefObject, IProductService
    {
        #region Private functions and declaration
        private Product MapObject(NullHandler oReader)
        {
            Product oProduct = new Product();
            oProduct.ProductID = oReader.GetInt32("ProductID");
            oProduct.ProductCode = oReader.GetString("ProductCode");
            oProduct.ProductName = oReader.GetString("ProductName");
            oProduct.UnitType = (EnumUniteType) oReader.GetInt32("UnitType");
            oProduct.UnitTypeInInt = oReader.GetInt32("UnitType");
            oProduct.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oProduct.ShortName = oReader.GetString("ShortName");
            oProduct.IsApplySizer = oReader.GetBoolean("IsApplySizer");
            oProduct.ApplyInventory = oReader.GetBoolean("ApplyInventory");
            oProduct.ApplyProperty = oReader.GetBoolean("ApplyProperty");
            oProduct.IsFixedAsset = oReader.GetBoolean("IsFixedAsset");
            oProduct.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oProduct.ProductBaseID = oReader.GetInt32("ProductBaseID");
            oProduct.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oProduct.MUnitName = oReader.GetString("MUnitName");
            oProduct.MUnit = oReader.GetString("MUnit");
            oProduct.GroupName = oReader.GetString("GroupName");
            oProduct.AddOne = oReader.GetString("AddOne");
            oProduct.AddTwo = oReader.GetString("AddTwo");
            oProduct.AddThree = oReader.GetString("AddThree");
            oProduct.NameCode = oReader.GetString("NameCode");
            oProduct.Brand = oReader.GetString("Brand");
            oProduct.ProductType = (EnumProductType)oReader.GetInt32("ProductType");
            oProduct.ProductTypeInInt =oReader.GetInt32("ProductType");
            oProduct.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oProduct.AccountCode = oReader.GetString("AccountCode");
            oProduct.AccountHeadName = oReader.GetString("AccountHeadName");
            oProduct.IsSerialNoApply = oReader.GetBoolean("IsSerialNoApply");
            oProduct.IsApplyCategory = oReader.GetBoolean("IsApplyCategory");
            oProduct.IsApplyGroup = oReader.GetBoolean("IsApplyGroup");
            oProduct.IsApplyColor = oReader.GetBoolean("IsApplyColor");
            oProduct.IsApplySize = oReader.GetBoolean("IsApplySize");
            oProduct.IsApplyMeasurement = oReader.GetBoolean("IsApplyMeasurement");
            oProduct.ApplyGroup_IsShow = oReader.GetBoolean("ApplyGroup_IsShow");
            oProduct.ApplyProductType_IsShow = oReader.GetBoolean("ApplyProductType_IsShow");
            oProduct.ApplyProperty_IsShow = oReader.GetBoolean("ApplyProperty_IsShow");
            oProduct.ApplyPlantNo_IsShow = oReader.GetBoolean("ApplyPlantNo_IsShow");
            oProduct.ModelReferenceID = oReader.GetInt32("ModelReferenceID");
            oProduct.ModelReferenceName = oReader.GetString("ModelReferenceName");
            oProduct.ProductDescription = oReader.GetString("ProductDescription");
            oProduct.FinishGoodsWeight = oReader.GetDouble("FinishGoodsWeight");
            oProduct.NaliWeight = oReader.GetDouble("NaliWeight");
            oProduct.WeigthFor = oReader.GetDouble("WeigthFor");
            oProduct.FinishGoodsUnit = oReader.GetInt32("FinishGoodsUnit");
            oProduct.FGUSymbol = oReader.GetString("ProductDescription");
            oProduct.ReportingUnitID = oReader.GetInt32("ReportingUnitID");
            oProduct.Activity = oReader.GetBoolean("Activity");
            oProduct.ReportingUnitName = oReader.GetString("ReportingUnitName");
            oProduct.ReportingUnit = oReader.GetString("ReportingUnit");
            oProduct.ShortQty = oReader.GetDouble("ShortQty");
            oProduct.PurchasePrice = oReader.GetDouble("PurchasePrice");
            oProduct.SalePrice = oReader.GetDouble("SalePrice");
            oProduct.QtyType = oReader.GetInt32("QtyType");
            oProduct.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oProduct.LastUpdateByName = oReader.GetString("LastUpdateByName");
            oProduct.LastUpdateDateTime = oReader.GetDateTime("LastUpdateDateTime");
            return oProduct;
        }

        private Product CreateObject(NullHandler oReader)
        {
            Product oProduct = new Product();
            oProduct = MapObject(oReader);
            return oProduct;
        }

        private List<Product> CreateObjects(IDataReader oReader)
        {
            List<Product> oProduct = new List<Product>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Product oItem = CreateObject(oHandler);
                oProduct.Add(oItem);
            }
            return oProduct;
        }

        #endregion

        #region Interface implementation
        public ProductService() { }

        public Product Save(Product oProduct, int nUserID)
        {
            TransactionContext tc = null;
            oProduct.PPIs = new List<ProductPropertyInformation>();
            try
            {
                tc = TransactionContext.Begin(true);
                #region Product
                IDataReader reader;
                if (oProduct.ProductID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "Product", EnumRoleOperationType.Add);
                    reader = ProductDA.InsertUpdate(tc, oProduct, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "Product", EnumRoleOperationType.Edit);
                    reader = ProductDA.InsertUpdate(tc, oProduct, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProduct = new Product();
                    oProduct = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }

            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProduct.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oProduct;
        }
        public Double GetStockQuantity(int nBUID, int nProductID, int nUserID)
        {
            double nStockQuantity = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nStockQuantity = ProductDA.GetStockQuantity(tc, nBUID, nProductID, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to get stock quantity.", e);
                #endregion
            }
            return nStockQuantity;
        }
        public Product CommitActivity(int id, bool ActiveInActive, int nUserId)
        {
            Product oAccountHead = new Product();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                ProductDA.CommitActivity(tc, id, ActiveInActive, nUserId);
                IDataReader reader = ProductDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public string Delete(Product oProduct, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
              
             //   AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "Product", EnumRoleOperationType.Delete);
             
                ProductDA.Delete(tc, oProduct, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                    return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public string DeleteGeneralizeProduct(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Product oProduct = new Product();
               // AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "Product", EnumRoleOperationType.Delete);
                oProduct.ProductID = id;
                ProductDA.Delete(tc, oProduct, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                    return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }
        public Product Get(int id,  int nUserId)
        {
            Product oProduct = new Product();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ProductDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProduct = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }
            return oProduct;
        }

        public List<Product> GetsbyName(string sProductName, string sProductCategorys,int nUserID)
        {
            List<Product> oProduct = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ProductDA.GetsbyName(tc, sProductName, sProductCategorys);
                NullHandler oReader = new NullHandler(reader);
                oProduct = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }
            return oProduct;
        }
        public List<Product> Gets( int nUserID)
        {
            List<Product> oProduct = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductDA.Gets(tc);
                oProduct = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }
            return oProduct;
        }
        public List<Product> Gets(string sSQL, int nUserId) 
        {
            List<Product> oProduct = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductDA.Gets(tc, sSQL);
                oProduct = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oProduct;
        }
        public List<Product> ProductGroupChangeSave(string productIds, int ProductCategoryID, int ProductBaseID, int nUserId)
        {
            List<Product> oProduct = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                string sSQL = "UPDATE PRODUCT SET ProductCategoryID = " + ProductCategoryID + "," +"ProductBaseID = " + ProductBaseID + " WHERE ProductID IN (" + productIds +")";
                IDataReader reader = null;
                reader = ProductDA.ProductGroupChangeSave(tc, sSQL);
                reader.Close();
                reader = null;
                sSQL = "SELECT * FROM View_Product WHERE ProductID IN(" + productIds + ")";
                reader = ProductDA.Gets(tc, sSQL);
                oProduct = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oProduct;
        }
        public List<Product> GetsbyConfigure(string sProductName, string sParentCategoryIDs, int nUserID)
        {
            List<Product> oProduct = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductDA.GetsbyConfigure(tc, sProductName, sParentCategoryIDs);
                NullHandler oReader = new NullHandler(reader);
                oProduct = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oProduct;
        }
        public List<Product> Getsby(int nProductBaseID, int nUserID)
        {
            List<Product> oProduct = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductDA.GetsByBaseProduct(tc, nProductBaseID);
                NullHandler oReader = new NullHandler(reader);
                oProduct = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oProduct;
        }
        public List<Product> GetsByPCategory(int nCategoryID, int nUserId)
        {
            List<Product> oProduct = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductDA.GetsByPCategory(tc, nCategoryID);
                oProduct = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oProduct;
        }
        public List<Product> GetsByBU(int nBUID, string sProductName, int nUserId)
        {
            List<Product> oProduct = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductDA.GetsByBU(tc, nBUID, sProductName);
                oProduct = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oProduct;
        }
        public List<Product> GetsByCodeOrName(Product oProduct, int nUserID)
        {
            List<Product> oProducts = new List<Product>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductDA.GetsByCodeOrName(tc, oProduct);
                oProducts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProducts = new List<Product>();
                oProduct = new Product();
                oProduct.ErrorMessage = e.Message;
                oProducts.Add(oProduct);
                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message);
                #endregion
            }
            return oProducts;
        }
        public List<Product> GetsPermittedProduct(int nBUID, EnumModuleName eModuleName, EnumProductUsages eProductUsages, int nUserID)
        {
            List<Product> oProducts = new List<Product>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductDA.GetsPermittedProduct(tc, nBUID, eModuleName, eProductUsages, nUserID);
                oProducts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProducts = new List<Product>();
                Product oProduct = new Product();
                oProduct.ErrorMessage = e.Message;
                oProducts.Add(oProduct);
                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message);
                #endregion
            }
            return oProducts;
        }
        public List<Product> GetsPermittedProductByNameCode(int nBUID, EnumModuleName eModuleName, EnumProductUsages eProductUsages, string sNameCode, int nUserID)
        {
            List<Product> oProducts = new List<Product>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductDA.GetsPermittedProductByNameCode(tc, nBUID, eModuleName, eProductUsages, sNameCode, nUserID);
                oProducts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProducts = new List<Product>();
                Product oProduct = new Product();
                oProduct.ErrorMessage = e.Message;
                oProducts.Add(oProduct);
                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message);
                #endregion
            }
            return oProducts;
        }
        public List<Product> Gets_Import(string sNameCode , int nBUID,  int nProductType, int nUserID)
        {
            List<Product> oProducts = new List<Product>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductDA.Gets_Import(tc,  sNameCode ,  nBUID,   nProductType, nUserID);
                oProducts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProducts = new List<Product>();
                Product oProduct = new Product();
                oProduct.ErrorMessage = e.Message;
                oProducts.Add(oProduct);
                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message);
                #endregion
            }
            return oProducts;
        }
        public Product ProductMarge(Product oProduct, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                ProductDA.ProductMarge(tc, oProduct, nUserId);
                IDataReader reader = ProductDA.Get(tc, oProduct.ProductID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProduct = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oProduct.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }

            return oProduct;
        }
        #endregion
    }
}

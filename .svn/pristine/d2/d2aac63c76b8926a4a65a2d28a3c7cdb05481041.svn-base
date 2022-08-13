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
    public class ProductCategoryService : MarshalByRefObject, IProductCategoryService
    {
        #region Private functions and declaration
        private ProductCategory MapObject(NullHandler oReader)
        {
            ProductCategory oProductCategory = new ProductCategory();
            oProductCategory.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oProductCategory.ProductCategoryName = oReader.GetString("ProductCategoryName");
            
            oProductCategory.ParentCategoryID = oReader.GetInt32("ParentCategoryID");
            oProductCategory.DrAccountHeadID = oReader.GetInt32("DrAccountHeadID");
            oProductCategory.DrAccountHeadName = oReader.GetString("DrAccountHeadName");
            oProductCategory.CrAccountHeadID = oReader.GetInt32("CrAccountHeadID");
            oProductCategory.CrAccountHeadName = oReader.GetString("CrAccountHeadName");
            oProductCategory.Note = oReader.GetString("Note");
            oProductCategory.IsLastLayer = oReader.GetBoolean("IsLastLayer");
            oProductCategory.IsApplyCategory = oReader.GetBoolean("IsApplyCategory");
            oProductCategory.IsApplyGroup = oReader.GetBoolean("IsApplyGroup");
            oProductCategory.ApplyGroup_IsShow = oReader.GetBoolean("ApplyGroup_IsShow");
            oProductCategory.ApplyProductType_IsShow = oReader.GetBoolean("ApplyProductType_IsShow");
            oProductCategory.ApplyProperty_IsShow = oReader.GetBoolean("ApplyProperty_IsShow");
            oProductCategory.ApplyPlantNo_IsShow = oReader.GetBoolean("ApplyPlantNo_IsShow");
            oProductCategory.ParentCategoryName = oReader.GetString("ParentCategoryName");
            return oProductCategory;
        }

        private ProductCategory CreateObject(NullHandler oReader)
        {
            ProductCategory oProductCategory = new ProductCategory();
            oProductCategory = MapObject(oReader);
            return oProductCategory;
        }

        private List<ProductCategory> CreateObjects(IDataReader oReader)
        {
            List<ProductCategory> oProductCategory = new List<ProductCategory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductCategory oItem = CreateObject(oHandler);
                oProductCategory.Add(oItem);
            }
            return oProductCategory;
        }

        #endregion

        #region Interface implementation
        public ProductCategoryService() { }

        public ProductCategory IUD(ProductCategory oProductCategory, int nDBOperation, int nUserId)
        {
            TransactionContext tc = null;
            ProductSetup oProductSetup = new ProductSetup();
            oProductSetup = oProductCategory.ProductSetup;
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = oProductCategory.BusinessUnits;
            try
            {
                tc = TransactionContext.Begin(true);
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "ProductCategory", (EnumRoleOperationType)nDBOperation);
                
                IDataReader reader;
                reader = ProductCategoryDA.InsertUpdate(tc, oProductCategory, nDBOperation, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductCategory = new ProductCategory();
                    oProductCategory = CreateObject(oReader);
                }

                reader.Close();
                if (nDBOperation != (int)EnumDBOperation.Delete)
                {
                    oProductSetup.ProductCategoryID = oProductCategory.ProductCategoryID;

                    if (oProductSetup.ProductSetupID <= 0)
                    {
                        reader = ProductSetupDA.InsertUpdate(tc, oProductSetup, EnumDBOperation.Insert, nUserId);
                    }
                    else
                    {
                        reader = ProductSetupDA.InsertUpdate(tc, oProductSetup, EnumDBOperation.Update, nUserId);
                    }
                    reader.Close();
                }

                #region BUWiseProductCategory
                BUWiseProductCategory oBUWiseProductCategory = new BUWiseProductCategory();
                oBUWiseProductCategory.ProductCategoryID = oProductCategory.ProductCategoryID;
                BUWiseProductCategoryDA.Delete(tc, oBUWiseProductCategory, EnumDBOperation.Delete, nUserId);
                if (oBusinessUnits != null)
                {
                    foreach (BusinessUnit oItem in oBusinessUnits)
                    {
                        IDataReader readerBUProductCategory;
                        oBUWiseProductCategory = new BUWiseProductCategory();
                        oBUWiseProductCategory.ProductCategoryID = oProductCategory.ProductCategoryID;
                        oBUWiseProductCategory.BUID = oItem.BusinessUnitID;
                        readerBUProductCategory = BUWiseProductCategoryDA.InsertUpdate(tc, oBUWiseProductCategory, EnumDBOperation.Insert, nUserId);
                        NullHandler oReaderTNC = new NullHandler(readerBUProductCategory);
                        readerBUProductCategory.Close();
                    }
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ProductCategory. Because of " + e.Message, e);
                oProductCategory.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oProductCategory;
        }
        public ProductCategory Update_AccountHead(ProductCategory oProductCategory, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductCategoryDA.Update_AccountHead(tc, oProductCategory);
                IDataReader reader = ProductCategoryDA.Get(tc, oProductCategory.ProductCategoryID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductCategory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oProductCategory.ErrorMessage = e.Message;
                #endregion
            }
            return oProductCategory;
        }

        public ProductCategory Get(int id, int nUserId)
        {
            ProductCategory oAccountHead = new ProductCategory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ProductCategoryDA.Get(tc, id);
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
                throw new ServiceException(e.Message);
                #endregion
            }
            return oAccountHead;
        }
        public List<ProductCategory> Gets(string sSQL, int nUserId)
        {
            List<ProductCategory> oProductCategory = null; 
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductCategoryDA.Gets(tc, sSQL);
                oProductCategory = CreateObjects(reader);
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
            return oProductCategory;
        }
      
        public List<ProductCategory> GetsByParentID(int nParentID, int nUserId)
        {
            List<ProductCategory> oProductCategory = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductCategoryDA.GetsByParentID(tc, nParentID);
                oProductCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductCategory", e);
                #endregion
            }
            return oProductCategory;
        }

        public List<ProductCategory> Gets(int nUserId)
        {
            List<ProductCategory> oProductCategory = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductCategoryDA.Gets(tc);
                oProductCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductCategory", e);
                #endregion
            }
            return oProductCategory;
        }

        public List<ProductCategory> BUWiseGets(int nBUID, int nUserId)
        {
            List<ProductCategory> oProductCategory = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductCategoryDA.BUWiseGets(nBUID,tc);
                oProductCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductCategory", e);
                #endregion
            }
            return oProductCategory;
        }
        public List<ProductCategory> GetsBUWiseLastLayer(int nBUID, int nUserID)
        {
            List<ProductCategory> oProductCategory = new List<ProductCategory>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductCategoryDA.GetsBUWiseLastLayer(tc, nBUID);
                oProductCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductCategory", e);
                #endregion
            }
            return oProductCategory;
        }
        public List<ProductCategory> GetsLastLayer(int nUserId)
        {
            List<ProductCategory> oProductCategory = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductCategoryDA.GetsLastLayer(tc);
                oProductCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductCategory", e);
                #endregion
            }
            return oProductCategory;
        }

    
        public List<ProductCategory> GetsForTree(string sTableName, string sPK, string sPCID, bool IsParent, bool IsChild, string IDs, bool IsAll, Int64 nUserId)
        {
            List<ProductCategory> oProductCategorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductCategoryDA.GetsForTree(tc, sTableName, sPK, sPCID, IsParent, IsChild, IDs, IsAll);
                oProductCategorys = CreateObjects(reader);
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

            return oProductCategorys;
        }
      
        #endregion
    } 
}
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
    public class ProductBaseService : MarshalByRefObject, IProductBaseService
    {
        #region Private functions and declaration
        private ProductBase MapObject(NullHandler oReader)
        {
            ProductBase oProductBase = new ProductBase();
            oProductBase.ProductBaseID = oReader.GetInt32("ProductBaseID");
            oProductBase.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oProductBase.ProductCode = oReader.GetString("ProductCode");
            oProductBase.ProductName = oReader.GetString("ProductName");
            oProductBase.ShortName = oReader.GetString("ShortName");
        
            oProductBase.Note = oReader.GetString("Note");
            oProductBase.ManufacturerModelCode = oReader.GetString("ManufacturerModelCode");
            oProductBase.IsActivate = oReader.GetBoolean("IsActivate");
            oProductBase.ProductCategoryName = oReader.GetString("ProductCategoryName");
          
            return oProductBase;
        }

        private ProductBase CreateObject(NullHandler oReader)
        {
            ProductBase oProductBase = new ProductBase();
            oProductBase = MapObject(oReader);
            return oProductBase;
        }

        private List<ProductBase> CreateObjects(IDataReader oReader)
        {
            List<ProductBase> oProductBase = new List<ProductBase>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductBase oItem = CreateObject(oHandler);
                oProductBase.Add(oItem);
            }
            return oProductBase;
        }

        #endregion

        #region Interface implementation
        public ProductBaseService() { }

        public List<ProductBase> Gets(string sSQL, Int64 nUserId)
        {
            List<ProductBase> oProductBase = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductBaseDA.Gets(tc, sSQL);
                oProductBase = CreateObjects(reader);
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

            return oProductBase;
        }

        public ProductBase Save(ProductBase oProductBase, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProductBase.ProductBaseID <= 0)
                {
                    reader = ProductBaseDA.InsertUpdate(tc, oProductBase, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ProductBaseDA.InsertUpdate(tc, oProductBase, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductBase = new ProductBase();
                    oProductBase = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oProductBase = new ProductBase();
                oProductBase.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oProductBase;
        }
    

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductBase oProductBase = new ProductBase();
                oProductBase.ProductBaseID = id;
               // AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "ProductBase", EnumRoleOperationType.Delete);
                ProductBaseDA.Delete(tc, oProductBase, EnumDBOperation.Delete, nUserId);
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



        public ProductBase Get(int id, Int64 nUserId)
        {
            ProductBase oProductBase = new ProductBase();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductBaseDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductBase = CreateObject(oReader);
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

            return oProductBase;
        }
        public List<ProductBase> GetsByCategory(int nCategoryID, Int64 nUserId)
        {
            List<ProductBase> oProductBase = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductBaseDA.GetsByCategory(tc, nCategoryID);
                oProductBase = CreateObjects(reader);
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

            return oProductBase;
        }

        public List<ProductBase> Gets(Int64 nUserId)
        {
            List<ProductBase> oProductBase = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductBaseDA.Gets(tc);
                oProductBase = CreateObjects(reader);
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

            return oProductBase;
        }
        #endregion
    }
}
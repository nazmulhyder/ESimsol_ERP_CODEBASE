using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;


using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ProductSetupService : MarshalByRefObject, IProductSetupService
    {
        #region Private functions and declaration
        private ProductSetup MapObject(NullHandler oReader)
        {
            ProductSetup oProductSetup = new ProductSetup();
            oProductSetup.ProductSetupID = oReader.GetInt32("ProductSetupID");
            oProductSetup.IsApplyCategory = oReader.GetBoolean("IsApplyCategory");
            oProductSetup.IsApplyGroup = oReader.GetBoolean("IsApplyGroup");
            oProductSetup.IsApplyProductType = oReader.GetBoolean("IsApplyProductType");
            oProductSetup.IsApplyProperty = oReader.GetBoolean("IsApplyProperty");
            oProductSetup.IsApplyPlantNo = oReader.GetBoolean("IsApplyPlantNo");
            oProductSetup.IsApplyColor = oReader.GetBoolean("IsApplyColor");
            oProductSetup.IsApplySize = oReader.GetBoolean("IsApplySize");
            oProductSetup.IsApplyMeasurement = oReader.GetBoolean("IsApplyMeasurement");
            oProductSetup.IsApplySizer = oReader.GetBoolean("IsApplySizer");
            oProductSetup.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oProductSetup.ApplyGroup_IsShow = oReader.GetBoolean("ApplyGroup_IsShow");
            oProductSetup.ApplyProductType_IsShow = oReader.GetBoolean("ApplyProductType_IsShow");
            oProductSetup.ApplyProperty_IsShow = oReader.GetBoolean("ApplyProperty_IsShow");
            oProductSetup.ApplyPlantNo_IsShow = oReader.GetBoolean("ApplyPlantNo_IsShow");
            return oProductSetup;
        }

        private ProductSetup CreateObject(NullHandler oReader)
        {
            ProductSetup oProductSetup = new ProductSetup();
            oProductSetup = MapObject(oReader);
            return oProductSetup;
        }

        private List<ProductSetup> CreateObjects(IDataReader oReader)
        {
            List<ProductSetup> oProductSetups = new List<ProductSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductSetup oItem = CreateObject(oHandler);
                oProductSetups.Add(oItem);
            }
            return oProductSetups;
        }

        #endregion

        #region Interface implementation
        public ProductSetupService() { }


        public ProductSetup Save(ProductSetup oProductSetup, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region ProductSetup
                IDataReader reader;
                if (oProductSetup.ProductSetupID <= 0)
                {
                    reader = ProductSetupDA.InsertUpdate(tc, oProductSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ProductSetupDA.InsertUpdate(tc, oProductSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductSetup = new ProductSetup();
                    oProductSetup = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oProductSetup = new ProductSetup();
                oProductSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oProductSetup;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            
            string sMessage = "Delete sucessfully";
            try
            {
                tc = TransactionContext.Begin(true);
                ProductSetup oProductSetup = new ProductSetup();
                oProductSetup.ProductSetupID = id;
                ProductSetupDA.Delete(tc, oProductSetup, EnumDBOperation.Delete, nUserId);
               
                tc.End();
            }
            catch (Exception e)
            {
                sMessage = "Delete operation could not complete";
            }

            return sMessage;
        }

        public ProductSetup Get(int id, Int64 nUserId)
        {
            ProductSetup oProductSetup = new ProductSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oProductSetup;
        }
        public ProductSetup GetByCategory(int id, Int64 nUserId)
        {
            ProductSetup oProductSetup = new ProductSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductSetupDA.GetByCategory(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ProductSetup", e);
                #endregion
            }

            return oProductSetup;
        }

        public List<ProductSetup> Gets(Int64 nUserId)
        {
            List<ProductSetup> oProductSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductSetupDA.Gets(tc);
                oProductSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oProductSetups;
        }


        #endregion
    }
}
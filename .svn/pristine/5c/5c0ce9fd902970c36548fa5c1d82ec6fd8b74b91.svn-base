using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class ProductRateService : MarshalByRefObject, IProductRateService
    {
        #region Private functions and declaration
        private ProductRate MapObject(NullHandler oReader)
        {
            ProductRate oProductRate= new ProductRate();
            oProductRate.ProductRateID = oReader.GetInt32("ProductRateID");
            oProductRate.ProductID = oReader.GetInt32("ProductID");
            oProductRate.Rate = oReader.GetDouble("Rate");
            oProductRate.ActivationDate = oReader.GetDateTime("ActivationDate");
            oProductRate.ProductCode = oReader.GetString("ProductCode");
            oProductRate.ProductName = oReader.GetString("ProductName");
            oProductRate.SaleSchemeID= oReader.GetInt32("SaleSchemeID");
            return oProductRate;
        }

        private ProductRate CreateObject(NullHandler oReader)
        {
            ProductRate oProductRate = new ProductRate();
            oProductRate= MapObject(oReader);
            return oProductRate;
        }

        private List<ProductRate> CreateObjects(IDataReader oReader)
        {
            List<ProductRate> oProductRate= new List<ProductRate>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductRate oItem = CreateObject(oHandler);
                oProductRate.Add(oItem);
            }
            return oProductRate;
        }

        #endregion

        #region Interface implementation
        public ProductRateService() { }
        public ProductRate Save(ProductRate oProductRate, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProductRate.ProductRateID <= 0)
                {
                    reader = ProductRateDA.InsertUpdate(tc, oProductRate, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ProductRateDA.InsertUpdate(tc, oProductRate, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductRate = new ProductRate();
                    oProductRate = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ProductRate. Because of " + e.Message, e);
                #endregion
            }
            return oProductRate;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductRate oProductRate = new ProductRate();
                oProductRate.ProductRateID = id;
                ProductRateDA.Delete(tc, oProductRate, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ProductRate Get(int id, Int64 nUserId)
        {
            ProductRate oProductRate = new ProductRate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ProductRateDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductRate = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Product Rate", e);
                #endregion
            }
            return oProductRate;
        }


        public List<ProductRate> Gets(int ProductID, Int64 nUserID)
        {
            List<ProductRate> oProductRates = new List<ProductRate>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductRateDA.Gets(tc, ProductID);
                oProductRates = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductRates", e);
                #endregion
            }
            return oProductRates;
        }
        public List<ProductRate> Gets( Int64 nUserID)
        {
            List<ProductRate> oProductRates = new List<ProductRate>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductRateDA.Gets(tc);
                oProductRates = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductRates", e);
                #endregion
            }
            return oProductRates;
        }
       
        #endregion
    }
}
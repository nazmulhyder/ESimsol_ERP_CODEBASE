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
    public class ProductStockService : MarshalByRefObject, IProductStockService
    {
        #region Private functions and declaration
        private ProductStock MapObject(NullHandler oReader)
        {
            ProductStock oProductStock = new ProductStock();
            oProductStock.ProductID = oReader.GetInt32("ProductID");
            oProductStock.BUID = oReader.GetInt32("BUID");
            oProductStock.ProductCode = oReader.GetString("ProductCode");
            oProductStock.ProductName = oReader.GetString("ProductName");            
            oProductStock.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oProductStock.ShortName = oReader.GetString("ShortName");            
            oProductStock.ProductCategoryID = oReader.GetInt32("ProductCategoryID");            
            oProductStock.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oProductStock.MUnitName = oReader.GetString("MUnitName");
            oProductStock.MUnit = oReader.GetString("MUnit");
            oProductStock.CurrentStock = oReader.GetDouble("CurrentStock");
            oProductStock.StockValue = oReader.GetDouble("StockValue");
            oProductStock.ShortQty = oReader.GetDouble("ShortQty");
            return oProductStock;
        }

        private ProductStock CreateObject(NullHandler oReader)
        {
            ProductStock oProductStock = new ProductStock();
            oProductStock = MapObject(oReader);
            return oProductStock;
        }

        private List<ProductStock> CreateObjects(IDataReader oReader)
        {
            List<ProductStock> oProductStock = new List<ProductStock>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductStock oItem = CreateObject(oHandler);
                oProductStock.Add(oItem);
            }
            return oProductStock;
        }

        #endregion

        #region Interface implementation
        public ProductStockService() { }
        public ProductStock SetReorderLevel(ProductStock oProductStock, int nUserID)
        {
            ProductStock oTempProductStock = new ProductStock();
            TransactionContext tc = null;            
            try
            {
                tc = TransactionContext.Begin(true);
                #region Product
                IDataReader reader;
                ProductStockDA.SetReorderLevel(tc, oProductStock, nUserID);
                oTempProductStock = new ProductStock();
                oTempProductStock.ProductID = oProductStock.ProductID;
                oTempProductStock.BUID = oProductStock.BUID;
                reader = ProductStockDA.Gets(tc, oTempProductStock);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductStock = new ProductStock();
                    oProductStock = CreateObject(oReader);
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

                oProductStock = new ProductStock();
                oProductStock.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oProductStock;
        }
        public List<ProductStock> Gets(ProductStock oProductStock, int nUserID)
        {
            List<ProductStock> oProductStocks = new List<ProductStock>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;

                reader = ProductStockDA.Gets(tc, oProductStock);
                oProductStocks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductStock", e);
                #endregion
            }
            return oProductStocks;
        }       
        #endregion
    }
}

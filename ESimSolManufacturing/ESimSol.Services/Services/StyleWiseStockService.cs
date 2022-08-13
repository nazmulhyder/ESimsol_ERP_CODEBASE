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
    public class StyleWiseStockService : MarshalByRefObject, IStyleWiseStockService
    {
        #region Private functions and declaration
        private StyleWiseStock MapObject(NullHandler oReader)
        {
            StyleWiseStock oStyleWiseStock = new StyleWiseStock();
            oStyleWiseStock.LotID = oReader.GetInt32("LotID");
            oStyleWiseStock.ProductID = oReader.GetInt32("ProductID");
            oStyleWiseStock.LotNo = oReader.GetString("LotNo");
            oStyleWiseStock.LogNo = oReader.GetString("LogNo");
            oStyleWiseStock.LotBalance = oReader.GetDouble("LotBalance");
            oStyleWiseStock.MUnitID = oReader.GetInt32("MUnitID");
            oStyleWiseStock.UnitPrice = oReader.GetDouble("UnitPrice");
            oStyleWiseStock.CurrencyID = oReader.GetInt32("CurrencyID");
            oStyleWiseStock.ParentLotID = oReader.GetInt32("ParentLotID");
            oStyleWiseStock.ParentType = (EnumTriggerParentsType)oReader.GetInt32("ParentType");
            oStyleWiseStock.ParentID = oReader.GetInt32("ParentID");
            oStyleWiseStock.StoreID = oReader.GetInt32("StoreID");
            oStyleWiseStock.BUID = oReader.GetInt32("BUID");
            oStyleWiseStock.StyleID = oReader.GetInt32("StyleID");
            oStyleWiseStock.ColorID = oReader.GetInt32("ColorID");
            oStyleWiseStock.SizeID = oReader.GetInt32("SizeID");
            oStyleWiseStock.ReqQty = oReader.GetDouble("ReqQty");
            oStyleWiseStock.CuttingQty = oReader.GetDouble("CuttingQty");
            oStyleWiseStock.ConsumptionQty = oReader.GetDouble("ConsumptionQty");
            oStyleWiseStock.OrderQty = oReader.GetDouble("OrderQty");
            oStyleWiseStock.ReceivedQty = oReader.GetDouble("ReceivedQty");
            oStyleWiseStock.IssueQty = oReader.GetDouble("IssueQty");
            oStyleWiseStock.DueQty = oReader.GetDouble("DueQty");
            oStyleWiseStock.StockBalance = oReader.GetDouble("StockBalance");
            oStyleWiseStock.ProductCode = oReader.GetString("ProductCode");
            oStyleWiseStock.ProductName = oReader.GetString("ProductName");
            oStyleWiseStock.MUnitSymbol = oReader.GetString("MUnitSymbol");
            oStyleWiseStock.MUnitName = oReader.GetString("MUnitName");
            oStyleWiseStock.Currency = oReader.GetString("Currency");
            oStyleWiseStock.StoreName = oReader.GetString("StoreName");
            oStyleWiseStock.StyleNo = oReader.GetString("StyleNo");
            oStyleWiseStock.BuyerName = oReader.GetString("BuyerName");
            oStyleWiseStock.SessionName = oReader.GetString("SessionName");
            oStyleWiseStock.ColorName = oReader.GetString("ColorName");
            oStyleWiseStock.POCode = oReader.GetString("POCode");
            oStyleWiseStock.SizeName = oReader.GetString("SizeName");
            oStyleWiseStock.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oStyleWiseStock.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oStyleWiseStock.SupplierID = oReader.GetInt32("SupplierID");
            oStyleWiseStock.SupplierName = oReader.GetString("SupplierName");
            oStyleWiseStock.BillNote = oReader.GetString("BillNote");
            oStyleWiseStock.ItemDescription = oReader.GetString("ItemDescription");
            oStyleWiseStock.StoreWithQty = oReader.GetString("StoreWithQty");
            oStyleWiseStock.ReturnQty = oReader.GetDouble("ReturnQty");
            oStyleWiseStock.BookingQty = oReader.GetDouble("BookingQty");
            oStyleWiseStock.BookingConsumption = oReader.GetDouble("BookingConsumption");
            oStyleWiseStock.BookingConsumptionInPercent = oReader.GetDouble("BookingConsumptionInPercent");
            oStyleWiseStock.TransferQty = oReader.GetDouble("TransferQty");
            return oStyleWiseStock;
        }

        private StyleWiseStock CreateObject(NullHandler oReader)
        {
            StyleWiseStock oStyleWiseStock = new StyleWiseStock();
            oStyleWiseStock = MapObject(oReader);
            return oStyleWiseStock;
        }

        private List<StyleWiseStock> CreateObjects(IDataReader oReader)
        {
            List<StyleWiseStock> oStyleWiseStock = new List<StyleWiseStock>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                StyleWiseStock oItem = CreateObject(oHandler);
                oStyleWiseStock.Add(oItem);
            }
            return oStyleWiseStock;
        }

        #endregion

        #region Interface implementation
        public StyleWiseStockService() { }     
        public List<StyleWiseStock> Gets(string sSQL, int nSelectedQty, Int64 nUserID)
        {
            List<StyleWiseStock> oStyleWiseStocks = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = StyleWiseStockDA.Gets(tc, sSQL, nSelectedQty);
                oStyleWiseStocks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StyleWiseStock", e);
                #endregion
            }
            return oStyleWiseStocks;
        }
        #endregion
    }
}

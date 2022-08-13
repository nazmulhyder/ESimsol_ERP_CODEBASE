using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region StyleWiseStock
    public class StyleWiseStock : BusinessObject
    {
        public StyleWiseStock()
        {
            LotID = 0;
            ProductID = 0;
            LotNo = "";
            LogNo = "";
            LotBalance = 0;
            MUnitID = 0;
            UnitPrice = 0;
            CurrencyID = 0;
            ParentLotID = 0;
            ParentType = EnumTriggerParentsType.None;
            ParentID = 0;
            StoreID = 0;
            BUID = 0;
            StyleID = 0;
            ColorID = 0;
            SizeID = 0;
            ReqQty = 0;
            CuttingQty = 0;
            ConsumptionQty = 0;
            OrderQty = 0;
            ReceivedQty = 0;
            IssueQty = 0;
            StockBalance = 0;
            ProductCode = "";
            ProductName = "";
            MUnitSymbol = "";
            MUnitName = "";
            Currency = "";
            StoreName = "";
            StyleNo = "";
            BuyerName = "";
            SessionName = "";
            ColorName = "";
            POCode = "";
            SizeName = "";
            ProductCategoryID = 0;
            ProductCategoryName = "";
            SupplierID = 0;
            SupplierName = "";
            SearchingData = "";
            DueQty = 0;
            ReturnQty = 0;
            BookingQty = 0;
            BookingConsumption = 0;
            BookingConsumptionInPercent = 0;
            ItemDescription = "";
            StoreWithQty = "";
            BillNote = "";
            TransferQty = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int LotID { get; set; }
        public int ProductID { get; set; }
        public string LotNo { get; set; }
        public string LogNo { get; set; }
        public double LotBalance { get; set; }
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public int CurrencyID { get; set; }
        public int ParentLotID { get; set; }
        public EnumTriggerParentsType ParentType { get; set; }
        public int ParentID { get; set; }
        public int StoreID { get; set; }
        public int BUID { get; set; }
        public int StyleID { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public double ReqQty { get; set; }
        public double CuttingQty { get; set; }
        public double ConsumptionQty { get; set; }
        public double OrderQty { get; set; }
        public double ReceivedQty { get; set; }
        public double IssueQty { get; set; }
        public double StockBalance { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUnitSymbol { get; set; }
        public string MUnitName { get; set; }
        public string Currency { get; set; }
        public string StoreName { get; set; }
        public string StyleNo { get; set; }
        public string BuyerName { get; set; }
        public string SessionName { get; set; }
        public string ColorName { get; set; }
        public string POCode { get; set; }
        public string SizeName { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ErrorMessage { get; set; }
        public double ReturnQty { get; set; }
        public double DueQty { get; set; }
        public double BookingQty { get; set; }
        public double BookingConsumption { get; set; }
        public double BookingConsumptionInPercent { get; set; }
        public string ItemDescription { get; set; }
        public string BillNote { get; set; }
        public string StoreWithQty { get; set; }
        public double TransferQty { get; set; }
        public string SearchingData { get; set; }
        #endregion

        #region Derived Property
        public Company Company { get; set; }
        #endregion

        #region Functions
        public static List<StyleWiseStock> Gets(string sSQL, int nSelectedQty, Int64 nUserID)
        {
            return StyleWiseStock.Service.Gets(sSQL, nSelectedQty, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IStyleWiseStockService Service
        {
            get { return (IStyleWiseStockService)Services.Factory.CreateService(typeof(IStyleWiseStockService)); }
        }
        #endregion
    }
    #endregion

    #region IStyleWiseStock interface

    public interface IStyleWiseStockService
    {
        List<StyleWiseStock> Gets(string sSQL, int nSelectedQty, Int64 nUserID);
    }
    #endregion
}
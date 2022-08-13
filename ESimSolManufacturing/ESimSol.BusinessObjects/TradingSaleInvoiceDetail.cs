using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region TradingSaleInvoiceDetail
    public class TradingSaleInvoiceDetail
    {
        #region  Constructor
        public TradingSaleInvoiceDetail()
        {
            TradingSaleInvoiceDetailID = 0;
            TradingSaleInvoiceID = 0;
            ProductID = 0;
            ItemDescription = "";
            MeasurementUnitID = 0;
            InvoiceQty = 0;
            UnitPrice = 0;
            Amount = 0;
            Discount = 0;
            NetAmount = 0;
            ProductCode = "";
            ProductName = "";
            UnitName = "";
            Symbol = "";
            ProductCategoryName = "";
            ErrorMessage = "";
            YetToChallanQty = 0;
            PurchaseAmount = 0;
            ReceivedQty = 0;
            CurrentStock = 0;
            LotNo = "";
            ChallanQty = 0;
            TradingSaleInvoiceDetailLogID = 0;
            TradingSaleInvoiceLogID = 0;
        }
        #endregion

        #region Properties
        public int TradingSaleInvoiceDetailID { get; set; }
        public int TradingSaleInvoiceID { get; set; }
        public int ProductID { get; set; }
        public string ItemDescription { get; set; }
        public int MeasurementUnitID { get; set; }
        public double InvoiceQty { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public double Discount { get; set; }
        public double NetAmount { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string Symbol { get; set; }
        public string ProductCategoryName { get; set; }
        public double YetToChallanQty { get; set; }
        public double PurchaseAmount { get; set; }
        public double CurrentStock { get; set; }
        public double ChallanQty { get; set; }
        public int TradingSaleInvoiceDetailLogID { get; set; }
        public int TradingSaleInvoiceLogID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public double ReceivedQty { get; set; }
        public string LotNo { get; set; }

        public double DiscountInPercentage
        {
            get
            {
                return (this.Discount > 0) ? (this.Discount * 100) / this.Amount : 0;
            }
        }
        #endregion

        #region Functions
        public TradingSaleInvoiceDetail Get(int nTradingSaleInvoiceDetailID, int nUserID)
        {
            return TradingSaleInvoiceDetail.Service.Get(nTradingSaleInvoiceDetailID, nUserID);
        }
        public TradingSaleInvoiceDetail Save(int nUserID)
        {
            return TradingSaleInvoiceDetail.Service.Save(this, nUserID);
        }
        public string Delete(int nUserID)
        {
            return TradingSaleInvoiceDetail.Service.Delete(this, nUserID);
        }
        public static List<TradingSaleInvoiceDetail> Gets(int nUserID)
        {
            return TradingSaleInvoiceDetail.Service.Gets(nUserID);
        }
        public static List<TradingSaleInvoiceDetail> Gets(int nTradingSaleInvoiceID, int nUserID)
        {
            return TradingSaleInvoiceDetail.Service.Gets(nTradingSaleInvoiceID, nUserID);
        }
        public static List<TradingSaleInvoiceDetail> GetsLog(int nTradingSaleInvoiceLogID, int nUserID)
        {
            return TradingSaleInvoiceDetail.Service.GetsLog(nTradingSaleInvoiceLogID, nUserID);
        }
        public static List<TradingSaleInvoiceDetail> Gets(string sSQl, int nUserID)
        {
            return TradingSaleInvoiceDetail.Service.Gets(sSQl, nUserID);
        }
        #endregion

        #region Non DB Function
        public string IDInString(List<TradingSaleInvoiceDetail> oTradingSaleInvoiceDetails)
        {
            string sReturn = "";
            foreach (TradingSaleInvoiceDetail oItem in oTradingSaleInvoiceDetails)
            {
                sReturn = sReturn + oItem.TradingSaleInvoiceDetailID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static ITradingSaleInvoiceDetailService Service
        {
            get { return (ITradingSaleInvoiceDetailService)Services.Factory.CreateService(typeof(ITradingSaleInvoiceDetailService)); }
        }
        #endregion
    }
    #endregion

    #region ITradingSaleInvoiceDetail interface
    public interface ITradingSaleInvoiceDetailService
    {
        TradingSaleInvoiceDetail Get(int id, int nUserID);
        List<TradingSaleInvoiceDetail> Gets(int nUserID);
        List<TradingSaleInvoiceDetail> Gets(string sSQL, int nUserID);
        List<TradingSaleInvoiceDetail> Gets(int nTradingSaleInvoiceID, int nUserID);
        List<TradingSaleInvoiceDetail> GetsLog(int nTradingSaleInvoiceLogID, int nUserID);
        TradingSaleInvoiceDetail Save(TradingSaleInvoiceDetail oTradingSaleInvoiceDetail, int nUserID);
        string Delete(TradingSaleInvoiceDetail oTradingSaleInvoiceDetail, int nUserID);
    }
    #endregion
}

using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;


namespace ESimSol.BusinessObjects
{
    public class TradingSaleInvoiceRegister : BusinessObject
    {
        public TradingSaleInvoiceRegister()
        {
            TradingSaleInvoiceID = 0;
            TradingSaleInvoiceDetailID = 0;
            InvoicePaymentID = 0;
            BUID = 0;
            BuyerID = 0;
            ProductID = 0;
            ProductName = "";
            ContactPersonID = 0;//SalseBY
            ContactPersonName = "";
            MeasurementUnitID = 0;
            Symbol = "";
            ErrorMessage = "";
            InvoiceNo = "";
            BuyerName = "";
            UnitPrice = 0;
            Amount = 0;
            GrossAmount = 0;
            Discount = 0;
            ChallanQty = 0;
            NetAmount = 0;
            CustomerName = "";
            InvoiceQty = 0;
            ItemDiscount = 0;
            ItemNetAmount = 0;
            DiscountAmount = 0;
            VatAmount = 0;
            ServiceCharge = 0;
            ReceivedAmount = 0;
            TotalReceivedAmount = 0;
            DueAmount = 0;
            PurchasePrice = 0;
            ProfitAmount = 0;
            ReceivedDate = DateTime.Now;
            ReceivedNo = "";
            ProductCode = "";
            SalseType = EnumSalesType.None;
            InvoiceDate = DateTime.Now;
        }
        #region Properties
        public int TradingSaleInvoiceID { get; set; }
        public int TradingSaleInvoiceDetailID { get; set; }
        public int InvoicePaymentID { get; set; }
        public int BUID { get; set; }
        public int BuyerID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public int ContactPersonID { get; set; }
        public string ContactPersonName { get; set; }
        public int MeasurementUnitID { get; set; }
        public string Symbol { get; set; }
        public string ErrorMessage { get; set; }
        public string InvoiceNo { get; set; }
        public string ReceivedNo { get; set; }
        public string ProductCode { get; set; }
        public DateTime ReceivedDate { get; set; } 
        public string BuyerName { get; set; }
        public double UnitPrice { get; set; }
        public double InvoiceQty { get; set; }
        public double ItemDiscount { get; set; }
        public double ItemNetAmount { get; set; }
        public double Amount { get; set; }
        public double ChallanQty { get; set; }
        public double GrossAmount { get; set; }
        public double Discount { get; set; }
        public double NetAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double VatAmount { get; set; }
        public double ServiceCharge { get; set; }
        public double ReceivedAmount { get; set; }
        public double TotalReceivedAmount { get; set; }
        public double DueAmount { get; set; }
        public double PurchasePrice { get; set; }
        public double ProfitAmount { get; set; }
        public EnumSalesType SalseType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion
        #region DerivedProperties
        public string SalesTypeInString
        {
            get
            {
                return this.SalseType.ToString();
            }
        }
        public string InvoiceDateInString
        {
            get
            {
                return this.InvoiceDate.ToString("dd MMM yyyy");
            }
        }
        public string ReceivedDateInString
        {
            get
            {
                return this.ReceivedDate.ToString("dd MMM yyyy");
            }
        }
        #endregion
        #region Fuinction
        public static List<TradingSaleInvoiceRegister> Gets(string sSql, int ReportLayout)
        {
            return TradingSaleInvoiceRegister.Service.Gets(sSql, ReportLayout);
        }
        #endregion
        #region ServiceFactory
        internal static ITradingSaleInvoiceRegisterService Service
        {
            get { return (ITradingSaleInvoiceRegisterService)Services.Factory.CreateService(typeof(ITradingSaleInvoiceRegisterService)); }
        }
        #endregion
    }
    public interface ITradingSaleInvoiceRegisterService
    {
        List<TradingSaleInvoiceRegister> Gets(string sSql, int ReportLayout);
    }
     
}

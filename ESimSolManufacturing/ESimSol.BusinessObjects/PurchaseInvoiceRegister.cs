using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region PurchaseInvoiceRegister
    public class PurchaseInvoiceRegister : BusinessObject
    {
        public PurchaseInvoiceRegister()
        {
            PurchaseInvoiceDetailID = 0;
            PurchaseInvoiceID = 0;
            ProductID = 0;
            MUnitID = 0;
            UnitPrice = 0;
            Qty = 0;
            ReceiveQty = 0;
            InvoiceID = 0;
            OrderRecapID = 0;
            GRNID = 0;
            InvoiceType = EnumPInvoiceType.None;
            InvoicePaymentMode = EnumInvoicePaymentMode.None;
            Amount = 0;
            PurchaseInvoiceNo = "";
            BUID = 0;
            BillNo = "";
            ApprovedDate = DateTime.MinValue;
            InvoiceStatus = EnumPInvoiceStatus.Initialize;
            ContractorID = 0;
            ContractorPersonalID = 0;
            CurrencyID = 0;
            ApprovedBy = 0;
            DateofInvoice = DateTime.MinValue;
            DeliveryTo = 0;
            DateofMaturity = DateTime.MinValue;
            ShipmentBy = EnumShipmentBy.None;
            Remarks = "";
            ApprovedByName = "";
            ProductCode = "";
            ProductName = "";
            MUName = "";
            MUSymbol = "";
            SupplierName = "";
            SCPName = "";
            CurrencyName = "";
            CurrencySymbol = "";
            ErrorMessage = "";
            SearchingData = "";
            OrderRecapID = 0;
            BuyerName = "";
            OrderRecapNo = "";
            StyleNo= "";
            BillToName = "";
            DeliveryToName = "";
            DateofBill = DateTime.Now;
            ReportLayout = EnumReportLayout.None;
        }

        #region Properties
        public int PurchaseInvoiceDetailID { get; set; }
        public int PurchaseInvoiceID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double Qty { get; set; }
        public double Amount { get; set; }
        public string PurchaseInvoiceNo { get; set; }
        public int BUID { get; set; }
        public string BillNo { get; set; }
        public DateTime ApprovedDate { get; set; }
        public EnumPInvoiceStatus InvoiceStatus { get; set; }
        public int ContractorID { get; set; }
        public int ContractorPersonalID { get; set; }
        public int CurrencyID { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime DateofInvoice { get; set; }
        public int DeliveryTo { get; set; }
        public DateTime DateofMaturity { get; set; }
        public EnumShipmentBy ShipmentBy { get; set; }
        public string Remarks { get; set; }
        public string ApprovedByName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public string SupplierName { get; set; }
        public string SCPName { get; set; }
        public double ReceiveQty { get; set; }
        public int InvoiceID { get; set; }
        public int GRNID { get; set; }
        public EnumPInvoiceType InvoiceType { get; set; }
        public EnumInvoicePaymentMode InvoicePaymentMode { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        public int OrderRecapID { get; set; }
        public string BuyerName { get; set; }
        public string OrderRecapNo { get; set; }
        public string StyleNo { get; set; }
        public string BillToName { get; set; }
        public string DeliveryToName { get; set; }
        public DateTime DateofBill { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property


        public string UnitPriceSt
        {
            get
            {
                if (this.OrderRecapID <= 1)
                {
                    return Global.MillionFormat(this.UnitPrice);
                }
                else
                {
                    return Global.MillionFormat(this.UnitPrice) + "/" + this.OrderRecapID.ToString();
                }
            }
        }
        public string ApproveDateSt
        {
            get 
            {
                if (this.ApprovedDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ApprovedDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string DateofInvoiceSt
        {
            get
            {
                if (this.DateofInvoice == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DateofInvoice.ToString("dd MMM yyyy");
                }
            }
        }
        public string DateofMaturitySt
        {
            get
            {
                if (this.DateofMaturity == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DateofMaturity.ToString("dd MMM yyyy");
                }
            }
        }

        public string DateofBillSt
        {
            get
            {
                if (this.DateofBill == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DateofBill.ToString("dd MMM yyyy");
                }
            }
        }

        public string PurchaseInvoiceStatusSt
        {
            get
            {
                return EnumObject.jGet(this.InvoiceStatus);
            }
        }

        #endregion

        #region Functions
        public static List<PurchaseInvoiceRegister> Gets(string sSQL, long nUserID)
        {
            return PurchaseInvoiceRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPurchaseInvoiceRegisterService Service
        {
            get { return (IPurchaseInvoiceRegisterService)Services.Factory.CreateService(typeof(IPurchaseInvoiceRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IPurchaseInvoiceRegister interface

    public interface IPurchaseInvoiceRegisterService
    {
        List<PurchaseInvoiceRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}

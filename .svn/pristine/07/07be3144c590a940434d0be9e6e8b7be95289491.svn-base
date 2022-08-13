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
    #region PurchaseOrder
    public class PurchaseOrderRegister : BusinessObject
    {
        public PurchaseOrderRegister()
        {
            POID=0;
            ContractorID=0;
            Qty=0;
            UnitPrice=0;
            ProductID=0;
            ProductCode="";
            ProductName="";
            UnitSymbol="";
            UnitName="";
            BuyerName="";
            OrderRecapNo="";
            StyleNo="";
            ContractorName="";
            ContractorShortName="";
            ApprovedByName="";
            PrepareByName="";
            ConcernPersonName="";
            ContactPersonName="";
            CurrencySymbol="";
            CurrencyName="";
            BUCode="";
            BUName="";
            DeliveryToName="";
            DeliveryToContactPersonName="";
            BillToName="";
            BIllToContactPersonName="";
            PaymentTermText="";
            ApproveDate = DateTime.MinValue;
            RefNo="";
            RefDate = DateTime.MinValue;
            RefBy = "";
            ModelShortName = "";
            Qty_Invoice=0;
            YetToGRNQty=0;
            GRNValue=0;
            YetToPurchaseReturnQty=0;
            LotBalance=0;
            LotID=0;
            LotNo = "";
            YetToInvoiceQty=0;
            AdvInvoice=0;
            AdvanceSettle=0;
            InvoiceValue=0;

            RefType = EnumPOReferenceType.Open;
            RefTypeInt = 0;
            Status = EnumPOStatus.Initialize;
            StatusInt = -1;
            ContractorID = 0;
        }

        #region Properties
        public int POID { get; set; }
        public int BUID { get; set; }
        public string PONo { get; set; }
        public DateTime PODate { get; set; }
        public EnumPOReferenceType RefType { get; set; }
        public int RefTypeInt { get; set; }
        public int RefID { get; set; }
        public EnumPOStatus Status { get; set; }
        public int StatusInt { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public DateTime RefDate { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ApprovedByName { get; set; }
        public string PrepareByName { get; set; }
        public string ConcernPersonName { get; set; }
        public string ContactPersonName { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; } // Currency In Word Before Decimal Point
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public string RefNo { get; set; }
        public string RefBy { get; set; }
        public double Amount { get; set; }
        public string DeliveryToName { get; set; }
        public string DeliveryToContactPersonName { get; set; }
        public string BillToName { get; set; }
        public string BIllToContactPersonName { get; set; }
        public string PaymentTermText { get; set; }
        public double CRate { get; set; }
        public string ErrorMessage { get; set; }
        public double DiscountInPercent{get; set;}
        public double DiscountInAmount { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UnitSymbol { get; set; }
        public string UnitName { get; set; }
        public double Qty_Invoice { get; set; }
        public double YetToGRNQty { get; set; }
        public double GRNValue { get; set; }
        public double YetToInvoiceQty { get; set; }
        public double AdvInvoice { get; set; }
        public double AdvanceSettle { get; set; }
        public double InvoiceValue { get; set; }
        public string BuyerName { get; set; }
        public string OrderRecapNo { get; set; }
        public double YetToPurchaseReturnQty { get; set; }
        public double LotBalance { get; set; }
        public string StyleNo { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public string ModelShortName { get; set; }
        public double YetToInvocieQty { get; set; }
        public int PaymentTermID { get; set; }
        public string ShipBy { get; set; }
        public string TradeTerm { get; set; }
        public string ContractorShortName { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public string SearchingData { get; set; }
        public string Remarks { get; set; }
        public int ApprovedBy { get; set; }
        #endregion

        #region Derived Property
        public string FullPONo
        {
            get
            {
                return "PO-" + this.PONo;
            }
        }
        public string PODateSt
        {
            get
            {
                return PODate.ToString("dd MMM yyyy");
            }
        }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string ApproveDateSt
        {
            get
            {
                return (ApproveDate==DateTime.MinValue?"": ApproveDate.ToString("dd MMM yyyy"));
            }
        }
        public string RefDateSt
        {
            get
            {
                return RefDate.ToString("dd MMM yyyy");
            }
        }
        public string POStatusSt
        {
            get
            {
                return this.Status.ToString();
            }
        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty);
            }
        }
        public string UnitPriceSt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.UnitPrice);
            }
        }
        #endregion

        #region Functions
        public static List<PurchaseOrderRegister> Gets(string sSQL, long nUserID)
        {
            return PurchaseOrderRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPurchaseOrderRegisterService Service
        {
            get { return (IPurchaseOrderRegisterService)Services.Factory.CreateService(typeof(IPurchaseOrderRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IPurchaseOrder interface
    public interface IPurchaseOrderRegisterService
    {
        List<PurchaseOrderRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion

}
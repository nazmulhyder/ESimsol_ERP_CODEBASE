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
    #region PurchaseInvoiceDetail

    public class PurchaseInvoiceDetail : BusinessObject
    {
        #region  Constructor
        public PurchaseInvoiceDetail()
        {
            PurchaseInvoiceDetailID = 0;
            PurchaseInvoiceID = 0;
            ProductID = 0;
            UnitPrice = 0;
            Qty = 0;
            ReceiveQty = 0;
            MUnitID = 0;
            RefDetailID = 0;
            GRNID = 0;
            RefID = 0;
            Amount = 0;
            AdvanceSettle = 0;            
            LCID = 0;
            InvoiceID = 0;
            CostHeadID = 0;
            Remarks = "";
            ProductCode = "";
            ProductName = "";
            ProductSpec = "";
            AccountHeadID = 0;
            MUName = "";
            MUSymbol = "";
            PODQty = 0;
            PODRate = 0;
            PODAmount = 0;
            LCNo = "";
            ShipmentDate = DateTime.Today;
            InvoiceNo = "";
            InvoiceDate = DateTime.Today;
            CostHeadCode = "";
            CostHeadName = "";
            ConvertionRate = 0;
            CurrencyName = "";
            CurrencySymbol = "";
            YetToInvoiceQty = 0;
            YetToGRNQty = 0;
            PrevoiusInvoice = 0;
            AdvInvoice = 0;
            BuyerName = "";
            OrderRecapNo = "";
            StyleNo = "";
            OrderRecapID = 0;
            ColorName = "";
            SizeName = "";
            CurrencyID = 0;
            WorkOrderNo = "";
            WOGRNQty = 0;
            YetToPurchaseReturnQty = 0;
            LotBalance = 0;
            LotID = 0;
            LotNo = "";
            ErrorMessage = "";
            ModelNo = "";
            ModelShortName = "";
            ProductWithGroupName = "";
            VehicleModelID = 0;
            InvoiceDetailID = 0;
            LandingCostType = EnumLandingCostType.Ledger;
            LandingCostTypeInt = (int)EnumLandingCostType.Ledger;
        }
        #endregion

        #region Properties
        public int PurchaseInvoiceDetailID { get; set; }
        public int VehicleModelID { get; set; }
        public string ModelNo { get; set; }
        public string ModelShortName { get; set; }
        public int PurchaseInvoiceID { get; set; }
        public int ProductID { get; set; }
        public double UnitPrice { get; set; }
        public double Qty { get; set; }
        public double ReceiveQty { get; set; }
        public int MUnitID { get; set; }
        public int RefDetailID { get; set; }
        public int GRNID { get; set; }
        public int RefID { get; set; }
        public double Amount { get; set; }
        public double AdvanceSettle { get; set; }    
        public int LCID { get; set; }
        public int InvoiceID { get; set; }
        public int CostHeadID { get; set; }
        public string Remarks { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductSpec { get; set; }
        public int AccountHeadID { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public double PODQty { get; set; }
        public double PODRate { get; set; }
        public double PODAmount { get; set; }
        public double WOGRNQty { get; set; }
        public string LCNo { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CostHeadCode { get; set; }
        public string CostHeadName { get; set; }
        public double ConvertionRate { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public double YetToInvoiceQty { get; set; }
        public double YetToGRNQty { get; set; }
        public double PrevoiusInvoice { get; set; }
        public double AdvInvoice { get; set; }
        public int OrderRecapID { get; set; }
        public string BuyerName { get; set; }
        public string OrderRecapNo { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string StyleNo { get; set; }
        public int CurrencyID { get; set; }
        public string WorkOrderNo { get; set; }
        public double YetToPurchaseReturnQty { get; set; }
        public double LotBalance { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public string ProductWithGroupName { get; set; }
        public int InvoiceDetailID { get; set; }
        public EnumLandingCostType LandingCostType { get; set; }
        public int LandingCostTypeInt { get; set; }
        public string ErrorMessage { get; set; }
        #region DerivedProperties
        public string RefNo { get; set; }
        public DateTime RefDate { get; set; }
        public double RefAmount { get; set; }
        
        public string AmountST
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string UnitPriceSt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.UnitPrice);
            }
        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty);
            }
        }
        public string ShipmentDateSt
        {
            get
            {
                if (this.ShipmentDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ShipmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string RefDateST
        {
            get
            {
                if (this.RefDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.RefDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string InvoiceDateSt
        {
            get
            {
                if (this.InvoiceDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.InvoiceDate.ToString("dd MMM yyyy");
                }
            }
        }
         #endregion

        #endregion

        #region Functions
        public PurchaseInvoiceDetail Get(int nPCDetailID, long nUserID)
        {
            return PurchaseInvoiceDetail.Service.Get(nPCDetailID, nUserID);
        }
        public PurchaseInvoiceDetail Save(long nUserID)
        {
            return PurchaseInvoiceDetail.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return PurchaseInvoiceDetail.Service.Delete(this, nUserID);
        }
        public static List<PurchaseInvoiceDetail> Gets(int PurchaseInvoiceID, long nUserID)
        {
            return PurchaseInvoiceDetail.Service.Gets(PurchaseInvoiceID, nUserID);
        }

        public static List<PurchaseInvoiceDetail> Gets(string sSQL, long nUserID)
        {
            return PurchaseInvoiceDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<PurchaseInvoiceDetail> GetsByPurchaseInvoiceID(int nPurchaseInvoiceId, long nUserID)
        {
            return PurchaseInvoiceDetail.Service.GetsByPurchaseInvoiceID(nPurchaseInvoiceId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPurchaseInvoiceDetailService Service
        {
            get { return (IPurchaseInvoiceDetailService)Services.Factory.CreateService(typeof(IPurchaseInvoiceDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IPurchaseInvoiceDetail interface
    public interface IPurchaseInvoiceDetailService
    {
        PurchaseInvoiceDetail Get(int nID, Int64 nUserId);
        List<PurchaseInvoiceDetail> Gets(int PurchaseInvoiceID, Int64 nUserId);
        List<PurchaseInvoiceDetail> Gets(string sSQL, Int64 nUserID);
        List<PurchaseInvoiceDetail> GetsByPurchaseInvoiceID(int nPurchaseInvoiceId, Int64 nUserID);
        string Delete(PurchaseInvoiceDetail oPurchaseInvoiceDetail, Int64 nUserId);
        PurchaseInvoiceDetail Save(PurchaseInvoiceDetail oPurchaseInvoiceDetail, Int64 nUserID);
    }
    #endregion
}

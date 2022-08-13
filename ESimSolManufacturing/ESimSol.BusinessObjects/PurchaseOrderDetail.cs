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
    #region PurchaseOrderDetail
    public class PurchaseOrderDetail : BusinessObject
    {
        public PurchaseOrderDetail()
        {
            PODetailID = 0;
            POID = 0;
            ProductID = 0;
            Qty = 0;
            UnitPrice = 0;
            MUnitID = 0;
            Note = "";
            ProductCode = "";
            ProductName = "";
            ProductSpec = "";
            UnitSymbol = "";
            UnitName = "";
            Qty_Invoice = 0;
            YetToGRNQty = 0;
            RefNo = "";
            GRNValue = 0;
            YetToInvoiceQty = 0;
            AdvInvoice = 0;
            AdvanceSettle = 0;
            InvoiceValue = 0;
            BuyerName = "";
            OrderRecapNo = "";
            StyleNo = "";
            OrderRecapID = 0;
            YetToPurchaseReturnQty = 0;
            LotBalance = 0;
            LotID = 0;
            LotNo = "";
            VehicleModelID = 0;
            ModelShortName = "";
            ErrorMessage = "";
            RefDetailID = 0;
            Specifications = "";
        }

        #region Properties
        public int PODetailID { get; set; }
        public int POID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public int MUnitID { get; set; }
        public string Note { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductSpec { get; set; }
        public string UnitSymbol { get; set; }
        public string UnitName { get; set; }
        public string RefNo { get; set; }
        public double Qty_Invoice { get; set; }
        public double YetToGRNQty { get; set; }
        public double GRNValue { get; set; }
        public double YetToInvoiceQty { get; set; }
        public double AdvInvoice { get; set; }
        public double AdvanceSettle { get; set; }
        public double InvoiceValue { get; set; }
        public int OrderRecapID { get; set; }
        public string BuyerName { get; set; }
        public string OrderRecapNo { get; set; }
        public double YetToPurchaseReturnQty { get; set; }
        public double LotBalance { get; set; }
        public string StyleNo { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public int VehicleModelID { get; set; }
        public string ModelShortName { get; set; }
        public string ErrorMessage { get; set; }
        public int RefDetailID { get; set; }
        #endregion

        #region Derived Property
        public string Specifications { get; set; }

        public double Qty_Entry
        {
            get
            {
                return this.Qty;
            }
        }
        public double UnitPrice_Entry
        {
            get
            {
                return this.UnitPrice;
            }
        }
        public double Amount_Entry
        {
            get
            {
                return this.Qty*this.UnitPrice;
            }
        }
        public string AmountST
        {
            get
            {
                return Global.MillionFormat(this.Qty * this.UnitPrice);
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
        public static List<PurchaseOrderDetail> Gets(int PurchaseOrderID, long nUserID)
        {
            return PurchaseOrderDetail.Service.Gets(PurchaseOrderID, nUserID);
        }
        public static List<PurchaseOrderDetail> GetsForInvoice(PurchaseInvoice oPurchaseInvoice, long nUserID)
        {
            return PurchaseOrderDetail.Service.GetsForInvoice(oPurchaseInvoice, nUserID);
        }
        public static List<PurchaseOrderDetail> Gets(string sSQL, long nUserID)
        {
            return PurchaseOrderDetail.Service.Gets(sSQL, nUserID);
        }

        public PurchaseOrderDetail Get(int PurchaseOrderDetailID, long nUserID)
        {
            return PurchaseOrderDetail.Service.Get(PurchaseOrderDetailID, nUserID);
        }
        public string Delete(long nUserID)
        {
            return PurchaseOrderDetail.Service.Delete(this, nUserID);
        }
        public PurchaseOrderDetail Save(long nUserID)
        {
            return PurchaseOrderDetail.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IPurchaseOrderDetailService Service
        {
            get { return (IPurchaseOrderDetailService)Services.Factory.CreateService(typeof(IPurchaseOrderDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IPurchaseOrderDetail interface

    public interface IPurchaseOrderDetailService
    {
        PurchaseOrderDetail Get(int PurchaseOrderDetailID, Int64 nUserID);
        List<PurchaseOrderDetail> Gets(int nPurchaseOrderID, Int64 nUserID);
        List<PurchaseOrderDetail> GetsForInvoice(PurchaseInvoice oPurchaseInvoice, long nUserID);
        List<PurchaseOrderDetail> Gets(string sSQL, Int64 nUserID);
        PurchaseOrderDetail Save(PurchaseOrderDetail oPurchaseOrderDetail, Int64 nUserID);
        string Delete(PurchaseOrderDetail oPurchaseRequisitionDetail, Int64 nUserID);
    }
    #endregion
}

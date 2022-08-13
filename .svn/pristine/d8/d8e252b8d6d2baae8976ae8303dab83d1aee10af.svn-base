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
    #region PurchaseQuotationDetail
    public class PurchaseQuotationDetail : BusinessObject
    {
        public PurchaseQuotationDetail()
        {
            PurchaseQuotationDetailLogID = 0;
            PurchaseQuotationDetailID = 0;
            PurchaseQuotationID = 0;
            ProductID = 0;
            ProductCode = "";
            ProductName = "";
            MUnitID = 0;
            UnitName = "";
            UnitSymbol = "";
            UnitPrice = 0;
            Quantity = 0;
            ApprovedBy = 0;
            CurrencySymbol = "";
            ApprovedDate = DateTime.Now;
            ItemDescription = "";
            ApprovedByName = "";
            CollectByName = "";
            SupplierName = "";
            PurchaseQuotationNo = "";
            IsCheck = false;
            SupplierID = 0;
            Discount = 0;
            Vat = 0;
            TransportCost = 0;
            Specifications = "";
            ErrorMessage = "";
            PRDetailID = 0;
            PRNo = "";
        }

        #region Properties

        public int PurchaseQuotationDetailLogID { get; set; }
        public int PurchaseQuotationDetailID { get; set; }

        public int PurchaseQuotationID { get; set; }
        public int PRDetailID { get; set; }
        public int ProductID { get; set; }

        public string ProductCode { get; set; }
        public string CurrencySymbol { get; set; }

        public string ProductName { get; set; }
        public string ProductSpec { get; set; }

        public string CollectByName { get; set; }
        public string SupplierName { get; set; }
        public string PurchaseQuotationNo { get; set; }
        public double ActualPrice { get; set; }
        public double Discount { get; set; }
        public double Vat { get; set; }
        public double TransportCost { get; set; }
        
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        public int MUnitID { get; set; }
        public bool IsCheck { get; set; }
        public int ApprovedBy { get; set; }

        public string UnitName { get; set; }

        public string UnitSymbol { get; set; }

        public DateTime ApprovedDate { get; set; }

        public string ItemDescription { get; set; }

        public string ApprovedByName { get; set; }
        public int SupplierID {get;set;}
        public string PaymentTerm { get; set; }
        public string PRNo { get; set; }
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string Specifications { get; set; }
        public double Discount_Amount
        {
            get
            {
                if (this.Discount > 0)
                {
                    return Math.Abs(this.ActualPrice - this.UnitPrice);
                }
                return 0;
                    
            }
        }

        public double ItemWiseTotalDiscount
        {
            get
            {
                if (this.Discount > 0)
                {
                    if (this.ActualPrice > 0)
                    {
                        return this.Discount_Amount * this.Quantity;
                    }
                }
                
                return 0;
                
            }
        }

        public double Vat_Amount
        {
            get
            {
                //return (this.ActualPrice * this.Vat)/100;
                if (this.Vat > 0)
                {
                    return Math.Abs(this.UnitPrice - this.ActualPrice);
                }
                return 0;
            }
        }
        public double ItemWiseTotalVat
        {
            get
            {
                if (this.Vat > 0)
                {
                    if (this.ActualPrice > 0)
                    {
                        return this.Vat_Amount * this.Quantity;
                    }
                }
                return 0;
            }
        }

        public double TransportCost_Amount
        {
            get
            {
                //return (this.ActualPrice * this.TransportCost)/100;
                if (this.TransportCost > 0)
                {
                    return Math.Abs(this.UnitPrice - this.ActualPrice);
                }
                return 0;
            }
        }
        public double ItemWiseTotalTransportCost
        {
            get
            {
                if (this.TransportCost > 0)
                {
                    if (this.ActualPrice > 0)
                    {
                        return this.TransportCost_Amount * this.Quantity;
                    }
                }
                return 0;
            }
        }
        public string Amountst
        {
            get
            {
                return Global.MillionFormat((this.Quantity*this.UnitPrice));
            }
        }

        public string ApprovedDateInString
        {
            get
            {
                if(this.ApprovedDate==DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ApprovedDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion

        #region Functions
        public static List<PurchaseQuotationDetail> Gets(int PurchaseQuotationID, long nUserID)
        {
            return PurchaseQuotationDetail.Service.Gets(PurchaseQuotationID, nUserID);
        }
        public static List<PurchaseQuotationDetail> GetsByLog(int PurchaseQuotationID, long nUserID)
        {
            return PurchaseQuotationDetail.Service.GetsByLog(PurchaseQuotationID, nUserID);
        }
        public static List<PurchaseQuotationDetail> Gets(string sSQL, long nUserID)
        {
            return PurchaseQuotationDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<PurchaseQuotationDetail> Approve(PurchaseQuotation oPurchaseQuotation, long nUserID)
        {
            return PurchaseQuotationDetail.Service.Approve(oPurchaseQuotation, nUserID);
        }
        public PurchaseQuotationDetail Get(int PurchaseQuotationDetailID, long nUserID)
        {
            return PurchaseQuotationDetail.Service.Get(PurchaseQuotationDetailID, nUserID);
        }
        public static List<PurchaseQuotationDetail> GetsForNOA(int nProductID, int nMunitID, long nUserID)
        {
            return PurchaseQuotationDetail.Service.GetsForNOA(nProductID, nMunitID, nUserID);
        }
        public static List<PurchaseQuotationDetail> GetsBy(int nProductID, int nMunitID, long nUserID)
        {
            return PurchaseQuotationDetail.Service.GetsBy(nProductID, nMunitID, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IPurchaseQuotationDetailService Service
        {
            get { return (IPurchaseQuotationDetailService)Services.Factory.CreateService(typeof(IPurchaseQuotationDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IPurchaseQuotationDetail interface

    public interface IPurchaseQuotationDetailService
    {

        PurchaseQuotationDetail Get(int PurchaseQuotationDetailID, Int64 nUserID);
        List<PurchaseQuotationDetail> Approve(PurchaseQuotation oPurchaseQuotation, Int64 nUserID);
        List<PurchaseQuotationDetail> Gets(int CourierServiceID, Int64 nUserID);
        List<PurchaseQuotationDetail> GetsByLog(int CourierServiceID, Int64 nUserID);
        List<PurchaseQuotationDetail> GetsForNOA(int nProductID, int nMunitID, Int64 nUserID);
        List<PurchaseQuotationDetail> GetsBy(int nProductID, int nMunitID, Int64 nUserID);
        List<PurchaseQuotationDetail> Gets(string sSQL, Int64 nUserID);
    }
    #endregion

}

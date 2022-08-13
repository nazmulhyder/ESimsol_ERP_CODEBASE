using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region CommercialInvoiceDetail
    
    public class CommercialInvoiceDetail : BusinessObject
    {
        public CommercialInvoiceDetail()
        {
            CommercialInvoiceDetailID = 0;
            CommercialInvoiceID = 0;
            ReferenceDetailID = 0;
            TechnicalSheetID = 0;
            OrderRecapID = 0;
            ShipmentDate = DateTime.Now;
            TransferQty = 0;
            InvoiceQty = 0;
            YetToInvoiceQty = 0;
            ShipmentQty = 0;
            PIDetailQty = 0;
            UnitPrice = 0;
            Discount = 0;
            FOB = 0;
            Amount = 0;
            StyleNo = "";
            OrderRecapNo = "";
            OrderNo = ""; 
            ProductName = "";
            Fabrication = "";
            InvoiceNo = "";
            InvoiceDate = DateTime.Now;
            HSCode = "";
            CAT = "";
            CartonQty = 0;
		    TotalGrossWeight = 0;
            TotalNetWeight = 0;
            TotalVolume = 0;
            DiscountInPercent = 0;
            AdditionInPercent = 0;
            UnitName = "";
            CurrencySymbol = "";
            MeasurementUnitID = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int CommercialInvoiceDetailID { get; set; }         
        public int CommercialInvoiceID { get; set; }
        public int ReferenceDetailID { get; set; }         
        public int TechnicalSheetID { get; set; }         
        public int OrderRecapID { get; set; }         
        public DateTime ShipmentDate { get; set; }         
        public double TransferQty { get; set; }         
        public double InvoiceQty { get; set; }         
        public double YetToInvoiceQty { get; set; }
        public double ShipmentQty { get; set; }
        public double PIDetailQty { get; set; }
        public double UnitPrice { get; set; }         
        public double Discount { get; set; }         
        public double FOB { get; set; }         
        public double Amount { get; set; }         
        public string StyleNo { get; set; }         
        public string OrderRecapNo { get; set; }         
        public string OrderNo { get; set; }         
        public string ProductName { get; set; }         
        public string Fabrication { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string BuyerName { get; set; }
        public string FactoryName { get; set; }
        public string HSCode { get; set; }
        public string CAT { get; set; }
        public double CartonQty { get; set; }
        public double TotalGrossWeight { get; set; }
        public double TotalNetWeight { get; set; }
        public double TotalVolume { get; set; }
        public double DiscountInPercent { get; set; }
        public double AdditionInPercent { get; set; }
        public string  UnitName{ get; set; }
		public string  CurrencySymbol { get; set; }
        public int MeasurementUnitID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        //
        public string InvoiceDateInString
        {
            get
            {
                return InvoiceDate.ToString("dd MMM yyyy");
            }
        }

        public string ShipmentDateInString
        {
            get
            {
                return ShipmentDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions

        public static List<CommercialInvoiceDetail> Gets(int ProformaInvoiceID, long nUserID)
        {
            return CommercialInvoiceDetail.Service.Gets(ProformaInvoiceID, nUserID);
        }
        public static List<CommercialInvoiceDetail> Gets(string sSQL, long nUserID)
        {
            return CommercialInvoiceDetail.Service.Gets(sSQL, nUserID);
        }
        public CommercialInvoiceDetail Get(int CommercialInvoiceDetailID, long nUserID)
        {
            
            return CommercialInvoiceDetail.Service.Get(CommercialInvoiceDetailID, nUserID);
        }
        public CommercialInvoiceDetail Save(long nUserID)
        {
            
            return CommercialInvoiceDetail.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static ICommercialInvoiceDetailService Service
        {
            get { return (ICommercialInvoiceDetailService)Services.Factory.CreateService(typeof(ICommercialInvoiceDetailService)); }
        }

        #endregion
    }
    #endregion

    #region ICommercialInvoiceDetail interface
     
    public interface ICommercialInvoiceDetailService
    {
         
        CommercialInvoiceDetail Get(int CommercialInvoiceDetailID, Int64 nUserID);
         
        List<CommercialInvoiceDetail> Gets(int ProformaInvoiceID, Int64 nUserID);
         
        List<CommercialInvoiceDetail> Gets(string sSQL, Int64 nUserID);
         
        CommercialInvoiceDetail Save(CommercialInvoiceDetail oCommercialInvoiceDetail, Int64 nUserID);
    }
    #endregion
}

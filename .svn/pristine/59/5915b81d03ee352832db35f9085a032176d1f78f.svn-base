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
    #region ProformaInvoiceDetail
    
    public class ProformaInvoiceDetail : BusinessObject
    {
        public ProformaInvoiceDetail()
        {
            ProformaInvoiceDetailID = 0;
            ProformaInvoiceID = 0;
            OrderRecapID = 0;
            TechnicalSheetID = 0;
            ShipmentDate = DateTime.Now;
            Quantity  = 0;
            FOB  = 0;
            BuyerCommissionInPercent = 0;
            BuyerCommission = 0;
            AdjustAdditon = 0;
            AdjustDeduction = 0;
            UnitPrice = 0;
            Amount = 0;
            OrderRecapNo ="";
            StyleNo ="";
            ProductName ="";
            FabricName ="";
            OrderRecapQty = 0;
            DEPT = 0;
            CMValue = 0;//Factory CM
            DeptName = "";
            ProformaInvoiceDetailLogID = 0;
            ProformaInvoiceLogID = 0;
            PINo = "";
            YetToTransfer = 0;
            YetToInvoiceQty = 0;
            InvoiceQty = 0;
            ShipmentQty = 0;
            SessionName = "";
            DiscountInPercent = 0;
            AdditionInPercent = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int ProformaInvoiceDetailID { get; set; }         
        public int ProformaInvoiceID { get; set; }         
        public int OrderRecapID { get; set; }         
        public int TechnicalSheetID { get; set; }         
        public DateTime ShipmentDate { get; set; }         
        public double Quantity { get; set; }         
        public double FOB { get; set; }         
        public double BuyerCommissionInPercent { get; set; }         
        public double BuyerCommission { get; set; }
        public double AdjustAdditon { get; set; }
        public double AdjustDeduction { get; set; } 
        public double UnitPrice { get; set; }         
        public double Amount { get; set; }
        public double CMValue { get; set; }
        public double YetToInvoiceQty { get; set; }
        public double InvoiceQty { get; set; }
        public double ShipmentQty { get; set; }
        public string OrderRecapNo { get; set; }         
        public string StyleNo { get; set; }         
        public string ProductName { get; set; }         
        public string FabricName { get; set; }         
        public double OrderRecapQty { get; set; }         
        public int DEPT { get; set; }         
        public int ProformaInvoiceDetailLogID { get; set; }         
        public int ProformaInvoiceLogID { get; set; }         
        public string PINo { get; set; }
        public double YetToTransfer { get; set; }         
        public string SessionName { get; set; }
        public string DeptName { get; set; }
        public double DiscountInPercent { get; set; }
        public double AdditionInPercent { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string ShipmentDateInString
        {
            get
            {
                return ShipmentDate.ToString("dd MMM yyyy");
            }
        }

        public string YeToTransferQtyInString
        {
            get
            {
                return Global.MillionFormat(this.YetToTransfer);
            }
        }

        public List<ProformaInvoiceDetail> ProformaInvoiceDetails { get; set; }

        #endregion

        #region Functions

        public static List<ProformaInvoiceDetail> Gets(int ProformaInvoiceID, long nUserID)
        {
            
            return ProformaInvoiceDetail.Service.Gets(ProformaInvoiceID, nUserID);
        }

        public static List<ProformaInvoiceDetail> GetsPILog(int ProformaInvoiceLogID, long nUserID)
        {
            return ProformaInvoiceDetail.Service.GetsPILog(ProformaInvoiceLogID, nUserID);
        }

        public static List<ProformaInvoiceDetail> Gets(string sSQL, long nUserID)
        {
            
            return ProformaInvoiceDetail.Service.Gets(sSQL, nUserID);
        }
        public ProformaInvoiceDetail Get(int ProformaInvoiceDetailID, long nUserID)
        {
            return ProformaInvoiceDetail.Service.Get(ProformaInvoiceDetailID, nUserID);
        }

        public ProformaInvoiceDetail Save(long nUserID)
        {
            return ProformaInvoiceDetail.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory

 
        internal static IProformaInvoiceDetailService Service
        {
            get { return (IProformaInvoiceDetailService)Services.Factory.CreateService(typeof(IProformaInvoiceDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IProformaInvoiceDetail interface
     
    public interface IProformaInvoiceDetailService
    {
         
        ProformaInvoiceDetail Get(int ProformaInvoiceDetailID, Int64 nUserID);
         
        List<ProformaInvoiceDetail> Gets(int ProformaInvoiceID, Int64 nUserID);
         
        List<ProformaInvoiceDetail> GetsPILog(int ProformaInvoiceLogID, Int64 nUserID);
        
         
        List<ProformaInvoiceDetail> Gets(string sSQL, Int64 nUserID);
         
        ProformaInvoiceDetail Save(ProformaInvoiceDetail oProformaInvoiceDetail, Int64 nUserID);


    }
    #endregion
}

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
    #region LCTransferDetail
    
    public class LCTransferDetail : BusinessObject
    {
        public LCTransferDetail()
        {
            LCTransferDetailID = 0;
            LCTransferID = 0;
            ProformaInvoiceDetailID = 0;
            TechnicalSheetID = 0;
            OrderRecapID = 0;
            TransferQty = 0;
            FOB = 0;
            Amount = 0;
            CommissionInPercent = 0;
            FactoryFOB = 0;
            CommissionPerPcs = 0;
            CommissionAmount = 0;
            OrderRecapNo = "";
            OrderNo = "";
            StyleNo = "";
            ProductName = "";
            Fabrication = "";
            PIDetailQty = 0;
            ShipmentDate = DateTime.Now;
            YeToTransferQty = 0;
            YetToInvoiceQty = 0;
            DiscountInPercent = 0;
            AdditionInPercent = 0;
            LCTransferDetailLogID = 0;
            LCTransferLogID = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int LCTransferDetailID { get; set; }
        public int LCTransferDetailLogID { get; set; }         
        public int LCTransferLogID { get; set; }         
        public int LCTransferID { get; set; }         
        public int ProformaInvoiceDetailID { get; set; }         
        public int TechnicalSheetID { get; set; }         
        public int OrderRecapID { get; set; }         
        public double TransferQty { get; set; }         
        public double FOB { get; set; }         
        public double Amount { get; set; }         
        public double CommissionInPercent { get; set; }
        public double FactoryFOB { get; set; }
        public double CommissionPerPcs { get; set; }         
        public double CommissionAmount { get; set; }         
        public string OrderRecapNo { get; set; }         
        public string OrderNo { get; set; }         
        public string StyleNo { get; set; }         
        public string ProductName { get; set; }         
        public string Fabrication { get; set; }         
        public string ErrorMessage { get; set; }         
        public double PIDetailQty { get; set; }         
        public double YeToTransferQty  { get; set; }         
        public double YetToInvoiceQty  { get; set; }
        public double DiscountInPercent { get; set; }
        public double AdditionInPercent { get; set; }  
        public DateTime ShipmentDate { get; set; }
        #endregion

        #region Derived Property
        public List<LCTransferDetail> LCTransferDetails { get; set; }
        public string YeToTransferQtyInString
        {
            get
            {
                return (this.YeToTransferQty).ToString("0");
            }
        }
        public double YeToTransferQtyInDouble
        {
            get
            {
                return (this.YeToTransferQty + this.TransferQty);
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
        public static List<LCTransferDetail> Gets(int id, long nUserID)
        {
            return LCTransferDetail.Service.Gets(id, nUserID);
        }
        public static List<LCTransferDetail> GetsLog(int id, long nUserID)
        {
            return LCTransferDetail.Service.GetsLog(id, nUserID);
        }
        public static List<LCTransferDetail> Gets(string sSQL, long nUserID)
        {
            return LCTransferDetail.Service.Gets(sSQL, nUserID);
        }
        public LCTransferDetail Get(int LCTransferDetailID, long nUserID)
        {           
            return LCTransferDetail.Service.Get(LCTransferDetailID, nUserID);
        }
        public LCTransferDetail Save(long nUserID)
        {
            
            return LCTransferDetail.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static ILCTransferDetailService Service
        {
            get { return (ILCTransferDetailService)Services.Factory.CreateService(typeof(ILCTransferDetailService)); }
        }


        #endregion
    }
    #endregion

    #region ILCTransferDetail interface
     
    public interface ILCTransferDetailService
    {
         
        LCTransferDetail Get(int id, Int64 nUserID);
         
        List<LCTransferDetail> Gets(int id, Int64 nUserID);
         
        List<LCTransferDetail> GetsLog(int id, Int64 nUserID);
         
        List<LCTransferDetail> Gets(string sSQL, Int64 nUserID);
         
        LCTransferDetail Save(LCTransferDetail oLCTransferDetail, Int64 nUserID);
    }
    #endregion
}

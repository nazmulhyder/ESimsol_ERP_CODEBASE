using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region OrderTracking

    public class OrderTracking : BusinessObject
    {
        public OrderTracking()
        {
            ExportSCDetailID = 0;
            ExportSCID = 0;
            ExportPIDetailID = 0;
            ExportPIID = 0;
            BUID = 0;
            ProductID = 0;
            ColorID = 0;
            PINo = "";
            PIDate = DateTime.Now;
            DODate= DateTime.Now;
            ExportLCFileNo= "";
            ExportLCNo= "";
            BuyerName= "";
            ContractorName= "";
            ProductCode= "";
            ProductName= "";
            ColorName= "";
            SizeName= "";
            StyleRef= "";
            MUName= "";
            StockUnitName = "";
            Remarks= "";
            DONo= "";
            ChallanNo= "";
            PIQty= 0;
            POQty= 0;
            YetToPO= 0;
            DOQty= 0;
            YetToDO= 0;
            ChallanQty= 0;
            YetToChallanQty= 0;
            StockQty= 0;
            HangerWeight= 0;
            WastagePercentWithWeight = 0;
            TotalWeight = 0;
            ProductNatureInInt = 0;
            PSQty= 0;
			PipeLineQty =0;
            ReturnQty = 0;
            PTUTransferQty  = 0;
            PTUTransferUnitName = "";
            ProductNature = EnumProductNature.Hanger;
            ProductionOrderDetails = new List<ProductionOrderDetail>();
            DeliveryOrderDetails = new List<DeliveryOrderDetail>();
            DeliveryChallanDetails = new List<DeliveryChallanDetail>();
            ProductionSheets = new List<ProductionSheet>();
            PTUUnit2Logs = new List<PTUUnit2Log>();
            ReturnChallanDetails = new List<ReturnChallanDetail>();
            PTUUnit2DistributionLogs = new List<PTUUnit2Distribution>();
            Params = "";
            bIsYetToChallan = false;
            bIsYetToDO = false;
            bIsYetToChallanWithDOEntry = false;
        }

        #region Properties
        public int ExportSCDetailID { get; set; }
        public int ExportSCID { get; set; }
        public int ExportPIDetailID { get; set; }
        public int ExportPIID { get; set; }
        public int BUID { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public int ProductNatureInInt { get; set; }
        public int ColorID { get; set; }
        public int ProductID { get; set; }
        public DateTime PIDate { get; set; }
        public string ColorName { get; set; }
        public DateTime DODate { get; set; }
        public string ExportLCFileNo { get; set; }
        public double PIQty { get; set; }
        public double POQty { get; set; }
        public double YetToPO { get; set; }
        public double DOQty { get; set; }
        public double YetToDO { get; set; }
        public double ChallanQty { get; set; }
        public double YetToChallanQty { get; set; }
        public double HangerWeight { get; set; }
        public double StockQty { get; set; }
        public double WastagePercentWithWeight { get; set; }
        public double TotalWeight { get; set; }
        public double PSQty { get; set; }
        public double PipeLineQty { get; set; }
        public double ReturnQty { get; set; }
        public int BuyerID { get; set; }
        public int ContractorID { get; set; }
        public string PINo { get; set; }
        public double PTUTransferQty  { get; set; }
        public string PTUTransferUnitName { get; set; }
        public string Remarks{get;set;}
        public string DONo{get;set;}
        public string ChallanNo{get;set;}
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string ProductCode { get; set; }
        public string ExportLCNo { get; set; }
        public string ProductName { get; set; } 
        public string SizeName { get; set; }
        public string StyleRef { get; set; }
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        public string SampleInvoiceNo { get; set; }
        public string MUName { get; set; }
        public string StockUnitName { get; set; }
        public int LayoutType { get; set; }
        public bool bIsYetToChallan { get; set; }
        public bool bIsYetToChallanWithDOEntry { get; set; }
        public bool bIsYetToDO { get; set; }
        public bool bIsPTUTransferQty { get; set; }
        
        #region derived properties
        public List<ProductionOrderDetail> ProductionOrderDetails { get; set; }
        public List<DeliveryOrderDetail> DeliveryOrderDetails { get; set; }
        public List<ProductionSheet> ProductionSheets { get; set; }
        public List<PTUUnit2Log> PTUUnit2Logs { get; set; }
        public List<DeliveryChallanDetail> DeliveryChallanDetails { get; set; }
        public List<ReturnChallanDetail> ReturnChallanDetails { get; set; }
        public List<ExportPIDetail> ExportPIDetails { get; set; }
        public List<PTUUnit2Distribution> PTUUnit2DistributionLogs { get; set; }
        public string DODateInString
        {
            get 
            { 
                if(this.DODate==DateTime.MinValue)
                {
                    return "";
                }
                else 
                {
                    return this.DODate.ToString("dd MMM yyyy");
                }
                
            }
        }

         public string PIDateInString
        {
            get { return this.PIDate.ToString("dd MMM yyyy"); }
        }
         public string PIQtyWithOutFormatting
         {
             get
             {
                 return Global.MillionFormatActualDigit(this.PIQty);
             }
         }

        //Ref for PO:1;PS:2;Pipelline:3; DO:4;Challan:5;Return:6;PIQty :7,PTUTransferQty =8
         public string POQtyInString
         {
             get
             {
                 return this.ExportPIID + "~" + this.ExportSCDetailID + "~" + this.ProductID + "~1~"+Global.MillionFormatActualDigit(this.POQty);
             }
         }

         public string YetToPOInString
         {
             get
             {
                 return Global.MillionFormatActualDigit(this.YetToPO);
             }
         }

         public string PSQtyInString
         {
             get
             {
                 return this.ExportPIID + "~" + this.ExportSCDetailID + "~" + this.ProductID + "~2~" + Global.MillionFormatActualDigit(this.PSQty);
             }
         }
         public string PipeLineQtyInString
         {
             get
             {
                 return this.ExportPIID + "~" + this.ExportSCDetailID + "~" + this.ProductID + "~3~" + Global.MillionFormatActualDigit(this.PipeLineQty);
             }
         }
         public string DOQtyInString
         {
             get
             {
                 return this.ExportPIID + "~" + this.ExportSCDetailID + "~" + this.ProductID + "~4~" + Global.MillionFormatActualDigit(this.DOQty);
             }
         }

         public string YetToDOInString
         {
             get
             {
                 return Global.MillionFormatActualDigit(this.YetToDO);
             }
         }


         public string ChallanQtyInString
         {
             get
             {
                 return this.ExportPIID + "~" + this.ExportSCDetailID + "~" + this.ProductID + "~5~" + Global.MillionFormatActualDigit(this.ChallanQty);
             }
         }

         public string YetToChallanQtyInString
         {
             get
             {
                 return Global.MillionFormatActualDigit(this.YetToChallanQty);
             }
         }

        
         public string ReturnQtyInString
         {
             get
             {
                 return this.ExportPIID + "~" + this.ExportSCDetailID + "~" + this.ProductID + "~6~" + Global.MillionFormatActualDigit(this.ReturnQty);
             }
         }
         public string PIQtyInString
         {
             get
             {
                 return this.ExportPIID + "~" + this.ExportSCDetailID + "~" + this.ProductID + "~7~" + Global.MillionFormatActualDigit(this.PIQty);
             }
         }
        public string StockQtyInString
        {
            get
            {
                return Global.MillionFormatActualDigit(this.StockQty) + " " + this.StockUnitName;
            }
        }
        public string PTUTransferQtyInString
        {
            get
            {
                return this.ExportPIID + "~" + this.ExportSCDetailID + "~" + this.ProductID + "~8~" + Global.MillionFormatActualDigit(this.PTUTransferQty) + " " + this.PTUTransferUnitName;
            }
        }
        #endregion

        #endregion
        #region Functions

        public static List<OrderTracking> Gets(string sSQL, int nLayeoutType,  bool bIsYetToDO, bool bIsYetToChallan,  bool bIsYetToChallanWithDOEntry, bool bIsPTUTransferQty, long nUserID)
        {
            return OrderTracking.Service.Gets(sSQL, nLayeoutType, bIsYetToDO, bIsYetToChallan, bIsYetToChallanWithDOEntry,bIsPTUTransferQty,  nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IOrderTrackingService Service
        {
            get { return (IOrderTrackingService)Services.Factory.CreateService(typeof(IOrderTrackingService)); }
        }
        #endregion

    }
    #endregion

    #region IOrderTracking interface

    public interface IOrderTrackingService
    {

        List<OrderTracking> Gets(string sSQL, int nLayeoutType, bool bIsYetToDO, bool bIsYetToChallan, bool bIsYetToChallanWithDOEntry, bool bIsPTUTransferQty, long nUserID);

    }
    #endregion
}

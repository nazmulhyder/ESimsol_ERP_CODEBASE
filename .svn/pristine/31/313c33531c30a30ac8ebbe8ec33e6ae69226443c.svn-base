using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExecutionOrderUpdateStatus 
    public class RptExecutionOrderUpdateStatus : BusinessObject
    {
        public RptExecutionOrderUpdateStatus()
        {
            FEOID = 0;
            FEONo = string.Empty; 
            OrderType = EnumOrderType.None;
            IsInHouse  = false;
            OrderDate = DateTime.MinValue;
            StartDate  = DateTime.MinValue;
            OrderQty  = 0;
            ProcessTypeName = string.Empty;
            BuyerID = 0;
            BuyerName = string.Empty;

            FabricID = 0;
            Construction = string.Empty;

            YTODate = string.Empty;
            YTOCount = string.Empty;
            YTOQty = string.Empty;

            DUIssueDate = string.Empty;
            DUYarnReceiveQty = 0;
            DEOCount = string.Empty;
            Color = string.Empty;
            DEOQty = string.Empty;
            WURecvQty = string.Empty;

            WUReceiveDate = string.Empty;
            WUColor = string.Empty;
            WUYarnReceiveQty = string.Empty;

            WarpingDate = string.Empty;
            WarpingPlanQty = 0;
            WarpingConsumeQty = 0;
            WarpingDoneQty = 0;

            SizingDate = string.Empty;
            SizingDoneQty = 0;

            LoomDate = string.Empty;
            LoomDoneQty = 0;

            InspectionDate = string.Empty;
            QcAndInspectionQty = 0;
            ReadyToDelivery = 0;
            DeliverdToFU = 0;
            TransferDate = string.Empty;

            FNReceiveDate = string.Empty;
            FNReceiveQty = 0;

            DeliveryDate = string.Empty;
            DeliveryQty = 0;
            ErrorMessage = "";
            Params = "";
            StyleRef = string.Empty;
            FactoryName = string.Empty;
            FinishType = string.Empty;
            FinishWidth = string.Empty;
            DEODate = DateTime.MinValue;
        }

        #region Properties
        public string StyleRef { get; set; }
        public string FactoryName { get; set; }
        public string FinishType { get; set; }
        public string FinishWidth { get; set; }
        public DateTime DEODate { get; set; }
        public int FEOID {get;set;} 
        public string FEONo  {get;set;} 
        public EnumOrderType OrderType  {get;set;}
        public bool IsInHouse {get;set;} 
        public DateTime OrderDate  {get;set;} 
        public DateTime StartDate  {get;set;} 
        public double OrderQty  {get;set;} 
        public string ProcessTypeName  {get;set;} 
        public DateTime ExpDelEndDate {get;set;}  
        public DateTime PPSampleDate {get;set;}
        public int BuyerID  {get;set;} 
        public string BuyerName  {get;set;} 

        public int FabricID  {get;set;} 
        public string Construction  {get;set;} 

        public string YTODate  {get;set;} 
        public string YTOCount  {get;set;} 
        public string YTOQty {get;set;} 


        public double DUYarnReceiveQty  {get;set;} 
        public string DUIssueDate { get; set; } 
        public string DEOCount  {get;set;} 
        public string Color  {get;set;} 
        public string DEOQty  {get;set;}
        public string WURecvQty { get; set; } 
        

        public string WUReceiveDate  {get;set;}
        public string WUColor { get; set; }
        public string WUYarnReceiveQty  {get;set;} 

        public string WarpingDate  {get;set;}  
        public double WarpingPlanQty {get;set;}
        public double WarpingConsumeQty { get; set; }
        public double WarpingDoneQty  {get;set;} 

        public string SizingDate  {get;set;} 
        public double SizingDoneQty  {get;set;} 

        public string LoomDate  {get;set;} 
        public double LoomDoneQty  {get;set;} 

        public string InspectionDate  {get;set;} 
        public double QcAndInspectionQty  {get;set;} 
        public double ReadyToDelivery  {get;set;} 
        public double DeliverdToFU  {get;set;} 
        public string TransferDate  {get;set;} 


        public string FNReceiveDate  {get;set;} 
        public double FNReceiveQty  {get;set;} 

        public string DeliveryDate  {get;set;}
        public double DeliveryQty { get; set; }

        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derive Properties
        public string OrderNo
        {
            get
            {
                string sPrifix = "";
                if (this.FEOID > 0)
                {
                    if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SCW";
                    //if (this.OrderType == EnumOrderType.Bulk) { sPrifix = sPrifix + "-BLK-"; }
                    //else if (this.OrderType == EnumOrderType.AkijExtra) { sPrifix = sPrifix + "-EXT-"; }
                    //else if (this.OrderType == EnumOrderType.Development) { sPrifix = sPrifix + "-DEV-"; }
                    //else if (this.OrderType == EnumOrderType.SMS) { sPrifix = sPrifix + "-SMS-"; }
                    //else { sPrifix = sPrifix + "-"; }
                    return sPrifix + this.FEONo;
                }
                else return "";
            }
        }

        public string ExpDelEndDateSt
        {
            get
            {
                return this.ExpDelEndDate.ToString("dd MMM yyyy");
            }
        }

        public string PPSampleDateSt
        {
            get
            {
                return this.PPSampleDate.ToString("dd MMM yyyy");
            }
        }

        public string OrderDateSt
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");
            }
        }

        public string StartDateSt
        {
            get
            {
                return this.StartDate.ToString("dd MMM yyyy");
            }
        }

        public string ExecutionStatus
        {
            get
            {
                if (!string.IsNullOrEmpty(this.DeliveryDate)) return "Delivered";
                else if (!string.IsNullOrEmpty(this.FNReceiveDate)) return "Recv. In Finishing";
                else if (!string.IsNullOrEmpty(this.TransferDate)) return "Transfer to Finishing";
                else if (!string.IsNullOrEmpty(this.InspectionDate)) return "Weaving QC & Inspection";
                else if (!string.IsNullOrEmpty(this.LoomDate)) return "In Loom";
                else if (!string.IsNullOrEmpty(this.SizingDate)) return "In Sizing";
                else if (!string.IsNullOrEmpty(this.WarpingDate)) return "In Warping";
                else return "";
            }
        }

        #endregion

        #region Functions

        public static List<RptExecutionOrderUpdateStatus> Gets(string sFEOIDs, long nUserID)
        {
            return RptExecutionOrderUpdateStatus.Service.Gets(sFEOIDs, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRptExecutionOrderUpdateStatusService Service
        {
            get { return (IRptExecutionOrderUpdateStatusService)Services.Factory.CreateService(typeof(IRptExecutionOrderUpdateStatusService)); }
        }
        #endregion
    }
    #endregion

    #region IRptExecutionOrderUpdateStatus interface
    public interface IRptExecutionOrderUpdateStatusService
    {
        List<RptExecutionOrderUpdateStatus> Gets(string sFEOIDs, long nUserID);
    }
    #endregion
}

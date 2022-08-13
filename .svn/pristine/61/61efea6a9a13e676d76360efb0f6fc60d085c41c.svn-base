using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region FNExecutionOrderStatus

    public class FNExecutionOrderStatus : BusinessObject
    {
        public FNExecutionOrderStatus()
        {
            FNExOID = 0;
            FEOID = 0;
            FNExONo = "";
            IssueDate = DateTime.Now;
            Remark = "";
            Quality = "";
            PeachStandard = "";
            StyleNo = "";
            ApproveByID = 0;
            ApproveByDate = DateTime.Now;
            ExpectedDeliveryDate = DateTime.Now;
            LCDate = DateTime.MinValue;
            ErrorMessage = "";
            Params = "";
            Construction = "";
            LCNo = "";
            ReviseCount = 0;
            OrderQty = 0;
            ProcessTypeName = "";
            OrderTypeInt = (int)EnumOrderType.None;
            FabricSalesContractDetailID = 0;
        }

        public int FNExOID { get; set; }
        public int FEOID { get; set; }
        public string FNExONo { get; set; }
        public DateTime IssueDate { get; set; }
        public string Remark { get; set; }
        public string Quality { get; set; }
        public string PeachStandard { get; set; }
        public string StyleNo { get; set; }
        public string Construction { get; set; }
        public string LCNo { get; set; }
        public int ApproveByID { get; set; }
        public string SCNo { get; set; }
        public string DispoNo { get; set; }
        public int FabricSalesContractID { get; set; }
        public int FabricSalesContractDetailID { get; set; }
        public string ReviseNo { get; set; }
        public int FabricID { get; set; }
        public int ProcessType { get; set; }
        public string ProcessTypeName { get; set; }
        public double DispoQty { get; set; }
        public double Balance { get; set; }
        public double ReadyStock { get; set; }
        public double InceptionQty { get; set; }
        public double RawFabricRcvQty { get; set; }
        public double OrderQty { get; set; }
        public DateTime ApproveByDate { get; set; }
        public EnumOrderType OrderType { get; set; }
        public int OrderTypeInt { get; set; } 
        public double PlannedQty { get; set; }
        public double BatchQty { get; set; }
        public double InspectionQty { get; set; }
        public double DeliveredQty { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public DateTime LCDate { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public int ReviseCount { get; set; }

        #region Derive Value
        public string BuyerName { get; set; }
        public string FabricNo { get; set; }
        public bool IsInHouse { get; set; }
        public string OrderQtyInStr
        {
            get{
                if (this.OrderQty > 0)
                {
                    return Global.MillionFormat(this.OrderQty) + "(Y)";
                }
                else
                {
                    return "-";
                }
            }
       
    }
        public string DispoPercent
        {
            get
            {
                if (this.DispoQty > 0)
                {
                    double temp = DispoQty - OrderQty;
                    if (temp > 0.4 && this.OrderQty>0)
                    {
                        return Global.MillionFormat(Math.Round((temp * 100 )/ this.OrderQty),2)+ "%";
                    }
                    else
                    {
                        return "-";
                    }
                }
                else
                {
                    return "-";
                }
            }
        }
       
        public string DispoQtyInStr
        {
            get{
                if (this.DispoQty > 0)
                {
                    return Global.MillionFormat(this.DispoQty) + "(Y)";
                }
                else
                {
                    return "-";
                }
            }
       
    }
        
        public string RawFabricRcvQtyInStr
        {
            get
            {
                if (this.RawFabricRcvQty > 0)
                {
                    return Global.MillionFormat(this.RawFabricRcvQty);
                }
                else
                {
                    return "-";
                }
            }

        }

        public string PlannedQtyInStr
        {
            get
            {
                if (this.PlannedQty > 0)
                {
                    return Global.MillionFormat(this.PlannedQty);
                }
                else
                {
                    return "-";
                }
            }
        }

        public string BatchQtyInStr
        {
            get
            {
                if (this.BatchQty > 0)
                {
                    return Global.MillionFormat(this.BatchQty);
                }
                else
                {
                    return "-";
                }
            }

        }
        public string DeliveredQtyInStr
        {
            get
            {
                if (this.DeliveredQty > 0)
                {
                    return Global.MillionFormat(this.DeliveredQty);
                }
                else
                {
                    return "-";
                }
            }

        }

        public string BalanceInStr
        {
            get
            {
                if (this.Balance > 0)
                {
                    return Global.MillionFormat(this.Balance);
                }
                else
                {
                    return "-";
                }
            }

        }
        //public string OrderNo
        //{
        //    get
        //    {
        //        string sPrifix = "";
        //        string sReviseCount = "";
        //        if (this.IsInHouse && this.FEOID > 0) { sPrifix = "EXE"; } else sPrifix = "FNC";

        //        if (this.OrderType == EnumOrderType.Bulk) { sPrifix = sPrifix + "-BLK-"; }
        //        else if (this.OrderType == EnumOrderType.AkijExtra) { sPrifix = sPrifix + "-EXT-"; }
        //        else if (this.OrderType == EnumOrderType.Development) { sPrifix = sPrifix + "-DEV-"; }
        //        else if (this.OrderType == EnumOrderType.SMS) { sPrifix = sPrifix + "-SMS-"; }
        //        else { sPrifix = sPrifix + "-"; }
        //        if (this.ReviseCount > 0) { sReviseCount = "-R" + this.ReviseCount; }
        //        return sPrifix + this.FNExONo + sReviseCount;
        //    }
        //}
        #endregion

        #region Functions
      
        public static List<FNExecutionOrderStatus> Gets(string sFNExONo,string sFNExOIDs,  long nUserID)
        {
            return FNExecutionOrderStatus.Service.Gets(sFNExONo, sFNExOIDs, nUserID);
        }
        public static List<FNExecutionOrderStatus> GetsReport(string sSQL, long nUserID)
        {
            return FNExecutionOrderStatus.Service.GetsReport(sSQL, nUserID);
        }
    
        #endregion

        #region ServiceFactory

        internal static IFNExecutionOrderStatusService Service
        {
            get { return (IFNExecutionOrderStatusService)Services.Factory.CreateService(typeof(IFNExecutionOrderStatusService)); }
        }
        #endregion
    }
    #endregion


    #region IFNExecutionOrderStatus interface

    public interface IFNExecutionOrderStatusService
    {
        List<FNExecutionOrderStatus> Gets(string sFNExONo, string sFNExOIDs, Int64 nUserID);
        List<FNExecutionOrderStatus> GetsReport(string sSQL, Int64 nUserID);

   
    }
    #endregion
}

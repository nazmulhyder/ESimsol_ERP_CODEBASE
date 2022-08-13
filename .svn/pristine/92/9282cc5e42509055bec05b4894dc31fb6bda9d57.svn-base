using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class WUStockReport
    {
        public WUStockReport()
        {
            FEOID = 0;
            FEONo = string.Empty;
            BuyerID = 0;
            FabricID = 0;
            OrderType = EnumOrderType.None;
            IsInHouse = false;
            OrderQty = 0;
            ProcessTypeName = string.Empty;
            BuyerName = string.Empty;
            Construction = string.Empty;
            CurrentStockQty = 0;
            StoreWiseReceive = string.Empty;
            TransferQty = 0; 
        }
        #region Properties
        public int FEOID { get; set; }
        public string FEONo { get; set; }
        public int BuyerID { get; set; }
        public int FabricID { get; set; }
        public EnumOrderType OrderType { get; set; }
        public bool IsInHouse { get; set; }
        public double OrderQty { get; set; }
        public string ProcessTypeName { get; set; }
        public string BuyerName { get; set; }
        public string Construction { get; set; }
        public double CurrentStockQty { get; set; }
        public string StoreWiseReceive { get; set; }
        public double TransferQty  { get; set; }
        
        public string ErrorMessage { get; set; }

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
        #endregion

        #region Functions
        public static List<WUStockReport> Gets(short orderType, int processType, string buyerIds, string feoIds, bool bIsCurrentStock, long nUserID)
        {
            return WUStockReport.Service.Gets(orderType, processType, buyerIds, feoIds, bIsCurrentStock, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IWUStockReportService Service
        {
            get { return (IWUStockReportService)Services.Factory.CreateService(typeof(IWUStockReportService)); }
        }
        #endregion
    }

    #region IWUStockReport interface
    public interface IWUStockReportService
    {
        List<WUStockReport> Gets(short orderType, int processType, string buyerIds, string feoIds, bool bIsCurrentStock, long nUserID);
    }
    #endregion
}

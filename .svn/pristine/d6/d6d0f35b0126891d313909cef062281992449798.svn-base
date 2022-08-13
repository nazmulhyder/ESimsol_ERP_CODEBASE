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
    public class FNOrderUpdateStatus
    {
        public FNOrderUpdateStatus()
        {
            FSCDetailID = 0;  
            ColorInfo = string.Empty;
            Construction= string.Empty;
            FabricID = 0;  
            OrderQty = 0;  
            SCNo = string.Empty;
            OrderType = 0;  
            BuyerID = 0;   
            FabricNo = string.Empty;
            BuyerName = string.Empty;
            SCNoFull = string.Empty;
            GradeAQty = 0;  
            GradeBQty = 0;  
            GradeCQty = 0;  
            GradeDQty = 0;
            RejQty = 0;
            OrderName = "";
            DeliveryQty = 0;
            StoreRcvQtyDay = 0;
            DCQtyDay = 0;
            ExcessDCQty = 0;
            ExcessQty = 0;
            ReqGreyRcv = 0;
            
        }
        #region Properties
        public int FSCDetailID { get; set; }
        public string ColorInfo { get; set; } 
        public string Construction { get; set; }
        public int FabricID { get; set; }
        public double OrderQty { get; set; }
        public string ExeNo { get; set; }
        public string SCNo { get; set; }
        public int OrderType { get; set; }
        public int BuyerID { get; set; }
        public string FabricNo { get; set; }
        public string BuyerName { get; set; }
        public string SCNoFull { get; set; }
        public int ContractorID { get; set; }
	    public string ContractorName { get; set; }
        public double GradeAQty{ get; set; } 
        public double GradeBQty{ get; set; } 
        public double GradeCQty{ get; set; } 
        public double GradeDQty{ get; set; }
        public double RejQty { get; set; } 
        public double DeliveryQty { get; set; }
        public string DONo { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public string OrderName { get; set; }
        public double GreyRecd { get; set; }
        public double BatchQty { get; set; }
        public double StoreRcvQty { get; set; }
        public double WForRcvQty { get; set; }
        public double DCQty { get; set; }
        public double RCQty { get; set; }
        public double StoreRcvQtyDay { get; set; }
        public double DCQtyDay { get; set; }
        public double StockInHand { get; set; }
        public double ExcessQty { get; set; }
        public double ExcessDCQty { get; set; }
        public double ReqGreyRcv { get; set; }
        
        #endregion

        #region Derive Property
        public double PendingGreyRcv
        {
            get
            {
                return (this.ReqGreyRcv - this.GreyRecd);
            }
        }
        public double TotalQty
        {
            get
            {
                return this.GradeAQty + this.GradeBQty + this.GradeCQty + this.GradeDQty + this.RejQty;
            }
        }

        public double BalanceQty
        {
            get
            {
                return Math.Round(this.OrderQty+this.RCQty-this.DCQty,2);
            }
        }

        public double WForRcvQtyInCalST
        {
            get
            {
                return (this.TotalQty - this.StoreRcvQty);
            }
        }
     

        #endregion
        #region Stock Report
        public string PINo { get; set; }
        public string MKTPerson { get; set; }
        public string Color { get; set; }
        public int FabricSalesContractID { get; set; }
        public int ReviseNo { get; set; }
        public bool IsInHouse { get; set; }
        public int ProcessType { get; set; }
        public int MKTPersonID { get; set; }
        public string ProcessTypeName { get; set; }
        public int FabricWeave { get; set; }
        public string WeaveName { get; set; }
        public string FabricWidth { get; set; }
        public double Balance { get; set; }
        public double DOQty { get; set; }
        public double QtyOpen { get; set; }
        public double QtyIn { get; set; }
        public double QtyOut { get; set; }
        public double RollNo { get; set; }
        public string Location { get; set; }
        public int DaysStay { get; set; }
        public double YetToDelivery
        {
            get
            {
                if (this.OrderQty < this.DeliveryQty) return 0;
                else  return Math.Round(( this.OrderQty - this.DeliveryQty),2);
            }
        }
        public double StockBalance
        {
            get
            {
                return this.QtyOpen + this.QtyIn - this.QtyOut;
            }
        }
        #endregion


        #region Functions
        public static List<FNOrderUpdateStatus> Gets(string sSQL, int nType, DateTime dtStart, DateTime dtEnd,  long nUserID)
        {
            return FNOrderUpdateStatus.Service.Gets(sSQL,nType, dtStart, dtEnd, nUserID);
        }
        public static List<FNOrderUpdateStatus> GetStockReport(DateTime dtStart, DateTime dtEnd, int nReportType, int nWorkingUnitID, long nUserID)
        {
            return FNOrderUpdateStatus.Service.GetStockReport(dtStart, dtEnd, nReportType, nWorkingUnitID,nUserID);
        }
        public static List<FNOrderUpdateStatus> Gets(string sSQL,  long nUserID)
        {
            return FNOrderUpdateStatus.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNOrderUpdateStatusService Service
        {
            get { return (IFNOrderUpdateStatusService)Services.Factory.CreateService(typeof(IFNOrderUpdateStatusService)); }
        }
        #endregion
    }

    #region IFNOrderUpdateStatus interface
    public interface IFNOrderUpdateStatusService
    {
        List<FNOrderUpdateStatus> Gets(string sSQL, int nType, DateTime dtStart, DateTime dtEnd, long nUserID);
        List<FNOrderUpdateStatus> GetStockReport(DateTime dtStart, DateTime dtEnd, int nReportType, int nWorkingUnitID, long nUserID);
        List<FNOrderUpdateStatus> Gets(string sSQL, long nUserID);

    }
    #endregion
}


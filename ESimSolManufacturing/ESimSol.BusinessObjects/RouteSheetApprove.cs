using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region RouteSheetApprove

    public class RouteSheetApprove : BusinessObject
    {
        public RouteSheetApprove()
        {
            ProductID = 0;
            ProductCode = string.Empty;
            ProductName = string.Empty;
            BaseName = string.Empty;
            ProductBaseID = 0;
            RSQty = 0;
            StockQty = 0;
            MUName = string.Empty;
            RSState = EnumRSState.Initialized;
            OrderType = 0;
            ColorName = "";
        }

        #region Properties
        public int ProductID { get; set; }

        public int LotID { get; set; }
        public int ProductBaseID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string BaseName { get; set; }
        public string MUName { get; set; }
        public string OrderNo { get; set; }
        public double Qty { get; set; }
        public double RSQty { get; set; }
        public double RSCount { get; set; }
        public double Qty_Booking { get; set; }
        public double StockQty { get; set; }
        public string LotNo { get; set; }
        public int RouteSheetID { get; set; }
        public string RouteSheetNo { get; set; }
        public DateTime RouteSheetDate { get; set; }
        public EnumRSState RSState { get; set; }
        public string ColorName { get; set; }
        public string WUnit { get; set; }
        public string RSCount_Product
        {
            get
            {
                return this.RSCount + "~" + this.ProductID;
            }
        }
        public string RSCount_Lot
        {
            get
            {
                return this.RSCount + "~" + this.LotID;
            }
        }
        public string RouteSheetDateSt
        {
            get
            {
                return this.RouteSheetDate.ToString("dd MMM yyyy");
            }
        }
        public string RSStateSt
        {
            get
            {
                return this.RSState.ToString();
            }
        }
        public string ErrorMessage { get; set; }
        public string Balance
        {
            get
            {
                if ((this.StockQty - this.RSQty -this.Qty_Booking) > 0)
                {
                    return Global.MillionFormat(this.StockQty - this.RSQty - this.Qty_Booking);
                }
                else
                {
                    return "(" + Global.MillionFormat((this.Qty_Booking+this.RSQty) - this.StockQty) + ")";
                }
            }
        }
        public int OrderType { get; set; }
        private string _sOrderNoFull = "";
        public string OrderNoFull
        {
            get
            {
                if (this.OrderType == (int)EnumOrderType.SampleOrder)
                {
                    _sOrderNoFull = "BSY-" + this.OrderNo;
                }
                else if (this.OrderType == (int)EnumOrderType.BulkOrder)
                {
                    _sOrderNoFull = "BPO-" + this.OrderNo;
                }
                else if (this.OrderType == (int)EnumOrderType.DyeingOnly)
                {
                    _sOrderNoFull = "BRD-" + this.OrderNo;
                }
                else if (this.OrderType == (int)EnumOrderType.ClaimOrder)
                {
                    _sOrderNoFull = "BCO-" + this.OrderNo;
                }
                else if (this.OrderType == (int)EnumOrderType.Sampling)
                {
                    _sOrderNoFull = "SP-" + this.OrderNo;
                }
                return _sOrderNoFull;
            }
        }
        #endregion

    


    #endregion


        #region Functions

        public static RouteSheetApprove Get(int nRouteSheetID, long nUserID)
        {
            return RouteSheetApprove.Service.Get(nRouteSheetID, nUserID);
        }
        public static List<RouteSheetApprove> Gets( int nReportType,string sSQL, Int64 nUserID)
        {
            return RouteSheetApprove.Service.Gets(nReportType,sSQL, nUserID);
        }
        public static List<RouteSheetApprove> Gets_LotWise(int nProductID, Int64 nUserID)
        {
            return RouteSheetApprove.Service.Gets_LotWise(nProductID, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IRouteSheetApproveService Service
        {
            get { return (IRouteSheetApproveService)Services.Factory.CreateService(typeof(IRouteSheetApproveService)); }
        }
        #endregion
    }

    #region IPIReport interface
    public interface IRouteSheetApproveService
    {
        RouteSheetApprove Get(int nRouteSheetID, long nUserID);
        List<RouteSheetApprove> Gets(int nReportType,string sSQL, Int64 nUserID);
        List<RouteSheetApprove> Gets_LotWise(int nProductID, Int64 nUserID);
        
    }
    #endregion
}

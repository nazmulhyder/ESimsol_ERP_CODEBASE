using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects.ReportingObject
{
    #region RptFEOSalesSummary

    public class RptFEOSalesSummary : BusinessObject
    {
        #region  Constructor
        public RptFEOSalesSummary()
        {
            MktPersonID = 0;
            BuyerID = 0;
            OrderType = EnumOrderType.None;
            OrderDate = DateTime.Now;
            YarnDyedQty = 0;
            SolidDyedQty = 0;
            BuyerName = string.Empty;
            MktPersonName = string.Empty;
            ErrorMessage = string.Empty;
            Params = "";
        }
        #endregion

        #region Properties
        public int MktPersonID  { get; set; }
        public int BuyerID { get; set; }
        public EnumOrderType OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public double YarnDyedQty { get; set; }
        public double SolidDyedQty { get; set; }
        public double YarnDyedValue { get; set; }
        public double SolidDyedValue { get; set; }
        public string BuyerName { get; set; }
        public string MktPersonName { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region 
        public string Params { get; set; }
        public double TotalQty { get { return this.YarnDyedQty + this.SolidDyedQty; } }

        public double TotalValue { get { return this.YarnDyedValue + this.SolidDyedValue; } }

        public string OrderDateStr { get { return this.OrderDate.ToString("MMM yyyy"); } }


        public double AvgYDQtyPerMonth { get; set; }
        public double AvgSDQtyPerMonth { get; set; }
        public double AvgQtyPerMonth { get { return this.AvgYDQtyPerMonth + this.AvgSDQtyPerMonth; } }

        public double YDRatioQty { get; set; }
        public double SDRatioQty { get; set; }
        public double RatioQty { get { return this.YDRatioQty + this.SDRatioQty; } }


        public double AvgYDValuePerMonth { get; set; }
        public double AvgSDValuePerMonth { get; set; }
        public double AvgValuePerMonth { get { return this.AvgYDValuePerMonth + this.AvgSDValuePerMonth; } }

        public double YDRatioValue { get; set; }
        public double SDRatioValue { get; set; }
        public double RatioValue { get { return this.YDRatioValue + this.SDRatioValue; } }

        #endregion

        #region Functions

        public static List<RptFEOSalesSummary> Gets(Int16 nOrderType, DateTime dtFrom, DateTime dtTo, bool bIsBuyerWise, Int16 nExeType, bool bIsOrderTypeWise, long nUserID)
        {
            return RptFEOSalesSummary.Service.Gets(nOrderType, dtFrom, dtTo, bIsBuyerWise, nExeType, bIsOrderTypeWise, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IRptFEOSalesSummaryService Service
        {
            get { return (IRptFEOSalesSummaryService)Services.Factory.CreateService(typeof(IRptFEOSalesSummaryService)); }
        }

        #endregion
    }
    #endregion

    #region IRptFEOSalesSummary interface

    public interface IRptFEOSalesSummaryService
    {
        List<RptFEOSalesSummary> Gets(Int16 nOrderType, DateTime dtFrom, DateTime dtTo, bool bIsBuyerWise, Int16 nExeType, bool bIsOrderTypeWise, long nUserID);

    }
    #endregion
}
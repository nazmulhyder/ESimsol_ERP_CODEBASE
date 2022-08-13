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


namespace ESimSol.BusinessObjects
{
    #region RouteSheetYarnOut

    public class RouteSheetYarnOut : BusinessObject
    {
        public RouteSheetYarnOut()
        {
            RouteSheetID = 0;
            RouteSheetNo = string.Empty;
            ProductID_Raw = 0;
            LotID = 0;
            Qty = 0;
            RSState = EnumRSState.None;
            EventTime = DateTime.MinValue;
            ProductCode = string.Empty;
            ProductName = string.Empty;
            LotNo = string.Empty;
            WorkingUnitID = 0;
            OperationUnitName = string.Empty;
            LocationName = string.Empty;
            ErrorMessage = string.Empty;
            OrderNo = string.Empty;
            Params = string.Empty;
            UserName = "";
            DyeingType = "";
            OrderType = 0;
        }

        #region Properties
        public int RouteSheetID { get; set; }
        public string RouteSheetNo { get; set; }
        public int ProductID_Raw { get; set; }
        public int LotID { get; set; }
        public double Qty { get; set; }
        public EnumRSState RSState { get; set; }
        public DateTime EventTime { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string LotNo { get; set; }
        public int WorkingUnitID { get; set; }
        public string OperationUnitName { get; set; }
        public string LocationName { get; set; }
        public string OrderNo { get; set; }
        public int OrderType { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public string UserName { get; set; }
        public int NoOfHanksCone { get; set; }
        public string DyeingType { get; set; }

        #endregion

        #region Aggregate
        private string _sOrderNoFull = "";
        public string OrderNoFull
        {
            get
            {
                _sOrderNoFull = this.OrderNo;
                return _sOrderNoFull;
            }
        }
        public string RSStateStr
        {
            get
            {
                return Global.EnumerationFormatter(this.RSState.ToString());
            }
        }
        public string EventTimeStr
        {
            get
            {
                return this.EventTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string WUName
        {
            get
            {
                return this.LocationName + "[" + this.OperationUnitName + "]";
            }
        }

        #endregion

        #region Functions
        public static List<RouteSheetYarnOut> Gets(string sSQL, long nUserID)
        {
            return RouteSheetYarnOut.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IRouteSheetYarnOutService Service
        {
            get { return (IRouteSheetYarnOutService)Services.Factory.CreateService(typeof(IRouteSheetYarnOutService)); }
        }
        #endregion


    }

    #endregion

    #region IRouteSheet interface
    public interface IRouteSheetYarnOutService
    {
        List<RouteSheetYarnOut> Gets(string sSQL, long nUserID);

    }
    #endregion

}
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
    #region RouteSheetCancel
    public class RouteSheetCancel : BusinessObject
    {
        public RouteSheetCancel()
        {
            RouteSheetID = 0;
            IsNewLot = false;
            WorkingUnitID = 0;
            Remarks = string.Empty;
            ApproveBy = 0;
            ApprovalRemarks = string.Empty;
            ErrorMessage = string.Empty;
            Params = string.Empty;
            RouteSheet = new RouteSheet();
            ProductID = 0;
            ProductName = "";
            OrderNo = "";
            Qty = 0;
            LotNo = "";
            BuyerName = "";
            ContractorName = "";
            DBServerDateTime = DateTime.Now;
            ReDyeingStatus = EnumReDyeingStatus.None;
        }

        #region Properties
        public int RouteSheetID { get; set; }
        public bool IsNewLot { get; set; }
        public int WorkingUnitID { get; set; }
        public string Remarks { get; set; }
        public int ApproveBy { get; set; }
        public string ApprovalRemarks { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public int ProductID { get; set; }
        public EnumReDyeingStatus ReDyeingStatus { get; set; }
        public string ProductName { get; set; }
        #endregion

        #region Derive Properties
        public string RequestedByName { get; set; }
        public string ApprovedByName { get; set; }
        public string OrderNo { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string LotNo { get; set; }
        public double Qty { get; set; }
        public string RouteSheetNo { get; set; }
        public string StoreName { get; set; }

        public string IsNewLotStr
        {
            get
            {
                return (this.IsNewLot) ? "New Lot" : "Main Lot";
            }
        }
        public DateTime DBServerDateTime { get; set; }
        

        public RouteSheet RouteSheet { get; set; }
        #endregion

        #region Functions
        public static RouteSheetCancel Get(int nId, long nUserID)
        {
            return RouteSheetCancel.Service.Get(nId, nUserID);
        }

        public static List<RouteSheetCancel> Gets(string sSQL, long nUserID)
        {
            return RouteSheetCancel.Service.Gets(sSQL, nUserID);
        }

        public RouteSheetCancel IUD(int nDBOperation, long nUserID)
        {
            return RouteSheetCancel.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IRouteSheetCancelService Service
        {
            get { return (IRouteSheetCancelService)Services.Factory.CreateService(typeof(IRouteSheetCancelService)); }
        }
        #endregion
    }
    #endregion

    #region IRouteSheetCancel interface

    public interface IRouteSheetCancelService
    {

        RouteSheetCancel Get(int id, long nUserID);
        List<RouteSheetCancel> Gets(string sSQL, long nUserID);
        RouteSheetCancel IUD(RouteSheetCancel oRouteSheetCancel, int nDBOperation, long nUserID);
    }
    #endregion
}

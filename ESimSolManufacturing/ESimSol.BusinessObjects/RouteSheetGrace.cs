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
    #region RouteSheetGrace

    public class RouteSheetGrace : BusinessObject
    {
        public RouteSheetGrace()
        {
            RouteSheetGraceID = 0;
            RouteSheetID = 0;
            DyeingOrderDetailID = 0;
            QtyGrace = 0;
            GraceCount = 0;
            Note = "";
            ApprovedByID = 0;
            RouteSheetDate = DateTime.Today;
            ApproveDate = DateTime.Today;
            NoteApp = "";
            LastUpdateBy = 0;
            LastUpdateDateTime = DateTime.Today;
            PrepareByName = "";
            ApprovedByName = "";
            ProductName = "";
            LabdipNo = "";
            LabDipDetailID = 0;
            ColorName = "";
            ColorNo = "";
            PantonNo = "";
            DyeingOrderID = 0;
            OrderQty = 0;
            ApproveLotNo = "";
            OrderNo = "";
            OrderDate = DateTime.Today;
            StyleNo = "";
            ContractorID = 0;
            ContractorName = "";
            DeliveryToName = "";
            OrderType = 0;
            OrderTypeSt = "";
            NoCode = "";
            Qty_Pro = 0;
            RSState = 0;
            RouteSheetNo = "";
            QtyRS = 0;
            WastageQty = 0;
            RecycleQty = 0;
            QtyGrace_Previous = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int RouteSheetGraceID { get; set; }
        public int RouteSheetID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public double QtyGrace { get; set; }
        public int GraceCount { get; set; }
        public string Note { get; set; }
        public int ApprovedByID { get; set; }
        public DateTime ApproveDate { get; set; }
        public string NoteApp { get; set; }
        public int LastUpdateBy { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string PrepareByName { get; set; }
        public string ApprovedByName { get; set; }
        public string ProductName { get; set; }
        public string LabdipNo { get; set; }
        public int LabDipDetailID { get; set; }
        public string ColorName { get; set; }
        public string ColorNo { get; set; }
        public string PantonNo { get; set; }
        public int DyeingOrderID { get; set; }
        public string ApproveLotNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string StyleNo { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public double OrderQty { get; set; }
        public string DeliveryToName { get; set; }
        public int OrderType { get; set; }
        public string OrderTypeSt { get; set; }
        public string NoCode { get; set; }
        public double Qty_Pro { get; set; }
        public int RSState { get; set; }
        public string RouteSheetNo { get; set; }
        public DateTime RouteSheetDate { get; set; }
        public double QtyRS { get; set; }
        public double QtyRS_Previous { get; set; }
        public double QtyGrace_Previous { get; set; }
        public double RecycleQty { get; set; }
        public double WastageQty { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ApproveDateSt
        {
            get
            {
                if (this.ApproveDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.ApproveDate.ToString("dd MMM yyyy HH:mm");
                }
            }
        }
        public string LastUpdateDateTimeSt
        {
            get
            {
                if (this.LastUpdateDateTime == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.LastUpdateDateTime.ToString("dd MMM yyyy HH:mm");
                }
            }
        }
        public string RouteSheetDateSt
        {
            get
            {
                if (this.RouteSheetDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.RouteSheetDate.ToString("dd MMM yyyy");
                }
            }
        }
        public double YetToProduction
        {
            get
            {
                return this.OrderQty - this.Qty_Pro;
            }
        }
        public string YetToProductionSt
        {
            get
            {
                if ((this.OrderQty - this.Qty_Pro) < 0) return Global.MillionFormat(this.OrderQty - this.Qty_Pro);
                else return Global.MillionFormat(this.OrderQty - this.Qty_Pro);
            }
        }
        public double QtyGraceForApprove
        {
            get
            {
                if (this.ApprovedByID > 0 || this.ApprovedByID==-9)
                {
                    return Math.Round(this.QtyGrace, 2);
                }
                else
                {
                    return 0.00;
                }
            }
        }
        #endregion

        #region Functions

        public static List<RouteSheetGrace> Gets(long nUserID)
        {
            return RouteSheetGrace.Service.Gets(nUserID);
        }
        public static List<RouteSheetGrace> Gets(string sSQL, Int64 nUserID)
        {
            return RouteSheetGrace.Service.Gets(sSQL, nUserID);
        }
        public RouteSheetGrace Get(int nId, long nUserID)
        {
            return RouteSheetGrace.Service.Get(nId, nUserID);
        }
        public RouteSheetGrace GetByRS(int nRSID, long nUserID)
        {
            return RouteSheetGrace.Service.GetByRS(nRSID, nUserID);
        }
        public RouteSheetGrace Save(RouteSheetGrace oRouteSheetGrace, long nUserID)
        {
            return RouteSheetGrace.Service.Save(oRouteSheetGrace, nUserID);
        }
        public RouteSheetGrace Approve(RouteSheetGrace oRouteSheetGrace, long nUserID)
        {
            return RouteSheetGrace.Service.Approve(oRouteSheetGrace, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return RouteSheetGrace.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRouteSheetGraceService Service
        {
            get { return (IRouteSheetGraceService)Services.Factory.CreateService(typeof(IRouteSheetGraceService)); }
        }
        #endregion
    }
    #endregion

    #region IRouteSheetGrace interface

    public interface IRouteSheetGraceService
    {
        RouteSheetGrace Get(int id, long nUserID);
        RouteSheetGrace GetByRS(int nRSID, long nUserID);
        List<RouteSheetGrace> Gets(long nUserID);
        List<RouteSheetGrace> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        RouteSheetGrace Save(RouteSheetGrace oRouteSheetGrace, long nUserID);
        RouteSheetGrace Approve(RouteSheetGrace oRouteSheetGrace, long nUserID);
    }
    #endregion
}


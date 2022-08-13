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
    #region RouteSheetDO

    public class RouteSheetDO : BusinessObject
    {
        public RouteSheetDO()
        {
            RouteSheetDOID = 0;
            DyeingOrderDetailID = 0;
            OrderNo = "";
            OrderDate = DateTime.Now;
            ProductName = "";
            OrderDate = DateTime.Now;
            Qty_RS = 0.0;
            Qty_QC = 0.0;
            Qty_Finish = 0.0;
            StyleNo = "";
            OrderType = EnumOrderType.None;
            ContractorID = 0;
            SMUnitValue = 0;
            NoCode = "";
            Qty_PSD = 0;
            ApproveLotNo = "";
            Note = "";
            DeliveryToName = "";
            SLNo = 0;
            HankorCone = 0;
            DyeingOrderID = 0;
            RecycleQty=0;
            WastageQty = 0;
            RSState = EnumRSState.None;
            RoutesheetNo = "";
            BuyerRef = "";
            RefNo = "";
            StyleNo = "";
        }

        #region Properties
        public int RouteSheetDOID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int DyeingOrderID { get; set; }
        public int RouteSheetID { get; set; }
        public double Qty_RS { get; set; }
        public double Qty_Pro { get; set; }
        public double Qty_QC { get; set; }
        public double Qty_PSD { get; set; }
        public double Qty_Finish { get; set; }
        public string OrderNo { get; set; }
        public string ClaimOrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductName { get; set; }
        public string LabdipNo { get; set; }
        public string ColorName { get; set; }
        public string ColorNo { get; set; }
        public string PantonNo { get; set; }
        public EnumShade Shade { get; set; }
        public string StyleNo { get; set; }
        public string RefNo { get; set; }
        public EnumOrderType OrderType { get; set; }
        public EnumRSState RSState { get; set; }
        public int OrderTypeInt { get; set; }
        public short HankorCone { get; set; }
        public int SLNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region Derived Property
        public string ContractorName { get; set; }
        public string DeliveryToName { get; set; }//Buyer Name
        public double OrderQty { get; set; }
        public double RecycleQty { get; set; }
        public double WastageQty { get; set; }
        public int PTUID { get; set; }
        public int LabDipDetailID { get; set; }
        public int ProductID { get; set; }
        public int ContractorID { get; set; }
        public string NoCode { get; set; }
        public string ApproveLotNo { get; set; }
        public string BuyerRef { get; set; }
        public string RoutesheetNo { get; set; }
        public string Note { get; set; }
        public double SMUnitValue { get; set; }
        public string OrderDateSt
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string ShadeSt
        {
            get
            {
                return this.Shade.ToString();
            }
        }
        public string OrderTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumOrderType)this.OrderType);
            }
        }
        private double _nQty_RSKG;
        public double Qty_RSKG
        {
            get
            {
                _nQty_RSKG = Math.Round((this.Qty_RS * this.SMUnitValue),3);//0.45359237001003542909395360718511
                return _nQty_RSKG;
            }
        }
        private double _nOrderQtyKG;
        public double OrderQtyKG
        {
            get
            {
                _nOrderQtyKG = Math.Round((this.OrderQty * this.SMUnitValue),3); //0.45359237001003542909395360718511
                return _nOrderQtyKG;
            }
        }
        private string _sOrderNoFull = "";
        public string OrderNoFull
        {
            get
            {
                if (string.IsNullOrEmpty(this.NoCode)) { _sOrderNoFull =  this.OrderNo; }
                else { _sOrderNoFull = this.NoCode.Trim() + this.OrderNo; }

                return _sOrderNoFull.Trim();
            }
        }
       
        #endregion

        #region Functions
        public static RouteSheetDO Get(int nId, long nUserID)
        {
            return RouteSheetDO.Service.Get(nId, nUserID);
        }
        public static List<RouteSheetDO> Gets(string sSQL, long nUserID)
        {
            return RouteSheetDO.Service.Gets(sSQL, nUserID);
        }

        public static List<RouteSheetDO> GetsBy(int nRouteSheetID, long nUserID)
        {
            return RouteSheetDO.Service.GetsBy(nRouteSheetID, nUserID);
        }
      
        public RouteSheetDO Save(long nUserID)
        {
            return RouteSheetDO.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return RouteSheetDO.Service.Delete(this, nUserID);
        }
        public static List<RouteSheetDO> GetsDOYetTORS(string sSQL, long nUserID)
        {
            return RouteSheetDO.Service.GetsDOYetTORS(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRouteSheetDOService Service
        {
            get { return (IRouteSheetDOService)Services.Factory.CreateService(typeof(IRouteSheetDOService)); }
        }
        #endregion
    }


    #region IRouteSheetDO interface

    public interface IRouteSheetDOService
    {
        RouteSheetDO Get(int id, long nUserID);
        List<RouteSheetDO> Gets(string sSQL, long nUserID);
        List<RouteSheetDO> GetsBy(int nRouteSheetID, long nUserID);
        RouteSheetDO Save(RouteSheetDO oRouteSheetDO, long nUserID);
        string Delete(RouteSheetDO oRouteSheetDO, long nUserID);
        List<RouteSheetDO> GetsDOYetTORS(string sSQL, long nUserID);

    }
    #endregion

    #endregion
}

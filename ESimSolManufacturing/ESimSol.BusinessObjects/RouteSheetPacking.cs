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
    #region RouteSheetPacking
    public class RouteSheetPacking : BusinessObject
    {
        public RouteSheetPacking()
        {
            PackingID = 0;
            RouteSheetID = 0;
            Weight = 0;
            NoOfHankCone = 0;
            BagNo = 0;
            Note = string.Empty;
            Warp = EnumWarpWeft.None;
            Length = string.Empty;
            PackedByEmpID = 0;
            DyeingOrderDetailID = 0;
            YarnType = EnumYarnType.FreshDyedYarn;
            ErrorMessage = "";
            Params = "";
            Date = DateTime.Now;
            RouteSheetPackings = new List<RouteSheetPacking>();
            QCBYName = "";
            QCDate = DateTime.MinValue;
            QtyPack = 0;
            QtyGW = 0;
            LDPE = 0;
            HDPE = 0;
            CTN = 0;
            DUHardWindingID = 0;
        }

        #region Properties
        public int PackingID { get; set; }
        public int RouteSheetID { get; set; }
        public double Weight { get; set; }
        public int NoOfHankCone { get; set; }
        public int BagNo { get; set; }
        public string Note { get; set; }
        public EnumWarpWeft Warp { get; set; }
        public string Length { get; set; }
        public int PackedByEmpID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int DUHardWindingID { get; set; }
        public EnumYarnType YarnType { get; set; }
        public EnumRSBagType BagType { get; set; }
        public double BagWeight { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public DateTime QCDate { get; set; }
        public DateTime Date { get; set; }
        public double QtyGW { get; set; }
        public double LDPE { get; set; }
        public double HDPE { get; set; }
        public double CTN { get; set; }
        #endregion

        #region Derive Properties
        public double WeightWithMunitValue { get; set; }
        public double QtyGWWithMunitValue { get; set; }
        public double QtyPack { get; set; }
        public string RouteSheetNo { get; set; }
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public string ColorName { get; set; }
        public string ProductName { get; set; }
        public string QCBYName { get; set; }
        public string WarpStr
        {
            get
            {
                return this.Warp.ToString();
            }
        }
        public string YarnTypeSt
        {
            get
            {
                return this.YarnType.ToString();
            }

        }
        public string DateSt
        {
            get
            {
                return this.Date.ToString("dd MMM yyyy");
            }

        }
        public string QCDateSt
        {
            get
            {
                if (this.QCDate == DateTime.MinValue) return "";
                return this.QCDate.ToString("dd MMM yyyy");
            }

        }
        public double WeightTwo
        {
            get
            {
                return Global.GetKG(this.Weight,10);
            }
        }

        public string GrossWeight
        {
            get
            {
                return Global.MillionFormat(this.Weight+this.BagWeight);
            }
        }

        public List<RouteSheetPacking> RouteSheetPackings { get; set; }
        #endregion

        #region Functions
        public static RouteSheetPacking Get(int nId, long nUserID)
        {
            return RouteSheetPacking.Service.Get(nId, nUserID);
        }

        public static List<RouteSheetPacking> Gets(string sSQL, long nUserID)
        {
            return RouteSheetPacking.Service.Gets(sSQL, nUserID);
        }

        public RouteSheetPacking IUD(int nDBOperation, long nUserID)
        {
            return RouteSheetPacking.Service.IUD(this, nDBOperation, nUserID);
        }

        public List<RouteSheetPacking> PackingMultiple(int nBag, long nUserID)
        {
            return RouteSheetPacking.Service.PackingMultiple(this, nBag, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IRouteSheetPackingService Service
        {
            get { return (IRouteSheetPackingService)Services.Factory.CreateService(typeof(IRouteSheetPackingService)); }
        }
        #endregion
    }
    #endregion

    #region IRouteSheetPacking interface

    public interface IRouteSheetPackingService
    {

        RouteSheetPacking Get(int id, long nUserID);
        List<RouteSheetPacking> Gets(string sSQL, long nUserID);
        RouteSheetPacking IUD(RouteSheetPacking oRouteSheetPacking, int nDBOperation, long nUserID);
        List<RouteSheetPacking> PackingMultiple(RouteSheetPacking oRouteSheetPacking, int nBag, long nUserID);
        
    }
    #endregion
}

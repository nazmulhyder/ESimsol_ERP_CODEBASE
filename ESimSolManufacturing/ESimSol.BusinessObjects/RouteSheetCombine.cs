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
    #region RouteSheetCombine
    
    public class RouteSheetCombine : BusinessObject
    {
        public RouteSheetCombine()
        {
            RouteSheetCombineID = 0;
            ProductionScheduleID = 0;
            CombineRSDate = DateTime.Now;
            RSNo_Combine = "";
            CombineRSDate = DateTime.Now;
            TotalQty = 0.0;
            TotalLiquor = 0.0;
            TtlCotton = 0;
            RouteSheetID = 0;
            RSNo_Combine = "";
            RouteSheetNo = "";
            Note = "";
            OrderType = 0;
            Params = "";
            ApproveByName = "";
            CombineRSDate =DateTime.Now;
            RouteSheetDetail = new RouteSheetDetail();
        }
       
        #region Properties
        public int RouteSheetCombineID { get; set; }
        public int ProductionScheduleID { get; set; }
        public int RouteSheetID { get; set; }//As like Templet RS
        public double TotalQty { get; set; }
        public double TotalLiquor { get; set; }
        public double TtlCotton { get; set; }
        public string RSNo_Combine { get; set; }
         public string RouteSheetNo { get; set; }
         public string Note { get; set; }
         public string ContractorName { get; set; }
         public string OrderNo { get; set; }
         public string ApproveByName { get; set; }
         public int OrderType { get; set; }
         public int ApproveBy { get; set; }
        public DateTime CombineRSDate { get; set; }
        public List<RouteSheetCombineDetail> RouteSheetCombineDetails { get; set; }
        public RouteSheetDetail RouteSheetDetail { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        
        #endregion
        #region Derived Property
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
                    _sOrderNoFull = "BPO-" + this.OrderNo;
                }
                return _sOrderNoFull;
            }
        }
        public string CombineRSDateSt
        {
            get
            {
                return this.CombineRSDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public  RouteSheetCombine Get(int nId, long nUserID)
        {
            return RouteSheetCombine.Service.Get(nId, nUserID);
        }
        public RouteSheetCombine GetBy(int nRSID, long nUserID)
        {
            return RouteSheetCombine.Service.GetBy(nRSID, nUserID);
        }
        public static List<RouteSheetCombine> Gets(string sSQL, long nUserID)
        {
            return RouteSheetCombine.Service.Gets(sSQL, nUserID);
        }
        
        public static List<RouteSheetCombine> GetsBy(int nRouteSheetID, long nUserID)
        {
            return RouteSheetCombine.Service.GetsBy(nRouteSheetID, nUserID);
        }
        public static List<RouteSheetCombine> Gets(int nPTUID, long nUserID)
        {
            return RouteSheetCombine.Service.Gets(nPTUID, nUserID);
        }
        public RouteSheetCombine Save( long nUserID)
        {
            return RouteSheetCombine.Service.Save(this,  nUserID);
        }
        public RouteSheetCombine Approve(long nUserID)
        {
            return RouteSheetCombine.Service.Approve(this, nUserID);
        }
        public RouteSheetCombine UndoApprove(long nUserID)
        {
            return RouteSheetCombine.Service.UndoApprove(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return RouteSheetCombine.Service.Delete(this,nUserID);
        }
    
        #endregion

        #region ServiceFactory
        internal static IRouteSheetCombineService Service
        {
            get { return (IRouteSheetCombineService)Services.Factory.CreateService(typeof(IRouteSheetCombineService)); }
        }
        #endregion
    }


    #region IRouteSheetCombine interface
    
    public interface IRouteSheetCombineService
    {
        RouteSheetCombine Get(int id, long nUserID);
        RouteSheetCombine GetBy(int nRSID, long nUserID);
        List<RouteSheetCombine> Gets(string sSQL, long nUserID);
        List<RouteSheetCombine> GetsBy(int nRouteSheetID, long nUserID);
        List<RouteSheetCombine> Gets(int nPTUID, long nUserID);
        RouteSheetCombine Save(RouteSheetCombine oRouteSheetCombine,  long nUserID);
        RouteSheetCombine Approve(RouteSheetCombine oRouteSheetCombine, long nUserID);
        RouteSheetCombine UndoApprove(RouteSheetCombine oRouteSheetCombine, long nUserID);
        string Delete(RouteSheetCombine oRouteSheetCombine, long nUserID);
    }
    #endregion

    #endregion
}

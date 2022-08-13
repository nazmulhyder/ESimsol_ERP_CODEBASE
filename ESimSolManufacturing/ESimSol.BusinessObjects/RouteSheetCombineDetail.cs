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
    #region RouteSheetCombineDetail
    
    public class RouteSheetCombineDetail : BusinessObject
    {
        public RouteSheetCombineDetail()
        {
            RouteSheetCombineDetailID = 0;
            RouteSheetID = 0;
            LocationID = 0;
            RouteSheetDate = DateTime.Now;
            RouteSheetNo = "";
            ProductID_Raw = 0;
            LotID = 0;
            PTUID = 0;
            ProductID_Raw = 0;
            LotID = 0;
            LocationID = 0;
            MachineID = 0;
            TtlLiquire = 0;
            TtlCotton = 0;
            WorkingUnitID = 0;
            DyeingOrderDetailID = 0;
            RouteSheetDOs = new List<RouteSheetDO>();
            DUPScheduleID = 0;
            QtyDye = 0;
            NoOfHanksCone = 0;
            HanksCone = 0;
        }
       
        #region Properties
        public int RouteSheetCombineDetailID { get; set; }
        public int RouteSheetCombineID { get; set; }
        public int RouteSheetID { get; set; }
        public int DUPScheduleID { get; set; }
        public string RouteSheetNo { get; set; }
        public DateTime RouteSheetDate { get; set; }
        public int ProductID_Raw { get; set; }
        public int LotID { get; set; }
        public int PTUID { get; set; }
        public int LocationID { get; set; }
        public int MachineID { get; set; }
        public double Qty { get; set; }
        public double QtyDye { get; set; }
        public EnumRSState RSState { get; set; }
        public string Note { get; set; }
        public double TtlLiquire { get; set; }
        public double TtlCotton { get; set; }
        public string MachineName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductName_Raw { get; set; }
        public string LotNo { get; set; }
        public int OrderType { get; set; }
        public int WorkingUnitID { get; set; }
        public string OperationUnitName { get; set; }
        public string LocationName { get; set; }
        public int LabDipDetailID { get; set; }
        public string ColorName { get; set; }
        public string PantonNo { get; set; }
        public string ColorNo { get; set; }
        public string ColorNameShade
        {
            get { return (this.ColorName + "[" + ((EnumShade)this.Shade).ToString() + "]"); }
        }
        public string LabdipNo { get; set; }
        public int Shade { get; set; }
        public int HanksCone { get; set; }
        public int NoOfHanksCone { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region Derived Property
        public string RSStateStr
        {
            get
            {
                return this.RSState.ToString();
            }
        }
        public int RouteSheetDOID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public string OrderNo { get; set; }
        public double QtyKg
        {
            get
            {
                return Global.GetKG(this.Qty, 10);
            }
        }
        public List<RouteSheetDO> RouteSheetDOs { get; set; }
        #endregion

        #region Functions
        public static RouteSheetCombineDetail Get(int nId, long nUserID)
        {
            return RouteSheetCombineDetail.Service.Get(nId, nUserID);
        }
        public static List<RouteSheetCombineDetail> Gets(string sSQL, long nUserID)
        {
            return RouteSheetCombineDetail.Service.Gets(sSQL, nUserID);
        }
        
        public static List<RouteSheetCombineDetail> GetsBy(int nRouteSheetID, long nUserID)
        {
            return RouteSheetCombineDetail.Service.GetsBy(nRouteSheetID, nUserID);
        }
        public static List<RouteSheetCombineDetail> Gets(int nRSCID, long nUserID)
        {
            return RouteSheetCombineDetail.Service.Gets(nRSCID, nUserID);
        }
        public RouteSheetCombineDetail Save( long nUserID)
        {
            return RouteSheetCombineDetail.Service.Save(this,  nUserID);
        }
        public string Delete(long nUserID)
        {
            return RouteSheetCombineDetail.Service.Delete(this,nUserID);
        }
      
        #endregion

        #region ServiceFactory
        internal static IRouteSheetCombineDetailService Service
        {
            get { return (IRouteSheetCombineDetailService)Services.Factory.CreateService(typeof(IRouteSheetCombineDetailService)); }
        }
        #endregion
    }


    #region IRouteSheetCombineDetail interface
    
    public interface IRouteSheetCombineDetailService
    {
        RouteSheetCombineDetail Get(int id, long nUserID);
        List<RouteSheetCombineDetail> Gets(string sSQL, long nUserID);
        List<RouteSheetCombineDetail> GetsBy(int nRouteSheetID, long nUserID);
        List<RouteSheetCombineDetail> Gets(int nRSCID, long nUserID);
        RouteSheetCombineDetail Save(RouteSheetCombineDetail oRouteSheetCombineDetail,  long nUserID);
        string Delete(RouteSheetCombineDetail oRouteSheetCombineDetail, long nUserID);
       
       
    }
    #endregion

    #endregion
}

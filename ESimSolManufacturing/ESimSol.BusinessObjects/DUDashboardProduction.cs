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
    #region DUDashboardProduction
    public class DUDashboardProduction
    {
        public DUDashboardProduction()
        {
            LocationID = 0;
            Qty_Out = 0;
            Qty_Hydro = 0;
            Qty_Dryer = 0;
            Qty_WQC = 0;
            Qty_QCD = 0;
            Qty_UnManage = 0;
            Qty_WForStore = 0;
            Qty_Cancel = 0;
            Qty_Gain = 0;
            Qty_Loss = 0;
            Qty_Recycle = 0;
            Qty_Wastage = 0;
            Qty_Fresh = 0;
            Params = string.Empty;
            ErrorMessage = string.Empty;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            ID = 0;
            Qty_DC = 0;
            Qty_Manage = 0;
            Qty_Received = 0;
        }
        #region pproperties
        public int LocationID {get; set;}
        public double Qty_Out { get; set; }
        public double Qty_Hydro { get; set; }
        public double Qty_Dryer { get; set; }
        public double Qty_WQC { get; set; }
        public double Qty_QCD { get; set; }
        public double Qty_UnManage { get; set; }
        public double Qty_WForStore { get; set; }
        public double Qty_Machine { get; set; }
        public double Qty_Cancel { get; set; }
        public double Qty_Gain { get; set; }
        public double Qty_Loss { get; set; }
        public double Qty_Recycle { get; set; }
        public double Qty_Wastage { get; set; }
        public double Qty_Fresh { get; set; }
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDate { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double StockInHand { get; set; }
        public double Qty_Manage { get; set; }
        public double Qty_DC { get; set; }
        public double Qty_Received { get; set; }
        public double Qty { get; set; }
        public double ID { get; set; }
        public DateTime RouteSheetDate { get; set; }
        public string RouteSheetNo { get; set; }
        public string OrderNo { get; set; }
        public string RouteSheetDateSt { get { return (this.RouteSheetDate == DateTime.MinValue) ? "-" : this.RouteSheetDate.ToString("dd MMM yyyy"); } }

        #endregion
        #region Functions
        public  static List<DUDashboardProduction> Gets(DUDashboardProduction oDUDashboardProduction,long nUserID)
        {
            return DUDashboardProduction.Service.Gets(oDUDashboardProduction, nUserID);
        }
        public static List<DUDashboardProduction> Gets_Daily(DUDashboardProduction oDUDashboardProduction, long nUserID)
        {
            return DUDashboardProduction.Service.Gets_Daily(oDUDashboardProduction, nUserID);
        }
        public static DUDashboardProduction Get(DUDashboardProduction oDUDashboardProduction, long nUserID)
        {
            return DUDashboardProduction.Service.Get(oDUDashboardProduction, nUserID);
        }
        public static List<DUDashboardProduction> Gets(string sSQL, long nUserID)
        {
            return DUDashboardProduction.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDUDashboardProductionService Service
        {
            get { return (IDUDashboardProductionService)Services.Factory.CreateService(typeof(IDUDashboardProductionService)); }
        }
        #endregion
    }
    #endregion
    #region DUDashboardProduction Interface

    public interface IDUDashboardProductionService
    {

        DUDashboardProduction Get(DUDashboardProduction oDUDashboardProduction, Int64 nUserID);
        List<DUDashboardProduction> Gets(DUDashboardProduction oDUDashboardProduction, Int64 nUserID);
        List<DUDashboardProduction> Gets_Daily(DUDashboardProduction oDUDashboardProduction, Int64 nUserID);
        List<DUDashboardProduction> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}

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
    #region RouteSheet

    public class RouteSheet : BusinessObject
    {
        public RouteSheet()
        {
            RouteSheetID = 0;
            RouteSheetNo = string.Empty;
            RouteSheetDate = DateTime.Today;
            MachineID = 0;
            ProductID_Raw  = 0;
            LotID = 0;
            Qty = 0;
            RSState = EnumRSState.Initialized;
            LocationID = 0;
            PTUID = 0;
            DUPScheduleID = 0;
            Note = string.Empty;
            TtlLiquire = 0;
            TtlCotton = 0;
            HanksCone = 0;
            NoOfHanksCone = 0;
            CopiedFrom = 0;
            PrepareBy = 0;
            ApproveBy = 0;
            ErrorMessage = string.Empty;
            Params = string.Empty;
            RouteSheetDetail = new RouteSheetDetail();
            PTU = new PTU();
            RSHistorys = new List<RouteSheetHistory>();
            RouteSheetPackings = new List<RouteSheetPacking>();
            RSInQCDetails = new List<RSInQCDetail>();
            RouteSheetDOs = new List<RouteSheetDO>();
            DBServerDateTime = DateTime.Now;

            //EventTime = DateTime.MinValue;
            LabDipDetailID = 0;
            IsReDyeing = EnumReDyeingStatus.None;
            ColorName = "";
            DyeingType = "";
            NoCode = "";
            RSShiftID = 0;
            RSRawLots = new List<RSRawLot>();
            Shift = "";
            RecipeByName = "";
            QtyDye = 0;
            DyeingProgress = new RouteSheetHistory();
        }

        #region Properties
        public int RouteSheetID { get; set; }
        public string RouteSheetNo { get; set; }
        public DateTime RouteSheetDate { get; set; }
        public int MachineID { get; set; }
        public int ProductID_Raw { get; set; }
        public int LotID { get; set; }
        public double Qty { get; set; }// Total order Qty
        public double QtyDye { get; set; } //This Qty only for Recepi Calculation. /Dammi qty not batch Qty
        public double QtyOmit { get; set; } //This Qty only for Recepi Calculation. /omit fron  batch and Recepi as like Pre process Loss
        public int Label { get; set; }
        public EnumRSState RSState { get; set; }
        public int LocationID { get; set; }
        public int PTUID { get; set; }
        public int LabDipDetailID { get; set; }
        public int DUPScheduleID { get; set; }
        public string Note { get; set; }
        public string DyeingType { get; set; }
        public double TtlLiquire { get; set; }
        public double TtlCotton { get; set; }
        public int HanksCone { get; set; } 
        public int NoOfHanksCone { get; set; }
        public int CopiedFrom { get; set; }
        public EnumReDyeingStatus IsReDyeing { get; set; }
        public int PrepareBy { get; set; }
        public int ApproveBy { get; set; }
        public int OrderType { get; set; }
        public int RSShiftID { get; set; }
        public string Shift { get; set; }
        public string RecipeByName { get; set; }
        public string ContractorName { get; set; }

        //public DateTime EventTime { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public List<RSRawLot> RSRawLots { get; set; }
        #endregion

        #region Derive
        public string NoCode { get; set; }
        public string MachineName { get; set; }
        public string MName { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductName_Raw { get; set; }
        public string LotNo { get; set; }
		public int WorkingUnitID{ get; set; }
        public string OperationUnitName { get; set; }
        public string LocationName { get; set; }
        //public string CopiedFromRouteSheetNo { get; set; }
        public string PrepareByName { get; set; }
        public string ApproveByName { get; set; }
        public string OrderNo { get; set; }
        public string ColorName { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public RouteSheetDetail RouteSheetDetail { get; set; }
        public PTU PTU { get; set; }
        public string WidthWithMU { get; set; }
        public RouteSheetHistory DyeingProgress { get; set; }
        public List<RouteSheetHistory> RSHistorys { get; set; }
        public List<RouteSheetPacking> RouteSheetPackings { get; set; }
        public List<RSInQCDetail> RSInQCDetails { get; set; }
        public List<RouteSheetDO> RouteSheetDOs { get; set; }
        private string _sOrderNoFull = "";
        public string IsReDyeingSt
        {
            get
            {
                if (this.IsReDyeing == EnumReDyeingStatus.None)
                    return "";
                else
                    return this.IsReDyeing.ToString("");
            }
        }
        public string OrderNoFull
        {
            get
            {
                _sOrderNoFull = this.NoCode + this.OrderNo;
              
                return _sOrderNoFull;
            }
        }
        public string RSStateStr
        {
            get
            {
                return this.RSState.ToString();
            }
        }

        public string RouteSheetDateStr
        {
            get
            {
                return this.RouteSheetDate.ToString("dd MMM yyyy");
            }
        }

        public string RSSateDateStr
        {
            get
            {
                if (this.DBServerDateTime == DateTime.MinValue) return "";
                else return this.DBServerDateTime.ToString("dd MMM yyyy HH:mm");
            }
        }

        public string StoreName
        {
            get
            {
                return this.LocationName + " " + this.OperationUnitName;
            }
        }
        public double QtyKg
        {
            get
            {
                return  Global.GetKG(this.Qty, 10);
            }
        }
        public string NoOfHanksConeWithType
        {
            get
            {
                return this.NoOfHanksCone + " " + this.DyeingType;
            }
        }
      
        #endregion

        #region Functions

        public RouteSheet IUD(int nDBOperation, long nUserID)
        {
            return RouteSheet.Service.IUD(this, nDBOperation, nUserID);
        }

        public RouteSheet UpdateMachine(int nDBOperation, long nUserID)
        {
            return RouteSheet.Service.UpdateMachine(this, nDBOperation, nUserID);
        }
        public static RouteSheet Get(int nRouteSheetID, long nUserID)
        {
            return RouteSheet.Service.Get(nRouteSheetID, nUserID);
        }
        public static List<RouteSheet> Gets(string sSQL, long nUserID)
        {
            return RouteSheet.Service.Gets(sSQL, nUserID);
        }
        public RouteSheet CopyRouteSheet(long nUserID)
        {
            return RouteSheet.Service.CopyRouteSheet(this, nUserID);
        }
        public RouteSheet YarnOut(int nEventEmpID,long nUserID)
        {
            return RouteSheet.Service.YarnOut(this, nEventEmpID, nUserID);
        }
        public RouteSheet RSQCDOneByForce(long nUserID)
        {
            return RouteSheet.Service.RSQCDOneByForce(this, nUserID);
        }
        public RouteSheet RSQCDOne(RouteSheetDO oRouteSheetDO, long nUserID)
        {
            return RouteSheet.Service.RSQCDOne(oRouteSheetDO, nUserID);
        }
        public RouteSheet RSInRSInSubFinishing(long nUserID)
        {
            return RouteSheet.Service.RSInRSInSubFinishing(this, nUserID);
        }
        public  RouteSheet GetByPS(int nDUPScheduleID, long nUserID)
        {
            return RouteSheet.Service.GetByPS(nDUPScheduleID, nUserID);
        }
        public RouteSheet SaveRouteSheetDO(long nUserID)
        {
            return RouteSheet.Service.SaveRouteSheetDO(this, nUserID);
        }
        public RouteSheet RouteSheetEditSave(RouteSheet oRouteSheet,long nUserID)
        {
            return RouteSheet.Service.RouteSheetEditSave(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRouteSheetService Service
        {
            get { return (IRouteSheetService)Services.Factory.CreateService(typeof(IRouteSheetService)); }
        }
        #endregion

    }

    #endregion

    #region IRouteSheet interface
    public interface IRouteSheetService
    {
        RouteSheet IUD(RouteSheet oRouteSheet, int nDBOperation, long nUserID);
        RouteSheet UpdateMachine(RouteSheet oRouteSheet, int nDBOperation, long nUserID);
        RouteSheet Get(int nRouteSheetID, long nUserID);
        RouteSheet GetByPS(int nDUPScheduleID, long nUserID);
        List<RouteSheet> Gets(string sSQL, long nUserID);
        RouteSheet CopyRouteSheet(RouteSheet oRouteSheet, long nUserID);
        RouteSheet YarnOut(RouteSheet oRouteSheet, int nEventEmpID, long nUserID);
        RouteSheet RSQCDOneByForce(RouteSheet oRouteSheet, long nUserID);
        RouteSheet RSQCDOne(RouteSheetDO RouteSheetDO, long nUserID);
        RouteSheet RSInRSInSubFinishing(RouteSheet oRouteSheet, long nUserID);
        RouteSheet SaveRouteSheetDO(RouteSheet oRouteSheet, long nUserID);
        RouteSheet RouteSheetEditSave(RouteSheet oRouteSheet,long nUserID);
        
    }
    #endregion


}
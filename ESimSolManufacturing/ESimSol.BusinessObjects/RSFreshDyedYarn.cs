using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region RSFreshDyedYarn

    public class RSFreshDyedYarn : BusinessObject
    { 
       
        public RSFreshDyedYarn()
        {
            RouteSheetID = 0;
            RouteSheetNo = string.Empty;
            RSShiftID = 0;
            RSShiftName = string.Empty;
            OrderNo = string.Empty;
            FreshDyedYarnQty = 0;
            Note = string.Empty;
            ProductID = 0;
            ContractorID = 0;
            LocationID = 0;
            OrderType = 0;
            QCDate = DateTime.MinValue;
            RSDate = DateTime.Today;
            ManagedQty = 0;
            DCAddCount = 0;
            ColorName = string.Empty;
            ContractorName = string.Empty;
            ProductName = string.Empty;
            ProductCode = "";
            Note = string.Empty;
            MachineName = "";
            RSSubNote = "";
            RSSubStates = EnumRSSubStates.None;
            RSState = EnumRSState.None;
            RequestByName = "";
            UnloadTime = DateTime.MinValue;
			UnloadByName= "";
            LoadTime = DateTime.MinValue;
            LoadByName = "";
            WUName = "";
            WastageQty=0;
            RecycleQty = 0;
            WorkingUnitID = 0;
            OrderTypeSt = "";
            ReportType = 0;
            QtyDCAdd = 0;
            PackingQty = 0;
            IsFullQC = false;
            WIPQtyTwo = 0;
            IsInHouse = false;
        }

        #region Properties
        public int RouteSheetID { get; set; }
        public string RouteSheetNo { get; set; }
        public DateTime RSDate { get; set; }
        public DateTime QCDate { get; set; }
        public string ContractorName { get; set; }
        public int ProductID { get; set; }
        public int LocationID { get; set; }
        public double Qty_RS { get; set; }
        public double FreshDyedYarnQty { get; set; }
        public double PackingQty { get; set; }
        public int BagCount { get; set; }
        public double ManagedQty { get; set; }
        public int ContractorID { get; set; }
        public string Note { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string RawLotNo { get; set; }
        public string MachineName { get; set; }
        public string OrderNo { get; set; }
        public int OrderType { get; set; }
        public string OrderTypeSt { get; set; }
        public int RSShiftID { get; set; }
        public string RSShiftName { get; set; }
        public EnumRSState RSState { get; set; }
        public string DyeingType { get; set; }
        public int NoOfHanksCone { get; set; }
        public string RequestByName { get; set; }
        public DateTime LoadTime { get; set; }
        public string LoadByName { get; set; }
        public DateTime UnloadTime { get; set; }
        public string UnloadByName { get; set; }
        public double WastageQty { get; set; }
        public double RecycleQty { get; set; }
        public int DCAddCount { get; set; }
        public int WorkingUnitID { get; set; }
        public string WUName { get; set; }
        public bool IsReDyeing { get; set; }
        public double QtyDCAdd { get; set; }
        public int ReportType { get; set; }
        public bool IsFullQC { get; set; }
        public bool IsInHouse { get; set; }
        
        
        #endregion

        #region UnManagedQty
        public double UnManagedQty
        {
            get
            {
                return (this.WastageQty + this.RecycleQty - this.ManagedQty);
            }
        }
        public double WIPQtyTwo { get; set; }
        public double WIPQty
        {
            get
            {
                
                 if ((this.IsFullQC)) return 0;
                 else if ((this.RSState < EnumRSState.YarnOut)) return 0;
                 else if ((this.FreshDyedYarnQty + this.WastageQty + this.RecycleQty) > this.Qty_RS) return 0;
                else 
                    return Math.Round((this.Qty_RS - this.FreshDyedYarnQty - this.WastageQty - this.RecycleQty), 2);
            }
        }
        public int DyeingOrderDetailID { get; set; }
        public string ErrorMessage { get; set; }

        public double Gain { get; set; }
        public double Loss { get; set; }

        //private double _nGain = 0;
        //public double Gain
        //{
        //    get
        //    {
        //        _nGain = (this.FreshDyedYarnQty + this.WastageQty + this.RecycleQty) - this.Qty_RS;
        //        if (_nGain<=0)
        //        {
        //            _nGain = 0;
        //        }
        //        if ((!this.IsFullQC)) _nGain = 0;
        //        return _nGain;
        //    }
        //}
        //private double _nLoss = 0;
        //public double Loss
        //{
        //    get
        //    {
        //        _nLoss = this.Qty_RS - (this.FreshDyedYarnQty + this.WastageQty + this.RecycleQty);
        //        if (_nLoss <= 0)
        //        {
        //            _nLoss = 0;
        //        }
        //        if ((!this.IsFullQC)) _nLoss = 0;
        //        return _nLoss;
        //    }
        //}
        private double _nTotalShort = 0;
        public double TotalShort
        {
            get
            {
                _nTotalShort = this.Qty_RS - (this.FreshDyedYarnQty + this.RecycleQty);
                if (_nTotalShort <= 0)
                {
                    _nTotalShort = 0;
                }
                if ((!this.IsFullQC)) _nTotalShort = 0;
                return _nTotalShort;
            }
        }
        private double _nGainPer = 0;
        public double GainPer
        {
            get
            {
                if (this.Qty_RS > 0)
                {
                    _nGainPer = this.Gain * 100 / this.Qty_RS;
                }
                else
                {
                    _nGainPer = 0;
                }
                if ((!this.IsFullQC)) _nGainPer = 0;
                return _nGainPer;
            }
        }
        private double _nLossPer = 0;
        public double LossPer
        {
            get
            {
                if (this.Qty_RS > 0)
                {
                    _nLossPer = this.Loss * 100 / this.Qty_RS;
                }
                else
                {
                    _nLossPer = 0;
                }
                if ((!this.IsFullQC)) _nLossPer=0;
                return _nLossPer;
            }
        }
        public string QCDateStr
        {
            get
            {
                if (this.QCDate == DateTime.MinValue) { return ""; }
                return this.QCDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string RSDateStr
        {
            get
            {
                return this.RSDate.ToString("dd MMM yyyy");
            }
        }
       
        public string RSStateStr
        {
            get
            {
                return this.RSState.ToString();
            }
        }
        public string RSPartQC
        {
            get
            {
                if ((!this.IsFullQC) && this.FreshDyedYarnQty>0)   return "Part QC";
                else if((this.IsFullQC) && this.FreshDyedYarnQty > 0) return "Full QC";
                else return "";
            }
        }

        public string RSSubStatesSt
        {
            get
            {
                if (this.RSSubStates != EnumRSSubStates.None)
                {
                    return EnumObject.jGet(this.RSSubStates);
                }
                else return "";
            }
        }
        public string RSSubNote { get; set; }
        public string UnloadTimeStr
        {
            get
            {
                if (this.UnloadTime == DateTime.MinValue) { return ""; }
                return this.UnloadTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string LoadTimeStr
        {
            get
            {
                if (this.LoadTime == DateTime.MinValue) { return ""; }
                return this.LoadTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string IsReDyeingSt
        {
            get
            {
                if (this.IsReDyeing)  return "Re-Dye";
                return "Fresh Dye";
            }
        }
        public EnumRSSubStates RSSubStates { get; set; }
        public string TimeLag
        {
            get
            {
                if (this.UnloadTime != DateTime.MinValue && this.QCDate == DateTime.MinValue)
                {
                    return Math.Round((DateTime.Now - this.UnloadTime).TotalDays, 0).ToString() + ":" + (DateTime.Now - this.UnloadTime).Hours.ToString();
                }
                else if (this.UnloadTime != DateTime.MinValue && this.QCDate != DateTime.MinValue)
                {
                    return Math.Round((this.QCDate - this.UnloadTime).TotalDays,0).ToString() + ":" + (this.QCDate - this.UnloadTime).Hours.ToString();
                }
                else
                {
                    return "";
                }
            }

        }
        #endregion

        #region Functions


        public static List<RSFreshDyedYarn> Gets(DateTime dStartDate, DateTime dEndDate, int nOrderType, int nReportType, Int64 nUserID)
        {
            return RSFreshDyedYarn.Service.Gets( dStartDate,  dEndDate,  nOrderType, nReportType, nUserID);
        }
        public static List<RSFreshDyedYarn> Gets(string sSQL, int nReportType, Int64 nUserID)
        {
            return RSFreshDyedYarn.Service.Gets(sSQL,nReportType, nUserID);
        }
          public static List<RSFreshDyedYarn> Gets(string sSQL,int cboQCdate,DateTime dStartDate, DateTime dEndDate, int nReportType, Int64 nUserID)
        {
            return RSFreshDyedYarn.Service.Gets(sSQL,cboQCdate,dStartDate,dEndDate, nReportType, nUserID);
        }
        public static List<RSFreshDyedYarn> Gets_Product(string sSQL, Int64 nUserID)
        {
            return RSFreshDyedYarn.Service.Gets_Product(sSQL, nUserID);
        }
        public static List<RSFreshDyedYarn> Gets(string sSQL, Int64 nUserID)
        {
            return RSFreshDyedYarn.Service.Gets(sSQL, nUserID);
        }
        public static List<RSFreshDyedYarn> GetsLoadUnload(string sSQL, Int64 nUserID)
        {
            return RSFreshDyedYarn.Service.GetsLoadUnload(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IRSFreshDyedYarnService Service
        {
            get { return (IRSFreshDyedYarnService)Services.Factory.CreateService(typeof(IRSFreshDyedYarnService)); }
        }
        #endregion

    }
    #endregion
    
    #region IRSFreshDyedYarn interface
    public interface IRSFreshDyedYarnService
    {
        List<RSFreshDyedYarn> Gets(DateTime dStartDate, DateTime dEndDate, int nOrderType, int nReportType, Int64 nUserID);
        List<RSFreshDyedYarn> Gets(string sSQL, int nReportType, Int64 nUserID);
        List<RSFreshDyedYarn> Gets(string sSQL, int cboQCdate,DateTime dStartDate, DateTime dEndDate,int nReportType, Int64 nUserID);
        List<RSFreshDyedYarn> Gets_Product(string sSQL, Int64 nUserID);
        List<RSFreshDyedYarn> Gets(string sSQL, Int64 nUserID);
        List<RSFreshDyedYarn> GetsLoadUnload(string sSQL, Int64 nUserID);
     
    }
    #endregion
}

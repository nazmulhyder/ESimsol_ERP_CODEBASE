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
    #region DUOrderRS

    public class DUOrderRS : BusinessObject
    {
        public DUOrderRS()
        {
            RouteSheetID = 0;
            RouteSheetNo = string.Empty;
            OrderNo = string.Empty;
            FreshDyedYarnQty = 0;
            Note = string.Empty;
            ProductID = 0;
            ContractorID = 0;
            OrderType = 0;
            QCDate = DateTime.Today;
            RSDate = DateTime.Today;
            ManagedQty = 0;
            UnManagedQty = 0;
            ColorName = string.Empty;
            ContractorName = string.Empty;
            ProductName = string.Empty;
            ProductCode = "";
            Note = string.Empty;
            RSState = EnumRSState.None;
            Pro_Pipline = 0;
            GainLoss = 0;
            LotNo = "";
            WUName = "";

            /*FOR :: SP_RPT_RouteSheetQC*/
			Qty= 0.0;
			MachineID = 0;
			MachineName = ""; 
			LocationID = 0;
			LocationName = ""; 
			WorkingUnitID = 0;
			StartTime =DateTime.Now;
			EndTime =DateTime.Now;
			LotID_Yarn = 0;
			ProductCategoryName = "";
			MUnitID = 0;
            IsReDyeing = false;
			HanksCone = 0;
			ExportPIID = 0;
			BuyerID = 0;
			UnitPrice = 0.0;
			IsInHouse = false;
			DyeingOrderDetailID  = 0;
			DyeingOrderID  = 0;
			PINo = "";
			MUnit = "";
			DUPScheduleID = 0;
			OrderQty = 0.0;
			FinishQty = 0.0;
			PackingQty = 0.0;
			ShadingQty = 0.0;
			WastageQty = 0.0;
			ColorMisQty = 0.0;
			QtyDC = 0.0;
            QtyRC = 0.0;
			RSShiftID = 0;
			ShiftName = "";
            QtyTR = 0;
            QtyRC = 0;
        }

        #region Properties
        public int RouteSheetID { get; set; }
        public string RouteSheetNo { get; set; }
        public DateTime RSDate { get; set; }
        public DateTime QCDate { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public int ProductID { get; set; }
        public double Qty_RS { get; set; }
        public EnumRSState RSState { get; set; }
        public double FreshDyedYarnQty { get; set; }
        public int BagCount { get; set; }
        public double ManagedQty { get; set; }
        public double UnManagedQty { get; set; }
        public double DeliveryQty { get; set; }
        public double Pro_Pipline { get; set; }
        public double GainLoss { get; set; }
        public double StockInHand { get; set; }
        public int ContractorID { get; set; }
        public string Note { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string OrderNo { get; set; }
        public int OrderType { get; set; }
        public string MUName { get; set; }
        public string LotNo { get; set; }
        public string WUName { get; set; }

        public int ReportLevelType { get; set; }


        /* FOR:: RS QC REPORT*/
        public double Qty { get; set; }
        public int MachineID { get; set; }
        public string MachineName { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int WorkingUnitID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int LotID_Yarn { get; set; }
        public string ProductCategoryName { get; set; }
        public int MUnitID { get; set; }
        public bool IsReDyeing { get; set; }
        public int HanksCone { get; set; }
        public int ExportPIID { get; set; }
        public int BuyerID { get; set; }
        public double UnitPrice { get; set; }
        public bool IsInHouse { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int DyeingOrderID { get; set; }
        public string PINo { get; set; }
        public string MUnit { get; set; }
        public int DUPScheduleID { get; set; }
        public double OrderQty { get; set; }
        public double FinishQty { get; set; }
        public double PackingQty { get; set; }
        public double ShadingQty { get; set; }
        public double WastageQty { get; set; }
        public double RecycleQty { get; set; }
        public double ColorMisQty { get; set; }
        public double QtyDC { get; set; }
        public double QtyRC { get; set; }
        public double QtyTR { get; set; }
        
        public int RSShiftID { get; set; }
        public string ShiftName { get; set; }
        public string QCApproveByName { get; set; }
        #endregion

        #region Derive
        public string ErrorMessage { get; set; }

        private double _nGain = 0;
        public double Gain
        {
            get
            {
                _nGain = this.Qty_RS - (this.FreshDyedYarnQty + this.UnManagedQty + this.ManagedQty);
                if (_nGain<=0)
                {
                    _nGain = 0;
                }
                return _nGain;
            }
        }
        private double _nLoss = 0;
        public double Loss
        {
            get
            {
                _nLoss = this.Qty_RS - (this.FreshDyedYarnQty + this.UnManagedQty + this.ManagedQty);
                if (_nLoss <= 0)
                {
                    _nLoss = (-1) * _nLoss;
                }
                else
                {
                    _nLoss = 0;
                }
                return _nLoss;
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
                    _nGainPer = this.Loss * 100 / this.Qty_RS;
                }
                else
                {
                    _nGainPer = 0;
                }
                return _nGainPer;
            }
        }
        public string SearchRSDate { get; set; }
        public string SearchQCDate { get; set; }
        public string QCDateSt
        {
            get
            {
                return this.QCDate.ToString("dd MMM yyyy");
            }
        }
        public string RSDateSt
        {
            get
            {
                return this.RSDate.ToString("dd MMM yyyy");
            }
        }
        public string OrderTypeSt
        {
            get
            {
                return ((EnumOrderType)this.OrderType).ToString(); 
            }
        }
        public string RSStateSt
        {
            get
            {
                return ((EnumRSState)this.RSState).ToString();
            }
        }
        public double ExcessShortQty { get { return (this.FinishQty - this.Qty); } }
        public double BalanceQty { get { return (this.PackingQty - this.Qty); } }
        public double ProcessGLPercent { get { return this.Qty != 0 ? (this.ExcessShortQty * 100 / this.Qty) : 0; } }
        public double FreshGLPercent { get { return this.Qty != 0 ? (this.BalanceQty * 100 / this.Qty) : 0; } }

        #endregion

        #endregion

        #region Functions
        public static List<DUOrderRS> Gets(string sDyeingOrderDetailID, int nRSID, int nOrderType, int nReportType, Int64 nUserID)
        {
            return DUOrderRS.Service.Gets(  sDyeingOrderDetailID,  nRSID,  nOrderType,  nReportType, nUserID);
        }
        public static List<DUOrderRS> Gets(string sSql, int nReportType, Int64 nUserID)
        {
            return DUOrderRS.Service.Gets(sSql, nReportType, nUserID);
        }
        public static List<DUOrderRS> GetsQC(string sSql, int nReportType, Int64 nUserID)
        {
            return DUOrderRS.Service.GetsQC(sSql, nReportType, nUserID);
        }
        public static List<DUOrderRS> GetsQCByRaqLot(int nRawLotID, int nReportType, Int64 nUserID)
        {
            return DUOrderRS.Service.GetsQCByRaqLot(nRawLotID, nReportType, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUOrderRSService Service
        {
            get { return (IDUOrderRSService)Services.Factory.CreateService(typeof(IDUOrderRSService)); }
        }
        #endregion
    }

    #region IDUOrderRS interface
    public interface IDUOrderRSService
    {
        List<DUOrderRS> Gets(string sDyeingOrderDetailID, int nRSID, int nOrderType, int nReportType, Int64 nUserID);
        List<DUOrderRS> Gets(string sSql, int nReportType, Int64 nUserID);
        List<DUOrderRS> GetsQC(string sSql, int nReportType, Int64 nUserID);
        List<DUOrderRS> GetsQCByRaqLot(int nRawLotID, int nReportType, Int64 nUserID);
    }
    #endregion
}

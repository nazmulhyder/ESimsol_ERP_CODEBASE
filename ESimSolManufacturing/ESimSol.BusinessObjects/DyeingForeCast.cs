using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    public class DyeingForeCast : BusinessObject
    {
        public DyeingForeCast()
        {
            DyeingForeCastID = 0;
            DyeingType = EumDyeingType.None;
            DyeingTypeInt = 0;
            YetToProdQty = 0;
            VirtualYetToProdQty = 0;
            ProductionHour = 0;
            ProductionCapacity = 0;
            CapacityPerHour = 0;
            ReqDays = 0;
            DyeingOrderID = 0;
            DyeingOrderNo = "";
            OrderDate = DateTime.Now;
            ContractorID = 0;
            ContractorName = "";
            DyeingOrderType = EnumOrderType.None;
            ProductID = 0;
            ProductCode = "";
            ProductName = "";
            ReqDyeingPeriod = 0;
            ExportPIID = 0;
            ExportPINo = "";
            ExportLCID = 0;
            ExportLCNo = "";
            BUID = 0;
            ForecastLayout = EnumForecastLayout.None;
            ForecastLayoutInt = 0;
            ErrorMessage = "";
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        #region Properites
        public int DyeingForeCastID { get; set; }
        public EumDyeingType DyeingType { get; set; }
        public int DyeingTypeInt { get; set; }
        public double YetToProdQty { get; set; }
        public double VirtualYetToProdQty { get; set; }
        public double ProductionHour { get; set; }
        public double ProductionCapacity { get; set; }
        public double CapacityPerHour { get; set; }
        public int ReqDays { get; set; }
        public int DyeingOrderID { get; set; }
        public string DyeingOrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public EnumOrderType DyeingOrderType { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double ReqDyeingPeriod { get; set; }
        public int ExportPIID { get; set; }
        public string ExportPINo { get; set; }
        public int ExportLCID { get; set; }
        public string ExportLCNo { get; set; }
        public int BUID { get; set; }
        public EnumForecastLayout ForecastLayout { get; set; }
        public int ForecastLayoutInt { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        #endregion

        #region Derived property
        public string DyeingOrderTypeSt
        {
            get
            {
                return EnumObject.jGet(this.DyeingOrderType);
            }
        }
        public string DyeingTypeSt
        {
            get
            {
                return EnumObject.jGet(this.DyeingType);
            }
        }
        public string YetToProdQtySt
        {
            get
            {
                return Global.MillionFormat(this.YetToProdQty);
            }
        }
        public string VirtualYetToProdQtySt
        {
            get
            {
                return Global.MillionFormat(this.VirtualYetToProdQty);
            }
        }
        public string ProductionCapacitySt
        {
            get
            {
                return Global.MillionFormat(this.ProductionCapacity);
            }
        }
        public string ReqDaysSt
        {
            get
            {
                return this.ReqDays.ToString("#,##0") + " Days";
            }
        }
        #endregion

        #region Functions
        public static List<DyeingForeCast> Gets(int nBuid, EnumForecastLayout eForecastLayout, DateTime dStartDate, DateTime dEndDate, long nUserID)
        {
            return DyeingForeCast.Service.Gets(nBuid, eForecastLayout, dStartDate, dEndDate, nUserID);
        }
        public static List<DyeingForeCast> GetsDetails(int nBuid, EumDyeingType eDyeingType, EnumForecastLayout eForecastLayout, DateTime dStartDate, DateTime dEndDate, long nUderID)
        {
            return DyeingForeCast.Service.GetsDetails(nBuid, eDyeingType, eForecastLayout, dStartDate, dEndDate, nUderID);
        }
        #endregion

        #region ServiceFactory
        internal static IDyeingForeCastService Service
        {
            get { return (IDyeingForeCastService)Services.Factory.CreateService(typeof(IDyeingForeCastService)); }
        }
        #endregion
    }
    public interface IDyeingForeCastService
    {
        List<DyeingForeCast> Gets(int nBuid, EnumForecastLayout eForecastLayout, DateTime dStartDate, DateTime dEndDate, long nUserID);
        List<DyeingForeCast> GetsDetails(int nBuid, EumDyeingType eDyeingType, EnumForecastLayout ForecastLayout, DateTime dStartDate, DateTime dEndDate, long nUserID);

    }
}

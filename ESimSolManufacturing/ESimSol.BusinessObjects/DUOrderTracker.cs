using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DUOrderTracker

    public class DUOrderTracker : BusinessObject
    {
        public DUOrderTracker()
        {

            DyeingOrderID = 0;
            OrderType = 0;
            ProductID = 0;
            LabDipDetailID = 0;
            ColorName = "";
            PantonNo = "";
            ColorNo = "";
            OrderQty = 0;
            Pro_PipeLineQty = 0;
            ProductionFinishedQty = 0;
            ClaimOrderQty = 0;
            UnitPrice = 0;
            BuyerID = 0;
            ContractorID = 0;
            State = 0;
            ReturnQty = 0;
            ActualDeliveryQty = 0;
            YetToDelivery = 0;
            ReadyStockInhand = 0;
            ClaimOrderQty = 0;
            ProductName = "";
            MKT = "";
            PINo = "";
            DyeingOrderDetailID = 0;
            BuyerName = "";
            ContractorName = "";
            Params = "";
            IsSample = false;
            OrderDate = DateTime.Today;
            RowNo = 0;
            Qty_SC = 0;
        }

        #region Properties
        public int PTUID { get; set; }
        public int DyeingOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int OrderType { get; set; }
        public int ProductID { get; set; }
        public int LabDipDetailID { get; set; }
        public string ColorName { get; set; }
        public string PantonNo { get; set; }
        public string ColorNo { get; set; }
        public int Shade { get; set; }
        public double Qty_PI { get; set; }
        public double OrderQty { get; set; }
        public double ClaimOrderQty { get; set; }
        public double Qty_ProIssue { get; set; }
        public double Pro_PipeLineQty { get; set; }
        public double Qty_SC { get; set; }
        public double ProductionFinishedQty { get; set; }
        public double ReturnQty { get; set; }
        public double ReadyStockInhand { get; set; }
        public double StockInHand { get; set; }
        public double ActualDeliveryQty { get; set; }
        public double YetToProduction { get; set; }
        public double YetToDelivery { get; set; }
        public double UnitPrice { get; set; }
        public int BuyerID { get; set; }
        public int ContractorID { get; set; }
        public int State { get; set; }
        public string MKT { get; set; }
        public string PINo { get; set; }
        public string OrderNo { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string ProductCode { get; set; }
        public string YarnCount { get; set; }
        public string ProductName { get; set; }
        public string LabdipNo { get; set; }
        public string LCNo { get; set; }
        public bool IsInHouse { get; set; } 
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        public string SampleInvoiceNo { get; set; }
        public string MUName { get; set; }
        public int ReportLevelType { get; set; }
        public bool IsSample { get; set; }
        public int RowNo { get; set; } 

        #region derived properties
        public string ColorNameShade
        {
            get { return (this.ColorName + "[" + ((EnumShade)this.Shade).ToString() + "]"); }
        }
        public string OrderDateSt
        {
            get { return this.OrderDate.ToString("dd MMM yyyy"); }
        }

        public double StockInAval
        {
            get
            {
                double nStockInAval = 0;
                nStockInAval = (this.ReadyStockInhand - this.StockInHand);
                if (nStockInAval < 0)
                {
                    return 0;
                }
                return nStockInAval;
            }
        }
        public string Color
        {
            get
            {
                return ColorName + "[" + this.ColorNo + "]" + "[" + ((EnumShade)this.Shade).ToString() + "]";
            }
        }
        #region RawYarnUseCapacity
        public double RawYarnUseCapacity
        {
            get
            {
                double nRawYarnUseCapacity = 0;
                nRawYarnUseCapacity = this.OrderQty - this.ReadyStockInhand - this.Pro_PipeLineQty - this.ActualDeliveryQty + this.ReturnQty + this.ClaimOrderQty;
                if (nRawYarnUseCapacity < 0)
                {
                    return 0;
                }
                return nRawYarnUseCapacity;
            }
        }
        #endregion

        #region StateSt
        public string StateSt
        {
            get { return ((EnumPTUState)this.State).ToString(); }

        }
        #endregion

        #endregion

        #endregion
        #region Functions

        public static List<DUOrderTracker> Gets(string sSQL, int nReportType, bool bIsSample, long nUserID)
        {
            return DUOrderTracker.Service.Gets(sSQL, nReportType, bIsSample, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IDUOrderTrackerService Service
        {
            get { return (IDUOrderTrackerService)Services.Factory.CreateService(typeof(IDUOrderTrackerService)); }
        }
        #endregion

    }
    #endregion



    #region IDUOrderTracker interface

    public interface IDUOrderTrackerService
    {

        List<DUOrderTracker> Gets(string sSQL, int nReportType,bool bIsSample,  long nUserID);

    }
    #endregion
}

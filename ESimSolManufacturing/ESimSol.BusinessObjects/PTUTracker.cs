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
    #region PTUTracker
    
    public class PTUTracker : BusinessObject
    {
        public PTUTracker()
        {
           
            OrderID = 0;
            OrderType = 0;
            ProductID = 0;
            LabDipDetailID = 0;
            ColorName = "";
            PantonNo = "";
            ColorNo = "";
            OrderQty = 0;
            ProductionGraceQty = 0;
            ProductionPipeLineQty = 0;
            ProductionFinishedQty = 0;
            ReOrderQty = 0;
            UnitPrice = 0;
            BuyerID = 0;
            ContractorID = 0;
            State =0;
            ReturnQty = 0;
            ActualDeliveryQty = 0;
            ReadyStockInhand = 0;
            ReOrderQty = 0;
            ProductName = "";
            MKT = "";
            PINo = "";
            BuyerName = "";
            ContractorName = "";
            ScheduledQty = 0;
            OrderRef = "";
            Params = "";
        }

        #region Properties
        public int PTUID { get; set; }
        public int OrderID { get; set; }
        public int OrderType { get; set; }
        public int ProductID { get; set; }
        public int LabDipDetailID { get; set; }
        public string ColorName { get; set; }
        public string PantonNo { get; set; }
        public string ColorNo { get; set; }
        public int Shade { get; set; }
        public double Qty_PI { get; set; }
        public double OrderQty { get; set; }
        public double ReOrderQty { get; set; }
        public double ProductionPipeLineQty { get; set; }
        public double ProductionFinishedQty { get; set; }
        public double ProductionLossGainQty { get; set; }
        public double ProductionGraceQty { get; set; }
        public double ReturnQty { get; set; }
        public double ReadyStockInhand { get; set; }
        public double StockInHand { get; set; }
       
        
        public double ActualDeliveryQty { get; set; }
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
        public string OrderRef { get; set; } 
        public bool IsInHouse { get; set; }  // Added By Sagor on 22 Sep 2014
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        public string SampleInvoiceNo { get; set; }
        public string MUName { get; set; }
      

        #region derived properties
        public string ColorNameShade
        {
            get { return (this.ColorName + "[" + ((EnumShade)this.Shade).ToString() + "]"); }
        }
        public string OrderQtyKG
        {
            get { return Global.MillionFormat(Global.GetKG(OrderQty, 2)); }
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
     

        public string OrderQtySt
        {
            get
            {
                return Global.MillionFormat(this.OrderQty);
            }
        }
      
        public string ProductionFinishedQtySt
        {
            get
            {
                return Global.MillionFormat(this.ProductionFinishedQty);
            }
        }
        public string ProductionPipeLineQtySt
        {
            get
            {
                return Global.MillionFormat(this.ProductionPipeLineQty);
            }
        }

        public string ProductionGraceQtySt
        {
            get
            {
                return Global.MillionFormat(this.ProductionGraceQty);
            }
        }

       
        #region RawYarnUseCapacity
        public double RawYarnUseCapacity
        {
            get
            {
                double nRawYarnUseCapacity = 0;
                nRawYarnUseCapacity = this.OrderQty - this.ReadyStockInhand - this.ProductionPipeLineQty - this.ActualDeliveryQty + this.ReturnQty + this.ReOrderQty;
                if (nRawYarnUseCapacity < 0)
                {
                    return 0;
                }
                return nRawYarnUseCapacity;
            }
        }
        #endregion
        #region YetToProduction
        private double _nYetToProduction = 0;
        public double YetToProduction
        {
            get
            {
                _nYetToProduction = this.OrderQty + this.ReOrderQty + this.ReturnQty - this.ReadyStockInhand - this.ActualDeliveryQty;
                if (_nYetToProduction > 0)
                {
                    return _nYetToProduction;
                }
                else
                {
                    _nYetToProduction = 0;
                    return _nYetToProduction;
                }
            }

        }
        #endregion
        #region DeliveryQty
        public double DeliveryQty
        {
            get { return (this.ActualDeliveryQty - this.ReturnQty); }

        }
        #endregion
        #region YetToDeliver
        double dYetToDeliver = 0;
        public double YetToDelivery
        {
            get
            {
                dYetToDeliver = (this.OrderQty - this.ActualDeliveryQty) + this.ReturnQty + this.ReOrderQty;
                if (dYetToDeliver < 0) dYetToDeliver = 0;
                return dYetToDeliver;
            }
        }
        #endregion
        //#region StockInHand
        //public double StockInHand
        //{
        //    get
        //    {
        //        //double nPTUTrackerDQty = 0;
        //        //double nStockInHand = 0;

        //        //nPTUTrackerDQty = this.PTUTrackerDistributionQTY;

        //        if (this.ReadyStockInhand > this.YetToDelivery)
        //        {
        //            nStockInHand = this.YetToDelivery;
        //        }
        //        else
        //        {
        //            nStockInHand = ReadyStockInhand;

        //        }

        //        return nStockInHand;
        //    }
        //}
        //#endregion
        #region RemainingJobOrderQty
        public string ProductNameCode
        {
            get { return "[" + this.ProductCode + "]" + this.ProductName; }

        }
      
        #endregion

        #region JobOrderValue
        public double JobOrderValue
        {
            get { return (this.OrderQty * this.UnitPrice); }

        }
        #endregion
        #region JobOrderValueST
        public string JobOrderValueST
        {
            get { return Global.MillionFormat(this.OrderQty * this.UnitPrice); }

        }
        #endregion
        #region DeliveryValue
        public double DeliveryValue
        {
            get { return (this.ActualDeliveryQty * this.UnitPrice) - ((this.ReturnQty * this.UnitPrice) + (this.ReOrderQty * this.UnitPrice)); }

        }
        #endregion
        #region DeliveryValueST
        public string DeliveryValueST
        {
            get { return Global.MillionFormat((this.ActualDeliveryQty * this.UnitPrice) - ((this.ReturnQty * this.UnitPrice) + (this.ReOrderQty * this.UnitPrice))); }

        }
        #endregion
        #region YetToDeliveryValue
        public double YetToDeliveryValue
        {
            get { return (this.YetToDelivery * this.UnitPrice); }

        }
        #endregion
        #region YetToDeliveryValueST
        public string YetToDeliveryValueST
        {
            get { return Global.MillionFormat((this.YetToDelivery * this.UnitPrice)); }

        }
        #endregion
        #region YetToProductionValue
        public double YetToProductionValue
        {
            get { return (this.YetToProduction * this.UnitPrice); }

        }
        #endregion
        #region YetToProductionValueST
        public string YetToProductionValueST
        {
            get { return Global.MillionFormat(this.YetToProduction * this.UnitPrice); }

        }
        #endregion
        #region RateInLBSST
        public string UnitPriceSt
        {
            get { return Global.MillionFormat(this.UnitPrice); }

        }
        #endregion
        #region StateSt
        public string StateSt
        {
            get { return ((EnumPTUState)this.State).ToString(); }

        }
        #endregion
        
        #endregion

        #region Schedule Using Dyeing Execution Order
        public string Color
        {
            get
            {
                return ColorName + "[" + this.ColorNo + "]"  + "[" +((EnumShade) this.Shade).ToString() + "]";
            }
        }
        public double ScheduledQty { get; set; }
        
        #endregion
        #endregion
        #region Functions

      
        //public static List<PTUTracker> GetsByOrder(int nOrderID, int eOrderType, long nUserID)
        //{
        //    return PTUTracker.Service.GetsByOrder(nOrderID, (int)eOrderType, nUserID);
        //}
        public static List<PTUTracker> Gets(string sSQL, int nReportType, int nViewType, int nOrderType, long nUserID)
        {
            return PTUTracker.Service.Gets(sSQL, nReportType,  nViewType, nOrderType, nUserID);
        }
        public static List<PTUTracker> GetsYetTOPro(int nReportType, int nViewType, long nUserID)
        {
            return PTUTracker.Service.GetsYetTOPro(nReportType, nViewType, nUserID);
        }
  
      
        public static PTUTracker Get(int nProductionTracingUnitID, long nUserID)
        {
            return PTUTracker.Service.Get(nProductionTracingUnitID, nUserID);
        }
      
     
      
        #endregion
        #region ServiceFactory
        internal static IPTUTrackerService Service
        {
            get { return (IPTUTrackerService)Services.Factory.CreateService(typeof(IPTUTrackerService)); }
        }
        #endregion

       
    }
    #endregion



    #region IPTUTracker interface
    
    public interface IPTUTrackerService
    {
        PTUTracker Get(int nProductionTracingUnitID, long nUserID);
        List<PTUTracker> Gets(string sSQL, int nReportType, int nViewType, int nOrderType, long nUserID);
        List<PTUTracker> GetsYetTOPro( int nReportType, int nViewType, long nUserID);
        //List<PTUTracker> Gets(string sSQL, long nUserID);
    }
    #endregion
}

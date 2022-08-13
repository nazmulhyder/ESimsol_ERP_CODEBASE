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
    #region ProductReconciliationReport
    public class ProductReconciliationReport : BusinessObject
    {
        #region  Constructor
        public ProductReconciliationReport()
        {
            BUID = 0;
            ProductBaseID = 0;
            ProductCode = "";
            StockInHand = 0;
            BookingQty = 0;
            ProYetToWithLC = 0;
            ProYetToSample = 0;
            LCReceiveDONoReceive = 0;
            PIIssueLCnDONotReceive = 0;

            DocInCnF = 0;
            DocReceive = 0;
            ShipmentDone = 0;
            InvoiceWithoutBL = 0;
            LCOpen = 0;
            MinimumStock = 0;
            ReportType = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            ProductionYetTo = 0;
            GoodsinTrasit = 0;
            GoodsinTrasit = 0;
            SortType = 0;
            CategoryName = "";
            ProductCategoryID = 0;
        }
        #endregion

        #region Properties

        #region ProductName
        private string _sProductName = "";
        public string ProductName
        {
            get { return _sProductName; }
            set { _sProductName = value; }
        }
        #endregion
        #region ProductBaseID
        private int _nProductBaseID = 0;
        public int ProductBaseID
        {
            get { return _nProductBaseID; }
            set { _nProductBaseID = value; }
        }
        #endregion
        #region ProductID
        private int _nProductID = 0;
        public int ProductID
        {
            get { return _nProductID; }
            set { _nProductID = value; }
        }
        #endregion
        
        #region SLNo
        private int _nSLNo = 0;
        public int SLNo
        {
            get { return _nSLNo; }
            set { _nSLNo = value; }
        }
        #endregion
        public string ProductCode { get; set; }
        public string CategoryName { get; set; }
        public int ProductCategoryID { get; set; }
        public double StockInHand { get; set; }
        public double BookingQty { get; set; }
        public double ProYetToWithLC { get; set; }
        public double ProYetToWithoutLC { get; set; }
        public double ProYetToSample { get; set; }
        public double LCReceiveDONoReceive { get; set; }
        public double LCReceiveDONoReceive_Part { get; set; }
        public double PIIssueLCnDONotReceive { get; set; }
        public double ProductionYetToSample { get; set; }
        public double DocInCnF { get; set; }
        public double DocReceive { get; set; }
        public double ShipmentDone { get; set; }
        public double InvoiceWithoutBL { get; set; }
        public double LCOpen { get; set; }
        public double GoodsRelease { get; set; }
        public double GoodsinTrasit { get; set; }
        public double MinimumStock { get; set; }
        public double Short_Excess { get; set; }
        public double POReceive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        #region  Reporting Property
        public double ProductionYetTo { get; set; }
        public int ReportType { get; set; }
        public int SortType { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }
        public double CurrentSalable
        {
            get
            {
                return (this.StockInHand - this.ProductionYetTo - this.LCReceiveDONoReceive - this.PIIssueLCnDONotReceive);
            }
        }
        public double Total_Import
        {
            get
            {
                return (this.StockInHand + this.GoodsRelease + this.GoodsinTrasit + this.DocInCnF + this.DocReceive + this.ShipmentDone + this.InvoiceWithoutBL + this.LCOpen + this.POReceive);
            }
        }
        public double NetSalable
        {
            get
            {
                return (this.StockInHand + this.GoodsRelease + this.GoodsinTrasit + this.DocInCnF + this.DocReceive + this.ShipmentDone + this.InvoiceWithoutBL + this.LCOpen + this.POReceive - this.ProductionYetTo - this.LCReceiveDONoReceive - this.PIIssueLCnDONotReceive);
            }
        }
        public double ShipmentInTransit
        {
            get
            {
                return (this.LCOpen + this.InvoiceWithoutBL);
            }
        }
    
        #endregion

        #endregion

       

        #region Functions
        public static List<ProductReconciliationReport> GetsPR(int nBUID, string sProductIDs, DateTime dStartDate, DateTime dEndDate,  int nReportType,int nSortType, long nUserID)
        {
            return ProductReconciliationReport.Service.GetsPR(nBUID,sProductIDs, dStartDate, dEndDate, nReportType,nSortType, nUserID);
        }
        public static List<ProductReconciliationReport> GetsImport(int nBUID, string sProductIDs, int nReportType, long nUserID)
        {
            return ProductReconciliationReport.Service.GetsImport( nBUID,sProductIDs, nReportType, nUserID);
        }
      
        #endregion


        #region ServiceFactory
        internal static IProductReconciliationReportService Service
        {
            get { return (IProductReconciliationReportService)Services.Factory.CreateService(typeof(IProductReconciliationReportService)); }
        }
    
        #endregion
        
    }
    #endregion

    

    #region IProductReconciliationReport interface
    public interface IProductReconciliationReportService
    {
        List<ProductReconciliationReport> GetsPR(int nBUID, string sProductIDs, DateTime dStartDate, DateTime dEndDate, int nReportType,int nSortType, Int64 nUserID);
       List<ProductReconciliationReport> GetsImport(int nBUID, string sProductIDs, int nReportType, Int64 nUserID);
    }
    #endregion

    
}
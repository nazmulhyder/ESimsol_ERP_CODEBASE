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
    #region ProductReconciliationReportDetail
    public class ProductReconciliationReportDetail : BusinessObject
    {
        #region  Constructor
        public ProductReconciliationReportDetail()
        {
            LotNo = "";
            IssueDate = DateTime.Now;
            ProductCode = "";
            BuyerName = "";
            PINo = "";
            Qty = 0;
            BLNo = "";
            BLDate = DateTime.MinValue;
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
        #region ProductParentID
        private int _nProductParentID = 0;
        public int ProductParentID
        {
            get { return _nProductParentID; }
            set { _nProductParentID = value; }
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
        #region LCNo
        private string _sLCNo = "";
        public string LCNo
        {
            get { return _sLCNo; }
            set { _sLCNo = value; }
        }
        #endregion
        #region InvoiceNo
        private string _sInvoiceNo = "";
        public string InvoiceNo
        {
            get { return _sInvoiceNo; }
            set { _sInvoiceNo = value; }
        }
        #endregion
        #region ContractorName
        private string _sContractorName = "";
        public string ContractorName
        {
            get { return _sContractorName; }
            set { _sContractorName = value; }
        }
        #endregion
        #region WUName
        private string _sWUName = "";
        public string WUName
        {
            get { return _sWUName; }
            set { _sWUName = value; }
        }
        #endregion
        public DateTime IssueDate { get; set; }
        public string ProductCode { get; set; }
        public string BuyerName { get; set; }
        public string PINo { get; set; }
        public string GRNNo { get; set; }
        public string BLNo { get; set; }
        public DateTime BLDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public double StockInHand { get; set; }
        public string PONo { get; set; }
        public string LotNo { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
        public double Value { get; set; }
        public double ShipmentDone { get; set; }
        public EnumPIStatus PIStatus { get; set; }
        public double POQty { get; set; }
        //public double Balance { get; set; }
        public double OrderQty { get; set; }
        public double Qty_Prod { get; set; }
        public double DeliveryQty { get; set; }
        public double YetToDelivery { get; set; }
        public double YetToProduction { get; set; }
        public double ReadyStockInhand { get; set; }
        public double PIQty { get; set; }
        public int CurrentStatus_LC { get; set; }
        public int AmendmentNo { get; set; }
        public bool AmendmentRequired { get; set; }
        public DateTime AmendmentDate { get; set; }
        
        #region  Reporting Property
        public string ErrorMessage { get; set; }  
        public int ReportType { get; set; }
        public string IssueDateSt
        {
            get
            {
                if (this.IssueDate == DateTime.MinValue) { return ""; }
                else return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string BLDateSt
        {
            get
            {
                if (this.BLDate == DateTime.MinValue) { return ""; }
                else return this.BLDate.ToString("dd MMM yyyy");
            }
        }
        public string ShipmentDateSt
        {
            get
            {
                if (this.ShipmentDate == DateTime.MinValue) { return ""; }
                else return this.ShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string PIStatusSt
        {
            get
            {
                return this.PIStatus.ToString();
            }
        }
        public string LCStatusSt
        {
            get
            {
                if (this.AmendmentRequired)
                {
                    return "AmendmentRequired";
                }
                else
                {
                    return ((EnumExportLCStatus)this.CurrentStatus_LC).ToString();
                }
            }
        }
        #endregion

        #endregion

       

        #region Functions
        public static DataSet Gets_ProductReconciliationReportDetail(int nProductID, DateTime dStartDate, DateTime dEndDate, int nReportType, Int64 nUserID)
        {
            return ProductReconciliationReportDetail.Service.Gets_ProductReconciliationReportDetail(nProductID,  dStartDate, dEndDate,nReportType, nUserID);
        }
        public static List<ProductReconciliationReportDetail> Gets(string sSQL, Int64 nUserID)
        {
            return ProductReconciliationReportDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<ProductReconciliationReportDetail> Gets_PRDetail(int nBUID, int nProductID, DateTime dStartDate, DateTime dEndDate, int nReportType, Int64 nUserID)
        {
            return ProductReconciliationReportDetail.Service.Gets_PRDetail(nBUID, nProductID, dStartDate, dEndDate, nReportType, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IProductReconciliationReportDetailService Service
        {
            get { return (IProductReconciliationReportDetailService)Services.Factory.CreateService(typeof(IProductReconciliationReportDetailService)); }
        }
      
        #endregion
        
    }
    #endregion

    

    #region IProductReconciliationReportDetail interface
    public interface IProductReconciliationReportDetailService
    {
        DataSet Gets_ProductReconciliationReportDetail(int nProductID, DateTime dStartDate, DateTime dEndDate,int nReportType, Int64 nUserID);
        List<ProductReconciliationReportDetail> Gets(string sSQL, Int64 nUserID);
        List<ProductReconciliationReportDetail> Gets_PRDetail(int nBUID, int nProductID, DateTime dStartDate, DateTime dEndDate, int nReportType, Int64 nUserID);
    }
    #endregion

    
}
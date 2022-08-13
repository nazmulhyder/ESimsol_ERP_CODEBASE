using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Security.Cryptography.X509Certificates;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class LotStockReport
    {
        public LotStockReport()
        {
            BUID = 0;
            WorkingUnitID = 0;
            LotID = 0;
            LotNo = "";
            LCNo = "";
            InvoiceNo = "";
            ProductID = 0;
            ProductBaseID= 0;
            ProductCategoryID = 0;
            ProductName = "";
            CategoryName = "";
            ProductName_Base = "";
            ProductCode = "";
            Balance = 0;
            Qty_Total = 0;
            ContractorName = "";
            LocationName = "";
            OperationUnitName = "";
            
			ReceiveDate =DateTime.MinValue;
			IssueDate =DateTime.MinValue;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

			MUnitID =0;
			MUnit ="";
            Qty_Opening = 0.0;
            Qty_Received = 0.0;
            Qty_Issue = 0.0;
            Qty_Balance = 0.0;
			ContractorID =0;
            ErrorMessage = "";
        }

        #region Properties
        public int BUID { get; set; }
        public int WorkingUnitID { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public string LCNo { get; set; }
        public string InvoiceNo { get; set; }
        public double Qty_Total { get; set; }
        public double Balance { get; set; }
        public int ProductID { get; set; }
        public int ProductCategoryID { get; set; }
        public int ProductBaseID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string ProductName_Base { get; set; }
        public string OperationUnitName { get; set; }
        public string ContractorName { get; set; }
        public string LocationName { get; set; }

        public string OrderNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MUnitID { get; set; }
        public string MUnit { get; set; }
        public double Qty_Opening { get; set; }
        public double Qty_Received { get; set; }
        public double Qty_Issue { get; set; }
        public double Qty_Balance { get; set; }
        //public double QtyGRN { get; set; }
        public double QtyAdjIn { get; set; }
        public double QtyAdjOut { get; set; }
        public double QtyTr { get; set; }
        public double QtyProOut { get; set; }
        public double QtyProIn { get; set; }
        public double QtyAssign { get; set; }
        public int ContractorID { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingCriteria { get; set; }
        public double Qty_Out
        {
            get
            {
                return Math.Round((this.Qty_Total -this.Balance),2);
            }
        }
        public string Params { get; set; }

        #endregion

        #region Derived
        public string IssueDateSt { get { return IssueDate.ToString("dd MMM yyyy"); } }
        public string ReceiveDateSt { get { return ReceiveDate.ToString("dd MMM yyyy"); } }
        #endregion

        #region Function
        public static List<LotStockReport> Gets(string sSQL, Int64 nUserID)
        {
            return LotStockReport.Service.Gets(sSQL, nUserID);
        }
        public static List<LotStockReport> GetsRPTLot(LotStockReport oLotStockReport, Int64 nUserID)
        {
            return LotStockReport.Service.GetsRPTLot(oLotStockReport, nUserID);
        }
        public static List<LotStockReport> GetsAll_RPTLot(LotStockReport oLotStockReport, Int64 nUserID)
        {
            return LotStockReport.Service.GetsAll_RPTLot(oLotStockReport, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILotStockReportService Service
        {
            get { return (ILotStockReportService)Services.Factory.CreateService(typeof(ILotStockReportService)); }
        }
        #endregion

        public static List<LotStockReport> Gets_StockWiseLotReport(DateTime startdate, DateTime endate, string WUIDs, int nUserID)
        {
            return LotStockReport.Service.Gets_StockWiseLotReport(startdate, endate, WUIDs, nUserID);
        }
    }

    #region ILotStockReport interface
    public interface ILotStockReportService
    {
        List<LotStockReport> Gets(string sSQL, long nUserID);
        List<LotStockReport> GetsRPTLot(LotStockReport oLotStockReport, long nUserID);
        List<LotStockReport> GetsAll_RPTLot(LotStockReport oLotStockReport, long nUserID);
        List<LotStockReport> Gets_StockWiseLotReport(DateTime startdate, DateTime endate, string WUIDs, int nUserID);
    }
    #endregion
}

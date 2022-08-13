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
    #region ConsumptionReport
    public class ConsumptionReport : BusinessObject
    {
        public ConsumptionReport()
        {
            ITransactionID = 0;
            LotNo = "";
            ProductID = 0;
            ConsumptionUnitID = 0;
            ConsumptionDetailID = 0;
            ColorID = 0;
            SizeID = 0;
            MUnitID = 0;
            IssueQty = 0;
            UnitPrice = 0;
            ConsumptionValue = 0;
            ProductCode = "";
            ProductName = "";
            MUnitSymbol = "";
            MUnitName = "";
            ColorName = "";
            SizeName = "";
            ParentConsumptionUnitID = 0;
            CUSequence = 0;
            ParentConsumptionUnitName = "";
            ProductCategoryID = 0;
            ProductCategoryName = "";
            ConsumptionUnitName = "";
            TransactionTime = DateTime.Now;
            ConsumptionBy = 0;
            ConsumptionByName = "";
            BUID = 0;
            BUName = "";
            StoreID = 0;
            ShiftInInt = 0;
            StoreName = "";
            SearchingData = "";
            ReportLayout = 0;
            FileNo = "";
            StyleNo = "";
            TransactionTimeInString = "";
            ErrorMessage = "";
            CUGroupID = 0;
            CUGroupName = "";
        }

        #region Properties
        public int ITransactionID { get; set; }
        public string LotNo { get; set; }
        public int ProductID { get; set; }
        public int ConsumptionUnitID { get; set; }
        public int ConsumptionDetailID { get; set; }
        public int ShiftInInt { get; set; } 
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public int MUnitID { get; set; }
        public double IssueQty { get; set; }
        public double UnitPrice { get; set; }
        public double ConsumptionValue { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUnitSymbol { get; set; }
        public string MUnitName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public int ParentConsumptionUnitID { get; set; }
        public int CUSequence { get; set; }
        public string ParentConsumptionUnitName { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public string ConsumptionUnitName { get; set; }
        public DateTime TransactionTime { get; set; }
        public int ConsumptionBy { get; set; }
        public string ConsumptionByName { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string ShiftName { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        public string TransactionTimeInString { get; set; }
        public string StyleNo { get; set; }
        public string FileNo { get; set; }

        public int CUGroupID { get; set; }
        public string CUGroupName { get; set; }
        #endregion

        #region Derived Property
        public int ReportLayout { get; set; }
        public int ParentSequence { get; set; }
        public Company Company { get; set; }
        
        #endregion

        #region Functions
        public static List<ConsumptionReport> Gets(string sSQL, Int64 nUserID)
        {
            return ConsumptionReport.Service.Gets(sSQL, nUserID);
        }
        public static List<ConsumptionReport> GetsConsumptionSummary(string sSQL, Int64 nUserID)
        {
            return ConsumptionReport.Service.GetsConsumptionSummary(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IConsumptionReportService Service
        {
            get { return (IConsumptionReportService)Services.Factory.CreateService(typeof(IConsumptionReportService)); }
        }
        #endregion
    }
    #endregion

    #region IConsumptionReport interface

    public interface IConsumptionReportService
    {
        List<ConsumptionReport> Gets(string sSQL, Int64 nUserID);
        List<ConsumptionReport> GetsConsumptionSummary(string sSQL, Int64 nUserID);
    }
    #endregion
}
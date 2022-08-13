using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region SalesComissionRegister
    public class SalesComissionRegister : BusinessObject
    {
        public SalesComissionRegister()
        {
            ExportPIDetailID = 0;
            ExportPIID = 0;
            ProductID = 0;
            MUnitID = 0;
            Qty = 0;
            StyleNo = "";
            BUID = 0;
            IssueDate = DateTime.MinValue;
            PIStatus = EnumPIStatus.Initialized;
            MotherBuyerID = 0;
            BuyerID = 0;
            OrderSheetDetailID = 0;
            ModelReferenceID = 0;            
            ApproveBy = 0;
            Description = "";
            BuyerReference = "";
            ProductDescription = "";
            ProductCode = "";
            ProductName = "";
            MUName = "";
            MUSymbol = "";
            BuyerName = "";
            MotherBuyerName = "";
            Measurement = "";
            ColorInfo = "";
            CRate = 0;
            PINo = "";
            ExportLCNo = "";
            Amount = 0;
            PIDateMonth = DateTime.Now;
            LCOpenDateMonth = DateTime.Now;
            ErrorMessage = "";
            SearchingData = "";
            ChallanInfo = "";
            Balance = 0;
            LCRecivedDate = DateTime.Now;
            LCValue = 0;
            ExportLCID = 0;
            UnitPrice = 0;
            RateUnit = 1;
            ExportBills = new List<ExportBill>();
            ReportLayout = EnumReportLayout.None;
        }

        #region Properties
        public int ExportPIDetailID { get; set; }
        public int ExportPIID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public string StyleNo { get; set; }
        public int BUID { get; set; }
        public DateTime PIDateMonth { get; set; }
        public DateTime LCOpenDateMonth { get; set; }
        public DateTime IssueDate { get; set; }
        public EnumPIStatus PIStatus { get; set; }
        public int PIStatusInInt { get; set; }
        public int MotherBuyerID { get; set; }
        public int BuyerID { get; set; }
        public double Amount { get; set; }
        public int OrderSheetDetailID { get; set; }
        public int ModelReferenceID { get; set; }
        public int ColorID { get; set; }
        public int ApproveBy { get; set; }
        public double UnitPrice { get; set; }
        public string Description { get; set; }
        public string BuyerReference { get; set; }
        public string PINo { get; set; }
        public string ExportLCNo { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public string BuyerName { get; set; }
        public string MotherBuyerName { get; set; }
        public string Measurement { get; set; }
        public string ColorInfo { get; set; }
        public double CRate { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        public string ChallanInfo { get; set; }
        public double Balance { get; set; }
        public DateTime LCRecivedDate { get; set; }
        public double LCValue { get; set; }
        public int ExportLCID { get; set; }
        public int RateUnit { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property
        public List<ExportBill> ExportBills { get; set; }

        public string LCRecivedDateInString
        {
            get
            {
                if (this.LCRecivedDate!=DateTime.MinValue)
                {
                    return this.LCRecivedDate.ToString("dd MMM yyyy");
                }
                else
                {
                    return "";
                }
            }
        }
        public string PIDateMonthSt
        {
            get
            {
                return this.PIDateMonth.ToString("MMM yyyy");
            }
        }
        public string LCOpenDateMonthSt
        {
            get
            {
                return this.LCOpenDateMonth.ToString("MMM yyyy");
            }
        }
        
        public string IssueDateSt
        {
            get 
            {
                if (this.IssueDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.IssueDate.ToString("dd MMM yyyy");
                }
            }
        }
    

        public string PIStatusSt
        {
            get
            {
                return EnumObject.jGet(this.PIStatus);
            }
        }
   
        
        #endregion

        #region Functions
        public static List<SalesComissionRegister> Gets(string sSQL, long nUserID)
        {
            return SalesComissionRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISalesComissionRegisterService Service
        {
            get { return (ISalesComissionRegisterService)Services.Factory.CreateService(typeof(ISalesComissionRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region ISalesComissionRegister interface

    public interface ISalesComissionRegisterService
    {
        List<SalesComissionRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}

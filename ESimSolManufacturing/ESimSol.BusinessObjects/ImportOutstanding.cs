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
    public class ImportOutstanding
    {
        public ImportOutstanding()
        {
            BankShortName = "";
            ErrorMessage = "";
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            BUID = 0;
            BankName = "";
            BUID = 0;
            BankName = "";
            BUName = "";
            ShipmenmentInTransit = 0;
            ShipmenDone = 0;
            DocInBank = 0;
            DocInHand = 0;
            DocInCnF = 0;
            GoodsInTransit = 0;
            Accpt_WithoutStockIn = 0;
            ABP_WithoutStockIn = 0;
            ABP_WithStockIn = 0;
            CurrencyName="";
            Amount = 0;
            nReportType = 0;
        }
        #region Properties


        public double LCOpen { get; set; }
        public double ShipmenmentInTransit { get; set; }
        public double ShipmenDone { get; set; }
        public double DocInBank { get; set; }
        public double DocInHand { get; set; }
        public double DocInCnF { get; set; }
        public double GoodsInTransit { get; set; }
        public double Accpt_WithoutStockIn { get; set; }
        public double Accpt_WithStockIn { get; set; }
        public double ABP_WithoutStockIn { get; set; }
        public double ABP_WithStockIn { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public int CurrencyID { get; set; }
        public int BankBranchID { get; set; }
        public EnumLCPaymentType LCPaymentType { get; set; }
        public string BankShortName { get; set; }
        public string BankName { get; set; }
        public string CurrencyName { get; set; }
        public bool IsBaseCurrency { get; set; }
        public int nReportType { get; set; }

        public double Amount { get; set; }
        public int Year_Maturity { get; set; }
        public int Month_Maturity { get; set; }
        public string MonthName { get; set; }



        #endregion

        #region Derive Properties
        public double Total
        {
            get
            {
                return this.LCOpen + this.ShipmenmentInTransit + this.ShipmenDone + this.DocInBank + this.DocInHand + this.DocInCnF + this.GoodsInTransit;
            }
        }
        public double TotalABP
        {
            get
            {
                return this.ABP_WithStockIn + this.ABP_WithStockIn;
            }
        }
        public string LCPaymentTypeSt
        {
            get
            {
                return this.LCPaymentType.ToString();
            }
        }

        public string StartDateSt
        {
            get
            {
                return (this.StartDate == DateTime.Now ? " " : this.StartDate.ToString("dd MMM yyyy"));
            }
        }
        public string ToDateSt
        {
            get
            {
                return (this.EndDate == DateTime.Now ? " " : this.EndDate.ToString("dd MMM yyyy")); 
            }
        }
        #endregion
        #region Functions
        public static List<ImportOutstanding> Gets(DateTime dFromDODate, DateTime dToDODate, int nBUID, int nCurrencyID,int nDate, long nUserID)
        {
            return ImportOutstanding.Service.Gets(dFromDODate, dToDODate, nBUID, nCurrencyID, nDate, nUserID);
        }
        public static List<ImportOutstanding> GetsImportOutstandingReport(string sSQL, int nUserID)
        {
            return ImportOutstanding.Service.GetsImportOutstandingReport(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IImportOutstandingService Service
        {
            get { return (IImportOutstandingService)Services.Factory.CreateService(typeof(IImportOutstandingService)); }
        }
        #endregion
    }

    #region IImportOutstanding interface
    public interface IImportOutstandingService
    {
        List<ImportOutstanding> Gets(DateTime dFromDODate, DateTime dToDODate, int nBUID, int nCurrencyID, int nDate, long nUserID);
        List<ImportOutstanding> GetsImportOutstandingReport(string sSQL, Int64 nUserID);
    }
    #endregion
}

using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region SP_ChangesInEquity
    public class SP_ChangesInEquity : BusinessObject
    {
        public SP_ChangesInEquity()
        {
            TransactionType = EnumEquityTransactionType.None;
            ShareCapital = 0;
            SharePremium = 0;
            ExcessOfIssuePrice = 0;
            CapitalReserve = 0;
            RevaluationSurplus = 0;
            FairValueGainOnInvestment = 0;
            RetainedEarnings = 0;
            TotalAmount = 0;

            AccountingSessionID = 0;
            BusinessUnitID = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            SessionName = "";
            PreviousSessionName = "";
            SP_ChangesInEquitys = new List<SP_ChangesInEquity>();

            ErrorMessage = "";
        }
        #region Properties
        public EnumEquityTransactionType TransactionType { get; set; }
        public Double ShareCapital { get; set; }
        public Double SharePremium { get; set; }
        public Double ExcessOfIssuePrice { get; set; }
        public Double CapitalReserve { get; set; }
        public Double RevaluationSurplus { get; set; }
        public Double FairValueGainOnInvestment { get; set; }
        public Double RetainedEarnings { get; set; }
        public Double TotalAmount { get; set; }
        public int AccountingSessionID { get; set; }
        public int BusinessUnitID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PreviousSessionName { get; set; }
        public string SessionName { get; set; }


        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<SP_ChangesInEquity> SP_ChangesInEquitys { get; set; }
        public string IncomeSt { get { return "Total Comprehensive Income for "; } }
        public string StockSt { get { return "Transactions with The Shareholders:"; } }
        public string TransactionTypeSt { get { return EnumObject.jGet(this.TransactionType); } }
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        public string StartDateFullSt { get { return this.StartDate.ToString("MMMM dd, yyyy"); } }
        public string EndDateFullSt { get { return this.EndDate.ToString("MMMM dd, yyyy"); } }
        public string EndDateHeaderSt { get { return this.EndDate.ToString("dd MMMM yyyy"); } }
        public string ShareCapitalSt { get { return this.ShareCapital < 0 ? "(" + Global.MillionFormat(this.ShareCapital * (-1)) + ")" : this.ShareCapital == 0 ? "-" : Global.MillionFormat(this.ShareCapital); } }
        public string SharePremiumSt { get { return this.SharePremium < 0 ? "(" + Global.MillionFormat(this.SharePremium * (-1)) + ")" : this.SharePremium == 0 ? "-" : Global.MillionFormat(this.SharePremium); } }
        public string ExcessOfIssuePriceSt { get { return this.ExcessOfIssuePrice < 0 ? "(" + Global.MillionFormat(this.ExcessOfIssuePrice * (-1)) + ")" : this.ExcessOfIssuePrice == 0 ? "-" : Global.MillionFormat(this.ExcessOfIssuePrice); } }
        public string CapitalReserveSt { get { return this.CapitalReserve < 0 ? "(" + Global.MillionFormat(this.CapitalReserve * (-1)) + ")" : this.CapitalReserve == 0 ? "-" : Global.MillionFormat(this.CapitalReserve); } }
        public string RevaluationSurplusSt { get { return this.RevaluationSurplus < 0 ? "(" + Global.MillionFormat(this.RevaluationSurplus * (-1)) + ")" : this.RevaluationSurplus == 0 ? "-" : Global.MillionFormat(this.RevaluationSurplus); } }
        public string FairValueGainOnInvestmentSt { get { return this.FairValueGainOnInvestment < 0 ? "(" + Global.MillionFormat(this.FairValueGainOnInvestment * (-1)) + ")" : this.FairValueGainOnInvestment == 0 ? "-" : Global.MillionFormat(this.FairValueGainOnInvestment); } }
        public string RetainedEarningsSt { get { return this.RetainedEarnings < 0 ? "(" + Global.MillionFormat(this.RetainedEarnings * (-1)) + ")" : this.RetainedEarnings == 0 ? "-" : Global.MillionFormat(this.RetainedEarnings); } }
        public string TotalAmountSt { get { return this.TotalAmount < 0 ? "(" + Global.MillionFormat(this.TotalAmount * (-1)) + ")" : this.TotalAmount == 0 ? "-" : Global.MillionFormat(this.TotalAmount); } }
        #endregion

        #region Functions


        public static List<SP_ChangesInEquity> Gets(int nAccountingSessionID, int nBusinessUnitID, int nUserID)
        {
            return SP_ChangesInEquity.Service.Gets(nAccountingSessionID, nBusinessUnitID, nUserID);
        }

        #endregion


        #region ServiceFactory
        internal static ISP_ChangesInEquityService Service
        {
            get { return (ISP_ChangesInEquityService)Services.Factory.CreateService(typeof(ISP_ChangesInEquityService)); }
        }
        #endregion
    }
    #endregion



    #region ISP_ChangesInEquity interface
    public interface ISP_ChangesInEquityService
    {

        List<SP_ChangesInEquity> Gets(int nAccountingSessionID, int nBusinessUnitID, int nUserID);


    }
    #endregion
}
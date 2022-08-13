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
    #region GLMonthlySummary
    public class GLMonthlySummary : BusinessObject
    {
        public GLMonthlySummary()
        {
            NameOfMonth = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now; ;
            DebitAmount = 0;
            CreditAmount = 0;
            ClosingAmount = 0;
            ComponentType = EnumComponentType.None;
            AccountHeadID = 0;
            BusinessUnitID = 0;
            CurrencyID = 0;
            IsApproved = false;
            IsForCurrentDate = false;
            BusinessUnitIDs = "0";
            BUName = "Group Accounts";
            ErrorMessage = "";
        }

        #region Properties
        public int AccountHeadID { get; set; }
        public string NameOfMonth { get; set; }
        public EnumComponentType ComponentType { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double ClosingAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ErrorMessage { get; set; }
        public int BusinessUnitID { get; set; }
        public string BusinessUnitIDs { get; set; }
        public string BUName { get; set; }
         
        public int CurrencyID { get; set; }
        public bool IsApproved { get; set; }
        public bool IsForCurrentDate { get; set; }
        #endregion

        #region Derived Property
        public List<GLMonthlySummary> GLMonthlySummarys { get; set; }
        public string DebitAmountInString { get { return this.DebitAmount < 0 ? "(" + Global.MillionFormat(this.DebitAmount * (-1)) + ")" : this.DebitAmount == 0 ? "-" : Global.MillionFormat(this.DebitAmount); } }
        public string CreditAmountInString { get { return this.CreditAmount < 0 ? "(" + Global.MillionFormat(this.CreditAmount * (-1)) + ")" : this.CreditAmount == 0 ? "-" : Global.MillionFormat(this.CreditAmount); } }

        #region ClosingBalanceSt
        public string ClosingBalanceSt { get { return this.ClosingAmount == 0 ? "-" : this.ClosingAmount < 0 ? "(" + Global.MillionFormat(this.ClosingAmount * (-1)) + ")" : Global.MillionFormat(Math.Abs(this.ClosingAmount)); } }

        public string DebitClosingBalanceSt { get { return this.ComponentType == EnumComponentType.Asset || this.ComponentType == EnumComponentType.Expenditure ? this.ClosingBalanceSt : "-"; } }
        public string CreditClosingBalanceSt { get { return this.ComponentType == EnumComponentType.Asset || this.ComponentType == EnumComponentType.Expenditure ? "-" : this.ClosingBalanceSt; } }
        #endregion

        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        #endregion

        #region Functions
        public static List<GLMonthlySummary> Gets(int nAccountHead, DateTime dStartDate, DateTime dEndDate, int nCurrencyID, bool bISApproved, string BusinessUnitIDs, int nUserID)
        {
            return GLMonthlySummary.Service.Gets(nAccountHead, dStartDate, dEndDate, nCurrencyID, bISApproved, BusinessUnitIDs, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IGLMonthlySummaryService Service
        {
            get { return (IGLMonthlySummaryService)Services.Factory.CreateService(typeof(IGLMonthlySummaryService)); }
        }
        #endregion
    }
    #endregion

    #region IGLMonthlySummary interface
    public interface IGLMonthlySummaryService
    {
        List<GLMonthlySummary> Gets(int nAccountHead, DateTime dStartDate, DateTime dEndDate, int nCurrencyID, bool bISApproved, string BusinessUnitIDs, int nUserID);
    }
    #endregion


}
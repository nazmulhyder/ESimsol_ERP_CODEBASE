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

    #region CCOpeningBreakdown
    public class CCOpeningBreakdown : BusinessObject
    {
        public CCOpeningBreakdown()
        {
            AccountHeadID = 0;
            IsDebit = false;
            IsDrClosing = false;
            ComponentID = 0;
            CCID = 0;
            VoucherBillID = 0;
            ProductID = 0;
            OrderID = 0;
            BreakdownType = EnumBreakdownType.VoucherDetail;
            BreakodwnID = 0;
            BreakdownName = "";
            AccountHeadName = "";
            AccountHeadCode = "";
            CCName = "";

            OpeningAmount = 0;
            ClosingAmount = 0;
            DebitOpeningAmount = 0;
            CreditOpeningAmount = 0;
            DebitAmount = 0;
            CreditAmount = 0;
            StartDate = DateTime.Now;
            BUName = "Group Accounts";
            BusinessUnitIDs = "0";
            ErrorMessage = "";
        }

        #region Properties
        public int CCID { get; set; }
        public int ProductID { get; set; }
        public int OrderID { get; set; }
        public int BreakodwnID { get; set; }
        public EnumBreakdownType BreakdownType { get; set; }
        public string BreakdownName { get; set; }
        public bool IsDebit { get; set; }
        public bool IsDrClosing { get; set; }
        public int ComponentID { get; set; }
        public double DebitOpeningAmount { get; set; }
        public double CreditOpeningAmount { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double OpeningAmount { get; set; }
        public double ClosingAmount { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime StartDate { get; set; }
        public int AccountHeadID { get; set; }
        public bool IsApproved { get; set; }
        public int CurrencyID { get; set; }
        public bool IsForCurrentDate { get; set; }
        public string AccountHeadName { get; set; }
        public string AccountHeadCode { get; set; }
        public string CCName { get; set; }
        public string CCCode { get; set; }
        public int VoucherBillID { get; set; }
        public int BusinessUnitID { get; set; }
        public string BusinessUnitIDs  { get; set; }
        public string BUName { get; set; }
        #endregion

        #region Derived Property
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public  List<CCOpeningBreakdown> CCOpeningBreakdowns { get; set; }
        public Company Company { get; set; }
        public Currency Currency { get; set; }
        public string DebitAmountSt { get { return this.DebitAmount == 0 ? "-" : Global.MillionFormat(this.DebitAmount); } }
        public string CreditAmountSt { get { return this.CreditAmount == 0 ? "-" : Global.MillionFormat(this.CreditAmount); } }

        public string IsDebitSt { get { return this.IsDebit ? "Dr" : "Cr"; } }
        public string IsDrClosingSt { get { return this.IsDrClosing ? "Dr" : "Cr"; } }
        public string OpeningAmountSt { get { return this.OpeningAmount == 0 ? "-" : this.IsDebitSt + " " + Global.MillionFormat(this.OpeningAmount < 0 ? this.OpeningAmount * (-1) : this.OpeningAmount); } }
        public string ClosingAmountSt { get { return this.ClosingAmount == 0 ? "-" : this.IsDrClosingSt + " " + Global.MillionFormat(this.ClosingAmount < 0 ? this.ClosingAmount * (-1) : this.ClosingAmount); } }
        
        #endregion

        #region Functions
        public static List<CCOpeningBreakdown> Gets(string BusinessUnitIDs, int nCCID, int nAccountHeadID, int nCurrencyID, DateTime StartDate, bool bIsApproved, int nUserID)
        {
            return CCOpeningBreakdown.Service.Gets(BusinessUnitIDs, nCCID, nAccountHeadID, nCurrencyID, StartDate, bIsApproved, nUserID);
        }
       
        #endregion

        #region ServiceFactory
        internal static ICCOpeningBreakdownService Service
        {
            get { return (ICCOpeningBreakdownService)Services.Factory.CreateService(typeof(ICCOpeningBreakdownService)); }
        }
        #endregion
    }
    #endregion

    #region ICCOpeningBreakdown interface
    public interface ICCOpeningBreakdownService
    {
        List<CCOpeningBreakdown> Gets(string BusinessUnitIDs, int nCCID, int nAccountHeadID, int nCurrencyID, DateTime StartDate, bool bIsApproved, int nUserID);
    }
    #endregion
    
   
}

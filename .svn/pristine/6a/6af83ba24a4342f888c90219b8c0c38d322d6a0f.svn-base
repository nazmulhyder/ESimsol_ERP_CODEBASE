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

    #region CostCenterBreakdown
    public class CostCenterBreakdown : BusinessObject
    {
        public CostCenterBreakdown()
        {
            CCTID = 0;
            CCID = 0;
            CCCode = "";
            CCName = "";
            OpeiningValue = 0;
            IsDebit = true;
            DebitAmount = 0;
            CreditAmount = 0;
            ClosingValue = 0;
            IsDrClosing = true;
            CurrencySymbol = "";
            CostCenterBreakdowns = new List<CostCenterBreakdown>();
            ErrorMessage = "";
            VoucherID = 0;
            VoucherDate = DateTime.Now;
            VoucherNo = "";
            IsApproved = true;
            AccountHeadName = "";
            ParentHeadID = 0;
            ParentHeadName = "";
            ParentHeadCode = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            IsNullOrNot = false;
            VoucherDetailID = 0;
            ConfigTitle = "";
            Narration = "";
            VoucherNarration = "";
            Description = "";
            Level = 0;
            CCOptionID = 0;
            BusinessUnitID = 0;
            VoucherBillID = 0;
            ComponentID = 0;
            BusinessUnitIDs = "0";
            BUName = "Group Accounts";
            ConfigType = EnumConfigureType.None;
            ACConfigs = new List<ACConfig>();
            BalanceStatus = EnumBalanceStatus.None;
            BalanceStatusInt = 0;

        }

        #region Properties
        public int CCTID { get; set; }
        public int CCID { get; set; }
        public string CCCode { get; set; }
        public string CCName { get; set; }
        public double OpeiningValue { get; set; }
        public bool IsDebit { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double ClosingValue { get; set; }
        public bool IsDrClosing { get; set; }
        public string  CurrencySymbol  {get;set;}
        public string ErrorMessage { get; set; }
        public int VoucherID { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public string DateType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AccountHeadID { get; set; }
        public bool IsApproved { get; set; }
        public int CurrencyID { get; set; }
        public bool IsForCurrentDate { get; set; }
        public string AccountHeadName { get; set; }
        public int ParentHeadID { get; set; }
        public string ParentHeadName { get; set; }
        public string ParentHeadCode { get; set; }
        public bool IsNullOrNot { get; set; }
        public int VoucherDetailID { get; set; }
        public string ConfigTitle { get; set; }
        public string Narration { get; set; }
        public string VoucherNarration { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public int CCOptionID { get; set; }
        public int VoucherBillID { get; set; }
        public int BusinessUnitID { get; set; }
        public int ComponentID { get; set; }
        public string BusinessUnitIDs { get; set; }
        public string BUName { get; set; }
        public EnumConfigureType ConfigType { get; set; }
        #endregion

        #region Derived Property
        public EnumBalanceStatus BalanceStatus { get; set; }
        public int BalanceStatusInt { get; set; }
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        public string NameCode { get { return this.CCName + " [" + this.CCCode + "]"; } }
        public string ParentHeadNameCode { get { return this.ParentHeadName + " [" + this.ParentHeadCode + "]"; } }
        public List<ACConfig> ACConfigs { get; set; }
        public List<Contractor> Contractors { get; set; }
        public List<Currency> Currencies { get; set; }
        public  List<CostCenterBreakdown> CostCenterBreakdowns { get; set; }
        public Company Company { get; set; }
        public Currency Currency { get; set; }
        public string DebitAmountInString
        {
            get

            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.DebitAmount);
            }
        }
        public string CreditAmountInString
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.CreditAmount);
            }
        }
        public string OpeningValueInString
        {
            get
            {
                if (this.IsDebit == true)
                {
                    return "Dr " + this.CurrencySymbol + " " + Global.MillionFormat(this.OpeiningValue);
                }
                else
                {
                    return "Cr " + this.CurrencySymbol + " " + Global.MillionFormat(this.OpeiningValue);
                }
            }
        }
        public string IsDrOpenInString
        {
            get
            {
                if(this.IsDebit == true)
                {
                    return "Dr";
                }
                else
                {
                    return "Cr";
                }
            }
        }
        public string VoucherDateInString { get { return this.VoucherID <= 0 && this.AccountHeadName != "Opening Balance" ? this.ConfigTitle : VoucherDate.ToString("dd MMM yyyy"); } }
        public string ClosingValueString
        {
            get
            {
                if (this.ClosingValue < 0)
                {
                    return "(" + Global.MillionFormat(this.ClosingValue * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.ClosingValue);
                }

            }
        }
        public string ClosingValueST
        {
            get
            {
                if (this.IsDrClosing == true)
                {
                    if (this.ClosingValue < 0)
                    {
                        return "Dr (" + Global.MillionFormat(this.ClosingValue * (-1)) + ")";
                    }
                    else
                    {
                        return "Dr " + Global.MillionFormat(this.ClosingValue);
                    }
                }
                else
                {
                    if (this.ClosingValue < 0)
                    {
                        return "Cr (" + Global.MillionFormat(this.ClosingValue * (-1)) + ")";
                    }
                    else
                    {
                        return "Cr " + Global.MillionFormat(this.ClosingValue);
                    }
                }
            }
        }

        //public string ClosingValueInString
        //{
        //    get
        //    {
        //        if (this.IsDrClosing == true)
        //        {
        //            return "Dr " + this.CurrencySymbol + " " + Global.MillionFormat(this.ClosingValue);
        //        }
        //        else
        //        {
        //            return "Cr " + this.CurrencySymbol + " " + Global.MillionFormat(this.ClosingValue);
        //        }

        //    }
        //}
        //public string IsDrClosingInString
        //{
        //    get
        //    {
        //        if(this.IsDrClosing == true)
        //        {
        //            return "Dr";
        //        }
        //        else
        //        {
        //            return "Cr";
        //        }
        //    }
        //}
        public string VoucherIDWithNo
        {
            get
            {
                return this.VoucherNo + "~" + this.VoucherID;
            }
        }
        public string CostCenterIDWithName
        {
            get
            {
                return this.CCName + "~" + this.CCID;
            }
        }
        #endregion

        #region Functions
        public static List<CostCenterBreakdown> Gets(string BUIDs, int nAccountHeadID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, int nUserID)
        {
            return CostCenterBreakdown.Service.Gets(BUIDs, nAccountHeadID, nCurrencyID, StartDate, EndDate, bIsApproved, nUserID);
        }
        public static List<CostCenterBreakdown> GetsAccountWiseBreakdown(int nAccountHeadID, string BUIDs, int nCCID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, EnumBalanceStatus eBalanceStatus, int nUserID)
        {
            return CostCenterBreakdown.Service.GetsAccountWiseBreakdown(nAccountHeadID, BUIDs, nCCID, nCurrencyID, StartDate, EndDate, bIsApproved, eBalanceStatus, nUserID);
        }
        public static List<CostCenterBreakdown> GetsForCostCenter(string BUIDs, int nAccountHeadID, int nCostCenterID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool IsApproved, int nUserID)
        {
            return CostCenterBreakdown.Service.GetsForCostCenter(BUIDs, nAccountHeadID, nCostCenterID, nCurrencyID, StartDate, EndDate, IsApproved, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICostCenterBreakdownService Service
        {
            get { return (ICostCenterBreakdownService)Services.Factory.CreateService(typeof(ICostCenterBreakdownService)); }
        }
        #endregion
    }
    #endregion

    #region ICostCenterBreakdown interface
    public interface ICostCenterBreakdownService
    {
        List<CostCenterBreakdown> Gets(string BUIDs, int nAccountHeadID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, int nUserID);
        List<CostCenterBreakdown> GetsAccountWiseBreakdown(int nAccountHeadID, string BUIDs, int nCCID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, EnumBalanceStatus eBalanceStatus, int nUserID);
        List<CostCenterBreakdown> GetsForCostCenter(string BUIDs, int nAccountHeadID, int nCostCenterID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool IsApproved, int nUserID);
    }
    #endregion
    
   
}

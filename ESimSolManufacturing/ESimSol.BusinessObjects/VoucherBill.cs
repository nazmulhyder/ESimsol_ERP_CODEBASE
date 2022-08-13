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
    #region VoucherBill
    public class VoucherBill : BusinessObject
    {
        public VoucherBill()
        {
            VoucherBillID = 0;
            AccountHeadID = 0;
            SubLedgerID = 0;
            BUID = 0;
            BillNo = "";            
            IsDebit = false;
            CreditDays = 0;
            ErrorMessage = "";
            BillDate = DateTime.Now;
            DueDate = DateTime.Now;
            TrType = EnumVoucherBillTrType.None;
            TrTypeInInt = 0;
            CurrencyRate = 0;
            CurrencyAmount = 0;
            ReferenceType = EnumVoucherBillReferenceType.None;
            ReferenceTypeInInt = 0;
            ReferenceObjID = 0;
            CurrencySymbol = "";
            BaseCurrencyID = 0;
            BaseCurrencySymbol = "";
            RemainningBalance = 0;
            AccountHeadName = "";
            SubLedgerCode = "";
            SubLedgerName = "";
            BUCode = "";
            BUName = "";
            BUShortName = "";
            IDs = "";
            AccSessionID = 0;
            BillAmountOpeType = 0;
            FromAmount = 0;
            ToAmount = 0;
            BillDateOpeType = 0;
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            FromDateString = "";
            ToDateString = "";
            ComponentID = 0; // Receivable=1 or payable=2
            OpeningBillAmount = 0;
            OpeningBillDate = DateTime.Today;
            Remarks = "";
            OverDueDays = 0;
            DueDays = 0;
            IsHoldBill = false;
            VoucherBillAgeSlabs = new List<VoucherBillAgeSlab>();
            VoucherBillAgings = new List<VoucherBillAging>();
            DueType = EnumDueType.AllDueBill;
            DueTypeInt = 1;
            VoucherBills = new List<VoucherBill>();            
            PartyAddress = "";
            BusinessUnits = new List<BusinessUnit>();
            DueCheque = 0;
        }

        #region Properties
        public int VoucherBillID { get; set; }
        public int AccountHeadID { get; set; }
        public int SubLedgerID { get; set; } // ACCostCenterID
        public int BUID { get; set; }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public int CreditDays { get; set; }
        public double Amount { get; set; }
        public double CurrencyRate { get; set; }
        public double CurrencyAmount { get; set; }
        public EnumVoucherBillReferenceType ReferenceType { get; set; }
        public int ReferenceTypeInInt { get; set; }
        public int ReferenceObjID { get; set; }
        public bool IsDebit { get; set; }
        public bool IsActive { get; set; }
        public double OpeningBillAmount { get; set; }
        public DateTime OpeningBillDate { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        public bool Selected { get; set; }
        public EnumVoucherBillTrType TrType { get; set; }
        public int TrTypeInInt { get; set; }
        public double RemainningBalance { get; set; }
        public int BaseCurrencyID { get; set; }
        public string BaseCurrencySymbol { get; set; }
        public string CurrencySymbol { get; set; }
        public string AccountHeadName { get; set; }
        public string SubLedgerCode { get; set; }
        public string SubLedgerName { get; set; }
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public string PartyAddress { get; set; }
        public string IDs { get; set; }
        public int AccSessionID { get; set; }
        public int BillAmountOpeType { get; set; }
        public double FromAmount { get; set; }
        public double ToAmount { get; set; }
        public int BillDateOpeType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FromDateString { get; set; }
        public string ToDateString { get; set; }
        public int ComponentID { get; set; }
        public int OverDueDays { get; set; }
        public int DueDays { get; set; }        
        public bool IsHoldBill { get; set; }        
        public EnumDueType DueType { get; set; } //only used for reporting 
        public int DueTypeInt { get; set; }
        public double DueCheque { get; set; }

        #region Derive Property
        public List<VoucherBillAgeSlab> VoucherBillAgeSlabs { get; set; }
        public List<VoucherBillAging> VoucherBillAgings { get; set; }       
        public int OverDueByDays { get { return this.OverDueDays < 0 ? 0 : this.OverDueDays; } }
        public int DueForDays { get { return this.DueDays < 0 ? 0 : this.DueDays; } }
        public List<Company> Companys { get; set; }
        public List<Currency> Currencys { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public List<EnumObject> CompareOperatorObjs { get; set; }
        public List<AccountingSession> AccountingSessions { get; set; }
        public List<ChartsOfAccount> COA_ChartsOfAccounts { get; set; }
        public List<VoucherBill> VoucherBills { get; set; }
        public Company Company { get; set; }
        public int CurrencyID { get; set; }
        public double CurrencyConversionRate { get; set; }
        public string Activity
        {
            get { return this.IsActive.ToString(); }
        }
        public string OpeningBillDateInString
        {
            get { return this.OpeningBillDate.ToString("dd MMM yyyy"); }
        }
        public string DueDateInString
        {
            get { return this.DueDate.ToString("dd MMM yyyy"); }
        }
        public string BillDateInString
        {
            get { return this.BillDate.ToString("dd MMM yyyy"); }
        }
        public string IsHoldBillSt
        {
            get 
            {
                if (this.IsHoldBill)
                {
                    return "Hold Bill";
                }
                else
                {
                    return "Regular Bill";
                }
            }
        }
        public string RemainningBalanceCFormat
        {
            get
            {
                if (this.CurrencyID == this.BaseCurrencyID)
                {
                    return this.BaseCurrencySymbol + " " + Global.MillionFormat(this.RemainningBalance)+ " " + (this.IsDebit == true ? "Dr" : "Cr");
                }
                else
                {
                    return this.CurrencySymbol + " " + Global.MillionFormat(this.RemainningBalance) + "@" + Global.MillionFormat(this.CurrencyRate) + " " + this.BaseCurrencySymbol + " " + Global.MillionFormat(this.RemainningBalance * this.CurrencyRate) + " " + (this.IsDebit == true ? "Dr" : "Cr");
                }
            }
        }
        public string CurrencyAmountWithSymbol
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.CurrencyAmount);
            }
        }
        public string CurrencyAmountAndSymbol
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.CurrencyAmount);
            }
        }
        public string AmountInMillionFormat
        {
            get
            {
                return this.BaseCurrencySymbol + " " + Global.MillionFormat(this.Amount) + " " + (this.IsDebit == true ? "Dr" : "Cr");
            }
        }
        public string RemainningBalanceInMillionFormat
        {
            get
            {
                return this.BaseCurrencySymbol + " " + Global.MillionFormat(this.RemainningBalance * this.CurrencyRate) + " " + (this.IsDebit == true ? "Dr" : "Cr");
            }
        }

        public string OpeningBillAmountAndSymbol
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.OpeningBillAmount);
            }
        }
        #endregion

        #endregion

        #region Functions
        public VoucherBill Get(int id, int nUserID)
        {
            return VoucherBill.Service.Get(id, nUserID);
        }
        public VoucherBill Save(int nUserID)
        {
            return VoucherBill.Service.Save(this, nUserID);
        }
        public VoucherBill HoldUnHold(int nUserID)
        {
            return VoucherBill.Service.HoldUnHold(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return VoucherBill.Service.Delete(id, nUserID);
        }
        public static List<VoucherBill> GetsReceivableOrPayableBill(int nComponentType, int nUserID)
        {
            return VoucherBill.Service.GetsReceivableOrPayableBill(nComponentType, nUserID);
        }
        public static List<VoucherBill> Gets(int nUserID)
        {
            return VoucherBill.Service.Gets(nUserID);
        }
        public static List<VoucherBill> Gets(string sSQL, int nUserID)
        {
            return VoucherBill.Service.Gets(sSQL, nUserID);
        }
        public static List<VoucherBill> GetsBy(int nAccountHeadID, int nUserID)
        {
            return VoucherBill.Service.GetsBy(nAccountHeadID, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IVoucherBillService Service
        {
            get { return (IVoucherBillService)Services.Factory.CreateService(typeof(IVoucherBillService)); }
        }
        #endregion
    }
    #endregion
    
    #region IVoucherBill interface
    public interface IVoucherBillService
    {
        VoucherBill Get(int id, int nUserID);
        string Delete(int id, int nUserID);
        VoucherBill Save(VoucherBill oVoucherBill, int nUserID);
        VoucherBill HoldUnHold(VoucherBill oVoucherBill, int nUserID);
        List<VoucherBill> GetsBy(int nAccountHeadID, int nUserID);
        List<VoucherBill> Gets(string sSQL, int nUserID);
        List<VoucherBill> Gets(int nUserID);
        List<VoucherBill> GetsReceivableOrPayableBill(int nComponentType, int nUserID);
    }
    #endregion
}

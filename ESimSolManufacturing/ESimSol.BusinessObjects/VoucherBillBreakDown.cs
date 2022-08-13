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
    #region VoucherBillBreakDown
    public class VoucherBillBreakDown : BusinessObject
    {
        public VoucherBillBreakDown()
        {
            VoucherBillID = 0;
            BillNo = "";
            OpeningValue = 0;
            IsDrOpen = true;
            DebitAmount = 0;
            CreditAmount = 0;
            ClosingValue = 0;
            IsDrClosing = true;
            CurrencySymbol = "";
            VoucherBillBreakDowns = new List<VoucherBillBreakDown>();
            ErrorMessage = "";
            VoucherID = 0;
            VoucherDate = DateTime.Now;
            VoucherNo = "";
            IsApproved = true;
            AccountHeadName = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        #region Properties
        public int VoucherBillID { get; set; }
        public string BillNo { get; set; }
        public double OpeningValue { get; set; }
        public bool IsDrOpen { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double ClosingValue { get; set; }
        public bool IsDrClosing { get; set; }
        public string CurrencySymbol { get; set; }
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
        public string AccountHeadName { get; set; }
        #endregion

        #region Derived Property
        public List<Contractor> Contractors { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<VoucherBillBreakDown> VoucherBillBreakDowns { get; set; }
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
                if (this.IsDrOpen == true)
                {
                    return "Dr " + this.CurrencySymbol + " " + Global.MillionFormat(this.OpeningValue);
                }
                else
                {
                    return "Cr " + this.CurrencySymbol + " " + Global.MillionFormat(this.OpeningValue);
                }

            }
        }
        public string IsDrOpenInString
        {
            get
            {
                if (this.IsDrOpen == true)
                {
                    return "Dr";
                }
                else
                {
                    return "Cr";
                }
            }
        }
        public string VoucherDateInString
        {
            get
            {
                return VoucherDate.ToString("dd MMM yyyy");
            }
        }
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
        public string ClosingValueInString
        {
            get
            {
                if (this.IsDrClosing == true)
                {
                    return "Dr " + this.CurrencySymbol + " " + Global.MillionFormat(this.ClosingValue);
                }
                else
                {
                    return "Cr " + this.CurrencySymbol + " " + Global.MillionFormat(this.ClosingValue);
                }
            }
        }
        public string IsDrClosingInString
        {
            get
            {
                if (this.IsDrClosing == true)
                {
                    return "Dr";
                }
                else
                {
                    return "Cr";
                }
            }
        }
        public string VoucherIDWithNo
        {
            get
            {
                return this.VoucherNo + "~" + this.VoucherID;
            }
        }
        public string VoucherBillIDWithNo
        {
            get
            {
                return this.BillNo + "~" + this.VoucherBillID;
            }
        }
        #endregion

        #region Functions
        public static List<VoucherBillBreakDown> Gets(int nAccountHeadID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, int nCompanyID, int nUserID)
        {
            return VoucherBillBreakDown.Service.Gets(nAccountHeadID, nCurrencyID, StartDate, EndDate, bIsApproved, nCompanyID, nUserID);
        }
        public static List<VoucherBillBreakDown> GetsForVoucherBill(int nAccountHeadID, int nVoucherBillID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool IsApproved, int nCompanyID, int nUserID)
        {
            return VoucherBillBreakDown.Service.GetsForVoucherBill(nAccountHeadID, nVoucherBillID,nCurrencyID, StartDate, EndDate, IsApproved, nCompanyID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVoucherBillBreakDownService Service
        {
            get { return (IVoucherBillBreakDownService)Services.Factory.CreateService(typeof(IVoucherBillBreakDownService)); }
        }
        #endregion
    }
    #endregion

    #region IVoucherBillBreakDown interface
    public interface IVoucherBillBreakDownService
    {
        List<VoucherBillBreakDown> Gets(int nAccountHeadID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, int nCompanyID, int nUserID);
        List<VoucherBillBreakDown> GetsForVoucherBill(int nAccountHeadID, int nVoucherBillID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool IsApproved, int nCompanyID, int nUserID);
    }
    #endregion
}

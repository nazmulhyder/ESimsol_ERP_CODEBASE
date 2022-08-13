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
    #region VoucherRefReport
    public class VoucherRefReport : BusinessObject
    {
        public VoucherRefReport()
        {
            VoucherBillID = 0;
            BillNo = "";
            AccountHeadID = 0;
            CCID = 0;
            CreditDays = 0;
            CompanyID = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            IsAllBill = false;
            BusinessUnitIDs = "0";
            BUName = "Group Accounts";
            ErrorMessage = "";
        }

        #region Properties
        public int VoucherBillID { get; set; }
        public int AccountHeadID { get; set; }
        public string BillNo { get; set; }
        public string RefObjCode { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public int CreditDays { get; set; }
        public int CompanyID { get; set; }
        public Double OpeningBalance { get; set; }
        public Double ClosingBalance { get; set; }
        public Double DebitAmount { get; set; }
        public Double CreditAmount { get; set; }
        public Double Amount { get; set; }
        public bool IsDebit { get; set; }
        public Double RemainingBalance { get; set; }
        public EnumComponentType ComponentType { get; set; }
        public string ErrorMessage { get; set; }
        public int OverdueByDays { get; set; }
        public int CCID { get; set; }
        public bool IsApproved { get; set; }
        public int CurrencyID { get; set; }
        public int BusinessUnitID { get; set; }
        public int VoucherID { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string AccountHeadName { get; set; }
        public string SubledgerCode { get; set; }
        public string SubledgerName { get; set; }
        public bool IsForCurrentDate { get; set; }
        public string BusinessUnitIDs { get; set; }
        public string BUName { get; set; }
        public bool IsAllBill { get; set; }
        
        
        #region Derive Property
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        public List<VoucherRefReport> VoucherRefReports { get; set; }
        public List<Company> Companys { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DueDateInString { get { return this.DueDate.ToString("dd MMM yyyy"); } }
        public string BillDateInString { get { return this.BillDate.ToString("dd MMM yyyy"); } }
        public string VoucherDateSt { get { return this.VoucherID > 0 ? this.VoucherDate.ToString("dd MMM yyyy") : ""; } }
        #region OpeningBalanceSt
        public string OpeningBalanceSt { get { return this.OpeningBalance < 0 ? "(" + Global.MillionFormat(this.OpeningBalance * -1) + ")" : this.OpeningBalance == 0 ? "-" : Global.MillionFormat(this.OpeningBalance); } }
        #endregion
        #region ClosingBalanceSt
        public string ClosingBalanceSt { get { return this.ClosingBalance < 0 ? "(" + Global.MillionFormat(this.ClosingBalance * -1) + ")" : this.ClosingBalance == 0 ? "-" : Global.MillionFormat(this.ClosingBalance); } }
        #endregion
        public Double DebitClosingBalance { get { return this.IsDebit ? this.ClosingBalance : 0; } }
        public Double CreditClosingBalance { get { return this.IsDebit ? 0 : this.ClosingBalance; } }
        public string DebitClosingBalanceSt { get { return this.IsDebit ? this.ClosingBalanceSt : "-" ; } }
        public string CreditClosingBalanceSt { get { return this.IsDebit ? "-" : this.ClosingBalanceSt; } }

        public string DebitAmountSt { get { return Global.MillionFormat(this.DebitAmount); } }
        public string CreditAmountSt { get { return Global.MillionFormat(this.CreditAmount); } }
        public string RemainingBalanceSt { get { return Global.MillionFormat(this.RemainingBalance); } }
        public string OverdueDays { get { return this.OverdueByDays < 0 ? "" : this.OverdueByDays.ToString(); } }
        #endregion
    
        #endregion

        #region Functions
        public static List<VoucherRefReport> GetsVoucherBillBreakDown(VoucherRefReport oCCT, int nUserID)
        {
            return VoucherRefReport.Service.GetsVoucherBillBreakDown(oCCT, nUserID);
        }
        public static List<VoucherRefReport> GetsVoucherBillDetails(VoucherRefReport oCCT, int nUserID)
        {
            return VoucherRefReport.Service.GetsVoucherBillDetails(oCCT, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVoucherRefReportService Service
        {
            get { return (IVoucherRefReportService)Services.Factory.CreateService(typeof(IVoucherRefReportService)); }
        }
        #endregion
    }
    #endregion
    
    #region IVoucherRefReport interface
    public interface IVoucherRefReportService
    {
        List<VoucherRefReport> GetsVoucherBillBreakDown(VoucherRefReport oCCT, int nUserID);
        List<VoucherRefReport> GetsVoucherBillDetails(VoucherRefReport oCCT, int nUserID);
    }
    #endregion

    
}

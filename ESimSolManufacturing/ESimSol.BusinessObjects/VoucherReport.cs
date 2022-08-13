using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region VoucherReport
    public class VoucherReport : BusinessObject
    {
        public VoucherReport()
        {
            VoucherID = 0;
            VoucherTypeID = 0;
            VoucherNo = "";
            VoucherDate = DateTime.Today;
            CurrencyId = 0;
            CurrencyConversionRate = 0;
            VoucherReportAmount = 0;
            CurrencySymbol = "";
            AmountInBaseCurrency = 0;
            ErrorMessage = "";
            AccountingSessionID = 0;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            AccountTypeName = "";
            VoucherTypeName = "";
        }

        #region Properties
        public int VoucherTypeID { get; set; }
        public string VoucherTypeName { get; set; }
        public string AccountTypeName { get; set; }
        public int VoucherTypeCount { get; set; }
        public int AccountTypeCount { get; set; }
        public long VoucherID { get; set; }
        public string VoucherNo { get; set; }
        public string CurrencySymbol { get; set; }
        public int VoucherCount { get; set; }
        public string Particulars { get; set; }
        public DateTime VoucherDate { get; set; }
        public int CurrencyId { get; set; }
        public double CurrencyConversionRate { get; set; }
        public double AmountInBaseCurrency { get; set; }
        public int AccountHeadID { get; set; }
        public bool Selected { get; set; }
        public string ErrorMessage { get; set; }
        public double VoucherReportAmount { get; set; }
        public int SLNo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DateType { get; set; }
        public int DateTypeInt { get; set; } 
        #endregion
                
        #region Derive Property
        public string VoucherDateInString
        {
            get { return this.VoucherDate.ToString("dd MMM yyyy"); }
        }
        public string StartDateInString
        {
            get { return this.StartDate.ToString("dd MMM yyyy"); }
        }
        public string EndDateInString
        {
            get { return this.EndDate.ToString("dd MMM yyyy"); }
        }
        public string VoucherReportAmountInString
        {
            get
            {
                return CurrencySymbol +" "+Global.MillionFormat(this.VoucherReportAmount);
            }
        }
        public string VoucherTypeNameSLNo
        {
            get
            {
                return this.VoucherTypeName.ToString() + "~" + this.SLNo.ToString();
            }
        }
        public string MonthNameOfStartDate // For Voucher Register
        {
            get
            {
                return this.StartDate.ToString("MMMM yy");
            }
        }
        public string MonthNameSLNo /// For View  Voucher For a VoucherType
        {
            get
            {
                return this.MonthNameOfStartDate.ToString() + "~" + this.SLNo.ToString();
            }
        }
        public AccountingSession RunningAccountingYear { get; set; }
        public string VoucherReportName { get; set; }
        public EnumNumberMethod NumberMethod { get; set; }
        public string CurrencyNameSymbol { get; set; }
        public int BaseCurrencyId { get; set; }
        public string BaseCurrencyNameSymbol { get; set; }
        public string AuthorizedByName { get; set; }
        public List<VoucherDetail> VoucherDetailLst { get; set; }
        public List<AccountingSession> AccountingSessions { get; set; }
        public int AccountingSessionID { get; set; }
        public List<VoucherReport> VoucherReportList { get; set; }
        public List<VoucherType> VoucherTypeList { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        private string sDebitAmountST = "";
        public string DebitAmountST // For Voucher Register
        {
            get
            {
                sDebitAmountST = Global.MillionFormat(this.DebitAmount);
                return sDebitAmountST;
            }
        }
        private string sCreditAmountST = "";
        public string CreditAmountST // For Voucher Register
        {
            get
            {
                sCreditAmountST = Global.MillionFormat(this.CreditAmount);
                return sCreditAmountST;
            }
        }
        #endregion
       
        #region Functions
        public static DataSet Gets_Report(DateTime dStartDate, DateTime dEndDate, int nVoucherTypeID, int nReportType, int nUserID)
        {
            return VoucherReport.Service.Gets_Report(dStartDate, dEndDate, nVoucherTypeID, nReportType, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVoucherReportService Service
        {
            get { return (IVoucherReportService)Services.Factory.CreateService(typeof(IVoucherReportService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class VoucherReportList : List<VoucherReport>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IVoucherReport interface
    public interface IVoucherReportService
    {
        DataSet Gets_Report(DateTime dStartDate, DateTime dEndDate, int nVoucherTypeID, int nReportType, int nUserID);
    }
    #endregion
}
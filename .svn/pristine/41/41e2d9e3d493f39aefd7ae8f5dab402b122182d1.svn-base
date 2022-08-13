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
    #region ExportBillEncashment
    [DataContract]
    public class ExportBillEncashment : BusinessObject
    {
        public ExportBillEncashment()
        {
            ExportBillEncashmentID = 0;
            ExportBillID = 0;
            AccountHeadID = 0;
            SubledgerID = 0;
            LoanInstallmentID = 0;
            CurrencyID = 0;
            CCRate = 0;
            Amount = 0;
            CurrencyName = "";
            Currency = "";
            ExportLCID = 0;
            ExportLCNo = "";
            EncashmentDate = DateTime.MinValue;
            LoanNo = "";
            LoanRefType = EnumLoanRefType.None;
            LoanLCID = 0;
            LoanStartDate = DateTime.MinValue;
            LoanCurencyID = 0;
            LoanID = 0;
            AccountCode = "";
            AccountHeadName = "";
            SubledgerCode = "";
            SubledgerName = "";
            LoanCurency = "";
            PrincipalAmount = 0;
            PrincipalAmountBC = 0;
            InstallmentPrincipalAmount = 0;           
            TotalInterestAmount = 0;
            ChargeAmount = 0;
            DiscountPaidAmount = 0;
            DiscountRcvAmount = 0;
            TotalPayableAmount = 0;            
            LoanLCNo = "";
            LoanInstallment = new LoanInstallment();
            ErrorMessage = "";
        }

        #region Properties
        public int ExportBillEncashmentID { get; set; }
        public int ExportBillID { get; set; }
        public int AccountHeadID { get; set; }
        public int SubledgerID { get; set; }
        public int LoanInstallmentID { get; set; }
        public int CurrencyID { get; set; }
        public double CCRate { get; set; }
        public double Amount { get; set; }
        public string CurrencyName { get; set; }
        public string Currency { get; set; }
        public int ExportLCID { get; set; }
        public string ExportLCNo { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public string SubledgerCode { get; set; }
        public string SubledgerName { get; set; }
        public DateTime EncashmentDate { get; set; }
        public string LoanNo { get; set; }
        public EnumLoanRefType LoanRefType { get; set; }
        public int LoanLCID { get; set; }
        public DateTime LoanStartDate { get; set; }
        public int LoanCurencyID { get; set; }
        public int LoanID { get; set; }
        public string LoanCurency { get; set; }
        public double PrincipalAmount { get; set; }
        public double PrincipalAmountBC { get; set; }
        public double InstallmentPrincipalAmount { get; set; }
        public double TotalInterestAmount { get; set; }
        public double ChargeAmount { get; set; }
        public double DiscountPaidAmount { get; set; }
        public double DiscountRcvAmount { get; set; }
        public double TotalPayableAmount { get; set; }
        public string LoanLCNo { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public LoanInstallment LoanInstallment { get; set; }
        public int SLNo { get; set; }
       
       
        public double AmountBC
        {
            get
            {
                return (this.Amount * this.CCRate);
            }
        }
        public string AmountSt
        {
            get
            {
                return this.Currency + " " + Global.MillionFormat(this.Amount);
            }
        }
        public string AmountBCSt
        {
            get
            {
                if (this.Amount > 0)
                {
                    return "TK " + Global.MillionFormatActualDigit(this.Amount * this.CCRate);
                }
                else
                {
                    return "";
                }
            }
        }
        public string PrincipalAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.PrincipalAmount);
            }
        }
        public string InstallmentPrincipalAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.InstallmentPrincipalAmount);
            }
        }        
       
        public string TotalInterestAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.TotalInterestAmount);
            }
        }
        public string ChargeAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.ChargeAmount);
            }
        }
        public string DiscountPaidAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.DiscountPaidAmount);
            }
        }
        public string DiscountRcvAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.DiscountRcvAmount);
            }
        }
        public string TotalPayableAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.TotalPayableAmount);
            }
        }
        
   
        #endregion

        #region Functions
        public ExportBillEncashment Get(int id, Int64 nUserID)
        {
            return ExportBillEncashment.Service.Get(id, nUserID);
        }

        public static List<ExportBillEncashment> Gets(int nExportLCID, Int64 nUserID)
        {
            return ExportBillEncashment.Service.Gets(nExportLCID, nUserID);
        }
        public ExportBillEncashment Save(Int64 nUserID)
        {
            return ExportBillEncashment.Service.Save(this, nUserID);
        }

        public string Delete(Int64 nUserID)
        {
            return ExportBillEncashment.Service.Delete(this, nUserID);
        }



        #endregion

        #region ServiceFactory


        internal static IExportBillEncashmentService Service
        {
            get { return (IExportBillEncashmentService)Services.Factory.CreateService(typeof(IExportBillEncashmentService)); }
        }
        #endregion
    }
    #endregion

    #region IExportBillEncashment interface
    public interface IExportBillEncashmentService
    {
        ExportBillEncashment Get(int id, Int64 nUserID);
        List<ExportBillEncashment> Gets(int nExportBillID, Int64 nUserID);
        List<ExportBillEncashment> Gets(string sSQL, Int64 nUserID);
        string Delete(ExportBillEncashment oExportBillEncashment, Int64 nUserID);
        ExportBillEncashment Save(ExportBillEncashment oExportBillEncashment, Int64 nUserID);

    }

    #endregion
}
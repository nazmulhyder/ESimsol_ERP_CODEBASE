using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region LoanInstallment
    public class LoanInstallment : BusinessObject
    {
        public LoanInstallment()
        {
            LoanInstallmentID = 0;
            LoanID = 0;
            InstallmentNo = "";
            InstallmentStartDate = DateTime.Now;
            InstallmentDate = DateTime.Now;
            PrincipalAmount = 0;
            LoanTransferType = EnumLoanTransfer.None;
            LoanTransferTypeInt = 0;
            TransferDate = DateTime.Now;
            TransferDays = 0;
            TransferInterestRate = 0;
            TransferInterestAmount = 0;
            SettlementDate = DateTime.Now;
            InterestDays = 0;
            InterestRate = 0;
            InterestAmount = 0;
            LiborRate = 0;
            LiborInterestAmount = 0;
            TotalInterestAmount = 0;
            ChargeAmount = 0;
            DiscountPaidAmount =0;
            DiscountRcvAmount = 0;
            TotalPayableAmount = 0;
            PaidAmount = 0;
            PaidAmountBC = 0;
            PrincipalDeduct = 0;
            PrincipalBalance = 0;
            Remarks = "";
            SettlementBy = 0;
            SettlementByName = "";
            FileNo = "";
            LoanNo = "";
            LoanRefType = EnumLoanRefType.None;
            LoanCRate = 0;
            LoanPrincipalAmount = 0;
            LoanRefID = 0;
            LoanRefNo = "";
            LoanType = EnumFinanceLoanType.None;
            LoanTypeInt = 0;
            ApproxSettlement = DateTime.Today;
            IssueDate = DateTime.Today;
            BankAccNo = "";
            LoanCurencyID = 0;
            LoanCurency = "";
            ErrorMessage = "";
            LoanchargeList = new List<LoanSettlement>();
            PaymentList = new List<LoanSettlement>();
            TrnasferTypes = new List<EnumObject>();
            BankAccounts = new List<BankAccount>();
            ExpenditureHeads = new List<ExpenditureHead>();
        }

        #region Property
        public int LoanInstallmentID { get; set; }
        public int LoanID { get; set; }
        public string InstallmentNo { get; set; }
        public DateTime InstallmentStartDate { get; set; }
        public DateTime InstallmentDate { get; set; }
        public double PrincipalAmount { get; set; }
        public EnumLoanTransfer LoanTransferType { get; set; }
        public int LoanTransferTypeInt { get; set; }
        public DateTime TransferDate { get; set; }
        public int TransferDays { get; set; }
        public double TransferInterestRate { get; set; }
        public double TransferInterestAmount { get; set; }
        public DateTime SettlementDate { get; set; }
        public int InterestDays { get; set; }
        public double InterestRate { get; set; }
        public double InterestAmount { get; set; }
        public double LiborRate { get; set; }
        public double LiborInterestAmount { get; set; }
        public double TotalInterestAmount { get; set; }
        public double ChargeAmount { get; set; }
        public double DiscountPaidAmount { get; set; }
        public double DiscountRcvAmount { get; set; }
        public double TotalPayableAmount { get; set; }
        public double PaidAmount { get; set; }
        public double PaidAmountBC { get; set; }
        public double PrincipalDeduct { get; set; }
        public double PrincipalBalance { get; set; }
        public string Remarks { get; set; }
        public int SettlementBy { get; set; }  
        public string SettlementByName { get; set; }
        public string FileNo { get; set; }
        public string LoanNo { get; set; }
        public EnumLoanRefType LoanRefType { get; set; }
        public int LoanRefID { get; set; }
        public string LoanRefNo { get; set; }
        public EnumFinanceLoanType LoanType { get; set; }
        public int LoanTypeInt { get; set; }
        public double LoanCRate { get; set; }
        public double LoanPrincipalAmount { get; set; }
        public DateTime ApproxSettlement { get; set; }
        public DateTime IssueDate { get; set; }
        public string BankAccNo { get; set; }
        public int LoanCurencyID { get; set; }
        public string LoanCurency { get; set; }
        public string BaseCSymbol { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<LoanSettlement> LoanchargeList { get; set; }
        public List<LoanSettlement> PaymentList { get; set; }
        public List<EnumObject> TrnasferTypes { get; set; }
        public List<BankAccount> BankAccounts { get; set; }
        public List<ExpenditureHead> ExpenditureHeads { get; set; }
        public string InstallmentDateInString
        {
            get
            {
                return InstallmentDate.ToString("dd MMM yyyy");
            }
        }
        public string InstallmentStartDateInString
        {
            get
            {
                return InstallmentStartDate.ToString("dd MMM yyyy");
            }
        }

        public string LoanTransferTypeSt
        {
            get
            {
                if (this.LoanTransferType == EnumLoanTransfer.None)
                {
                    return "";
                }
                else
                {
                    return EnumObject.jGet(this.LoanTransferType);
                }
            }
        }
        public string TransferDateInString
        {
            get
            {
                if (this.TransferDate == DateTime.MinValue || this.LoanTransferType == EnumLoanTransfer.None)
                {
                    return "";
                }
                else
                {
                    return TransferDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string SettlementDateInString
        {
            get
            {
                return SettlementDate.ToString("dd MMM yyyy");
            }
        }
        public string LoanTypeSt
        {
            get
            {
                return EnumObject.jGet(this.LoanType);
            }
        }
        public string LoanRefTypeSt
        {
            get
            {
                return EnumObject.jGet(this.LoanRefType);
            }
        }
        public string IssueDateSt
        {
            get
            {
                return IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string IssueDateShortSt
        {
            get
            {
                return IssueDate.ToString("dd MMM yy");
            }
        }
        public string ApproxSettlementSt
        {
            get
            {
                return ApproxSettlement.ToString("dd MMM yyyy");
            }
        }
        public string ApproxSettlementShortSt
        {
            get
            {
                return ApproxSettlement.ToString("dd MMM yy");
            }
        }
        public string PrincipalAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.PrincipalAmount);
            }
        }
        public string LoanPrincipalAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.LoanPrincipalAmount);
            }
        }
        public string PrincipalAmountBCSt
        {
            get
            {
                return this.BaseCSymbol + " " + Global.MillionFormat(this.PrincipalAmount * this.LoanCRate);
            }
        }
        public string TransferInterestRateSt
        {
            get
            {
                if (this.TransferInterestRate == 0.00)
                {
                    return "";
                }
                else
                {
                    return this.LoanCurency + " " + this.TransferInterestRate.ToString("0.00##");
                }
            }
        }
        public string InterestRateSt
        {
            get
            {
                if (this.InterestRate == 0.00)
                {
                    return "";
                }
                else
                {
                    return this.LoanCurency + " " + this.InterestRate.ToString("0.00##");
                }
            }
        }
        public string LiborRateSt
        {
            get
            {
                if (this.LiborRate == 0.00)
                {
                    return "";
                }
                else
                {
                    return this.LoanCurency + " " + this.LiborRate.ToString("0.00##");
                }
            }
        }
        public string TransferDaysSt
        {
            get
            {
                if (this.TransferDays == 0)
                {
                    return "";
                }
                else
                {
                    return this.TransferDays.ToString("00")+" Days";
                }
            }
        }
        public string InterestDaysSt
        {
            get
            {
                if (this.InterestDays == 0)
                {
                    return "";
                }
                else
                {
                    return this.InterestDays.ToString("00") + " Days";
                }
            }
        }
        public string TransferInterestAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.TransferInterestAmount);
            }
        }
        public string InterestAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.InterestAmount);
            }
        }
        public string LiborInterestAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.LiborInterestAmount);
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
        public string PaidAmountSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.PaidAmount);
            }
        }
        public string PaidAmountBCSt
        {
            get
            {
                return this.BaseCSymbol + " " + Global.MillionFormat(this.PaidAmountBC);
            }
        }
        public string PrincipalBalanceSt
        {
            get
            {
                return this.LoanCurency + " " + Global.MillionFormat(this.PrincipalBalance);
            }
        }
        #endregion

        #region Functions
        public static List<LoanInstallment> Gets(int LoanID, long nUserID)
        {
            return LoanInstallment.Service.Gets(LoanID, nUserID);
        }
        public static List<LoanInstallment> Gets(string sSQL, long nUserID)
        {
            return LoanInstallment.Service.Gets(sSQL, nUserID);
        }
        public LoanInstallment Get(int id, long nUserID)
        {
            return LoanInstallment.Service.Get(id, nUserID);
        }
        public LoanInstallment Save(long nUserID)
        {
            return LoanInstallment.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return LoanInstallment.Service.Delete(id, nUserID);
        }
        public LoanInstallment Approved(long nUserID)
        {
            return LoanInstallment.Service.Approved(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILoanInstallmentService Service
        {
            get { return (ILoanInstallmentService)Services.Factory.CreateService(typeof(ILoanInstallmentService)); }
        }
        #endregion
    }
    #endregion

    #region ILoanInstallment interface
    public interface ILoanInstallmentService
    {
        LoanInstallment Get(int id, Int64 nUserID);
        List<LoanInstallment> Gets(int LoanID, Int64 nUserID);
        List<LoanInstallment> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        LoanInstallment Save(LoanInstallment oLoanInstallment, Int64 nUserID);
        LoanInstallment Approved(LoanInstallment oLoanInstallment, Int64 nUserID);
    }
    #endregion
}

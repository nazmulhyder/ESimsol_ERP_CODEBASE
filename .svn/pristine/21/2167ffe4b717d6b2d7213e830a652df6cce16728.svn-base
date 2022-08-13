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
	#region Loan  
	public class Loan : BusinessObject
	{	
		public Loan()
		{
			LoanID = 0; 
			FileNo = "";
            FullFileNo = "";
			BUID = 0;
            CRate = 0;
            LoanType = EnumFinanceLoanType.None;
            LoanTypeInt = 0;
			LoanNo = "";
            LoanStatus = EnumLoanStatus.Initialize;
            LoanStatusInt = (int)EnumLoanStatus.Initialize; 
            LoanRefType =EnumLoanRefType.None;
            LoanRefTypeInt = 0;
			LoanRefID = 0; 
			IssueDate = DateTime.Now;
            LoanStartDate = DateTime.Now;
			PrincipalAmountBC = 0; 
			LoanCurencyID = 0; 
			LoanAmount = 0; 
			InterestRate = 0; 
			LiborRate = 0;
            CompoundType = EnumLoanCompoundType.None;
            CompoundTypeInt = 0;
			ApproxSettlement = DateTime.Now; 
			RcvBankAccountID = 0; 
			RcvDate = DateTime.Now; 
			ApprovedBy = 0; 
			LoanRemarks = "";
            ProcessFeePercent = 0;
            ProcessFeeAmount = 0;
			TransferDate = DateTime.Now; 
			SettlementDate = DateTime.Now; 
			InterestDays = 0; 
			InterestAmount = 0; 
			LiborInterestAmount = 0; 
			TotalInterestAmount = 0; 
			TotalChargeAmount = 0; 
			SettlementAmount = 0; 
			PaidAmount = 0; 
			SettlementBy = 0; 
			SettlementRemarks = ""; 
			NoOfInstallment = 0;
            InstallmentCycle = EnumCycleType.None;
            InstallmentCycleInt = 0;
			InstallmentStartDate = DateTime.Now; 
			InstallmentAmount = 0; 
            ApprovedDate = DateTime.Now;
            StlmtStartDate = DateTime.Now;
            PrincipalAmount = 0;
            ReceivedBy = 0;
            ReceivedByName = "";
            BankName = "";
            BankAccountName = "";
			ErrorMessage = "";
            LoanchargeList =  new List<LoanSettlement>();
            PaymentList = new List<LoanSettlement>();
            LoanInstallments = new List<LoanInstallment>();
            LoanInterests = new List<LoanInterest>();
            SearchingData = "";
		}

		#region Property
		public int LoanID { get; set; }
		public string FileNo { get; set; }
		public int BUID { get; set; }
        public int CRate { get; set; }
        public EnumFinanceLoanType LoanType { get; set; }
        public int LoanTypeInt { get; set; }
        public int LoanRefTypeInt { get; set; }
		public string LoanNo { get; set; }
        public EnumLoanStatus LoanStatus { get; set; }
        public int LoanStatusInt { get; set; }
		public EnumLoanRefType LoanRefType { get; set; }
		public int LoanRefID { get; set; }
		public DateTime IssueDate { get; set; }
        public DateTime LoanStartDate { get; set; }
		public double PrincipalAmountBC { get; set; }
		public int LoanCurencyID { get; set; }
		public double LoanAmount { get; set; }
		public double InterestRate { get; set; }
		public double LiborRate { get; set; }
        public EnumLoanCompoundType CompoundType { get; set; }
        public int CompoundTypeInt { get; set; }
		public DateTime ApproxSettlement { get; set; }
		public int RcvBankAccountID { get; set; }
		public DateTime RcvDate { get; set; }
		public int ApprovedBy { get; set; }
       public DateTime ApprovedDate { get; set; }
       public int ReceivedBy { get; set; }
       public double ProcessFeePercent { get; set; }
       public double ProcessFeeAmount { get; set; }
       public string ReceivedByName { get; set; }
       public string BankAccountName { get; set; }
		public string LoanRemarks { get; set; }
        public string BankName { get; set; }    
		public DateTime TransferDate { get; set; }		
		public DateTime SettlementDate { get; set; }
		public int InterestDays { get; set; }
		public double InterestAmount { get; set; }
		public double LiborInterestAmount { get; set; }
		public double TotalInterestAmount { get; set; }
		public double TotalChargeAmount { get; set; }
		public double SettlementAmount { get; set; }
		public double PaidAmount { get; set; }
		public int SettlementBy { get; set; }
		public string SettlementRemarks { get; set; }
		public int NoOfInstallment { get; set; }
        public EnumCycleType InstallmentCycle { get; set; }
        public int InstallmentCycleInt { get; set; }
		public DateTime InstallmentStartDate { get; set; }
        public DateTime StlmtStartDate { get; set; }
        public double PrincipalAmount { get; set; }
		public double InstallmentAmount { get; set; }
        public double TotalPaidAmount { get; set; }
        public double TotalIntarastAmount { get; set; }
        public double TotalIntarastDays { get; set; }
        public double TotalCharge { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string FullFileNo { get; set; }
        public List<LoanSettlement> LoanchargeList { get; set; }
        public List<LoanSettlement> PaymentList { get; set; }
        public List<LoanInstallment> LoanInstallments { get; set; }
        public List<LoanInterest> LoanInterests { get; set; }
        public String SearchingData { get; set; }
       
        public string TotalInterestAmountSt
        {
            get
            {
                if (this.TotalInterestAmount > 0)
                {
                    return this.CurrencySymbol + " " + Global.MillionFormat(this.TotalInterestAmount);
                }
                else
                {
                    return "";
                }
            }
        }
        public string TotalChargeAmountSt
        {
            get
            {
                if (this.TotalChargeAmount > 0)
                {
                    return this.CurrencySymbol + " " + Global.MillionFormat(this.TotalChargeAmount);
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
                if (this.PrincipalAmount > 0)
                {
                    return this.CurrencySymbol + " " + Global.MillionFormat(this.PrincipalAmount);
                }
                else
                {
                    return "";
                }
            }
        }

        public string TotalPaybleAmountSt
        {
            get
            {
                   return this.CurrencySymbol + " " + Global.MillionFormat(this.PrincipalAmount + this.TotalChargeAmount + this.TotalIntarastAmount);
            }
        }
        public string SettlementAmountSt
        {
            get
            {
                if (this.SettlementAmount > 0)
                {
                    return this.CurrencySymbol + " " + Global.MillionFormat(this.SettlementAmount);
                }
                else
                {
                    return "";
                }
            }
        }
        public string LoanAmountSt
        {
            get
            {
                if (this.LoanAmount > 0)
                {
                    return this.CurrencySymbol + " " + Global.MillionFormat(this.LoanAmount);
                }
                else
                {
                    return "";
                }
            }
        }
        public string CompoundTypeSt
        {
            get
            {
                return EnumObject.jGet(this.CompoundType);
            }
        }
        public string LoanTypeSt
        {
            get
            {
                return EnumObject.jGet(this.LoanType);
            }
        }
        public double LoanAmountInBC
        {
          get
            {
                return (this.LoanAmount * this.CRate);
            }
        }
        public string PaidAmountSt
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.TotalPaidAmount);
            }
        }
        public string LoanStatusSt
        {
            get
            {
                return EnumObject.jGet(this.LoanStatus);
            }
        }
        public string InstallmentCycleSt
        {
            get
            {
                return EnumObject.jGet(this.InstallmentCycle);
            }
        }
        public string LoanRefTypeSt
        {
            get
            {
                return EnumObject.jGet(this.LoanRefType); 
            }
        }
		public string IssueDateInString 
		{
			get
			{
				return IssueDate.ToString("dd MMM yyyy") ; 
			}
		}
        public string LoanStartDateInString
        {
            get
            {
                return LoanStartDate.ToString("dd MMM yyyy");
            }
        }
        public string StlmtStartDateInString
        {
            get
            {
                return StlmtStartDate.ToString("dd MMM yyyy");
            }
        }    
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public string LoanRefNo { get; set; }
        public string CurrencySymbol { get; set; }
        public string BankAccNo { get; set; }
        public string BankShortName { get; set; }
        public string ApprovedByName { get; set; }
        public string SettlementByName { get; set; }
		public string ApproxSettlementInString 
		{
			get
			{
				return ApproxSettlement.ToString("dd MMM yyyy") ; 
			}
		}
		public string RcvDateInString 
		{
			get
			{
				return RcvDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string InstallmentStartDateInString 
		{
			get
			{
				return InstallmentStartDate.ToString("dd MMM yyyy") ; 
			}
		}
		#endregion 

		#region Functions 
		public static List<Loan> Gets(int buid, long nUserID)
		{
            return Loan.Service.Gets(buid,nUserID);
		}
        public static List<Loan> Gets(EnumLoanRefType eLoanRefType, int nRefID, long nUserID)
        {
            return Loan.Service.Gets(eLoanRefType, nRefID, nUserID);
        }
		public static List<Loan> Gets(string sSQL, long nUserID)
		{
			return Loan.Service.Gets(sSQL,nUserID);
		}
		public Loan Get(int id, long nUserID)
		{
			return Loan.Service.Get(id,nUserID);
		}
      	public Loan Save(long nUserID)
		{
			return Loan.Service.Save(this,nUserID);
		}
        public Loan ApproveOrReceived(bool bIsApprove, long nUserID)
        {
            return Loan.Service.ApproveOrReceived(this, bIsApprove, nUserID);
        }
        public Loan UpdateStmlStartDate(long nUserID)
        {
            return Loan.Service.UpdateStmlStartDate(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return Loan.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ILoanService Service
		{
			get { return (ILoanService)Services.Factory.CreateService(typeof(ILoanService)); }
		}
		#endregion
	}
	#endregion

	#region ILoan interface
	public interface ILoanService 
	{
		Loan Get(int id, Int64 nUserID);        
		List<Loan> Gets(int buid, Int64 nUserID);
        List<Loan> Gets(EnumLoanRefType eLoanRefType, int nRefID, Int64 nUserID);
		List<Loan> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		Loan Save(Loan oLoan, Int64 nUserID);
        Loan UpdateStmlStartDate(Loan oLoan, Int64 nUserID);
        Loan ApproveOrReceived(Loan oLoan, bool bIsApprove, Int64 nUserID);
        
     
	}
	#endregion
}

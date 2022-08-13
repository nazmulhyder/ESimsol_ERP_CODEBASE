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
	#region LoanSettlement  
	public class LoanSettlement : BusinessObject
	{	
		public LoanSettlement()
		{
			LoanSettlementID = 0;
            LoanInstallmentID = 0;
            LoanID = 0; 
			BankAccountID = 0; 
			ExpenseHeadID = 0; 
			CurrencyID = 0; 
			AmountBC = 0; 
			CRate = 0; 
			Amount = 0; 
			Remarks = "";
            AccountNo = "";
            AccountName = "";
            CurrencySymbol = "";
			ErrorMessage = "";
		}

		#region Property
		public int LoanSettlementID { get; set; }
        public int LoanInstallmentID { get; set; }
		public int LoanID { get; set; }
		public int BankAccountID { get; set; }
		public int ExpenseHeadID { get; set; }
		public int CurrencyID { get; set; }
		public double AmountBC { get; set; }
		public double CRate { get; set; }
		public double Amount { get; set; }
		public string Remarks { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string CurrencySymbol { get; set; }
		#endregion 

		#region Functions 
		public static List<LoanSettlement> Gets(long nUserID)
		{
			return LoanSettlement.Service.Gets(nUserID);
		}
		public static List<LoanSettlement> Gets(string sSQL, long nUserID)
		{
			return LoanSettlement.Service.Gets(sSQL,nUserID);
		}
		public LoanSettlement Get(int id, long nUserID)
		{
			return LoanSettlement.Service.Get(id,nUserID);
		}
	
		public  string  Delete(int id, long nUserID)
		{
			return LoanSettlement.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ILoanSettlementService Service
		{
			get { return (ILoanSettlementService)Services.Factory.CreateService(typeof(ILoanSettlementService)); }
		}
		#endregion
	}
	#endregion

	#region ILoanSettlement interface
	public interface ILoanSettlementService 
	{
		LoanSettlement Get(int id, Int64 nUserID); 
		List<LoanSettlement> Gets(Int64 nUserID);
		List<LoanSettlement> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		
	}
	#endregion
}

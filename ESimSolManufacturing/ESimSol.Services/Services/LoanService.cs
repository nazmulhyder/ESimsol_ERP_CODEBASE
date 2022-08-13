using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
	public class LoanService : MarshalByRefObject, ILoanService
	{
		#region Private functions and declaration

		private Loan MapObject(NullHandler oReader)
		{
			Loan oLoan = new Loan();
			oLoan.LoanID = oReader.GetInt32("LoanID");
			oLoan.FileNo = oReader.GetString("FileNo");
            oLoan.FullFileNo = oReader.GetString("FullFileNo");
			oLoan.BUID = oReader.GetInt32("BUID");
            oLoan.CRate = oReader.GetInt32("CRate");
            oLoan.LoanType = (EnumFinanceLoanType)oReader.GetInt32("LoanType");
            oLoan.LoanTypeInt = oReader.GetInt32("LoanType");
			oLoan.LoanNo = oReader.GetString("LoanNo");
			oLoan.LoanStatus = (EnumLoanStatus) oReader.GetInt16("LoanStatus");
            oLoan.LoanStatusInt =oReader.GetInt16("LoanStatus");
            
			oLoan.LoanRefType = (EnumLoanRefType) oReader.GetInt16("LoanRefType");
            oLoan.LoanRefTypeInt = oReader.GetInt16("LoanRefType");

			oLoan.LoanRefID = oReader.GetInt32("LoanRefID");
			oLoan.IssueDate = oReader.GetDateTime("IssueDate");
            oLoan.StlmtStartDate = oReader.GetDateTime("StlmtStartDate");
            oLoan.LoanStartDate = oReader.GetDateTime("LoanStartDate");
            oLoan.PrincipalAmount = oReader.GetDouble("PrincipalAmount");
            oLoan.PrincipalAmountBC = oReader.GetDouble("PrincipalAmountBC");
			oLoan.LoanCurencyID = oReader.GetInt32("LoanCurencyID");
			oLoan.LoanAmount = oReader.GetDouble("LoanAmount");
			oLoan.InterestRate = oReader.GetDouble("InterestRate");
			oLoan.LiborRate = oReader.GetDouble("LiborRate");
            oLoan.CompoundType = (EnumLoanCompoundType)oReader.GetInt32("CompoundType");
            oLoan.CompoundTypeInt = oReader.GetInt32("CompoundType");
			oLoan.ApproxSettlement = oReader.GetDateTime("ApproxSettlement");
			oLoan.RcvBankAccountID = oReader.GetInt32("RcvBankAccountID");
			oLoan.RcvDate = oReader.GetDateTime("RcvDate");
			oLoan.ApprovedBy = oReader.GetInt32("ApprovedBy");
			oLoan.LoanRemarks = oReader.GetString("LoanRemarks");
          
			oLoan.TransferDate = oReader.GetDateTime("TransferDate");
			
			oLoan.SettlementDate = oReader.GetDateTime("SettlementDate");
			oLoan.InterestDays = oReader.GetInt32("InterestDays");
			oLoan.InterestAmount = oReader.GetDouble("InterestAmount");
			oLoan.LiborInterestAmount = oReader.GetDouble("LiborInterestAmount");
			oLoan.TotalInterestAmount = oReader.GetDouble("TotalInterestAmount");
			oLoan.TotalChargeAmount = oReader.GetDouble("TotalChargeAmount");
			oLoan.SettlementAmount = oReader.GetDouble("SettlementAmount");
            oLoan.BankAccountName = oReader.GetString("BankAccountName");

            oLoan.TotalPaidAmount = oReader.GetDouble("TotalPaidAmount");
            oLoan.TotalIntarastAmount = oReader.GetDouble("TotalIntarastAmount");
            oLoan.TotalIntarastDays = oReader.GetDouble("TotalIntarastDays");
            oLoan.TotalCharge = oReader.GetDouble("TotalCharge");

			oLoan.SettlementBy = oReader.GetInt32("SettlementBy");
            oLoan.SettlementRemarks = oReader.GetString("SettlementRemarks");
            oLoan.BankName = oReader.GetString("BankName");
			oLoan.NoOfInstallment = oReader.GetInt32("NoOfInstallment");
            oLoan.InstallmentCycle = (EnumCycleType)oReader.GetInt32("InstallmentCycle");
            oLoan.InstallmentCycleInt = oReader.GetInt32("InstallmentCycle");
			oLoan.InstallmentStartDate = oReader.GetDateTime("InstallmentStartDate");
			oLoan.InstallmentAmount = oReader.GetDouble("InstallmentAmount");

            oLoan.BUCode = oReader.GetString("BUCode");
            oLoan.BUName = oReader.GetString("BUName");
            oLoan.BUShortName = oReader.GetString("BUShortName");
            oLoan.LoanRefNo = oReader.GetString("LoanRefNo");
            oLoan.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oLoan.BankAccNo = oReader.GetString("BankAccNo");
            oLoan.BankShortName = oReader.GetString("BankShortName");
            oLoan.ApprovedByName = oReader.GetString("ApprovedByName");
            oLoan.SettlementByName = oReader.GetString("SettlementByName");

            oLoan.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oLoan.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oLoan.ReceivedByName = oReader.GetString("ReceivedByName");
            oLoan.ProcessFeePercent = oReader.GetDouble("ProcessFeePercent");
            oLoan.ProcessFeeAmount = oReader.GetDouble("ProcessFeeAmount");
			return oLoan;
		}

		private Loan CreateObject(NullHandler oReader)
		{
			Loan oLoan = new Loan();
			oLoan = MapObject(oReader);
			return oLoan;
		}

		private List<Loan> CreateObjects(IDataReader oReader)
		{
			List<Loan> oLoan = new List<Loan>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				Loan oItem = CreateObject(oHandler);
				oLoan.Add(oItem);
			}
			return oLoan;
		}

		#endregion

		#region Interface implementation
			public Loan Save(Loan oLoan, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{

					tc = TransactionContext.Begin(true);                    
                    IDataReader reader;
					if (oLoan.LoanID <= 0)
					{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Loan, EnumRoleOperationType.Add);
						reader = LoanDA.InsertUpdate(tc, oLoan, EnumDBOperation.Insert, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Loan, EnumRoleOperationType.Edit);
						reader = LoanDA.InsertUpdate(tc, oLoan, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oLoan = new Loan();
						oLoan = CreateObject(oReader);
					}
					reader.Close();                                   
                    tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					{
						tc.HandleError();
						oLoan = new Loan();
						oLoan.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oLoan;
			}

            public Loan ApproveOrReceived(Loan oLoan, bool bIsApproved, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
                    if (bIsApproved)
					{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Loan, EnumRoleOperationType.Approved);
						reader = LoanDA.InsertUpdate(tc, oLoan, EnumDBOperation.Approval, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Loan, EnumRoleOperationType.Received);
						reader = LoanDA.InsertUpdate(tc, oLoan, EnumDBOperation.Receive, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oLoan = new Loan();
						oLoan = CreateObject(oReader);
					}
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					{
						tc.HandleError();
						oLoan = new Loan();
						oLoan.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oLoan;
			}

            public Loan UpdateStmlStartDate(Loan oLoan, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{

					tc = TransactionContext.Begin(true);
                    LoanDA.UpdateStmlStartDate(tc, oLoan, nUserID);
                    IDataReader reader = LoanDA.Get(tc, oLoan.LoanID);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oLoan = new Loan();
						oLoan = CreateObject(oReader);
					}
					reader.Close();                                   
                    tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					{
						tc.HandleError();
						oLoan = new Loan();
						oLoan.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oLoan;
			}
        

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					Loan oLoan = new Loan();
					oLoan.LoanID = id;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Loan, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "Loan", id);
					LoanDA.Delete(tc, oLoan, EnumDBOperation.Delete, nUserId);
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exceptionif (tc != null)
					tc.HandleError();
					return e.Message.Split('!')[0];
					#endregion
				}
				return Global.DeleteMessage;
			}

			public Loan Get(int id, Int64 nUserId)
			{
				Loan oLoan = new Loan();
                LoanSettlementService oLoanSettlementService = new LoanSettlementService();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = LoanDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					    oLoan = CreateObject(oReader);
					}
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get Loan", e);
					#endregion
				}
				return oLoan;
			}

			public List<Loan> Gets(int buid, Int64 nUserID)
			{
				List<Loan> oLoans = new List<Loan>();
                LoanSettlementService oLoanSettlementService = new LoanSettlementService();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = LoanDA.Gets(buid,tc);
					oLoans = CreateObjects(reader);                   
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					Loan oLoan = new Loan();
					oLoan.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oLoans;
			}

            public List<Loan> Gets(EnumLoanRefType eLoanRefType, int nRefID, Int64 nUserID)
            {
                List<Loan> oLoans = new List<Loan>();                
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = LoanDA.Gets(eLoanRefType, nRefID, tc);
                    oLoans = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    Loan oLoan = new Loan();
                    oLoan.ErrorMessage = e.Message.Split('!')[0];
                    oLoans = new List<Loan>();
                    oLoans.Add(oLoan);
                    #endregion
                }
                return oLoans;
            }
			public List<Loan> Gets (string sSQL, Int64 nUserID)
			{
				List<Loan> oLoans = new List<Loan>();
                LoanSettlementService oLoanSettlementService = new LoanSettlementService();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = LoanDA.Gets(tc, sSQL);
					oLoans = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get Loan", e);
					#endregion
				}
				return oLoans;
			}

		#endregion
	}

}

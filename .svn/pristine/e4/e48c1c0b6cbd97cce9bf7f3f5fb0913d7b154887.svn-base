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
	public class LoanSettlementService : MarshalByRefObject, ILoanSettlementService
	{
		#region Private functions and declaration

		private LoanSettlement MapObject(NullHandler oReader)
		{
			LoanSettlement oLoanSettlement = new LoanSettlement();
			oLoanSettlement.LoanSettlementID = oReader.GetInt32("LoanSettlementID");
            oLoanSettlement.LoanInstallmentID = oReader.GetInt32("LoanInstallmentID");
            oLoanSettlement.LoanID = oReader.GetInt32("LoanID");
			oLoanSettlement.BankAccountID = oReader.GetInt32("BankAccountID");
			oLoanSettlement.ExpenseHeadID = oReader.GetInt32("ExpenseHeadID");
			oLoanSettlement.CurrencyID = oReader.GetInt32("CurrencyID");
            oLoanSettlement.AmountBC = oReader.GetDouble("AmountBC");
			oLoanSettlement.CRate = oReader.GetDouble("CRate");
			oLoanSettlement.Amount = oReader.GetDouble("Amount");
			oLoanSettlement.Remarks = oReader.GetString("Remarks");
            oLoanSettlement.AccountNo = oReader.GetString("AccountNo");
            oLoanSettlement.AccountName = oReader.GetString("AccountName");
            oLoanSettlement.CurrencySymbol = oReader.GetString("CurrencySymbol");

			return oLoanSettlement;
		}

		private LoanSettlement CreateObject(NullHandler oReader)
		{
			LoanSettlement oLoanSettlement = new LoanSettlement();
			oLoanSettlement = MapObject(oReader);
			return oLoanSettlement;
		}

		private List<LoanSettlement> CreateObjects(IDataReader oReader)
		{
			List<LoanSettlement> oLoanSettlement = new List<LoanSettlement>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				LoanSettlement oItem = CreateObject(oHandler);
				oLoanSettlement.Add(oItem);
			}
			return oLoanSettlement;
		}

		#endregion

		#region Interface implementation
		
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					LoanSettlement oLoanSettlement = new LoanSettlement();
					oLoanSettlement.LoanSettlementID = id;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.LoanInstallment, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "LoanSettlement", id);
					LoanSettlementDA.Delete(tc, oLoanSettlement, EnumDBOperation.Delete, nUserId,"");
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exceptionif (tc != null)
					tc.HandleError();
					return e.Message.Split('!')[0];
					#endregion
				}
				return "Data delete successfully";
			}

			public LoanSettlement Get(int id, Int64 nUserId)
			{
				LoanSettlement oLoanSettlement = new LoanSettlement();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = LoanSettlementDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oLoanSettlement = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get LoanSettlement", e);
					#endregion
				}
				return oLoanSettlement;
			}

			public List<LoanSettlement> Gets(Int64 nUserID)
			{
				List<LoanSettlement> oLoanSettlements = new List<LoanSettlement>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = LoanSettlementDA.Gets(tc);
					oLoanSettlements = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					LoanSettlement oLoanSettlement = new LoanSettlement();
					oLoanSettlement.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oLoanSettlements;
			}

			public List<LoanSettlement> Gets (string sSQL, Int64 nUserID)
			{
				List<LoanSettlement> oLoanSettlements = new List<LoanSettlement>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = LoanSettlementDA.Gets(tc, sSQL);
					oLoanSettlements = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get LoanSettlement", e);
					#endregion
				}
				return oLoanSettlements;
			}

		#endregion
	}

}

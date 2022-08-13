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
	public class LoanInterestService : MarshalByRefObject, ILoanInterestService
	{
		#region Private functions and declaration

        private LoanInterest MapObject(NullHandler oReader)
        {
            LoanInterest oLoanInterest = new LoanInterest();
            oLoanInterest.LoanInterestID = oReader.GetInt32("LoanInterestID");
            oLoanInterest.LoanID = oReader.GetInt32("LoanID");
            oLoanInterest.LoanAmount = oReader.GetDouble("LoanAmount");
            oLoanInterest.InterestEffectDate = oReader.GetDateTime("InterestEffectDate");
            oLoanInterest.InterestStartDate = oReader.GetDateTime("InterestStartDate");
            oLoanInterest.InterestEndDate = oReader.GetDateTime("InterestEndDate");
            oLoanInterest.InterestType = (EnumInterestType)oReader.GetInt32("InterestType");
            oLoanInterest.InterestDays = oReader.GetInt32("InterestDays");
            oLoanInterest.CurrencyID = oReader.GetInt32("CurrencyID");
            oLoanInterest.InterestRate = oReader.GetDouble("InterestRate");
            oLoanInterest.InterestAmount = oReader.GetDouble("InterestAmount");
            oLoanInterest.CRate = oReader.GetDouble("CRate");
            oLoanInterest.InterestAmountBC = oReader.GetDouble("InterestAmountBC");
            oLoanInterest.EntryUserID = oReader.GetInt32("EntryUserID");
            oLoanInterest.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oLoanInterest.EntryUserName = oReader.GetString("EntryUserName");
            return oLoanInterest;
        }

		private LoanInterest CreateObject(NullHandler oReader)
		{
			LoanInterest oLoanInterest = new LoanInterest();
			oLoanInterest = MapObject(oReader);
			return oLoanInterest;
		}

		private List<LoanInterest> CreateObjects(IDataReader oReader)
		{
			List<LoanInterest> oLoanInterest = new List<LoanInterest>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				LoanInterest oItem = CreateObject(oHandler);
				oLoanInterest.Add(oItem);
			}
			return oLoanInterest;
		}

		#endregion

		#region Interface implementation
		
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					LoanInterest oLoanInterest = new LoanInterest();
					oLoanInterest.LoanInterestID = id;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.LoanInstallment, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "LoanInterest", id);
					LoanInterestDA.Delete(tc, oLoanInterest, EnumDBOperation.Delete, nUserId);
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

			public LoanInterest Get(int id, Int64 nUserId)
			{
				LoanInterest oLoanInterest = new LoanInterest();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = LoanInterestDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oLoanInterest = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get LoanInterest", e);
					#endregion
				}
				return oLoanInterest;
			}

			public List<LoanInterest> Gets(Int64 nUserID)
			{
				List<LoanInterest> oLoanInterests = new List<LoanInterest>();
				TransactionContext tc = null;
				try
				{ 
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = LoanInterestDA.Gets(tc);
					oLoanInterests = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					LoanInterest oLoanInterest = new LoanInterest();
					oLoanInterest.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oLoanInterests;
			}
            public List<LoanInterest> GetsByLoan(int nLoanID, Int64 nUserID)
			{
				List<LoanInterest> oLoanInterests = new List<LoanInterest>();
				TransactionContext tc = null;
				try
				{ 
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = LoanInterestDA.GetsByLoan(tc, nLoanID);
					oLoanInterests = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					LoanInterest oLoanInterest = new LoanInterest();
					oLoanInterest.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oLoanInterests;
			}
                
			public List<LoanInterest> Gets (string sSQL, Int64 nUserID)
			{
				List<LoanInterest> oLoanInterests = new List<LoanInterest>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = LoanInterestDA.Gets(tc, sSQL);
					oLoanInterests = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get LoanInterest", e);
					#endregion
				}
				return oLoanInterests;
			}

		#endregion
	}

}

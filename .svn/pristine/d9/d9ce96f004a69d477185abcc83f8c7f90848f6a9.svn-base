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
	public class CashFlowSetupService : MarshalByRefObject, ICashFlowSetupService
	{
		#region Private functions and declaration

		private CashFlowSetup MapObject(NullHandler oReader)
		{
			CashFlowSetup oCashFlowSetup = new CashFlowSetup();
			oCashFlowSetup.CashFlowSetupID = oReader.GetInt32("CashFlowSetupID");
			oCashFlowSetup.CFTransactionCategory = (EnumCFTransactionCategory) oReader.GetInt32("CFTransactionCategory");
            oCashFlowSetup.CFTransactionCategoryInInt = oReader.GetInt32("CFTransactionCategory");
			oCashFlowSetup.CFTransactionGroup = (EnumCFTransactionGroup)oReader.GetInt32("CFTransactionGroup");
            oCashFlowSetup.CFTransactionGroupInInt = oReader.GetInt32("CFTransactionGroup");
            oCashFlowSetup.CFDataType = (EnumCFDataType)oReader.GetInt32("CFDataType");
            oCashFlowSetup.CFDataTypeInInt = oReader.GetInt32("CFDataType");
			oCashFlowSetup.SubGroupID = oReader.GetInt32("SubGroupID");
			oCashFlowSetup.DisplayCaption = oReader.GetString("DisplayCaption");
			oCashFlowSetup.Remarks = oReader.GetString("Remarks");
            oCashFlowSetup.SubGroupCode = oReader.GetString("SubGroupCode");
			oCashFlowSetup.SubGroupName = oReader.GetString("SubGroupName");
            oCashFlowSetup.Amount = oReader.GetDouble("Amount");
			return oCashFlowSetup;
		}

		private CashFlowSetup CreateObject(NullHandler oReader)
		{
			CashFlowSetup oCashFlowSetup = new CashFlowSetup();
			oCashFlowSetup = MapObject(oReader);
			return oCashFlowSetup;
		}

		private List<CashFlowSetup> CreateObjects(IDataReader oReader)
		{
			List<CashFlowSetup> oCashFlowSetup = new List<CashFlowSetup>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				CashFlowSetup oItem = CreateObject(oHandler);
				oCashFlowSetup.Add(oItem);
			}
			return oCashFlowSetup;
		}

		#endregion

		#region Interface implementation
			public CashFlowSetup Save(CashFlowSetup oCashFlowSetup, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oCashFlowSetup.CashFlowSetupID <= 0)
					{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CashFlowSetup, EnumRoleOperationType.Add);
						reader = CashFlowSetupDA.InsertUpdate(tc, oCashFlowSetup, EnumDBOperation.Insert, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CashFlowSetup, EnumRoleOperationType.Edit);
						reader = CashFlowSetupDA.InsertUpdate(tc, oCashFlowSetup, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oCashFlowSetup = new CashFlowSetup();
						oCashFlowSetup = CreateObject(oReader);
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
						oCashFlowSetup = new CashFlowSetup();
						oCashFlowSetup.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oCashFlowSetup;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					CashFlowSetup oCashFlowSetup = new CashFlowSetup();
					oCashFlowSetup.CashFlowSetupID = id;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.CashFlowSetup, EnumRoleOperationType.Delete);
					CashFlowSetupDA.Delete(tc, oCashFlowSetup, EnumDBOperation.Delete, nUserId);
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

			public CashFlowSetup Get(int id, Int64 nUserId)
			{
				CashFlowSetup oCashFlowSetup = new CashFlowSetup();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = CashFlowSetupDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oCashFlowSetup = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get CashFlowSetup", e);
					#endregion
				}
				return oCashFlowSetup;
			}

			public List<CashFlowSetup> Gets(Int64 nUserID)
			{
				List<CashFlowSetup> oCashFlowSetups = new List<CashFlowSetup>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CashFlowSetupDA.Gets(tc);
					oCashFlowSetups = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					CashFlowSetup oCashFlowSetup = new CashFlowSetup();
					oCashFlowSetup.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oCashFlowSetups;
			}

            public List<CashFlowSetup> Gets(int nBUID, DateTime dStartDate, DateTime dEndDate, bool bIsDetails, Int64 nUserID)
            {
                List<CashFlowSetup> oCashFlowSetups = new List<CashFlowSetup>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = CashFlowSetupDA.Gets(nBUID, dStartDate, dEndDate,bIsDetails, tc);
                    oCashFlowSetups = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    CashFlowSetup oCashFlowSetup = new CashFlowSetup();
                    oCashFlowSetup.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oCashFlowSetups;
            }

			public List<CashFlowSetup> Gets (string sSQL, Int64 nUserID)
			{
				List<CashFlowSetup> oCashFlowSetups = new List<CashFlowSetup>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CashFlowSetupDA.Gets(tc, sSQL);
					oCashFlowSetups = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get CashFlowSetup", e);
					#endregion
				}
				return oCashFlowSetups;
			}

		#endregion
	}

}

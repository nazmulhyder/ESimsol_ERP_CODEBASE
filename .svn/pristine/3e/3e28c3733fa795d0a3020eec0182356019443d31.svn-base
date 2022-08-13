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
	public class CashFlowDmSetupService : MarshalByRefObject, ICashFlowDmSetupService
	{
		#region Private functions and declaration

        private CashFlowDmSetup MapObject(NullHandler oReader)
        {
            CashFlowDmSetup oCashFlowDmSetup = new CashFlowDmSetup();
            oCashFlowDmSetup.CashFlowDmSetupID = oReader.GetInt32("CashFlowDmSetupID");
            oCashFlowDmSetup.CashFlowHeadID = oReader.GetInt32("CashFlowHeadID");
            oCashFlowDmSetup.SubGroupID = oReader.GetInt32("SubGroupID");
            oCashFlowDmSetup.IsDebit = oReader.GetBoolean("IsDebit");
            oCashFlowDmSetup.Remarks = oReader.GetString("Remarks");
            oCashFlowDmSetup.SubGroupCode = oReader.GetString("SubGroupCode");
            oCashFlowDmSetup.SubGroupName = oReader.GetString("SubGroupName");
            oCashFlowDmSetup.DisplayCaption = oReader.GetString("DisplayCaption");
            oCashFlowDmSetup.CashFlowHeadType = (EnumCashFlowHeadType)oReader.GetInt32("CashFlowHeadType");
            oCashFlowDmSetup.CashFlowHeadTypeInt = oReader.GetInt32("CashFlowHeadType");
            oCashFlowDmSetup.Amount = oReader.GetDouble("Amount");
            return oCashFlowDmSetup;
        }

		private CashFlowDmSetup CreateObject(NullHandler oReader)
		{
			CashFlowDmSetup oCashFlowDmSetup = new CashFlowDmSetup();
			oCashFlowDmSetup = MapObject(oReader);
			return oCashFlowDmSetup;
		}

		private List<CashFlowDmSetup> CreateObjects(IDataReader oReader)
		{
			List<CashFlowDmSetup> oCashFlowDmSetup = new List<CashFlowDmSetup>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				CashFlowDmSetup oItem = CreateObject(oHandler);
				oCashFlowDmSetup.Add(oItem);
			}
			return oCashFlowDmSetup;
		}

		#endregion

		#region Interface implementation
			public CashFlowDmSetup Save(CashFlowDmSetup oCashFlowDmSetup, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oCashFlowDmSetup.CashFlowDmSetupID <= 0)
					{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CashFlowDmSetup, EnumRoleOperationType.Add);
						reader = CashFlowDmSetupDA.InsertUpdate(tc, oCashFlowDmSetup, EnumDBOperation.Insert, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CashFlowDmSetup, EnumRoleOperationType.Edit);
						reader = CashFlowDmSetupDA.InsertUpdate(tc, oCashFlowDmSetup, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oCashFlowDmSetup = new CashFlowDmSetup();
						oCashFlowDmSetup = CreateObject(oReader);
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
						oCashFlowDmSetup = new CashFlowDmSetup();
						oCashFlowDmSetup.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oCashFlowDmSetup;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					CashFlowDmSetup oCashFlowDmSetup = new CashFlowDmSetup();
					oCashFlowDmSetup.CashFlowDmSetupID = id;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.CashFlowDmSetup, EnumRoleOperationType.Delete);
					CashFlowDmSetupDA.Delete(tc, oCashFlowDmSetup, EnumDBOperation.Delete, nUserId);
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

			public CashFlowDmSetup Get(int id, Int64 nUserId)
			{
				CashFlowDmSetup oCashFlowDmSetup = new CashFlowDmSetup();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = CashFlowDmSetupDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oCashFlowDmSetup = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get CashFlowDmSetup", e);
					#endregion
				}
				return oCashFlowDmSetup;
			}

			public List<CashFlowDmSetup> Gets(Int64 nUserID)
			{
				List<CashFlowDmSetup> oCashFlowDmSetups = new List<CashFlowDmSetup>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CashFlowDmSetupDA.Gets(tc);
					oCashFlowDmSetups = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					CashFlowDmSetup oCashFlowDmSetup = new CashFlowDmSetup();
					oCashFlowDmSetup.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oCashFlowDmSetups;
			}

            public List<CashFlowDmSetup> Gets(int nBUID, DateTime dStartDate, DateTime dEndDate, bool bIsDetails, Int64 nUserID)
            {                
                List<CashFlowDmSetup> oCashFlowDmSetups = new List<CashFlowDmSetup>();              
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();                   
                    IDataReader reader = null;
                    reader = CashFlowDmSetupDA.Gets(nBUID, dStartDate, dEndDate,bIsDetails, tc);
                    oCashFlowDmSetups = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    CashFlowDmSetup oCashFlowDmSetup = new CashFlowDmSetup();
                    oCashFlowDmSetup.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oCashFlowDmSetups;
            }

			public List<CashFlowDmSetup> Gets (string sSQL, Int64 nUserID)
			{
				List<CashFlowDmSetup> oCashFlowDmSetups = new List<CashFlowDmSetup>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CashFlowDmSetupDA.Gets(tc, sSQL);
					oCashFlowDmSetups = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get CashFlowDmSetup", e);
					#endregion
				}
				return oCashFlowDmSetups;
			}

		#endregion
	}

}

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
	public class FPMReportSetupService : MarshalByRefObject, IFPMReportSetupService
	{
		#region Private functions and declaration

		private FPMReportSetup MapObject(NullHandler oReader)
		{
			FPMReportSetup oFPMReportSetup = new FPMReportSetup();
			oFPMReportSetup.FPMReportSetupID = oReader.GetInt32("FPMReportSetupID");
			oFPMReportSetup.SetUpType = (EnumFPReportSetUpType) oReader.GetInt32("SetUpType");
			oFPMReportSetup.AccountHeadID = oReader.GetInt32("AccountHeadID");
			oFPMReportSetup.SubSetup = (EnumFPReportSubSetup) oReader.GetInt32("SubSetup");
			oFPMReportSetup.AccountCode = oReader.GetString("AccountCode");
			oFPMReportSetup.AccountHeadName = oReader.GetString("AccountHeadName");
			return oFPMReportSetup;
		}

		private FPMReportSetup CreateObject(NullHandler oReader)
		{
			FPMReportSetup oFPMReportSetup = new FPMReportSetup();
			oFPMReportSetup = MapObject(oReader);
			return oFPMReportSetup;
		}

		private List<FPMReportSetup> CreateObjects(IDataReader oReader)
		{
			List<FPMReportSetup> oFPMReportSetup = new List<FPMReportSetup>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FPMReportSetup oItem = CreateObject(oHandler);
				oFPMReportSetup.Add(oItem);
			}
			return oFPMReportSetup;
		}

		#endregion

		#region Interface implementation
			public FPMReportSetup Save(FPMReportSetup oFPMReportSetup, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oFPMReportSetup.FPMReportSetupID <= 0)
					{
                        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FPMReportSetup", EnumRoleOperationType.Add);
						reader = FPMReportSetupDA.InsertUpdate(tc, oFPMReportSetup, EnumDBOperation.Insert, nUserID);
					}
					else{
                        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FPMReportSetup", EnumRoleOperationType.Edit);
						reader = FPMReportSetupDA.InsertUpdate(tc, oFPMReportSetup, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oFPMReportSetup = new FPMReportSetup();
						oFPMReportSetup = CreateObject(oReader);
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
						oFPMReportSetup = new FPMReportSetup();
						oFPMReportSetup.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFPMReportSetup;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FPMReportSetup oFPMReportSetup = new FPMReportSetup();
					oFPMReportSetup.FPMReportSetupID = id;
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "FPMReportSetup", EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "FPMReportSetup", id);
					FPMReportSetupDA.Delete(tc, oFPMReportSetup, EnumDBOperation.Delete, nUserId);
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

			public FPMReportSetup Get(int id, Int64 nUserId)
			{
				FPMReportSetup oFPMReportSetup = new FPMReportSetup();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FPMReportSetupDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFPMReportSetup = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FPMReportSetup", e);
					#endregion
				}
				return oFPMReportSetup;
			}

			public List<FPMReportSetup> Gets(Int64 nUserID)
			{
				List<FPMReportSetup> oFPMReportSetups = new List<FPMReportSetup>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FPMReportSetupDA.Gets(tc);
					oFPMReportSetups = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FPMReportSetup oFPMReportSetup = new FPMReportSetup();
					oFPMReportSetup.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFPMReportSetups;
			}

			public List<FPMReportSetup> Gets (string sSQL, Int64 nUserID)
			{
				List<FPMReportSetup> oFPMReportSetups = new List<FPMReportSetup>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FPMReportSetupDA.Gets(tc, sSQL);
					oFPMReportSetups = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FPMReportSetup", e);
					#endregion
				}
				return oFPMReportSetups;
			}

		#endregion
	}

}

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
	public class WOTermsAndConditionService : MarshalByRefObject, IWOTermsAndConditionService
	{
		#region Private functions and declaration

		private WOTermsAndCondition MapObject(NullHandler oReader)
		{
			WOTermsAndCondition oWOTermsAndCondition = new WOTermsAndCondition();
			oWOTermsAndCondition.WOTermsAndConditionID = oReader.GetInt32("WOTermsAndConditionID");
			oWOTermsAndCondition.WOID = oReader.GetInt32("WOID");
			oWOTermsAndCondition.TermsAndCondition = oReader.GetString("TermsAndCondition");
			return oWOTermsAndCondition;
		}

		private WOTermsAndCondition CreateObject(NullHandler oReader)
		{
			WOTermsAndCondition oWOTermsAndCondition = new WOTermsAndCondition();
			oWOTermsAndCondition = MapObject(oReader);
			return oWOTermsAndCondition;
		}

		private List<WOTermsAndCondition> CreateObjects(IDataReader oReader)
		{
			List<WOTermsAndCondition> oWOTermsAndCondition = new List<WOTermsAndCondition>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				WOTermsAndCondition oItem = CreateObject(oHandler);
				oWOTermsAndCondition.Add(oItem);
			}
			return oWOTermsAndCondition;
		}

		#endregion

		#region Interface implementation
	
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					WOTermsAndCondition oWOTermsAndCondition = new WOTermsAndCondition();
					oWOTermsAndCondition.WOTermsAndConditionID = id;
					//AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "WOTermsAndCondition", EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "WOTermsAndCondition", id);
					//WOTermsAndConditionDA.Delete(tc, oWOTermsAndCondition, EnumDBOperation.Delete, nUserId);
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

			public WOTermsAndCondition Get(int id, Int64 nUserId)
			{
				WOTermsAndCondition oWOTermsAndCondition = new WOTermsAndCondition();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = WOTermsAndConditionDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oWOTermsAndCondition = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get WOTermsAndCondition", e);
					#endregion
				}
				return oWOTermsAndCondition;
			}

			public List<WOTermsAndCondition> Gets(int nWOID, Int64 nUserID)
			{
				List<WOTermsAndCondition> oWOTermsAndConditions = new List<WOTermsAndCondition>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = WOTermsAndConditionDA.Gets(tc, nWOID);
					oWOTermsAndConditions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					WOTermsAndCondition oWOTermsAndCondition = new WOTermsAndCondition();
					oWOTermsAndCondition.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oWOTermsAndConditions;
			}

			public List<WOTermsAndCondition> Gets (string sSQL, Int64 nUserID)
			{
				List<WOTermsAndCondition> oWOTermsAndConditions = new List<WOTermsAndCondition>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = WOTermsAndConditionDA.Gets(tc, sSQL);
					oWOTermsAndConditions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get WOTermsAndCondition", e);
					#endregion
				}
				return oWOTermsAndConditions;
			}

		#endregion
	}

}

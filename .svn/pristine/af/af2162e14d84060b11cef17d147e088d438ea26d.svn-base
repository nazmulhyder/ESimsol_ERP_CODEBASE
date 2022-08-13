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
	public class TermsAndConditionService : MarshalByRefObject, ITermsAndConditionService
	{
		#region Private functions and declaration

		private TermsAndCondition MapObject(NullHandler oReader)
		{
			TermsAndCondition oTermsAndCondition = new TermsAndCondition();
			oTermsAndCondition.TermsAndConditionID = oReader.GetInt32("TermsAndConditionID");
			oTermsAndCondition.ModuleID = oReader.GetInt32("ModuleID");
            oTermsAndCondition.TermsAndConditionText = oReader.GetString("TermsAndConditionText");
			return oTermsAndCondition;
		}

		private TermsAndCondition CreateObject(NullHandler oReader)
		{
			TermsAndCondition oTermsAndCondition = new TermsAndCondition();
			oTermsAndCondition = MapObject(oReader);
			return oTermsAndCondition;
		}

		private List<TermsAndCondition> CreateObjects(IDataReader oReader)
		{
			List<TermsAndCondition> oTermsAndCondition = new List<TermsAndCondition>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				TermsAndCondition oItem = CreateObject(oHandler);
				oTermsAndCondition.Add(oItem);
			}
			return oTermsAndCondition;
		}

		#endregion

		#region Interface implementation
			public TermsAndCondition Save(TermsAndCondition oTermsAndCondition, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oTermsAndCondition.TermsAndConditionID <= 0)
					{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, Enum TermsAndCondition, EnumRoleOperationType.Add);
						reader = TermsAndConditionDA.InsertUpdate(tc, oTermsAndCondition, EnumDBOperation.Insert, nUserID);
					}
					else{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "TermsAndCondition", EnumRoleOperationType.Edit);
						reader = TermsAndConditionDA.InsertUpdate(tc, oTermsAndCondition, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oTermsAndCondition = new TermsAndCondition();
						oTermsAndCondition = CreateObject(oReader);
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
						oTermsAndCondition = new TermsAndCondition();
						oTermsAndCondition.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oTermsAndCondition;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					TermsAndCondition oTermsAndCondition = new TermsAndCondition();
					oTermsAndCondition.TermsAndConditionID = id;
					//AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "TermsAndCondition", EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "TermsAndCondition", id);
					TermsAndConditionDA.Delete(tc, oTermsAndCondition, EnumDBOperation.Delete, nUserId);
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

		public TermsAndCondition Get(int id, Int64 nUserId)
		{
			TermsAndCondition oTermsAndCondition = new TermsAndCondition();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = TermsAndConditionDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
					oTermsAndCondition = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get TermsAndCondition", e);
				#endregion
			}
			return oTermsAndCondition;
		}

		public List<TermsAndCondition> Gets(Int64 nUserID)
			{
				List<TermsAndCondition> oTermsAndConditions = new List<TermsAndCondition>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = TermsAndConditionDA.Gets(tc);
					oTermsAndConditions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					TermsAndCondition oTermsAndCondition = new TermsAndCondition();
					oTermsAndCondition.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oTermsAndConditions;
			}


            public List<TermsAndCondition> GetsByModule(int ModuleId, Int64 nUserID)
			{
				List<TermsAndCondition> oTermsAndConditions = new List<TermsAndCondition>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = TermsAndConditionDA.GetsByModule(tc, ModuleId);
					oTermsAndConditions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					TermsAndCondition oTermsAndCondition = new TermsAndCondition();
					oTermsAndCondition.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oTermsAndConditions;
			}
			public List<TermsAndCondition> Gets (string sSQL, Int64 nUserID)
			{
				List<TermsAndCondition> oTermsAndConditions = new List<TermsAndCondition>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = TermsAndConditionDA.Gets(tc, sSQL);
					oTermsAndConditions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get TermsAndCondition", e);
					#endregion
				}
				return oTermsAndConditions;
			}

		#endregion
	}

}

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
	public class StyleBudgetCMService : MarshalByRefObject, IStyleBudgetCMService
	{
		#region Private functions and declaration

		private StyleBudgetCM MapObject(NullHandler oReader)
		{
			StyleBudgetCM oStyleBudgetCM = new StyleBudgetCM();
			oStyleBudgetCM.StyleBudgetCMID = oReader.GetInt32("StyleBudgetCMID");
			oStyleBudgetCM.StyleBudgetID = oReader.GetInt32("StyleBudgetID");
			oStyleBudgetCM.NumberOfMachine = oReader.GetInt32("NumberOfMachine");
			oStyleBudgetCM.MachineCost = oReader.GetDouble("MachineCost");
			oStyleBudgetCM.ProductionPerDay = oReader.GetDouble("ProductionPerDay");
			oStyleBudgetCM.BufferDays = oReader.GetInt32("BufferDays");
			oStyleBudgetCM.TotalRequiredDays = oReader.GetInt32("TotalRequiredDays");
			oStyleBudgetCM.CMAdditionalPerent = oReader.GetDouble("CMAdditionalPerent");
            oStyleBudgetCM.CMPart = oReader.GetString("CMPart");
            
            oStyleBudgetCM.CSQty = oReader.GetDouble("CSQty");
			return oStyleBudgetCM;
		}

		private StyleBudgetCM CreateObject(NullHandler oReader)
		{
			StyleBudgetCM oStyleBudgetCM = new StyleBudgetCM();
			oStyleBudgetCM = MapObject(oReader);
			return oStyleBudgetCM;
		}

		private List<StyleBudgetCM> CreateObjects(IDataReader oReader)
		{
			List<StyleBudgetCM> oStyleBudgetCM = new List<StyleBudgetCM>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				StyleBudgetCM oItem = CreateObject(oHandler);
				oStyleBudgetCM.Add(oItem);
			}
			return oStyleBudgetCM;
		}

		#endregion

		#region Interface implementation
		

			public StyleBudgetCM Get(int id, Int64 nUserId)
			{
				StyleBudgetCM oStyleBudgetCM = new StyleBudgetCM();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = StyleBudgetCMDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oStyleBudgetCM = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get StyleBudgetCM", e);
					#endregion
				}
				return oStyleBudgetCM;
			}

			public List<StyleBudgetCM> Gets(int CSID, Int64 nUserID)
			{
				List<StyleBudgetCM> oStyleBudgetCMs = new List<StyleBudgetCM>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = StyleBudgetCMDA.Gets(CSID, tc);
					oStyleBudgetCMs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					StyleBudgetCM oStyleBudgetCM = new StyleBudgetCM();
					oStyleBudgetCM.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oStyleBudgetCMs;
			}
            public List<StyleBudgetCM> GetsByLog(int CSLogID, Int64 nUserID)
			{
				List<StyleBudgetCM> oStyleBudgetCMs = new List<StyleBudgetCM>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = StyleBudgetCMDA.GetsByLog(CSLogID, tc);
					oStyleBudgetCMs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					StyleBudgetCM oStyleBudgetCM = new StyleBudgetCM();
					oStyleBudgetCM.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oStyleBudgetCMs;
			}
        

			public List<StyleBudgetCM> Gets (string sSQL, Int64 nUserID)
			{
				List<StyleBudgetCM> oStyleBudgetCMs = new List<StyleBudgetCM>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = StyleBudgetCMDA.Gets(tc, sSQL);
					oStyleBudgetCMs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get StyleBudgetCM", e);
					#endregion
				}
				return oStyleBudgetCMs;
			}

		#endregion
	}

}

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
	public class StyleBudgetRecapService : MarshalByRefObject, IStyleBudgetRecapService
	{
		#region Private functions and declaration

		private StyleBudgetRecap MapObject(NullHandler oReader)
		{
			StyleBudgetRecap oStyleBudgetRecap = new StyleBudgetRecap();
			oStyleBudgetRecap.StyleBudgetRecapID = oReader.GetInt32("StyleBudgetRecapID");
			oStyleBudgetRecap.StyleBudgetID = oReader.GetInt32("StyleBudgetID");
            oStyleBudgetRecap.RefID = oReader.GetInt32("RefID");
			oStyleBudgetRecap.Note = oReader.GetString("Note");
            oStyleBudgetRecap.SessionName = oReader.GetString("SessionName");
            oStyleBudgetRecap.RefNo = oReader.GetString("RefNo");
            oStyleBudgetRecap.OrderConfimationDate = oReader.GetDateTime("OrderConfimationDate");
            oStyleBudgetRecap.UnitSymbol = oReader.GetString("UnitSymbol");
            oStyleBudgetRecap.RefType = (EnumStyleBudgetRecapType) oReader.GetInt16("RefType");
            
			oStyleBudgetRecap.UnitPrice = oReader.GetDouble("UnitPrice");
			oStyleBudgetRecap.Quantity = oReader.GetDouble("Quantity");
			oStyleBudgetRecap.Amount = oReader.GetDouble("Amount");
			return oStyleBudgetRecap;
		}

		private StyleBudgetRecap CreateObject(NullHandler oReader)
		{
			StyleBudgetRecap oStyleBudgetRecap = new StyleBudgetRecap();
			oStyleBudgetRecap = MapObject(oReader);
			return oStyleBudgetRecap;
		}

		private List<StyleBudgetRecap> CreateObjects(IDataReader oReader)
		{
			List<StyleBudgetRecap> oStyleBudgetRecap = new List<StyleBudgetRecap>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				StyleBudgetRecap oItem = CreateObject(oHandler);
				oStyleBudgetRecap.Add(oItem);
			}
			return oStyleBudgetRecap;
		}

		#endregion

		#region Interface implementation
		

			public StyleBudgetRecap Get(int id, Int64 nUserId)
			{
				StyleBudgetRecap oStyleBudgetRecap = new StyleBudgetRecap();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = StyleBudgetRecapDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oStyleBudgetRecap = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get StyleBudgetRecap", e);
					#endregion
				}
				return oStyleBudgetRecap;
			}

			public List<StyleBudgetRecap> Gets(int nCSID, Int64 nUserID)
			{
				List<StyleBudgetRecap> oStyleBudgetRecaps = new List<StyleBudgetRecap>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = StyleBudgetRecapDA.Gets(nCSID, tc);
					oStyleBudgetRecaps = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					StyleBudgetRecap oStyleBudgetRecap = new StyleBudgetRecap();
					oStyleBudgetRecap.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oStyleBudgetRecaps;
			}

			public List<StyleBudgetRecap> Gets (string sSQL, Int64 nUserID)
			{
				List<StyleBudgetRecap> oStyleBudgetRecaps = new List<StyleBudgetRecap>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = StyleBudgetRecapDA.Gets(tc, sSQL);
					oStyleBudgetRecaps = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get StyleBudgetRecap", e);
					#endregion
				}
				return oStyleBudgetRecaps;
			}

		#endregion
	}

}

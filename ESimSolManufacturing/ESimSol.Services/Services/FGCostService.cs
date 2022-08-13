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
	public class FGCostService : MarshalByRefObject, IFGCostService
	{
		#region Private functions and declaration

		private FGCost MapObject(NullHandler oReader)
		{
			FGCost oFGCost = new FGCost();
			oFGCost.FGCostID = oReader.GetInt32("FGCostID");
			oFGCost.ITransactionID = oReader.GetInt32("ITransactionID");
			oFGCost.QCID = oReader.GetInt32("QCID");
			oFGCost.RMID = oReader.GetInt32("RMID");
			oFGCost.MUnitID = oReader.GetInt32("MUnitID");
			oFGCost.RMQty = oReader.GetDouble("RMQty");
			oFGCost.UnitPrice = oReader.GetDouble("UnitPrice");
			oFGCost.Amount = oReader.GetDouble("Amount");
			oFGCost.MUName = oReader.GetString("MUName");
			oFGCost.ProductCode = oReader.GetString("ProductCode");
			oFGCost.ProductName = oReader.GetString("ProductName");
			oFGCost.LotNo = oReader.GetString("LotNo");
            oFGCost.RMRequisitionNo = oReader.GetString("RMRequisitionNo");
            oFGCost.CurrencySymbol = oReader.GetString("CurrencySymbol");
            
			return oFGCost;
		}

		private FGCost CreateObject(NullHandler oReader)
		{
			FGCost oFGCost = new FGCost();
			oFGCost = MapObject(oReader);
			return oFGCost;
		}

		private List<FGCost> CreateObjects(IDataReader oReader)
		{
			List<FGCost> oFGCost = new List<FGCost>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FGCost oItem = CreateObject(oHandler);
				oFGCost.Add(oItem);
			}
			return oFGCost;
		}

		#endregion

		#region Interface implementation
     

			public List<FGCost> Gets(int nQCID, Int64 nUserID)
			{
				List<FGCost> oFGCosts = new List<FGCost>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = FGCostDA.Gets(tc, nQCID);
					oFGCosts = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FGCost oFGCost = new FGCost();
					oFGCost.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFGCosts;
			}

			public List<FGCost> Gets (string sSQL, Int64 nUserID)
			{
				List<FGCost> oFGCosts = new List<FGCost>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FGCostDA.Gets(tc, sSQL);
					oFGCosts = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FGCost", e);
					#endregion
				}
				return oFGCosts;
			}

		#endregion
	}

}

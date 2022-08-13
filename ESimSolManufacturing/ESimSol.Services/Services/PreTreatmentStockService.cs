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
	public class PreTreatmentStockService : MarshalByRefObject, IPreTreatmentStockService
	{
		#region Private functions and declaration

		private PreTreatmentStock MapObject(NullHandler oReader)
		{
			PreTreatmentStock oPreTreatmentStock = new PreTreatmentStock();
			oPreTreatmentStock.ProductID = oReader.GetInt32("ProductID");
			oPreTreatmentStock.ProductName = oReader.GetString("ProductName");
			oPreTreatmentStock.ProductCode = oReader.GetString("ProductCode");
            oPreTreatmentStock.LotNo = oReader.GetString("LotNo");
			oPreTreatmentStock.LotID = oReader.GetInt32("LotID");
            //oPreTreatmentStock.TreatmentID = oReader.GetInt32("TreatmentID");
			oPreTreatmentStock.DisburseQty = oReader.GetDouble("DisburseQty");
			oPreTreatmentStock.ConsumptionQty = oReader.GetDouble("ConsumptionQty");
            oPreTreatmentStock.YetToConsumptionQty = oReader.GetDouble("YetToConsumptionQty");
			return oPreTreatmentStock;
		}

		private PreTreatmentStock CreateObject(NullHandler oReader)
		{
			PreTreatmentStock oPreTreatmentStock = new PreTreatmentStock();
			oPreTreatmentStock = MapObject(oReader);
			return oPreTreatmentStock;
		}

		private List<PreTreatmentStock> CreateObjects(IDataReader oReader)
		{
			List<PreTreatmentStock> oPreTreatmentStock = new List<PreTreatmentStock>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				PreTreatmentStock oItem = CreateObject(oHandler);
				oPreTreatmentStock.Add(oItem);
			}
			return oPreTreatmentStock;
		}

		#endregion

		#region Interface implementation
			
			public List<PreTreatmentStock> Gets (string sSQL, Int64 nUserID)
			{
				List<PreTreatmentStock> oPreTreatmentStocks = new List<PreTreatmentStock>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PreTreatmentStockDA.Gets(tc, sSQL);
					oPreTreatmentStocks = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get PreTreatmentStock", e);
					#endregion
				}
				return oPreTreatmentStocks;
			}
            public List<PreTreatmentStock> GetsStock(int ProductID, int TreatmentProcess, Int64 nUserID)
			    {
				    List<PreTreatmentStock> oPreTreatmentStocks = new List<PreTreatmentStock>();
				    TransactionContext tc = null;
				    try
				    {
					    tc = TransactionContext.Begin();
					    IDataReader reader = null;
                        reader = PreTreatmentStockDA.GetsStock(tc, ProductID, TreatmentProcess);
					    oPreTreatmentStocks = CreateObjects(reader);
					    reader.Close();
					    tc.End();
				    }
				    catch (Exception e)
				    {
					    #region Handle Exception
					    if (tc != null);
					    tc.HandleError();
					    ExceptionLog.Write(e);
					    throw new ServiceException("Failed to Get PreTreatmentStock", e);
					    #endregion
				    }
				    return oPreTreatmentStocks;
			    }

		#endregion
	}

}

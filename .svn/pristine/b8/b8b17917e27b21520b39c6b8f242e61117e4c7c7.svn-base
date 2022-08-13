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
	public class RawMaterialSourcingService : MarshalByRefObject, IRawMaterialSourcingService
	{
		#region Private functions and declaration

		private RawMaterialSourcing MapObject(NullHandler oReader)
		{
			RawMaterialSourcing oRawMaterialSourcing = new RawMaterialSourcing();
			oRawMaterialSourcing.ProductID = oReader.GetInt32("ProductID");
			oRawMaterialSourcing.ProductCode = oReader.GetString("ProductCode");
			oRawMaterialSourcing.ProductName = oReader.GetString("ProductName");
			oRawMaterialSourcing.OrderRecapID = oReader.GetInt32("OrderRecapID");
			oRawMaterialSourcing.ColorName = oReader.GetString("ColorName");
			oRawMaterialSourcing.SizeName = oReader.GetString("SizeName");
			oRawMaterialSourcing.ReqQty = oReader.GetDouble("ReqQty");
			oRawMaterialSourcing.UnitName = oReader.GetString("UnitName");
			oRawMaterialSourcing.Balance = oReader.GetDouble("Balance");
			oRawMaterialSourcing.ContractorID = oReader.GetInt32("ContractorID");
			oRawMaterialSourcing.SupplierName = oReader.GetString("SupplierName");
            oRawMaterialSourcing.ConsumptionQty = oReader.GetDouble("ConsumptionQty");
			return oRawMaterialSourcing;
		}

		private RawMaterialSourcing CreateObject(NullHandler oReader)
		{
			RawMaterialSourcing oRawMaterialSourcing = new RawMaterialSourcing();
			oRawMaterialSourcing = MapObject(oReader);
			return oRawMaterialSourcing;
		}

		private List<RawMaterialSourcing> CreateObjects(IDataReader oReader)
		{
			List<RawMaterialSourcing> oRawMaterialSourcing = new List<RawMaterialSourcing>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				RawMaterialSourcing oItem = CreateObject(oHandler);
				oRawMaterialSourcing.Add(oItem);
			}
			return oRawMaterialSourcing;
		}

		#endregion

		#region Interface implementation
			public List<RawMaterialSourcing> Gets(int OrderRecapID, Int64 nUserID)
			{
				List<RawMaterialSourcing> oRawMaterialSourcings = new List<RawMaterialSourcing>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = RawMaterialSourcingDA.Gets(tc, OrderRecapID);
					oRawMaterialSourcings = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					RawMaterialSourcing oRawMaterialSourcing = new RawMaterialSourcing();
					oRawMaterialSourcing.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oRawMaterialSourcings;
			}

			public List<RawMaterialSourcing> Gets (string sSQL, Int64 nUserID)
			{
				List<RawMaterialSourcing> oRawMaterialSourcings = new List<RawMaterialSourcing>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = RawMaterialSourcingDA.Gets(tc, sSQL);
					oRawMaterialSourcings = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get RawMaterialSourcing", e);
					#endregion
				}
				return oRawMaterialSourcings;
			}

		#endregion
	}

}

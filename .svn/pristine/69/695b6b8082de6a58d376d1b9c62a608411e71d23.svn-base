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
	public class RMConsumptionDetailService : MarshalByRefObject, IRMConsumptionDetailService
	{
		#region Private functions and declaration

		private RMConsumptionDetail MapObject(NullHandler oReader)
		{
			RMConsumptionDetail oRMConsumptionDetail = new RMConsumptionDetail();
			oRMConsumptionDetail.RMConsumptionDetailID = oReader.GetInt32("RMConsumptionDetailID");
			oRMConsumptionDetail.RMConsumptionID = oReader.GetInt32("RMConsumptionID");
			oRMConsumptionDetail.ITransactionID = oReader.GetInt32("ITransactionID");
			oRMConsumptionDetail.ProductID = oReader.GetInt32("ProductID");
			oRMConsumptionDetail.LotID = oReader.GetInt32("LotID");
			oRMConsumptionDetail.WUID = oReader.GetInt32("WUID");
			oRMConsumptionDetail.MUnitID = oReader.GetInt32("MUnitID");
			oRMConsumptionDetail.Qty = oReader.GetDouble("Qty");
			oRMConsumptionDetail.UnitPrice = oReader.GetDouble("UnitPrice");
			oRMConsumptionDetail.Amount = oReader.GetDouble("Amount");
			oRMConsumptionDetail.ProductCode = oReader.GetString("ProductCode");
			oRMConsumptionDetail.ProductName = oReader.GetString("ProductName");
			oRMConsumptionDetail.WUName = oReader.GetString("WUName");
            oRMConsumptionDetail.MUName = oReader.GetString("MUName");
			oRMConsumptionDetail.LotNo = oReader.GetString("LotNo");
            oRMConsumptionDetail.RefNo = oReader.GetString("RefNo");
			return oRMConsumptionDetail;
		}

		private RMConsumptionDetail CreateObject(NullHandler oReader)
		{
			RMConsumptionDetail oRMConsumptionDetail = new RMConsumptionDetail();
			oRMConsumptionDetail = MapObject(oReader);
			return oRMConsumptionDetail;
		}

		private List<RMConsumptionDetail> CreateObjects(IDataReader oReader)
		{
			List<RMConsumptionDetail> oRMConsumptionDetail = new List<RMConsumptionDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				RMConsumptionDetail oItem = CreateObject(oHandler);
				oRMConsumptionDetail.Add(oItem);
			}
			return oRMConsumptionDetail;
		}

		#endregion

		#region Interface implementation
			
			public List<RMConsumptionDetail> Gets(int id, Int64 nUserID)
			{
				List<RMConsumptionDetail> oRMConsumptionDetails = new List<RMConsumptionDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = RMConsumptionDetailDA.Gets(id, tc);
					oRMConsumptionDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					RMConsumptionDetail oRMConsumptionDetail = new RMConsumptionDetail();
					oRMConsumptionDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oRMConsumptionDetails;
			}

			public List<RMConsumptionDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<RMConsumptionDetail> oRMConsumptionDetails = new List<RMConsumptionDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = RMConsumptionDetailDA.Gets(tc, sSQL);
					oRMConsumptionDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get RMConsumptionDetail", e);
					#endregion
				}
				return oRMConsumptionDetails;
			}

		#endregion
	}

}

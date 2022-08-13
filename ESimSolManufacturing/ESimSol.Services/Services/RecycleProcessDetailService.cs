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
	public class RecycleProcessDetailService : MarshalByRefObject, IRecycleProcessDetailService
	{
		#region Private functions and declaration

		private RecycleProcessDetail MapObject(NullHandler oReader)
		{
			RecycleProcessDetail oRecycleProcessDetail = new RecycleProcessDetail();
			oRecycleProcessDetail.RecycleProcessDetailID = oReader.GetInt32("RecycleProcessDetailID");
			oRecycleProcessDetail.RecycleProcessID = oReader.GetInt32("RecycleProcessID");
			oRecycleProcessDetail.ProductID = oReader.GetInt32("ProductID");
			oRecycleProcessDetail.UnitID = oReader.GetInt32("UnitID");
			oRecycleProcessDetail.Qty = oReader.GetDouble("Qty");
            oRecycleProcessDetail.ProcessProductType = (EnumProcessProductType)oReader.GetInt32("ProcessProductType");
            oRecycleProcessDetail.LotID = oReader.GetInt32("LotID");
            oRecycleProcessDetail.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
			oRecycleProcessDetail.ProductCode = oReader.GetString("ProductCode");
			oRecycleProcessDetail.ProductName = oReader.GetString("ProductName");
			oRecycleProcessDetail.Symbol = oReader.GetString("Symbol");
			oRecycleProcessDetail.UnitName = oReader.GetString("UnitName");
            oRecycleProcessDetail.WorkingUnitName = oReader.GetString("WorkingUnitName");
            oRecycleProcessDetail.WorkingUName = oReader.GetString("WorkingUName");
            oRecycleProcessDetail.LotNo = oReader.GetString("LotNo");
			return oRecycleProcessDetail;
		}

		private RecycleProcessDetail CreateObject(NullHandler oReader)
		{
			RecycleProcessDetail oRecycleProcessDetail = new RecycleProcessDetail();
			oRecycleProcessDetail = MapObject(oReader);
			return oRecycleProcessDetail;
		}

		private List<RecycleProcessDetail> CreateObjects(IDataReader oReader)
		{
			List<RecycleProcessDetail> oRecycleProcessDetail = new List<RecycleProcessDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				RecycleProcessDetail oItem = CreateObject(oHandler);
				oRecycleProcessDetail.Add(oItem);
			}
			return oRecycleProcessDetail;
		}

		#endregion

		#region Interface implementation
		
        public RecycleProcessDetail Get(int id, Int64 nUserId)
			{
				RecycleProcessDetail oRecycleProcessDetail = new RecycleProcessDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = RecycleProcessDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oRecycleProcessDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get RecycleProcessDetail", e);
					#endregion
				}
				return oRecycleProcessDetail;
			}

		public List<RecycleProcessDetail> Gets(int id, Int64 nUserID)
		{
			List<RecycleProcessDetail> oRecycleProcessDetails = new List<RecycleProcessDetail>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = RecycleProcessDetailDA.Gets(tc, id);
				oRecycleProcessDetails = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				RecycleProcessDetail oRecycleProcessDetail = new RecycleProcessDetail();
				oRecycleProcessDetail.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oRecycleProcessDetails;
		}

	   public List<RecycleProcessDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<RecycleProcessDetail> oRecycleProcessDetails = new List<RecycleProcessDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = RecycleProcessDetailDA.Gets(tc, sSQL);
					oRecycleProcessDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get RecycleProcessDetail", e);
					#endregion
				}
				return oRecycleProcessDetails;
			}

		#endregion
	}

}

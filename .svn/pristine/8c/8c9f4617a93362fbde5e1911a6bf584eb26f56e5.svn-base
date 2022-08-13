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
	public class VehicleOrderDetailService : MarshalByRefObject, IVehicleOrderDetailService
	{
		#region Private functions and declaration

		private VehicleOrderDetail MapObject(NullHandler oReader)
		{
			VehicleOrderDetail oVehicleOrderDetail = new VehicleOrderDetail();
			oVehicleOrderDetail.VehicleOrderDetailID = oReader.GetInt32("VehicleOrderDetailID");
            oVehicleOrderDetail.VehicleOrderID = oReader.GetInt32("VehicleOrderID");
            oVehicleOrderDetail.FeatureID = oReader.GetInt32("FeatureID");
            oVehicleOrderDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oVehicleOrderDetail.FeatureType =  (EnumFeatureType)oReader.GetInt32("FeatureType");
            oVehicleOrderDetail.FeatureTypeInInt = oReader.GetInt32("FeatureType");
            oVehicleOrderDetail.FeatureCode = oReader.GetString("FeatureCode");
            oVehicleOrderDetail.FeatureName = oReader.GetString("FeatureName");
            oVehicleOrderDetail.CurrencyName = oReader.GetString("CurrencyName");
            oVehicleOrderDetail.CurrencySymbol = oReader.GetString("Symbol");
            oVehicleOrderDetail.Remarks = oReader.GetString("Remarks");
            oVehicleOrderDetail.Price = oReader.GetDouble("Price");
			return oVehicleOrderDetail;
		}

		private VehicleOrderDetail CreateObject(NullHandler oReader)
		{
			VehicleOrderDetail oVehicleOrderDetail = new VehicleOrderDetail();
			oVehicleOrderDetail = MapObject(oReader);
			return oVehicleOrderDetail;
		}

		private List<VehicleOrderDetail> CreateObjects(IDataReader oReader)
		{
			List<VehicleOrderDetail> oVehicleOrderDetail = new List<VehicleOrderDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				VehicleOrderDetail oItem = CreateObject(oHandler);
				oVehicleOrderDetail.Add(oItem);
			}
			return oVehicleOrderDetail;
		}

		#endregion

		#region Interface implementation
		
        public VehicleOrderDetail Get(int id, Int64 nUserId)
			{
				VehicleOrderDetail oVehicleOrderDetail = new VehicleOrderDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = VehicleOrderDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oVehicleOrderDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get VehicleOrderDetail", e);
					#endregion
				}
				return oVehicleOrderDetail;
			}

		public List<VehicleOrderDetail> Gets(int id, Int64 nUserID)
		{
			List<VehicleOrderDetail> oVehicleOrderDetails = new List<VehicleOrderDetail>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = VehicleOrderDetailDA.Gets(tc, id);
				oVehicleOrderDetails = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				VehicleOrderDetail oVehicleOrderDetail = new VehicleOrderDetail();
				oVehicleOrderDetail.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oVehicleOrderDetails;
		}

	   public List<VehicleOrderDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<VehicleOrderDetail> oVehicleOrderDetails = new List<VehicleOrderDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = VehicleOrderDetailDA.Gets(tc, sSQL);
					oVehicleOrderDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get VehicleOrderDetail", e);
					#endregion
				}
				return oVehicleOrderDetails;
			}

		#endregion
	}

}

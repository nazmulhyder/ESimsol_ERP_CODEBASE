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
	public class ServiceOrderDetailService : MarshalByRefObject, IServiceOrderDetailService
	{
		#region Private functions and declaration

		private ServiceOrderDetail MapObject(NullHandler oReader)
		{
			ServiceOrderDetail oServiceOrderDetail = new ServiceOrderDetail();
			oServiceOrderDetail.ServiceOrderDetailID = oReader.GetInt32("ServiceOrderDetailID");
            oServiceOrderDetail.ServiceOrderID = oReader.GetInt32("ServiceOrderID");
            oServiceOrderDetail.WorkDescription = oReader.GetString("WorkDescription");
            oServiceOrderDetail.ServiceWorkType = (EnumServiceType)oReader.GetInt32("ServiceWorkType");
            oServiceOrderDetail.ServiceWorkTypeInt = oReader.GetInt32("ServiceWorkType");
			return oServiceOrderDetail;
		}

		private ServiceOrderDetail CreateObject(NullHandler oReader)
		{
			ServiceOrderDetail oServiceOrderDetail = new ServiceOrderDetail();
			oServiceOrderDetail = MapObject(oReader);
			return oServiceOrderDetail;
		}

		private List<ServiceOrderDetail> CreateObjects(IDataReader oReader)
		{
			List<ServiceOrderDetail> oServiceOrderDetail = new List<ServiceOrderDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				ServiceOrderDetail oItem = CreateObject(oHandler);
				oServiceOrderDetail.Add(oItem);
			}
			return oServiceOrderDetail;
		}

		#endregion

		#region Interface implementation
		
        public ServiceOrderDetail Get(int id, Int64 nUserId)
			{
				ServiceOrderDetail oServiceOrderDetail = new ServiceOrderDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = ServiceOrderDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oServiceOrderDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get ServiceOrderDetail", e);
					#endregion
				}
				return oServiceOrderDetail;
			}

		public List<ServiceOrderDetail> Gets(int id, Int64 nUserID)
		{
			List<ServiceOrderDetail> oServiceOrderDetails = new List<ServiceOrderDetail>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = ServiceOrderDetailDA.Gets(tc, id);
				oServiceOrderDetails = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				ServiceOrderDetail oServiceOrderDetail = new ServiceOrderDetail();
				oServiceOrderDetail.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oServiceOrderDetails;
		}

	   public List<ServiceOrderDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<ServiceOrderDetail> oServiceOrderDetails = new List<ServiceOrderDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ServiceOrderDetailDA.Gets(tc, sSQL);
					oServiceOrderDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get ServiceOrderDetail", e);
					#endregion
				}
				return oServiceOrderDetails;
			}

		#endregion
	}

}

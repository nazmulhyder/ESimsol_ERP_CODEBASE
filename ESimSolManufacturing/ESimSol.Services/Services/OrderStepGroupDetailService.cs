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
	public class OrderStepGroupDetailService : MarshalByRefObject, IOrderStepGroupDetailService
	{
		#region Private functions and declaration

		private OrderStepGroupDetail MapObject(NullHandler oReader)
		{
			OrderStepGroupDetail oOrderStepGroupDetail = new OrderStepGroupDetail();
			oOrderStepGroupDetail.OrderStepGroupDetailID = oReader.GetInt32("OrderStepGroupDetailID");
			oOrderStepGroupDetail.OrderStepGroupID = oReader.GetInt32("OrderStepGroupID");
			oOrderStepGroupDetail.OrderStepID = oReader.GetInt32("OrderStepID");
			oOrderStepGroupDetail.Sequence = oReader.GetInt32("Sequence");
			oOrderStepGroupDetail.StepName = oReader.GetString("StepName");
			return oOrderStepGroupDetail;
		}

		private OrderStepGroupDetail CreateObject(NullHandler oReader)
		{
			OrderStepGroupDetail oOrderStepGroupDetail = new OrderStepGroupDetail();
			oOrderStepGroupDetail = MapObject(oReader);
			return oOrderStepGroupDetail;
		}

		private List<OrderStepGroupDetail> CreateObjects(IDataReader oReader)
		{
			List<OrderStepGroupDetail> oOrderStepGroupDetail = new List<OrderStepGroupDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				OrderStepGroupDetail oItem = CreateObject(oHandler);
				oOrderStepGroupDetail.Add(oItem);
			}
			return oOrderStepGroupDetail;
		}

		#endregion

		#region Interface implementation
		
			public OrderStepGroupDetail Get(int id, Int64 nUserId)
			{
				OrderStepGroupDetail oOrderStepGroupDetail = new OrderStepGroupDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = OrderStepGroupDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oOrderStepGroupDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get OrderStepGroupDetail", e);
					#endregion
				}
				return oOrderStepGroupDetail;
			}

			public List<OrderStepGroupDetail> Gets(int id, Int64 nUserID)
			{
				List<OrderStepGroupDetail> oOrderStepGroupDetails = new List<OrderStepGroupDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = OrderStepGroupDetailDA.Gets(tc, id);
					oOrderStepGroupDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					OrderStepGroupDetail oOrderStepGroupDetail = new OrderStepGroupDetail();
					oOrderStepGroupDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oOrderStepGroupDetails;
			}

			public List<OrderStepGroupDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<OrderStepGroupDetail> oOrderStepGroupDetails = new List<OrderStepGroupDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = OrderStepGroupDetailDA.Gets(tc, sSQL);
					oOrderStepGroupDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get OrderStepGroupDetail", e);
					#endregion
				}
				return oOrderStepGroupDetails;
			}

		#endregion
	}

}

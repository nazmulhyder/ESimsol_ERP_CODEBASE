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
	public class JobDetailService : MarshalByRefObject, IJobDetailService
	{
		#region Private functions and declaration

		private JobDetail MapObject(NullHandler oReader)
		{
			JobDetail oJobDetail = new JobDetail();
			oJobDetail.JobDetailID = oReader.GetInt32("JobDetailID");
			oJobDetail.JobID = oReader.GetInt32("JobID");
			oJobDetail.OrderRecapID = oReader.GetInt32("OrderRecapID");
			oJobDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
			oJobDetail.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oJobDetail.TotalQuantity = oReader.GetDouble("TotalQuantity");
            oJobDetail.ProductName = oReader.GetString("ProductName");
            oJobDetail.DeptName = oReader.GetString("DeptName");
            oJobDetail.TSTypeInt = oReader.GetInt32("TSType");
			return oJobDetail;
		}

		private JobDetail CreateObject(NullHandler oReader)
		{
			JobDetail oJobDetail = new JobDetail();
			oJobDetail = MapObject(oReader);
			return oJobDetail;
		}

		private List<JobDetail> CreateObjects(IDataReader oReader)
		{
			List<JobDetail> oJobDetail = new List<JobDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				JobDetail oItem = CreateObject(oHandler);
				oJobDetail.Add(oItem);
			}
			return oJobDetail;
		}

		#endregion

		#region Interface implementation
		

			public JobDetail Get(int id, Int64 nUserId)
			{
				JobDetail oJobDetail = new JobDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = JobDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oJobDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get JobDetail", e);
					#endregion
				}
				return oJobDetail;
			}

			public List<JobDetail> Gets(int nJobID,  Int64 nUserID)
			{
				List<JobDetail> oJobDetails = new List<JobDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = JobDetailDA.Gets(tc, nJobID);
					oJobDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					JobDetail oJobDetail = new JobDetail();
					oJobDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oJobDetails;
			}

			public List<JobDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<JobDetail> oJobDetails = new List<JobDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = JobDetailDA.Gets(tc, sSQL);
					oJobDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get JobDetail", e);
					#endregion
				}
				return oJobDetails;
			}

		#endregion
	}

}

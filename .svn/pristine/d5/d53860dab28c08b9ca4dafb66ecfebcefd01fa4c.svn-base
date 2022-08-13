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
	public class ServiceILaborChargeDetailService : MarshalByRefObject, IServiceILaborChargeDetailService
	{
		#region Private functions and declaration

		private ServiceILaborChargeDetail MapObject(NullHandler oReader)
		{
			ServiceILaborChargeDetail oServiceILaborChargeDetail = new ServiceILaborChargeDetail();
			oServiceILaborChargeDetail.ServiceILaborChargeDetailID = oReader.GetInt32("ServiceILaborChargeDetailID");
            oServiceILaborChargeDetail.ServiceInvoiceID = oReader.GetInt32("ServiceInvoiceID");
            oServiceILaborChargeDetail.ServiceILaborChargeDetailLogID = oReader.GetInt32("ServiceILaborChargeDetailLogID");
            oServiceILaborChargeDetail.ServiceInvoiceLogID = oReader.GetInt32("ServiceInvoiceLogID");
            oServiceILaborChargeDetail.ServiceName = oReader.GetString("ServiceName");
            oServiceILaborChargeDetail.ServiceCode = oReader.GetString("ServiceCode");
            oServiceILaborChargeDetail.ServiceWorkID = oReader.GetInt32("ServiceWorkID");
            oServiceILaborChargeDetail.WorkingHour = oReader.GetDouble("WorkingHour");
            oServiceILaborChargeDetail.WorkingCost = oReader.GetDouble("WorkingCost");
            oServiceILaborChargeDetail.ChargeAmount = oReader.GetDouble("ChargeAmount");
            oServiceILaborChargeDetail.ChargeDescription = oReader.GetString("ChargeDescription");
            oServiceILaborChargeDetail.LaborChargeType = (EnumServiceILaborChargeType)oReader.GetInt32("LaborChargeType");
			return oServiceILaborChargeDetail;
		}

		private ServiceILaborChargeDetail CreateObject(NullHandler oReader)
		{
			ServiceILaborChargeDetail oServiceILaborChargeDetail = new ServiceILaborChargeDetail();
			oServiceILaborChargeDetail = MapObject(oReader);
			return oServiceILaborChargeDetail;
		}

		private List<ServiceILaborChargeDetail> CreateObjects(IDataReader oReader)
		{
			List<ServiceILaborChargeDetail> oServiceILaborChargeDetail = new List<ServiceILaborChargeDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				ServiceILaborChargeDetail oItem = CreateObject(oHandler);
				oServiceILaborChargeDetail.Add(oItem);
			}
			return oServiceILaborChargeDetail;
		}

		#endregion

		#region Interface implementation
		
        public ServiceILaborChargeDetail Get(int id, Int64 nUserId)
			{
				ServiceILaborChargeDetail oServiceILaborChargeDetail = new ServiceILaborChargeDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = ServiceILaborChargeDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oServiceILaborChargeDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get ServiceILaborChargeDetail", e);
					#endregion
				}
				return oServiceILaborChargeDetail;
			}

		public List<ServiceILaborChargeDetail> Gets(int id, Int64 nUserID)
		{
			List<ServiceILaborChargeDetail> oServiceILaborChargeDetails = new List<ServiceILaborChargeDetail>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = ServiceILaborChargeDetailDA.Gets(tc, id);
				oServiceILaborChargeDetails = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				ServiceILaborChargeDetail oServiceILaborChargeDetail = new ServiceILaborChargeDetail();
				oServiceILaborChargeDetail.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oServiceILaborChargeDetails;
		}
        public List<ServiceILaborChargeDetail> GetsLog(int id, Int64 nUserID)
        {
            List<ServiceILaborChargeDetail> oServiceILaborChargeDetails = new List<ServiceILaborChargeDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ServiceILaborChargeDetailDA.GetsLog(tc, id);
                oServiceILaborChargeDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ServiceILaborChargeDetail oServiceILaborChargeDetail = new ServiceILaborChargeDetail();
                oServiceILaborChargeDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oServiceILaborChargeDetails;
        }

	   public List<ServiceILaborChargeDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<ServiceILaborChargeDetail> oServiceILaborChargeDetails = new List<ServiceILaborChargeDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ServiceILaborChargeDetailDA.Gets(tc, sSQL);
					oServiceILaborChargeDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get ServiceILaborChargeDetail", e);
					#endregion
				}
				return oServiceILaborChargeDetails;
			}

		#endregion
	}

}

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
	public class ServiceInvoiceDetailService : MarshalByRefObject, IServiceInvoiceDetailService
	{
		#region Private functions and declaration

		private ServiceInvoiceDetail MapObject(NullHandler oReader)
		{
			ServiceInvoiceDetail oServiceInvoiceDetail = new ServiceInvoiceDetail();
			oServiceInvoiceDetail.ServiceInvoiceDetailID = oReader.GetInt32("ServiceInvoiceDetailID");
            oServiceInvoiceDetail.ServiceInvoiceID = oReader.GetInt32("ServiceInvoiceID");
            oServiceInvoiceDetail.ServiceInvoiceDetailLogID = oReader.GetInt32("ServiceInvoiceDetailLogID");
            oServiceInvoiceDetail.ServiceInvoiceLogID = oReader.GetInt32("ServiceInvoiceLogID");
            oServiceInvoiceDetail.MUnitID = oReader.GetInt32("MUnitID");
            oServiceInvoiceDetail.PartsCode = oReader.GetString("PartsCode");
            oServiceInvoiceDetail.VehiclePartsID = oReader.GetInt32("VehiclePartsID");
            oServiceInvoiceDetail.PartsName = oReader.GetString("PartsName");
            oServiceInvoiceDetail.PartsNo = oReader.GetString("PartsNo");
            oServiceInvoiceDetail.MUName = oReader.GetString("MUName");
            oServiceInvoiceDetail.Remarks = oReader.GetString("Remarks");
            oServiceInvoiceDetail.Qty = oReader.GetDouble("Qty");
            oServiceInvoiceDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oServiceInvoiceDetail.Amount = oReader.GetDouble("Amount");
            oServiceInvoiceDetail.ServiceType = (EnumServiceType)oReader.GetInt32("ServiceType");
            oServiceInvoiceDetail.WorkChargeType = (EnumServiceILaborChargeType)oReader.GetInt32("WorkChargeType");
			return oServiceInvoiceDetail;
		}

		private ServiceInvoiceDetail CreateObject(NullHandler oReader)
		{
			ServiceInvoiceDetail oServiceInvoiceDetail = new ServiceInvoiceDetail();
			oServiceInvoiceDetail = MapObject(oReader);
			return oServiceInvoiceDetail;
		}

		private List<ServiceInvoiceDetail> CreateObjects(IDataReader oReader)
		{
			List<ServiceInvoiceDetail> oServiceInvoiceDetail = new List<ServiceInvoiceDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				ServiceInvoiceDetail oItem = CreateObject(oHandler);
				oServiceInvoiceDetail.Add(oItem);
			}
			return oServiceInvoiceDetail;
		}

		#endregion

		#region Interface implementation
		
        public ServiceInvoiceDetail Get(int id, Int64 nUserId)
			{
				ServiceInvoiceDetail oServiceInvoiceDetail = new ServiceInvoiceDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = ServiceInvoiceDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oServiceInvoiceDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get ServiceInvoiceDetail", e);
					#endregion
				}
				return oServiceInvoiceDetail;
			}

		public List<ServiceInvoiceDetail> Gets(int id, Int64 nUserID)
		{
			List<ServiceInvoiceDetail> oServiceInvoiceDetails = new List<ServiceInvoiceDetail>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = ServiceInvoiceDetailDA.Gets(tc, id);
				oServiceInvoiceDetails = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				ServiceInvoiceDetail oServiceInvoiceDetail = new ServiceInvoiceDetail();
				oServiceInvoiceDetail.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oServiceInvoiceDetails;
		}
        public List<ServiceInvoiceDetail> GetsLog(int id, Int64 nUserID)
        {
            List<ServiceInvoiceDetail> oServiceInvoiceDetails = new List<ServiceInvoiceDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ServiceInvoiceDetailDA.GetsLog(tc, id);
                oServiceInvoiceDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ServiceInvoiceDetail oServiceInvoiceDetail = new ServiceInvoiceDetail();
                oServiceInvoiceDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oServiceInvoiceDetails;
        }

	   public List<ServiceInvoiceDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<ServiceInvoiceDetail> oServiceInvoiceDetails = new List<ServiceInvoiceDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ServiceInvoiceDetailDA.Gets(tc, sSQL);
					oServiceInvoiceDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get ServiceInvoiceDetail", e);
					#endregion
				}
				return oServiceInvoiceDetails;
			}

		#endregion
	}

}

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
    public class ServiceInvoiceTermsService : MarshalByRefObject, IServiceInvoiceTermsService
	{
		#region Private functions and declaration

		private ServiceInvoiceTerms MapObject(NullHandler oReader)
		{
			ServiceInvoiceTerms oServiceInvoiceDetail = new ServiceInvoiceTerms();
            oServiceInvoiceDetail.ServiceInvoiceTermsID = oReader.GetInt32("ServiceInvoiceTermsID");
            oServiceInvoiceDetail.ServiceInvoiceID = oReader.GetInt32("ServiceInvoiceID");
            oServiceInvoiceDetail.ServiceInvoiceTermsLogID = oReader.GetInt32("ServiceInvoiceTermsLogID");
            oServiceInvoiceDetail.ServiceInvoiceLogID = oReader.GetInt32("ServiceInvoiceLogID");
            oServiceInvoiceDetail.Terms = oReader.GetString("Terms");
			return oServiceInvoiceDetail;
		}

		private ServiceInvoiceTerms CreateObject(NullHandler oReader)
		{
			ServiceInvoiceTerms oServiceInvoiceDetail = new ServiceInvoiceTerms();
			oServiceInvoiceDetail = MapObject(oReader);
			return oServiceInvoiceDetail;
		}

		private List<ServiceInvoiceTerms> CreateObjects(IDataReader oReader)
		{
			List<ServiceInvoiceTerms> oServiceInvoiceDetail = new List<ServiceInvoiceTerms>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				ServiceInvoiceTerms oItem = CreateObject(oHandler);
				oServiceInvoiceDetail.Add(oItem);
			}
			return oServiceInvoiceDetail;
		}

		#endregion

		#region Interface implementation
		
        public ServiceInvoiceTerms Get(int id, Int64 nUserId)
			{
				ServiceInvoiceTerms oServiceInvoiceDetail = new ServiceInvoiceTerms();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = ServiceInvoiceTermsDA.Get(tc, id);
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
					throw new ServiceException("Failed to Get ServiceInvoiceTerms", e);
					#endregion
				}
				return oServiceInvoiceDetail;
			}

		public List<ServiceInvoiceTerms> Gets(int id, Int64 nUserID)
		{
			List<ServiceInvoiceTerms> oServiceInvoiceDetails = new List<ServiceInvoiceTerms>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = ServiceInvoiceTermsDA.Gets(tc, id);
				oServiceInvoiceDetails = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				ServiceInvoiceTerms oServiceInvoiceDetail = new ServiceInvoiceTerms();
				oServiceInvoiceDetail.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oServiceInvoiceDetails;
		}
        public List<ServiceInvoiceTerms> GetsLog(int id, Int64 nUserID)
        {
            List<ServiceInvoiceTerms> oServiceInvoiceDetails = new List<ServiceInvoiceTerms>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ServiceInvoiceTermsDA.GetsLog(tc, id);
                oServiceInvoiceDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ServiceInvoiceTerms oServiceInvoiceDetail = new ServiceInvoiceTerms();
                oServiceInvoiceDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oServiceInvoiceDetails;
        }

	   public List<ServiceInvoiceTerms> Gets (string sSQL, Int64 nUserID)
			{
				List<ServiceInvoiceTerms> oServiceInvoiceDetails = new List<ServiceInvoiceTerms>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ServiceInvoiceTermsDA.Gets(tc, sSQL);
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
					throw new ServiceException("Failed to Get ServiceInvoiceTerms", e);
					#endregion
				}
				return oServiceInvoiceDetails;
			}

		#endregion
	}

}

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
	public class SalesQuotationDetailService : MarshalByRefObject, ISalesQuotationDetailService
	{
		#region Private functions and declaration

		private SalesQuotationDetail MapObject(NullHandler oReader)
		{
			SalesQuotationDetail oSalesQuotationDetail = new SalesQuotationDetail();
			oSalesQuotationDetail.SalesQuotationDetailID = oReader.GetInt32("SalesQuotationDetailID");
            oSalesQuotationDetail.SalesQuotationID = oReader.GetInt32("SalesQuotationID");
            oSalesQuotationDetail.FeatureID = oReader.GetInt32("FeatureID");
            oSalesQuotationDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oSalesQuotationDetail.FeatureType =  (EnumFeatureType)oReader.GetInt32("FeatureType");
            oSalesQuotationDetail.FeatureTypeInInt = oReader.GetInt32("FeatureType");
            oSalesQuotationDetail.FeatureCode = oReader.GetString("FeatureCode");
            oSalesQuotationDetail.FeatureName = oReader.GetString("FeatureName");
            oSalesQuotationDetail.CurrencyName = oReader.GetString("CurrencyName");
            oSalesQuotationDetail.CurrencySymbol = oReader.GetString("Symbol");
            oSalesQuotationDetail.Remarks = oReader.GetString("Remarks");
            oSalesQuotationDetail.Price = oReader.GetDouble("Price");
			return oSalesQuotationDetail;
		}

		private SalesQuotationDetail CreateObject(NullHandler oReader)
		{
			SalesQuotationDetail oSalesQuotationDetail = new SalesQuotationDetail();
			oSalesQuotationDetail = MapObject(oReader);
			return oSalesQuotationDetail;
		}

		private List<SalesQuotationDetail> CreateObjects(IDataReader oReader)
		{
			List<SalesQuotationDetail> oSalesQuotationDetail = new List<SalesQuotationDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				SalesQuotationDetail oItem = CreateObject(oHandler);
				oSalesQuotationDetail.Add(oItem);
			}
			return oSalesQuotationDetail;
		}

		#endregion

		#region Interface implementation
		
        public SalesQuotationDetail Get(int id, Int64 nUserId)
			{
				SalesQuotationDetail oSalesQuotationDetail = new SalesQuotationDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = SalesQuotationDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oSalesQuotationDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get SalesQuotationDetail", e);
					#endregion
				}
				return oSalesQuotationDetail;
			}

		public List<SalesQuotationDetail> Gets(int id, Int64 nUserID)
		{
			List<SalesQuotationDetail> oSalesQuotationDetails = new List<SalesQuotationDetail>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = SalesQuotationDetailDA.Gets(tc, id);
				oSalesQuotationDetails = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				SalesQuotationDetail oSalesQuotationDetail = new SalesQuotationDetail();
				oSalesQuotationDetail.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oSalesQuotationDetails;
		}

	   public List<SalesQuotationDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<SalesQuotationDetail> oSalesQuotationDetails = new List<SalesQuotationDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = SalesQuotationDetailDA.Gets(tc, sSQL);
					oSalesQuotationDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get SalesQuotationDetail", e);
					#endregion
				}
				return oSalesQuotationDetails;
			}

		#endregion
	}

}

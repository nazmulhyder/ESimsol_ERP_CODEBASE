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
	public class KommFileDetailService : MarshalByRefObject, IKommFileDetailService
	{
		#region Private functions and declaration

		private KommFileDetail MapObject(NullHandler oReader)
		{
			KommFileDetail oKommFileDetail = new KommFileDetail();
			oKommFileDetail.KommFileDetailID = oReader.GetInt32("KommFileDetailID");
            oKommFileDetail.KommFileID = oReader.GetInt32("KommFileID");
            oKommFileDetail.FeatureID = oReader.GetInt32("FeatureID");
            oKommFileDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oKommFileDetail.FeatureType =  (EnumFeatureType)oReader.GetInt32("FeatureType");
            oKommFileDetail.FeatureTypeInInt = oReader.GetInt32("FeatureType");
            oKommFileDetail.FeatureCode = oReader.GetString("FeatureCode");
            oKommFileDetail.FeatureName = oReader.GetString("FeatureName");
            oKommFileDetail.CurrencyName = oReader.GetString("CurrencyName");
            oKommFileDetail.CurrencySymbol = oReader.GetString("Symbol");
            oKommFileDetail.Remarks = oReader.GetString("Remarks");
            oKommFileDetail.Price = oReader.GetDouble("Price");
			return oKommFileDetail;
		}

		private KommFileDetail CreateObject(NullHandler oReader)
		{
			KommFileDetail oKommFileDetail = new KommFileDetail();
			oKommFileDetail = MapObject(oReader);
			return oKommFileDetail;
		}

		private List<KommFileDetail> CreateObjects(IDataReader oReader)
		{
			List<KommFileDetail> oKommFileDetail = new List<KommFileDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KommFileDetail oItem = CreateObject(oHandler);
				oKommFileDetail.Add(oItem);
			}
			return oKommFileDetail;
		}

		#endregion

		#region Interface implementation
		
        public KommFileDetail Get(int id, Int64 nUserId)
			{
				KommFileDetail oKommFileDetail = new KommFileDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = KommFileDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKommFileDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KommFileDetail", e);
					#endregion
				}
				return oKommFileDetail;
			}

		public List<KommFileDetail> Gets(int id, Int64 nUserID)
		{
			List<KommFileDetail> oKommFileDetails = new List<KommFileDetail>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = KommFileDetailDA.Gets(tc, id);
				oKommFileDetails = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				KommFileDetail oKommFileDetail = new KommFileDetail();
				oKommFileDetail.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oKommFileDetails;
		}

	   public List<KommFileDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<KommFileDetail> oKommFileDetails = new List<KommFileDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KommFileDetailDA.Gets(tc, sSQL);
					oKommFileDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get KommFileDetail", e);
					#endregion
				}
				return oKommFileDetails;
			}

		#endregion
	}

}

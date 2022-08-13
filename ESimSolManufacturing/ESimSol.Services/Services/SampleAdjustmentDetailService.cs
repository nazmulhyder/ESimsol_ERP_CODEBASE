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
	public class SampleAdjustmentDetailService : MarshalByRefObject, ISampleAdjustmentDetailService
	{
		#region Private functions and declaration

		private SampleAdjustmentDetail MapObject(NullHandler oReader)
		{
			SampleAdjustmentDetail oSampleAdjustmentDetail = new SampleAdjustmentDetail();
			oSampleAdjustmentDetail.SampleAdjustmentID = oReader.GetInt32("SampleAdjustmentID");
			oSampleAdjustmentDetail.SampleAdjustmentDetailID = oReader.GetInt32("SampleAdjustmentDetailID");
			oSampleAdjustmentDetail.SampleInvoiceID = oReader.GetInt32("SampleInvoiceID");
			oSampleAdjustmentDetail.AdjustAmount = oReader.GetDouble("AdjustAmount");
			oSampleAdjustmentDetail.Remarks = oReader.GetString("Remarks");
			oSampleAdjustmentDetail.InvoiceNo = oReader.GetString("InvoiceNo");
            oSampleAdjustmentDetail.RefAmount = oReader.GetDouble("RefAmount");
            oSampleAdjustmentDetail.SampleInvoiceDate = oReader.GetDateTime("SampleInvoiceDate");

			return oSampleAdjustmentDetail;
		}

		private SampleAdjustmentDetail CreateObject(NullHandler oReader)
		{
			SampleAdjustmentDetail oSampleAdjustmentDetail = new SampleAdjustmentDetail();
			oSampleAdjustmentDetail = MapObject(oReader);
			return oSampleAdjustmentDetail;
		}

		private List<SampleAdjustmentDetail> CreateObjects(IDataReader oReader)
		{
			List<SampleAdjustmentDetail> oSampleAdjustmentDetail = new List<SampleAdjustmentDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				SampleAdjustmentDetail oItem = CreateObject(oHandler);
				oSampleAdjustmentDetail.Add(oItem);
			}
			return oSampleAdjustmentDetail;
		}

		#endregion

		#region Interface implementation
        public SampleAdjustmentDetail Get(int id, Int64 nUserId)
			{
				SampleAdjustmentDetail oSampleAdjustmentDetail = new SampleAdjustmentDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = SampleAdjustmentDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oSampleAdjustmentDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get SampleAdjustmentDetail", e);
					#endregion
				}
				return oSampleAdjustmentDetail;
			}
		public List<SampleAdjustmentDetail> Gets(int nID, Int64 nUserID)
			{
				List<SampleAdjustmentDetail> oSampleAdjustmentDetails = new List<SampleAdjustmentDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = SampleAdjustmentDetailDA.Gets(nID,tc);
					oSampleAdjustmentDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					SampleAdjustmentDetail oSampleAdjustmentDetail = new SampleAdjustmentDetail();
					oSampleAdjustmentDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oSampleAdjustmentDetails;
			}
		public List<SampleAdjustmentDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<SampleAdjustmentDetail> oSampleAdjustmentDetails = new List<SampleAdjustmentDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = SampleAdjustmentDetailDA.Gets(tc, sSQL);
					oSampleAdjustmentDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get SampleAdjustmentDetail", e);
					#endregion
				}
				return oSampleAdjustmentDetails;
			}

		#endregion
	}

}

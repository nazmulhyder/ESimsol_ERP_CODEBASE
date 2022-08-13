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
	public class CommercialBSDetailService : MarshalByRefObject, ICommercialBSDetailService
	{
		#region Private functions and declaration

		private CommercialBSDetail MapObject(NullHandler oReader)
		{
			CommercialBSDetail oCommercialBSDetail = new CommercialBSDetail();
			oCommercialBSDetail.CommercialBSDetailID = oReader.GetInt32("CommercialBSDetailID");
			oCommercialBSDetail.CommercialBSID = oReader.GetInt32("CommercialBSID");
			oCommercialBSDetail.CommercialInvoiceID = oReader.GetInt32("CommercialInvoiceID");
			oCommercialBSDetail.InvoiceAmount = oReader.GetDouble("InvoiceAmount");
			oCommercialBSDetail.Remarks = oReader.GetString("Remarks");
			oCommercialBSDetail.InvoiceNo = oReader.GetString("InvoiceNo");
            oCommercialBSDetail.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oCommercialBSDetail.ShipmentMode = (EnumTransportType)oReader.GetInt32("ShipmentMode");
			return oCommercialBSDetail;
		}

		private CommercialBSDetail CreateObject(NullHandler oReader)
		{
			CommercialBSDetail oCommercialBSDetail = new CommercialBSDetail();
			oCommercialBSDetail = MapObject(oReader);
			return oCommercialBSDetail;
		}

		private List<CommercialBSDetail> CreateObjects(IDataReader oReader)
		{
			List<CommercialBSDetail> oCommercialBSDetail = new List<CommercialBSDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				CommercialBSDetail oItem = CreateObject(oHandler);
				oCommercialBSDetail.Add(oItem);
			}
			return oCommercialBSDetail;
		}

		#endregion

		#region Interface implementation
		
			public CommercialBSDetail Get(int id, Int64 nUserId)
			{
				CommercialBSDetail oCommercialBSDetail = new CommercialBSDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = CommercialBSDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oCommercialBSDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get CommercialBSDetail", e);
					#endregion
				}
				return oCommercialBSDetail;
			}

			public List<CommercialBSDetail> Gets(int CommercialBSID, Int64 nUserID)
			{
				List<CommercialBSDetail> oCommercialBSDetails = new List<CommercialBSDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CommercialBSDetailDA.Gets(CommercialBSID, tc);
					oCommercialBSDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					CommercialBSDetail oCommercialBSDetail = new CommercialBSDetail();
					oCommercialBSDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oCommercialBSDetails;
			}

			public List<CommercialBSDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<CommercialBSDetail> oCommercialBSDetails = new List<CommercialBSDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CommercialBSDetailDA.Gets(tc, sSQL);
					oCommercialBSDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get CommercialBSDetail", e);
					#endregion
				}
				return oCommercialBSDetails;
			}

		#endregion
	}

}

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
	public class CommercialFDBPDetailService : MarshalByRefObject, ICommercialFDBPDetailService
	{
		#region Private functions and declaration

		private CommercialFDBPDetail MapObject(NullHandler oReader)
		{
			CommercialFDBPDetail oCommercialFDBPDetail = new CommercialFDBPDetail();
			oCommercialFDBPDetail.CommercialFDBPDetailID = oReader.GetInt32("CommercialFDBPDetailID");
			oCommercialFDBPDetail.CommercialFDBPID = oReader.GetInt32("CommercialFDBPID");
			oCommercialFDBPDetail.BankAccountID = oReader.GetInt32("BankAccountID");
			oCommercialFDBPDetail.AmountInCurrency = oReader.GetDouble("AmountInCurrency");
			oCommercialFDBPDetail.CRate = oReader.GetDouble("CRate");
			oCommercialFDBPDetail.AmountBC = oReader.GetDouble("AmountBC");
			oCommercialFDBPDetail.Remarks = oReader.GetString("Remarks");
			oCommercialFDBPDetail.BankName = oReader.GetString("BankName");
			oCommercialFDBPDetail.BankAccountNo = oReader.GetString("BankAccountNo");
			return oCommercialFDBPDetail;
		}

		private CommercialFDBPDetail CreateObject(NullHandler oReader)
		{
			CommercialFDBPDetail oCommercialFDBPDetail = new CommercialFDBPDetail();
			oCommercialFDBPDetail = MapObject(oReader);
			return oCommercialFDBPDetail;
		}

		private List<CommercialFDBPDetail> CreateObjects(IDataReader oReader)
		{
			List<CommercialFDBPDetail> oCommercialFDBPDetail = new List<CommercialFDBPDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				CommercialFDBPDetail oItem = CreateObject(oHandler);
				oCommercialFDBPDetail.Add(oItem);
			}
			return oCommercialFDBPDetail;
		}

		#endregion

		#region Interface implementation
		
	
			public List<CommercialFDBPDetail> Gets(int CommercialFDBPID, Int64 nUserID)
			{
				List<CommercialFDBPDetail> oCommercialFDBPDetails = new List<CommercialFDBPDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = CommercialFDBPDetailDA.Gets(tc, CommercialFDBPID);
					oCommercialFDBPDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					CommercialFDBPDetail oCommercialFDBPDetail = new CommercialFDBPDetail();
					oCommercialFDBPDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oCommercialFDBPDetails;
			}

			public List<CommercialFDBPDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<CommercialFDBPDetail> oCommercialFDBPDetails = new List<CommercialFDBPDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CommercialFDBPDetailDA.Gets(tc, sSQL);
					oCommercialFDBPDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get CommercialFDBPDetail", e);
					#endregion
				}
				return oCommercialFDBPDetails;
			}

		#endregion
	}

}

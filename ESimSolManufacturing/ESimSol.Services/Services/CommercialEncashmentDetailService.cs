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
	public class CommercialEncashmentDetailService : MarshalByRefObject, ICommercialEncashmentDetailService
	{
		#region Private functions and declaration

		private CommercialEncashmentDetail MapObject(NullHandler oReader)
		{
			CommercialEncashmentDetail oCommercialEncashmentDetail = new CommercialEncashmentDetail();
			oCommercialEncashmentDetail.CommercialEncashmentDetailID = oReader.GetInt32("CommercialEncashmentDetailID");
			oCommercialEncashmentDetail.CommercialEncashmentID = oReader.GetInt32("CommercialEncashmentID");
			oCommercialEncashmentDetail.BankAccountID = oReader.GetInt32("BankAccountID");
			oCommercialEncashmentDetail.BankAccountNo = oReader.GetString("BankAccountNo");
			oCommercialEncashmentDetail.ExpenditureHeadID = oReader.GetInt32("ExpenditureHeadID");
			oCommercialEncashmentDetail.ExpenditureHeadName = oReader.GetString("ExpenditureHeadName");
			oCommercialEncashmentDetail.CurrencyID = oReader.GetInt32("CurrencyID");
			oCommercialEncashmentDetail.AmountInCurrency = oReader.GetDouble("AmountInCurrency");
			oCommercialEncashmentDetail.CRate = oReader.GetDouble("CRate");
			oCommercialEncashmentDetail.AmountBC = oReader.GetDouble("AmountBC");
			oCommercialEncashmentDetail.CurrencySymbol = oReader.GetString("CurrencySymbol");
			return oCommercialEncashmentDetail;
		}

		private CommercialEncashmentDetail CreateObject(NullHandler oReader)
		{
			CommercialEncashmentDetail oCommercialEncashmentDetail = new CommercialEncashmentDetail();
			oCommercialEncashmentDetail = MapObject(oReader);
			return oCommercialEncashmentDetail;
		}

		private List<CommercialEncashmentDetail> CreateObjects(IDataReader oReader)
		{
			List<CommercialEncashmentDetail> oCommercialEncashmentDetail = new List<CommercialEncashmentDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				CommercialEncashmentDetail oItem = CreateObject(oHandler);
				oCommercialEncashmentDetail.Add(oItem);
			}
			return oCommercialEncashmentDetail;
		}

		#endregion

		#region Interface implementation
			public List<CommercialEncashmentDetail> Gets(int CommercialEncashmentID, Int64 nUserID)
			{
				List<CommercialEncashmentDetail> oCommercialEncashmentDetails = new List<CommercialEncashmentDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = CommercialEncashmentDetailDA.Gets(tc, CommercialEncashmentID);
					oCommercialEncashmentDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					CommercialEncashmentDetail oCommercialEncashmentDetail = new CommercialEncashmentDetail();
					oCommercialEncashmentDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oCommercialEncashmentDetails;
			}
			public List<CommercialEncashmentDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<CommercialEncashmentDetail> oCommercialEncashmentDetails = new List<CommercialEncashmentDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CommercialEncashmentDetailDA.Gets(tc, sSQL);
					oCommercialEncashmentDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get CommercialEncashmentDetail", e);
					#endregion
				}
				return oCommercialEncashmentDetails;
			}
		#endregion
	}

}

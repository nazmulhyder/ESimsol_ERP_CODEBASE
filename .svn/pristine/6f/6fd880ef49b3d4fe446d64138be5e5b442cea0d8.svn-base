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
	public class PaymentDetailService : MarshalByRefObject, IPaymentDetailService
	{
		#region Private functions and declaration

		private PaymentDetail MapObject(NullHandler oReader)
		{
			PaymentDetail oPaymentDetail = new PaymentDetail();
            oPaymentDetail.PaymentDetailID = oReader.GetInt32("PaymentDetailID");
            oPaymentDetail.PaymentID = oReader.GetInt32("PaymentID");
            oPaymentDetail.ReferenceID = oReader.GetInt32("ReferenceID");
            oPaymentDetail.ReferenceType =(EnumSampleInvoiceType) oReader.GetInt32("ReferenceType");
            oPaymentDetail.ReferenceTypeInInt = oReader.GetInt32("ReferenceType");
            oPaymentDetail.PaymentAmount = oReader.GetDouble("Amount");
            oPaymentDetail.AmountInCurrency = oReader.GetDouble("AmountInCurrency");
            oPaymentDetail.CRate = oReader.GetDouble("CRate");
            oPaymentDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oPaymentDetail.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oPaymentDetail.AmountWithExchangeRate = oReader.GetDouble("AmountWithExchangeRate");
            oPaymentDetail.DisCount = oReader.GetDouble("DisCount");
            oPaymentDetail.SampleInvoiceNo = oReader.GetString("SampleInvoiceNo");
            oPaymentDetail.InvoiceType = oReader.GetInt32("InvoiceType");
            oPaymentDetail.PaymentAmount = oReader.GetDouble("PaymentAmount");
            oPaymentDetail.AlreadyPaid = oReader.GetDouble("AlreadyPaid");
            oPaymentDetail.AlreadyAdditionalAmount = oReader.GetDouble("AlreadyAdditionalAmount");
            oPaymentDetail.AlreadyDiscount = oReader.GetDouble("AlreadyDiscount");
            oPaymentDetail.YetToPayment = oReader.GetDouble("YetToPayment");
            oPaymentDetail.ExchangeCurrencyID = oReader.GetInt32("ExchangeCurrencyID");
            oPaymentDetail.RateUnit = oReader.GetInt32("RateUnit");
            oPaymentDetail.Note = oReader.GetString("Note");
            oPaymentDetail.ExchangeCurrencySymbol = oReader.GetString("ExchangeCurrencySymbol");
            oPaymentDetail.SampleInvoiceDate = oReader.GetDateTime("SampleInvoiceDate");
            oPaymentDetail.MRDate = oReader.GetDateTime("MRDate");
            oPaymentDetail.MRNo = oReader.GetString("MRNo");
            oPaymentDetail.MKTEmpID = oReader.GetInt32("MKTEmpID");
            oPaymentDetail.MKTEmpName = oReader.GetString("MKTEmpName");
            oPaymentDetail.ContractorID = oReader.GetInt32("ContractorID");
            oPaymentDetail.ContractorName = oReader.GetString("ContractorName");
            oPaymentDetail.AdditionalAmount = oReader.GetDouble("AdditionalAmount");
			return oPaymentDetail;
		}

		private PaymentDetail CreateObject(NullHandler oReader)
		{
			PaymentDetail oPaymentDetail = new PaymentDetail();
			oPaymentDetail = MapObject(oReader);
			return oPaymentDetail;
		}

		private List<PaymentDetail> CreateObjects(IDataReader oReader)
		{
			List<PaymentDetail> oPaymentDetail = new List<PaymentDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				PaymentDetail oItem = CreateObject(oHandler);
				oPaymentDetail.Add(oItem);
			}
			return oPaymentDetail;
		}

		#endregion

		#region Interface implementation
			public PaymentDetail Save(PaymentDetail oPaymentDetail, Int64 nUserID)
			{
				TransactionContext tc = null;
               
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oPaymentDetail.PaymentDetailID <= 0)
					{
                        
						reader = PaymentDetailDA.InsertUpdate(tc, oPaymentDetail, EnumDBOperation.Insert, nUserID,"");
					}
					else{
                      
						reader = PaymentDetailDA.InsertUpdate(tc, oPaymentDetail, EnumDBOperation.Update, nUserID,"");
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oPaymentDetail = new PaymentDetail();
						oPaymentDetail = CreateObject(oReader);
					}
					reader.Close();
                  

					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					{
						tc.HandleError();
						oPaymentDetail = new PaymentDetail();
						oPaymentDetail.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oPaymentDetail;
			}

            public string Delete(PaymentDetail oPaymentDetail, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					
					PaymentDetailDA.Delete(tc, oPaymentDetail, EnumDBOperation.Delete, nUserId,"");
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exceptionif (tc != null)
					tc.HandleError();
					return e.Message.Split('~')[0];
					#endregion
				}
				return Global.DeleteMessage;
			}
			public PaymentDetail Get(int id, Int64 nUserId)
			{
				PaymentDetail oPaymentDetail = new PaymentDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = PaymentDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oPaymentDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get PaymentDetail", e);
					#endregion
				}
				return oPaymentDetail;
			}
			public List<PaymentDetail> Gets(int nPaymentID,Int64 nUserID)
			{
				List<PaymentDetail> oPaymentDetails = new List<PaymentDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = PaymentDetailDA.Gets(nPaymentID,tc);
					oPaymentDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					PaymentDetail oPaymentDetail = new PaymentDetail();
					oPaymentDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oPaymentDetails;
			}
			public List<PaymentDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<PaymentDetail> oPaymentDetails = new List<PaymentDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PaymentDetailDA.Gets(tc, sSQL);
					oPaymentDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get PaymentDetail", e);
					#endregion
				}
				return oPaymentDetails;
			}
          

		#endregion
	}

}

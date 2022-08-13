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
	public class CommercialEncashmentService : MarshalByRefObject, ICommercialEncashmentService
	{
		#region Private functions and declaration

		private CommercialEncashment MapObject(NullHandler oReader)
		{
			CommercialEncashment oCommercialEncashment = new CommercialEncashment();
			oCommercialEncashment.CommercialEncashmentID = oReader.GetInt32("CommercialEncashmentID");
			oCommercialEncashment.CommercialBSID = oReader.GetInt32("CommercialBSID");
			oCommercialEncashment.EncashmentDate = oReader.GetDateTime("EncashmentDate");
			oCommercialEncashment.ApprovedBy = oReader.GetInt32("ApprovedBy");
			oCommercialEncashment.ApprovedByName = oReader.GetString("ApprovedByName");
			oCommercialEncashment.AmountInCurrency = oReader.GetDouble("AmountInCurrency");
			oCommercialEncashment.AmountBC = oReader.GetDouble("AmountBC");
			oCommercialEncashment.EncashRate = oReader.GetDouble("EncashRate");
			oCommercialEncashment.OverDueInCurrency = oReader.GetDouble("OverDueInCurrency");
			oCommercialEncashment.OverDueBC = oReader.GetDouble("OverDueBC");
			oCommercialEncashment.GainLossCurrencyID = oReader.GetInt32("GainLossCurrencyID");
            oCommercialEncashment.GainLossCurrencySymbol = oReader.GetString("GainLossCurrencySymbol");
			oCommercialEncashment.GainLossAmount = oReader.GetDouble("GainLossAmount");
			oCommercialEncashment.IsGain = oReader.GetBoolean("IsGain");
			oCommercialEncashment.PDeductionInCurrency = oReader.GetDouble("PDeductionInCurrency");
			oCommercialEncashment.PDeductionBC = oReader.GetDouble("PDeductionBC");
			oCommercialEncashment.Remarks = oReader.GetString("Remarks");
			oCommercialEncashment.MasterLCNo = oReader.GetString("MasterLCNo");
            oCommercialEncashment.BankID = oReader.GetInt32("BankID");
			oCommercialEncashment.BankRefNo = oReader.GetString("BankRefNo");
			oCommercialEncashment.FDBPNo = oReader.GetString("FDBPNo");
			oCommercialEncashment.BuyerName = oReader.GetString("BuyerName");
			oCommercialEncashment.BankName = oReader.GetString("BankName");
            oCommercialEncashment.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oCommercialEncashment.BCurrencySymbol = oReader.GetString("BCurrencySymbol");
			oCommercialEncashment.BSAmount = oReader.GetDouble("BSAmount");
			oCommercialEncashment.BSAmountBC = oReader.GetDouble("BSAmountBC");
			oCommercialEncashment.CRate = oReader.GetDouble("CRate");
			oCommercialEncashment.LCValue = oReader.GetDouble("LCValue");
			oCommercialEncashment.PurchaseValue = oReader.GetDouble("PurchaseValue");
			oCommercialEncashment.BankCharge = oReader.GetDouble("BankCharge");
            oCommercialEncashment.BankChargeBC = oReader.GetDouble("BankChargeBC");
            oCommercialEncashment.PurchaseAmountBC = oReader.GetDouble("PurchaseAmountBC");
            oCommercialEncashment.RemainingBalance = oReader.GetDouble("RemainingBalance");
            oCommercialEncashment.RemainingBalanceBC = oReader.GetDouble("RemainingBalanceBC");
            oCommercialEncashment.LCCurrencyID = oReader.GetInt32("LCCurrencyID");
            
            oCommercialEncashment.Balance = oReader.GetDouble("Balance");
			oCommercialEncashment.BSIssueDate = oReader.GetDateTime("BSIssueDate");
			oCommercialEncashment.SubmissionDate = oReader.GetDateTime("SubmissionDate");
			oCommercialEncashment.FDBPReceiveDate = oReader.GetDateTime("FDBPReceiveDate");
			oCommercialEncashment.MaturityDate = oReader.GetDateTime("MaturityDate");
			oCommercialEncashment.RealizationDate = oReader.GetDateTime("RealizationDate");
			return oCommercialEncashment;
		}

		private CommercialEncashment CreateObject(NullHandler oReader)
		{
			CommercialEncashment oCommercialEncashment = new CommercialEncashment();
			oCommercialEncashment = MapObject(oReader);
			return oCommercialEncashment;
		}

		private List<CommercialEncashment> CreateObjects(IDataReader oReader)
		{
			List<CommercialEncashment> oCommercialEncashment = new List<CommercialEncashment>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				CommercialEncashment oItem = CreateObject(oHandler);
				oCommercialEncashment.Add(oItem);
			}
			return oCommercialEncashment;
		}

		#endregion

		#region Interface implementation
        public CommercialEncashment Save(CommercialEncashment oCommercialEncashment, Int64 nUserID)
        {
            List<CommercialEncashmentDetail> oCommercialEncashmentDetails = new List<CommercialEncashmentDetail>();
            CommercialEncashmentDetail oCommercialEncashmentDetail = new CommercialEncashmentDetail();
            oCommercialEncashmentDetails = oCommercialEncashment.CommercialEncashmentDetails;

            string sCommercialEncashmentDetailIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region CBS
                IDataReader reader;
                if (oCommercialEncashment.CommercialEncashmentID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CommercialEncashment, EnumRoleOperationType.Add);
                    reader = CommercialEncashmentDA.InsertUpdate(tc, oCommercialEncashment, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CommercialEncashment, EnumRoleOperationType.Edit);
                    reader = CommercialEncashmentDA.InsertUpdate(tc, oCommercialEncashment, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommercialEncashment = new CommercialEncashment();
                    oCommercialEncashment = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                #region CommercialEncashment Detail Part
                if (oCommercialEncashmentDetails != null)
                {
                    foreach (CommercialEncashmentDetail oItem in oCommercialEncashmentDetails)
                    {
                        IDataReader readerdetail;
                        oItem.CommercialEncashmentID = oCommercialEncashment.CommercialEncashmentID;
                        if (oItem.CommercialEncashmentDetailID <= 0)
                        {
                            readerdetail = CommercialEncashmentDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = CommercialEncashmentDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sCommercialEncashmentDetailIDs = sCommercialEncashmentDetailIDs + oReaderDetail.GetString("CommercialEncashmentDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sCommercialEncashmentDetailIDs.Length > 0)
                    {
                        sCommercialEncashmentDetailIDs = sCommercialEncashmentDetailIDs.Remove(sCommercialEncashmentDetailIDs.Length - 1, 1);
                    }
                    oCommercialEncashmentDetail = new CommercialEncashmentDetail();
                    oCommercialEncashmentDetail.CommercialEncashmentID = oCommercialEncashment.CommercialEncashmentID;
                    CommercialEncashmentDetailDA.Delete(tc, oCommercialEncashmentDetail, EnumDBOperation.Delete, nUserID, sCommercialEncashmentDetailIDs);

                }

                #endregion
                #region CommercialEncashment Get
                reader = CommercialEncashmentDA.Get(tc, oCommercialEncashment.CommercialEncashmentID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommercialEncashment = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oCommercialEncashment = new CommercialEncashment();
                    oCommercialEncashment.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oCommercialEncashment;
        }

            public CommercialEncashment Approve(CommercialEncashment oCommercialEncashment, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                     AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CommercialEncashment, EnumRoleOperationType.Approved);
                     IDataReader reader = CommercialEncashmentDA.InsertUpdate(tc, oCommercialEncashment, EnumDBOperation.Approval, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oCommercialEncashment = new CommercialEncashment();
                        oCommercialEncashment = CreateObject(oReader);
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
                        oCommercialEncashment = new CommercialEncashment();
                        oCommercialEncashment.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oCommercialEncashment;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					CommercialEncashment oCommercialEncashment = new CommercialEncashment();
					oCommercialEncashment.CommercialEncashmentID = id;
					AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.CommercialEncashment, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "CommercialEncashment", id);
					CommercialEncashmentDA.Delete(tc, oCommercialEncashment, EnumDBOperation.Delete, nUserId);
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exceptionif (tc != null)
					tc.HandleError();
					return e.Message.Split('!')[0];
					#endregion
				}
				return Global.DeleteMessage;
			}

			public CommercialEncashment Get(int id, Int64 nUserId)
			{
				CommercialEncashment oCommercialEncashment = new CommercialEncashment();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = CommercialEncashmentDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oCommercialEncashment = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get CommercialEncashment", e);
					#endregion
				}
				return oCommercialEncashment;
			}

		
			public List<CommercialEncashment> Gets (string sSQL, Int64 nUserID)
			{
				List<CommercialEncashment> oCommercialEncashments = new List<CommercialEncashment>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CommercialEncashmentDA.Gets(tc, sSQL);
					oCommercialEncashments = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get CommercialEncashment", e);
					#endregion
				}
				return oCommercialEncashments;
			}

		#endregion
	}

}

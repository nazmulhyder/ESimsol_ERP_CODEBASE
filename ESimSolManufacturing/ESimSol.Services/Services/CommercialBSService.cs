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
	public class CommercialBSService : MarshalByRefObject, ICommercialBSService
	{
		#region Private functions and declaration

		private CommercialBS MapObject(NullHandler oReader)
		{
			CommercialBS oCommercialBS = new CommercialBS();
			oCommercialBS.CommercialBSID = oReader.GetInt32("CommercialBSID");
			oCommercialBS.MasterLCID = oReader.GetInt32("MasterLCID");
			oCommercialBS.RefNo = oReader.GetString("RefNo");
            oCommercialBS.CurrencySymbol = oReader.GetString("CurrencySymbol");
			oCommercialBS.BUID = oReader.GetInt32("BUID");
			oCommercialBS.BuyerID = oReader.GetInt32("BuyerID");
			oCommercialBS.BSStatus = (EnumCommercialBSStatus) oReader.GetInt32("BSStatus");
            oCommercialBS.BSStatusInInt =oReader.GetInt32("BSStatus");
			oCommercialBS.BankID = oReader.GetInt32("BankID");
			oCommercialBS.BSAmount = oReader.GetDouble("BSAmount");
			oCommercialBS.Remarks = oReader.GetString("Remarks");

            oCommercialBS.IssueDate = oReader.GetDateTime("IssueDate");
			oCommercialBS.SubmissionDate = oReader.GetDateTime("SubmissionDate");
			oCommercialBS.SubmissionRemarks = oReader.GetString("SubmissionRemarks");
			oCommercialBS.FDBPNo = oReader.GetString("FDBPNo");
			oCommercialBS.BankRefNo = oReader.GetString("BankRefNo");
			oCommercialBS.FDBPReceiveDate = oReader.GetDateTime("FDBPReceiveDate");
			oCommercialBS.FDBPRemarks = oReader.GetString("FDBPRemarks");
			oCommercialBS.MaturityRcvDate = oReader.GetDateTime("MaturityRcvDate");
			oCommercialBS.MaturityDate = oReader.GetDateTime("MaturityDate");
			oCommercialBS.MaturityRemarks = oReader.GetString("MaturityRemarks");
			oCommercialBS.RealizationDate = oReader.GetDateTime("RealizationDate");
			oCommercialBS.RealizationRemarks = oReader.GetString("RealizationRemarks");
			oCommercialBS.BankName = oReader.GetString("BankName");
			oCommercialBS.BuyerName = oReader.GetString("BuyerName");
			oCommercialBS.BUName = oReader.GetString("BUName");
			oCommercialBS.MasterLCNo = oReader.GetString("MasterLCNo");
			oCommercialBS.BankBranchID = oReader.GetInt32("BankBranchID");
            oCommercialBS.CommercialFDBPID = oReader.GetInt32("CommercialFDBPID");
            oCommercialBS.CommercialEncashmentID = oReader.GetInt32("CommercialEncashmentID");
            oCommercialBS.AdviceBankID = oReader.GetInt32("AdviceBankID");
            oCommercialBS.CurrencyID = oReader.GetInt32("CurrencyID");
			oCommercialBS.BankBranchName = oReader.GetString("BankBranchName");
            oCommercialBS.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oCommercialBS.ApprovedByName = oReader.GetString("ApprovedByName");
            oCommercialBS.BCurrencyID = oReader.GetInt32("BCurrencyID");
            oCommercialBS.BSAmountBC = oReader.GetDouble("BSAmountBC");
            oCommercialBS.CRate = oReader.GetDouble("CRate");
            oCommercialBS.BCurrencySymbol = oReader.GetString("BCurrencySymbol");
            oCommercialBS.LCValue = oReader.GetDouble("LCValue");
            oCommercialBS.BankCharge = oReader.GetDouble("BankCharge");
            oCommercialBS.PurchaseAmount = oReader.GetDouble("PurchaseAmount");
            oCommercialBS.BankChargeBC = oReader.GetDouble("BankChargeBC");
            oCommercialBS.PurchaseAmountBC = oReader.GetDouble("PurchaseAmountBC");

            oCommercialBS.PurchseApproveBy = oReader.GetInt32("PurchseApproveBy");
            oCommercialBS.EncashApproveBy = oReader.GetInt32("EncashApproveBy");
            oCommercialBS.PurchseApproveByName = oReader.GetString("PurchseApproveByName");
            oCommercialBS.EncashApproveByName = oReader.GetString("EncashApproveByName");
			return oCommercialBS;
		}

		private CommercialBS CreateObject(NullHandler oReader)
		{
			CommercialBS oCommercialBS = new CommercialBS();
			oCommercialBS = MapObject(oReader);
			return oCommercialBS;
		}

		private List<CommercialBS> CreateObjects(IDataReader oReader)
		{
			List<CommercialBS> oCommercialBS = new List<CommercialBS>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				CommercialBS oItem = CreateObject(oHandler);
				oCommercialBS.Add(oItem);
			}
			return oCommercialBS;
		}

		#endregion

		#region Interface implementation
			public CommercialBS Save(CommercialBS oCommercialBS, Int64 nUserID)
			{
                List<CommercialBSDetail> oCommercialBSDetails = new List<CommercialBSDetail>();
                CommercialBSDetail oCommercialBSDetail = new CommercialBSDetail();
                oCommercialBSDetails = oCommercialBS.CommercialBSDetails;
                
                string sCommercialBSDetailIDs = "";
                TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
                    #region CBS
                    IDataReader reader;
					if (oCommercialBS.CommercialBSID <= 0)
					{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CommercialBS, EnumRoleOperationType.Add);
						reader = CommercialBSDA.InsertUpdate(tc, oCommercialBS, EnumDBOperation.Insert, nUserID);
					}
					else{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CommercialBS, EnumRoleOperationType.Edit);
						reader = CommercialBSDA.InsertUpdate(tc, oCommercialBS, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oCommercialBS = new CommercialBS();
						oCommercialBS = CreateObject(oReader);
					}
					reader.Close();
                    #endregion
                    #region CommercialBS Detail Part
                    if (oCommercialBSDetails != null)
                    {
                        foreach (CommercialBSDetail oItem in oCommercialBSDetails)
                        {
                            IDataReader readerdetail;
                            oItem.CommercialBSID = oCommercialBS.CommercialBSID;
                            if (oItem.CommercialBSDetailID <= 0)
                            {
                                readerdetail = CommercialBSDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = CommercialBSDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sCommercialBSDetailIDs = sCommercialBSDetailIDs + oReaderDetail.GetString("CommercialBSDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sCommercialBSDetailIDs.Length > 0)
                        {
                            sCommercialBSDetailIDs = sCommercialBSDetailIDs.Remove(sCommercialBSDetailIDs.Length - 1, 1);
                        }
                        oCommercialBSDetail = new CommercialBSDetail();
                        oCommercialBSDetail.CommercialBSID = oCommercialBS.CommercialBSID;
                        CommercialBSDetailDA.Delete(tc, oCommercialBSDetail, EnumDBOperation.Delete, nUserID, sCommercialBSDetailIDs);

                    }

                    #endregion
                    #region CommercialBS Get
                    reader = CommercialBSDA.Get(tc, oCommercialBS.CommercialBSID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oCommercialBS = CreateObject(oReader);
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
						oCommercialBS = new CommercialBS();
						oCommercialBS.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oCommercialBS;
			}
            public CommercialBS ChangeStatus(CommercialBS oCommercialBS, Int64 nUserID)
            {
                List<CommercialBSDetail> oCommercialBSDetails = new List<CommercialBSDetail>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    #region CBS
                    IDataReader reader;
                     reader = CommercialBSDA.ChangeStatus(tc, oCommercialBS,  nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oCommercialBS = new CommercialBS();
                        oCommercialBS = CreateObject(oReader);
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
                        oCommercialBS = new CommercialBS();
                        oCommercialBS.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oCommercialBS;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					CommercialBS oCommercialBS = new CommercialBS();
					oCommercialBS.CommercialBSID = id;
					AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.CommercialBS, EnumRoleOperationType.Delete);
                    DBTableReferenceDA.HasReference(tc, "CommercialBS", id);
					CommercialBSDA.Delete(tc, oCommercialBS, EnumDBOperation.Delete, nUserId);
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
			public CommercialBS Get(int id, Int64 nUserId)
			{
				CommercialBS oCommercialBS = new CommercialBS();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = CommercialBSDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oCommercialBS = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get CommercialBS", e);
					#endregion
				}
				return oCommercialBS;
			}
			public List<CommercialBS> Gets (string sSQL, Int64 nUserID)
			{
				List<CommercialBS> oCommercialBSs = new List<CommercialBS>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CommercialBSDA.Gets(tc, sSQL);
					oCommercialBSs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get CommercialBS", e);
					#endregion
				}
				return oCommercialBSs;
			}

		#endregion
	}

}

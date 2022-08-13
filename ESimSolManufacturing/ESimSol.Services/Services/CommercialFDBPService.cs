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
	public class CommercialFDBPService : MarshalByRefObject, ICommercialFDBPService
	{
		#region Private functions and declaration

		private CommercialFDBP MapObject(NullHandler oReader)
		{
			CommercialFDBP oCommercialFDBP = new CommercialFDBP();
			oCommercialFDBP.CommercialFDBPID = oReader.GetInt32("CommercialFDBPID");
            oCommercialFDBP.CommercialBSID = oReader.GetInt32("CommercialBSID");
			oCommercialFDBP.PurchaseDate = oReader.GetDateTime("PurchaseDate");
			oCommercialFDBP.PurchaseAmount = oReader.GetDouble("PurchaseAmount");
			oCommercialFDBP.BankCharge = oReader.GetDouble("BankCharge");
			oCommercialFDBP.CRate = oReader.GetDouble("CRate");
			oCommercialFDBP.CurrencyID = oReader.GetInt32("CurrencyID");
			oCommercialFDBP.CurrencySymbol = oReader.GetString("CurrencySymbol");
			oCommercialFDBP.Remarks = oReader.GetString("Remarks");
            oCommercialFDBP.BSAmount = oReader.GetDouble("BSAmount");
            oCommercialFDBP.BuyerName = oReader.GetString("BuyerName");
            oCommercialFDBP.MasterLCNo = oReader.GetString("MasterLCNo");
            oCommercialFDBP.BankID = oReader.GetInt32("BankID");
            oCommercialFDBP.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oCommercialFDBP.ApprovedByName = oReader.GetString("ApprovedByName");
            oCommercialFDBP.FDBPNo = oReader.GetString("FDBPNo");
            oCommercialFDBP.BankRefNo = oReader.GetString("BankRefNo");
			return oCommercialFDBP;
		}

		private CommercialFDBP CreateObject(NullHandler oReader)
		{
			CommercialFDBP oCommercialFDBP = new CommercialFDBP();
			oCommercialFDBP = MapObject(oReader);
			return oCommercialFDBP;
		}

		private List<CommercialFDBP> CreateObjects(IDataReader oReader)
		{
			List<CommercialFDBP> oCommercialFDBP = new List<CommercialFDBP>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				CommercialFDBP oItem = CreateObject(oHandler);
				oCommercialFDBP.Add(oItem);
			}
			return oCommercialFDBP;
		}

		#endregion

		#region Interface implementation
			public CommercialFDBP Save(CommercialFDBP oCommercialFDBP, Int64 nUserID)
			{
				TransactionContext tc = null;
                CommercialFDBPDetail oCommercialFDBPDetail = new CommercialFDBPDetail();
                List<CommercialFDBPDetail> oCommercialFDBPDetails = new List<CommercialFDBPDetail>();
                oCommercialFDBPDetails = oCommercialFDBP.CommercialFDBPDetails;

                string sCommercialFDBPDetailIDs = "";
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oCommercialFDBP.CommercialFDBPID <= 0)
					{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CommercialFDBP, EnumRoleOperationType.Add);
						reader = CommercialFDBPDA.InsertUpdate(tc, oCommercialFDBP, EnumDBOperation.Insert, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CommercialFDBP, EnumRoleOperationType.Edit);
						reader = CommercialFDBPDA.InsertUpdate(tc, oCommercialFDBP, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oCommercialFDBP = new CommercialFDBP();
						oCommercialFDBP = CreateObject(oReader);
					}
					reader.Close();
                    #region CommercialFDBP Detail Part
                    if (oCommercialFDBPDetails != null)
                    {
                        foreach (CommercialFDBPDetail oItem in oCommercialFDBPDetails)
                        {
                            IDataReader readerdetail;
                            oItem.CommercialFDBPID = oCommercialFDBP.CommercialFDBPID;
                            if (oItem.CommercialFDBPDetailID <= 0)
                            {
                                readerdetail = CommercialFDBPDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = CommercialFDBPDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sCommercialFDBPDetailIDs = sCommercialFDBPDetailIDs + oReaderDetail.GetString("CommercialFDBPDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sCommercialFDBPDetailIDs.Length > 0)
                        {
                            sCommercialFDBPDetailIDs = sCommercialFDBPDetailIDs.Remove(sCommercialFDBPDetailIDs.Length - 1, 1);
                        }
                        oCommercialFDBPDetail = new CommercialFDBPDetail();
                        oCommercialFDBPDetail.CommercialFDBPID = oCommercialFDBP.CommercialFDBPID;
                        CommercialFDBPDetailDA.Delete(tc, oCommercialFDBPDetail, EnumDBOperation.Delete, nUserID, sCommercialFDBPDetailIDs);

                    }

                    #endregion
                    #region CommercialFDBP Get
                    reader = CommercialFDBPDA.Get(tc, oCommercialFDBP.CommercialFDBPID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oCommercialFDBP = CreateObject(oReader);
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
						oCommercialFDBP = new CommercialFDBP();
						oCommercialFDBP.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oCommercialFDBP;
			}

            public CommercialFDBP Approve(CommercialFDBP oCommercialFDBP, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CommercialFDBP, EnumRoleOperationType.Approved);
                    IDataReader   reader = CommercialFDBPDA.InsertUpdate(tc, oCommercialFDBP, EnumDBOperation.Approval, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oCommercialFDBP = new CommercialFDBP();
                        oCommercialFDBP = CreateObject(oReader);
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
                        oCommercialFDBP = new CommercialFDBP();
                        oCommercialFDBP.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oCommercialFDBP;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					CommercialFDBP oCommercialFDBP = new CommercialFDBP();
					oCommercialFDBP.CommercialFDBPID = id;
					AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.CommercialFDBP, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "CommercialFDBP", id);
					CommercialFDBPDA.Delete(tc, oCommercialFDBP, EnumDBOperation.Delete, nUserId);
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
			public CommercialFDBP Get(int id, Int64 nUserId)
			{
				CommercialFDBP oCommercialFDBP = new CommercialFDBP();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = CommercialFDBPDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oCommercialFDBP = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get CommercialFDBP", e);
					#endregion
				}
				return oCommercialFDBP;
			}
			public List<CommercialFDBP> Gets (string sSQL, Int64 nUserID)
			{
				List<CommercialFDBP> oCommercialFDBPs = new List<CommercialFDBP>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CommercialFDBPDA.Gets(tc, sSQL);
					oCommercialFDBPs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get CommercialFDBP", e);
					#endregion
				}
				return oCommercialFDBPs;
			}

		#endregion
	}

}

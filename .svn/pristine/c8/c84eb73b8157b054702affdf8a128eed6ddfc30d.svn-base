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
	public class PurchaseReturnService : MarshalByRefObject, IPurchaseReturnService
	{
		#region Private functions and declaration

		private PurchaseReturn MapObject(NullHandler oReader)
		{
			PurchaseReturn oPurchaseReturn = new PurchaseReturn();
			oPurchaseReturn.PurchaseReturnID = oReader.GetInt32("PurchaseReturnID");
			oPurchaseReturn.ReturnNo = oReader.GetString("ReturnNo");
			oPurchaseReturn.ReturnDate = oReader.GetDateTime("ReturnDate");
			oPurchaseReturn.RefType = (EnumPurchaseReturnType) oReader.GetInt32("RefType");
            oPurchaseReturn.RefTypeInt = oReader.GetInt32("RefType");
			oPurchaseReturn.SupplierID = oReader.GetInt32("SupplierID");
			oPurchaseReturn.RefObjectID = oReader.GetInt32("RefObjectID");
			oPurchaseReturn.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
			oPurchaseReturn.Remarks = oReader.GetString("Remarks");
			oPurchaseReturn.ApprovedBy = oReader.GetInt32("ApprovedBy");
			oPurchaseReturn.ApprovedDate = oReader.GetDateTime("ApprovedDate");
			oPurchaseReturn.SupplierName = oReader.GetString("SupplierName");
			oPurchaseReturn.WorkingUnitName = oReader.GetString("WorkingUnitName");
			oPurchaseReturn.RefNo = oReader.GetString("RefNo");
			oPurchaseReturn.ApprovedByName = oReader.GetString("ApprovedByName");
            oPurchaseReturn.DisbursedBy = oReader.GetInt32("DisbursedBy");
            oPurchaseReturn.DisbursedByName = oReader.GetString("DisbursedByName");
            oPurchaseReturn.BUID = oReader.GetInt32("BUID");
            oPurchaseReturn.RefDate = oReader.GetDateTime("RefDate");	
            
			return oPurchaseReturn;
		}

		private PurchaseReturn CreateObject(NullHandler oReader)
		{
			PurchaseReturn oPurchaseReturn = new PurchaseReturn();
			oPurchaseReturn = MapObject(oReader);
			return oPurchaseReturn;
		}

		private List<PurchaseReturn> CreateObjects(IDataReader oReader)
		{
			List<PurchaseReturn> oPurchaseReturn = new List<PurchaseReturn>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				PurchaseReturn oItem = CreateObject(oHandler);
				oPurchaseReturn.Add(oItem);
			}
			return oPurchaseReturn;
		}

		#endregion

		#region Interface implementation
			public PurchaseReturn Save(PurchaseReturn oPurchaseReturn, Int64 nUserID)
			{
				TransactionContext tc = null;
                string sPurchaseReturnDetailIDs = "";
                List<PurchaseReturnDetail> oPurchaseReturnDetails = new List<PurchaseReturnDetail>();
                oPurchaseReturnDetails = oPurchaseReturn.PurchaseReturnDetails;
				try
				{
					tc = TransactionContext.Begin(true);
                    #region PurchaseReturn
                    IDataReader reader;
					if (oPurchaseReturn.PurchaseReturnID <= 0)
					{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseReturn, EnumRoleOperationType.Add);
						reader = PurchaseReturnDA.InsertUpdate(tc, oPurchaseReturn, EnumDBOperation.Insert, nUserID);
					}
					else{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseReturn, EnumRoleOperationType.Edit);
						reader = PurchaseReturnDA.InsertUpdate(tc, oPurchaseReturn, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oPurchaseReturn = new PurchaseReturn();
						oPurchaseReturn = CreateObject(oReader);
					}
					reader.Close();
                    #endregion

                    #region PurchaseReturn Detail Part
                    if (oPurchaseReturnDetails != null)
                    {
                        foreach (PurchaseReturnDetail oItem in oPurchaseReturnDetails)
                        {
                            IDataReader readerdetail;
                            oItem.PurchaseReturnID = oPurchaseReturn.PurchaseReturnID;
                            if (oItem.PurchaseReturnDetailID <= 0)
                            {
                                readerdetail = PurchaseReturnDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = PurchaseReturnDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sPurchaseReturnDetailIDs = sPurchaseReturnDetailIDs + oReaderDetail.GetString("PurchaseReturnDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sPurchaseReturnDetailIDs.Length > 0)
                        {
                            sPurchaseReturnDetailIDs = sPurchaseReturnDetailIDs.Remove(sPurchaseReturnDetailIDs.Length - 1, 1);
                        }
                    }
                    PurchaseReturnDetail oPurchaseReturnDetail = new PurchaseReturnDetail();
                    oPurchaseReturnDetail.PurchaseReturnID = oPurchaseReturn.PurchaseReturnID;
                    PurchaseReturnDetailDA.Delete(tc, oPurchaseReturnDetail, EnumDBOperation.Delete, nUserID, sPurchaseReturnDetailIDs);
                    #endregion

                    #region Get PurchaseReturn
                    reader = PurchaseReturnDA.Get(tc, oPurchaseReturn.PurchaseReturnID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPurchaseReturn = new PurchaseReturn();
                        oPurchaseReturn = CreateObject(oReader);
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
						oPurchaseReturn = new PurchaseReturn();
						oPurchaseReturn.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oPurchaseReturn;
			}

            public PurchaseReturn Approve(PurchaseReturn oPurchaseReturn, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    #region PurchaseReturn
                    IDataReader reader;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseReturn, EnumRoleOperationType.Approved);
                    reader = PurchaseReturnDA.InsertUpdate(tc, oPurchaseReturn, EnumDBOperation.Approval, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPurchaseReturn = new PurchaseReturn();
                        oPurchaseReturn = CreateObject(oReader);
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
                        oPurchaseReturn = new PurchaseReturn();
                        oPurchaseReturn.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oPurchaseReturn;
            }

            public PurchaseReturn Disburse(PurchaseReturn oPurchaseReturn, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    #region PurchaseReturn
                    IDataReader reader;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseReturn, EnumRoleOperationType.Disburse);
                    reader = PurchaseReturnDA.InsertUpdate(tc, oPurchaseReturn, EnumDBOperation.Disburse, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPurchaseReturn = new PurchaseReturn();
                        oPurchaseReturn = CreateObject(oReader);
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
                        oPurchaseReturn = new PurchaseReturn();
                        oPurchaseReturn.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oPurchaseReturn;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					PurchaseReturn oPurchaseReturn = new PurchaseReturn();
					oPurchaseReturn.PurchaseReturnID = id;
					AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.PurchaseReturn, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "PurchaseReturn", id);
					PurchaseReturnDA.Delete(tc, oPurchaseReturn, EnumDBOperation.Delete, nUserId);
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exceptionif (tc != null)
					tc.HandleError();
					return e.Message.Split('!')[0];
					#endregion
				}
				return "Data delete successfully";
			}

			public PurchaseReturn Get(int id, Int64 nUserId)
			{
				PurchaseReturn oPurchaseReturn = new PurchaseReturn();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = PurchaseReturnDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oPurchaseReturn = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get PurchaseReturn", e);
					#endregion
				}
				return oPurchaseReturn;
			}

			public List<PurchaseReturn> Gets(Int64 nUserID)
			{
				List<PurchaseReturn> oPurchaseReturns = new List<PurchaseReturn>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PurchaseReturnDA.Gets(tc);
					oPurchaseReturns = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					PurchaseReturn oPurchaseReturn = new PurchaseReturn();
					oPurchaseReturn.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oPurchaseReturns;
			}

			public List<PurchaseReturn> Gets (string sSQL, Int64 nUserID)
			{
				List<PurchaseReturn> oPurchaseReturns = new List<PurchaseReturn>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PurchaseReturnDA.Gets(tc, sSQL);
					oPurchaseReturns = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get PurchaseReturn", e);
					#endregion
				}
				return oPurchaseReturns;
			}

		#endregion
	}

}

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
	public class S2SLotTransferService : MarshalByRefObject, IS2SLotTransferService
	{
		#region Private functions and declaration

		private S2SLotTransfer MapObject(NullHandler oReader)
		{
			S2SLotTransfer oS2SLotTransfer = new S2SLotTransfer();
			oS2SLotTransfer.S2SLotTransferID = oReader.GetInt32("S2SLotTransferID");
			oS2SLotTransfer.StoreID = oReader.GetInt32("StoreID");
			oS2SLotTransfer.TransferDate = oReader.GetDateTime("TransferDate");
			oS2SLotTransfer.IssueStyleID = oReader.GetInt32("IssueStyleID");
			oS2SLotTransfer.ReceiveStyleID = oReader.GetInt32("ReceiveStyleID");
			oS2SLotTransfer.IssueLotID = oReader.GetInt32("IssueLotID");
			oS2SLotTransfer.ReceiveLotID = oReader.GetInt32("ReceiveLotID");
			oS2SLotTransfer.TransferQty = oReader.GetDouble("TransferQty");
			oS2SLotTransfer.AuthorizedByID = oReader.GetInt32("AuthorizedByID");
			oS2SLotTransfer.IssueStyleNo = oReader.GetString("IssueStyleNo");
			oS2SLotTransfer.ReceiveStyleNo = oReader.GetString("ReceiveStyleNo");
			oS2SLotTransfer.StoreName = oReader.GetString("StoreName");
			oS2SLotTransfer.IssueLotNo = oReader.GetString("IssueLotNo");
			oS2SLotTransfer.ReceiveLotNo = oReader.GetString("ReceiveLotNo");
			oS2SLotTransfer.ProductCode = oReader.GetString("ProductCode");
			oS2SLotTransfer.ProductName = oReader.GetString("ProductName");
            oS2SLotTransfer.AuthorizedByName = oReader.GetString("AuthorizedByName");
            oS2SLotTransfer.IssueLotBalance = oReader.GetDouble("IssueLotBalance");
            oS2SLotTransfer.BUID = oReader.GetInt32("BUID");
            oS2SLotTransfer.ProductID = oReader.GetInt32("ProductID");
            oS2SLotTransfer.Remarks = oReader.GetString("Remarks");
			return oS2SLotTransfer;
		}

		private S2SLotTransfer CreateObject(NullHandler oReader)
		{
			S2SLotTransfer oS2SLotTransfer = new S2SLotTransfer();
			oS2SLotTransfer = MapObject(oReader);
			return oS2SLotTransfer;
		}

		private List<S2SLotTransfer> CreateObjects(IDataReader oReader)
		{
			List<S2SLotTransfer> oS2SLotTransfer = new List<S2SLotTransfer>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				S2SLotTransfer oItem = CreateObject(oHandler);
				oS2SLotTransfer.Add(oItem);
			}
			return oS2SLotTransfer;
		}

		#endregion

		#region Interface implementation
			public S2SLotTransfer Save(S2SLotTransfer oS2SLotTransfer, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oS2SLotTransfer.S2SLotTransferID <= 0)
					{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.S2SLotTransfer, EnumRoleOperationType.Add);
						reader = S2SLotTransferDA.InsertUpdate(tc, oS2SLotTransfer, EnumDBOperation.Insert, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.S2SLotTransfer, EnumRoleOperationType.Edit);
						reader = S2SLotTransferDA.InsertUpdate(tc, oS2SLotTransfer, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oS2SLotTransfer = new S2SLotTransfer();
						oS2SLotTransfer = CreateObject(oReader);
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
						oS2SLotTransfer = new S2SLotTransfer();
						oS2SLotTransfer.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oS2SLotTransfer;
			}

            public S2SLotTransfer Commit(S2SLotTransfer oS2SLotTransfer, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.S2SLotTransfer, EnumRoleOperationType.Approved);
                    reader = S2SLotTransferDA.InsertUpdate(tc, oS2SLotTransfer, EnumDBOperation.Approval, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oS2SLotTransfer = new S2SLotTransfer();
                        oS2SLotTransfer = CreateObject(oReader);
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
                        oS2SLotTransfer = new S2SLotTransfer();
                        oS2SLotTransfer.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oS2SLotTransfer;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					S2SLotTransfer oS2SLotTransfer = new S2SLotTransfer();
					oS2SLotTransfer.S2SLotTransferID = id;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.S2SLotTransfer, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "S2SLotTransfer", id);
					S2SLotTransferDA.Delete(tc, oS2SLotTransfer, EnumDBOperation.Delete, nUserId);
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

			public S2SLotTransfer Get(int id, Int64 nUserId)
			{
				S2SLotTransfer oS2SLotTransfer = new S2SLotTransfer();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = S2SLotTransferDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oS2SLotTransfer = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get S2SLotTransfer", e);
					#endregion
				}
				return oS2SLotTransfer;
			}

			public List<S2SLotTransfer> Gets(Int64 nUserID)
			{
				List<S2SLotTransfer> oS2SLotTransfers = new List<S2SLotTransfer>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = S2SLotTransferDA.Gets(tc);
					oS2SLotTransfers = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					S2SLotTransfer oS2SLotTransfer = new S2SLotTransfer();
					oS2SLotTransfer.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oS2SLotTransfers;
			}

			public List<S2SLotTransfer> Gets (string sSQL, Int64 nUserID)
			{
				List<S2SLotTransfer> oS2SLotTransfers = new List<S2SLotTransfer>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = S2SLotTransferDA.Gets(tc, sSQL);
					oS2SLotTransfers = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get S2SLotTransfer", e);
					#endregion
				}
				return oS2SLotTransfers;
			}

		#endregion
	}

}

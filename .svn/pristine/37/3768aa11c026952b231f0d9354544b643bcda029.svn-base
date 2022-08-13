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
	public class FabricBatchQCTransferService : MarshalByRefObject, IFabricBatchQCTransferService
	{
		#region Private functions and declaration

		private FabricBatchQCTransfer MapObject(NullHandler oReader)
		{
			FabricBatchQCTransfer oFabricBatchQCTransfer = new FabricBatchQCTransfer();
			oFabricBatchQCTransfer.FBQCTransferID = oReader.GetInt32("FBQCTransferID");
			oFabricBatchQCTransfer.TransferNo = oReader.GetString("TransferNo");
			oFabricBatchQCTransfer.IssueDate = oReader.GetDateTime("IssueDate");
			oFabricBatchQCTransfer.IssueBy = oReader.GetInt32("IssueBy");
            oFabricBatchQCTransfer.TSIssueName = oReader.GetString("TSIssueName");
            oFabricBatchQCTransfer.RoleCount = oReader.GetString("RoleCount");
            oFabricBatchQCTransfer.BatchNo = oReader.GetString("BatchNo");
			return oFabricBatchQCTransfer;
		}

		private FabricBatchQCTransfer CreateObject(NullHandler oReader)
		{
			FabricBatchQCTransfer oFabricBatchQCTransfer = new FabricBatchQCTransfer();
			oFabricBatchQCTransfer = MapObject(oReader);
			return oFabricBatchQCTransfer;
		}

		private List<FabricBatchQCTransfer> CreateObjects(IDataReader oReader)
		{
			List<FabricBatchQCTransfer> oFabricBatchQCTransfer = new List<FabricBatchQCTransfer>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FabricBatchQCTransfer oItem = CreateObject(oHandler);
				oFabricBatchQCTransfer.Add(oItem);
			}
			return oFabricBatchQCTransfer;
		}

		#endregion

		#region Interface implementation
			public FabricBatchQCTransfer Save(FabricBatchQCTransfer oFabricBatchQCTransfer, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oFabricBatchQCTransfer.FBQCTransferID <= 0)
					{
                        reader = FabricBatchQCTransferDA.InsertUpdate(tc, oFabricBatchQCTransfer, EnumDBOperation.Insert, nUserID);
					}
					else{
						
						reader = FabricBatchQCTransferDA.InsertUpdate(tc, oFabricBatchQCTransfer, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oFabricBatchQCTransfer = new FabricBatchQCTransfer();
						oFabricBatchQCTransfer = CreateObject(oReader);
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
						oFabricBatchQCTransfer = new FabricBatchQCTransfer();
						oFabricBatchQCTransfer.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFabricBatchQCTransfer;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FabricBatchQCTransfer oFabricBatchQCTransfer = new FabricBatchQCTransfer();
					oFabricBatchQCTransfer.FBQCTransferID = id;
					
					DBTableReferenceDA.HasReference(tc, "FabricBatchQCTransfer", id);
					FabricBatchQCTransferDA.Delete(tc, oFabricBatchQCTransfer, EnumDBOperation.Delete, nUserId);
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

			public FabricBatchQCTransfer Get(int id, Int64 nUserId)
			{
				FabricBatchQCTransfer oFabricBatchQCTransfer = new FabricBatchQCTransfer();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FabricBatchQCTransferDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFabricBatchQCTransfer = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FabricBatchQCTransfer", e);
					#endregion
				}
				return oFabricBatchQCTransfer;
			}

			public List<FabricBatchQCTransfer> Gets(Int64 nUserID)
			{
				List<FabricBatchQCTransfer> oFabricBatchQCTransfers = new List<FabricBatchQCTransfer>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FabricBatchQCTransferDA.Gets(tc);
					oFabricBatchQCTransfers = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FabricBatchQCTransfer oFabricBatchQCTransfer = new FabricBatchQCTransfer();
					oFabricBatchQCTransfer.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFabricBatchQCTransfers;
			}

			public List<FabricBatchQCTransfer> Gets (string sSQL, Int64 nUserID)
			{
				List<FabricBatchQCTransfer> oFabricBatchQCTransfers = new List<FabricBatchQCTransfer>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FabricBatchQCTransferDA.Gets(tc, sSQL);
					oFabricBatchQCTransfers = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FabricBatchQCTransfer", e);
					#endregion
				}
				return oFabricBatchQCTransfers;
			}

		#endregion
	}

}

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
	public class CostHeadService : MarshalByRefObject, ICostHeadService
	{
		#region Private functions and declaration

		private CostHead MapObject(NullHandler oReader)
		{
			CostHead oCostHead = new CostHead();
			oCostHead.CostHeadID = oReader.GetInt32("CostHeadID");
			oCostHead.Name = oReader.GetString("Name");
			oCostHead.Note = oReader.GetString("Note");
            oCostHead.CostHeadType = (EnumCostHeadType)oReader.GetInt32("CostHeadType");
			//oCostHead.CostHeadCategorey  =(EnumCostHeadCategorey) oReader.GetInt32("CostHeadCategorey");
			return oCostHead;
		}

		private CostHead CreateObject(NullHandler oReader)
		{
			CostHead oCostHead = new CostHead();
			oCostHead = MapObject(oReader);
			return oCostHead;
		}

		private List<CostHead> CreateObjects(IDataReader oReader)
		{
			List<CostHead> oCostHead = new List<CostHead>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				CostHead oItem = CreateObject(oHandler);
				oCostHead.Add(oItem);
			}
			return oCostHead;
		}

		#endregion

		#region Interface implementation
			public CostHead Save(CostHead oCostHead, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oCostHead.CostHeadID <= 0)
					{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "CostHead", EnumRoleOperationType.Add);
						reader = CostHeadDA.InsertUpdate(tc, oCostHead, EnumDBOperation.Insert, nUserID);
					}
					else{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "CostHead", EnumRoleOperationType.Edit);
						reader = CostHeadDA.InsertUpdate(tc, oCostHead, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oCostHead = new CostHead();
						oCostHead = CreateObject(oReader);
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
						oCostHead = new CostHead();
						oCostHead.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oCostHead;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					CostHead oCostHead = new CostHead();
					oCostHead.CostHeadID = id;
					//AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "CostHead", EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "CostHead", id);
					CostHeadDA.Delete(tc, oCostHead, EnumDBOperation.Delete, nUserId);
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exceptionif (tc != null)
					tc.HandleError();
					return e.Message.Split('!')[0];
					#endregion
				}
				return  Global.DeleteMessage;
			}

			public CostHead Get(int id, Int64 nUserId)
			{
				CostHead oCostHead = new CostHead();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = CostHeadDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oCostHead = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get CostHead", e);
					#endregion
				}
				return oCostHead;
			}

			public List<CostHead> Gets(Int64 nUserID)
			{
				List<CostHead> oCostHeads = new List<CostHead>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CostHeadDA.Gets(tc);
					oCostHeads = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					CostHead oCostHead = new CostHead();
					oCostHead.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oCostHeads;
			}

			public List<CostHead> Gets (string sSQL, Int64 nUserID)
			{
				List<CostHead> oCostHeads = new List<CostHead>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CostHeadDA.Gets(tc, sSQL);
					oCostHeads = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get CostHead", e);
					#endregion
				}
				return oCostHeads;
			}

		#endregion
	}

}

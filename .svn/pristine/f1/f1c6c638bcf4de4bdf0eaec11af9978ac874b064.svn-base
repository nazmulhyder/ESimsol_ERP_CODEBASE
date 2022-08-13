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
	public class PurchaseCostService : MarshalByRefObject, IPurchaseCostService
	{
		#region Private functions and declaration

		private PurchaseCost MapObject(NullHandler oReader)
		{
			PurchaseCost oPurchaseCost = new PurchaseCost();
			oPurchaseCost.PurchaseCostID = oReader.GetInt32("PurchaseCostID");
			oPurchaseCost.RefID = oReader.GetInt32("RefID");
			oPurchaseCost.CostHeadID = oReader.GetInt32("CostHeadID");
			oPurchaseCost.ValueInPercent = oReader.GetDouble("ValueInPercent");
			oPurchaseCost.ValueInAmount = oReader.GetDouble("ValueInAmount");
			oPurchaseCost.RefType = oReader.GetInt32("RefType");
            oPurchaseCost.Name = oReader.GetString("Name");
            oPurchaseCost.CostHeadType = (EnumCostHeadType)oReader.GetInt32("CostHeadType");
            //oPurchaseCost.CostHeadCategorey = (EnumCostHeadCategorey)oReader.GetInt32("CostHeadCategorey");
            oPurchaseCost.Sequence = oReader.GetInt32("Sequence");
			return oPurchaseCost;
		}

		private PurchaseCost CreateObject(NullHandler oReader)
		{
			PurchaseCost oPurchaseCost = new PurchaseCost();
			oPurchaseCost = MapObject(oReader);
			return oPurchaseCost;
		}

		private List<PurchaseCost> CreateObjects(IDataReader oReader)
		{
			List<PurchaseCost> oPurchaseCost = new List<PurchaseCost>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				PurchaseCost oItem = CreateObject(oHandler);
				oPurchaseCost.Add(oItem);
			}
			return oPurchaseCost;
		}

		#endregion

		#region Interface implementation
			public PurchaseCost Save(PurchaseCost oPurchaseCost, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oPurchaseCost.PurchaseCostID <= 0)
					{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseCost", EnumRoleOperationType.Add);
						reader = PurchaseCostDA.InsertUpdate(tc, oPurchaseCost, EnumDBOperation.Insert, nUserID,"");
					}
					else{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseCost", EnumRoleOperationType.Edit);
						reader = PurchaseCostDA.InsertUpdate(tc, oPurchaseCost, EnumDBOperation.Update, nUserID,"");
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oPurchaseCost = new PurchaseCost();
						oPurchaseCost = CreateObject(oReader);
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
						oPurchaseCost = new PurchaseCost();
						oPurchaseCost.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oPurchaseCost;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					PurchaseCost oPurchaseCost = new PurchaseCost();
					oPurchaseCost.PurchaseCostID = id;
					//AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "PurchaseCost", EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "PurchaseCost", id);
					PurchaseCostDA.Delete(tc, oPurchaseCost, EnumDBOperation.Delete, nUserId,"");
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

			public PurchaseCost Get(int id, Int64 nUserId)
			{
				PurchaseCost oPurchaseCost = new PurchaseCost();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = PurchaseCostDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oPurchaseCost = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get PurchaseCost", e);
					#endregion
				}
				return oPurchaseCost;
			}

			public List<PurchaseCost> Gets(Int64 nUserID)
			{
				List<PurchaseCost> oPurchaseCosts = new List<PurchaseCost>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PurchaseCostDA.Gets(tc);
					oPurchaseCosts = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					PurchaseCost oPurchaseCost = new PurchaseCost();
					oPurchaseCost.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oPurchaseCosts;
			}

			public List<PurchaseCost> Gets (string sSQL, Int64 nUserID)
			{
				List<PurchaseCost> oPurchaseCosts = new List<PurchaseCost>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PurchaseCostDA.Gets(tc, sSQL);
					oPurchaseCosts = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get PurchaseCost", e);
					#endregion
				}
				return oPurchaseCosts;
			}

		#endregion
	}

}

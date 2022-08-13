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
	public class FabricPlanCountService : MarshalByRefObject, IFabricPlanCountService
	{
		#region Private functions and declaration

		private FabricPlanCount MapObject(NullHandler oReader)
		{
			FabricPlanCount oFabricPlanCount = new FabricPlanCount();
			oFabricPlanCount.FabricPlanCountID = oReader.GetInt32("FabricPlanCountID");
			oFabricPlanCount.FabricPlanningID = oReader.GetInt32("FabricPlanningID");
			oFabricPlanCount.SLNo = oReader.GetInt32("SLNo");
			oFabricPlanCount.Repeat = oReader.GetString("Repeat");
			oFabricPlanCount.RepeatCount = oReader.GetInt32("RepeatCount");
			return oFabricPlanCount;
		}

		private FabricPlanCount CreateObject(NullHandler oReader)
		{
			FabricPlanCount oFabricPlanCount = new FabricPlanCount();
			oFabricPlanCount = MapObject(oReader);
			return oFabricPlanCount;
		}

		private List<FabricPlanCount> CreateObjects(IDataReader oReader)
		{
			List<FabricPlanCount> oFabricPlanCount = new List<FabricPlanCount>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FabricPlanCount oItem = CreateObject(oHandler);
				oFabricPlanCount.Add(oItem);
			}
			return oFabricPlanCount;
		}

		#endregion

		#region Interface implementation
			public FabricPlanCount Save(FabricPlanCount oFabricPlanCount, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oFabricPlanCount.FabricPlanCountID <= 0)
					{
						reader = FabricPlanCountDA.InsertUpdate(tc, oFabricPlanCount, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = FabricPlanCountDA.InsertUpdate(tc, oFabricPlanCount, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oFabricPlanCount = new FabricPlanCount();
						oFabricPlanCount = CreateObject(oReader);
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
						oFabricPlanCount = new FabricPlanCount();
						oFabricPlanCount.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFabricPlanCount;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FabricPlanCount oFabricPlanCount = new FabricPlanCount();
					oFabricPlanCount.FabricPlanCountID = id;
					DBTableReferenceDA.HasReference(tc, "FabricPlanCount", id);
					FabricPlanCountDA.Delete(tc, oFabricPlanCount, EnumDBOperation.Delete, nUserId);
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

			public FabricPlanCount Get(int id, Int64 nUserId)
			{
				FabricPlanCount oFabricPlanCount = new FabricPlanCount();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FabricPlanCountDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFabricPlanCount = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FabricPlanCount", e);
					#endregion
				}
				return oFabricPlanCount;
			}

			public List<FabricPlanCount> Gets(Int64 nUserID)
			{
				List<FabricPlanCount> oFabricPlanCounts = new List<FabricPlanCount>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FabricPlanCountDA.Gets(tc);
					oFabricPlanCounts = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FabricPlanCount oFabricPlanCount = new FabricPlanCount();
					oFabricPlanCount.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFabricPlanCounts;
			}

			public List<FabricPlanCount> Gets (string sSQL, Int64 nUserID)
			{
				List<FabricPlanCount> oFabricPlanCounts = new List<FabricPlanCount>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FabricPlanCountDA.Gets(tc, sSQL);
					oFabricPlanCounts = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FabricPlanCount", e);
					#endregion
				}
				return oFabricPlanCounts;
			}

		#endregion
	}

}

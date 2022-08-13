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
	public class KnitDyeingBatchGrayChallanService : MarshalByRefObject, IKnitDyeingBatchGrayChallanService
	{
		#region Private functions and declaration

		public static KnitDyeingBatchGrayChallan MapObject(NullHandler oReader)
		{
			KnitDyeingBatchGrayChallan oKnitDyeingBatchGrayChallan = new KnitDyeingBatchGrayChallan();
			oKnitDyeingBatchGrayChallan.KnitDyeingBatchGrayChallanID = oReader.GetInt32("KnitDyeingBatchGrayChallanID");
			oKnitDyeingBatchGrayChallan.KnitDyeingBatchDetailID = oReader.GetInt32("KnitDyeingBatchDetailID");
			oKnitDyeingBatchGrayChallan.GrayFabricID = oReader.GetInt32("GrayFabricID");
			oKnitDyeingBatchGrayChallan.StoreID = oReader.GetInt32("StoreID");
			oKnitDyeingBatchGrayChallan.StoreName = oReader.GetString("StoreName");
			oKnitDyeingBatchGrayChallan.LotID = oReader.GetInt32("LotID");
            oKnitDyeingBatchGrayChallan.LotNo = oReader.GetString("LotNo");
			oKnitDyeingBatchGrayChallan.MUnitID = oReader.GetInt32("MUnitID");
			oKnitDyeingBatchGrayChallan.UnitName = oReader.GetString("UnitName");
            oKnitDyeingBatchGrayChallan.FabricName = oReader.GetString("FabricName");
			oKnitDyeingBatchGrayChallan.Qty = oReader.GetDouble("Qty");
			oKnitDyeingBatchGrayChallan.Remarks = oReader.GetString("Remarks");
			oKnitDyeingBatchGrayChallan.DisburseBy = oReader.GetInt32("DisburseBy");
			oKnitDyeingBatchGrayChallan.DisburseByName = oReader.GetString("DisburseByName");
			return oKnitDyeingBatchGrayChallan;
		}

		public static KnitDyeingBatchGrayChallan CreateObject(NullHandler oReader)
		{
			KnitDyeingBatchGrayChallan oKnitDyeingBatchGrayChallan = new KnitDyeingBatchGrayChallan();
			oKnitDyeingBatchGrayChallan = MapObject(oReader);
			return oKnitDyeingBatchGrayChallan;
		}

        public static List<KnitDyeingBatchGrayChallan> CreateObjects(IDataReader oReader)
		{
			List<KnitDyeingBatchGrayChallan> oKnitDyeingBatchGrayChallan = new List<KnitDyeingBatchGrayChallan>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KnitDyeingBatchGrayChallan oItem = CreateObject(oHandler);
				oKnitDyeingBatchGrayChallan.Add(oItem);
			}
			return oKnitDyeingBatchGrayChallan;
		}

		#endregion

		#region Interface implementation
			public KnitDyeingBatchGrayChallan Save(KnitDyeingBatchGrayChallan oKnitDyeingBatchGrayChallan, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oKnitDyeingBatchGrayChallan.KnitDyeingBatchGrayChallanID <= 0)
					{
						reader = KnitDyeingBatchGrayChallanDA.InsertUpdate(tc, oKnitDyeingBatchGrayChallan, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = KnitDyeingBatchGrayChallanDA.InsertUpdate(tc, oKnitDyeingBatchGrayChallan, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oKnitDyeingBatchGrayChallan = new KnitDyeingBatchGrayChallan();
						oKnitDyeingBatchGrayChallan = CreateObject(oReader);
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
						oKnitDyeingBatchGrayChallan = new KnitDyeingBatchGrayChallan();
						oKnitDyeingBatchGrayChallan.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oKnitDyeingBatchGrayChallan;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					KnitDyeingBatchGrayChallan oKnitDyeingBatchGrayChallan = new KnitDyeingBatchGrayChallan();
					oKnitDyeingBatchGrayChallan.KnitDyeingBatchGrayChallanID = id;
					DBTableReferenceDA.HasReference(tc, "KnitDyeingBatchGrayChallan", id);
					KnitDyeingBatchGrayChallanDA.Delete(tc, oKnitDyeingBatchGrayChallan, EnumDBOperation.Delete, nUserId);
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

			public KnitDyeingBatchGrayChallan Get(int id, Int64 nUserId)
			{
				KnitDyeingBatchGrayChallan oKnitDyeingBatchGrayChallan = new KnitDyeingBatchGrayChallan();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = KnitDyeingBatchGrayChallanDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKnitDyeingBatchGrayChallan = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KnitDyeingBatchGrayChallan", e);
					#endregion
				}
				return oKnitDyeingBatchGrayChallan;
			}

			public List<KnitDyeingBatchGrayChallan> Gets(Int64 nUserID)
			{
				List<KnitDyeingBatchGrayChallan> oKnitDyeingBatchGrayChallans = new List<KnitDyeingBatchGrayChallan>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingBatchGrayChallanDA.Gets(tc);
					oKnitDyeingBatchGrayChallans = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					KnitDyeingBatchGrayChallan oKnitDyeingBatchGrayChallan = new KnitDyeingBatchGrayChallan();
					oKnitDyeingBatchGrayChallan.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oKnitDyeingBatchGrayChallans;
			}

			public List<KnitDyeingBatchGrayChallan> Gets (string sSQL, Int64 nUserID)
			{
				List<KnitDyeingBatchGrayChallan> oKnitDyeingBatchGrayChallans = new List<KnitDyeingBatchGrayChallan>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingBatchGrayChallanDA.Gets(tc, sSQL);
					oKnitDyeingBatchGrayChallans = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get KnitDyeingBatchGrayChallan", e);
					#endregion
				}
				return oKnitDyeingBatchGrayChallans;
			}

		#endregion
	}

}

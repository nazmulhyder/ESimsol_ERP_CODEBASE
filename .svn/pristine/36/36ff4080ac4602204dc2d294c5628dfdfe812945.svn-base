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
	public class KnitDyeingGrayChallanDetailService : MarshalByRefObject, IKnitDyeingGrayChallanDetailService
	{
		#region Private functions and declaration

		private KnitDyeingGrayChallanDetail MapObject(NullHandler oReader)
		{
			KnitDyeingGrayChallanDetail oKnitDyeingGrayChallanDetail = new KnitDyeingGrayChallanDetail();
			oKnitDyeingGrayChallanDetail.KnitDyeingGrayChallanDetailID = oReader.GetInt32("KnitDyeingGrayChallanDetailID");
			oKnitDyeingGrayChallanDetail.KnitDyeingGrayChallanID = oReader.GetInt32("KnitDyeingGrayChallanID");
			oKnitDyeingGrayChallanDetail.KnitDyeingBatchDetailID = oReader.GetInt32("KnitDyeingBatchDetailID");
			oKnitDyeingGrayChallanDetail.GrayFabricID = oReader.GetInt32("GrayFabricID");
			oKnitDyeingGrayChallanDetail.StoreID = oReader.GetInt32("StoreID");
			oKnitDyeingGrayChallanDetail.LotID = oReader.GetInt32("LotID");
			oKnitDyeingGrayChallanDetail.MUnitID = oReader.GetInt32("MUnitID");
			oKnitDyeingGrayChallanDetail.Qty = oReader.GetDouble("Qty");
			oKnitDyeingGrayChallanDetail.Remarks = oReader.GetString("Remarks");
			oKnitDyeingGrayChallanDetail.StoreName = oReader.GetString("StoreName");
			oKnitDyeingGrayChallanDetail.LotNo = oReader.GetString("LotNo");
            oKnitDyeingGrayChallanDetail.UnitName = oReader.GetString("UnitName");
            oKnitDyeingGrayChallanDetail.FabricName = oReader.GetString("FabricName");
			return oKnitDyeingGrayChallanDetail;
		}

		private KnitDyeingGrayChallanDetail CreateObject(NullHandler oReader)
		{
			KnitDyeingGrayChallanDetail oKnitDyeingGrayChallanDetail = new KnitDyeingGrayChallanDetail();
			oKnitDyeingGrayChallanDetail = MapObject(oReader);
			return oKnitDyeingGrayChallanDetail;
		}

		private List<KnitDyeingGrayChallanDetail> CreateObjects(IDataReader oReader)
		{
			List<KnitDyeingGrayChallanDetail> oKnitDyeingGrayChallanDetail = new List<KnitDyeingGrayChallanDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KnitDyeingGrayChallanDetail oItem = CreateObject(oHandler);
				oKnitDyeingGrayChallanDetail.Add(oItem);
			}
			return oKnitDyeingGrayChallanDetail;
		}

		#endregion

		#region Interface implementation
			public KnitDyeingGrayChallanDetail Save(KnitDyeingGrayChallanDetail oKnitDyeingGrayChallanDetail, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oKnitDyeingGrayChallanDetail.KnitDyeingGrayChallanDetailID <= 0)
					{
						reader = KnitDyeingGrayChallanDetailDA.InsertUpdate(tc, oKnitDyeingGrayChallanDetail, EnumDBOperation.Insert, nUserID, "");
					}
					else{
						reader = KnitDyeingGrayChallanDetailDA.InsertUpdate(tc, oKnitDyeingGrayChallanDetail, EnumDBOperation.Update, nUserID, "");
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oKnitDyeingGrayChallanDetail = new KnitDyeingGrayChallanDetail();
						oKnitDyeingGrayChallanDetail = CreateObject(oReader);
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
						oKnitDyeingGrayChallanDetail = new KnitDyeingGrayChallanDetail();
						oKnitDyeingGrayChallanDetail.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oKnitDyeingGrayChallanDetail;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					KnitDyeingGrayChallanDetail oKnitDyeingGrayChallanDetail = new KnitDyeingGrayChallanDetail();
					oKnitDyeingGrayChallanDetail.KnitDyeingGrayChallanDetailID = id;
					DBTableReferenceDA.HasReference(tc, "KnitDyeingGrayChallanDetail", id);
					KnitDyeingGrayChallanDetailDA.Delete(tc, oKnitDyeingGrayChallanDetail, EnumDBOperation.Delete, nUserId, "");
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

			public KnitDyeingGrayChallanDetail Get(int id, Int64 nUserId)
			{
				KnitDyeingGrayChallanDetail oKnitDyeingGrayChallanDetail = new KnitDyeingGrayChallanDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = KnitDyeingGrayChallanDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKnitDyeingGrayChallanDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KnitDyeingGrayChallanDetail", e);
					#endregion
				}
				return oKnitDyeingGrayChallanDetail;
			}

			public List<KnitDyeingGrayChallanDetail> Gets(Int64 nUserID)
			{
				List<KnitDyeingGrayChallanDetail> oKnitDyeingGrayChallanDetails = new List<KnitDyeingGrayChallanDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingGrayChallanDetailDA.Gets(tc);
					oKnitDyeingGrayChallanDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					KnitDyeingGrayChallanDetail oKnitDyeingGrayChallanDetail = new KnitDyeingGrayChallanDetail();
					oKnitDyeingGrayChallanDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oKnitDyeingGrayChallanDetails;
			}

			public List<KnitDyeingGrayChallanDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<KnitDyeingGrayChallanDetail> oKnitDyeingGrayChallanDetails = new List<KnitDyeingGrayChallanDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingGrayChallanDetailDA.Gets(tc, sSQL);
					oKnitDyeingGrayChallanDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get KnitDyeingGrayChallanDetail", e);
					#endregion
				}
				return oKnitDyeingGrayChallanDetails;
			}

		#endregion
	}

}

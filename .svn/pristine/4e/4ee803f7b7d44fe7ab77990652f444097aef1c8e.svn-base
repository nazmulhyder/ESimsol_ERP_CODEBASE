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
	public class KnitDyeingBatchDetailService : MarshalByRefObject, IKnitDyeingBatchDetailService
	{
		#region Private functions and declaration

		public static KnitDyeingBatchDetail MapObject(NullHandler oReader)
		{
			KnitDyeingBatchDetail oKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
			oKnitDyeingBatchDetail.KnitDyeingBatchDetailID = oReader.GetInt32("KnitDyeingBatchDetailID");
			oKnitDyeingBatchDetail.KnitDyeingBatchID = oReader.GetInt32("KnitDyeingBatchID");
			oKnitDyeingBatchDetail.KnitDyeingPTUID = oReader.GetInt32("KnitDyeingPTUID");
			oKnitDyeingBatchDetail.FabricTypeID = oReader.GetInt32("FabricTypeID");
			oKnitDyeingBatchDetail.GrayDiaID = oReader.GetInt32("GrayDiaID");
			oKnitDyeingBatchDetail.RollQty = oReader.GetDouble("RollQty");
			oKnitDyeingBatchDetail.GrayQty = oReader.GetDouble("GrayQty");
			oKnitDyeingBatchDetail.ProcessLoss = oReader.GetDouble("ProcessLoss");
			oKnitDyeingBatchDetail.FinishQty = oReader.GetDouble("FinishQty");
			oKnitDyeingBatchDetail.FinishDiaID = oReader.GetInt32("FinishDiaID");
			oKnitDyeingBatchDetail.FinishGSMID = oReader.GetInt32("FinishGSMID");
			oKnitDyeingBatchDetail.Remarks = oReader.GetString("Remarks");
            oKnitDyeingBatchDetail.FabricTypeName = oReader.GetString("FabricTypeName");
            oKnitDyeingBatchDetail.FinishGSMName = oReader.GetString("FinishGSMName");
            oKnitDyeingBatchDetail.FinishDiaName = oReader.GetString("FinishDiaName");
            oKnitDyeingBatchDetail.GrayDiaName = oReader.GetString("GrayDiaName");
            oKnitDyeingBatchDetail.GrayFabricID = oReader.GetInt32("GrayFabricID");
            oKnitDyeingBatchDetail.GrayFabricName = oReader.GetString("GrayFabricName");

			return oKnitDyeingBatchDetail;
		}

		public static KnitDyeingBatchDetail CreateObject(NullHandler oReader)
		{
			KnitDyeingBatchDetail oKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
			oKnitDyeingBatchDetail = MapObject(oReader);
			return oKnitDyeingBatchDetail;
		}

		private List<KnitDyeingBatchDetail> CreateObjects(IDataReader oReader)
		{
			List<KnitDyeingBatchDetail> oKnitDyeingBatchDetail = new List<KnitDyeingBatchDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KnitDyeingBatchDetail oItem = CreateObject(oHandler);
				oKnitDyeingBatchDetail.Add(oItem);
			}
			return oKnitDyeingBatchDetail;
		}

		#endregion

		#region Interface implementation
			public KnitDyeingBatchDetail Save(KnitDyeingBatchDetail oKnitDyeingBatchDetail, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oKnitDyeingBatchDetail.KnitDyeingBatchDetailID <= 0)
					{
						reader = KnitDyeingBatchDetailDA.InsertUpdate(tc, oKnitDyeingBatchDetail, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = KnitDyeingBatchDetailDA.InsertUpdate(tc, oKnitDyeingBatchDetail, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
						oKnitDyeingBatchDetail = CreateObject(oReader);
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
						oKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
						oKnitDyeingBatchDetail.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oKnitDyeingBatchDetail;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					KnitDyeingBatchDetail oKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
					oKnitDyeingBatchDetail.KnitDyeingBatchDetailID = id;
					KnitDyeingBatchDetailDA.Delete(tc, oKnitDyeingBatchDetail, EnumDBOperation.Delete, nUserId);
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
            public List<KnitDyeingBatchDetail> Gets(int id, Int64 nUserID)
            {
                List<KnitDyeingBatchDetail> oKnitDyeingBatchDetails = new List<KnitDyeingBatchDetail>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = KnitDyeingBatchDetailDA.Gets(tc, id);
                    oKnitDyeingBatchDetails = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    KnitDyeingBatchDetail oKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
                    oKnitDyeingBatchDetail.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oKnitDyeingBatchDetails;
            }
			public KnitDyeingBatchDetail Get(int id, Int64 nUserId)
			{
				KnitDyeingBatchDetail oKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = KnitDyeingBatchDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKnitDyeingBatchDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KnitDyeingBatchDetail", e);
					#endregion
				}
				return oKnitDyeingBatchDetail;
			}

			public List<KnitDyeingBatchDetail> Gets(Int64 nUserID)
			{
				List<KnitDyeingBatchDetail> oKnitDyeingBatchDetails = new List<KnitDyeingBatchDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingBatchDetailDA.Gets(tc);
					oKnitDyeingBatchDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					KnitDyeingBatchDetail oKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
					oKnitDyeingBatchDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oKnitDyeingBatchDetails;
			}

			public List<KnitDyeingBatchDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<KnitDyeingBatchDetail> oKnitDyeingBatchDetails = new List<KnitDyeingBatchDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingBatchDetailDA.Gets(tc, sSQL);
					oKnitDyeingBatchDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get KnitDyeingBatchDetail", e);
					#endregion
				}
				return oKnitDyeingBatchDetails;
			}

		#endregion
	}

}

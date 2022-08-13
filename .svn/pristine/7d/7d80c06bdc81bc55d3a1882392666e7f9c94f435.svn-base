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
	public class KnitDyeingPTUService : MarshalByRefObject, IKnitDyeingPTUService
	{
		#region Private functions and declaration

		private KnitDyeingPTU MapObject(NullHandler oReader)
		{
			KnitDyeingPTU oKnitDyeingPTU = new KnitDyeingPTU();
			oKnitDyeingPTU.KnitDyeingPTUID = oReader.GetInt32("KnitDyeingPTUID");
			oKnitDyeingPTU.KnitDyeingProgramDetailID = oReader.GetInt32("KnitDyeingProgramDetailID");
            oKnitDyeingPTU.KnitDyeingProgramID = oReader.GetInt32("KnitDyeingProgramID");
			oKnitDyeingPTU.ColorID = oReader.GetInt32("ColorID");
			oKnitDyeingPTU.GarmentsQty = oReader.GetDouble("GarmentsQty");
			oKnitDyeingPTU.GarmentsMUnitID = oReader.GetInt32("GarmentsMUnitID");
            oKnitDyeingPTU.FinishDiaID = oReader.GetInt32("FinishDiaID");
			oKnitDyeingPTU.FabricTypeID = oReader.GetInt32("FabricTypeID");
			oKnitDyeingPTU.GSMID = oReader.GetInt32("GSMID");
			oKnitDyeingPTU.CompositionID = oReader.GetInt32("CompositionID");
			oKnitDyeingPTU.PantoneNo = oReader.GetString("PantoneNo");
			oKnitDyeingPTU.ShadeRecipe = oReader.GetString("ShadeRecipe");
			oKnitDyeingPTU.ReqFabricQty = oReader.GetDouble("ReqFabricQty");
			oKnitDyeingPTU.MUnitID = oReader.GetInt32("MUnitID");
			oKnitDyeingPTU.KnitYarnBookQty = oReader.GetDouble("KnitYarnBookQty");
			oKnitDyeingPTU.KnitYarnIssueQty = oReader.GetDouble("KnitYarnIssueQty");
			oKnitDyeingPTU.KnitPipeLineQty = oReader.GetDouble("KnitPipeLineQty");
			oKnitDyeingPTU.KnitProcessLossQty = oReader.GetDouble("KnitProcessLossQty");
			oKnitDyeingPTU.KnitRejectQty = oReader.GetDouble("KnitRejectQty");
			oKnitDyeingPTU.GrayFabricRcvQty = oReader.GetDouble("GrayFabricRcvQty");
			oKnitDyeingPTU.DyeingIssueQty = oReader.GetDouble("DyeingIssueQty");
			oKnitDyeingPTU.DyeingPipeLineQty = oReader.GetDouble("DyeingPipeLineQty");
			oKnitDyeingPTU.ReDyeingQty = oReader.GetDouble("ReDyeingQty");
			oKnitDyeingPTU.DyeingGainLossQty = oReader.GetDouble("DyeingGainLossQty");
			oKnitDyeingPTU.DyeingFinishQty = oReader.GetDouble("DyeingFinishQty");
			oKnitDyeingPTU.ReFinishingQty = oReader.GetDouble("ReFinishingQty");
			oKnitDyeingPTU.FinishingGainLossQty = oReader.GetDouble("FinishingGainLossQty");
			oKnitDyeingPTU.FinishingQty = oReader.GetDouble("FinishingQty");
            oKnitDyeingPTU.ProcessLoss = oReader.GetDouble("ProcessLoss");
            oKnitDyeingPTU.DeliveryBalance = oReader.GetDouble("DeliveryBalance");
            oKnitDyeingPTU.DyeingBalance = oReader.GetDouble("DyeingBalance");
			oKnitDyeingPTU.ChallanQty = oReader.GetDouble("ChallanQty");
            oKnitDyeingPTU.UnitName = oReader.GetString("UnitName");
			oKnitDyeingPTU.FabricTypeName = oReader.GetString("FabricTypeName");
			oKnitDyeingPTU.GSMName = oReader.GetString("GSMName");
			oKnitDyeingPTU.FinishDiaName = oReader.GetString("FinishDiaName");
            oKnitDyeingPTU.ColorName = oReader.GetString("ColorName");
            oKnitDyeingPTU.RefObjectNo = oReader.GetString("RefObjectNo");
            oKnitDyeingPTU.CompositionName = oReader.GetString("CompositionName");
            oKnitDyeingPTU.PendingDyeingBatchQty = oReader.GetDouble("PendingDyeingBatchQty");
			return oKnitDyeingPTU;
		}

		private KnitDyeingPTU CreateObject(NullHandler oReader)
		{
			KnitDyeingPTU oKnitDyeingPTU = new KnitDyeingPTU();
			oKnitDyeingPTU = MapObject(oReader);
			return oKnitDyeingPTU;
		}

		private List<KnitDyeingPTU> CreateObjects(IDataReader oReader)
		{
			List<KnitDyeingPTU> oKnitDyeingPTU = new List<KnitDyeingPTU>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KnitDyeingPTU oItem = CreateObject(oHandler);
				oKnitDyeingPTU.Add(oItem);
			}
			return oKnitDyeingPTU;
		}

		#endregion

		#region Interface implementation
			public KnitDyeingPTU Save(KnitDyeingPTU oKnitDyeingPTU, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oKnitDyeingPTU.KnitDyeingPTUID <= 0)
					{
						reader = KnitDyeingPTUDA.InsertUpdate(tc, oKnitDyeingPTU, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = KnitDyeingPTUDA.InsertUpdate(tc, oKnitDyeingPTU, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oKnitDyeingPTU = new KnitDyeingPTU();
						oKnitDyeingPTU = CreateObject(oReader);
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
						oKnitDyeingPTU = new KnitDyeingPTU();
						oKnitDyeingPTU.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oKnitDyeingPTU;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					KnitDyeingPTU oKnitDyeingPTU = new KnitDyeingPTU();
					oKnitDyeingPTU.KnitDyeingPTUID = id;
					DBTableReferenceDA.HasReference(tc, "KnitDyeingPTU", id);
					KnitDyeingPTUDA.Delete(tc, oKnitDyeingPTU, EnumDBOperation.Delete, nUserId);
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

			public KnitDyeingPTU Get(int id, Int64 nUserId)
			{
				KnitDyeingPTU oKnitDyeingPTU = new KnitDyeingPTU();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = KnitDyeingPTUDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKnitDyeingPTU = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KnitDyeingPTU", e);
					#endregion
				}
				return oKnitDyeingPTU;
			}

			public List<KnitDyeingPTU> Gets(Int64 nUserID)
			{
				List<KnitDyeingPTU> oKnitDyeingPTUs = new List<KnitDyeingPTU>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingPTUDA.Gets(tc);
					oKnitDyeingPTUs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					KnitDyeingPTU oKnitDyeingPTU = new KnitDyeingPTU();
					oKnitDyeingPTU.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oKnitDyeingPTUs;
			}

			public List<KnitDyeingPTU> Gets (string sSQL, Int64 nUserID)
			{
				List<KnitDyeingPTU> oKnitDyeingPTUs = new List<KnitDyeingPTU>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingPTUDA.Gets(tc, sSQL);
					oKnitDyeingPTUs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get KnitDyeingPTU", e);
					#endregion
				}
				return oKnitDyeingPTUs;
			}

		#endregion
	}

}

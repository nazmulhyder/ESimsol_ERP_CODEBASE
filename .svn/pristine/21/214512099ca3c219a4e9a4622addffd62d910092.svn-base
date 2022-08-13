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
	public class KnitDyeingProgramDetailService : MarshalByRefObject, IKnitDyeingProgramDetailService
	{
		#region Private functions and declaration

		private KnitDyeingProgramDetail MapObject(NullHandler oReader)
		{
			KnitDyeingProgramDetail oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
			oKnitDyeingProgramDetail.KnitDyeingProgramDetailID = oReader.GetInt32("KnitDyeingProgramDetailID");
			oKnitDyeingProgramDetail.KnitDyeingProgramID = oReader.GetInt32("KnitDyeingProgramID");
            oKnitDyeingProgramDetail.KnitDyeingProgramDetailLogID = oReader.GetInt32("KnitDyeingProgramDetailLogID");
            oKnitDyeingProgramDetail.KnitDyeingProgramLogID = oReader.GetInt32("KnitDyeingProgramLogID");
            oKnitDyeingProgramDetail.RefObjectID = oReader.GetInt32("RefObjectID");
            oKnitDyeingProgramDetail.RefObjectNo = oReader.GetString("RefObjectNo");
            oKnitDyeingProgramDetail.RefProgramNo = oReader.GetString("RefProgramNo");
			oKnitDyeingProgramDetail.ColorID = oReader.GetInt32("ColorID");
            oKnitDyeingProgramDetail.ColorName = oReader.GetString("ColorName");
            oKnitDyeingProgramDetail.GarmentsQty = oReader.GetDouble("GarmentsQty");
            oKnitDyeingProgramDetail.GarmentsMUnitID = oReader.GetInt32("GarmentsMUnitID");
            oKnitDyeingProgramDetail.FabricTypeID = oReader.GetInt32("FabricTypeID");
            oKnitDyeingProgramDetail.FabricTypeName = oReader.GetString("FabricTypeName");
            oKnitDyeingProgramDetail.GSMID = oReader.GetInt32("GSMID");
            oKnitDyeingProgramDetail.ConsumPtionMUnitID = oReader.GetInt32("ConsumPtionMUnitID");
            
            oKnitDyeingProgramDetail.GSMName = oReader.GetString("GSMName");
            oKnitDyeingProgramDetail.CompositionID = oReader.GetInt32("CompositionID");
            oKnitDyeingProgramDetail.CompositionName = oReader.GetString("CompositionName");
            oKnitDyeingProgramDetail.ConsumptionPerDzn = oReader.GetDouble("ConsumptionPerDzn");
            oKnitDyeingProgramDetail.FinishDiaID = oReader.GetInt32("FinishDiaID");
            oKnitDyeingProgramDetail.FinishDiaName = oReader.GetString("FinishDiaName");
			oKnitDyeingProgramDetail.PantoneNo = oReader.GetString("PantoneNo");
            oKnitDyeingProgramDetail.ApprovedShade = oReader.GetInt32("ApprovedShade");
            oKnitDyeingProgramDetail.ShadeRecipe = oReader.GetString("ShadeRecipe");
			oKnitDyeingProgramDetail.ShadeRemarks = oReader.GetString("ShadeRemarks");
            oKnitDyeingProgramDetail.ReqFinishFabricQty = oReader.GetDouble("ReqFinishFabricQty");
            oKnitDyeingProgramDetail.GracePercent = oReader.GetDouble("GracePercent");
            oKnitDyeingProgramDetail.ReqFabricQty = oReader.GetDouble("ReqFabricQty");
            oKnitDyeingProgramDetail.MUnitID = oReader.GetInt32("MUnitID");
			oKnitDyeingProgramDetail.Remarks = oReader.GetString("Remarks");
            oKnitDyeingProgramDetail.StyleID = oReader.GetInt32("StyleID");
			oKnitDyeingProgramDetail.StyleNo = oReader.GetString("StyleNo");
			oKnitDyeingProgramDetail.BuyerName = oReader.GetString("BuyerName");
            oKnitDyeingProgramDetail.FabricID = oReader.GetInt32("FabricID");
			oKnitDyeingProgramDetail.FabricName = oReader.GetString("FabricName");
			oKnitDyeingProgramDetail.FinishGSMName = oReader.GetString("FinishGSMName");
            oKnitDyeingProgramDetail.TotalGarmentsQty = oReader.GetDouble("TotalGarmentsQty");
            oKnitDyeingProgramDetail.FinishGSMID = oReader.GetInt32("FinishGSMID");
            oKnitDyeingProgramDetail.GarmentsMUnitName = oReader.GetString("GarmentsMUnitName");
            oKnitDyeingProgramDetail.YetReqFabricQty = oReader.GetDouble("YetReqFabricQty");
            oKnitDyeingProgramDetail.KDProgramNo = oReader.GetString("KDProgramNo");

            return oKnitDyeingProgramDetail;
		}

		private KnitDyeingProgramDetail CreateObject(NullHandler oReader)
		{
			KnitDyeingProgramDetail oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
			oKnitDyeingProgramDetail = MapObject(oReader);
			return oKnitDyeingProgramDetail;
		}

		private List<KnitDyeingProgramDetail> CreateObjects(IDataReader oReader)
		{
			List<KnitDyeingProgramDetail> oKnitDyeingProgramDetail = new List<KnitDyeingProgramDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KnitDyeingProgramDetail oItem = CreateObject(oHandler);
				oKnitDyeingProgramDetail.Add(oItem);
			}
			return oKnitDyeingProgramDetail;
		}

		#endregion

		#region Interface implementation
			public KnitDyeingProgramDetail Save(KnitDyeingProgramDetail oKnitDyeingProgramDetail, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oKnitDyeingProgramDetail.KnitDyeingProgramDetailID <= 0)
					{
						reader = KnitDyeingProgramDetailDA.InsertUpdate(tc, oKnitDyeingProgramDetail, EnumDBOperation.Insert, nUserID,"");
					}
					else{
						reader = KnitDyeingProgramDetailDA.InsertUpdate(tc, oKnitDyeingProgramDetail, EnumDBOperation.Update, nUserID,"");
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
						oKnitDyeingProgramDetail = CreateObject(oReader);
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
						oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
						oKnitDyeingProgramDetail.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oKnitDyeingProgramDetail;
			}
            public List<KnitDyeingProgramDetail> Gets(int id, Int64 nUserID)
            {
                List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = KnitDyeingProgramDetailDA.Gets(tc, id);
                    oKnitDyeingProgramDetails = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    KnitDyeingProgramDetail oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                    oKnitDyeingProgramDetail.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oKnitDyeingProgramDetails;
            }
            //
            public List<KnitDyeingProgramDetail> GetsLog(int LogId, Int64 nUserID)
            {
                List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = KnitDyeingProgramDetailDA.GetsLog(tc, LogId);
                    oKnitDyeingProgramDetails = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    KnitDyeingProgramDetail oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                    oKnitDyeingProgramDetail.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oKnitDyeingProgramDetails;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					KnitDyeingProgramDetail oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
					oKnitDyeingProgramDetail.KnitDyeingProgramDetailID = id;
					DBTableReferenceDA.HasReference(tc, "KnitDyeingProgramDetail", id);
					KnitDyeingProgramDetailDA.Delete(tc, oKnitDyeingProgramDetail, EnumDBOperation.Delete, nUserId,"");
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

			public KnitDyeingProgramDetail Get(int id, Int64 nUserId)
			{
				KnitDyeingProgramDetail oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = KnitDyeingProgramDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKnitDyeingProgramDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KnitDyeingProgramDetail", e);
					#endregion
				}
				return oKnitDyeingProgramDetail;
			}

			public List<KnitDyeingProgramDetail> Gets(Int64 nUserID)
			{
				List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingProgramDetailDA.Gets(tc);
					oKnitDyeingProgramDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					KnitDyeingProgramDetail oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
					oKnitDyeingProgramDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oKnitDyeingProgramDetails;
			}

			public List<KnitDyeingProgramDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingProgramDetailDA.Gets(tc, sSQL);
					oKnitDyeingProgramDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get KnitDyeingProgramDetail", e);
					#endregion
				}
				return oKnitDyeingProgramDetails;
			}

		#endregion
	}

}

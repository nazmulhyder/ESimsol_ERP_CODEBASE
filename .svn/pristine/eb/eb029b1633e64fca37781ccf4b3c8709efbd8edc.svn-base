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
	public class LotMixingDetailService : MarshalByRefObject, ILotMixingDetailService
	{
		#region Private functions and declaration

		private LotMixingDetail MapObject(NullHandler oReader)
		{
			LotMixingDetail oLotMixingDetail = new LotMixingDetail();
			oLotMixingDetail.LotMixingDetailID = oReader.GetInt32("LotMixingDetailID");
			oLotMixingDetail.LotMixingID = oReader.GetInt32("LotMixingID");

			oLotMixingDetail.ProductID = oReader.GetInt32("ProductID");
			oLotMixingDetail.LotID = oReader.GetInt32("LotID");
			oLotMixingDetail.Qty = oReader.GetDouble("Qty");
			oLotMixingDetail.Qty_Percentage  = oReader.GetDouble("Qty_Percentage");
			oLotMixingDetail.MUnitID = oReader.GetInt32("MUnitID");
			oLotMixingDetail.BagCount = oReader.GetDouble("BagCount");
			oLotMixingDetail.UnitPrice = oReader.GetDouble("UnitPrice");
			oLotMixingDetail.CurrencyID = oReader.GetInt32("CurrencyID");
			oLotMixingDetail.Remarks = oReader.GetString("Remarks");
			oLotMixingDetail.IsLotMendatory = oReader.GetBoolean("IsLotMendatory");
            oLotMixingDetail.InOutType = (EnumInOutType)oReader.GetInt32("InOutType");
            oLotMixingDetail.InOutTypeInt = oReader.GetInt32("InOutType");
            oLotMixingDetail.LotNo = oReader.GetString("LotNo");
            oLotMixingDetail.ProductCode = oReader.GetString("ProductCode");
            oLotMixingDetail.ProductName = oReader.GetString("ProductName");
            oLotMixingDetail.MUnit = oReader.GetString("MUnit");
            oLotMixingDetail.MUSymbol = oReader.GetString("MUSymbol");
            oLotMixingDetail.CurrenName = oReader.GetString("CurrenName");
            oLotMixingDetail.CurrenSymbol = oReader.GetString("CurrenSymbol");

            oLotMixingDetail.LotBalance = oReader.GetDouble("LotBalance");
			return oLotMixingDetail;
		}

		private LotMixingDetail CreateObject(NullHandler oReader)
		{
			LotMixingDetail oLotMixingDetail = new LotMixingDetail();
			oLotMixingDetail = MapObject(oReader);
			return oLotMixingDetail;
		}

		private List<LotMixingDetail> CreateObjects(IDataReader oReader)
		{
			List<LotMixingDetail> oLotMixingDetail = new List<LotMixingDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				LotMixingDetail oItem = CreateObject(oHandler);
				oLotMixingDetail.Add(oItem);
			}
			return oLotMixingDetail;
		}

		#endregion

		#region Interface implementation
			public LotMixingDetail Save(LotMixingDetail oLotMixingDetail, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oLotMixingDetail.LotMixingDetailID <= 0)
					{
						reader = LotMixingDetailDA.InsertUpdate(tc, oLotMixingDetail, EnumDBOperation.Insert, nUserID,"");
					}
					else{
                        reader = LotMixingDetailDA.InsertUpdate(tc, oLotMixingDetail, EnumDBOperation.Update, nUserID, "");
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oLotMixingDetail = new LotMixingDetail();
						oLotMixingDetail = CreateObject(oReader);
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
						oLotMixingDetail = new LotMixingDetail();
						oLotMixingDetail.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oLotMixingDetail;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					LotMixingDetail oLotMixingDetail = new LotMixingDetail();
					oLotMixingDetail.LotMixingDetailID = id;
					DBTableReferenceDA.HasReference(tc, "LotMixingDetail", id);
					LotMixingDetailDA.Delete(tc, oLotMixingDetail, EnumDBOperation.Delete, nUserId,"");
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

			public LotMixingDetail Get(int id, Int64 nUserId)
			{
				LotMixingDetail oLotMixingDetail = new LotMixingDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = LotMixingDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oLotMixingDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get LotMixingDetail", e);
					#endregion
				}
				return oLotMixingDetail;
			}

			public List<LotMixingDetail> Gets(Int64 nUserID)
			{
				List<LotMixingDetail> oLotMixingDetails = new List<LotMixingDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = LotMixingDetailDA.Gets(tc);
					oLotMixingDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					LotMixingDetail oLotMixingDetail = new LotMixingDetail();
					oLotMixingDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oLotMixingDetails;
			}

			public List<LotMixingDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<LotMixingDetail> oLotMixingDetails = new List<LotMixingDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = LotMixingDetailDA.Gets(tc, sSQL);
					oLotMixingDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get LotMixingDetail", e);
					#endregion
				}
				return oLotMixingDetails;
			}

		#endregion
	}

}

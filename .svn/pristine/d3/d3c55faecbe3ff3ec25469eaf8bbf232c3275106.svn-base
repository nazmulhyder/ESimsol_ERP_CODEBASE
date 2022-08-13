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
	public class DyeingOrderFabricDetailService : MarshalByRefObject, IDyeingOrderFabricDetailService
	{
		#region Private functions and declaration

		private DyeingOrderFabricDetail MapObject(NullHandler oReader)
		{
			DyeingOrderFabricDetail oDyeingOrderFabricDetail = new DyeingOrderFabricDetail();
			oDyeingOrderFabricDetail.DyeingOrderFabricDetailID = oReader.GetInt32("DyeingOrderFabricDetailID");
			oDyeingOrderFabricDetail.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
			oDyeingOrderFabricDetail.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
			oDyeingOrderFabricDetail.FSCDetailID = oReader.GetInt32("FSCDetailID");
			oDyeingOrderFabricDetail.FEOSID = oReader.GetInt32("FEOSID");
			oDyeingOrderFabricDetail.FEOSDID = oReader.GetInt32("FEOSDID");
			oDyeingOrderFabricDetail.SLNo = oReader.GetString("SLNo");
            oDyeingOrderFabricDetail.Qty = oReader.GetDouble("Qty");
            oDyeingOrderFabricDetail.QtyDyed = oReader.GetDouble("QtyDyed");
            oDyeingOrderFabricDetail.Qty_Req = oReader.GetDouble("Qty_Req");
            oDyeingOrderFabricDetail.Qty_RS = oReader.GetDouble("Qty_RS");
            oDyeingOrderFabricDetail.Qty_Assign = oReader.GetDouble("Qty_Assign");
            oDyeingOrderFabricDetail.WarpWeftType = (EnumWarpWeft)oReader.GetInt32("WarpWeftType");
            oDyeingOrderFabricDetail.WarpWeftTypeInt = oReader.GetInt32("WarpWeftType");
			oDyeingOrderFabricDetail.ProductID = oReader.GetInt32("ProductID");
			oDyeingOrderFabricDetail.BuyerReference = oReader.GetString("BuyerReference");
			oDyeingOrderFabricDetail.ColorInfo = oReader.GetString("ColorInfo");
			oDyeingOrderFabricDetail.StyleNo = oReader.GetString("StyleNo");
			oDyeingOrderFabricDetail.FinishType = oReader.GetInt32("FinishType");
			oDyeingOrderFabricDetail.ProcessType = oReader.GetInt32("ProcessType");
			oDyeingOrderFabricDetail.Construction = oReader.GetString("Construction");
			oDyeingOrderFabricDetail.FabricWidth = oReader.GetString("FabricWidth");
			oDyeingOrderFabricDetail.ExeNo = oReader.GetString("ExeNo");
			oDyeingOrderFabricDetail.IsWarp = oReader.GetBoolean("IsWarp");
			oDyeingOrderFabricDetail.ColorName = oReader.GetString("ColorName");
			oDyeingOrderFabricDetail.EndsCount = oReader.GetInt32("EndsCount");
			oDyeingOrderFabricDetail.BatchNo = oReader.GetString("BatchNo");

            oDyeingOrderFabricDetail.OrderNo = oReader.GetString("OrderNo");
            oDyeingOrderFabricDetail.OrderDate = oReader.GetDateTime("OrderDate");
            oDyeingOrderFabricDetail.CustomerName = oReader.GetString("CustomerName");
            oDyeingOrderFabricDetail.BuyerName = oReader.GetString("BuyerName");
            oDyeingOrderFabricDetail.ProductName = oReader.GetString("ProductName");
            oDyeingOrderFabricDetail.LotNo = oReader.GetString("LotNo");
            oDyeingOrderFabricDetail.ProductID = oReader.GetInt32("ProductID");
            oDyeingOrderFabricDetail.LengthReq = oReader.GetDouble("LengthReq");
            oDyeingOrderFabricDetail.ConeReq = oReader.GetDouble("ConeReq");
			return oDyeingOrderFabricDetail;
		}

		private DyeingOrderFabricDetail CreateObject(NullHandler oReader)
		{
			DyeingOrderFabricDetail oDyeingOrderFabricDetail = new DyeingOrderFabricDetail();
			oDyeingOrderFabricDetail = MapObject(oReader);
			return oDyeingOrderFabricDetail;
		}

		private List<DyeingOrderFabricDetail> CreateObjects(IDataReader oReader)
		{
			List<DyeingOrderFabricDetail> oDyeingOrderFabricDetail = new List<DyeingOrderFabricDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				DyeingOrderFabricDetail oItem = CreateObject(oHandler);
				oDyeingOrderFabricDetail.Add(oItem);
			}
			return oDyeingOrderFabricDetail;
		}

		#endregion

		#region Interface implementation
			public DyeingOrderFabricDetail Save(DyeingOrderFabricDetail oDyeingOrderFabricDetail, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oDyeingOrderFabricDetail.DyeingOrderFabricDetailID <= 0)
					{
						reader = DyeingOrderFabricDetailDA.InsertUpdate(tc, oDyeingOrderFabricDetail, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = DyeingOrderFabricDetailDA.InsertUpdate(tc, oDyeingOrderFabricDetail, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oDyeingOrderFabricDetail = new DyeingOrderFabricDetail();
						oDyeingOrderFabricDetail = CreateObject(oReader);
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
						oDyeingOrderFabricDetail = new DyeingOrderFabricDetail();
						oDyeingOrderFabricDetail.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oDyeingOrderFabricDetail;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					DyeingOrderFabricDetail oDyeingOrderFabricDetail = new DyeingOrderFabricDetail();
					oDyeingOrderFabricDetail.DyeingOrderFabricDetailID = id;
					DBTableReferenceDA.HasReference(tc, "DyeingOrderFabricDetail", id);
					DyeingOrderFabricDetailDA.Delete(tc, oDyeingOrderFabricDetail, EnumDBOperation.Delete, nUserId);
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

			public DyeingOrderFabricDetail Get(int id, Int64 nUserId)
			{
				DyeingOrderFabricDetail oDyeingOrderFabricDetail = new DyeingOrderFabricDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = DyeingOrderFabricDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oDyeingOrderFabricDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get DyeingOrderFabricDetail", e);
					#endregion
				}
				return oDyeingOrderFabricDetail;
			}

			public List<DyeingOrderFabricDetail> Gets(Int64 nUserID)
			{
				List<DyeingOrderFabricDetail> oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = DyeingOrderFabricDetailDA.Gets(tc);
					oDyeingOrderFabricDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					DyeingOrderFabricDetail oDyeingOrderFabricDetail = new DyeingOrderFabricDetail();
					oDyeingOrderFabricDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oDyeingOrderFabricDetails;
			}

			public List<DyeingOrderFabricDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<DyeingOrderFabricDetail> oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = DyeingOrderFabricDetailDA.Gets(tc, sSQL);
					oDyeingOrderFabricDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get DyeingOrderFabricDetail", e);
					#endregion
				}
				return oDyeingOrderFabricDetails;
			}

		#endregion
	}

}

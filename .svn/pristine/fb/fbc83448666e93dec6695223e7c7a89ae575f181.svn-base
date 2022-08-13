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
	public class OrderSheetDetailService : MarshalByRefObject, IOrderSheetDetailService
	{
		#region Private functions and declaration

		private OrderSheetDetail MapObject(NullHandler oReader)
		{
			OrderSheetDetail oOrderSheetDetail = new OrderSheetDetail();
			oOrderSheetDetail.OrderSheetDetailID = oReader.GetInt32("OrderSheetDetailID");
            oOrderSheetDetail.OrderSheetID = oReader.GetInt32("OrderSheetID");
            oOrderSheetDetail.OrderSheetLogID = oReader.GetInt32("OrderSheetLogID");
            oOrderSheetDetail.OrderSheetDetailLogID = oReader.GetInt32("OrderSheetDetailLogID");
			oOrderSheetDetail.ProductID = oReader.GetInt32("ProductID");
			oOrderSheetDetail.ColorID = oReader.GetInt32("ColorID");
            oOrderSheetDetail.PolyMUnitID = oReader.GetInt32("PolyMUnitID");
            
            oOrderSheetDetail.ProductDescription = oReader.GetString("ProductDescription");
			oOrderSheetDetail.StyleDescription = oReader.GetString("StyleDescription");
			oOrderSheetDetail.Measurement = oReader.GetString("Measurement");
			oOrderSheetDetail.Qty = oReader.GetDouble("Qty");
			oOrderSheetDetail.UnitPrice = oReader.GetDouble("UnitPrice");
			oOrderSheetDetail.DeliveryDate = oReader.GetDateTime("DeliveryDate");
			oOrderSheetDetail.BuyerRef = oReader.GetString("BuyerRef");
			oOrderSheetDetail.Note = oReader.GetString("Note");
			oOrderSheetDetail.ModelReferenceID = oReader.GetInt32("ModelReferenceID");
            oOrderSheetDetail.ProductCode = oReader.GetString("ProductCode");
			oOrderSheetDetail.ProductName = oReader.GetString("ProductName");
			oOrderSheetDetail.ModelReferenceName = oReader.GetString("ModelReferenceName");
			oOrderSheetDetail.ColorName = oReader.GetString("ColorName");
			oOrderSheetDetail.SizeName = oReader.GetString("SizeName");
            oOrderSheetDetail.UnitID = oReader.GetInt32("UnitID");
            oOrderSheetDetail.UnitName = oReader.GetString("UnitName");
            oOrderSheetDetail.UnitSymbol = oReader.GetString("UnitSymbol");
            oOrderSheetDetail.YetToPIQty = oReader.GetDouble("YetToPIQty");
            oOrderSheetDetail.IsApplySizer = oReader.GetBoolean("IsApplySizer");
            oOrderSheetDetail.Amount = oReader.GetDouble("Amount");
            oOrderSheetDetail.ColorQty = oReader.GetInt32("ColorQty");

            oOrderSheetDetail.RecipeID = oReader.GetInt32("RecipeID");
            oOrderSheetDetail.RecipeName = oReader.GetString("RecipeName");
			return oOrderSheetDetail;
		}

		private OrderSheetDetail CreateObject(NullHandler oReader)
		{
			OrderSheetDetail oOrderSheetDetail = new OrderSheetDetail();
			oOrderSheetDetail = MapObject(oReader);
			return oOrderSheetDetail;
		}

		private List<OrderSheetDetail> CreateObjects(IDataReader oReader)
		{
			List<OrderSheetDetail> oOrderSheetDetail = new List<OrderSheetDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				OrderSheetDetail oItem = CreateObject(oHandler);
				oOrderSheetDetail.Add(oItem);
			}
			return oOrderSheetDetail;
		}

		#endregion

		#region Interface implementation
		
	

			public OrderSheetDetail Get(int id, Int64 nUserId)
			{
				OrderSheetDetail oOrderSheetDetail = new OrderSheetDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = OrderSheetDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oOrderSheetDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get OrderSheetDetail", e);
					#endregion
				}
				return oOrderSheetDetail;
			}

			public List<OrderSheetDetail> Gets(int nORSID, Int64 nUserID)
			{
				List<OrderSheetDetail> oOrderSheetDetails = new List<OrderSheetDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = OrderSheetDetailDA.Gets(nORSID,tc);
					oOrderSheetDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					OrderSheetDetail oOrderSheetDetail = new OrderSheetDetail();
					oOrderSheetDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oOrderSheetDetails;
			}

            public List<OrderSheetDetail> GetsByLog(int nORSLogID, Int64 nUserID)
            {
                List<OrderSheetDetail> oOrderSheetDetails = new List<OrderSheetDetail>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = OrderSheetDetailDA.GetsByLog(nORSLogID, tc);
                    oOrderSheetDetails = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    OrderSheetDetail oOrderSheetDetail = new OrderSheetDetail();
                    oOrderSheetDetail.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oOrderSheetDetails;
            }

			public List<OrderSheetDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<OrderSheetDetail> oOrderSheetDetails = new List<OrderSheetDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = OrderSheetDetailDA.Gets(tc, sSQL);
					oOrderSheetDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get OrderSheetDetail", e);
					#endregion
				}
				return oOrderSheetDetails;
			}

		#endregion
	}

}

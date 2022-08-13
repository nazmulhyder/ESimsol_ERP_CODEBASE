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
	public class WorkOrderDetailService : MarshalByRefObject, IWorkOrderDetailService
	{
		#region Private functions and declaration

		private WorkOrderDetail MapObject(NullHandler oReader)
		{
			WorkOrderDetail oWorkOrderDetail = new WorkOrderDetail();
            oWorkOrderDetail.WorkOrderDetailID = oReader.GetInt32("WorkOrderDetailID");
            oWorkOrderDetail.WorkOrderID = oReader.GetInt32("WorkOrderID");
            oWorkOrderDetail.ProductID = oReader.GetInt32("ProductID");
            oWorkOrderDetail.StyleID = oReader.GetInt32("StyleID");
            oWorkOrderDetail.ColorID = oReader.GetInt32("ColorID");
            oWorkOrderDetail.SizeID = oReader.GetInt32("SizeID");
            oWorkOrderDetail.Measurement = oReader.GetString("Measurement");
            oWorkOrderDetail.ProductDescription = oReader.GetString("ProductDescription");
            oWorkOrderDetail.UnitID = oReader.GetInt32("UnitID");
            oWorkOrderDetail.Qty = oReader.GetDouble("Qty");
            oWorkOrderDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oWorkOrderDetail.ProductCode = oReader.GetString("ProductCode");
            oWorkOrderDetail.ProductName = oReader.GetString("ProductName");
            oWorkOrderDetail.IsApplyColor = oReader.GetBoolean("IsApplyColor");
            oWorkOrderDetail.IsApplySize = oReader.GetBoolean("IsApplySize");
            oWorkOrderDetail.ColorName = oReader.GetString("ColorName");
            oWorkOrderDetail.SizeName = oReader.GetString("SizeName");
            oWorkOrderDetail.StyleNo = oReader.GetString("StyleNo");
            oWorkOrderDetail.BuyerName = oReader.GetString("BuyerName");
            oWorkOrderDetail.UnitName = oReader.GetString("UnitName");
            oWorkOrderDetail.UnitSymbol = oReader.GetString("UnitSymbol");
            oWorkOrderDetail.WorkOrderNo = oReader.GetString("WorkOrderNo");
            oWorkOrderDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oWorkOrderDetail.Amount = oReader.GetDouble("Amount");
            oWorkOrderDetail.GRNQty = oReader.GetDouble("GRNQty");
            oWorkOrderDetail.YetToGRNQty = oReader.GetDouble("YetToGRNQty");
            oWorkOrderDetail.WorkOrderDetailLogID = oReader.GetInt32("WorkOrderDetailLogID");
            oWorkOrderDetail.WorkOrderLogID = oReader.GetInt32("WorkOrderLogID");
            oWorkOrderDetail.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oWorkOrderDetail.WorkOrderStatus =  (EnumWorkOrderStatus) oReader.GetInt32("WorkOrderStatus");
            oWorkOrderDetail.WorkOrderStatusInInt = oReader.GetInt32("WorkOrderStatus");
            oWorkOrderDetail.RateUnit = oReader.GetDouble("RateUnit");
            oWorkOrderDetail.LotBalance = oReader.GetDouble("LotBalance");
            oWorkOrderDetail.YetToPurchaseReturnQty = oReader.GetDouble("YetToPurchaseReturnQty");
            oWorkOrderDetail.LotNo = oReader.GetString("LotNo");
            oWorkOrderDetail.LotID = oReader.GetInt32("LotID");
            oWorkOrderDetail.MCDia = oReader.GetString("MCDia");
            oWorkOrderDetail.FinishDia = oReader.GetString("FinishDia");
            oWorkOrderDetail.GSM = oReader.GetString("GSM");
            oWorkOrderDetail.Remarks = oReader.GetString("Remarks");
            oWorkOrderDetail.Stretch_Length = oReader.GetString("Stretch_Length");            
			return oWorkOrderDetail;
		}

		private WorkOrderDetail CreateObject(NullHandler oReader)
		{
			WorkOrderDetail oWorkOrderDetail = new WorkOrderDetail();
			oWorkOrderDetail = MapObject(oReader);
			return oWorkOrderDetail;
		}

		private List<WorkOrderDetail> CreateObjects(IDataReader oReader)
		{
			List<WorkOrderDetail> oWorkOrderDetail = new List<WorkOrderDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				WorkOrderDetail oItem = CreateObject(oHandler);
				oWorkOrderDetail.Add(oItem);
			}
			return oWorkOrderDetail;
		}

		#endregion

		#region Interface implementation
		
	

			public WorkOrderDetail Get(int id, Int64 nUserId)
			{
				WorkOrderDetail oWorkOrderDetail = new WorkOrderDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = WorkOrderDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oWorkOrderDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get WorkOrderDetail", e);
					#endregion
				}
				return oWorkOrderDetail;
			}

			public List<WorkOrderDetail> Gets(int nORSID, Int64 nUserID)
			{
				List<WorkOrderDetail> oWorkOrderDetails = new List<WorkOrderDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = WorkOrderDetailDA.Gets(nORSID,tc);
					oWorkOrderDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					WorkOrderDetail oWorkOrderDetail = new WorkOrderDetail();
					oWorkOrderDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oWorkOrderDetails;
			}

            public List<WorkOrderDetail> GetsByLog(int nORSLogID, Int64 nUserID)
            {
                List<WorkOrderDetail> oWorkOrderDetails = new List<WorkOrderDetail>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = WorkOrderDetailDA.GetsByLog(nORSLogID, tc);
                    oWorkOrderDetails = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    WorkOrderDetail oWorkOrderDetail = new WorkOrderDetail();
                    oWorkOrderDetail.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oWorkOrderDetails;
            }

			public List<WorkOrderDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<WorkOrderDetail> oWorkOrderDetails = new List<WorkOrderDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = WorkOrderDetailDA.Gets(tc, sSQL);
					oWorkOrderDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get WorkOrderDetail", e);
					#endregion
				}
				return oWorkOrderDetails;
			}

		#endregion
	}

}

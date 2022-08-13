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
	public class ProductionOrderDetailService : MarshalByRefObject, IProductionOrderDetailService
	{
		#region Private functions and declaration

		private ProductionOrderDetail MapObject(NullHandler oReader)
		{
			ProductionOrderDetail oProductionOrderDetail = new ProductionOrderDetail();
			oProductionOrderDetail.ProductionOrderDetailID = oReader.GetInt32("ProductionOrderDetailID");
            oProductionOrderDetail.ProductionOrderID = oReader.GetInt32("ProductionOrderID");
            oProductionOrderDetail.ProductionOrderLogID = oReader.GetInt32("ProductionOrderLogID");
            oProductionOrderDetail.ProductionOrderDetailLogID = oReader.GetInt32("ProductionOrderDetailLogID");
			oProductionOrderDetail.ProductID = oReader.GetInt32("ProductID");
			oProductionOrderDetail.ColorID = oReader.GetInt32("ColorID");
            oProductionOrderDetail.ProductDescription = oReader.GetString("ProductDescription");
			oProductionOrderDetail.StyleDescription = oReader.GetString("StyleDescription");
            oProductionOrderDetail.PolyMUnitID = oReader.GetInt32("PolyMUnitID");
			oProductionOrderDetail.Measurement = oReader.GetString("Measurement");
			oProductionOrderDetail.Qty = oReader.GetDouble("Qty");
			oProductionOrderDetail.BuyerRef = oReader.GetString("BuyerRef");
			oProductionOrderDetail.Note = oReader.GetString("Note");
			oProductionOrderDetail.ModelReferenceID = oReader.GetInt32("ModelReferenceID");
            oProductionOrderDetail.ProductCode = oReader.GetString("ProductCode");
			oProductionOrderDetail.ProductName = oReader.GetString("ProductName");
			oProductionOrderDetail.ModelReferenceName = oReader.GetString("ModelReferenceName");
			oProductionOrderDetail.ColorName = oReader.GetString("ColorName");
			oProductionOrderDetail.SizeName = oReader.GetString("SizeName");
            oProductionOrderDetail.SizeInCM = oReader.GetString("SizeInCM");
            oProductionOrderDetail.UnitID = oReader.GetInt32("UnitID");
            oProductionOrderDetail.UnitName = oReader.GetString("UnitName");
            oProductionOrderDetail.UnitSymbol = oReader.GetString("UnitSymbol");
            oProductionOrderDetail.YetToProductionOrderQty = oReader.GetDouble("YetToProductionOrderQty");
            oProductionOrderDetail.ExportSCDetailID = oReader.GetInt32("ExportSCDetailID");
            oProductionOrderDetail.YetToProductionSheeteQty = oReader.GetDouble("YetToProductionSheeteQty");
            oProductionOrderDetail.IsApplySizer = oReader.GetBoolean("IsApplySizer");
            oProductionOrderDetail.ColorQty = oReader.GetInt32("ColorQty");
            oProductionOrderDetail.ExportPINo = oReader.GetString("ExportPINo");
            oProductionOrderDetail.PONo = oReader.GetString("PONo");
            oProductionOrderDetail.ContractorID = oReader.GetInt32("ContractorID");
            oProductionOrderDetail.ContractorName = oReader.GetString("ContractorName");
            oProductionOrderDetail.BUID = oReader.GetInt32("BUID");
            oProductionOrderDetail.FinishGoodsWeight = oReader.GetDouble("FinishGoodsWeight");
            oProductionOrderDetail.NaliWeight = oReader.GetDouble("NaliWeight");
            oProductionOrderDetail.WeigthFor = oReader.GetDouble("WeigthFor");
            oProductionOrderDetail.FinishGoodsUnit = oReader.GetInt32("FinishGoodsUnit");
            oProductionOrderDetail.FGUSymbol = oReader.GetString("FGUSymbol");
            oProductionOrderDetail.Model = oReader.GetString("Model");
            oProductionOrderDetail.PantonNo = oReader.GetString("PantonNo");
            oProductionOrderDetail.ProductNature = (EnumProductNature)oReader.GetInt32("ProductNature");
            oProductionOrderDetail.ProductNatureInInt = oReader.GetInt32("ProductNature"); ;
			return oProductionOrderDetail;
		}

		private ProductionOrderDetail CreateObject(NullHandler oReader)
		{
			ProductionOrderDetail oProductionOrderDetail = new ProductionOrderDetail();
			oProductionOrderDetail = MapObject(oReader);
			return oProductionOrderDetail;
		}

		private List<ProductionOrderDetail> CreateObjects(IDataReader oReader)
		{
			List<ProductionOrderDetail> oProductionOrderDetail = new List<ProductionOrderDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				ProductionOrderDetail oItem = CreateObject(oHandler);
				oProductionOrderDetail.Add(oItem);
			}
			return oProductionOrderDetail;
		}

		#endregion

		#region Interface implementation
		
	

			public ProductionOrderDetail Get(int id, Int64 nUserId)
			{
				ProductionOrderDetail oProductionOrderDetail = new ProductionOrderDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = ProductionOrderDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oProductionOrderDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get ProductionOrderDetail", e);
					#endregion
				}
				return oProductionOrderDetail;
			}

			public List<ProductionOrderDetail> Gets(int nPOID, Int64 nUserID)
			{
				List<ProductionOrderDetail> oProductionOrderDetails = new List<ProductionOrderDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = ProductionOrderDetailDA.Gets(nPOID,tc);
					oProductionOrderDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					ProductionOrderDetail oProductionOrderDetail = new ProductionOrderDetail();
					oProductionOrderDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oProductionOrderDetails;
			}
            //
            public List<ProductionOrderDetail> GetsByLog(int nPOLogID, Int64 nUserID)
            {
                List<ProductionOrderDetail> oProductionOrderDetails = new List<ProductionOrderDetail>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = ProductionOrderDetailDA.GetsByLog(nPOLogID, tc);
                    oProductionOrderDetails = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    ProductionOrderDetail oProductionOrderDetail = new ProductionOrderDetail();
                    oProductionOrderDetail.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oProductionOrderDetails;
            }
			public List<ProductionOrderDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<ProductionOrderDetail> oProductionOrderDetails = new List<ProductionOrderDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ProductionOrderDetailDA.Gets(tc, sSQL);
					oProductionOrderDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get ProductionOrderDetail", e);
					#endregion
				}
				return oProductionOrderDetails;
			}

		#endregion
	}

}

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
	public class PurchaseReturnDetailService : MarshalByRefObject, IPurchaseReturnDetailService
	{
		#region Private functions and declaration

		private PurchaseReturnDetail MapObject(NullHandler oReader)
		{
			PurchaseReturnDetail oPurchaseReturnDetail = new PurchaseReturnDetail();
			oPurchaseReturnDetail.PurchaseReturnDetailID = oReader.GetInt32("PurchaseReturnDetailID");
			oPurchaseReturnDetail.PurchaseReturnID = oReader.GetInt32("PurchaseReturnID");
			oPurchaseReturnDetail.ProductID = oReader.GetInt32("ProductID");
			oPurchaseReturnDetail.MUnitID = oReader.GetInt32("MUnitID");
			oPurchaseReturnDetail.LotID = oReader.GetInt32("LotID");
            oPurchaseReturnDetail.RefTypeInt = oReader.GetInt32("RefType");
			oPurchaseReturnDetail.StyleID = oReader.GetInt32("StyleID");
			oPurchaseReturnDetail.RefObjectDetailID = oReader.GetInt32("RefObjectDetailID");
			oPurchaseReturnDetail.ColorID = oReader.GetInt32("ColorID");
			oPurchaseReturnDetail.SizeID = oReader.GetInt32("SizeID");
			oPurchaseReturnDetail.ReturnQty = oReader.GetDouble("ReturnQty");
			oPurchaseReturnDetail.ProductCode = oReader.GetString("ProductCode");
			oPurchaseReturnDetail.ProductName = oReader.GetString("ProductName");
			oPurchaseReturnDetail.MUName = oReader.GetString("MUName");
            oPurchaseReturnDetail.MUSymbol = oReader.GetString("MUSymbol");
			oPurchaseReturnDetail.LotNo = oReader.GetString("LotNo");
			oPurchaseReturnDetail.StyleNo = oReader.GetString("StyleNo");
			oPurchaseReturnDetail.ColorName = oReader.GetString("ColorName");
			oPurchaseReturnDetail.SizeName = oReader.GetString("SizeName");
			oPurchaseReturnDetail.RefDetailQty = oReader.GetDouble("RefDetailQty");
            oPurchaseReturnDetail.YetToPurchaseReturnQty = oReader.GetDouble("YetToPurchaseReturnQty");
            oPurchaseReturnDetail.LotBalance = oReader.GetDouble("LotBalance");

			return oPurchaseReturnDetail;
		}

		private PurchaseReturnDetail CreateObject(NullHandler oReader)
		{
			PurchaseReturnDetail oPurchaseReturnDetail = new PurchaseReturnDetail();
			oPurchaseReturnDetail = MapObject(oReader);
			return oPurchaseReturnDetail;
		}

		private List<PurchaseReturnDetail> CreateObjects(IDataReader oReader)
		{
			List<PurchaseReturnDetail> oPurchaseReturnDetail = new List<PurchaseReturnDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				PurchaseReturnDetail oItem = CreateObject(oHandler);
				oPurchaseReturnDetail.Add(oItem);
			}
			return oPurchaseReturnDetail;
		}

		#endregion

		#region Interface implementation


	
			public PurchaseReturnDetail Get(int id, Int64 nUserId)
			{
				PurchaseReturnDetail oPurchaseReturnDetail = new PurchaseReturnDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = PurchaseReturnDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oPurchaseReturnDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get PurchaseReturnDetail", e);
					#endregion
				}
				return oPurchaseReturnDetail;
			}

			public List<PurchaseReturnDetail> Gets(int id, Int64 nUserID)
			{
				List<PurchaseReturnDetail> oPurchaseReturnDetails = new List<PurchaseReturnDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PurchaseReturnDetailDA.Gets(id, tc);
					oPurchaseReturnDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					PurchaseReturnDetail oPurchaseReturnDetail = new PurchaseReturnDetail();
					oPurchaseReturnDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oPurchaseReturnDetails;
			}

			public List<PurchaseReturnDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<PurchaseReturnDetail> oPurchaseReturnDetails = new List<PurchaseReturnDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PurchaseReturnDetailDA.Gets(tc, sSQL);
					oPurchaseReturnDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get PurchaseReturnDetail", e);
					#endregion
				}
				return oPurchaseReturnDetails;
			}
		#endregion
	}

}

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
	public class KnittingYarnReturnDetailService : MarshalByRefObject, IKnittingYarnReturnDetailService
	{
		#region Private functions and declaration

		private KnittingYarnReturnDetail MapObject(NullHandler oReader)
		{
			KnittingYarnReturnDetail oKnittingYarnReturnDetail = new KnittingYarnReturnDetail();
			oKnittingYarnReturnDetail.KnittingYarnReturnDetailID = oReader.GetInt32("KnittingYarnReturnDetailID");
			oKnittingYarnReturnDetail.KnittingYarnReturnID = oReader.GetInt32("KnittingYarnReturnID");
			oKnittingYarnReturnDetail.KnittingYarnChallanDetailID = oReader.GetInt32("KnittingYarnChallanDetailID");
			oKnittingYarnReturnDetail.YarnID = oReader.GetInt32("YarnID");
			oKnittingYarnReturnDetail.ReceiveStoreID = oReader.GetInt32("ReceiveStoreID");
			oKnittingYarnReturnDetail.LotID = oReader.GetInt32("LotID");
			oKnittingYarnReturnDetail.NewLotNo = oReader.GetString("NewLotNo");
			oKnittingYarnReturnDetail.MUnitID = oReader.GetInt32("MUnitID");
			oKnittingYarnReturnDetail.Qty = oReader.GetDouble("Qty");
			oKnittingYarnReturnDetail.Remarks = oReader.GetString("Remarks");
            oKnittingYarnReturnDetail.MUnitName = oReader.GetString("MUnitName");
            oKnittingYarnReturnDetail.OperationUnitName = oReader.GetString("OperationUnitName");
            oKnittingYarnReturnDetail.YarnName = oReader.GetString("YarnName");
            oKnittingYarnReturnDetail.YarnCode = oReader.GetString("YarnCode");
            oKnittingYarnReturnDetail.LotNo = oReader.GetString("LotNo");
            oKnittingYarnReturnDetail.LotBalance = oReader.GetDouble("LotBalance");
            oKnittingYarnReturnDetail.ChallanNo = oReader.GetString("ChallanNo");
            oKnittingYarnReturnDetail.ChallanQty = oReader.GetDouble("ChallanQty");
            oKnittingYarnReturnDetail.ReturnBalance = oReader.GetDouble("ReturnBalance"); 
            oKnittingYarnReturnDetail.ChallanBalance = oReader.GetDouble("ChallanBalance");
            oKnittingYarnReturnDetail.KnittingOrderDetailID = oReader.GetInt32("KnittingOrderDetailID");
            oKnittingYarnReturnDetail.StyleNo = oReader.GetString("StyleNo");
            oKnittingYarnReturnDetail.BuyerName = oReader.GetString("BuyerName");
            oKnittingYarnReturnDetail.PAM = oReader.GetInt32("PAM");
            oKnittingYarnReturnDetail.ColorName = oReader.GetString("ColorName");
            oKnittingYarnReturnDetail.BrandShortName = oReader.GetString("BrandShortName");
            oKnittingYarnReturnDetail.DetailQty = oReader.GetDouble("DetailQty");
            oKnittingYarnReturnDetail.DetailMUShortName = oReader.GetString("DetailMUShortName");

			return oKnittingYarnReturnDetail;
		}

		private KnittingYarnReturnDetail CreateObject(NullHandler oReader)
		{
			KnittingYarnReturnDetail oKnittingYarnReturnDetail = new KnittingYarnReturnDetail();
			oKnittingYarnReturnDetail = MapObject(oReader);
			return oKnittingYarnReturnDetail;
		}

		private List<KnittingYarnReturnDetail> CreateObjects(IDataReader oReader)
		{
			List<KnittingYarnReturnDetail> oKnittingYarnReturnDetail = new List<KnittingYarnReturnDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KnittingYarnReturnDetail oItem = CreateObject(oHandler);
				oKnittingYarnReturnDetail.Add(oItem);
			}
			return oKnittingYarnReturnDetail;
		}

		#endregion

		#region Interface implementation
			public KnittingYarnReturnDetail Save(KnittingYarnReturnDetail oKnittingYarnReturnDetail, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oKnittingYarnReturnDetail.KnittingYarnReturnDetailID <= 0)
					{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingYarnReturnDetail", EnumRoleOperationType.Add);
						reader = KnittingYarnReturnDetailDA.InsertUpdate(tc, oKnittingYarnReturnDetail, EnumDBOperation.Insert, nUserID,"");
					}
					else{
						///CheckUserPermission(tc, nUserID, "KnittingYarnReturnDetail", EnumRoleOperationType.Edit);
						reader = KnittingYarnReturnDetailDA.InsertUpdate(tc, oKnittingYarnReturnDetail, EnumDBOperation.Update, nUserID,"");
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oKnittingYarnReturnDetail = new KnittingYarnReturnDetail();
						oKnittingYarnReturnDetail = CreateObject(oReader);
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
						oKnittingYarnReturnDetail = new KnittingYarnReturnDetail();
						oKnittingYarnReturnDetail.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oKnittingYarnReturnDetail;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					KnittingYarnReturnDetail oKnittingYarnReturnDetail = new KnittingYarnReturnDetail();
					oKnittingYarnReturnDetail.KnittingYarnReturnDetailID = id;
					//AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "KnittingYarnReturnDetail", EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "KnittingYarnReturnDetail", id);
					KnittingYarnReturnDetailDA.Delete(tc, oKnittingYarnReturnDetail, EnumDBOperation.Delete, nUserId,"");
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

			public KnittingYarnReturnDetail Get(int id, Int64 nUserId)
			{
				KnittingYarnReturnDetail oKnittingYarnReturnDetail = new KnittingYarnReturnDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = KnittingYarnReturnDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKnittingYarnReturnDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KnittingYarnReturnDetail", e);
					#endregion
				}
				return oKnittingYarnReturnDetail;
			}

			public List<KnittingYarnReturnDetail> Gets(Int64 nUserID)
			{
				List<KnittingYarnReturnDetail> oKnittingYarnReturnDetails = new List<KnittingYarnReturnDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnittingYarnReturnDetailDA.Gets(tc);
					oKnittingYarnReturnDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					KnittingYarnReturnDetail oKnittingYarnReturnDetail = new KnittingYarnReturnDetail();
					oKnittingYarnReturnDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oKnittingYarnReturnDetails;
			}

			public List<KnittingYarnReturnDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<KnittingYarnReturnDetail> oKnittingYarnReturnDetails = new List<KnittingYarnReturnDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnittingYarnReturnDetailDA.Gets(tc, sSQL);
					oKnittingYarnReturnDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get KnittingYarnReturnDetail", e);
					#endregion
				}
				return oKnittingYarnReturnDetails;
			}

		#endregion
	}

}

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
	public class KnittingYarnReturnService : MarshalByRefObject, IKnittingYarnReturnService
	{
		#region Private functions and declaration

		private KnittingYarnReturn MapObject(NullHandler oReader)
		{
			KnittingYarnReturn oKnittingYarnReturn = new KnittingYarnReturn();
			oKnittingYarnReturn.KnittingYarnReturnID = oReader.GetInt32("KnittingYarnReturnID");
			oKnittingYarnReturn.KnittingOrderID = oReader.GetInt32("KnittingOrderID");
			oKnittingYarnReturn.ReturnNo = oReader.GetString("ReturnNo");
			oKnittingYarnReturn.ReturnDate = oReader.GetDateTime("ReturnDate");
			oKnittingYarnReturn.PartyChallanNo = oReader.GetString("PartyChallanNo");
			oKnittingYarnReturn.Remarks = oReader.GetString("Remarks");
			oKnittingYarnReturn.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oKnittingYarnReturn.ApprovedByName = oReader.GetString("ApprovedByName");
            oKnittingYarnReturn.BUID = oReader.GetInt32("BUID");
            oKnittingYarnReturn.KnittingOrderNo = oReader.GetString("KnittingOrderNo");
            oKnittingYarnReturn.KnittingOrderDate = oReader.GetDateTime("KnittingOrderDate");
            oKnittingYarnReturn.BuyerName = oReader.GetString("BuyerName");
            oKnittingYarnReturn.StyleNo = oReader.GetString("StyleNo");
            oKnittingYarnReturn.FactoryName = oReader.GetString("FactoryName");
            oKnittingYarnReturn.KnittingOrderQty = oReader.GetDouble("KnittingOrderQty");
			return oKnittingYarnReturn;
		}

		private KnittingYarnReturn CreateObject(NullHandler oReader)
		{
			KnittingYarnReturn oKnittingYarnReturn = new KnittingYarnReturn();
			oKnittingYarnReturn = MapObject(oReader);
			return oKnittingYarnReturn;
		}

		private List<KnittingYarnReturn> CreateObjects(IDataReader oReader)
		{
			List<KnittingYarnReturn> oKnittingYarnReturn = new List<KnittingYarnReturn>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KnittingYarnReturn oItem = CreateObject(oHandler);
				oKnittingYarnReturn.Add(oItem);
			}
			return oKnittingYarnReturn;
		}

		#endregion

		#region Interface implementation
			public KnittingYarnReturn Save(KnittingYarnReturn oKnittingYarnReturn, Int64 nUserID)
			{
                KnittingYarnReturnDetail oKnittingYarnReturnDetail = new KnittingYarnReturnDetail();

                KnittingYarnReturn oUG = new KnittingYarnReturn();

                oUG = oKnittingYarnReturn;

                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);

                    #region KnittingYarnReturn
                    IDataReader reader;
                    if (oKnittingYarnReturn.KnittingYarnReturnID <= 0)
                    {

                        reader = KnittingYarnReturnDA.InsertUpdate(tc, oKnittingYarnReturn, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {

                        reader = KnittingYarnReturnDA.InsertUpdate(tc, oKnittingYarnReturn, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oKnittingYarnReturn = new KnittingYarnReturn();
                        oKnittingYarnReturn = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion

                    #region KnittingFabricReceiveDetail

                    if (oKnittingYarnReturn.KnittingYarnReturnID > 0)
                    {
                        string sKnittingFabricReceiveIDs = "";
                        if (oUG.KnittingYarnReturnDetails.Count > 0)
                        {
                            IDataReader readerdetail;
                            foreach (KnittingYarnReturnDetail oDRD in oUG.KnittingYarnReturnDetails)
                            {
                                oDRD.KnittingYarnReturnID = oKnittingYarnReturn.KnittingYarnReturnID;
                                if (oDRD.KnittingYarnReturnDetailID <= 0)
                                {
                                    readerdetail = KnittingYarnReturnDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                                }
                                else
                                {
                                    readerdetail = KnittingYarnReturnDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                                }
                                NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                                int nKnittingYarnReturnDetailID = 0;
                                if (readerdetail.Read())
                                {
                                    nKnittingYarnReturnDetailID = oReaderDevRecapdetail.GetInt32("KnittingYarnReturnDetailID");
                                    sKnittingFabricReceiveIDs = sKnittingFabricReceiveIDs + oReaderDevRecapdetail.GetString("KnittingYarnReturnDetailID") + ",";
                                }
                                readerdetail.Close();
                            }
                        }
                        if (sKnittingFabricReceiveIDs.Length > 0)
                        {
                            sKnittingFabricReceiveIDs = sKnittingFabricReceiveIDs.Remove(sKnittingFabricReceiveIDs.Length - 1, 1);
                        }
                        oKnittingYarnReturnDetail = new KnittingYarnReturnDetail();
                        oKnittingYarnReturnDetail.KnittingYarnReturnID = oKnittingYarnReturn.KnittingYarnReturnID;
                        KnittingYarnReturnDetailDA.Delete(tc, oKnittingYarnReturnDetail, EnumDBOperation.Delete, nUserID, sKnittingFabricReceiveIDs);
                    }

                    #endregion

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
             
				return oKnittingYarnReturn;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					KnittingYarnReturn oKnittingYarnReturn = new KnittingYarnReturn();
					oKnittingYarnReturn.KnittingYarnReturnID = id;
					//AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "KnittingYarnReturn", EnumRoleOperationType.Delete);
					//DBTableReferenceDA.HasReference(tc, "KnittingYarnReturn", id);
					KnittingYarnReturnDA.Delete(tc, oKnittingYarnReturn, EnumDBOperation.Delete, nUserId);
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

			public KnittingYarnReturn Get(int id, Int64 nUserId)
			{
				KnittingYarnReturn oKnittingYarnReturn = new KnittingYarnReturn();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = KnittingYarnReturnDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKnittingYarnReturn = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KnittingYarnReturn", e);
					#endregion
				}
				return oKnittingYarnReturn;
			}

			public List<KnittingYarnReturn> Gets(Int64 nUserID)
			{
				List<KnittingYarnReturn> oKnittingYarnReturns = new List<KnittingYarnReturn>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnittingYarnReturnDA.Gets(tc);
					oKnittingYarnReturns = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					KnittingYarnReturn oKnittingYarnReturn = new KnittingYarnReturn();
					oKnittingYarnReturn.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oKnittingYarnReturns;
			}

			public List<KnittingYarnReturn> Gets (string sSQL, Int64 nUserID)
			{
				List<KnittingYarnReturn> oKnittingYarnReturns = new List<KnittingYarnReturn>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnittingYarnReturnDA.Gets(tc, sSQL);
					oKnittingYarnReturns = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get KnittingYarnReturn", e);
					#endregion
				}
				return oKnittingYarnReturns;
			}

            public KnittingYarnReturn Approve(KnittingYarnReturn oKnittingYarnReturn, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    reader = KnittingYarnReturnDA.Approve(tc, oKnittingYarnReturn, EnumDBOperation.Approval, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oKnittingYarnReturn = new KnittingYarnReturn();
                        oKnittingYarnReturn = CreateObject(oReader);
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
                        oKnittingYarnReturn = new KnittingYarnReturn();
                        oKnittingYarnReturn.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oKnittingYarnReturn;

            }

		#endregion
	}

}

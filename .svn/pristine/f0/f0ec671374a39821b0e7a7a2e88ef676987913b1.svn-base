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
	public class OrderStepGroupService : MarshalByRefObject, IOrderStepGroupService
	{
		#region Private functions and declaration

		private OrderStepGroup MapObject(NullHandler oReader)
		{
			OrderStepGroup oOrderStepGroup = new OrderStepGroup();
			oOrderStepGroup.OrderStepGroupID = oReader.GetInt32("OrderStepGroupID");
			oOrderStepGroup.GroupName = oReader.GetString("GroupName");
			oOrderStepGroup.Note = oReader.GetString("Note");
            oOrderStepGroup.BUID = oReader.GetInt32("BUID");
            
			return oOrderStepGroup;
		}

		private OrderStepGroup CreateObject(NullHandler oReader)
		{
			OrderStepGroup oOrderStepGroup = new OrderStepGroup();
			oOrderStepGroup = MapObject(oReader);
			return oOrderStepGroup;
		}

		private List<OrderStepGroup> CreateObjects(IDataReader oReader)
		{
			List<OrderStepGroup> oOrderStepGroup = new List<OrderStepGroup>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				OrderStepGroup oItem = CreateObject(oHandler);
				oOrderStepGroup.Add(oItem);
			}
			return oOrderStepGroup;
		}

		#endregion

		#region Interface implementation
			public OrderStepGroup Save(OrderStepGroup oOrderStepGroup, Int64 nUserID)
			{
				TransactionContext tc = null;
                List<OrderStepGroupDetail> oOrderStepGroupDetails = new List<OrderStepGroupDetail>();
                oOrderStepGroupDetails = oOrderStepGroup.OrderStepGroupDetails;
                string sOrderStepGroupDetailIDs = "";
                bool bIsInitialSave = oOrderStepGroup.bIsInitialSave;
				try
				{
					tc = TransactionContext.Begin(true);
                    #region Group
                    IDataReader reader;
					if (oOrderStepGroup.OrderStepGroupID <= 0)
					{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OrderStepGroup, EnumRoleOperationType.Add);
						reader = OrderStepGroupDA.InsertUpdate(tc, oOrderStepGroup, EnumDBOperation.Insert, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OrderStepGroup, EnumRoleOperationType.Edit);
						reader = OrderStepGroupDA.InsertUpdate(tc, oOrderStepGroup, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oOrderStepGroup = new OrderStepGroup();
						oOrderStepGroup = CreateObject(oReader);
					}
					reader.Close();
                    #endregion
                    #region OrderStepGroup Detail Part
                    if (oOrderStepGroupDetails != null)
                    {
                        foreach (OrderStepGroupDetail oItem in oOrderStepGroupDetails)
                        {
                            IDataReader readerdetail;
                            oItem.OrderStepGroupID = oOrderStepGroup.OrderStepGroupID;
                            if (oItem.OrderStepGroupDetailID <= 0)
                            {
                                readerdetail = OrderStepGroupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = OrderStepGroupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sOrderStepGroupDetailIDs = sOrderStepGroupDetailIDs + oReaderDetail.GetString("OrderStepGroupDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sOrderStepGroupDetailIDs.Length > 0)
                        {
                            sOrderStepGroupDetailIDs = sOrderStepGroupDetailIDs.Remove(sOrderStepGroupDetailIDs.Length - 1, 1);
                        }
                    }
                    OrderStepGroupDetail oOrderStepGroupDetail = new OrderStepGroupDetail();
                    oOrderStepGroupDetail.OrderStepGroupID = oOrderStepGroup.OrderStepGroupID;
                    OrderStepGroupDetailDA.Delete(tc, oOrderStepGroupDetail, EnumDBOperation.Delete, nUserID, sOrderStepGroupDetailIDs);
                    #endregion

                    #region REfresh Sequence
                    OrderStepGroupDetail oNewOrderStepGroupDetail = new OrderStepGroupDetail();
                    oNewOrderStepGroupDetail.OrderStepGroupID = oOrderStepGroup.OrderStepGroupID;
                    OrderStepGroupDetailDA.UpDown(tc, oNewOrderStepGroupDetail, true);
                    #endregion

                    #region Get OrderStepGroup
                    reader = OrderStepGroupDA.Get(tc, oOrderStepGroup.OrderStepGroupID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oOrderStepGroup = new OrderStepGroup();
                        oOrderStepGroup = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion
                    tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					{
						tc.HandleError();
						oOrderStepGroup = new OrderStepGroup();
						oOrderStepGroup.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oOrderStepGroup;
			}

            public OrderStepGroup UpDown(OrderStepGroupDetail oOrderStepGroupDetail, Int64 nUserID)
            {
                OrderStepGroup oOrderStepGroup = new OrderStepGroup();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    OrderStepGroupDetailDA.UpDown(tc, oOrderStepGroupDetail, false);
                    #region Template Get
                    reader = OrderStepGroupDA.Get(tc, oOrderStepGroupDetail.OrderStepGroupID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oOrderStepGroup = CreateObject(oReader);
                    }
                    reader.Close();
                 
                    #endregion

                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();

                    oOrderStepGroup = new OrderStepGroup();
                    oOrderStepGroup.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oOrderStepGroup;
            }

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					OrderStepGroup oOrderStepGroup = new OrderStepGroup();
					oOrderStepGroup.OrderStepGroupID = id;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.OrderStepGroup, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "OrderStepGroup", id);
					OrderStepGroupDA.Delete(tc, oOrderStepGroup, EnumDBOperation.Delete, nUserId);
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

			public OrderStepGroup Get(int id, Int64 nUserId)
			{
				OrderStepGroup oOrderStepGroup = new OrderStepGroup();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = OrderStepGroupDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oOrderStepGroup = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get OrderStepGroup", e);
					#endregion
				}
				return oOrderStepGroup;
			}

			public List<OrderStepGroup> Gets(Int64 nUserID)
			{
				List<OrderStepGroup> oOrderStepGroups = new List<OrderStepGroup>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = OrderStepGroupDA.Gets(tc);
					oOrderStepGroups = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					OrderStepGroup oOrderStepGroup = new OrderStepGroup();
					oOrderStepGroup.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oOrderStepGroups;
			}
        public List<OrderStepGroup> Gets(int BUID, Int64 nUserID)
			{
				List<OrderStepGroup> oOrderStepGroups = new List<OrderStepGroup>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = OrderStepGroupDA.Gets(tc, BUID);
					oOrderStepGroups = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					OrderStepGroup oOrderStepGroup = new OrderStepGroup();
					oOrderStepGroup.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oOrderStepGroups;
			}

        
			public List<OrderStepGroup> Gets (string sSQL, Int64 nUserID)
			{
				List<OrderStepGroup> oOrderStepGroups = new List<OrderStepGroup>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = OrderStepGroupDA.Gets(tc, sSQL);
					oOrderStepGroups = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get OrderStepGroup", e);
					#endregion
				}
				return oOrderStepGroups;
			}

		#endregion
	}

}

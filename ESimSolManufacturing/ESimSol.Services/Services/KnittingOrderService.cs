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
    public class KnittingOrderService : MarshalByRefObject, IKnittingOrderService
    {
        #region Private functions and declaration

        private KnittingOrder MapObject(NullHandler oReader)
        {
            KnittingOrder oKnittingOrder = new KnittingOrder();
            oKnittingOrder.KnittingOrderID = oReader.GetInt32("KnittingOrderID");
            oKnittingOrder.BUID = oReader.GetInt32("BUID");
            oKnittingOrder.BusinessSessionID = oReader.GetInt32("BusinessSessionID");
            oKnittingOrder.OrderNo = oReader.GetString("OrderNo");
            oKnittingOrder.OrderDate = oReader.GetDateTime("OrderDate");  
            oKnittingOrder.YarnDeliveryStatus = (EnumFinishYarnChallan)oReader.GetInt32("YarnDeliveryStatus");
            oKnittingOrder.FabricReceivedStatus = (EnumFinishFabricReceive)oReader.GetInt32("FabricReceivedStatus");
            oKnittingOrder.OrderType = (EnumKnittingOrderType)oReader.GetInt32("OrderType");
            oKnittingOrder.FactoryID = oReader.GetInt32("FactoryID");
            oKnittingOrder.StartDate = oReader.GetDateTime("StartDate");
            oKnittingOrder.ApproxCompleteDate = oReader.GetDateTime("ApproxCompleteDate");
            oKnittingOrder.ActualCompleteDate = oReader.GetDateTime("ActualCompleteDate");
            oKnittingOrder.CurrencyID = oReader.GetInt32("CurrencyID");
            oKnittingOrder.Amount = oReader.GetDouble("Amount");
            oKnittingOrder.IssueQty = oReader.GetDouble("IssueQty");
            oKnittingOrder.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oKnittingOrder.Remarks = oReader.GetString("Remarks");
            oKnittingOrder.KnittingInstruction = oReader.GetString("KnittingInstruction");

            oKnittingOrder.BusinessSessionName = oReader.GetString("BusinessSessionName");
            oKnittingOrder.FactoryName = oReader.GetString("FactoryName");
            oKnittingOrder.CurrencyName = oReader.GetString("CurrencyName");
            oKnittingOrder.ApprovedByName = oReader.GetString("ApprovedByName");

            return oKnittingOrder;
        }

        private KnittingOrder CreateObject(NullHandler oReader)
        {
            KnittingOrder oKnittingOrder = new KnittingOrder();
            oKnittingOrder = MapObject(oReader);
            return oKnittingOrder;
        }

        private List<KnittingOrder> CreateObjects(IDataReader oReader)
        {
            List<KnittingOrder> oKnittingOrder = new List<KnittingOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnittingOrder oItem = CreateObject(oHandler);
                oKnittingOrder.Add(oItem);
            }
            return oKnittingOrder;
        }

        #endregion

        #region Interface implementation
        public KnittingOrder Save(KnittingOrder oKnittingOrder, Int64 nUserID)
        {
            KnittingOrderDetail oKnittingOrderDetail = new KnittingOrderDetail();
            KnittingOrderTermsAndCondition oKnittingOrderTermsAndCondition = new KnittingOrderTermsAndCondition();
            KnittingOrder oUG = new KnittingOrder();
            KnittingOrder oKO = new KnittingOrder();
            KnittingOrder oKOForTC = new KnittingOrder();
            oUG = oKnittingOrder;
            oKO = oKnittingOrder;
            oKOForTC = oKnittingOrder;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region KnittingOrder
                IDataReader reader;
                if (oKnittingOrder.KnittingOrderID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KnittingOrder, EnumRoleOperationType.Add);
                    reader = KnittingOrderDA.InsertUpdate(tc, oKnittingOrder, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KnittingOrder, EnumRoleOperationType.Edit);
                    reader = KnittingOrderDA.InsertUpdate(tc, oKnittingOrder, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingOrder = new KnittingOrder();
                    oKnittingOrder = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region KnittingOrderDetail

                if (oKnittingOrder.KnittingOrderID > 0)
                {
                    string sKnittingOrderDetailIDs = "";
                    if (oUG.KnittingOrderDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (KnittingOrderDetail oDRD in oUG.KnittingOrderDetails)
                        {
                            oDRD.KnittingOrderID = oKnittingOrder.KnittingOrderID;
                            if (oDRD.KnittingOrderDetailID <= 0)
                            {
                                readerdetail = KnittingOrderDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = KnittingOrderDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nKnittingOrderDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nKnittingOrderDetailID = oReaderDevRecapdetail.GetInt32("KnittingOrderDetailID");
                                sKnittingOrderDetailIDs = sKnittingOrderDetailIDs + oReaderDevRecapdetail.GetString("KnittingOrderDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sKnittingOrderDetailIDs.Length > 0)
                    {
                        sKnittingOrderDetailIDs = sKnittingOrderDetailIDs.Remove(sKnittingOrderDetailIDs.Length - 1, 1);
                    }
                    oKnittingOrderDetail = new KnittingOrderDetail();
                    oKnittingOrderDetail.KnittingOrderID = oKnittingOrder.KnittingOrderID;
                    KnittingOrderDetailDA.Delete(tc, oKnittingOrderDetail, EnumDBOperation.Delete, nUserID, sKnittingOrderDetailIDs);
                }

                #endregion

                #region Knitting Order Terms And Condition

                if (oKnittingOrder.KnittingOrderID > 0)
                {
                    string sKnittingOrderTermsAndConditionIDs = "";
                    if (oKOForTC.KnittingOrderTermsAndConditions.Count > 0)
                    {
                        IDataReader readerOrderTermsAndCondition;
                        foreach (KnittingOrderTermsAndCondition oDRD in oKOForTC.KnittingOrderTermsAndConditions)
                        {
                            oDRD.KnittingOrderID = oKnittingOrder.KnittingOrderID;
                            if (oDRD.KnittingOrderTermsAndConditionID <= 0)
                            {
                                readerOrderTermsAndCondition = KnittingOrderTermsAndConditionDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerOrderTermsAndCondition = KnittingOrderTermsAndConditionDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderOrderTermsAndCondition = new NullHandler(readerOrderTermsAndCondition);
                            int nKnittingOrderTermsAndConditionID = 0;
                            if (readerOrderTermsAndCondition.Read())
                            {
                                nKnittingOrderTermsAndConditionID = oReaderOrderTermsAndCondition.GetInt32("KnittingOrderTermsAndConditionID");
                                sKnittingOrderTermsAndConditionIDs = sKnittingOrderTermsAndConditionIDs + oReaderOrderTermsAndCondition.GetString("KnittingOrderTermsAndConditionID") + ",";
                            }
                            readerOrderTermsAndCondition.Close();
                        }
                    }
                    if (sKnittingOrderTermsAndConditionIDs.Length > 0)
                    {
                        sKnittingOrderTermsAndConditionIDs = sKnittingOrderTermsAndConditionIDs.Remove(sKnittingOrderTermsAndConditionIDs.Length - 1, 1);
                    }
                    oKnittingOrderTermsAndCondition = new KnittingOrderTermsAndCondition();
                    oKnittingOrderTermsAndCondition.KnittingOrderID = oKnittingOrder.KnittingOrderID;
                    KnittingOrderTermsAndConditionDA.Delete(tc, oKnittingOrderTermsAndCondition, EnumDBOperation.Delete, nUserID, sKnittingOrderTermsAndConditionIDs);
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
                    oKnittingOrder = new KnittingOrder();
                    oKnittingOrder.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnittingOrder;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnittingOrder oKnittingOrder = new KnittingOrder();
                oKnittingOrder.KnittingOrderID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.KnittingOrder, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "KnittingOrder", id);
                KnittingOrderDA.Delete(tc, oKnittingOrder, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public KnittingOrder Get(int id, Int64 nUserId)
        {
            KnittingOrder oKnittingOrder = new KnittingOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnittingOrderDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingOrder = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnittingOrder", e);
                #endregion
            }
            return oKnittingOrder;
        }

        public List<KnittingOrder> Gets(Int64 nUserID)
        {
            List<KnittingOrder> oKnittingOrders = new List<KnittingOrder>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingOrderDA.Gets(tc);
                oKnittingOrders = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingOrder oKnittingOrder = new KnittingOrder();
                oKnittingOrder.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingOrders;
        }

        public List<KnittingOrder> Gets(string sSQL, Int64 nUserID)
        {
            List<KnittingOrder> oKnittingOrders = new List<KnittingOrder>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingOrderDA.Gets(tc, sSQL);
                oKnittingOrders = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) 
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnittingOrder", e);
                #endregion
            }
            return oKnittingOrders;
        }

        public KnittingOrder Approve(KnittingOrder oKnittingOrder, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.KnittingOrder, EnumRoleOperationType.Approved);
                //reader = KnittingOrderDA.ApproveOrder(tc, oKnittingOrder, EnumDBOperation.Insert, nUserId);
                reader = KnittingOrderDA.InsertUpdate(tc, oKnittingOrder, EnumDBOperation.Approval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingOrder = new KnittingOrder();
                    oKnittingOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oKnittingOrder = new KnittingOrder();
                oKnittingOrder.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save KnittingOrder. Because of " + e.Message, e);
                #endregion
            }
            return oKnittingOrder;
        }

        public KnittingOrder UnApprove(KnittingOrder oKnittingOrder, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.KnittingOrder, EnumRoleOperationType.Approved);                
                reader = KnittingOrderDA.InsertUpdate(tc, oKnittingOrder, EnumDBOperation.UnApproval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingOrder = new KnittingOrder();
                    oKnittingOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oKnittingOrder = new KnittingOrder();
                oKnittingOrder.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save KnittingOrder. Because of " + e.Message, e);
                #endregion
            }
            return oKnittingOrder;
        }

        public KnittingOrder FinishYarnChallan(KnittingOrder oKnittingOrder, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.KnittingOrder, EnumRoleOperationType.Disburse);
                reader = KnittingOrderDA.InsertUpdate(tc, oKnittingOrder, EnumDBOperation.Disburse, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingOrder = new KnittingOrder();
                    oKnittingOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oKnittingOrder = new KnittingOrder();
                oKnittingOrder.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save KnittingOrder. Because of " + e.Message, e);
                #endregion
            }
            return oKnittingOrder;
        }

        public KnittingOrder FinishFabricReceive(KnittingOrder oKnittingOrder, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.KnittingOrder, EnumRoleOperationType.Received);
                reader = KnittingOrderDA.InsertUpdate(tc, oKnittingOrder, EnumDBOperation.Receive, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingOrder = new KnittingOrder();
                    oKnittingOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oKnittingOrder = new KnittingOrder();
                oKnittingOrder.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save KnittingOrder. Because of " + e.Message, e);
                #endregion
            }
            return oKnittingOrder;
        }

        #endregion
    }

}

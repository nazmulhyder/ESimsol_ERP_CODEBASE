using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 
namespace ESimSol.Services.Services
{
    public class WYRequisitionService : MarshalByRefObject, IWYRequisitionService
    {
        #region Private functions and declaration
        private WYRequisition MapObject(NullHandler oReader)
        {
            WYRequisition oWYRequisition = new WYRequisition();
            oWYRequisition.WYRequisitionID = oReader.GetInt32("WYRequisitionID");
            oWYRequisition.RequisitionNo = oReader.GetString("RequisitionNo");
            oWYRequisition.IssueDate = oReader.GetDateTime("IssueDate");
            oWYRequisition.IssueStoreID = oReader.GetInt32("IssueStoreID");
            oWYRequisition.BUID = oReader.GetInt32("BUID");
            oWYRequisition.ReceiveStoreID = oReader.GetInt32("ReceiveStoreID");
            oWYRequisition.ReceiveStoreName = oReader.GetString("ReceiveStoreName");
            oWYRequisition.IssueStoreName = oReader.GetString("IssueStoreName");
            oWYRequisition.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oWYRequisition.ApprovedByName = oReader.GetString("ApprovedByName");
            oWYRequisition.DisburseBy = oReader.GetInt32("DisburseBy");
            oWYRequisition.DisburseByName = oReader.GetString("DisburseByName");
            oWYRequisition.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oWYRequisition.ReceivedByName = oReader.GetString("ReceivedByName");
            oWYRequisition.ColorName = oReader.GetString("ColorName");
            oWYRequisition.Remarks = oReader.GetString("Remarks");
            oWYRequisition.RequisitionByName = oReader.GetString("RequisitionByName");
            oWYRequisition.WYarnTypeInt = oReader.GetInt32("WYarnType");
            oWYRequisition.WYarnType = (EnumWYarnType)oReader.GetInt32("WYarnType");
            oWYRequisition.RequisitionTypeInt = oReader.GetInt32("RequisitionType");
            oWYRequisition.RequisitionType = (EnumInOutType)oReader.GetInt32("RequisitionType");
            oWYRequisition.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oWYRequisition.WarpWeftType = (EnumWarpWeft)oReader.GetInt32("WarpWeftType");
            return oWYRequisition;
        }

        private WYRequisition CreateObject(NullHandler oReader)
        {
            WYRequisition oWYRequisition = new WYRequisition();
            oWYRequisition = MapObject(oReader);
            return oWYRequisition;
        }

        private List<WYRequisition> CreateObjects(IDataReader oReader)
        {
            List<WYRequisition> oWYRequisition = new List<WYRequisition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WYRequisition oItem = CreateObject(oHandler);
                oWYRequisition.Add(oItem);
            }
            return oWYRequisition;
        }

        #endregion

        #region Interface implementation
        public WYRequisitionService() { }

        public WYRequisition Save(WYRequisition oWYRequisition, Int64 nUserId)
        {
            TransactionContext tc = null;
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            string sFabricExecutionOrderYarnReceiveIDs = "";
            oFabricExecutionOrderYarnReceives = oWYRequisition.FEOYSList;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                
                if (oWYRequisition.WYRequisitionID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.WYRequisition, EnumRoleOperationType.Add);
                    reader = WYRequisitionDA.InsertUpdate(tc, oWYRequisition, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.WYRequisition, EnumRoleOperationType.Edit);
                    reader = WYRequisitionDA.InsertUpdate(tc, oWYRequisition, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWYRequisition = new WYRequisition();
                    oWYRequisition = CreateObject(oReader);
                }
                reader.Close();

                #region  Fabric Execution Order Yarn Receive Part
                if (oFabricExecutionOrderYarnReceives != null)
                {
                    foreach (FabricExecutionOrderYarnReceive oItem in oFabricExecutionOrderYarnReceives)
                    {
                        IDataReader readerdetail;
                        oItem.WYRequisitionID = oWYRequisition.WYRequisitionID;
                        if (oItem.FEOYID <= 0)
                        {
                            readerdetail = FabricExecutionOrderYarnReceiveDA.IUD(tc, oItem, EnumDBOperation.Insert, nUserId, "");
                        }
                        else
                        {
                            readerdetail = FabricExecutionOrderYarnReceiveDA.IUD(tc, oItem, EnumDBOperation.Update, nUserId, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sFabricExecutionOrderYarnReceiveIDs = sFabricExecutionOrderYarnReceiveIDs + oReaderDetail.GetString("FEOYID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sFabricExecutionOrderYarnReceiveIDs.Length > 0)
                    {
                        sFabricExecutionOrderYarnReceiveIDs = sFabricExecutionOrderYarnReceiveIDs.Remove(sFabricExecutionOrderYarnReceiveIDs.Length - 1, 1);
                    }
                    oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
                    oFabricExecutionOrderYarnReceive.WYRequisitionID = oWYRequisition.WYRequisitionID;
                    FabricExecutionOrderYarnReceiveDA.Delete(tc, oFabricExecutionOrderYarnReceive, EnumDBOperation.Delete, nUserId, sFabricExecutionOrderYarnReceiveIDs);
                }

                #endregion



                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message.Split('~')[0], e);
                #endregion
            }
            return oWYRequisition;
        }
        public WYRequisition Approve(WYRequisition oWYRequisition, Int64 nUserId)
        {
            TransactionContext tc = null;           
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = WYRequisitionDA.InsertUpdate(tc, oWYRequisition, EnumDBOperation.Approval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWYRequisition = new WYRequisition();
                    oWYRequisition = CreateObject(oReader);
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
                throw new ServiceException(e.Message.Split('~')[0], e);
                #endregion
            }
            return oWYRequisition;
        }
        public WYRequisition UndoApprove(WYRequisition oWYRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = WYRequisitionDA.InsertUpdate(tc, oWYRequisition, eEnumDBOperation, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWYRequisition = new WYRequisition();
                    oWYRequisition = CreateObject(oReader);
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
                throw new ServiceException(e.Message.Split('~')[0], e);
                #endregion
            }
            return oWYRequisition;
        }
        public WYRequisition Disburse(WYRequisition oWYRequisition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.WYRequisition, EnumRoleOperationType.Disburse);
                IDataReader reader = WYRequisitionDA.InsertUpdate(tc, oWYRequisition, EnumDBOperation.Disburse, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWYRequisition = new WYRequisition();
                    oWYRequisition = CreateObject(oReader);
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
                throw new ServiceException(e.Message.Split('~')[0], e);
                #endregion
            }
            return oWYRequisition;
        }
        public WYRequisition Receive(WYRequisition oWYRequisition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.WYRequisition, EnumRoleOperationType.Received);
                IDataReader reader = WYRequisitionDA.InsertUpdate(tc, oWYRequisition, EnumDBOperation.Receive, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWYRequisition = new WYRequisition();
                    oWYRequisition = CreateObject(oReader);
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
                throw new ServiceException(e.Message.Split('~')[0], e);
                #endregion
            }
            return oWYRequisition;
        }
        public string Delete(WYRequisition oWYRequisition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                
                
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.WYRequisition, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "WYRequisition", oWYRequisition.WYRequisitionID);
                WYRequisitionDA.Delete(tc, oWYRequisition, EnumDBOperation.Delete,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public WYRequisition Get(int id, Int64 nUserId)
        {
            WYRequisition oWYRequisition = new WYRequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = WYRequisitionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWYRequisition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get WYRequisition", e);
                #endregion
            }

            return oWYRequisition;
        }

        public List<WYRequisition> Gets(Int64 nUserId)
        {
            List<WYRequisition> oWYRequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WYRequisitionDA.Gets(tc);
                oWYRequisition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WYRequisition", e);
                #endregion
            }

            return oWYRequisition;
        }
        public List<WYRequisition> BUWiseGets(int buid, Int64 nUserId)
        {
            List<WYRequisition> oWYRequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WYRequisitionDA.BUWiseGets(buid, tc);
                oWYRequisition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                 throw new ServiceException("Failed to Get WYRequisition", e);
                #endregion
            }

            return oWYRequisition;
        }
        public List<WYRequisition> Gets(string sSQL, Int64 nUserId)
        {
            List<WYRequisition> oWYRequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WYRequisitionDA.Gets(tc, sSQL);
                oWYRequisition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WYRequisition", e);
                #endregion
            }

            return oWYRequisition;
        }
        
        public List<WYRequisition> GetsByName(string sName,  Int64 nUserId)
        {
            List<WYRequisition> oWYRequisitions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WYRequisitionDA.GetsByName(tc, sName );
                oWYRequisitions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WYRequisitions", e);
                #endregion
            }

            return oWYRequisitions;
        }
        #endregion
    } 
}
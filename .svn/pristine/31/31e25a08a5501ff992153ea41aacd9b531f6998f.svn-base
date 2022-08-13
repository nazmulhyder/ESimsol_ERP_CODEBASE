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


    public class ConsumptionRequisitionService : MarshalByRefObject, IConsumptionRequisitionService
    {
        #region Private functions and declaration
        private ConsumptionRequisition MapObject(NullHandler oReader)
        {
            ConsumptionRequisition oConsumptionRequisition = new ConsumptionRequisition();
            oConsumptionRequisition.ConsumptionRequisitionID = oReader.GetInt32("ConsumptionRequisitionID");
            oConsumptionRequisition.RequisitionNo = oReader.GetString("RequisitionNo");
            oConsumptionRequisition.BUID = oReader.GetInt32("BUID");
            oConsumptionRequisition.RefNo = oReader.GetString("RefNo");
            oConsumptionRequisition.CRType = (EnumConsumptionType)oReader.GetInt32("CRType");
            oConsumptionRequisition.CRTypeInt = oReader.GetInt32("CRType");
            oConsumptionRequisition.RequisitionBy = oReader.GetInt32("RequisitionBy");
            oConsumptionRequisition.CRStatus = (EnumCRStatus)oReader.GetInt32("CRStatus");
            oConsumptionRequisition.CRStatusInt = oReader.GetInt32("CRStatus");
            oConsumptionRequisition.IssueDate = oReader.GetDateTime("IssueDate");
            oConsumptionRequisition.RequisitionFor = oReader.GetInt32("RequisitionFor");
            oConsumptionRequisition.StoreID = oReader.GetInt32("StoreID");
            oConsumptionRequisition.Remarks = oReader.GetString("Remarks");
            oConsumptionRequisition.DeliveryBy = oReader.GetInt32("DeliveryBy");
            oConsumptionRequisition.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oConsumptionRequisition.StoreCode = oReader.GetString("StoreCode");
            oConsumptionRequisition.StoreName = oReader.GetString("StoreName");
            oConsumptionRequisition.RequisitionByName = oReader.GetString("RequisitionByName");
            oConsumptionRequisition.ApprovedByName = oReader.GetString("ApprovedByName");
            oConsumptionRequisition.DeliveryByName = oReader.GetString("DeliveryByName");
            oConsumptionRequisition.RequisitionForName = oReader.GetString("RequisitionForName");
            oConsumptionRequisition.Amount = oReader.GetDouble("Amount");
            oConsumptionRequisition.ConsumptionRequisitionLogID = oReader.GetInt32("ConsumptionRequisitionLogID");
            oConsumptionRequisition.Shift = (EnumShift)oReader.GetInt32("Shift");
            oConsumptionRequisition.ShiftInInt = oReader.GetInt32("Shift");
            oConsumptionRequisition.SubLedgerID = oReader.GetInt32("SubLedgerID");
            oConsumptionRequisition.SubLedgerName = oReader.GetString("SubLedgerName");
            oConsumptionRequisition.IsWillVoucherEffect = oReader.GetBoolean("IsWillVoucherEffect");
            oConsumptionRequisition.RefType = (EnumCRRefType)oReader.GetInt32("RefType");
            oConsumptionRequisition.RefTypeInt = oReader.GetInt32("RefType");
            oConsumptionRequisition.RefObjID = oReader.GetInt32("RefObjID");

            oConsumptionRequisition.RefObjNo = oReader.GetString("RefObjNo");

            oConsumptionRequisition.FabricSalesContractID = oReader.GetInt32("FabricSalesContractID");
            oConsumptionRequisition.FabricSalesContractDetailID = oReader.GetInt32("FabricSalesContractDetailID");

            oConsumptionRequisition.SCNoFull = oReader.GetString("SCNoFull");
            oConsumptionRequisition.ExeNoFull = oReader.GetString("ExeNoFull");
            oConsumptionRequisition.BuyerName = oReader.GetString("BuyerName");
            oConsumptionRequisition.OrderName = oReader.GetString("OrderName");
            oConsumptionRequisition.ColorInfo = oReader.GetString("ColorInfo");
            oConsumptionRequisition.Construction = oReader.GetString("Construction");
            oConsumptionRequisition.ProductName = oReader.GetString("ProductName");
            oConsumptionRequisition.FabricNo = oReader.GetString("FabricNo");
            oConsumptionRequisition.Qty = oReader.GetDouble("Qty");
            oConsumptionRequisition.SCDate = oReader.GetDateTime("SCDate");

            return oConsumptionRequisition;
        }

        private ConsumptionRequisition CreateObject(NullHandler oReader)
        {
            ConsumptionRequisition oConsumptionRequisition = new ConsumptionRequisition();
            oConsumptionRequisition = MapObject(oReader);
            return oConsumptionRequisition;
        }

        private List<ConsumptionRequisition> CreateObjects(IDataReader oReader)
        {
            List<ConsumptionRequisition> oConsumptionRequisition = new List<ConsumptionRequisition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ConsumptionRequisition oItem = CreateObject(oHandler);
                oConsumptionRequisition.Add(oItem);
            }
            return oConsumptionRequisition;
        }

        #endregion

        #region Interface implementation

        public ConsumptionRequisition Save(ConsumptionRequisition oConsumptionRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ConsumptionRequisitionDetail> oConsumptionRequisitionDetails = new List<ConsumptionRequisitionDetail>();
            string sConsumptionRequisitionDetailIDs = "";
            oConsumptionRequisitionDetails = oConsumptionRequisition.ConsumptionRequisitionDetails;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oConsumptionRequisition.ConsumptionRequisitionID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ConsumptionRequisition, EnumRoleOperationType.Add);
                    reader = ConsumptionRequisitionDA.InsertUpdate(tc, oConsumptionRequisition, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ConsumptionRequisition, EnumRoleOperationType.Edit);
                    VoucherDA.CheckVoucherReference(tc, "ConsumptionRequisition", "ConsumptionRequisitionID", oConsumptionRequisition.ConsumptionRequisitionID);
                    reader = ConsumptionRequisitionDA.InsertUpdate(tc, oConsumptionRequisition, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oConsumptionRequisition = new ConsumptionRequisition();
                    oConsumptionRequisition = CreateObject(oReader);
                }
                reader.Close();

                #region CR Detail Part
                if (oConsumptionRequisitionDetails != null)
                {
                    foreach (ConsumptionRequisitionDetail oItem in oConsumptionRequisitionDetails)
                    {
                        IDataReader readerdetail;
                        oItem.ConsumptionRequisitionID = oConsumptionRequisition.ConsumptionRequisitionID;
                        if (oItem.ConsumptionRequisitionDetailID <= 0)
                        {
                            readerdetail = ConsumptionRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = ConsumptionRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sConsumptionRequisitionDetailIDs = sConsumptionRequisitionDetailIDs + oReaderDetail.GetString("ConsumptionRequisitionDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sConsumptionRequisitionDetailIDs.Length > 0)
                    {
                        sConsumptionRequisitionDetailIDs = sConsumptionRequisitionDetailIDs.Remove(sConsumptionRequisitionDetailIDs.Length - 1, 1);
                    }
                    ConsumptionRequisitionDetail oConsumptionRequisitionDetail = new ConsumptionRequisitionDetail();
                    oConsumptionRequisitionDetail.ConsumptionRequisitionID = oConsumptionRequisition.ConsumptionRequisitionID;
                    ConsumptionRequisitionDetailDA.Delete(tc, oConsumptionRequisitionDetail, EnumDBOperation.Delete, nUserID, sConsumptionRequisitionDetailIDs);

                }

                #endregion

                #region Again Get CR
                reader = ConsumptionRequisitionDA.Get(tc, oConsumptionRequisition.ConsumptionRequisitionID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oConsumptionRequisition = CreateObject(oReader);
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

                oConsumptionRequisition = new ConsumptionRequisition();
                oConsumptionRequisition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oConsumptionRequisition;
        }

        public ConsumptionRequisition Delivery(ConsumptionRequisition oConsumptionRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ConsumptionRequisitionDetail> oConsumptionRequisitionDetails = new List<ConsumptionRequisitionDetail>();
            oConsumptionRequisitionDetails = oConsumptionRequisition.ConsumptionRequisitionDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ConsumptionRequisition, EnumRoleOperationType.Edit);
                reader = ConsumptionRequisitionDA.InsertUpdate(tc, oConsumptionRequisition, EnumDBOperation.Update, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oConsumptionRequisition = new ConsumptionRequisition();
                    oConsumptionRequisition = CreateObject(oReader);
                }
                reader.Close();

                #region CR Detail Part
                if (oConsumptionRequisitionDetails != null)
                {
                    foreach (ConsumptionRequisitionDetail oItem in oConsumptionRequisitionDetails)
                    {
                        IDataReader readerdetail;
                        oItem.ConsumptionRequisitionID = oConsumptionRequisition.ConsumptionRequisitionID;
                        readerdetail = ConsumptionRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {

                        }
                        readerdetail.Close();
                    }
                }

                #endregion

                #region Delivery Effect
                oConsumptionRequisition.CRActionType = EnumCRActionType.StockOut;
                oConsumptionRequisition.CRStatus = EnumCRStatus.Approve;
                reader = ConsumptionRequisitionDA.ChangeStatus(tc, oConsumptionRequisition, EnumDBOperation.Insert, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oConsumptionRequisition = CreateObject(oReader);
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

                oConsumptionRequisition = new ConsumptionRequisition();
                oConsumptionRequisition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oConsumptionRequisition;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ConsumptionRequisition oConsumptionRequisition = new ConsumptionRequisition();
                oConsumptionRequisition.ConsumptionRequisitionID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ConsumptionRequisition, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ConsumptionRequisition", id);
                VoucherDA.CheckVoucherReference(tc, "ConsumptionRequisition", "ConsumptionRequisitionID", oConsumptionRequisition.ConsumptionRequisitionID);
                ConsumptionRequisitionDA.Delete(tc, oConsumptionRequisition, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ConsumptionRequisition. Because of " + e.Message, e);
                #endregion
            }
            return "Delete successfully";
        }

        public ConsumptionRequisition Get(int id, Int64 nUserId)
        {
            ConsumptionRequisition oConsumptionRequisition = new ConsumptionRequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ConsumptionRequisitionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oConsumptionRequisition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ConsumptionRequisition", e);
                #endregion
            }

            return oConsumptionRequisition;
        }

        public ConsumptionRequisition GetLog(int id, Int64 nUserId)
        {
            ConsumptionRequisition oConsumptionRequisition = new ConsumptionRequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ConsumptionRequisitionDA.GetLog(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oConsumptionRequisition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ConsumptionRequisition", e);
                #endregion
            }

            return oConsumptionRequisition;
        }

        public ConsumptionRequisition ChangeStatus(ConsumptionRequisition oConsumptionRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oConsumptionRequisition.CRActionType == EnumCRActionType.Approve)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ConsumptionRequisition, EnumRoleOperationType.Approved);
                }

                reader = ConsumptionRequisitionDA.ChangeStatus(tc, oConsumptionRequisition, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oConsumptionRequisition = new ConsumptionRequisition();
                    oConsumptionRequisition = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oConsumptionRequisition.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ConsumptionRequisitionDetail. Because of " + e.Message, e);
                #endregion
            }
            return oConsumptionRequisition;
        }
        public List<ConsumptionRequisition> Gets(Int64 nUserID)
        {
            List<ConsumptionRequisition> oConsumptionRequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ConsumptionRequisitionDA.Gets(tc);
                oConsumptionRequisition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ConsumptionRequisition", e);
                #endregion
            }

            return oConsumptionRequisition;
        }

        public List<ConsumptionRequisition> Gets(string sSQL, Int64 nUserID)
        {
            List<ConsumptionRequisition> oConsumptionRequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ConsumptionRequisitionDA.Gets(tc, sSQL);
                oConsumptionRequisition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ConsumptionRequisition", e);
                #endregion
            }

            return oConsumptionRequisition;
        }
        public ConsumptionRequisition AcceptConsumptionRequisitionRevise(ConsumptionRequisition oConsumptionRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ConsumptionRequisitionDetail> oConsumptionRequisitionDetails = new List<ConsumptionRequisitionDetail>();
                ConsumptionRequisitionDetail oConsumptionRequisitionDetail = new ConsumptionRequisitionDetail();
                oConsumptionRequisitionDetails = oConsumptionRequisition.ConsumptionRequisitionDetails;
                string sConsumptionRequisitionDetailIDs = "";

                if (oConsumptionRequisition.ConsumptionRequisitionID > 0)
                {
                    #region ConsumptionRequisition part
                    IDataReader reader;
                    reader = ConsumptionRequisitionDA.AcceptConsumptionRequisitionRevise(tc, oConsumptionRequisition, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oConsumptionRequisition = new ConsumptionRequisition();
                        oConsumptionRequisition = CreateObject(oReader);
                    }
                    reader.Close();

                    #endregion

                    #region ConsumptionRequisition Detail Detail Part
                    if (oConsumptionRequisitionDetails != null)
                    {
                        foreach (ConsumptionRequisitionDetail oItem in oConsumptionRequisitionDetails)
                        {
                            IDataReader readerdetail;
                            oItem.ConsumptionRequisitionID = oConsumptionRequisition.ConsumptionRequisitionID;
                            if (oItem.ConsumptionRequisitionDetailID <= 0)
                            {
                                readerdetail = ConsumptionRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = ConsumptionRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sConsumptionRequisitionDetailIDs = sConsumptionRequisitionDetailIDs + oReaderDetail.GetString("ConsumptionRequisitionDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sConsumptionRequisitionDetailIDs.Length > 0)
                        {
                            sConsumptionRequisitionDetailIDs = sConsumptionRequisitionDetailIDs.Remove(sConsumptionRequisitionDetailIDs.Length - 1, 1);
                        }
                        oConsumptionRequisitionDetail = new ConsumptionRequisitionDetail();
                        oConsumptionRequisitionDetail.ConsumptionRequisitionID = oConsumptionRequisition.ConsumptionRequisitionID;
                        ConsumptionRequisitionDetailDA.Delete(tc, oConsumptionRequisitionDetail, EnumDBOperation.Delete, nUserID, sConsumptionRequisitionDetailIDs);
                    }

                    #endregion

                    #region ConsumptionRequisition Get
                    reader = ConsumptionRequisitionDA.Get(tc, oConsumptionRequisition.ConsumptionRequisitionID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oConsumptionRequisition = CreateObject(oReader);
                    }
                    reader.Close();
                }
                    #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oConsumptionRequisition.ErrorMessage = Message;

                #endregion
            }
            return oConsumptionRequisition;
        }

        public ConsumptionRequisition UpdateVoucherEffect(ConsumptionRequisition oConsumptionRequisition, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ConsumptionRequisitionDA.UpdateVoucherEffect(tc, oConsumptionRequisition);
                IDataReader reader;
                reader = ConsumptionRequisitionDA.Get(tc, oConsumptionRequisition.ConsumptionRequisitionID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oConsumptionRequisition = new ConsumptionRequisition();
                    oConsumptionRequisition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oConsumptionRequisition = new ConsumptionRequisition();
                oConsumptionRequisition.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oConsumptionRequisition;

        }
        #endregion
    }
}

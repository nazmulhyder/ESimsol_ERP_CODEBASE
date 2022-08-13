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

    public class MasterLCService : MarshalByRefObject, IMasterLCService
    {
        #region Private functions and declaration
        private MasterLC MapObject(NullHandler oReader)
        {
            MasterLC oMasterLC = new MasterLC();

            oMasterLC.MasterLCID = oReader.GetInt32("MasterLCID");
            oMasterLC.MasterLCLogID = oReader.GetInt32("MasterLCLogID");
            oMasterLC.FileNo = oReader.GetString("FileNo");
            oMasterLC.LCStatus = (EnumLCStatus)oReader.GetInt32("LCStatus");
            oMasterLC.BUID = oReader.GetInt32("BUID");
            oMasterLC.MasterLCNo = oReader.GetString("MasterLCNo");
            oMasterLC.Applicant = oReader.GetInt32("Applicant");
            oMasterLC.MasterLCDate = oReader.GetDateTime("MasterLCDate");
            oMasterLC.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oMasterLC.LastDateofShipment = oReader.GetDateTime("LastDateofShipment");
            oMasterLC.ExpireDate = oReader.GetDateTime("ExpireDate");
            oMasterLC.Beneficiary = oReader.GetInt32("Beneficiary");
            oMasterLC.IssueBankID = oReader.GetInt32("IssueBankID");
            oMasterLC.AdviceBankID = oReader.GetInt32("AdviceBankID");
            oMasterLC.CurrencyID = oReader.GetInt32("CurrencyID");
            oMasterLC.LCValue = Math.Round(oReader.GetDouble("LCValue"),2);
            oMasterLC.LCQty = oReader.GetDouble("LCQty");
            oMasterLC.Remark = oReader.GetString("Remark");
            oMasterLC.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oMasterLC.ShipmentPort = oReader.GetString("ShipmentPort");
            oMasterLC.PartialShipmentAllow = (EnumPartialShipmentAllow)oReader.GetInt32("PartialShipmentAllow");
            oMasterLC.PartialShipmentAllowInInt = oReader.GetInt32("PartialShipmentAllow");
            oMasterLC.Transferable = (EnumTransferable)oReader.GetInt32("Transferable");
            oMasterLC.TransferableInInt = oReader.GetInt32("Transferable");
            oMasterLC.LCType = (EnumLCType)oReader.GetInt32("LCType");
            oMasterLC.LCTypeInInt = oReader.GetInt32("LCType");
            oMasterLC.DeferredFrom = (EnumDefferedFrom)oReader.GetInt32("DeferredFrom");
            oMasterLC.DeferredFromInInt = oReader.GetInt32("DeferredFrom");
            oMasterLC.DefferedDaysCount = oReader.GetInt32("DefferedDaysCount");
            oMasterLC.DiscrepancyCharge = oReader.GetDouble("DiscrepancyCharge");
            oMasterLC.CurrencyName = oReader.GetString("CurrencyName");
            oMasterLC.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oMasterLC.ApplicantName = oReader.GetString("ApplicantName");
            oMasterLC.BeneficiaryName = oReader.GetString("BeneficiaryName");
            oMasterLC.IssueBankName = oReader.GetString("IssueBankName");
            oMasterLC.AdviceBankName = oReader.GetString("AdviceBankName");
            oMasterLC.ApprovedByName = oReader.GetString("ApprovedByName");
            oMasterLC.YetToTransferValue = oReader.GetDouble("YetToTransferValue");
            oMasterLC.Consignee = oReader.GetInt32("Consignee");
            oMasterLC.NotifyParty = oReader.GetInt32("NotifyParty");
            oMasterLC.MLCWithOrder = oReader.GetBoolean("MLCWithOrder");
            oMasterLC.ConsigneeName = oReader.GetString("ConsigneeName");
            oMasterLC.NotifyPartyName = oReader.GetString("NotifyPartyName");
            oMasterLC.MasterLCType = (EnumMasterLCType)oReader.GetInt32("MasterLCType");
            oMasterLC.MasterLCTypeInInt = oReader.GetInt32("MasterLCTypeInInt");
            oMasterLC.ActualAdvBankID = oReader.GetInt32("ActualAdvBankID");
            oMasterLC.ActualAdvBankName = oReader.GetString("ActualAdvBankName");
            oMasterLC.Country = oReader.GetString("Country");
            oMasterLC.ProductDesc = oReader.GetString("ProductDesc");
            oMasterLC.YetToInvoiceAmount = Math.Round(oReader.GetDouble("YetToInvoiceAmount"),2);
            oMasterLC.OrderTagAmount = oReader.GetDouble("OrderTagAmount");
            oMasterLC.YetToOrderTagAmount = oReader.GetDouble("YetToOrderTagAmount");
            return oMasterLC;
        }

        private MasterLC CreateObject(NullHandler oReader)
        {
            MasterLC oMasterLC = new MasterLC();
            oMasterLC = MapObject(oReader);
            return oMasterLC;
        }

        private List<MasterLC> CreateObjects(IDataReader oReader)
        {
            List<MasterLC> oMasterLC = new List<MasterLC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MasterLC oItem = CreateObject(oHandler);
                oMasterLC.Add(oItem);
            }
            return oMasterLC;
        }

        #endregion

        #region Interface implementation
        public MasterLCService() { }
        public MasterLC Save(MasterLC oMasterLC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<MasterLCDetail> oMasterLCDetails = new List<MasterLCDetail>();
                MasterLCDetail oMasterLCDetail = new MasterLCDetail();


                MasterLCTermsAndCondition oMasterLCTermsAndCondition = new MasterLCTermsAndCondition();
                List<MasterLCTermsAndCondition> oMasterLCTermsAndConditions = new List<MasterLCTermsAndCondition>();

                oMasterLCDetails = oMasterLC.MasterLCDetails;
                oMasterLCTermsAndConditions = oMasterLC.MasterLCTermsAndConditions;

                string sMasterLCDetailIDs = "";
                string sMasterLCTermsAndConditionIDs = "";

                #region Proforma Invoice part
                IDataReader reader;
                if (oMasterLC.MasterLCID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.MasterLC, EnumRoleOperationType.Add);
                    reader = MasterLCDA.InsertUpdate(tc, oMasterLC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.MasterLC, EnumRoleOperationType.Edit);
                    reader = MasterLCDA.InsertUpdate(tc, oMasterLC, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMasterLC = new MasterLC();
                    oMasterLC = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Proforma Invoice Detail Part
                if (oMasterLCDetails != null)
                {
                    foreach (MasterLCDetail oItem in oMasterLCDetails)
                    {
                        IDataReader readerdetail;
                        oItem.MasterLCID = oMasterLC.MasterLCID;
                        if (oItem.MasterLCDetailID <= 0)
                        {
                            readerdetail = MasterLCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = MasterLCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sMasterLCDetailIDs = sMasterLCDetailIDs + oReaderDetail.GetString("MasterLCDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sMasterLCDetailIDs.Length > 0)
                    {
                        sMasterLCDetailIDs = sMasterLCDetailIDs.Remove(sMasterLCDetailIDs.Length - 1, 1);
                    }
                    oMasterLCDetail = new MasterLCDetail();
                    oMasterLCDetail.MasterLCID = oMasterLC.MasterLCID;
                    MasterLCDetailDA.Delete(tc, oMasterLCDetail, EnumDBOperation.Delete, nUserID, sMasterLCDetailIDs);

                }

                #endregion


                #region Proforma Invoice Terms And Condition Part
                if (oMasterLCTermsAndConditions != null)
                {
                    foreach (MasterLCTermsAndCondition oItem in oMasterLCTermsAndConditions)
                    {
                        IDataReader readerTermsAndCondition;
                        oItem.MasterLCID = oMasterLC.MasterLCID;
                        if (oItem.MasterLCTermsAndConditionID <= 0)
                        {
                            readerTermsAndCondition = MasterLCTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, "", nUserID);
                        }
                        else
                        {
                            readerTermsAndCondition = MasterLCTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, "", nUserID);
                        }
                        NullHandler oReaderTermsAndCondition = new NullHandler(readerTermsAndCondition);
                        if (readerTermsAndCondition.Read())
                        {
                            sMasterLCTermsAndConditionIDs = sMasterLCTermsAndConditionIDs + oReaderTermsAndCondition.GetString("MasterLCTermsAndConditionID") + ",";
                        }
                        readerTermsAndCondition.Close();
                    }
                    if (sMasterLCTermsAndConditionIDs.Length > 0)
                    {
                        sMasterLCTermsAndConditionIDs = sMasterLCTermsAndConditionIDs.Remove(sMasterLCTermsAndConditionIDs.Length - 1, 1);
                    }
                    oMasterLCTermsAndCondition = new MasterLCTermsAndCondition();
                    oMasterLCTermsAndCondition.MasterLCID = oMasterLC.MasterLCID;
                    MasterLCTermsAndConditionDA.Delete(tc, oMasterLCTermsAndCondition, EnumDBOperation.Delete, sMasterLCTermsAndConditionIDs, nUserID);

                }

                #endregion

                #region PI Get

                reader = MasterLCDA.Get(tc, oMasterLC.MasterLCID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMasterLC = CreateObject(oReader);
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
                oMasterLC = new MasterLC();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oMasterLC.ErrorMessage = Message;

                #endregion
            }
            return oMasterLC;
        }
        public List<MasterLC> GetsByLCID(int nExportLCID, Int64 nUserId)
        {
            List<MasterLC> oMasterLCs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MasterLCDA.GetsByLCID(tc, nExportLCID);
                oMasterLCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Master LCs", e);
                #endregion
            }
            return oMasterLCs;
        }
        public MasterLC AcceptMasterLCAmmendment(MasterLC oMasterLC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<MasterLCDetail> oMasterLCDetails = new List<MasterLCDetail>();
                MasterLCDetail oMasterLCDetail = new MasterLCDetail();


                MasterLCTermsAndCondition oMasterLCTermsAndCondition = new MasterLCTermsAndCondition();
                List<MasterLCTermsAndCondition> oMasterLCTermsAndConditions = new List<MasterLCTermsAndCondition>();

                oMasterLCDetails = oMasterLC.MasterLCDetails;

                oMasterLCTermsAndConditions = oMasterLC.MasterLCTermsAndConditions;

                string sMasterLCDetailIDs = "";
                string sMasterLCTermsAndConditionIDs = "";

                #region Proforma Invoice part

                if (oMasterLC.MasterLCID > 0)
                {
                    IDataReader reader;
                    reader = MasterLCDA.AcceptMasterLCRevise(tc, oMasterLC, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oMasterLC = new MasterLC();
                        oMasterLC = CreateObject(oReader);
                    }

                    reader.Close();

                #endregion

                    #region Proforma Invoice Detail Part
                    if (oMasterLCDetails != null)
                    {
                        foreach (MasterLCDetail oItem in oMasterLCDetails)
                        {
                            IDataReader readerdetail;
                            oItem.MasterLCID = oMasterLC.MasterLCID;
                            if (oItem.MasterLCDetailID <= 0)
                            {
                                readerdetail = MasterLCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = MasterLCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sMasterLCDetailIDs = sMasterLCDetailIDs + oReaderDetail.GetString("MasterLCDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sMasterLCDetailIDs.Length > 0)
                        {
                            sMasterLCDetailIDs = sMasterLCDetailIDs.Remove(sMasterLCDetailIDs.Length - 1, 1);
                        }
                        oMasterLCDetail = new MasterLCDetail();
                        oMasterLCDetail.MasterLCID = oMasterLC.MasterLCID;
                        MasterLCDetailDA.Delete(tc, oMasterLCDetail, EnumDBOperation.Delete, nUserID, sMasterLCDetailIDs);

                    }

                    #endregion

                    #region Proforma Invoice Terms And Condition Part
                    if (oMasterLCTermsAndConditions != null)
                    {
                        foreach (MasterLCTermsAndCondition oItem in oMasterLCTermsAndConditions)
                        {
                            IDataReader readerTermsAndCondition;
                            oItem.MasterLCID = oMasterLC.MasterLCID;
                            if (oItem.MasterLCTermsAndConditionID <= 0)
                            {
                                readerTermsAndCondition = MasterLCTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, "", nUserID);
                            }
                            else
                            {
                                readerTermsAndCondition = MasterLCTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, "", nUserID);
                            }
                            NullHandler oReaderTermsAndCondition = new NullHandler(readerTermsAndCondition);
                            if (readerTermsAndCondition.Read())
                            {
                                sMasterLCTermsAndConditionIDs = sMasterLCTermsAndConditionIDs + oReaderTermsAndCondition.GetString("MasterLCTermsAndConditionID") + ",";
                            }
                            readerTermsAndCondition.Close();
                        }
                        if (sMasterLCTermsAndConditionIDs.Length > 0)
                        {
                            sMasterLCTermsAndConditionIDs = sMasterLCTermsAndConditionIDs.Remove(sMasterLCTermsAndConditionIDs.Length - 1, 1);
                        }
                        oMasterLCTermsAndCondition = new MasterLCTermsAndCondition();
                        oMasterLCTermsAndCondition.MasterLCID = oMasterLC.MasterLCID;
                        MasterLCTermsAndConditionDA.Delete(tc, oMasterLCTermsAndCondition, EnumDBOperation.Delete, sMasterLCTermsAndConditionIDs, nUserID);

                    }

                    #endregion

                    #region PI Get

                    reader = MasterLCDA.Get(tc, oMasterLC.MasterLCID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oMasterLC = CreateObject(oReader);
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
                oMasterLC.ErrorMessage = Message;

                #endregion
            }
            return oMasterLC;
        }
        public MasterLC ChangeStatus(MasterLC oMasterLC, Int64 nUserID)
        {
            TransactionContext tc = null;
            ApprovalRequest oApprovalRequest = new ApprovalRequest();
            ReviseRequest oReviseRequest = new ReviseRequest();
            oApprovalRequest = oMasterLC.ApprovalRequest;
            oReviseRequest = oMasterLC.ReviseRequest;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMasterLC.MasterLCActionType == EnumMasterLCActionType.Approve)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.MasterLC, EnumRoleOperationType.Approved);
                }
                if (oMasterLC.MasterLCActionType == EnumMasterLCActionType.Cancel)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.MasterLC, EnumRoleOperationType.Cancel);
                }

                reader = MasterLCDA.ChangeStatus(tc, oMasterLC, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMasterLC = new MasterLC();
                    oMasterLC = CreateObject(oReader);
                }
                reader.Close();
                if (oMasterLC.LCStatus == EnumLCStatus.Req_For_App)
                {
                    IDataReader ApprovalRequestreader;
                    ApprovalRequestreader = ApprovalRequestDA.InsertUpdate(tc, oApprovalRequest, EnumDBOperation.Insert, nUserID);
                    if (ApprovalRequestreader.Read())
                    {

                    }
                    ApprovalRequestreader.Close();
                }
                else if (oMasterLC.LCStatus == EnumLCStatus.Req_for_Ammendment)
                {
                    IDataReader AmendmentRequestreader;
                    AmendmentRequestreader = ReviseRequestDA.InsertUpdate(tc, oReviseRequest, EnumDBOperation.Insert, nUserID);
                    if (AmendmentRequestreader.Read())
                    {

                    }
                    AmendmentRequestreader.Close();
                }

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
                oMasterLC.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save MasterLCDetail. Because of " + e.Message, e);
                #endregion
            }
            return oMasterLC;
        }
        public string Delete(int nMasterLCID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MasterLC oMasterLC = new MasterLC();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.MasterLC, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "MasterLC", nMasterLCID);
                oMasterLC.MasterLCID = nMasterLCID;
                MasterLCDA.Delete(tc, oMasterLC, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public MasterLC Get(int id, Int64 nUserId)
        {
            MasterLC oMasterLC = new MasterLC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MasterLCDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMasterLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get MasterLC", e);
                #endregion
            }

            return oMasterLC;
        }
        public MasterLC GetBySaleOrder(int id, Int64 nUserId)
        {
            MasterLC oMasterLC = new MasterLC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MasterLCDA.GetBySaleOrder(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMasterLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get MasterLC", e);
                #endregion
            }

            return oMasterLC;
        }

        public MasterLC GetLog(int id, Int64 nUserId)
        {
            MasterLC oAccountHead = new MasterLC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MasterLCDA.GetLog(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get MasterLC", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<MasterLC> Gets(int buid, Int64 nUserID)
        {
            List<MasterLC> oMasterLC = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterLCDA.Gets(buid, tc);
                oMasterLC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLC", e);
                #endregion
            }

            return oMasterLC;
        }

        public List<MasterLC> GetsMasterLCLog(int id, Int64 nUserID)
        {
            List<MasterLC> oMasterLC = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterLCDA.GetsMasterLCLog(id, tc);
                oMasterLC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLC", e);
                #endregion
            }

            return oMasterLC;
        }

        public List<MasterLC> Gets(string sSQL, Int64 nUserID)
        {
            List<MasterLC> oMasterLC = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterLCDA.Gets(tc, sSQL);
                oMasterLC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLC", e);
                #endregion
            }

            return oMasterLC;
        }
        public List<MasterLC> GetsByContractorID(int nContractorID, Int64 nUserId)
        {
            List<MasterLC> oMasterLCs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MasterLCDA.GetsByContractorID(tc, nContractorID);
                oMasterLCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Master LCs", e);
                #endregion
            }
            return oMasterLCs;
        }
        #endregion
    }


}

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


    public class ProformaInvoiceService : MarshalByRefObject, IProformaInvoiceService
    {
        #region Private functions and declaration
        private ProformaInvoice MapObject(NullHandler oReader)
        {
            ProformaInvoice oProformaInvoice = new ProformaInvoice();
            oProformaInvoice.ProformaInvoiceID = oReader.GetInt32("ProformaInvoiceID");
            oProformaInvoice.ProformaInvoiceLogID = oReader.GetInt32("ProformaInvoiceLogID");
            oProformaInvoice.PINo = oReader.GetString("PINo");
            oProformaInvoice.IssueDate = oReader.GetDateTime("IssueDate");
            oProformaInvoice.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
            oProformaInvoice.PIStatusInInt = oReader.GetInt32("PIStatus");            
            oProformaInvoice.BuyerID = oReader.GetInt32("BuyerID");
            oProformaInvoice.BuyerName = oReader.GetString("BuyerName");
            oProformaInvoice.LCFavorOf = oReader.GetInt32("LCFavorOf");
            oProformaInvoice.LCFavorOfName = oReader.GetString("LCFavorOfName");
            oProformaInvoice.TransferBankAccountID = oReader.GetInt32("TransferBankAccountID");
            oProformaInvoice.UnitID = oReader.GetInt32("UnitID");
            oProformaInvoice.UnitName = oReader.GetString("UnitName");
            oProformaInvoice.CurrencyID = oReader.GetInt32("CurrencyID");
            oProformaInvoice.CurrencyName = oReader.GetString("CurrencyName");
            oProformaInvoice.PaymentTerm = (EnumPaymentTerm)oReader.GetInt32("PaymentTerm");
            oProformaInvoice.PaymentTermInInt = oReader.GetInt32("PaymentTerm");
            oProformaInvoice.Origin = oReader.GetString("Origin");
            oProformaInvoice.DeliveryTerm = (EnumDeliveryTerm)oReader.GetInt32("DeliveryTerm");
            oProformaInvoice.DeliveryTermInInt = oReader.GetInt32("DeliveryTerm");
            oProformaInvoice.PortOfLoadingAir = oReader.GetString("PortOfLoadingAir");
            oProformaInvoice.PortOfLoadingSea = oReader.GetString("PortOfLoadingSea");
            oProformaInvoice.Note = oReader.GetString("Note");
            oProformaInvoice.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oProformaInvoice.ApprovedByName = oReader.GetString("ApprovedByName");
            oProformaInvoice.Quantity = oReader.GetDouble("Quantity");            
            oProformaInvoice.TransferingBankName = oReader.GetString("TransferingBankName");
            oProformaInvoice.LCReceived = oReader.GetString("LCReceived");
            oProformaInvoice.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oProformaInvoice.UnitSymbol = oReader.GetString("UnitSymbol");
            oProformaInvoice.BuyerAddress = oReader.GetString("BuyerAddress");
            oProformaInvoice.BuyerPhone = oReader.GetString("BuyerPhone");
            oProformaInvoice.BuyerFax = oReader.GetString("BuyerFax");
            oProformaInvoice.BankName = oReader.GetString("BankName");
            oProformaInvoice.SwiftCode = oReader.GetString("SwiftCode");
            oProformaInvoice.AccountNo = oReader.GetString("AccountNo");
            oProformaInvoice.BranchName = oReader.GetString("BranchName");
            oProformaInvoice.BranchAddress = oReader.GetString("BranchAddress");
            oProformaInvoice.ApplicantID = oReader.GetInt32("ApplicantID");
            oProformaInvoice.ApplicantName = oReader.GetString("ApplicantName");
            oProformaInvoice.GrossAmount = oReader.GetDouble("GrossAmount");
            oProformaInvoice.DiscountAmount = oReader.GetDouble("DiscountAmount");
            oProformaInvoice.AdditionalAmount = oReader.GetDouble("AdditionalAmount");
            oProformaInvoice.NetAmount = oReader.GetDouble("NetAmount");
            oProformaInvoice.CauseOfAddition = oReader.GetString("CauseOfAddition");
            oProformaInvoice.CauseOfDiscount = oReader.GetString("CauseOfDiscount");
            oProformaInvoice.BUID = oReader.GetInt32("BUID");
            return oProformaInvoice;
        }

        private ProformaInvoice CreateObject(NullHandler oReader)
        {
            ProformaInvoice oProformaInvoice = new ProformaInvoice();
            oProformaInvoice = MapObject(oReader);
            return oProformaInvoice;
        }

        private List<ProformaInvoice> CreateObjects(IDataReader oReader)
        {
            List<ProformaInvoice> oProformaInvoice = new List<ProformaInvoice>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProformaInvoice oItem = CreateObject(oHandler);
                oProformaInvoice.Add(oItem);
            }
            return oProformaInvoice;
        }

        #endregion

        #region Interface implementation
        public ProformaInvoiceService() { }
        public ProformaInvoice Save(ProformaInvoice oProformaInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ProformaInvoiceDetail> oProformaInvoiceDetails = new List<ProformaInvoiceDetail>();
                ProformaInvoiceDetail oProformaInvoiceDetail = new ProformaInvoiceDetail();
                ProformaInvoiceRequiredDoc oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();
                List<ProformaInvoiceRequiredDoc> oProformaInvoiceRequiredDocs = new List<ProformaInvoiceRequiredDoc>();

                ProformaInvoiceTermsAndCondition oProformaInvoiceTermsAndCondition = new ProformaInvoiceTermsAndCondition();
                List<ProformaInvoiceTermsAndCondition> oProformaInvoiceTermsAndConditions = new List<ProformaInvoiceTermsAndCondition>();

                oProformaInvoiceDetails = oProformaInvoice.ProformaInvoiceDetails;
                oProformaInvoiceRequiredDocs = oProformaInvoice.ProformaInvoiceRequiredDocs;
                oProformaInvoiceTermsAndConditions = oProformaInvoice.ProformaInvoiceTermsAndConditions;

                string sProformaInvoiceDetailIDs = "";
                string sProformaInvoiceRequiredDocIDs = "";
                string sProformaInvoiceTermsAndConditionIDs = "";

                #region Proforma Invoice part
                IDataReader reader;
                if (oProformaInvoice.ProformaInvoiceID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProformaInvoice, EnumRoleOperationType.Add);
                    reader = ProformaInvoiceDA.InsertUpdate(tc, oProformaInvoice, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProformaInvoice, EnumRoleOperationType.Edit);
                    reader = ProformaInvoiceDA.InsertUpdate(tc, oProformaInvoice, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProformaInvoice = new ProformaInvoice();
                    oProformaInvoice = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Proforma Invoice Detail Part
                if (oProformaInvoiceDetails != null)
                {
                    foreach (ProformaInvoiceDetail oItem in oProformaInvoiceDetails)
                    {
                        IDataReader readerdetail;
                        oItem.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                        if (oItem.ProformaInvoiceDetailID <= 0)
                        {
                            readerdetail = ProformaInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = ProformaInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sProformaInvoiceDetailIDs = sProformaInvoiceDetailIDs + oReaderDetail.GetString("ProformaInvoiceDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sProformaInvoiceDetailIDs.Length > 0)
                    {
                        sProformaInvoiceDetailIDs = sProformaInvoiceDetailIDs.Remove(sProformaInvoiceDetailIDs.Length - 1, 1);
                    }
                    oProformaInvoiceDetail = new ProformaInvoiceDetail();
                    oProformaInvoiceDetail.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                    ProformaInvoiceDetailDA.Delete(tc, oProformaInvoiceDetail, EnumDBOperation.Delete, nUserID, sProformaInvoiceDetailIDs);

                }

                #endregion

                #region Proforma Invoice Required Doc Part
                if (oProformaInvoiceRequiredDocs != null)
                {
                    foreach (ProformaInvoiceRequiredDoc oItem in oProformaInvoiceRequiredDocs)
                    {
                        IDataReader readerRequiredDoc;
                        oItem.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                        if (oItem.ProformaInvoiceRequiredDocID <= 0)
                        {
                            readerRequiredDoc = ProformaInvoiceRequiredDocDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, "", nUserID);
                        }
                        else
                        {
                            readerRequiredDoc = ProformaInvoiceRequiredDocDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, "", nUserID);
                        }
                        NullHandler oReaderRequiredDoc = new NullHandler(readerRequiredDoc);
                        if (readerRequiredDoc.Read())
                        {
                            sProformaInvoiceRequiredDocIDs = sProformaInvoiceRequiredDocIDs + oReaderRequiredDoc.GetString("ProformaInvoiceRequiredDocID") + ",";
                        }
                        readerRequiredDoc.Close();
                    }
                    if (sProformaInvoiceRequiredDocIDs.Length > 0)
                    {
                        sProformaInvoiceRequiredDocIDs = sProformaInvoiceRequiredDocIDs.Remove(sProformaInvoiceRequiredDocIDs.Length - 1, 1);
                    }
                    oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();
                    oProformaInvoiceRequiredDoc.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                    ProformaInvoiceRequiredDocDA.Delete(tc, oProformaInvoiceRequiredDoc, EnumDBOperation.Delete, sProformaInvoiceRequiredDocIDs, nUserID);

                }

                #endregion

                #region Proforma Invoice Terms And Condition Part
                if (oProformaInvoiceTermsAndConditions != null)
                {
                    foreach (ProformaInvoiceTermsAndCondition oItem in oProformaInvoiceTermsAndConditions)
                    {
                        IDataReader readerTermsAndCondition;
                        oItem.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                        if (oItem.ProformaInvoiceTermsAndConditionID <= 0)
                        {
                            readerTermsAndCondition = ProformaInvoiceTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, "", nUserID);
                        }
                        else
                        {
                            readerTermsAndCondition = ProformaInvoiceTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, "", nUserID);
                        }
                        NullHandler oReaderTermsAndCondition = new NullHandler(readerTermsAndCondition);
                        if (readerTermsAndCondition.Read())
                        {
                            sProformaInvoiceTermsAndConditionIDs = sProformaInvoiceTermsAndConditionIDs + oReaderTermsAndCondition.GetString("ProformaInvoiceTermsAndConditionID") + ",";
                        }
                        readerTermsAndCondition.Close();
                    }
                    if (sProformaInvoiceTermsAndConditionIDs.Length > 0)
                    {
                        sProformaInvoiceTermsAndConditionIDs = sProformaInvoiceTermsAndConditionIDs.Remove(sProformaInvoiceTermsAndConditionIDs.Length - 1, 1);
                    }
                    oProformaInvoiceTermsAndCondition = new ProformaInvoiceTermsAndCondition();
                    oProformaInvoiceTermsAndCondition.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                    ProformaInvoiceTermsAndConditionDA.Delete(tc, oProformaInvoiceTermsAndCondition, EnumDBOperation.Delete, sProformaInvoiceTermsAndConditionIDs, nUserID);

                }

                #endregion

                #region PI Get

                reader = ProformaInvoiceDA.Get(tc, oProformaInvoice.ProformaInvoiceID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProformaInvoice = CreateObject(oReader);
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

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oProformaInvoice.ErrorMessage = Message;

                #endregion
            }
            return oProformaInvoice;
        }
        public ProformaInvoice AcceptProformaInvoiceRevise(ProformaInvoice oProformaInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ProformaInvoiceDetail> oProformaInvoiceDetails = new List<ProformaInvoiceDetail>();
                ProformaInvoiceDetail oProformaInvoiceDetail = new ProformaInvoiceDetail();
                ProformaInvoiceRequiredDoc oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();
                List<ProformaInvoiceRequiredDoc> oProformaInvoiceRequiredDocs = new List<ProformaInvoiceRequiredDoc>();

                ProformaInvoiceTermsAndCondition oProformaInvoiceTermsAndCondition = new ProformaInvoiceTermsAndCondition();
                List<ProformaInvoiceTermsAndCondition> oProformaInvoiceTermsAndConditions = new List<ProformaInvoiceTermsAndCondition>();

                oProformaInvoiceDetails = oProformaInvoice.ProformaInvoiceDetails;
                oProformaInvoiceRequiredDocs = oProformaInvoice.ProformaInvoiceRequiredDocs;
                oProformaInvoiceTermsAndConditions = oProformaInvoice.ProformaInvoiceTermsAndConditions;

                string sProformaInvoiceDetailIDs = "";
                string sProformaInvoiceRequiredDocIDs = "";
                string sProformaInvoiceTermsAndConditionIDs = "";
                double nTotalNewDetailQty = 0;//This value for Validation chek for Revise
                foreach(ProformaInvoiceDetail oItem in oProformaInvoiceDetails)
                {
                    nTotalNewDetailQty += oItem.Quantity;
                }

                    #region Proforma Invoice part

                if (oProformaInvoice.ProformaInvoiceID > 0)
                {
                    IDataReader reader;
                    reader = ProformaInvoiceDA.AcceptProformaInvoiceRevise(tc, oProformaInvoice,nTotalNewDetailQty, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oProformaInvoice = new ProformaInvoice();
                        oProformaInvoice = CreateObject(oReader);
                    }

                    reader.Close();

                #endregion

                    #region Proforma Invoice Detail Part
                    if (oProformaInvoiceDetails != null)
                    {
                        foreach (ProformaInvoiceDetail oItem in oProformaInvoiceDetails)
                        {
                            IDataReader readerdetail;
                            oItem.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                            if (oItem.ProformaInvoiceDetailID <= 0)
                            {
                                readerdetail = ProformaInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = ProformaInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sProformaInvoiceDetailIDs = sProformaInvoiceDetailIDs + oReaderDetail.GetString("ProformaInvoiceDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sProformaInvoiceDetailIDs.Length > 0)
                        {
                            sProformaInvoiceDetailIDs = sProformaInvoiceDetailIDs.Remove(sProformaInvoiceDetailIDs.Length - 1, 1);
                        }
                        oProformaInvoiceDetail = new ProformaInvoiceDetail();
                        oProformaInvoiceDetail.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                        ProformaInvoiceDetailDA.Delete(tc, oProformaInvoiceDetail, EnumDBOperation.Delete, nUserID, sProformaInvoiceDetailIDs);

                    }

                    #endregion

                    #region Proforma Invoice Required Doc Part
                    if (oProformaInvoiceRequiredDocs != null)
                    {
                        foreach (ProformaInvoiceRequiredDoc oItem in oProformaInvoiceRequiredDocs)
                        {
                            IDataReader readerRequiredDoc;
                            oItem.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                            if (oItem.ProformaInvoiceRequiredDocID <= 0)
                            {
                                readerRequiredDoc = ProformaInvoiceRequiredDocDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, "", nUserID);
                            }
                            else
                            {
                                readerRequiredDoc = ProformaInvoiceRequiredDocDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, "", nUserID);
                            }
                            NullHandler oReaderRequiredDoc = new NullHandler(readerRequiredDoc);
                            if (readerRequiredDoc.Read())
                            {
                                sProformaInvoiceRequiredDocIDs = sProformaInvoiceRequiredDocIDs + oReaderRequiredDoc.GetString("ProformaInvoiceRequiredDocID") + ",";
                            }
                            readerRequiredDoc.Close();
                        }
                        if (sProformaInvoiceRequiredDocIDs.Length > 0)
                        {
                            sProformaInvoiceRequiredDocIDs = sProformaInvoiceRequiredDocIDs.Remove(sProformaInvoiceRequiredDocIDs.Length - 1, 1);
                        }
                        oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();
                        oProformaInvoiceRequiredDoc.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                        ProformaInvoiceRequiredDocDA.Delete(tc, oProformaInvoiceRequiredDoc, EnumDBOperation.Delete, sProformaInvoiceRequiredDocIDs, nUserID);

                    }

                    #endregion

                    #region Proforma Invoice Terms And Condition Part
                    if (oProformaInvoiceTermsAndConditions != null)
                    {
                        foreach (ProformaInvoiceTermsAndCondition oItem in oProformaInvoiceTermsAndConditions)
                        {
                            IDataReader readerTermsAndCondition;
                            oItem.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                            if (oItem.ProformaInvoiceTermsAndConditionID <= 0)
                            {
                                readerTermsAndCondition = ProformaInvoiceTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, "", nUserID);
                            }
                            else
                            {
                                readerTermsAndCondition = ProformaInvoiceTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, "", nUserID);
                            }
                            NullHandler oReaderTermsAndCondition = new NullHandler(readerTermsAndCondition);
                            if (readerTermsAndCondition.Read())
                            {
                                sProformaInvoiceTermsAndConditionIDs = sProformaInvoiceTermsAndConditionIDs + oReaderTermsAndCondition.GetString("ProformaInvoiceTermsAndConditionID") + ",";
                            }
                            readerTermsAndCondition.Close();
                        }
                        if (sProformaInvoiceTermsAndConditionIDs.Length > 0)
                        {
                            sProformaInvoiceTermsAndConditionIDs = sProformaInvoiceTermsAndConditionIDs.Remove(sProformaInvoiceTermsAndConditionIDs.Length - 1, 1);
                        }
                        oProformaInvoiceTermsAndCondition = new ProformaInvoiceTermsAndCondition();
                        oProformaInvoiceTermsAndCondition.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                        ProformaInvoiceTermsAndConditionDA.Delete(tc, oProformaInvoiceTermsAndCondition, EnumDBOperation.Delete, sProformaInvoiceTermsAndConditionIDs, nUserID);

                    }

                    #endregion

                    #region PI Get

                    reader = ProformaInvoiceDA.Get(tc, oProformaInvoice.ProformaInvoiceID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oProformaInvoice = CreateObject(oReader);
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
                oProformaInvoice.ErrorMessage = Message;

                #endregion
            }
            return oProformaInvoice;
        }
        public ProformaInvoice ChangeStatus(ProformaInvoice oProformaInvoice,  Int64 nUserID)
        {
            ApprovalRequest oApprovalRequest = new ApprovalRequest();
            ReviseRequest oReviseRequest = new ReviseRequest();
            oApprovalRequest = oProformaInvoice.ApprovalRequest;
            oReviseRequest = oProformaInvoice.ReviseRequest;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProformaInvoice.ProformaInvoiceActionType == EnumProformaInvoiceActionType.Approve)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProformaInvoice, EnumRoleOperationType.Approved);
                }
                if (oProformaInvoice.ProformaInvoiceActionType == EnumProformaInvoiceActionType.Cancel)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProformaInvoice, EnumRoleOperationType.Cancel);
                }
                reader = ProformaInvoiceDA.ChangeStatus(tc, oProformaInvoice, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProformaInvoice = new ProformaInvoice();
                    oProformaInvoice = CreateObject(oReader);
                }
                reader.Close();
                if (oProformaInvoice.PIStatus == EnumPIStatus.RequestForApproved)
                {
                    IDataReader ApprovalRequestreader;
                    ApprovalRequestreader = ApprovalRequestDA.InsertUpdate(tc, oApprovalRequest, EnumDBOperation.Insert, nUserID);
                    if (ApprovalRequestreader.Read())
                    {

                    }
                    ApprovalRequestreader.Close();
                }
                else if (oProformaInvoice.PIStatus == EnumPIStatus.RequestForRevise)
                {
                    IDataReader ReviseRequestreader;
                    ReviseRequestreader = ReviseRequestDA.InsertUpdate(tc, oReviseRequest, EnumDBOperation.Insert, nUserID);
                    if (ReviseRequestreader.Read())
                    {

                    }
                    ReviseRequestreader.Close();
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
                oProformaInvoice.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ProformaInvoiceDetail. Because of " + e.Message, e);
                #endregion
            }
            return oProformaInvoice;
        }
        public string Delete(int nProformaInvoiceID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProformaInvoice oProformaInvoice = new ProformaInvoice();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ProformaInvoice, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ProformaInvoice", nProformaInvoiceID);
                oProformaInvoice.ProformaInvoiceID = nProformaInvoiceID;
                ProformaInvoiceDA.Delete(tc, oProformaInvoice, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }
        public ProformaInvoice Get(int id, Int64 nUserId)
        {
            ProformaInvoice oAccountHead = new ProformaInvoice();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProformaInvoiceDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ProformaInvoice", e);
                #endregion
            }

            return oAccountHead;
        }
        public ProformaInvoice GetLog(int id, Int64 nUserId)
        {
            ProformaInvoice oAccountHead = new ProformaInvoice();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProformaInvoiceDA.GetLog(tc, id);
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
                throw new ServiceException("Failed to Get ProformaInvoice", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<ProformaInvoice> Gets(int buid, Int64 nUserID)
        {
            List<ProformaInvoice> oProformaInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceDA.Gets(tc, buid);
                oProformaInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoice", e);
                #endregion
            }

            return oProformaInvoice;
        }
        public List<ProformaInvoice> GetsPILog(int id, Int64 nUserID)
        {
            List<ProformaInvoice> oProformaInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceDA.GetsPILog(id, tc);
                oProformaInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoice", e);
                #endregion
            }

            return oProformaInvoice;
        }
        public List<ProformaInvoice> Gets(string sSQL, Int64 nUserID)
        {
            List<ProformaInvoice> oProformaInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceDA.Gets(tc, sSQL);
                oProformaInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoice", e);
                #endregion
            }

            return oProformaInvoice;
        }
        #endregion
    }
  
}

using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ImportPaymentRequestService : MarshalByRefObject, IImportPaymentRequestService
    {
        #region Private functions and declaration
        private ImportPaymentRequest MapObject(NullHandler oReader)
        {
            ImportPaymentRequest oImportPaymentRequest = new ImportPaymentRequest();
            oImportPaymentRequest.ImportPaymentRequestID = oReader.GetInt32("ImportPaymentRequestID");
            oImportPaymentRequest.RefNo = oReader.GetString("RefNo");
            oImportPaymentRequest.BankAccountID = oReader.GetInt32("BankAccountID");
            oImportPaymentRequest.BUID = oReader.GetInt32("BUID");
            oImportPaymentRequest.LetterIssueDate = oReader.GetDateTime("LetterIssueDate");
            oImportPaymentRequest.RequestBy = oReader.GetInt32("RequestBy");
            oImportPaymentRequest.ApprovedBy = oReader.GetInt32("ApprovedBy");
            //oImportPaymentRequest.ValueInPercentage = oReader.GetDouble("ValueInPercentage");
            oImportPaymentRequest.Note = oReader.GetString("Note");
            oImportPaymentRequest.AccountNo = oReader.GetString("AccountNo");
            oImportPaymentRequest.BankName = oReader.GetString("BankName");
            oImportPaymentRequest.BankID = oReader.GetInt32("BankID");
            oImportPaymentRequest.BranchName = oReader.GetString("BranchName");
            oImportPaymentRequest.BranchAddress = oReader.GetString("BranchAddress");
            oImportPaymentRequest.RequestByName = oReader.GetString("RequestByName");
            oImportPaymentRequest.ApprovedByName = oReader.GetString("ApprovedByName");
            oImportPaymentRequest.LiabilityType = (EnumLiabilityType)oReader.GetInt32("LiabilityType");
            oImportPaymentRequest.LiabilityTypeInt = oReader.GetInt32("LiabilityType");
            oImportPaymentRequest.Status = oReader.GetInt32("Status");
            oImportPaymentRequest.StatusInt = oReader.GetInt32("Status");
            oImportPaymentRequest.BankBranchID = oReader.GetInt32("BankBranchID");
            oImportPaymentRequest.CurrencyType = oReader.GetInt32("CurrencyType");
            oImportPaymentRequest.Paymentthrough = oReader.GetInt32("Paymentthrough");
            oImportPaymentRequest.CRate = oReader.GetInt32("CRate");
            
            return oImportPaymentRequest;
        }

        private ImportPaymentRequest CreateObject(NullHandler oReader)
        {
            ImportPaymentRequest oImportPaymentRequest = new ImportPaymentRequest();
            oImportPaymentRequest = MapObject(oReader);
            return oImportPaymentRequest;
        }

        private List<ImportPaymentRequest> CreateObjects(IDataReader oReader)
        {
            List<ImportPaymentRequest> oImportPaymentRequests = new List<ImportPaymentRequest>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPaymentRequest oItem = CreateObject(oHandler);
                oImportPaymentRequests.Add(oItem);
            }
            return oImportPaymentRequests;
        }

        #endregion

        #region Interface implementation
        public ImportPaymentRequestService() { }

        public ImportPaymentRequest Save(ImportPaymentRequest oImportPaymentRequest, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ImportPaymentRequestDetail> oImportPaymentRequestDetails = new List<ImportPaymentRequestDetail>();
                ImportPaymentRequestDetail oImportPaymentRequestDetail = new ImportPaymentRequestDetail();
                oImportPaymentRequestDetails = oImportPaymentRequest.ImportPaymentRequestDetails;

                string sImportPaymentRequestDetailIDs = "";

                #region ImportPaymentRequest part
                IDataReader reader;
                if (oImportPaymentRequest.ImportPaymentRequestID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "ImportPaymentRequest", EnumRoleOperationType.Add);
                    reader = ImportPaymentRequestDA.InsertUpdate(tc, oImportPaymentRequest, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "ImportPaymentRequest", EnumRoleOperationType.Edit);
                    reader = ImportPaymentRequestDA.InsertUpdate(tc, oImportPaymentRequest, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPaymentRequest = new ImportPaymentRequest();
                    oImportPaymentRequest = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Invoice Purchase Detail Part
                if (oImportPaymentRequestDetails != null)
                {
                    foreach (ImportPaymentRequestDetail oItem in oImportPaymentRequestDetails)
                    {
                        IDataReader readerdetail;
                        oItem.PIPRID = oImportPaymentRequest.ImportPaymentRequestID;
                        if (oItem.PIPRDetailID <= 0)
                        {
                            readerdetail = ImportPaymentRequestDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = ImportPaymentRequestDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sImportPaymentRequestDetailIDs = sImportPaymentRequestDetailIDs + oReaderDetail.GetString("PIPRDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sImportPaymentRequestDetailIDs.Length > 0)
                    {
                        sImportPaymentRequestDetailIDs = sImportPaymentRequestDetailIDs.Remove(sImportPaymentRequestDetailIDs.Length - 1, 1);
                    }
                    oImportPaymentRequestDetail = new ImportPaymentRequestDetail();
                    oImportPaymentRequestDetail.PIPRID = oImportPaymentRequest.ImportPaymentRequestID;
                    //ImportPaymentRequestDetailDA.Delete(tc, oImportPaymentRequestDetail, EnumDBOperation.Delete, nUserID, sImportPaymentRequestDetailIDs);
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
                oImportPaymentRequest.ErrorMessage = Message;
                #endregion
            }
            return oImportPaymentRequest;
        }
        public ImportPaymentRequest Cancel_Request(ImportPaymentRequest oImportPaymentRequest, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ImportPaymentRequestDetail> oImportPaymentRequestDetails = new List<ImportPaymentRequestDetail>();
                ImportPaymentRequestDetail oImportPaymentRequestDetail = new ImportPaymentRequestDetail();
                oImportPaymentRequestDetails = oImportPaymentRequest.ImportPaymentRequestDetails;

                string sImportPaymentRequestDetailIDs = "";

                #region ImportPaymentRequest part
                IDataReader reader;
                if (oImportPaymentRequest.ImportPaymentRequestID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "ImportPaymentRequest", EnumRoleOperationType.Add);
                   reader = ImportPaymentRequestDA.InsertUpdate(tc, oImportPaymentRequest, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "ImportPaymentRequest", EnumRoleOperationType.Edit);
                    reader = ImportPaymentRequestDA.InsertUpdate(tc, oImportPaymentRequest, EnumDBOperation.Cancel, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPaymentRequest = new ImportPaymentRequest();
                    oImportPaymentRequest = CreateObject(oReader);
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
                oImportPaymentRequest.ErrorMessage = Message;
                #endregion
            }
            return oImportPaymentRequest;
        }
        public ImportPaymentRequest Approved(ImportPaymentRequest oImportPaymentRequest, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                                
                IDataReader reader;
                reader = ImportPaymentRequestDA.InsertUpdate(tc, oImportPaymentRequest, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPaymentRequest = new ImportPaymentRequest();
                    oImportPaymentRequest = CreateObject(oReader);
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
                oImportPaymentRequest.ErrorMessage = Message;
                #endregion
            }
            return oImportPaymentRequest;
        }


        public string Delete(ImportPaymentRequest oImportPaymentRequest, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
 
                ImportPaymentRequestDA.Delete(tc, oImportPaymentRequest, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ImportPaymentRequest. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ImportPaymentRequest Get(int id, Int64 nUserId)
        {
            ImportPaymentRequest oImportPaymentRequest = new ImportPaymentRequest();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPaymentRequestDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPaymentRequest = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportPaymentRequest", e);
                #endregion
            }

            return oImportPaymentRequest;
        }
        public ImportPaymentRequest GetByPInvoice(int id, Int64 nUserId)
        {
            ImportPaymentRequest oImportPaymentRequest = new ImportPaymentRequest();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPaymentRequestDA.GetByPInvoice(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPaymentRequest = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportPaymentRequest", e);
                #endregion
            }

            return oImportPaymentRequest;
        }
            
        public List<ImportPaymentRequest> Gets(Int64 nUserID)
        {
            List<ImportPaymentRequest> oImportPaymentRequest = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPaymentRequestDA.Gets(tc);
                oImportPaymentRequest = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPaymentRequest", e);
                #endregion
            }

            return oImportPaymentRequest;
        }

        public List<ImportPaymentRequest> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportPaymentRequest> oImportPaymentRequest = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPaymentRequestDA.Gets(tc, sSQL);
                oImportPaymentRequest = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPaymentRequest", e);
                #endregion
            }

            return oImportPaymentRequest;
        }

        public List<ImportPaymentRequest> WaitForApproval(int nBUID, Int64 nUserID)
        {
            List<ImportPaymentRequest> oImportPaymentRequest = new List<ImportPaymentRequest>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportPaymentRequestDA.WaitForApproval(tc, nBUID);
                oImportPaymentRequest = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPaymentRequest", e);
                #endregion
            }

            return oImportPaymentRequest;
        }       
        #endregion
    }
}

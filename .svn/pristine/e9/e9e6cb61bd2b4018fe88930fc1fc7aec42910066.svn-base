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
    public class SampleRequestService : MarshalByRefObject, ISampleRequestService
    {
        #region Private functions and declaration

        private SampleRequest MapObject(NullHandler oReader)
        {
            SampleRequest oSampleRequest = new SampleRequest();
            oSampleRequest.SampleRequestID = oReader.GetInt32("SampleRequestID");
            oSampleRequest.RequestBy = oReader.GetInt32("RequestBy");
            oSampleRequest.RequestDate = oReader.GetDateTime("RequestDate");
            oSampleRequest.RequestTo = oReader.GetInt32("RequestTo");
            oSampleRequest.BUID = oReader.GetInt32("BUID");
            oSampleRequest.Remarks = oReader.GetString("Remarks");
            oSampleRequest.RequestNo = oReader.GetString("RequestNo");
            oSampleRequest.RequestType = (EnumProductNature)oReader.GetInt32("RequestType");
            oSampleRequest.ContractorID = oReader.GetInt32("ContractorID");
            oSampleRequest.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oSampleRequest.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oSampleRequest.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oSampleRequest.DisbursedBy = oReader.GetInt32("DisbursedBy");
            oSampleRequest.ApprovedByName = oReader.GetString("ApprovedByName");
            oSampleRequest.DisbursedByName = oReader.GetString("DisbursedByName");
            oSampleRequest.WUName = oReader.GetString("WUName");
            oSampleRequest.ContactPersonName = oReader.GetString("ContactPersonName");
            oSampleRequest.ContractorName = oReader.GetString("ContractorName");
            oSampleRequest.RequestToName = oReader.GetString("RequestToName");
            oSampleRequest.RequestByName = oReader.GetString("RequestByName");

            return oSampleRequest;
        }

        private SampleRequest CreateObject(NullHandler oReader)
        {
            SampleRequest oSampleRequest = new SampleRequest();
            oSampleRequest = MapObject(oReader);
            return oSampleRequest;
        }

        private List<SampleRequest> CreateObjects(IDataReader oReader)
        {
            List<SampleRequest> oSampleRequest = new List<SampleRequest>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleRequest oItem = CreateObject(oHandler);
                oSampleRequest.Add(oItem);
            }
            return oSampleRequest;
        }

        #endregion

        #region Interface implementation
        public SampleRequest Approved(SampleRequest oSampleRequest, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Valid SampleRequest
                if (oSampleRequest.SampleRequestID <= 0)
                {
                    throw new Exception("Invalid TradingSaleInvoice!");
                }
                if (oSampleRequest.ApprovedBy != 0)
                {
                    throw new Exception("Your Selected Purchase Invoice Already Approved!");
                }
                #endregion

     
                    #region Approved PurchaseInvocie
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingSaleInvoice, EnumRoleOperationType.Approved);
                    SampleRequestDA.Approved(tc, oSampleRequest, nUserID);
                    #endregion

                    #region Get TradingSaleInvoice
                    IDataReader reader;
                    reader = SampleRequestDA.Get(tc, oSampleRequest.SampleRequestID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSampleRequest = CreateObject(oReader);
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

                oSampleRequest = new SampleRequest();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oSampleRequest.ErrorMessage = Message;

                #endregion
            }
            return oSampleRequest;
        }

        public SampleRequest UndoApproved(SampleRequest oSampleRequest, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Approved PurchaseInvocie
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingSaleInvoice, EnumRoleOperationType.Approved);
                SampleRequestDA.UndoApproved(tc, oSampleRequest, nUserID);
                #endregion

                #region Get SampleRequest
                IDataReader reader;
                reader = SampleRequestDA.Get(tc, oSampleRequest.SampleRequestID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleRequest = CreateObject(oReader);
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

                oSampleRequest = new SampleRequest();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oSampleRequest.ErrorMessage = Message;

                #endregion
            }
            return oSampleRequest;
        }



        public SampleRequest Save(SampleRequest oSampleRequest, Int64 nUserID)
        {
            SampleRequestDetail oSampleRequestDetail = new SampleRequestDetail();
            SampleRequest oUG = new SampleRequest();
            oUG = oSampleRequest;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region SampleRequest
                IDataReader reader;
                if (oSampleRequest.SampleRequestID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SampleRequest, EnumRoleOperationType.Add);
                    reader = SampleRequestDA.InsertUpdate(tc, oSampleRequest, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SampleRequest, EnumRoleOperationType.Edit);
                    reader = SampleRequestDA.InsertUpdate(tc, oSampleRequest, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleRequest = new SampleRequest();
                    oSampleRequest = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region SampleRequestDetail

                if (oSampleRequest.SampleRequestID > 0)
                {
                    string sSampleRequestDetailIDs = "";
                    if (oUG.SampleRequestDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (SampleRequestDetail oDRD in oUG.SampleRequestDetails)
                        {
                            oDRD.SampleRequestID = oSampleRequest.SampleRequestID;
                            if (oDRD.SampleRequestDetailID <= 0)
                            {
                                readerdetail = SampleRequestDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = SampleRequestDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nSampleRequestDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nSampleRequestDetailID = oReaderDevRecapdetail.GetInt32("SampleRequestDetailID");
                                sSampleRequestDetailIDs = sSampleRequestDetailIDs + oReaderDevRecapdetail.GetString("SampleRequestDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sSampleRequestDetailIDs.Length > 0)
                    {
                        sSampleRequestDetailIDs = sSampleRequestDetailIDs.Remove(sSampleRequestDetailIDs.Length - 1, 1);
                    }
                    oSampleRequestDetail = new SampleRequestDetail();
                    oSampleRequestDetail.SampleRequestID = oSampleRequest.SampleRequestID;
                    SampleRequestDetailDA.Delete(tc, oSampleRequestDetail, EnumDBOperation.Delete, nUserID, sSampleRequestDetailIDs);
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
                    oSampleRequest = new SampleRequest();
                    oSampleRequest.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oSampleRequest;
        }

        public SampleRequest Commit(SampleRequest oSampleRequest, Int64 nUserID)
        {
            SampleRequestDetail oSampleRequestDetail = new SampleRequestDetail();
            SampleRequest oUG = new SampleRequest();
            oUG = oSampleRequest;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region SampleRequest
                IDataReader reader;
                reader = SampleRequestDA.InsertUpdate(tc, oSampleRequest, EnumDBOperation.Disburse, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleRequest = new SampleRequest();
                    oSampleRequest = CreateObject(oReader);
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
                    oSampleRequest = new SampleRequest();
                    oSampleRequest.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oSampleRequest;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SampleRequest oSampleRequest = new SampleRequest();
                oSampleRequest.SampleRequestID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.SampleRequest, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "SampleRequest", id);
                SampleRequestDA.Delete(tc, oSampleRequest, EnumDBOperation.Delete, nUserId);
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

        public SampleRequest Get(int id, Int64 nUserId)
        {
            SampleRequest oSampleRequest = new SampleRequest();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SampleRequestDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleRequest = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SampleRequest", e);
                #endregion
            }
            return oSampleRequest;
        }

        public List<SampleRequest> Gets(Int64 nUserID)
        {
            List<SampleRequest> oSampleRequests = new List<SampleRequest>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SampleRequestDA.Gets( tc);
                oSampleRequests = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                SampleRequest oSampleRequest = new SampleRequest();
                oSampleRequest.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oSampleRequests;
        }

        public List<SampleRequest> Gets(string sSQL, Int64 nUserID)
        {
            List<SampleRequest> oSampleRequests = new List<SampleRequest>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SampleRequestDA.Gets(tc, sSQL);
                oSampleRequests = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleRequest", e);
                #endregion
            }
            return oSampleRequests;
        }

        #endregion
    }

}

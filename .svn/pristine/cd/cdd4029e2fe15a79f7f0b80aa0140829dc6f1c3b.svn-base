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
    public class FNReProRequestService : MarshalByRefObject, IFNReProRequestService
    {
        #region Private functions and declaration

        private FNReProRequest MapObject(NullHandler oReader)
        {
            FNReProRequest oFNReProRequest = new FNReProRequest();
            oFNReProRequest.FNReProRequestID = oReader.GetInt32("FNReProRequestID");
            oFNReProRequest.ReqNo = oReader.GetString("ReqNo");
            oFNReProRequest.RequestByID = oReader.GetInt32("RequestByID");
            oFNReProRequest.RequestDate = oReader.GetDateTime("RequestDate");
            oFNReProRequest.ApproveBy = oReader.GetInt32("ApproveBy");
            oFNReProRequest.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFNReProRequest.Status = (EnumFNReProRequestStatus)oReader.GetInt32("Status");
            oFNReProRequest.Note = oReader.GetString("Note");
            oFNReProRequest.Note_Approve = oReader.GetString("Note_Approve");
            oFNReProRequest.RequestByName = oReader.GetString("RequestByName");
            oFNReProRequest.ApproveByName = oReader.GetString("ApproveByName");

            return oFNReProRequest;
        }

        private FNReProRequest CreateObject(NullHandler oReader)
        {
            FNReProRequest oFNReProRequest = new FNReProRequest();
            oFNReProRequest = MapObject(oReader);
            return oFNReProRequest;
        }

        private List<FNReProRequest> CreateObjects(IDataReader oReader)
        {
            List<FNReProRequest> oFNReProRequest = new List<FNReProRequest>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNReProRequest oItem = CreateObject(oHandler);
                oFNReProRequest.Add(oItem);
            }
            return oFNReProRequest;
        }

        #endregion

        #region Interface implementation
        public FNReProRequest Save(FNReProRequest oFNReProRequest, Int64 nUserID)
        {
            FNReProRequestDetail oFNReProRequestDetail = new FNReProRequestDetail();
            FNReProRequest oUG = new FNReProRequest();
            oUG = oFNReProRequest;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region FNReProRequest
                IDataReader reader;
                if (oFNReProRequest.FNReProRequestID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FNReProRequest, EnumRoleOperationType.Add);
                    reader = FNReProRequestDA.InsertUpdate(tc, oFNReProRequest, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FNReProRequest, EnumRoleOperationType.Edit);
                    reader = FNReProRequestDA.InsertUpdate(tc, oFNReProRequest, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNReProRequest = new FNReProRequest();
                    oFNReProRequest = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region FNReProRequestDetail

                if (oFNReProRequest.FNReProRequestID > 0)
                {
                    string sFNReProRequestDetailIDs = "";
                    if (oUG.FNReProRequestDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (FNReProRequestDetail oDRD in oUG.FNReProRequestDetails)
                        {
                            oDRD.FNReProRequestID = oFNReProRequest.FNReProRequestID;
                            if (oDRD.FNReProRequestDetailID <= 0)
                            {
                                readerdetail = FNReProRequestDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = FNReProRequestDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nFNReProRequestDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nFNReProRequestDetailID = oReaderDevRecapdetail.GetInt32("FNReProRequestDetailID");
                                sFNReProRequestDetailIDs = sFNReProRequestDetailIDs + oReaderDevRecapdetail.GetString("FNReProRequestDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sFNReProRequestDetailIDs.Length > 0)
                    {
                        sFNReProRequestDetailIDs = sFNReProRequestDetailIDs.Remove(sFNReProRequestDetailIDs.Length - 1, 1);
                    }
                    oFNReProRequestDetail = new FNReProRequestDetail();
                    oFNReProRequestDetail.FNReProRequestID = oFNReProRequest.FNReProRequestID;
                    FNReProRequestDetailDA.Delete(tc, oFNReProRequestDetail, EnumDBOperation.Delete, nUserID, sFNReProRequestDetailIDs);
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
                    oFNReProRequest = new FNReProRequest();
                    oFNReProRequest.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFNReProRequest;
        }

        public FNReProRequest Approve(FNReProRequest oFNReProRequest, Int64 nUserID)
        {
            FNReProRequestDetail oFNReProRequestDetail = new FNReProRequestDetail();
            FNReProRequest oUG = new FNReProRequest();
            oUG = oFNReProRequest;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region FNReProRequest
                IDataReader reader;
                reader = FNReProRequestDA.InsertUpdate(tc, oFNReProRequest, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNReProRequest = new FNReProRequest();
                    oFNReProRequest = CreateObject(oReader);
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
                    oFNReProRequest = new FNReProRequest();
                    oFNReProRequest.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFNReProRequest;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNReProRequest oFNReProRequest = new FNReProRequest();
                oFNReProRequest.FNReProRequestID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.FNReProRequest, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FNReProRequest", id);
                FNReProRequestDA.Delete(tc, oFNReProRequest, EnumDBOperation.Delete, nUserId);
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

        public FNReProRequest Get(int id, Int64 nUserId)
        {
            FNReProRequest oFNReProRequest = new FNReProRequest();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FNReProRequestDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNReProRequest = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FNReProRequest", e);
                #endregion
            }
            return oFNReProRequest;
        }

        public List<FNReProRequest> Gets(Int64 nUserID)
        {
            List<FNReProRequest> oFNReProRequests = new List<FNReProRequest>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNReProRequestDA.Gets(tc);
                oFNReProRequests = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FNReProRequest oFNReProRequest = new FNReProRequest();
                oFNReProRequest.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFNReProRequests;
        }

        public List<FNReProRequest> Gets(string sSQL, Int64 nUserID)
        {
            List<FNReProRequest> oFNReProRequests = new List<FNReProRequest>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNReProRequestDA.Gets(tc, sSQL);
                oFNReProRequests = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNReProRequest", e);
                #endregion
            }
            return oFNReProRequests;
        }

        #endregion
    }

}

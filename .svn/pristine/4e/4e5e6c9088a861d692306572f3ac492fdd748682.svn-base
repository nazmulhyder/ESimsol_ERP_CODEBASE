using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class ExportLCAmendmentRequestService : MarshalByRefObject, IExportLCAmendmentRequestService
    {
        #region Private functions and declaration
        private ExportLCAmendmentRequest MapObject(NullHandler oReader)
        {
            ExportLCAmendmentRequest oExportLCAmendmentRequest = new ExportLCAmendmentRequest();
            oExportLCAmendmentRequest.ExportLCAmendmentRequestID = oReader.GetInt32("ExportLCAmendmentRequestID");
            oExportLCAmendmentRequest.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportLCAmendmentRequest.DateOfRequest = oReader.GetDateTime("DateOfRequest");
            oExportLCAmendmentRequest.DateOfReceive = oReader.GetDateTime("DateOfReceive");
            oExportLCAmendmentRequest.Sequence = oReader.GetInt32("Sequence");
            oExportLCAmendmentRequest.RequestByID = oReader.GetInt32("RequestByID");
            oExportLCAmendmentRequest.ReceivedByID = oReader.GetInt32("ReceivedByID");
            return oExportLCAmendmentRequest;
        }

        private ExportLCAmendmentRequest CreateObject(NullHandler oReader)
        {
            ExportLCAmendmentRequest oExportLCAmendmentRequest = new ExportLCAmendmentRequest();
            oExportLCAmendmentRequest=MapObject(oReader);
            return oExportLCAmendmentRequest;
        }

        private List<ExportLCAmendmentRequest> CreateObjects(IDataReader oReader)
        {
            List<ExportLCAmendmentRequest> oExportLCAmendmentRequests = new List<ExportLCAmendmentRequest>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportLCAmendmentRequest oItem = CreateObject(oHandler);
                oExportLCAmendmentRequests.Add(oItem);
            }
            return oExportLCAmendmentRequests;
        }
        #endregion

        #region Interface implementation
        public ExportLCAmendmentRequestService() { }

        public ExportLCAmendmentRequest Get(int nID, Int64 nUserID)
        {
            ExportLCAmendmentRequest oPLC = new ExportLCAmendmentRequest();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportLCAmendmentRequestDA.Get(tc, nID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseLC", e);
                #endregion
            }

            return oPLC;
        }
        
        public ExportLCAmendmentRequest Save(ExportLCAmendmentRequest oExportLCAmendmentRequest, Int64 nUserID)
        {
            string sExportLCAmendmentClauseID = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                List<ExportLCAmendmentClause> oExportLCAmendmentClauses = new List<ExportLCAmendmentClause>();
                ExportLCAmendmentClause oExportLCAmendmentClause = new ExportLCAmendmentClause();
                oExportLCAmendmentClauses = oExportLCAmendmentRequest.ExportLCAmendmentClauses;

                IDataReader reader;
                if (oExportLCAmendmentRequest.ExportLCAmendmentRequestID <= 0)
                {
                    reader = ExportLCAmendmentRequestDA.InsertUpdate(tc, oExportLCAmendmentRequest, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportLCAmendmentRequestDA.InsertUpdate(tc, oExportLCAmendmentRequest, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLCAmendmentRequest = new ExportLCAmendmentRequest();
                    oExportLCAmendmentRequest = CreateObject(oReader);
                }
                reader.Close();

                #region ExportLCAmendmentClause
              
                foreach (ExportLCAmendmentClause oItem in oExportLCAmendmentClauses)
                {
                    IDataReader readerdetail;
                    oItem.ExportLCAmendRequestID = oExportLCAmendmentRequest.ExportLCAmendmentRequestID;
                    if (oItem.ExportLCAmendmentClauseID <= 0)
                    {
                        readerdetail = ExportLCAmendmentClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                    }
                    else
                    {
                        readerdetail = ExportLCAmendmentClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sExportLCAmendmentClauseID = sExportLCAmendmentClauseID + oReaderDetail.GetString("ExportLCAmendmentClauseID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sExportLCAmendmentClauseID.Length > 0)
                {
                    sExportLCAmendmentClauseID = sExportLCAmendmentClauseID.Remove(sExportLCAmendmentClauseID.Length - 1, 1);
                }
                oExportLCAmendmentClause = new ExportLCAmendmentClause();
                oExportLCAmendmentClause.ExportLCAmendRequestID = oExportLCAmendmentRequest.ExportLCAmendmentRequestID;
                ExportLCAmendmentClauseDA.Delete(tc, oExportLCAmendmentClause, EnumDBOperation.Delete, nUserID, sExportLCAmendmentClauseID);
                #endregion


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportLCAmendmentRequest.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oExportLCAmendmentRequest;

        }

        public List<ExportLCAmendmentRequest> Gets(int nExportLCID, Int64 nUserId)
        {
            List<ExportLCAmendmentRequest> oExportLCAmendmentRequests = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportLCAmendmentRequestDA.Gets(tc, nExportLCID);
                oExportLCAmendmentRequests = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportLCAmendmentRequests", e);
                #endregion
            }

            return oExportLCAmendmentRequests;
        }

        public string Delete(ExportLCAmendmentRequest oExportLCAmendmentRequest, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);


                ExportLCAmendmentRequestDA.Delete(tc, oExportLCAmendmentRequest, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        #endregion
    }


}

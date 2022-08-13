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
    public class ReceivedChequeAttachmentService : MarshalByRefObject, IReceivedChequeAttachmentService
    {
        #region Private functions and declaration
        private ReceivedChequeAttachment MapObject(NullHandler oReader)
        {
            ReceivedChequeAttachment oReceivedChequeAttachment = new ReceivedChequeAttachment();
            oReceivedChequeAttachment.ReceivedChequeAttachmentID = oReader.GetInt32("ReceivedChequeAttachmentID");
            oReceivedChequeAttachment.ReceivedChequeID = oReader.GetInt32("ReceivedChequeID");
            oReceivedChequeAttachment.AttatchmentName = oReader.GetString("AttatchmentName");
            oReceivedChequeAttachment.AttatchFile = oReader.GetBytes("AttatchFile");
            oReceivedChequeAttachment.FileType = oReader.GetString("FileType");
            oReceivedChequeAttachment.Remarks = oReader.GetString("Remarks");
            
            return oReceivedChequeAttachment;
        }

        private ReceivedChequeAttachment CreateObject(NullHandler oReader)
        {
            ReceivedChequeAttachment oReceivedChequeAttachment = new ReceivedChequeAttachment();
            oReceivedChequeAttachment = MapObject(oReader);
            return oReceivedChequeAttachment;
        }

        private List<ReceivedChequeAttachment> CreateObjects(IDataReader oReader)
        {
            List<ReceivedChequeAttachment> oReceivedChequeAttachments = new List<ReceivedChequeAttachment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ReceivedChequeAttachment oItem = CreateObject(oHandler);
                oReceivedChequeAttachments.Add(oItem);
            }
            return oReceivedChequeAttachments;
        }

        #endregion

        #region Interface implementation
        public ReceivedChequeAttachmentService() { }

        public ReceivedChequeAttachment Save(ReceivedChequeAttachment oReceivedChequeAttachment, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oReceivedChequeAttachment.ReceivedChequeAttachmentID <= 0)
                {
                    oReceivedChequeAttachment.ReceivedChequeAttachmentID = ReceivedChequeAttachmentDA.GetNewID(tc);
                    ReceivedChequeAttachmentDA.Insert(tc, oReceivedChequeAttachment, nUserId);
                }
                else
                {
                    ReceivedChequeAttachmentDA.Update(tc, oReceivedChequeAttachment, nUserId);
                }
                tc.End();
                BusinessObject.Factory.SetObjectState(oReceivedChequeAttachment, ObjectState.Saved);
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save AttachedFile", e);
                #endregion
            }
            return oReceivedChequeAttachment;
        }

       

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ReceivedChequeAttachment oReceivedChequeAttachment = new ReceivedChequeAttachment();
                oReceivedChequeAttachment.ReceivedChequeAttachmentID = id;
                ReceivedChequeAttachmentDA.Delete(tc, id);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                #endregion
                return e.Message;
            }
            return "Data Delete Successfully";
        }


        public List<ReceivedChequeAttachment> Gets(int id, Int64 nUserId)
        {
            List<ReceivedChequeAttachment> oReceivedChequeAttachments = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ReceivedChequeAttachmentDA.Gets(tc, id);
                oReceivedChequeAttachments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oReceivedChequeAttachments;
        }

        public ReceivedChequeAttachment Get(int id, Int64 nUserId)
        {
            ReceivedChequeAttachment oReceivedChequeAttachment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ReceivedChequeAttachmentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oReceivedChequeAttachment = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oReceivedChequeAttachment;
        }

        public ReceivedChequeAttachment GetWithAttachFile(int id, Int64 nUserId)
        {
            ReceivedChequeAttachment oReceivedChequeAttachment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ReceivedChequeAttachmentDA.GetWithAttachFile(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oReceivedChequeAttachment = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oReceivedChequeAttachment;
        }

        #endregion
    }
}

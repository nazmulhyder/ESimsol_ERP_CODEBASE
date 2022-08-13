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
    public class PurchaseQuotationAttachmentService : MarshalByRefObject, IPurchaseQuotationAttachmentService
    {
        #region Private functions and declaration
        private PurchaseQuotationAttachment MapObject(NullHandler oReader)
        {
            PurchaseQuotationAttachment oPurchaseQuotationAttachment = new PurchaseQuotationAttachment();
            oPurchaseQuotationAttachment.PurchaseQuotationAttachmentID = oReader.GetInt32("PurchaseQuotationAttachmentID");
            oPurchaseQuotationAttachment.PurchaseQuotationID = oReader.GetInt32("PurchaseQuotationID");
            oPurchaseQuotationAttachment.AttatchmentName = oReader.GetString("AttatchmentName");
            oPurchaseQuotationAttachment.AttatchFile = oReader.GetBytes("AttatchFile");
            oPurchaseQuotationAttachment.FileType = oReader.GetString("FileType");
            oPurchaseQuotationAttachment.Remarks = oReader.GetString("Remarks");
            
            return oPurchaseQuotationAttachment;
        }

        private PurchaseQuotationAttachment CreateObject(NullHandler oReader)
        {
            PurchaseQuotationAttachment oPurchaseQuotationAttachment = new PurchaseQuotationAttachment();
            oPurchaseQuotationAttachment = MapObject(oReader);
            return oPurchaseQuotationAttachment;
        }

        private List<PurchaseQuotationAttachment> CreateObjects(IDataReader oReader)
        {
            List<PurchaseQuotationAttachment> oPurchaseQuotationAttachments = new List<PurchaseQuotationAttachment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseQuotationAttachment oItem = CreateObject(oHandler);
                oPurchaseQuotationAttachments.Add(oItem);
            }
            return oPurchaseQuotationAttachments;
        }

        #endregion

        #region Interface implementation
        public PurchaseQuotationAttachmentService() { }

        public PurchaseQuotationAttachment Save(PurchaseQuotationAttachment oPurchaseQuotationAttachment, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oPurchaseQuotationAttachment.PurchaseQuotationAttachmentID <= 0)
                {
                    oPurchaseQuotationAttachment.PurchaseQuotationAttachmentID = PurchaseQuotationAttachmentDA.GetNewID(tc);
                    PurchaseQuotationAttachmentDA.Insert(tc, oPurchaseQuotationAttachment, nUserId);
                }
                else
                {
                    PurchaseQuotationAttachmentDA.Update(tc, oPurchaseQuotationAttachment, nUserId);
                }
                tc.End();
                BusinessObject.Factory.SetObjectState(oPurchaseQuotationAttachment, ObjectState.Saved);
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
            return oPurchaseQuotationAttachment;
        }

       

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PurchaseQuotationAttachment oPurchaseQuotationAttachment = new PurchaseQuotationAttachment();
                oPurchaseQuotationAttachment.PurchaseQuotationAttachmentID = id;
                PurchaseQuotationAttachmentDA.Delete(tc, id);
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


        public List<PurchaseQuotationAttachment> Gets(int id, Int64 nUserId)
        {
            List<PurchaseQuotationAttachment> oPurchaseQuotationAttachments = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseQuotationAttachmentDA.Gets(tc, id);
                oPurchaseQuotationAttachments = CreateObjects(reader);
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

            return oPurchaseQuotationAttachments;
        }

        public PurchaseQuotationAttachment Get(int id, Int64 nUserId)
        {
            PurchaseQuotationAttachment oPurchaseQuotationAttachment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseQuotationAttachmentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseQuotationAttachment = CreateObject(oReader);
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

            return oPurchaseQuotationAttachment;
        }

        public PurchaseQuotationAttachment GetWithAttachFile(int id, Int64 nUserId)
        {
            PurchaseQuotationAttachment oPurchaseQuotationAttachment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseQuotationAttachmentDA.GetWithAttachFile(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseQuotationAttachment = CreateObject(oReader);
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

            return oPurchaseQuotationAttachment;
        }

        #endregion
    }
}

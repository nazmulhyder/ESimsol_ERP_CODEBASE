using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class ExportLCAttachmentService : MarshalByRefObject, IExportLCAttachmentService
    {
        #region Private functions and declaration
        private ExportLCAttachment MapObject(NullHandler oReader)
        {
            ExportLCAttachment oExportLCAttachment = new ExportLCAttachment();
            oExportLCAttachment.ExportLCAttachmentID = oReader.GetInt32("ExportLCAttachmentID");
            oExportLCAttachment.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportLCAttachment.AttatchmentName = oReader.GetString("AttatchmentName");
            oExportLCAttachment.AttatchFile = oReader.GetBytes("AttatchFile");
            oExportLCAttachment.FileType = oReader.GetString("FileType");
            oExportLCAttachment.Remarks = oReader.GetString("Remarks");
            return oExportLCAttachment;
        }

        private ExportLCAttachment CreateObject(NullHandler oReader)
        {
            ExportLCAttachment oExportLCAttachment = new ExportLCAttachment();
            oExportLCAttachment = MapObject(oReader);
            return oExportLCAttachment;
        }

        private List<ExportLCAttachment> CreateObjects(IDataReader oReader)
        {
            List<ExportLCAttachment> oExportLCAttachments = new List<ExportLCAttachment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportLCAttachment oItem = CreateObject(oHandler);
                oExportLCAttachments.Add(oItem);
            }
            return oExportLCAttachments;
        }

        #endregion

        #region Interface implementation
        public ExportLCAttachment Save(ExportLCAttachment oExportLCAttachment, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oExportLCAttachment.ExportLCAttachmentID <= 0)
                {
                    oExportLCAttachment.ExportLCAttachmentID = ExportLCAttachmentDA.GetNewID(tc);
                    ExportLCAttachmentDA.Insert(tc, oExportLCAttachment, nUserId);
                }
                else
                {
                    ExportLCAttachmentDA.Update(tc, oExportLCAttachment, nUserId);
                }
                tc.End();
                //BusinessObject.Factory.SetObjectState(oExportLCAttachment, ObjectState.Saved);
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
            return oExportLCAttachment;
        }


        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportLCAttachment oExportLCAttachment = new ExportLCAttachment();
                ExportLCAttachmentDA.Delete(tc, id);
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
            return Global.DeleteMessage;
        }

        public List<ExportLCAttachment> Gets(Int64 nUserId)
        {
            List<ExportLCAttachment> oExportLCAttachments = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportLCAttachmentDA.Gets(tc);
                oExportLCAttachments = CreateObjects(reader);
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

            return oExportLCAttachments;
        }

        public List<ExportLCAttachment> GetsAttachmentById(int nExportLCID, Int64 nUserId)
        {
            List<ExportLCAttachment> oExportLCAttachments = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportLCAttachmentDA.GetsAttachmentById(tc, nExportLCID);
                oExportLCAttachments = CreateObjects(reader);
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

            return oExportLCAttachments;
        }

        public ExportLCAttachment Get(int id, Int64 nUserId)
        {
            ExportLCAttachment oExportLCAttachment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportLCAttachmentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLCAttachment = CreateObject(oReader);
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

            return oExportLCAttachment;
        }

        public ExportLCAttachment GetWithAttachFile(int id, Int64 nUserId)
        {
            ExportLCAttachment oExportLCAttachment = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportLCAttachmentDA.GetWithAttachFile(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLCAttachment = CreateObject(oReader);
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

            return oExportLCAttachment;
        }

        #endregion
    }
}

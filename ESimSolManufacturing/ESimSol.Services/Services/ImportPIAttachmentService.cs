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
    public class ImportPIAttachmentService : MarshalByRefObject, IImportPIAttachmentService
    {
        #region Private functions and declaration
        private ImportPIAttachment MapObject(NullHandler oReader)
        {
            ImportPIAttachment oImportPIAttachment = new ImportPIAttachment();
            oImportPIAttachment.ImportPIAttachmentID = oReader.GetInt32("ImportPIAttachmentID");
            oImportPIAttachment.ImportPIID = oReader.GetInt32("ImportPIID");
            oImportPIAttachment.AttatchmentName = oReader.GetString("AttatchmentName");
            oImportPIAttachment.AttatchFile = oReader.GetBytes("AttatchFile");
            oImportPIAttachment.FileType = oReader.GetString("FileType");
            oImportPIAttachment.Remarks = oReader.GetString("Remarks");
            
            return oImportPIAttachment;
        }

        private ImportPIAttachment CreateObject(NullHandler oReader)
        {
            ImportPIAttachment oImportPIAttachment = new ImportPIAttachment();
            oImportPIAttachment = MapObject(oReader);
            return oImportPIAttachment;
        }

        private List<ImportPIAttachment> CreateObjects(IDataReader oReader)
        {
            List<ImportPIAttachment> oImportPIAttachments = new List<ImportPIAttachment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPIAttachment oItem = CreateObject(oHandler);
                oImportPIAttachments.Add(oItem);
            }
            return oImportPIAttachments;
        }

        #endregion

        #region Interface implementation
        public ImportPIAttachmentService() { }

        public ImportPIAttachment Save(ImportPIAttachment oImportPIAttachment, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oImportPIAttachment.ImportPIAttachmentID <= 0)
                {
                    oImportPIAttachment.ImportPIAttachmentID = ImportPIAttachmentDA.GetNewID(tc);
                    ImportPIAttachmentDA.Insert(tc, oImportPIAttachment, nUserId);
                }
                else
                {
                    ImportPIAttachmentDA.Update(tc, oImportPIAttachment, nUserId);
                }
                tc.End();
                BusinessObject.Factory.SetObjectState(oImportPIAttachment, ObjectState.Saved);
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
            return oImportPIAttachment;
        }

       

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportPIAttachment oImportPIAttachment = new ImportPIAttachment();
                oImportPIAttachment.ImportPIAttachmentID = id;
                ImportPIAttachmentDA.Delete(tc, id);
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


        public List<ImportPIAttachment> Gets(int id, Int64 nUserId)
        {
            List<ImportPIAttachment> oImportPIAttachments = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIAttachmentDA.Gets(tc, id);
                oImportPIAttachments = CreateObjects(reader);
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

            return oImportPIAttachments;
        }

        public ImportPIAttachment Get(int id, Int64 nUserId)
        {
            ImportPIAttachment oImportPIAttachment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIAttachmentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPIAttachment = CreateObject(oReader);
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

            return oImportPIAttachment;
        }

        public ImportPIAttachment GetWithAttachFile(int id, Int64 nUserId)
        {
            ImportPIAttachment oImportPIAttachment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIAttachmentDA.GetWithAttachFile(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPIAttachment = CreateObject(oReader);
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

            return oImportPIAttachment;
        }

        #endregion
    }
}

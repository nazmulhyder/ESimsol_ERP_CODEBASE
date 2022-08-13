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
    public class CompanyDocumentService : MarshalByRefObject, ICompanyDocumentService
    {
        #region Private functions and declaration
        private CompanyDocument MapObject(NullHandler oReader)
        {
            CompanyDocument oCompanyDocument = new CompanyDocument();
            oCompanyDocument.CDID = oReader.GetInt32("CDID");
            oCompanyDocument.CompanyID = oReader.GetInt32("CompanyID");
            oCompanyDocument.Description = oReader.GetString("Description");
            oCompanyDocument.DocFile = oReader.GetBytes("DocFile");
            oCompanyDocument.FileType = oReader.GetString("FileType");
            oCompanyDocument.FileName = oReader.GetString("FileName");
            return oCompanyDocument;
        }

        private CompanyDocument CreateObject(NullHandler oReader)
        {
            CompanyDocument oCompanyDocument = new CompanyDocument();
            oCompanyDocument = MapObject(oReader);
            return oCompanyDocument;
        }

        private List<CompanyDocument> CreateObjects(IDataReader oReader)
        {
            List<CompanyDocument> oCompanyDocuments = new List<CompanyDocument>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CompanyDocument oItem = CreateObject(oHandler);
                oCompanyDocuments.Add(oItem);
            }
            return oCompanyDocuments;
        }

        #endregion

        #region Interface implementation
        public CompanyDocument Save(CompanyDocument oCompanyDocument, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oCompanyDocument.CDID <= 0)
                {
                    oCompanyDocument.CDID = CompanyDocumentDA.GetNewID(tc);
                    CompanyDocumentDA.Insert(tc, oCompanyDocument, nUserId);
                }
                else
                {
                    CompanyDocumentDA.Update(tc, oCompanyDocument, nUserId);
                }
                tc.End();
                BusinessObject.Factory.SetObjectState(oCompanyDocument, ObjectState.Saved);
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
            return oCompanyDocument;
        }


        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CompanyDocument oCompanyDocument = new CompanyDocument();
                CompanyDocumentDA.Delete(tc, id);
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

        public List<CompanyDocument> Gets(Int64 nUserId)
        {
            List<CompanyDocument> oCompanyDocuments = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CompanyDocumentDA.Gets(tc);
                oCompanyDocuments = CreateObjects(reader);
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

            return oCompanyDocuments;
        }

        public CompanyDocument Get(int id, Int64 nUserId)
        {
            CompanyDocument oCompanyDocument = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CompanyDocumentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompanyDocument = CreateObject(oReader);
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

            return oCompanyDocument;
        }

        public CompanyDocument GetWithAttachFile(int id, Int64 nUserId)
        {
            CompanyDocument oCompanyDocument = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CompanyDocumentDA.GetWithAttachFile(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompanyDocument = CreateObject(oReader);
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

            return oCompanyDocument;
        }

        public List<CompanyDocument> Gets(string sSql, Int64 nUserId)
        {
            List<CompanyDocument> oCompanyDocuments = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CompanyDocumentDA.Gets(sSql,tc);
                oCompanyDocuments = CreateObjects(reader);
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

            return oCompanyDocuments;
        }

        #endregion
    }
}

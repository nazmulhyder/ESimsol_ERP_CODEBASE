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
    public class EmployeeDocumentService : MarshalByRefObject, IEmployeeDocumentService
    {
        #region Private functions and declaration
        private EmployeeDocument MapObject(NullHandler oReader)
        {
            EmployeeDocument oEmployeeDocument = new EmployeeDocument();
            oEmployeeDocument.EDID = oReader.GetInt32("EDID");
            oEmployeeDocument.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeDocument.Description = oReader.GetString("Description");
            oEmployeeDocument.DocFile = oReader.GetBytes("DocFile");
            oEmployeeDocument.DocFileType = oReader.GetString("DocFileType");
            oEmployeeDocument.FileName = oReader.GetString("FileName");
            return oEmployeeDocument;
        }

        private EmployeeDocument CreateObject(NullHandler oReader)
        {
            EmployeeDocument oEmployeeDocument = new EmployeeDocument();
            oEmployeeDocument = MapObject(oReader);
            return oEmployeeDocument;
        }

        private List<EmployeeDocument> CreateObjects(IDataReader oReader)
        {
            List<EmployeeDocument> oEmployeeDocuments = new List<EmployeeDocument>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeDocument oItem = CreateObject(oHandler);
                oEmployeeDocuments.Add(oItem);
            }
            return oEmployeeDocuments;
        }

        #endregion

        #region Interface implementation
        public EmployeeDocument Save(EmployeeDocument oEmployeeDocument, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oEmployeeDocument.EDID <= 0)
                {
                    oEmployeeDocument.EDID = EmployeeDocumentDA.GetNewID(tc);
                    EmployeeDocumentDA.Insert(tc, oEmployeeDocument, nUserId);
                }
                else
                {
                    EmployeeDocumentDA.Update(tc, oEmployeeDocument, nUserId);
                }
                tc.End();
                BusinessObject.Factory.SetObjectState(oEmployeeDocument, ObjectState.Saved);
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
            return oEmployeeDocument;
        }


        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeDocument oEmployeeDocument = new EmployeeDocument();
                EmployeeDocumentDA.Delete(tc, id);
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

        public List<EmployeeDocument> Gets(Int64 nUserId)
        {
            List<EmployeeDocument> oEmployeeDocuments = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDocumentDA.Gets(tc);
                oEmployeeDocuments = CreateObjects(reader);
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

            return oEmployeeDocuments;
        }

        public EmployeeDocument Get(int id, Int64 nUserId)
        {
            EmployeeDocument oEmployeeDocument = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDocumentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeDocument = CreateObject(oReader);
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

            return oEmployeeDocument;
        }

        public EmployeeDocument GetWithAttachFile(int id, Int64 nUserId)
        {
            EmployeeDocument oEmployeeDocument = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDocumentDA.GetWithAttachFile(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeDocument = CreateObject(oReader);
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

            return oEmployeeDocument;
        }

        public List<EmployeeDocument> Gets(string sSql, Int64 nUserId)
        {
            List<EmployeeDocument> oEmployeeDocuments = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDocumentDA.Gets(sSql, tc);
                oEmployeeDocuments = CreateObjects(reader);
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

            return oEmployeeDocuments;
        }

        #endregion
    }
}


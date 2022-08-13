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
    public class EmployeeDocService : MarshalByRefObject, IEmployeeDocService
    {
        #region Private functions and declaration
        private EmployeeDoc MapObject(NullHandler oReader)
        {
            EmployeeDoc oEmployeeDoc = new EmployeeDoc();
            oEmployeeDoc.EmployeeDocID = oReader.GetInt32("EmployeeDocID");
            oEmployeeDoc.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeDoc.DocType = (EnumEmployeeDoc)oReader.GetInt16("DocType");
            oEmployeeDoc.DocTypeInt = oReader.GetInt16("DocType");
            oEmployeeDoc.DocTypeID = oReader.GetInt32("DocTypeID");
            oEmployeeDoc.AttachmentFile = oReader.GetBytes("AttachmentFile");
            oEmployeeDoc.FileType = oReader.GetString("FileType");
            oEmployeeDoc.Description = oReader.GetString("Description");
            oEmployeeDoc.IssueDate = oReader.GetDateTime("IssueDate");
            oEmployeeDoc.ExpireDate = oReader.GetDateTime("ExpireDate");
            return oEmployeeDoc;
        }

        private EmployeeDoc CreateObject(NullHandler oReader)
        {
            EmployeeDoc oEmployeeDoc = new EmployeeDoc();
            oEmployeeDoc = MapObject(oReader);
            return oEmployeeDoc;
        }

        private List<EmployeeDoc> CreateObjects(IDataReader oReader)
        {
            List<EmployeeDoc> oEmployeeDocs = new List<EmployeeDoc>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeDoc oItem = CreateObject(oHandler);
                oEmployeeDocs.Add(oItem);
            }
            return oEmployeeDocs;
        }

        #endregion

        #region Interface implementation
        public EmployeeDocService() { }

        public EmployeeDoc Save(EmployeeDoc oEmployeeDoc, Int64 nUserId)
        {
            TransactionContext tc = null;
            EmployeeDoc oTempCom = new EmployeeDoc();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oEmployeeDoc.EmployeeDocID <= 0)
                {
                    oEmployeeDoc.EmployeeDocID = EmployeeDocDA.GetNewID(tc);
                    EmployeeDocDA.InsertUpdate(tc, oEmployeeDoc, EnumDBOperation.Insert, nUserId);
                    reader = EmployeeDocDA.Get(tc, oEmployeeDoc.EmployeeDocID);
                }
                else
                {
                    EmployeeDocDA.InsertUpdate(tc, oEmployeeDoc, EnumDBOperation.Update, nUserId);
                    reader = EmployeeDocDA.Get(tc, oEmployeeDoc.EmployeeDocID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeDoc = new EmployeeDoc();
                    oEmployeeDoc = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save Attachment. Because of " + e.Message, e);
                #endregion
            }
            return oEmployeeDoc;
        }
        public string Delete(string sIDs, Int64 nUserId)
        {
            TransactionContext tc = null;
            string sMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeDocDA.Delete(tc, sIDs, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                sMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete OrganizationalInformation. Because of " + e.Message, e);
                #endregion
            }
            return sMessage;
        }

        public EmployeeDoc Get(int id, Int64 nUserId)
        {
            EmployeeDoc oEmployeeDoc = new EmployeeDoc();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeDocDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeDoc = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get OrganizationalInformation", e);
                #endregion
            }

            return oEmployeeDoc;
        }

        public List<EmployeeDoc> Gets(int nEmployeeID, Int64 nUserId)
        {
            List<EmployeeDoc> oEmployeeDocs = new List<EmployeeDoc>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDocDA.Gets(tc, nEmployeeID);
                oEmployeeDocs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Attachments", e);
                #endregion
            }

            return oEmployeeDocs;
        }

        public List<EmployeeDoc> Gets(string sSQL, Int64 nUserId)
        {
            List<EmployeeDoc> oEmployeeDocs = new List<EmployeeDoc>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDocDA.Gets(tc, sSQL);
                oEmployeeDocs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Attachments", e);
                #endregion
            }

            return oEmployeeDocs;
        }
        #endregion
    }
}


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
    public class CompanyDocService : MarshalByRefObject, ICompanyDocService
    {
        #region Private functions and declaration
        private CompanyDoc MapObject(NullHandler oReader)
        {
            CompanyDoc oCompanyDoc = new CompanyDoc();            
            oCompanyDoc.CompanyDocID = oReader.GetInt32("CompanyDocID");
            oCompanyDoc.CompanyID = oReader.GetInt32("CompanyID");
            oCompanyDoc.Description = oReader.GetString("Description");
            oCompanyDoc.IssueDate = oReader.GetDateTime("IssueDate"); 
            oCompanyDoc.ExpireDate = oReader.GetDateTime("ExpireDate");
            oCompanyDoc.AttachmentFile = oReader.GetBytes("AttachmentFile");
            oCompanyDoc.FileType = oReader.GetString("FileType");
            oCompanyDoc.IsActive = oReader.GetBoolean("IsActive");
            return oCompanyDoc;                   
        }

        private CompanyDoc CreateObject(NullHandler oReader)
        {
            CompanyDoc oCompanyDoc = new CompanyDoc();
            oCompanyDoc= MapObject(oReader);
            return oCompanyDoc;
        }

        private List<CompanyDoc> CreateObjects(IDataReader oReader)
        {
            List<CompanyDoc> oCompanyDocs = new List<CompanyDoc>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CompanyDoc oItem = CreateObject(oHandler);
                oCompanyDocs.Add(oItem);
            }
            return oCompanyDocs;
        }     

        #endregion

        #region Interface implementation
        public CompanyDocService() { }

        public CompanyDoc Save(CompanyDoc oCompanyDoc, Int64 nUserId)
        {
            TransactionContext tc = null;
            CompanyDoc oTempCom = new CompanyDoc();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCompanyDoc.CompanyDocID <= 0)
                {
                    oCompanyDoc.CompanyDocID = CompanyDocDA.GetNewID(tc);
                    CompanyDocDA.InsertUpdate(tc, oCompanyDoc, EnumDBOperation.Insert, nUserId);
                    reader = CompanyDocDA.Get(tc, oCompanyDoc.CompanyDocID);
                }
                else
                {
                    CompanyDocDA.InsertUpdate(tc, oCompanyDoc, EnumDBOperation.Update, nUserId);
                    reader = CompanyDocDA.Get(tc, oCompanyDoc.CompanyDocID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompanyDoc = new CompanyDoc();
                    oCompanyDoc = CreateObject(oReader);
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
            return oCompanyDoc;
        }
        public string Delete(string sIDs, Int64 nUserId)
        {
            TransactionContext tc = null;
            string sMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                CompanyDocDA.Delete(tc, sIDs, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                sMessage=e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete OrganizationalInformation. Because of " + e.Message, e);
                #endregion
            }
            return sMessage;
        }

        public CompanyDoc Get(int id, Int64 nUserId)
        {
            CompanyDoc oCompanyDoc = new CompanyDoc();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CompanyDocDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompanyDoc = CreateObject(oReader);
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

            return oCompanyDoc;
        }

        public List<CompanyDoc> Gets(int nCompanyID, Int64 nUserId)
        {
            List<CompanyDoc> oCompanyDocs = new List<CompanyDoc>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CompanyDocDA.Gets(tc, nCompanyID);
                oCompanyDocs = CreateObjects(reader);
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

            return oCompanyDocs;
        }

        public List<CompanyDoc> Gets(string sSQL,Int64 nUserId)
        {
            List<CompanyDoc> oCompanyDocs = new List<CompanyDoc>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CompanyDocDA.Gets(tc, sSQL);
                oCompanyDocs = CreateObjects(reader);
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

            return oCompanyDocs;
        }
        #endregion
    }
}


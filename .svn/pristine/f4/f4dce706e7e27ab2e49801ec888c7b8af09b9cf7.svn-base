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
    public class ProformaInvoiceRequiredDocService : MarshalByRefObject, IProformaInvoiceRequiredDocService
    {
        #region Private functions and declaration
        private ProformaInvoiceRequiredDoc MapObject(NullHandler oReader)
        {
            ProformaInvoiceRequiredDoc oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();

            oProformaInvoiceRequiredDoc.ProformaInvoiceRequiredDocID = oReader.GetInt32("ProformaInvoiceRequiredDocID");
            oProformaInvoiceRequiredDoc.ProformaInvoiceID = oReader.GetInt32("ProformaInvoiceID");
            oProformaInvoiceRequiredDoc.ProformaInvoiceRequiredDocLogID = oReader.GetInt32("ProformaInvoiceRequiredDocLogID");
            oProformaInvoiceRequiredDoc.ProformaInvoiceLogID = oReader.GetInt32("ProformaInvoiceLogID");
            oProformaInvoiceRequiredDoc.DocType = (EnumDocumentType)oReader.GetInt32("DocType");
            oProformaInvoiceRequiredDoc.DocTypeInInt = oReader.GetInt32("DocType");
            return oProformaInvoiceRequiredDoc;
        }

        private ProformaInvoiceRequiredDoc CreateObject(NullHandler oReader)
        {
            ProformaInvoiceRequiredDoc oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();
            oProformaInvoiceRequiredDoc = MapObject(oReader);
            return oProformaInvoiceRequiredDoc;
        }

        private List<ProformaInvoiceRequiredDoc> CreateObjects(IDataReader oReader)
        {
            List<ProformaInvoiceRequiredDoc> oProformaInvoiceRequiredDoc = new List<ProformaInvoiceRequiredDoc>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProformaInvoiceRequiredDoc oItem = CreateObject(oHandler);
                oProformaInvoiceRequiredDoc.Add(oItem);
            }
            return oProformaInvoiceRequiredDoc;
        }

        #endregion

        #region Interface implementation
        public ProformaInvoiceRequiredDocService() { }

        public ProformaInvoiceRequiredDoc Save(ProformaInvoiceRequiredDoc oProformaInvoiceRequiredDoc, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProformaInvoiceRequiredDoc.ProformaInvoiceRequiredDocID <= 0)
                {
                    reader = ProformaInvoiceRequiredDocDA.InsertUpdate(tc, oProformaInvoiceRequiredDoc, EnumDBOperation.Insert,"", nUserId);
                }
                else
                {
                    reader = ProformaInvoiceRequiredDocDA.InsertUpdate(tc, oProformaInvoiceRequiredDoc, EnumDBOperation.Update,"", nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();
                    oProformaInvoiceRequiredDoc = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProformaInvoiceRequiredDoc.ErrorMessage = e.Message;
                #endregion
            }
            return oProformaInvoiceRequiredDoc;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProformaInvoiceRequiredDoc oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();
                oProformaInvoiceRequiredDoc.ProformaInvoiceRequiredDocID = id;
                ProformaInvoiceRequiredDocDA.Delete(tc, oProformaInvoiceRequiredDoc, EnumDBOperation.Delete, "",nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }

        public string ProformaInvoiceRequiredDocSave(List<ProformaInvoiceRequiredDoc> oProformaInvoiceRequiredDocs, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<ProformaInvoiceRequiredDoc> _oProformaInvoiceRequiredDocs = new List<ProformaInvoiceRequiredDoc>();
            ProformaInvoiceRequiredDoc oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();
            oProformaInvoiceRequiredDoc.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (ProformaInvoiceRequiredDoc oItem in oProformaInvoiceRequiredDocs)
                {
                    IDataReader reader;
                    reader = ProformaInvoiceRequiredDocDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert,"", nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();
                        oProformaInvoiceRequiredDoc = CreateObject(oReader);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProformaInvoiceRequiredDoc.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ProformaInvoiceRequiredDoc. Because of " + e.Message, e);
                #endregion
            }
            return "Save Successfully !";
        }

        public ProformaInvoiceRequiredDoc Get(int id, Int64 nUserId)
        {
            ProformaInvoiceRequiredDoc oAccountHead = new ProformaInvoiceRequiredDoc();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProformaInvoiceRequiredDocDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ProformaInvoiceRequiredDoc", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ProformaInvoiceRequiredDoc> Gets(int id, Int64 nUserID)
        {
            List<ProformaInvoiceRequiredDoc> oProformaInvoiceRequiredDoc = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceRequiredDocDA.Gets(tc,id);
                oProformaInvoiceRequiredDoc = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoiceRequiredDoc", e);
                #endregion
            }

            return oProformaInvoiceRequiredDoc;
        }

        public List<ProformaInvoiceRequiredDoc> GetsPILog(int id, Int64 nUserID)
        {
            List<ProformaInvoiceRequiredDoc> oProformaInvoiceRequiredDoc = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceRequiredDocDA.GetsPILog(tc, id);
                oProformaInvoiceRequiredDoc = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoiceRequiredDoc", e);
                #endregion
            }

            return oProformaInvoiceRequiredDoc;
        }
        

        public List<ProformaInvoiceRequiredDoc> Gets(string sSQL, Int64 nUserID)
        {
            List<ProformaInvoiceRequiredDoc> oProformaInvoiceRequiredDoc = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceRequiredDocDA.Gets(tc, sSQL);
                oProformaInvoiceRequiredDoc = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoiceRequiredDoc", e);
                #endregion
            }

            return oProformaInvoiceRequiredDoc;
        }
        #endregion
    }
}

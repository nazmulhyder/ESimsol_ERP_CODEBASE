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
    public class CommercialInvoiceDocService : MarshalByRefObject, ICommercialInvoiceDocService
    {
        #region Private functions and declaration
        private CommercialInvoiceDoc MapObject(NullHandler oReader)
        {
            CommercialInvoiceDoc oCommercialInvoiceDoc = new CommercialInvoiceDoc();
            oCommercialInvoiceDoc.CommercialInvoiceDocID = oReader.GetInt32("CommercialInvoiceDocID");
            oCommercialInvoiceDoc.CommercialInvoiceID = oReader.GetInt32("CommercialInvoiceID");
            oCommercialInvoiceDoc.DocType = (EnumDocumentType)oReader.GetInt32("DocType");
            oCommercialInvoiceDoc.DocTypeInInt = oReader.GetInt32("DocType");
            oCommercialInvoiceDoc.DocName = oReader.GetString("DocName");
            oCommercialInvoiceDoc.Note = oReader.GetString("Note");
            oCommercialInvoiceDoc.DocFile = oReader.GetBytes("DocFile");
            oCommercialInvoiceDoc.FileType = oReader.GetString("FileType");
            return oCommercialInvoiceDoc;
        }

        private CommercialInvoiceDoc CreateObject(NullHandler oReader)
        {
            CommercialInvoiceDoc oCommercialInvoiceDoc = new CommercialInvoiceDoc();
            oCommercialInvoiceDoc = MapObject(oReader);
            return oCommercialInvoiceDoc;
        }

        private List<CommercialInvoiceDoc> CreateObjects(IDataReader oReader)
        {
            List<CommercialInvoiceDoc> oCommercialInvoiceDocs = new List<CommercialInvoiceDoc>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CommercialInvoiceDoc oItem = CreateObject(oHandler);
                oCommercialInvoiceDocs.Add(oItem);
            }
            return oCommercialInvoiceDocs;
        }

        #endregion

        #region Interface implementation
        public CommercialInvoiceDocService() { }

        public CommercialInvoiceDoc Save(CommercialInvoiceDoc oCommercialInvoiceDoc, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oCommercialInvoiceDoc.CommercialInvoiceDocID <= 0)
                {
                    oCommercialInvoiceDoc.CommercialInvoiceDocID = CommercialInvoiceDocDA.GetNewID(tc);
                    CommercialInvoiceDocDA.Insert(tc, oCommercialInvoiceDoc, nUserId);
                }
                else
                {
                    CommercialInvoiceDocDA.Update(tc, oCommercialInvoiceDoc, nUserId);
                }
                tc.End();
                BusinessObject.Factory.SetObjectState(oCommercialInvoiceDoc, ObjectState.Saved);
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
            return oCommercialInvoiceDoc;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CommercialInvoiceDoc oCommercialInvoiceDoc = new CommercialInvoiceDoc();
                oCommercialInvoiceDoc.CommercialInvoiceDocID = id;
                CommercialInvoiceDocDA.Delete(tc, id);
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


        public List<CommercialInvoiceDoc> Gets(int id, Int64 nUserId)
        {
            List<CommercialInvoiceDoc> oCommercialInvoiceDocs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommercialInvoiceDocDA.Gets(tc, id);
                oCommercialInvoiceDocs = CreateObjects(reader);
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

            return oCommercialInvoiceDocs;
        }

        public CommercialInvoiceDoc Get(int id, Int64 nUserId)
        {
            CommercialInvoiceDoc oCommercialInvoiceDoc = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommercialInvoiceDocDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommercialInvoiceDoc = CreateObject(oReader);
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

            return oCommercialInvoiceDoc;
        }

        public CommercialInvoiceDoc GetWithDocFile(int id, Int64 nUserId)
        {
            CommercialInvoiceDoc oCommercialInvoiceDoc = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommercialInvoiceDocDA.GetWithDocFile(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommercialInvoiceDoc = CreateObject(oReader);
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

            return oCommercialInvoiceDoc;
        }

        #endregion
    }
}

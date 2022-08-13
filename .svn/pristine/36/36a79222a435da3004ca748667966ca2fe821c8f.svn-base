using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
	public class AttachDocumentService : MarshalByRefObject, IAttachDocumentService
	{
		#region Private functions and declaration

		private AttachDocument MapObject(NullHandler oReader)
		{
			AttachDocument oAttachDocument = new AttachDocument();
			oAttachDocument.AttachDocumentID = oReader.GetInt32("AttachDocumentID");
			oAttachDocument.RefID = oReader.GetInt32("RefID");
            oAttachDocument.RefType = (EnumAttachRefType)oReader.GetInt32("RefType");
            oAttachDocument.RefTypeInInt = oReader.GetInt32("RefType");
			oAttachDocument.FileName = oReader.GetString("FileName");
			oAttachDocument.AttachFile = oReader.GetBytes("AttachFile");
			oAttachDocument.FileType = oReader.GetString("FileType");
			oAttachDocument.Remarks = oReader.GetString("Remarks");
				
			return oAttachDocument;
		}

		private AttachDocument CreateObject(NullHandler oReader)
		{
			AttachDocument oAttachDocument = new AttachDocument();
			oAttachDocument = MapObject(oReader);
			return oAttachDocument;
		}

		private List<AttachDocument> CreateObjects(IDataReader oReader)
		{
			List<AttachDocument> oAttachDocument = new List<AttachDocument>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				AttachDocument oItem = CreateObject(oHandler);
				oAttachDocument.Add(oItem);
			}
			return oAttachDocument;
		}

		#endregion

		#region Interface implementation

        public AttachDocument Save(AttachDocument oAttachDocument, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oAttachDocument.AttachDocumentID <= 0)
                {
                    oAttachDocument.AttachDocumentID = AttachDocumentDA.GetNewID(tc);
                    AttachDocumentDA.Insert(tc, oAttachDocument, nUserId);
                }
                else
                {
                    AttachDocumentDA.Update(tc, oAttachDocument, nUserId);
                }
                tc.End();
                BusinessObject.Factory.SetObjectState(oAttachDocument, ObjectState.Saved);
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
            return oAttachDocument;
        }



        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AttachDocument oAttachDocument = new AttachDocument();
                oAttachDocument.AttachDocumentID = id;
                AttachDocumentDA.Delete(tc, id);
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


            public AttachDocument GetWithAttachFile(int id, Int64 nUserId)
			{
				AttachDocument oAttachDocument = new AttachDocument();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
                    IDataReader reader = AttachDocumentDA.GetWithAttachFile(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oAttachDocument = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get AttachDocument", e);
					#endregion
				}
				return oAttachDocument;
			}

            public AttachDocument GetUserSignature(int nSignatureUserID, Int64 nUserID)
            {
                AttachDocument oAttachDocument = new AttachDocument();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = AttachDocumentDA.GetUserSignature(tc, nSignatureUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oAttachDocument = CreateObject(oReader);
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
                    throw new ServiceException("Failed to Get AttachDocument", e);
                    #endregion
                }
                return oAttachDocument;
            }

			public List<AttachDocument> Gets(Int64 nUserID)
			{
				List<AttachDocument> oAttachDocuments = new List<AttachDocument>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = AttachDocumentDA.Gets(tc);
					oAttachDocuments = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					AttachDocument oAttachDocument = new AttachDocument();
					oAttachDocument.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oAttachDocuments;
			}

            public List<AttachDocument> Gets(int id, int nRefType, Int64 nUserID)
            {
                List<AttachDocument> oAttachDocuments = new List<AttachDocument>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = AttachDocumentDA.Gets(id,nRefType, tc);
                    oAttachDocuments = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    AttachDocument oAttachDocument = new AttachDocument();
                    oAttachDocument.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oAttachDocuments;
            }
			public List<AttachDocument> Gets (string sSQL, Int64 nUserID)
			{
				List<AttachDocument> oAttachDocuments = new List<AttachDocument>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = AttachDocumentDA.Gets(tc, sSQL);
					oAttachDocuments = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get AttachDocument", e);
					#endregion
				}
				return oAttachDocuments;
			}
            public List<AttachDocument> Gets_WithAttachFile(int nRefID, int nRefType, Int64 nUserID)
            {
                List<AttachDocument> oAttachDocuments = new List<AttachDocument>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = AttachDocumentDA.Gets_WithAttachFile(nRefID, nRefType, tc);
                    oAttachDocuments = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    AttachDocument oAttachDocument = new AttachDocument();
                    oAttachDocument.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oAttachDocuments;
            }

		#endregion
	}

}

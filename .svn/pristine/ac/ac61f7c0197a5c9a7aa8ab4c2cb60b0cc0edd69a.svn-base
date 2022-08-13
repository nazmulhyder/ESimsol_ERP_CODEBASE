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
    public class KnitDyeingProgramAttachmentService : MarshalByRefObject, IKnitDyeingProgramAttachmentService
	{
		#region Private functions and declaration

        private KnitDyeingProgramAttachment MapObject(NullHandler oReader)
		{
            KnitDyeingProgramAttachment oKnitDyeingProgramAttachment = new KnitDyeingProgramAttachment();
            oKnitDyeingProgramAttachment.KnitDyeingProgramAttachmentID = oReader.GetInt32("KnitDyeingProgramAttachmentID");
            oKnitDyeingProgramAttachment.KnitDyeingProgramID = oReader.GetInt32("KnitDyeingProgramID");
            oKnitDyeingProgramAttachment.FileName = oReader.GetString("FileName");
            oKnitDyeingProgramAttachment.AttachFile = oReader.GetBytes("AttachFile");
            oKnitDyeingProgramAttachment.FileType = oReader.GetString("FileType");
            oKnitDyeingProgramAttachment.Remarks = oReader.GetString("Remarks");

            return oKnitDyeingProgramAttachment;
		}

        private KnitDyeingProgramAttachment CreateObject(NullHandler oReader)
		{
            KnitDyeingProgramAttachment oKnitDyeingProgramAttachment = new KnitDyeingProgramAttachment();
            oKnitDyeingProgramAttachment = MapObject(oReader);
            return oKnitDyeingProgramAttachment;
		}

        private List<KnitDyeingProgramAttachment> CreateObjects(IDataReader oReader)
		{
            List<KnitDyeingProgramAttachment> oKnitDyeingProgramAttachment = new List<KnitDyeingProgramAttachment>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
                KnitDyeingProgramAttachment oItem = CreateObject(oHandler);
                oKnitDyeingProgramAttachment.Add(oItem);
			}
            return oKnitDyeingProgramAttachment;
		}

		#endregion

		#region Interface implementation

        public KnitDyeingProgramAttachment Save(KnitDyeingProgramAttachment oKnitDyeingProgramAttachment, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oKnitDyeingProgramAttachment.KnitDyeingProgramAttachmentID <= 0)
                {
                    oKnitDyeingProgramAttachment.KnitDyeingProgramAttachmentID = KnitDyeingProgramAttachmentDA.GetNewID(tc);
                    KnitDyeingProgramAttachmentDA.Insert(tc, oKnitDyeingProgramAttachment, nUserId);
                }
                else
                {
                    KnitDyeingProgramAttachmentDA.Update(tc, oKnitDyeingProgramAttachment, nUserId);
                }
                tc.End();
                BusinessObject.Factory.SetObjectState(oKnitDyeingProgramAttachment, ObjectState.Saved);
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
            return oKnitDyeingProgramAttachment;
        }
        public List<KnitDyeingProgramAttachment> Gets(int id, Int64 nUserID)
        {
            List<KnitDyeingProgramAttachment> oKnitDyeingProgramAttachments = new List<KnitDyeingProgramAttachment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnitDyeingProgramAttachmentDA.Gets(id, tc);
                oKnitDyeingProgramAttachments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnitDyeingProgramAttachment oKnitDyeingProgramAttachment = new KnitDyeingProgramAttachment();
                oKnitDyeingProgramAttachment.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnitDyeingProgramAttachments;
        }
        public KnitDyeingProgramAttachment GetWithAttachFile(int id, Int64 nUserId)
        {
            KnitDyeingProgramAttachment oKnitDyeingProgramAttachment = new KnitDyeingProgramAttachment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnitDyeingProgramAttachmentDA.GetWithAttachFile(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingProgramAttachment = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnitDyeing Program Attachment", e);
                #endregion
            }
            return oKnitDyeingProgramAttachment;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnitDyeingProgramAttachment oKnitDyeingProgramAttachment = new KnitDyeingProgramAttachment();
                oKnitDyeingProgramAttachment.KnitDyeingProgramAttachmentID = id;
                KnitDyeingProgramAttachmentDA.Delete(tc, id);
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

        public List<KnitDyeingProgramAttachment> Gets(string sSQL, Int64 nUserID)
        {
            List<KnitDyeingProgramAttachment> oKnitDyeingProgramAttachments = new List<KnitDyeingProgramAttachment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnitDyeingProgramAttachmentDA.Gets(tc, sSQL);
                oKnitDyeingProgramAttachments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnitDyeingProgramAttachment", e);
                #endregion
            }
            return oKnitDyeingProgramAttachments;
        }
		#endregion
	}

}

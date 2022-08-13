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
    public class FabricAttachmentService : MarshalByRefObject, IFabricAttachmentService
    {
        #region Private functions and declaration
        private FabricAttachment MapObject(NullHandler oReader)
        {
            FabricAttachment oFabricAttachment = new FabricAttachment();
            oFabricAttachment.FabricAttachmentID = oReader.GetInt32("FabricAttachmentID");
            oFabricAttachment.FabricID = oReader.GetInt32("FabricID");
            oFabricAttachment.AttatchmentName = oReader.GetString("AttatchmentName");
            oFabricAttachment.AttatchFile = oReader.GetBytes("AttatchFile");
            oFabricAttachment.FileType = oReader.GetString("FileType");
            oFabricAttachment.Remarks = oReader.GetString("Remarks");
            oFabricAttachment.SwatchType = (EnumSwatchType)oReader.GetInt32("SwatchType");
            oFabricAttachment.SwatchTypeInInt = oReader.GetInt32("SwatchType");
            return oFabricAttachment;
        }

        private FabricAttachment CreateObject(NullHandler oReader)
        {
            FabricAttachment oFabricAttachment = new FabricAttachment();
            oFabricAttachment = MapObject(oReader);
            return oFabricAttachment;
        }

        private List<FabricAttachment> CreateObjects(IDataReader oReader)
        {
            List<FabricAttachment> oFabricAttachments = new List<FabricAttachment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricAttachment oItem = CreateObject(oHandler);
                oFabricAttachments.Add(oItem);
            }
            return oFabricAttachments;
        }

        #endregion

        #region Interface implementation
        public FabricAttachment Save(FabricAttachment oFabricAttachment, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oFabricAttachment.FabricAttachmentID <= 0)
                {
                    oFabricAttachment.FabricAttachmentID = FabricAttachmentDA.GetNewID(tc);
                    FabricAttachmentDA.Insert(tc, oFabricAttachment, nUserId);
                }
                else
                {
                    FabricAttachmentDA.Update(tc, oFabricAttachment, nUserId);
                }
                tc.End();
                BusinessObject.Factory.SetObjectState(oFabricAttachment, ObjectState.Saved);
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
            return oFabricAttachment;
        }


        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricAttachment oFabricAttachment = new FabricAttachment();
                FabricAttachmentDA.Delete(tc, id);
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

        public List<FabricAttachment> Gets(Int64 nUserId)
        {
            List<FabricAttachment> oFabricAttachments = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricAttachmentDA.Gets(tc);
                oFabricAttachments = CreateObjects(reader);
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

            return oFabricAttachments;
        }

        public List<FabricAttachment> GetsAttachmentByFabric(int nFabricId, Int64 nUserId)
        {
            List<FabricAttachment> oFabricAttachments = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricAttachmentDA.GetsAttachmentByFabric(tc, nFabricId);
                oFabricAttachments = CreateObjects(reader);
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

            return oFabricAttachments;
        }

        public FabricAttachment Get(int id, Int64 nUserId)
        {
            FabricAttachment oFabricAttachment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricAttachmentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricAttachment = CreateObject(oReader);
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

            return oFabricAttachment;
        }

        public FabricAttachment GetWithAttachFile(int id, Int64 nUserId)
        {
            FabricAttachment oFabricAttachment = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricAttachmentDA.GetWithAttachFile(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricAttachment = CreateObject(oReader);
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

            return oFabricAttachment;
        }

        #endregion
    }
}

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
    public class MeasurementSpecAttachmentService : MarshalByRefObject, IMeasurementSpecAttachmentService
    {
        #region Private functions and declaration
        private MeasurementSpecAttachment MapObject(NullHandler oReader)
        {
            MeasurementSpecAttachment oMeasurementSpecAttachment = new MeasurementSpecAttachment();
            oMeasurementSpecAttachment.MeasurementSpecAttachmentID = oReader.GetInt32("MeasurementSpecAttachmentID");
            oMeasurementSpecAttachment.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oMeasurementSpecAttachment.AttatchmentName = oReader.GetString("AttatchmentName");
            oMeasurementSpecAttachment.AttatchFile = oReader.GetBytes("AttatchFile");
            oMeasurementSpecAttachment.FileType = oReader.GetString("FileType");
            oMeasurementSpecAttachment.Remarks = oReader.GetString("Remarks");

            return oMeasurementSpecAttachment;
        }

        private MeasurementSpecAttachment CreateObject(NullHandler oReader)
        {
            MeasurementSpecAttachment oMeasurementSpecAttachment = new MeasurementSpecAttachment();
            oMeasurementSpecAttachment = MapObject(oReader);
            return oMeasurementSpecAttachment;
        }

        private List<MeasurementSpecAttachment> CreateObjects(IDataReader oReader)
        {
            List<MeasurementSpecAttachment> oMeasurementSpecAttachments = new List<MeasurementSpecAttachment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MeasurementSpecAttachment oItem = CreateObject(oHandler);
                oMeasurementSpecAttachments.Add(oItem);
            }
            return oMeasurementSpecAttachments;
        }

        #endregion

        #region Interface implementation
        public MeasurementSpecAttachmentService() { }

        public MeasurementSpecAttachment Save(MeasurementSpecAttachment oMeasurementSpecAttachment, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oMeasurementSpecAttachment.MeasurementSpecAttachmentID <= 0)
                {
                    oMeasurementSpecAttachment.MeasurementSpecAttachmentID = MeasurementSpecAttachmentDA.GetNewID(tc);
                    MeasurementSpecAttachmentDA.Insert(tc, oMeasurementSpecAttachment, nUserId);
                }
                else
                {
                    MeasurementSpecAttachmentDA.Update(tc, oMeasurementSpecAttachment, nUserId);
                }
                tc.End();
                BusinessObject.Factory.SetObjectState(oMeasurementSpecAttachment, ObjectState.Saved);
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
            return oMeasurementSpecAttachment;
        }



        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MeasurementSpecAttachment oMeasurementSpecAttachment = new MeasurementSpecAttachment();
                oMeasurementSpecAttachment.MeasurementSpecAttachmentID = id;
                MeasurementSpecAttachmentDA.Delete(tc, id);
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


        public List<MeasurementSpecAttachment> Gets(int id, bool bIsMeasurmentSpecAttachment,  Int64 nUserId)
        {
            List<MeasurementSpecAttachment> oMeasurementSpecAttachments = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeasurementSpecAttachmentDA.Gets(tc, id, bIsMeasurmentSpecAttachment);
                oMeasurementSpecAttachments = CreateObjects(reader);
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

            return oMeasurementSpecAttachments;
        }

        public MeasurementSpecAttachment Get(int id, Int64 nUserId)
        {
            MeasurementSpecAttachment oMeasurementSpecAttachment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeasurementSpecAttachmentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeasurementSpecAttachment = CreateObject(oReader);
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

            return oMeasurementSpecAttachment;
        }

        public MeasurementSpecAttachment GetWithAttachFile(int id, Int64 nUserId)
        {
            MeasurementSpecAttachment oMeasurementSpecAttachment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeasurementSpecAttachmentDA.GetWithAttachFile(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeasurementSpecAttachment = CreateObject(oReader);
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

            return oMeasurementSpecAttachment;
        }

        #endregion
    }
}

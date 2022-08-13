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
    public class DispoProductionAttachmentService : MarshalByRefObject, IDispoProductionAttachmentService
    {
        #region Private functions and declaration
        private DispoProductionAttachment MapObject(NullHandler oReader)
        {
            DispoProductionAttachment oDispoProductionAttachment = new DispoProductionAttachment();
            oDispoProductionAttachment.DispoProductionAttachmentID = oReader.GetInt32("DispoProductionAttachmentID");
            oDispoProductionAttachment.DispoProductionCommentID = oReader.GetInt32("DispoProductionCommentID");
            oDispoProductionAttachment.FileName = oReader.GetString("FileName");
            return oDispoProductionAttachment;
        }

        public DispoProductionAttachment CreateObject(NullHandler oReader)
        {
            DispoProductionAttachment oDispoProductionAttachment = new DispoProductionAttachment();
            oDispoProductionAttachment = MapObject(oReader);
            return oDispoProductionAttachment;
        }

        private List<DispoProductionAttachment> CreateObjects(IDataReader oReader)
        {
            List<DispoProductionAttachment> oDispoProductionAttachment = new List<DispoProductionAttachment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DispoProductionAttachment oItem = CreateObject(oHandler);
                oDispoProductionAttachment.Add(oItem);
            }
            return oDispoProductionAttachment;
        }

        #endregion

        #region Interface implementation
        public DispoProductionAttachmentService() { }

        public DispoProductionAttachment Save(DispoProductionAttachment oDispoProductionAttachment, Int64 nUserID)
        {
            TransactionContext tc = null;
            oDispoProductionAttachment.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDispoProductionAttachment.DispoProductionAttachmentID <= 0)
                {
                    reader = DispoProductionAttachmentDA.InsertUpdate(tc, oDispoProductionAttachment, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DispoProductionAttachmentDA.InsertUpdate(tc, oDispoProductionAttachment, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDispoProductionAttachment = new DispoProductionAttachment();
                    oDispoProductionAttachment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDispoProductionAttachment.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save DispoProductionAttachment. Because of " + e.Message, e);
                #endregion
            }
            return oDispoProductionAttachment;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DispoProductionAttachment oDispoProductionAttachment = new DispoProductionAttachment();
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "DispoProductionAttachment", EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "DispoProductionAttachment", id);
                oDispoProductionAttachment.DispoProductionAttachmentID = id;
                DispoProductionAttachmentDA.Delete(tc, oDispoProductionAttachment, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete DispoProductionAttachment. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public DispoProductionAttachment Get(int id, Int64 nUserId)
        {
            DispoProductionAttachment oAccountHead = new DispoProductionAttachment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DispoProductionAttachmentDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get DispoProductionAttachment", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<DispoProductionAttachment> Gets(Int64 nUserID)
        {
            List<DispoProductionAttachment> oDispoProductionAttachment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DispoProductionAttachmentDA.Gets(tc);
                oDispoProductionAttachment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DispoProductionAttachment", e);
                #endregion
            }

            return oDispoProductionAttachment;
        }

        public List<DispoProductionAttachment> Gets(string sSQL, Int64 nUserID)
        {
            List<DispoProductionAttachment> oDispoProductionAttachment = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DispoProductionAttachmentDA.Gets(tc, sSQL);
                oDispoProductionAttachment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DispoProductionAttachment", e);
                #endregion
            }

            return oDispoProductionAttachment;
        }

        public List<DispoProductionAttachment> GetsByDispoProductionAttachment(string sDispoProductionAttachment, Int64 nUserID)
        {
            List<DispoProductionAttachment> oDispoProductionAttachment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DispoProductionAttachmentDA.GetsByDispoProductionAttachment(tc, sDispoProductionAttachment);
                oDispoProductionAttachment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DispoProductionAttachment", e);
                #endregion
            }

            return oDispoProductionAttachment;
        }


        #endregion
    }
}

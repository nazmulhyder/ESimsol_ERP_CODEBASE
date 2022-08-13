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
    public class BusinessSessionService : MarshalByRefObject, IBusinessSessionService
    {
        #region Private functions and declaration
        private BusinessSession MapObject(NullHandler oReader)
        {
            BusinessSession oBusinessSession = new BusinessSession();
            oBusinessSession.BusinessSessionID = oReader.GetInt32("BusinessSessionID");
            oBusinessSession.SessionName = oReader.GetString("SessionName");
            oBusinessSession.IsActive = oReader.GetBoolean("IsActive");
            oBusinessSession.Note = oReader.GetString("Note");
            return oBusinessSession;
        }

        private BusinessSession CreateObject(NullHandler oReader)
        {
            BusinessSession oBusinessSession = new BusinessSession();
            oBusinessSession = MapObject(oReader);
            return oBusinessSession;
        }

        private List<BusinessSession> CreateObjects(IDataReader oReader)
        {
            List<BusinessSession> oBusinessSession = new List<BusinessSession>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BusinessSession oItem = CreateObject(oHandler);
                oBusinessSession.Add(oItem);
            }
            return oBusinessSession;
        }

        #endregion

        #region Interface implementation
        public BusinessSessionService() { }

        public BusinessSession Save(BusinessSession oBusinessSession, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBusinessSession.BusinessSessionID <= 0)
                {
                    reader = BusinessSessionDA.InsertUpdate(tc, oBusinessSession, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = BusinessSessionDA.InsertUpdate(tc, oBusinessSession, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBusinessSession = new BusinessSession();
                    oBusinessSession = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBusinessSession = new BusinessSession();
                oBusinessSession.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save BusinessSession. Because of " + e.Message, e);
                #endregion
            }
            return oBusinessSession;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BusinessSession oBusinessSession = new BusinessSession();
                oBusinessSession.BusinessSessionID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.BusinessSession, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "BusinessSession", id);
                BusinessSessionDA.Delete(tc, oBusinessSession, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BusinessSession. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public BusinessSession Get(int id, Int64 nUserId)
        {
            BusinessSession oAccountHead = new BusinessSession();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BusinessSessionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get BusinessSession", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<BusinessSession> Gets(Int64 nUserID)
        {
            List<BusinessSession> oBusinessSession = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BusinessSessionDA.Gets(tc);
                oBusinessSession = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BusinessSession", e);
                #endregion
            }

            return oBusinessSession;
        }

        public List<BusinessSession> Gets(bool bIsActive, Int64 nUserID)
        {
            List<BusinessSession> oBusinessSession = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BusinessSessionDA.Gets(tc, bIsActive);
                oBusinessSession = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BusinessSession", e);
                #endregion
            }

            return oBusinessSession;
        }
        #endregion
    }
}

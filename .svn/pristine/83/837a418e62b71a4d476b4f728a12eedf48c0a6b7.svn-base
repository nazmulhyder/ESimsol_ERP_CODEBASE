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
    public class ServiceWorkService : MarshalByRefObject, IServiceWorkService
    {
        #region Private functions and declaration
        private ServiceWork MapObject(NullHandler oReader)
        {
            ServiceWork oServiceWork = new ServiceWork();
            oServiceWork.ServiceWorkID = oReader.GetInt32("ServiceWorkID");
            oServiceWork.ServiceCode = oReader.GetString("ServiceCode");
            oServiceWork.ServiceName = oReader.GetString("ServiceName");
            oServiceWork.ServiceType =(EnumServiceType)oReader.GetInt32("ServiceType");
            oServiceWork.Remarks = oReader.GetString("Remarks");
            return oServiceWork;
        }

        private ServiceWork CreateObject(NullHandler oReader)
        {
            ServiceWork oServiceWork = new ServiceWork();
            oServiceWork = MapObject(oReader);
            return oServiceWork;
        }

        private List<ServiceWork> CreateObjects(IDataReader oReader)
        {
            List<ServiceWork> oServiceWork = new List<ServiceWork>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ServiceWork oItem = CreateObject(oHandler);
                oServiceWork.Add(oItem);
            }
            return oServiceWork;
        }

        #endregion

        #region Interface implementation
        public ServiceWorkService() { }

        public ServiceWork Save(ServiceWork oServiceWork, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oServiceWork.ServiceWorkID <= 0)
                {
                    reader = ServiceWorkDA.InsertUpdate(tc, oServiceWork, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ServiceWorkDA.InsertUpdate(tc, oServiceWork, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceWork = new ServiceWork();
                    oServiceWork = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ServiceWork. Because of " + e.Message, e);
                #endregion
            }
            return oServiceWork;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ServiceWork oServiceWork = new ServiceWork();
                oServiceWork.ServiceWorkID = id;
                ServiceWorkDA.Delete(tc, oServiceWork, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ServiceWork Get(int id, Int64 nUserId)
        {
            ServiceWork oServiceWork = new ServiceWork();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ServiceWorkDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceWork = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ServiceWork", e);
                #endregion
            }
            return oServiceWork;
        }

        public List<ServiceWork> GetByServiceCode(string sServiceCode, Int64 nUserID)
        {
            List<ServiceWork> oServiceWorks = new List<ServiceWork>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ServiceWorkDA.GetsByServiceCode(tc, sServiceCode);
                oServiceWorks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ServiceWorks", e);
                #endregion
            }
            return oServiceWorks;
        }

        public List<ServiceWork> Gets(Int64 nUserID)
        {
            List<ServiceWork> oServiceWorks = new List<ServiceWork>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ServiceWorkDA.Gets(tc);
                oServiceWorks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ServiceWork", e);
                #endregion
            }
            return oServiceWorks;
        }
        public List<ServiceWork> Gets(string sSQL, Int64 nUserID)
        {
            List<ServiceWork> oServiceWorks = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ServiceWorkDA.Gets(tc, sSQL);
                oServiceWorks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ServiceWork", e);
                #endregion
            }
            return oServiceWorks;
        }


        public List<ServiceWork> GetsByServiceCode(string sServiceCode, Int64 nUserID)
        {
            List<ServiceWork> oServiceWorks = new List<ServiceWork>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ServiceWorkDA.GetsByServiceCode(tc, sServiceCode);
                oServiceWorks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ServiceWorks", e);
                #endregion
            }
            return oServiceWorks;
        }
        public List<ServiceWork> GetsByServiceNameWithType(string sServiceName,int nServiceType, Int64 nUserID)
        {
            List<ServiceWork> oServiceWorks = new List<ServiceWork>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ServiceWorkDA.GetsByServiceNameWithType(tc, sServiceName, nServiceType);
                oServiceWorks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ServiceWorks", e);
                #endregion
            }
            return oServiceWorks;
        }
       
        #endregion
    }
}
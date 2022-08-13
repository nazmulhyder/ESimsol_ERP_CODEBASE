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
    public class EmployeeProcessMailHistoryService : MarshalByRefObject, IEmployeeProcessMailHistoryService
    {
        #region Private functions and declaration
        private EmployeeProcessMailHistory MapObject(NullHandler oReader)
        {
            EmployeeProcessMailHistory oEmployeeProcessMailHistory = new EmployeeProcessMailHistory();
            oEmployeeProcessMailHistory.EPMHID = oReader.GetInt32("EPMHID");
            oEmployeeProcessMailHistory.PPMID = oReader.GetInt32("PPMID");
            oEmployeeProcessMailHistory.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeProcessMailHistory.IsStatus = oReader.GetBoolean("IsStatus");
            oEmployeeProcessMailHistory.FeedBackMessage = oReader.GetString("FeedBackMessage");
            oEmployeeProcessMailHistory.SendingTime = oReader.GetDateTime("SendingTime");
            oEmployeeProcessMailHistory.OperatedBy = oReader.GetInt32("OperatedBy");
            oEmployeeProcessMailHistory.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeProcessMailHistory.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeProcessMailHistory.OperatedByName = oReader.GetString("OperatedByName");
            oEmployeeProcessMailHistory.Email = oReader.GetString("Email");
            return oEmployeeProcessMailHistory;
        }
        private EmployeeProcessMailHistory CreateObject(NullHandler oReader)
        {
            EmployeeProcessMailHistory oEmployeeProcessMailHistory = new EmployeeProcessMailHistory();
            oEmployeeProcessMailHistory = MapObject(oReader);
            return oEmployeeProcessMailHistory;
        }
        private List<EmployeeProcessMailHistory> CreateObjects(IDataReader oReader)
        {
            List<EmployeeProcessMailHistory> oEmployeeProcessMailHistory = new List<EmployeeProcessMailHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeProcessMailHistory oItem = CreateObject(oHandler);
                oEmployeeProcessMailHistory.Add(oItem);
            }
            return oEmployeeProcessMailHistory;
        }

        #endregion

        #region Interface implementation
        public EmployeeProcessMailHistoryService() { }
        public EmployeeProcessMailHistory Save(EmployeeProcessMailHistory oEmployeeProcessMailHistory, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oEmployeeProcessMailHistory.EPMHID <= 0)
                {
                    reader = EmployeeProcessMailHistoryDA.InsertUpdate(tc, oEmployeeProcessMailHistory, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = EmployeeProcessMailHistoryDA.InsertUpdate(tc, oEmployeeProcessMailHistory, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeProcessMailHistory = new EmployeeProcessMailHistory();
                    oEmployeeProcessMailHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save EmployeeProcessMailHistory. Because of " + e.Message, e);
                #endregion
            }
            return oEmployeeProcessMailHistory;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeProcessMailHistory oEmployeeProcessMailHistory = new EmployeeProcessMailHistory();
                oEmployeeProcessMailHistory.EPMHID = id;
                EmployeeProcessMailHistoryDA.Delete(tc, oEmployeeProcessMailHistory, EnumDBOperation.Delete, nUserId);
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
        public EmployeeProcessMailHistory Get(int id, Int64 nUserId)
        {
            EmployeeProcessMailHistory oEmployeeProcessMailHistory = new EmployeeProcessMailHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = EmployeeProcessMailHistoryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeProcessMailHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeProcessMailHistory", e);
                #endregion
            }
            return oEmployeeProcessMailHistory;
        }
        public List<EmployeeProcessMailHistory> Gets(Int64 nUserID)
        {
            List<EmployeeProcessMailHistory> oEmployeeProcessMailHistorys = new List<EmployeeProcessMailHistory>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeProcessMailHistoryDA.Gets(tc);
                oEmployeeProcessMailHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeProcessMailHistory", e);
                #endregion
            }
            return oEmployeeProcessMailHistorys;
        }
        public List<EmployeeProcessMailHistory> Gets(string sSQL,Int64 nUserID)
        {
            List<EmployeeProcessMailHistory> oEmployeeProcessMailHistorys = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeProcessMailHistoryDA.Gets(tc,sSQL);
                oEmployeeProcessMailHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeProcessMailHistory", e);
                #endregion
            }
            return oEmployeeProcessMailHistorys;
        }
        #endregion
    }   
}


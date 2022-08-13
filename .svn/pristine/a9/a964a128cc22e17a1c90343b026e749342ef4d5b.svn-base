using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class EmployeeActiveInactiveHistoryService : MarshalByRefObject, IEmployeeActiveInactiveHistoryService
    {
        #region Private functions and declaration
        private EmployeeActiveInactiveHistory MapObject(NullHandler oReader)
        {
            EmployeeActiveInactiveHistory oEmployeeActiveInactiveHistory = new EmployeeActiveInactiveHistory();
            oEmployeeActiveInactiveHistory.EAIHID = oReader.GetInt32("EAIHID");
            oEmployeeActiveInactiveHistory.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeActiveInactiveHistory.ActiveDate = oReader.GetDateTime("ActiveDate");
            oEmployeeActiveInactiveHistory.InactiveDate = oReader.GetDateTime("InactiveDate");
            return oEmployeeActiveInactiveHistory;
        }
        private EmployeeActiveInactiveHistory CreateObject(NullHandler oReader)
        {
            EmployeeActiveInactiveHistory oEmployeeActiveInactiveHistory = new EmployeeActiveInactiveHistory();
            oEmployeeActiveInactiveHistory = MapObject(oReader);
            return oEmployeeActiveInactiveHistory;
        }
        private List<EmployeeActiveInactiveHistory> CreateObjects(IDataReader oReader)
        {
            List<EmployeeActiveInactiveHistory> oEmployeeActiveInactiveHistory = new List<EmployeeActiveInactiveHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeActiveInactiveHistory oItem = CreateObject(oHandler);
                oEmployeeActiveInactiveHistory.Add(oItem);
            }
            return oEmployeeActiveInactiveHistory;
        }
        #endregion

        #region Interface implementation
        public EmployeeActiveInactiveHistory Get(int id, int nUserId)
        {
            EmployeeActiveInactiveHistory oEmployeeActiveInactiveHistory = new EmployeeActiveInactiveHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeActiveInactiveHistoryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeActiveInactiveHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeActiveInactiveHistory", e);
                #endregion
            }
            return oEmployeeActiveInactiveHistory;
        }
        public List<EmployeeActiveInactiveHistory> Gets(int nUserID)
        {
            List<EmployeeActiveInactiveHistory> oEmployeeActiveInactiveHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeActiveInactiveHistoryDA.Gets(tc);
                oEmployeeActiveInactiveHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeActiveInactiveHistory", e);
                #endregion
            }
            return oEmployeeActiveInactiveHistory;
        }
        public List<EmployeeActiveInactiveHistory> Gets(string sSQL, int nUserID)
        {
            List<EmployeeActiveInactiveHistory> oEmployeeActiveInactiveHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeActiveInactiveHistoryDA.Gets(tc, sSQL);
                oEmployeeActiveInactiveHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeActiveInactiveHistory", e);
                #endregion
            }

            return oEmployeeActiveInactiveHistory;
        }
        #endregion
    }
}


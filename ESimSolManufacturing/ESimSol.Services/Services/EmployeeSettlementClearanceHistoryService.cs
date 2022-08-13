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
    public class EmployeeSettlementClearanceHistoryService : MarshalByRefObject, IEmployeeSettlementClearanceHistoryService
    {
        #region Private functions and declaration
        private EmployeeSettlementClearanceHistory MapObject(NullHandler oReader)
        {
            EmployeeSettlementClearanceHistory oEmployeeSettlementClearanceHistory = new EmployeeSettlementClearanceHistory();
            oEmployeeSettlementClearanceHistory.ESCHID = oReader.GetInt32("ESCHID");
            oEmployeeSettlementClearanceHistory.ESCID = oReader.GetInt32("ESCID");
            oEmployeeSettlementClearanceHistory.PreviousStatus = (EnumESCrearance)oReader.GetInt16("PreviousStatus");
            oEmployeeSettlementClearanceHistory.CurrentStatus = (EnumESCrearance)oReader.GetInt16("CurrentStatus");
            oEmployeeSettlementClearanceHistory.Note = oReader.GetString("Note");
            return oEmployeeSettlementClearanceHistory;
        }

        private EmployeeSettlementClearanceHistory CreateObject(NullHandler oReader)
        {
            EmployeeSettlementClearanceHistory oEmployeeSettlementClearanceHistory = MapObject(oReader);
            return oEmployeeSettlementClearanceHistory;
        }

        private List<EmployeeSettlementClearanceHistory> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSettlementClearanceHistory> oEmployeeSettlementClearanceHistorys = new List<EmployeeSettlementClearanceHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSettlementClearanceHistory oItem = CreateObject(oHandler);
                oEmployeeSettlementClearanceHistorys.Add(oItem);
            }
            return oEmployeeSettlementClearanceHistorys;
        }

        #endregion

        #region Interface implementation
        public EmployeeSettlementClearanceHistoryService() { }

        public EmployeeSettlementClearanceHistory IUD(EmployeeSettlementClearanceHistory oEmployeeSettlementClearanceHistory, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeSettlementClearanceHistoryDA.IUD(tc, oEmployeeSettlementClearanceHistory, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementClearanceHistory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeSettlementClearanceHistory.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeSettlementClearanceHistory.ESCHID = 0;
                #endregion
            }
            return oEmployeeSettlementClearanceHistory;
        }

        public EmployeeSettlementClearanceHistory Get(int nESCHID, Int64 nUserId)
        {
            EmployeeSettlementClearanceHistory oEmployeeSettlementClearanceHistory = new EmployeeSettlementClearanceHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = EmployeeSettlementClearanceHistoryDA.Get(nESCHID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementClearanceHistory = CreateObject(oReader);
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
                oEmployeeSettlementClearanceHistory.ErrorMessage = e.Message;
                #endregion
            }
            return oEmployeeSettlementClearanceHistory;
        }

        public EmployeeSettlementClearanceHistory Get(string sSQL, Int64 nUserId)
        {
            EmployeeSettlementClearanceHistory oEmployeeSettlementClearanceHistory = new EmployeeSettlementClearanceHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = EmployeeSettlementClearanceHistoryDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementClearanceHistory = CreateObject(oReader);
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

                oEmployeeSettlementClearanceHistory.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSettlementClearanceHistory;
        }

        public List<EmployeeSettlementClearanceHistory> Gets(Int64 nUserID)
        {
            List<EmployeeSettlementClearanceHistory> oEmployeeSettlementClearanceHistory = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeSettlementClearanceHistoryDA.Gets(tc);
                oEmployeeSettlementClearanceHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSettlementClearanceHistory", e);
                #endregion
            }
            return oEmployeeSettlementClearanceHistory;
        }

        public List<EmployeeSettlementClearanceHistory> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSettlementClearanceHistory> oEmployeeSettlementClearanceHistory = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeSettlementClearanceHistoryDA.Gets(sSQL, tc);
                oEmployeeSettlementClearanceHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSettlementClearanceHistory", e);
                #endregion
            }
            return oEmployeeSettlementClearanceHistory;
        }
        #endregion
    }
}

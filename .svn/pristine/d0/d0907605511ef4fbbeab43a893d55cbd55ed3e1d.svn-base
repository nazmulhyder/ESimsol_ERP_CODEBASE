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
    public class ReceivedChequeHistoryService : MarshalByRefObject, IReceivedChequeHistoryService
    {
        #region Private functions and declaration
        private ReceivedChequeHistory MapObject(NullHandler oReader)
        {
            ReceivedChequeHistory oReceivedChequeHistory = new ReceivedChequeHistory();
            oReceivedChequeHistory.ReceivedChequeHistoryID = oReader.GetInt32("ReceivedChequeHistoryID");
            oReceivedChequeHistory.ReceivedChequeID = oReader.GetInt32("ReceivedChequeID");
            oReceivedChequeHistory.PreviousStatus = (EnumReceivedChequeStatus)oReader.GetInt32("PreviousStatus");
            oReceivedChequeHistory.CurrentStatus = (EnumReceivedChequeStatus)oReader.GetInt32("CurrentStatus");
            oReceivedChequeHistory.Note = oReader.GetString("Note");
            oReceivedChequeHistory.ChangeLog = oReader.GetString("ChangeLog");
            oReceivedChequeHistory.OperationBy = oReader.GetInt32("OperationBy");
            oReceivedChequeHistory.OperationDateTime = oReader.GetDateTime("OperationDateTime");
            oReceivedChequeHistory.OperationByName = oReader.GetString("OperationByName");
            return oReceivedChequeHistory;
        }

        private ReceivedChequeHistory CreateObject(NullHandler oReader)
        {
            ReceivedChequeHistory oReceivedChequeHistory = new ReceivedChequeHistory();
            oReceivedChequeHistory = MapObject(oReader);
            return oReceivedChequeHistory;
        }

        private List<ReceivedChequeHistory> CreateObjects(IDataReader oReader)
        {
            List<ReceivedChequeHistory> oReceivedChequeHistorys = new List<ReceivedChequeHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ReceivedChequeHistory oItem = CreateObject(oHandler);
                oReceivedChequeHistorys.Add(oItem);
            }
            return oReceivedChequeHistorys;
        }

        #endregion

        #region Interface implementation
        public ReceivedChequeHistoryService() { }

        

        public ReceivedChequeHistory Get(int nReceivedChequeID, int nStatus, int nCurrentUserID)
        {
            ReceivedChequeHistory oReceivedChequeHistory = new ReceivedChequeHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ReceivedChequeHistoryDA.Get(tc, nReceivedChequeID, nStatus);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oReceivedChequeHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ReceivedChequeHistory", e);
                #endregion
            }

            return oReceivedChequeHistory;
        }
        public List<ReceivedChequeHistory> Gets(int nCurrentUserID)
        {
            List<ReceivedChequeHistory> oReceivedChequeHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ReceivedChequeHistoryDA.Gets(tc);
                oReceivedChequeHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ReceivedChequeHistory", e);
                #endregion
            }

            return oReceivedChequeHistory;
        }

        public List<ReceivedChequeHistory> Gets(int nReceivedChequeID, int nCurrentUserID)
        {
            List<ReceivedChequeHistory> oReceivedChequeHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ReceivedChequeHistoryDA.Gets(tc, nReceivedChequeID);
                oReceivedChequeHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ReceivedChequeHistory", e);
                #endregion
            }

            return oReceivedChequeHistorys;
        }
        public List<ReceivedChequeHistory> Gets(string sSQL, int nCurrentUserID)
        {
            List<ReceivedChequeHistory> oReceivedChequeHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ReceivedChequeHistoryDA.Gets(tc, sSQL);
                oReceivedChequeHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ReceivedChequeHistory", e);
                #endregion
            }

            return oReceivedChequeHistorys;
        }

        

       

         
        #endregion
    }
}

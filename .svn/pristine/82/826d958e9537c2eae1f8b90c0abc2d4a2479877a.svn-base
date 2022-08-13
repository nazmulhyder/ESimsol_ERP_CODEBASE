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
    public class ChequeHistoryService : MarshalByRefObject, IChequeHistoryService
    {
        #region Private functions and declaration
        private ChequeHistory MapObject(NullHandler oReader)
        {
            ChequeHistory oChequeHistory = new ChequeHistory();
            oChequeHistory.ChequeHistoryID = oReader.GetInt32("ChequeHistoryID");
            oChequeHistory.ChequeID = oReader.GetInt32("ChequeID");
            oChequeHistory.PreviousStatus = (EnumChequeStatus)oReader.GetInt32("PreviousStatus");
            oChequeHistory.CurrentStatus = (EnumChequeStatus)oReader.GetInt32("CurrentStatus");
            oChequeHistory.Note = oReader.GetString("Note");
            oChequeHistory.ChangeLog = oReader.GetString("ChangeLog");
            oChequeHistory.OperationBy = oReader.GetInt32("OperationBy");
            oChequeHistory.OperationDateTime = oReader.GetDateTime("OperationDateTime");
            oChequeHistory.OperationByName = oReader.GetString("OperationByName");
            return oChequeHistory;
        }

        private ChequeHistory CreateObject(NullHandler oReader)
        {
            ChequeHistory oChequeHistory = new ChequeHistory();
            oChequeHistory = MapObject(oReader);
            return oChequeHistory;
        }

        private List<ChequeHistory> CreateObjects(IDataReader oReader)
        {
            List<ChequeHistory> oChequeHistorys = new List<ChequeHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ChequeHistory oItem = CreateObject(oHandler);
                oChequeHistorys.Add(oItem);
            }
            return oChequeHistorys;
        }

        #endregion

        #region Interface implementation
        public ChequeHistoryService() { }

        

        public ChequeHistory Get(int nChequeID, int nStatus, int nCurrentUserID)
        {
            ChequeHistory oChequeHistory = new ChequeHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ChequeHistoryDA.Get(tc, nChequeID, nStatus);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ChequeHistory", e);
                #endregion
            }

            return oChequeHistory;
        }
        public List<ChequeHistory> Gets(int nCurrentUserID)
        {
            List<ChequeHistory> oChequeHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeHistoryDA.Gets(tc);
                oChequeHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChequeHistory", e);
                #endregion
            }

            return oChequeHistory;
        }

        public List<ChequeHistory> Gets(int nChequeID, int nCurrentUserID)
        {
            List<ChequeHistory> oChequeHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeHistoryDA.Gets(tc, nChequeID);
                oChequeHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChequeHistory", e);
                #endregion
            }

            return oChequeHistorys;
        }
        public List<ChequeHistory> Gets(string sSQL, int nCurrentUserID)
        {
            List<ChequeHistory> oChequeHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeHistoryDA.Gets(tc, sSQL);
                oChequeHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChequeHistory", e);
                #endregion
            }

            return oChequeHistorys;
        }

        

       

         
        #endregion
    }
}

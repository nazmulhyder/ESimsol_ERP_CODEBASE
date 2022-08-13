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
    public class EmployeeLoanHistoryService : MarshalByRefObject, IEmployeeLoanHistoryService
    {
        #region Private functions and declaration
        private EmployeeLoanHistory MapObject(NullHandler oReader)
        {
            EmployeeLoanHistory oEmployeeLoanHistory = new EmployeeLoanHistory();

            oEmployeeLoanHistory.ELHID = oReader.GetInt32("ELHID");
            oEmployeeLoanHistory.EmployeeLoanID = oReader.GetInt32("EmployeeLoanID");
            oEmployeeLoanHistory.Status = (EnumEmployeeLoanStatus)oReader.GetInt16("Status");
            oEmployeeLoanHistory.PreviousStatus = (EnumEmployeeLoanStatus)oReader.GetInt16("PreviousStatus");
            return oEmployeeLoanHistory;

        }

        private EmployeeLoanHistory CreateObject(NullHandler oReader)
        {
            EmployeeLoanHistory oEmployeeLoanHistory = MapObject(oReader);
            return oEmployeeLoanHistory;
        }

        private List<EmployeeLoanHistory> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLoanHistory> oEmployeeLoanHistorys = new List<EmployeeLoanHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLoanHistory oItem = CreateObject(oHandler);
                oEmployeeLoanHistorys.Add(oItem);
            }
            return oEmployeeLoanHistorys;
        }

        #endregion

        #region Interface implementation
        public EmployeeLoanHistoryService() { }

        public EmployeeLoanHistory IUD(EmployeeLoanHistory oEmployeeLoanHistory, int nDBOperation, Int64 nUserID)
        {


            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeLoanHistoryDA.IUD(tc, oEmployeeLoanHistory, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeLoanHistory = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeLoanHistory.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeLoanHistory.ELHID = 0;
                #endregion
            }
            return oEmployeeLoanHistory;
        }


        public EmployeeLoanHistory Get(int nELHID, Int64 nUserId)
        {
            EmployeeLoanHistory oEmployeeLoanHistory = new EmployeeLoanHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLoanHistoryDA.Get(nELHID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanHistory = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeLoanHistory", e);
                oEmployeeLoanHistory.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeLoanHistory;
        }

        public EmployeeLoanHistory Get(string sSQL, Int64 nUserId)
        {
            EmployeeLoanHistory oEmployeeLoanHistory = new EmployeeLoanHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLoanHistoryDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanHistory = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeLoanHistory", e);
                oEmployeeLoanHistory.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeLoanHistory;
        }

        public List<EmployeeLoanHistory> Gets(Int64 nUserID)
        {
            List<EmployeeLoanHistory> oEmployeeLoanHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLoanHistoryDA.Gets(tc);
                oEmployeeLoanHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeLoanHistory", e);
                #endregion
            }
            return oEmployeeLoanHistory;
        }

        public List<EmployeeLoanHistory> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeLoanHistory> oEmployeeLoanHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLoanHistoryDA.Gets(sSQL, tc);
                oEmployeeLoanHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeLoanHistory", e);
                #endregion
            }
            return oEmployeeLoanHistory;
        }

        #endregion


    }
}

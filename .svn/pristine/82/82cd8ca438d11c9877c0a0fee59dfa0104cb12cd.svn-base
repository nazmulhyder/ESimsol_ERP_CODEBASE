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
    public class EmployeeCardHistoryService : MarshalByRefObject, IEmployeeCardHistoryService
    {
        #region Private functions and declaration
        private EmployeeCardHistory MapObject(NullHandler oReader)
        {
            EmployeeCardHistory oEmployeeCardHistory = new EmployeeCardHistory();
            oEmployeeCardHistory.ECHID = oReader.GetInt32("EmployeeCardHistoryID");
            oEmployeeCardHistory.EmployeeCardID = oReader.GetInt32("EmployeeCardID");
            oEmployeeCardHistory.PreviousStatus = (EnumEmployeeCardStatus)oReader.GetInt32("PreviousStatus");
            oEmployeeCardHistory.CurrentStatus = (EnumEmployeeCardStatus)oReader.GetInt32("CurrentStatus");
            oEmployeeCardHistory.StatusChangeBy = oReader.GetString("UserName");
            oEmployeeCardHistory.StatusChangeDate = oReader.GetDateTime("DBServerDateTime");
            
            return oEmployeeCardHistory;
        }

        private EmployeeCardHistory CreateObject(NullHandler oReader)
        {
            EmployeeCardHistory oEmployeeCardHistory = MapObject(oReader);
            return oEmployeeCardHistory;
        }

        private List<EmployeeCardHistory> CreateObjects(IDataReader oReader)
        {
            List<EmployeeCardHistory> oEmployeeCardHistory = new List<EmployeeCardHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeCardHistory oItem = CreateObject(oHandler);
                oEmployeeCardHistory.Add(oItem);
            }
            return oEmployeeCardHistory;
        }

        #endregion

        #region Interface implementation
        public EmployeeCardHistoryService() { }

        public EmployeeCardHistory IUD(EmployeeCardHistory oEmployeeCardHistory, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (Employee oItem in oEmployeeCardHistory.Employees)
                {
                    

                    IDataReader reader;
                    reader = EmployeeCardHistoryDA.IUD(tc, oEmployeeCardHistory, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeCardHistory.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  

                #endregion
            }
            return oEmployeeCardHistory;
        }


        public EmployeeCardHistory Get(int nEmployeeCardHistoryID, Int64 nUserId)
        {
            EmployeeCardHistory oEmployeeCardHistory = new EmployeeCardHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeCardHistoryDA.Get(nEmployeeCardHistoryID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeCardHistory = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeCardHistory", e);
                oEmployeeCardHistory.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeCardHistory;
        }

        public EmployeeCardHistory Get(string sSql, Int64 nUserId)
        {
            EmployeeCardHistory oEmployeeCardHistory = new EmployeeCardHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeCardHistoryDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeCardHistory = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeCardHistory", e);
                oEmployeeCardHistory.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeCardHistory;
        }

        public List<EmployeeCardHistory> Gets(Int64 nUserID)
        {
            List<EmployeeCardHistory> oEmployeeCardHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeCardHistoryDA.Gets(tc);
                oEmployeeCardHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeCardHistory", e);
                #endregion
            }
            return oEmployeeCardHistory;
        }

        public List<EmployeeCardHistory> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeCardHistory> oEmployeeCardHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeCardHistoryDA.Gets(sSQL, tc);
                oEmployeeCardHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeCardHistory", e);
                #endregion
            }
            return oEmployeeCardHistory;
        }


        #endregion

    }
}

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
    public class AccountingSessionService : MarshalByRefObject, IAccountingSessionService
    {
        #region Private functions and declaration
        private AccountingSession MapObject(NullHandler oReader)
        {
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession.AccountingSessionID = oReader.GetInt32("AccountingSessionID");            
            oAccountingSession.SessionType = (EnumSessionType)oReader.GetInt32("SessionType");
            oAccountingSession.SessionCode = oReader.GetString("SessionCode");
            oAccountingSession.SessionName = oReader.GetString("SessionName");
            oAccountingSession.YearStatus = (EnumAccountYearStatus)oReader.GetInt32("YearStatus");
            oAccountingSession.StartDate = oReader.GetDateTime("StartDate");
            oAccountingSession.EndDate = oReader.GetDateTime("EndDate");
            oAccountingSession.LockDateTime = oReader.GetDateTime("LockDateTime");
            oAccountingSession.ActivationDateTime = oReader.GetDateTime("ActivationDateTime");
            oAccountingSession.ParentSessionID = oReader.GetInt32("ParentSessionID");
            oAccountingSession.SessionHierarchy = oReader.GetString("SessionHierarchy");            
            oAccountingSession.SessionID = oReader.GetInt32("SessionID");
            oAccountingSession.IsDateActivation = oReader.GetBoolean("IsDateActivation");
            oAccountingSession.WeekLyHolidays = oReader.GetString("WeekLyHolidays");
            return oAccountingSession;
        }

        private AccountingSession CreateObject(NullHandler oReader)
        {
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = MapObject(oReader);
            return oAccountingSession;
        }

        private List<AccountingSession> CreateObjects(IDataReader oReader)
        {
            List<AccountingSession> oAccountingSession = new List<AccountingSession>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AccountingSession oItem = CreateObject(oHandler);
                oAccountingSession.Add(oItem);
            }
            return oAccountingSession;
        }

        #endregion

        #region Interface implementation
        public AccountingSessionService() { }

        public AccountingSession Save(AccountingSession oAccountingSession, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oAccountingSession.AccountingSessionID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.AccountingSession, EnumRoleOperationType.Add);
                    reader = AccountingSessionDA.InsertUpdate(tc, oAccountingSession, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.AccountingSession, EnumRoleOperationType.Edit);
                    reader = AccountingSessionDA.InsertUpdate(tc, oAccountingSession, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountingSession = new AccountingSession();
                    oAccountingSession = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oAccountingSession = new AccountingSession();
                oAccountingSession.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save AccountingSession. Because of " + e.Message, e);
                #endregion
            }
            return oAccountingSession;
        }
        
        public AccountingSession LockUnLock(AccountingSession oAccountingSession, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AccountingSessionDA.LockUnLock(tc, oAccountingSession);               
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountingSession = new AccountingSession();
                    oAccountingSession = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                
                oAccountingSession = new AccountingSession();
                oAccountingSession.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save AccountingSession. Because of " + e.Message, e);
                #endregion
            }
            return oAccountingSession;
        }

        public AccountingSession DeclareNewAccountingYear(AccountingSession oAccountingSession, int nUserId)
        {
            int nBUID = 0;
            DateTime dStartDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;
            int nNewRunningSessionID = 0;
            int nPreRunningSessionID = 0;
            bool bIsOpenningBalanceTransfer = false;
            nNewRunningSessionID = oAccountingSession.AccountingSessionID;
            TransactionContext tc = null;
            IDataReader reader;
            NullHandler oReader = null;
            string sSQL = "";
            try
            {
                #region DeclareNewAccountingYear
                tc = TransactionContext.Begin(true);
                AccountingSessionDA.RestAccountOpeningBalanceTransfer(tc);
                AccountingSessionDA.RestAccountOpeningNeedTransfer(tc);                
                reader = AccountingSessionDA.DeclareNewAccountingYear(tc, oAccountingSession, nUserId);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    nPreRunningSessionID = oReader.GetInt32("PreRunningSessionID");
                    bIsOpenningBalanceTransfer = oReader.GetBoolean("IsOpenningTransfer");
                    dStartDate = oReader.GetDateTime("StartDate");
                    dEndDate = oReader.GetDateTime("EndDate");
                    nBUID = oReader.GetInt32("BUID");
                }
                reader.Close();
                tc.End();
                #endregion



                #region Get New Running Session
                tc = TransactionContext.Begin(true);
                sSQL = "SELECT * FROM View_AccountingSession WHERE AccountingSessionID=" + nNewRunningSessionID.ToString();
                reader = AccountingSessionDA.Gets(tc, sSQL);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountingSession = new AccountingSession();
                    oAccountingSession = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

                oAccountingSession.PreRunningSessionID = nPreRunningSessionID;
                oAccountingSession.IsOpeningTransfer = bIsOpenningBalanceTransfer;
                oAccountingSession.StartDate = dStartDate;
                oAccountingSession.EndDate = dEndDate;
                oAccountingSession.BUID = nBUID;
                #endregion
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oAccountingSession = new AccountingSession();
                oAccountingSession.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oAccountingSession;
        }

        public void TransferOpeningBalance(int nNewRunningSessionID, int nPreRunningSessionID, int nBusinessUnitID, int nAccountHeadID, int nSubledgerID, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AccountingSessionDA.TransferOpenningBalance(tc, nNewRunningSessionID, nPreRunningSessionID, nBusinessUnitID, nAccountHeadID, nSubledgerID, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                                
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Data Transfer. Because of " + e.Message, e);
                #endregion
            }
        }

        public AccountingSession AccountingYearClose(AccountingSession oAccountingSession, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AccountingSessionDA.AccountingYearClose(tc, oAccountingSession, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountingSession = new AccountingSession();
                    oAccountingSession = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oAccountingSession = new AccountingSession();
                oAccountingSession.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oAccountingSession;
        }

        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AccountingSession oAccountingSession = new AccountingSession();
                oAccountingSession.AccountingSessionID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.AccountingSession, EnumRoleOperationType.Delete);
                AccountingSessionDA.Delete(tc, oAccountingSession, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete AccountingSession. Because of " + e.Message, e);
                #endregion
            }
            return "Data Delete Successfully";
        }

        public AccountingSession Get(int id, int nUserId)
        {
            AccountingSession oAccountHead = new AccountingSession();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountingSessionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get AccountingSession", e);
                #endregion
            }

            return oAccountHead;
        }

        public AccountingSession GetSessionByDate(DateTime dSessionDate, int nUserId)
        {
            AccountingSession oAccountingSession = new AccountingSession();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountingSessionDA.GetSessionByDate(tc, dSessionDate);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountingSession = CreateObject(oReader);
                }
                reader.Close();
                if (oAccountingSession.AccountingSessionID <= 0)
                {
                    reader = AccountingSessionDA.Get(tc, 2);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oAccountingSession = CreateObject(oReader);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountingSession", e);
                #endregion
            }

            return oAccountingSession;
        }

        public List<AccountingSession> GetsTitleSessions(int nUserId)
        {
            List<AccountingSession> oAccountingSession = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountingSessionDA.GetsTitleSessions(tc);
                oAccountingSession = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountingSession", e);
                #endregion
            }

            return oAccountingSession;
        }

         public List<AccountingSession> Gets(int nUserId)
        {
            List<AccountingSession> oAccountingSession = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountingSessionDA.Gets(tc);
                oAccountingSession = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountingSession", e);
                #endregion
            }

            return oAccountingSession;
        }
         public AccountingSession GetOpenningAccountingYear(int nUserId)
        {
            AccountingSession oAccountHead = new AccountingSession();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountingSessionDA.GetOpenningAccountingYear(tc);
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
                throw new ServiceException("Failed to Get AccountingSession", e);
                #endregion
            }

            return oAccountHead;
        }        
        public AccountingSession GetRunningAccountingYear(int nUserId)
        {
            AccountingSession oAccountHead = new AccountingSession();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountingSessionDA.GetRunningAccountingYear(tc);
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
                throw new ServiceException("Failed to Get AccountingSession", e);
                #endregion
            }

            return oAccountHead;
        }
                
        public List<AccountingSession> GetRunningFreezeAccountingYear(int nUserId)
        {
            List<AccountingSession> oAccountingSession = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AccountingSessionDA.GetRunningFreezeAccountingYear(tc);
                oAccountingSession = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountingSession", e);
                #endregion
            }

            return oAccountingSession;
        }


        public List<AccountingSession> GetsAccountingYears(int nUserId)
        {
            List<AccountingSession> oAccountingSession = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AccountingSessionDA.GetsAccountingYears(tc);
                oAccountingSession = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountingSession", e);
                #endregion
            }

            return oAccountingSession;
        }

        public List<AccountingSession> Gets(string sSQL, int nUserId)
        {
            List<AccountingSession> oAccountingSession = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountingSessionDA.Gets(tc, sSQL);
                oAccountingSession = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountingSession", e);
                #endregion
            }

            return oAccountingSession;
        }
        #endregion
    }
}

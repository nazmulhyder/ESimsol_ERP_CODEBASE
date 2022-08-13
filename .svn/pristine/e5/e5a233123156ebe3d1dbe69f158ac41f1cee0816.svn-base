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
    public class TrailBalanceService : MarshalByRefObject, ITrailBalanceService
    {
        #region Private functions and declaration
        bool _bIsDateRange = false;
        private TrailBalance MapObject(NullHandler oReader)
        {
            TrailBalance oTrailBalance = new TrailBalance();
            oTrailBalance.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oTrailBalance.AccountCode = oReader.GetString("AccountCode");
            oTrailBalance.AccountHeadName = oReader.GetString("AccountHeadName");
            oTrailBalance.ComponentType = (EnumComponentType)oReader.GetInt32("ComponentType");
            oTrailBalance.AccountType = (EnumAccountType) oReader.GetInt32("AccountType");
            oTrailBalance.ParentAccountHeadID = oReader.GetInt32("ParentHeadID");
            oTrailBalance.OpenningBalance = oReader.GetDouble("OpenningBalance");
            oTrailBalance.DebitAmount = oReader.GetDouble("DebitAmount");
            oTrailBalance.CreditAmount = oReader.GetDouble("CreditAmount");
            oTrailBalance.ClosingBalance = oReader.GetDouble("ClosingBalance");
            return oTrailBalance;
        }

        private TrailBalance CreateObject(NullHandler oReader)
        {
            TrailBalance oTrailBalance = new TrailBalance();
            oTrailBalance = MapObject(oReader);
            return oTrailBalance;
        }

        private List<TrailBalance> CreateObjects(IDataReader oReader)
        {
            List<TrailBalance> oTrailBalance = new List<TrailBalance>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TrailBalance oItem = CreateObject(oHandler);
                oTrailBalance.Add(oItem);
            }
            return oTrailBalance;
        }

        #endregion

        #region Interface implementation
        public TrailBalanceService() { }
        public List<TrailBalance> Gets(int nAccountHead, int AccountTypeInInt, DateTime dStartDate, DateTime dEndDate, int nBusinessUnitID, int nUserID)
        {
            List<TrailBalance> oTrailBalance = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TrailBalanceDA.Gets(tc, nAccountHead, AccountTypeInInt, dStartDate, dEndDate, nBusinessUnitID);
                _bIsDateRange = true;
                oTrailBalance = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TrailBalance", e);
                #endregion
            }

            return oTrailBalance;
        }
        public List<TrailBalance> ProcessTrialBalance(DateTime dStartDate, DateTime dEndDate, string sStartLedgerCode, string sEndLedgerCode, string sSQL, int nUserID)
        {
            List<TrailBalance> oTrailBalances = new List<TrailBalance>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TrailBalanceDA.ProcessTrialBalance(tc, dStartDate, dEndDate, sStartLedgerCode, sEndLedgerCode, sSQL);                
                oTrailBalances = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTrailBalances = new List<TrailBalance>();
                TrailBalance oTrailBalance = new TrailBalance();
                oTrailBalance.ErrorMessage = e.Message;
                oTrailBalances.Add(oTrailBalance);

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get TrailBalance", e);
                #endregion
            }

            return oTrailBalances;
        }
        #endregion
    }
}

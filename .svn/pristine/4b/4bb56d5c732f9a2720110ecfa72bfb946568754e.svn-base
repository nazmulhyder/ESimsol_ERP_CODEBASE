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
    public class NegativeLedgerService : MarshalByRefObject, INegativeLedgerService
    {
        #region Private functions and declaration
        private NegativeLedger MapObject(NullHandler oReader)
        {
            NegativeLedger oNegativeLedger = new NegativeLedger();
            oNegativeLedger.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oNegativeLedger.AccountCode = oReader.GetString("AccountCode");
            oNegativeLedger.AccountHeadName = oReader.GetString("AccountHeadName");
            oNegativeLedger.ComponentType = (EnumComponentType)oReader.GetInt32("ComponentType");
            oNegativeLedger.ComponentTypeInInt = oReader.GetInt32("ComponentType");
            oNegativeLedger.AccountType = (EnumAccountType)oReader.GetInt32("AccountType");
            oNegativeLedger.AccountTypeInInt = oReader.GetInt32("AccountType");
            oNegativeLedger.OpenningBalance = oReader.GetDouble("OpenningBalance");
            oNegativeLedger.DebitAmount = oReader.GetDouble("DebitAmount");
            oNegativeLedger.CreditAmount = oReader.GetDouble("CreditAmount");
            oNegativeLedger.ClosingBalance = oReader.GetDouble("ClosingBalance");
            oNegativeLedger.AccountingSessionID = oReader.GetInt32("AccountingSessionID");
            oNegativeLedger.AccountingSessionName = oReader.GetString("AccountingSessionName");
            return oNegativeLedger;
        }
        private NegativeLedger CreateObject(NullHandler oReader)
        {
            NegativeLedger oNegativeLedger = new NegativeLedger();
            oNegativeLedger = MapObject(oReader);
            return oNegativeLedger;
        }

        private List<NegativeLedger> CreateObjects(IDataReader oReader)
        {
            List<NegativeLedger> oNegativeLedger = new List<NegativeLedger>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                NegativeLedger oItem = CreateObject(oHandler);
                oNegativeLedger.Add(oItem);
            }
            return oNegativeLedger;
        }
        #endregion

        #region Interface implementation
        public List<NegativeLedger> Gets(int nCompanyID, int nUserID)
        {
            List<NegativeLedger> oNegativeLedger = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = NegativeLedgerDA.Gets(tc, nCompanyID);
                oNegativeLedger = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Nagetive Ledger", e);
                #endregion
            }
            return oNegativeLedger;
        }
        #endregion
    }
}

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
    public class TrialBalance_CategorizedService : MarshalByRefObject, ITrialBalance_CategorizedService
    {
        #region Private functions and declaration
        bool _bIsDateRange = false;
        private TrialBalance_Categorized MapObject(NullHandler oReader)
        {
            TrialBalance_Categorized oTrialBalance_Categorized = new TrialBalance_Categorized();
            oTrialBalance_Categorized.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oTrialBalance_Categorized.AccountCode = oReader.GetString("AccountCode");
            oTrialBalance_Categorized.AccountHeadName = oReader.GetString("AccountHeadName");
            oTrialBalance_Categorized.ComponentType = (EnumComponentType)oReader.GetInt32("ComponentType");
            oTrialBalance_Categorized.AccountType = (EnumAccountType) oReader.GetInt32("AccountType");
            oTrialBalance_Categorized.ParentAccountHeadID = oReader.GetInt32("ParentHeadID");
            oTrialBalance_Categorized.OpenningBalance = oReader.GetDouble("OpenningBalance");
            oTrialBalance_Categorized.DebitAmount = oReader.GetDouble("DebitAmount");
            oTrialBalance_Categorized.CreditAmount = oReader.GetDouble("CreditAmount");
            oTrialBalance_Categorized.ClosingBalance = oReader.GetDouble("ClosingBalance");
            return oTrialBalance_Categorized;
        }

        private TrialBalance_Categorized CreateObject(NullHandler oReader)
        {
            TrialBalance_Categorized oTrialBalance_Categorized = new TrialBalance_Categorized();
            oTrialBalance_Categorized = MapObject(oReader);
            return oTrialBalance_Categorized;
        }

        private List<TrialBalance_Categorized> CreateObjects(IDataReader oReader)
        {
            List<TrialBalance_Categorized> oTrialBalance_Categorized = new List<TrialBalance_Categorized>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TrialBalance_Categorized oItem = CreateObject(oHandler);
                oTrialBalance_Categorized.Add(oItem);
            }
            return oTrialBalance_Categorized;
        }

        #endregion

        #region Interface implementation
        public TrialBalance_CategorizedService() { }
        public List<TrialBalance_Categorized> Gets(int nAccountHead, DateTime dStartDate, DateTime dEndDate, bool bIsApproved, int nCurrencyID, int nBusinessUnitID, int nUserID)
        {
            List<TrialBalance_Categorized> oTrialBalance_Categorized = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TrialBalance_CategorizedDA.Gets(tc, nAccountHead, dStartDate, dEndDate, bIsApproved, nCurrencyID, nBusinessUnitID);
                _bIsDateRange = true;
                oTrialBalance_Categorized = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TrialBalance_Categorized", e);
                #endregion
            }

            return oTrialBalance_Categorized;
        }
        public List<TrialBalance_Categorized> ProcessTrialBalance(DateTime dStartDate, DateTime dEndDate, string sStartLedgerCode, string sEndLedgerCode, string sSQL, int nUserID)
        {
            List<TrialBalance_Categorized> oTrialBalance_Categorizeds = new List<TrialBalance_Categorized>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TrialBalance_CategorizedDA.ProcessTrialBalance(tc, dStartDate, dEndDate, sStartLedgerCode, sEndLedgerCode, sSQL);                
                oTrialBalance_Categorizeds = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTrialBalance_Categorizeds = new List<TrialBalance_Categorized>();
                TrialBalance_Categorized oTrialBalance_Categorized = new TrialBalance_Categorized();
                oTrialBalance_Categorized.ErrorMessage = e.Message;
                oTrialBalance_Categorizeds.Add(oTrialBalance_Categorized);

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get TrialBalance_Categorized", e);
                #endregion
            }

            return oTrialBalance_Categorizeds;
        }
        #endregion
    }
}

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
    public class BalanceSheetService : MarshalByRefObject, IBalanceSheetService
    {
        #region Private functions and declaration
        private BalanceSheet MapObject(NullHandler oReader)
        {
            BalanceSheet oBalanceSheet = new BalanceSheet();
            oBalanceSheet.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oBalanceSheet.AccountCode = oReader.GetString("AccountCode");
            oBalanceSheet.AccountHeadName = oReader.GetString("AccountHeadName");
            oBalanceSheet.ParentAccountHeadID = oReader.GetInt32("ParentAccountHeadID");
            oBalanceSheet.ComponentType = (EnumComponentType)oReader.GetInt32("ComponentType");
            oBalanceSheet.ComponentTypeInt = oReader.GetInt32("ComponentType");
            oBalanceSheet.AccountType = (EnumAccountType)oReader.GetInt32("AccountType");
            oBalanceSheet.AccountTypeInt = oReader.GetInt32("AccountType");
            oBalanceSheet.OpenningBalance = oReader.GetDouble("OpenningBalance");
            oBalanceSheet.DebitTransaction = oReader.GetDouble("DebitTransaction");
            oBalanceSheet.CreditTransaction = oReader.GetDouble("CreditTransaction");
            oBalanceSheet.ClosingBalance = oReader.GetDouble("ClosingBalance");
            oBalanceSheet.Sequence = oReader.GetInt32("Sequence");
            return oBalanceSheet;
        }

        private BalanceSheet CreateObject(NullHandler oReader)
        {
            BalanceSheet oBalanceSheet = new BalanceSheet();
            oBalanceSheet = MapObject(oReader);
            return oBalanceSheet;
        }

        private List<BalanceSheet> CreateObjects(IDataReader oReader)
        {
            List<BalanceSheet> oBalanceSheet = new List<BalanceSheet>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BalanceSheet oItem = CreateObject(oHandler);
                oBalanceSheet.Add(oItem);
            }
            return oBalanceSheet;
        }

        #endregion

        #region Interface implementation
        public BalanceSheetService() { }

        public List<BalanceSheet> Gets(int nBUID, int nAccountType, DateTime dBalanceSheetStartDate, DateTime dBalanceSheetUptoDate, int nParentAccountHeadID, bool bIsApproved, int nUserID)
        {
            List<BalanceSheet> oBalanceSheet = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BalanceSheetDA.Gets(tc, nBUID, nAccountType, dBalanceSheetStartDate, dBalanceSheetUptoDate, nParentAccountHeadID, bIsApproved);
                oBalanceSheet = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BalanceSheet", e);
                #endregion
            }

            return oBalanceSheet;
        }

        public List<BalanceSheet> GetsForRationAnalysis(int nBUID, DateTime dBalanceSheetDate, int nRatioAnalysisID, bool bIsDivisible, int nParentAccountHeadID, int nUserID)
        {
            List<BalanceSheet> oBalanceSheet = new List<BalanceSheet>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BalanceSheetDA.GetsForRationAnalysis(tc, nBUID, dBalanceSheetDate, nRatioAnalysisID, bIsDivisible, nParentAccountHeadID);
                oBalanceSheet = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Ration Analysis Notes", e);
                #endregion
            }

            return oBalanceSheet;
        }

        public List<BalanceSheet> ProcessBalanceSheet(DateTime dBalanceSheetDate, string sStartBusinessCode, string sEndBusinessCode, int nUserID)
        {
            List<BalanceSheet> oBalanceSheet = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BalanceSheetDA.ProcessBalanceSheet(tc, dBalanceSheetDate, sStartBusinessCode, sEndBusinessCode);
                oBalanceSheet = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BalanceSheet", e);
                #endregion
            }

            return oBalanceSheet;
        }
        #endregion
    }
}

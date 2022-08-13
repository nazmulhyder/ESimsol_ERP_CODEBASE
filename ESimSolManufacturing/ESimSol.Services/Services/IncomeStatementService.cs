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
    public class IncomeStatementService : MarshalByRefObject, IIncomeStatementService
    {
        #region Private functions and declaration
        private IncomeStatement MapObject(NullHandler oReader)
        {
            IncomeStatement oIncomeStatement = new IncomeStatement();
            oIncomeStatement.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oIncomeStatement.AccountCode = oReader.GetString("AccountCode");
            oIncomeStatement.AccountHeadName = oReader.GetString("AccountHeadName");
            oIncomeStatement.ParentAccountHeadID = oReader.GetInt32("ParentAccountHeadID");
            oIncomeStatement.ComponentType = (EnumComponentType)oReader.GetInt32("ComponentType");
            oIncomeStatement.AccountType = (EnumAccountType)oReader.GetInt32("AccountType");
            oIncomeStatement.OpenningBalance = oReader.GetDouble("OpenningBalance");
            oIncomeStatement.DebitTransaction = oReader.GetDouble("DebitTransaction");
            oIncomeStatement.CreditTransaction = oReader.GetDouble("CreditTransaction");
            oIncomeStatement.ClosingBalance = oReader.GetDouble("ClosingBalance");
            oIncomeStatement.CISSetup = (EnumCISSetup)oReader.GetInt32("CISSetup");
            oIncomeStatement.PurchaseCreditTransaction = oReader.GetDouble("PurchaseCreditTransaction");
            return oIncomeStatement;
        }

        private IncomeStatement CreateObject(NullHandler oReader)
        {
            IncomeStatement oIncomeStatement = new IncomeStatement();
            oIncomeStatement = MapObject(oReader);
            return oIncomeStatement;
        }

        private List<IncomeStatement> CreateObjects(IDataReader oReader)
        {
            List<IncomeStatement> oIncomeStatement = new List<IncomeStatement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                IncomeStatement oItem = CreateObject(oHandler);
                oIncomeStatement.Add(oItem);
            }
            return oIncomeStatement;
        }

        #endregion

        #region Interface implementation
        public IncomeStatementService() { }

        public List<IncomeStatement> Gets(int nBUID, DateTime dStartDate, DateTime dEndDate, int ParentHeadID, int nAccountTypeInInt, int nUserID)
        {
            List<IncomeStatement> oIncomeStatement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = IncomeStatementDA.Gets(tc, nBUID, dStartDate, dEndDate, ParentHeadID, nAccountTypeInInt);
                oIncomeStatement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get IncomeStatement", e);
                #endregion
            }

            return oIncomeStatement;
        }
        public List<IncomeStatement> ProcessIncomeStatement(DateTime dStartDate, DateTime dEndDate, int nUserID)
        {
            List<IncomeStatement> oIncomeStatements = new List<IncomeStatement>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = IncomeStatementDA.ProcessIncomeStatement(tc, dStartDate, dEndDate);
                oIncomeStatements = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oIncomeStatements = new List<IncomeStatement>();
                IncomeStatement oIncomeStatement = new IncomeStatement();
                oIncomeStatement.ErrorMessage = e.Message;
                oIncomeStatements.Add(oIncomeStatement);

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get IncomeStatement", e);
                #endregion
            }

            return oIncomeStatements;
        }
        #endregion
    }
}

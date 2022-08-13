using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class BankReconcilationStatementService : MarshalByRefObject, IBankReconcilationStatementService
    {
        #region Private functions and declaration

        private BankReconcilationStatement MapObject(NullHandler oReader)
        {
            BankReconcilationStatement oBankReconcilationStatement = new BankReconcilationStatement();
            oBankReconcilationStatement.VoucherID = oReader.GetInt32("VoucherID");
            oBankReconcilationStatement.VoucherNo = oReader.GetString("VoucherNo");
            oBankReconcilationStatement.BUID = oReader.GetInt32("BUID");
            oBankReconcilationStatement.OperationHeadName = oReader.GetString("OperationHeadName");
            oBankReconcilationStatement.PaymentDate = oReader.GetDateTime("PaymentDate");
            oBankReconcilationStatement.DataType = oReader.GetInt32("DataType");
            oBankReconcilationStatement.RefAmount = oReader.GetDouble("RefAmount");
            oBankReconcilationStatement.Narration = oReader.GetString("Narration");
            oBankReconcilationStatement.ChequeNo = oReader.GetString("ChequeNo");
            oBankReconcilationStatement.PartyName = oReader.GetString("PartyName");

            return oBankReconcilationStatement;
        }

        private BankReconcilationStatement CreateObject(NullHandler oReader)
        {
            BankReconcilationStatement oBankReconcilationStatement = new BankReconcilationStatement();
            oBankReconcilationStatement = MapObject(oReader);
            return oBankReconcilationStatement;
        }

        private List<BankReconcilationStatement> CreateObjects(IDataReader oReader)
        {
            List<BankReconcilationStatement> oBankReconcilationStatement = new List<BankReconcilationStatement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BankReconcilationStatement oItem = CreateObject(oHandler);
                oBankReconcilationStatement.Add(oItem);
            }
            return oBankReconcilationStatement;
        }

        #endregion

        #region Interface implementation

        public List<BankReconcilationStatement> GetBankReconcilationStatements(BankReconcilationStatement oBankReconcilationStatement, Int64 nUserID)
        {
            //reader = BankReconcilationStatementDA.GetBankReconcilationStatements(tc, oBankReconcilationStatement);
            List<BankReconcilationStatement> oBankReconcilationStatements = new List<BankReconcilationStatement>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                //reader = BankReconcilationStatementDA.Gets(tc);
                reader = BankReconcilationStatementDA.GetBankReconcilationStatements(tc, oBankReconcilationStatement);
                oBankReconcilationStatements = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                BankReconcilationStatement ooBankReconcilationStatement = new BankReconcilationStatement();
                ooBankReconcilationStatement.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oBankReconcilationStatements;
        }


        public BankReconcilationStatement Get(int id, Int64 nUserId)
        {
            BankReconcilationStatement oBankReconcilationStatement = new BankReconcilationStatement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BankReconcilationStatementDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankReconcilationStatement = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BankReconcilationStatement", e);
                #endregion
            }
            return oBankReconcilationStatement;
        }

        public List<BankReconcilationStatement> Gets(Int64 nUserID)
        {
            List<BankReconcilationStatement> oBankReconcilationStatements = new List<BankReconcilationStatement>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BankReconcilationStatementDA.Gets(tc);
                oBankReconcilationStatements = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                BankReconcilationStatement oBankReconcilationStatement = new BankReconcilationStatement();
                oBankReconcilationStatement.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oBankReconcilationStatements;
        }

        public List<BankReconcilationStatement> Gets(string sSQL, Int64 nUserID)
        {
            List<BankReconcilationStatement> oBankReconcilationStatements = new List<BankReconcilationStatement>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BankReconcilationStatementDA.Gets(tc, sSQL);
                oBankReconcilationStatements = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankReconcilationStatement", e);
                #endregion
            }
            return oBankReconcilationStatements;
        }

        #endregion
    }

}

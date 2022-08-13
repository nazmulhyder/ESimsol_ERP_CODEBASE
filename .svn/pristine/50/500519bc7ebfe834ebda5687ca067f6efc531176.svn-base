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
    public class StatementService : MarshalByRefObject, IStatementService
    {
        #region Private functions and declaration
        private Statement MapObject(NullHandler oReader)
        {
            Statement oStatement = new Statement();
            oStatement.LedgerGroupSetupID = oReader.GetInt32("LedgerGroupSetupID");
            oStatement.OCSID = oReader.GetInt32("OCSID");
            oStatement.GroupName = oReader.GetString("LedgerGroupSetupName");
            oStatement.BalanceAmount = oReader.GetDouble("BalanceAmount");
            oStatement.IsDr = oReader.GetBoolean("IsDr");
            oStatement.OpeningBalance = oReader.GetDouble("OpeningBalance");
            return oStatement;
        }

        private Statement CreateObject(NullHandler oReader)
        {
            Statement oStatement = new Statement();
            oStatement = MapObject(oReader);
            return oStatement;
        }

        private List<Statement> CreateObjects(IDataReader oReader)
        {
            List<Statement> oStatement = new List<Statement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Statement oItem = CreateObject(oHandler);
                oStatement.Add(oItem);
            }
            return oStatement;
        }

        #endregion

        public List<Statement> Gets(int nStatementSetupID, DateTime dstartDate, DateTime dendDate, int nBUID, int nUserId)
        {
            List<Statement> oStatements = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = StatementDA.Gets(tc, nStatementSetupID, dstartDate, dendDate, nBUID);
                oStatements = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oStatements;
        }
    }
}

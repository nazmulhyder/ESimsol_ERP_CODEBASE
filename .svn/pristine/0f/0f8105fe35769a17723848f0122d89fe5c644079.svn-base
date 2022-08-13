using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class StatementNoteDA
    {
        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nStatementSetupID, DateTime dstartDate, DateTime dendDate, int nBUID, int nAccountHeadID, bool bIsDebit)
        {
            return tc.ExecuteReader("EXECUTE [SP_AccountingStatementLinkUp] %n, %n, %b, %d, %d, %n", nStatementSetupID, nAccountHeadID, bIsDebit, dstartDate, dendDate, nBUID);
        }
        #endregion
    }
}

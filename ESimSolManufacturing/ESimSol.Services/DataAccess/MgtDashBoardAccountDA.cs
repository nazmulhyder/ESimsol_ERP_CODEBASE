using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class MgtDashBoardAccountDA
    {
        #region Get & Exist Function        
        public static IDataReader Gets(TransactionContext tc, MgtDashBoardAccount oMgtDashBoardAccount)
        {
            return tc.ExecuteReader("EXEC [SP_MgtDashBoardAccounts]" + "%n,%d", oMgtDashBoardAccount.BUID, oMgtDashBoardAccount.ReportDate);
        }
        #endregion
    }

}

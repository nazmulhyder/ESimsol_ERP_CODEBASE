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
    public class DispoHWDA
    {
        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL, int nRptType)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_DispoHW]"
                                   + "%s,%n", sSQL, nRptType);
        }

        #endregion
    }

}

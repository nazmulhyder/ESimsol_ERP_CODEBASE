using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class RPT_LotTrackingsDA
    {
        public RPT_LotTrackingsDA() { }

        #region Insert Update Delete Function
        
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL, int nReportType, int nBUID, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [sp_RPT_LotTrackings]" + "%s, %n, %n", sSQL, nBUID, nReportType);
        }
        #endregion
    }  
}


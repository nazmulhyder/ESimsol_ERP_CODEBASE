using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class rptMPRToGRNDA
    {
        public rptMPRToGRNDA() { }

        #region Insert Update Delete Function
        public static IDataReader Gets(TransactionContext tc, int nBUID, DateTime StartDate, DateTime EndDate, string sPRNo)
        {
            return tc.ExecuteReader("EXEC [SP_MPRToGRN_Report]"  + "%n, %d, %d, %s",
                                       nBUID, StartDate, EndDate, sPRNo);
        }

        #endregion

        
    }

}

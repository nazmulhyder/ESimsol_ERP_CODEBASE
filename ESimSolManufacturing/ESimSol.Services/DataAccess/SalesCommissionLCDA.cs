using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class SalesCommissionLCDA
    {
        public SalesCommissionLCDA() { }

        #region CreatePayable
        public static IDataReader IUD(TransactionContext tc, SalesCommissionLC oSalesCommissionLC, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalesCommission_Payable] %n,%n,%s ,%n ,%n", oSalesCommissionLC.ExportLCID, oSalesCommissionLC.ExportPIID, "", nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetByExportPIID(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesCommissionLC WHERE ExportPIID=%n", nID);
        }
        #endregion

    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SalarySheetPropertyDA
    {
        public SalarySheetPropertyDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, SalarySheetProperty oSalarySheetProperty, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalarySheetProperty]" + "%n, %n, %n, %b, %n, %n", oSalarySheetProperty.SalarySheetPropertyID, oSalarySheetProperty.SalarySheetFormatProperty, oSalarySheetProperty.PropertyFor, oSalarySheetProperty.IsActive, nUserID, nDBOperation);
        }

        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM SalarySheetProperty WHERE SalarySheetPropertyID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

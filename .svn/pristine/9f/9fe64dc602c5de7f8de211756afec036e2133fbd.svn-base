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
    public class ITaxLedgerDA
    {
        public ITaxLedgerDA() { }

        #region Insert Update Delete Function

        public static IDataReader ITaxProcess(int nEmployeeID, int nITaxAssessmentYearID, bool IsConsiderMaxRebate, TransactionContext tc, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_ITax] %n,%n,%b,%n",
                   nITaxAssessmentYearID, nEmployeeID, IsConsiderMaxRebate, nUserID);
        }

        public static IDataReader ITaxReProcess(int nEmployeeID, int nITaxAssessmentYearID, bool IsConsiderMaxRebate, DateTime dtDate, TransactionContext tc, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_ITax_Reprocess] %n,%n,%b,%d,%n",
                   nITaxAssessmentYearID, nEmployeeID, IsConsiderMaxRebate,dtDate, nUserID);
        }

        public static void ITaxLedger_Delete(string sITaxLedgerIDs, TransactionContext tc)
        {
             tc.ExecuteNonQuery("EXEC [SP_Delete_ITaxLedger] %s",
                   sITaxLedgerIDs);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nITaxLedgerID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITaxLedger WHERE ITaxLedgerID=%n", nITaxLedgerID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITaxLedger");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}

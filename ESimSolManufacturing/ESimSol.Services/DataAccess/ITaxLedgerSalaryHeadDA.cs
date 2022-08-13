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
    public class ITaxLedgerSalaryHeadDA
    {
        public ITaxLedgerSalaryHeadDA() { }

        #region Insert Update Delete Function

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nITaxLedgerSalaryHeadID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITaxLedgerSalaryHead WHERE ITaxLedgerSalaryHeadID=%n", nITaxLedgerSalaryHeadID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITaxLedgerSalaryHead");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

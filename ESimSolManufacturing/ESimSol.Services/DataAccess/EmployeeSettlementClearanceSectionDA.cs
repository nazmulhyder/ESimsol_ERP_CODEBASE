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
    public class EmployeeSettlementClearanceSectionDA
    {
        public EmployeeSettlementClearanceSectionDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeSettlementClearanceSection oEmployeeSettlementClearanceSection, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeSettlementClearanceSection] %n,%s,%n,%n",
                   oEmployeeSettlementClearanceSection.ESCSID, oEmployeeSettlementClearanceSection.Name,
                   nUserID, nDBOperation);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nESCSID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeSettlementClearanceSection WHERE ESCSID=%n", nESCSID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeSettlementClearanceSection");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}

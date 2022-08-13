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
    public class EmployeeNomineeDA
    {
        public EmployeeNomineeDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EmployeeNominee oEN, int nEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeNominee]"
                                    + "%n, %n, %u, %u,%u, %s,%u,%n,%n,%n",
                                    oEN.ENID, oEN.EmployeeID, oEN.Name,
                                    oEN.Address, oEN.ContactNo, oEN.Email,
                                    oEN.Relation, oEN.Percentage,
                                    nUserID, nEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)//EmployeeNomineeID
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeNominee WHERE ENID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nEmployeeID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeNominee WHERE EmployeeID=%n", nEmployeeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

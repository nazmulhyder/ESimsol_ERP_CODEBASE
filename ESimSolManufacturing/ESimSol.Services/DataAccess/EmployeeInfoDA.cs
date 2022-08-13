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
    public class EmployeeInfoDA
    {
        public EmployeeInfoDA() { }

        #region Insert Update Delete Function


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader SearchProfile(TransactionContext tc, int nEmployeeID)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_EmployeeProfile] " + "%n",
                   nEmployeeID
                   );
        }

        #endregion
    }
}


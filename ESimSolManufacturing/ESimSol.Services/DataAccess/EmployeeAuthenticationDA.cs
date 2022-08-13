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
    public class EmployeeAuthenticationDA
    {
        public EmployeeAuthenticationDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeAuthentication oEmployeeAuthentication, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeAuthentication] %n,%n,%s,%s,%b,%n,%n",
                   oEmployeeAuthentication.EmployeeAuthenticationID,
                   oEmployeeAuthentication.EmployeeID,
                   oEmployeeAuthentication.CardNo,
                   oEmployeeAuthentication.Password,
                   oEmployeeAuthentication.IsActive,
                   nUserID, nDBOperation);
          
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, int nEmployeeAuthenticationID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeAuthentication WHERE EmployeeAuthenticationID=%n", nEmployeeAuthenticationID);
        }
        public static IDataReader Gets(TransactionContext tc,int nEmployeeID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeAuthentication WHERE EmployeeID=%n",nEmployeeID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}

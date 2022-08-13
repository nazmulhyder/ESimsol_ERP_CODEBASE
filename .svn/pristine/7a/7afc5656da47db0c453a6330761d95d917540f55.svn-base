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
    public class EmployeeCodeDA
    {
        public EmployeeCodeDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeCode oEmployeeCode, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeCode] %n, %n, %n, %n, %s,%n, %n",
                   oEmployeeCode.EmployeeCodeID, oEmployeeCode.DRPID, oEmployeeCode.DesignationID, oEmployeeCode.CompanyID,oEmployeeCode.EmployeeCodeDetailsInString,
                   nUserID, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nEmployeeCodeID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeCode WHERE EmployeeCodeID=%n", nEmployeeCodeID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeCode");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

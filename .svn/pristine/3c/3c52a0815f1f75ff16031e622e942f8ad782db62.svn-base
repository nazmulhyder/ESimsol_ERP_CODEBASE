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
    public class EmployeeCodeDetailDA
    {
        public EmployeeCodeDetailDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeCodeDetail oEmployeeCodeDetail, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeCodeDetail] %n, %n, %n, %n, %s,%n, %n",
                   oEmployeeCodeDetail.ECDID,
                   nUserID, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nEmployeeCodeDetailID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeCodeDetail WHERE EmployeeCodeDetailID=%n", nEmployeeCodeDetailID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeCodeDetail");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

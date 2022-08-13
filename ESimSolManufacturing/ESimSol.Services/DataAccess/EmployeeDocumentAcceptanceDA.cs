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
    public class EmployeeDocumentAcceptanceDA
    {
        public EmployeeDocumentAcceptanceDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeDocumentAcceptance oEmployeeDocumentAcceptance, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeDocumentAcceptance]" + "%n, %n, %n"
                , oEmployeeDocumentAcceptance.EDAID
                , oEmployeeDocumentAcceptance.EmployeeID
                , nUserID
            );
        }

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
        #endregion
    }
}



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
    public class EmployeeReferenceDA
    {
        public EmployeeReferenceDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EmployeeReference oER, int nEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeReference]"
                                    + "%n, %n, %u, %u, %u, %u, %u, %u, %u, %n, %n",
                                    oER.EmployeeReferenceID, oER.EmployeeID, oER.Name, oER.Designation, oER.Organization, oER.ContactNo, oER.Address, oER.Relation, oER.Description, nUserID, nEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)//EmployeeReferenceID
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeReference WHERE EmployeeReferenceID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nEmployeeID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeReference WHERE EmployeeID=%n", nEmployeeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

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
    public class EmployeeRecommendedDesignationDA
    {
        public EmployeeRecommendedDesignationDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeRecommendedDesignation oEmployeeRecommendedDesignation, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeRecommendedDesignation] %n,%n,%n,%n,%n,%n",
                   oEmployeeRecommendedDesignation.ARDID, oEmployeeRecommendedDesignation.EmployeeID,
                   oEmployeeRecommendedDesignation.DepartmentID, oEmployeeRecommendedDesignation.DesignationID, nUserID, nDBOperation);


        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nARDID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeRecommendedDesignation WHERE ARDID=%n", nARDID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(int nID ,TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeRecommendedDesignation WHERE EmployeeID=%n" , nID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

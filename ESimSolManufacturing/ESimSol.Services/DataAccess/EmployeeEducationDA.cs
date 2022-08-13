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
    public class EmployeeEducationDA
    {
        public EmployeeEducationDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EmployeeEducation oEE, int nEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeEducation]"
                                    + "%n, %n,%n, %u,%u, %u, %n, %u, %u, %u, %u, %n, %n",
                                    oEE.EmployeeEducationID, oEE.EmployeeID,oEE.Sequence, oEE.Degree, oEE.Major, oEE.Session, oEE.PassingYear, oEE.BoardUniversity, oEE.Institution, oEE.Result, oEE.Country, nUserID, nEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)//EmployeeEducationID
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeEducation WHERE EmployeeEducationID=%n ORDER BY Sequence ASC", nID);
        }

        public static IDataReader Gets(TransactionContext tc,int nEmployeeID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeEducation WHERE EmployeeID=%n ORDER BY Sequence ASC", nEmployeeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }        
        #endregion
    }  
}

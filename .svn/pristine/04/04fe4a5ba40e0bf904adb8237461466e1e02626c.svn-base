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
    public class EmployeeExperienceDA
    {
        public EmployeeExperienceDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EmployeeExperience oEE, int nEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeExperience]"
                                    + "%n, %n, %u, %u, %u, %u, %d, %n,%d,%n, %u, %u, %n, %n",
                                    oEE.EmployeeExperienceID, oEE.EmployeeID, oEE.Organization, oEE.OrganizationType, oEE.Address, oEE.Designation, oEE.StartDate,oEE.StartDateExFormatType, oEE.EndDate,oEE.EndDateExFormatType, oEE.Duration, oEE.MajorResponsibility, nUserID, nEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)//EmployeeExperienceID
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeExperience WHERE EmployeeExperienceID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nEmployeeID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeExperience WHERE EmployeeID=%n", nEmployeeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

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

    public class DepartmentRequirementPolicyDA
    {

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DepartmentRequirementPolicy oDepartmentRequirementPolicy, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DepartmentRequirementPolicy] %n, %n, %n, %n, %n, %s, %n, %n, %n, %n", oDepartmentRequirementPolicy.DepartmentRequirementPolicyID, oDepartmentRequirementPolicy.BusinessUnitID, oDepartmentRequirementPolicy.CompanyID, oDepartmentRequirementPolicy.LocationID, oDepartmentRequirementPolicy.DepartmentID, oDepartmentRequirementPolicy.Description, oDepartmentRequirementPolicy.HeadCount, oDepartmentRequirementPolicy.Budget, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, DepartmentRequirementPolicy oDepartmentRequirementPolicy, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DepartmentRequirementPolicy] %n, %n, %n, %n, %n, %s, %n, %n, %n, %n", oDepartmentRequirementPolicy.DepartmentRequirementPolicyID, oDepartmentRequirementPolicy.BusinessUnitID, oDepartmentRequirementPolicy.CompanyID, oDepartmentRequirementPolicy.LocationID, oDepartmentRequirementPolicy.DepartmentID, oDepartmentRequirementPolicy.Description, oDepartmentRequirementPolicy.HeadCount, oDepartmentRequirementPolicy.Budget, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DepartmentRequirementPolicy");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }

}

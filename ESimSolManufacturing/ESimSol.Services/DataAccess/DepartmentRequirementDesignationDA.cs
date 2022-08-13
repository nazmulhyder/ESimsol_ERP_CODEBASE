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

    public class DepartmentRequirementDesignationDA
    {

        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, DepartmentRequirementDesignation oDepartmentRequirementDesignation, Int64 nUserID, EnumDBOperation eEnumDBOperation, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DepartmentRequirementDesignation] %n, %n, %n, %n, %n, %n, %n, %s", 
                            oDepartmentRequirementDesignation.DepartmentRequirementDesignationID, oDepartmentRequirementDesignation.DepartmentRequirementPolicyID, oDepartmentRequirementDesignation.DesignationID, oDepartmentRequirementDesignation.HRResponsibilityID, oDepartmentRequirementDesignation.RequiredPerson, nUserID, (int)eEnumDBOperation, sIDs);
        }
        public static void Delete(TransactionContext tc, DepartmentRequirementDesignation oDepartmentRequirementDesignation, Int64 nUserID, EnumDBOperation eEnumDBOperation, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DepartmentRequirementDesignation] %n, %n, %n, %n, %n, %n, %n, %s",
                            oDepartmentRequirementDesignation.DepartmentRequirementDesignationID, oDepartmentRequirementDesignation.DepartmentRequirementPolicyID, oDepartmentRequirementDesignation.DesignationID, oDepartmentRequirementDesignation.HRResponsibilityID, oDepartmentRequirementDesignation.RequiredPerson, nUserID, (int)eEnumDBOperation, sIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DepartmentRequirementDesignation WHERE DepartmentRequirementDesignationID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DepartmentRequirementDesignation ORDER BY DepartmentRequirementPolicyID, DesignationSequence ASC");
        }
        public static IDataReader Gets(TransactionContext tc, int nDepartmentRequirementPolicyID, bool bIsShiftOrder)
        {
            if (bIsShiftOrder)
            {
                return tc.ExecuteReader("SELECT * FROM View_DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID =%n ORDER BY ShiftSequence", nDepartmentRequirementPolicyID);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID =%n ORDER BY DesignationSequence", nDepartmentRequirementPolicyID);
            }
            
        }

        public static IDataReader GetsPolicy(TransactionContext tc, int nDepartmentRequirementPolicyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID =%n ORDER BY DepartmentRequirementPolicyID ASC", nDepartmentRequirementPolicyID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static bool IsInEmployeeOfficial(string sSQL, TransactionContext tc)
        {
            object obj = tc.ExecuteScalar(sSQL);
            if (obj == null)
            {
                return false;
            }
            else
            {
                int n = Convert.ToInt32(obj);
                if (n > 0) return true;
                else
                    return false;
            }
        }
        #endregion
    }
}

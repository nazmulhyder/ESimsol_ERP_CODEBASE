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
    public class DesignationResponsibilityDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, DesignationResponsibility oDesignationResponsibility, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DesignationResponsibility] %n, %n, %n, %n, %n, %n", 
                oDesignationResponsibility.DesignationResponsibilityID, oDesignationResponsibility.DRPID, oDesignationResponsibility.DesignationID, oDesignationResponsibility.HRResponsibilityID, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DesignationResponsibility oDesignationResponsibility, int nDepartmentRequirementPolicyID, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DesignationResponsibility] %n, %n, %n, %n, %n, %n",
                oDesignationResponsibility.DesignationResponsibilityID, oDesignationResponsibility.DRPID, oDesignationResponsibility.DesignationID, oDesignationResponsibility.HRResponsibilityID, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DesignationResponsibility WHERE DesignationResponsibilityID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DesignationResponsibility");
        }
        public static IDataReader GetsByPolicy(TransactionContext tc, int nDepartmentRequirementPolicyID)
        {
            return tc.ExecuteReader("SELECT 0 AS DesignationResponsibilityID, 0 AS DRPID, TT.HRResponsibilityID, TT.HRResponsibilityCode, TT.HRResponsibilityText, TT.DesignationID FROM View_DesignationResponsibility AS TT WHERE TT.DRPID IN (SELECT DRD.DepartmentRequirementPolicyID FROM DepartmentRequirementDesignation AS DRD WHERE DRD.DepartmentRequirementPolicyID=%n) GROUP BY TT.DesignationID, TT.HRResponsibilityID, TT.HRResponsibilityText, TT.HRResponsibilityCode", nDepartmentRequirementPolicyID);
        }
        public static IDataReader Gets(TransactionContext tc, int nDepartmentRequirementDesignationID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DesignationResponsibility WHERE DepartmentRequirementDesignationID=%n", nDepartmentRequirementDesignationID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

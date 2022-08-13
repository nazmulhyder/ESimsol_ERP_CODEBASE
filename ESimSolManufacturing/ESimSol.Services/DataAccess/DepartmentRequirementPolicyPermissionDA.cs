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
    public class DepartmentRequirementPolicyPermissionDA
    {
        #region Insert Update Delete Function
        public static void ConfirmMenuPermission(TransactionContext tc, int nUserID, string sSelectedMenuKeys)
        {
            tc.ExecuteNonQuery("EXEC SP_IUD_UserDRPPermission %n, %s", nUserID, sSelectedMenuKeys);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(string sSql, TransactionContext tc)
        {
            return tc.ExecuteReader(sSql);
        }
        #endregion
    }
}

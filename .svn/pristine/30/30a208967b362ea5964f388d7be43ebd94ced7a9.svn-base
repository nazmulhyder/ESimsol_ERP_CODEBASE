using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class DepartmentRequirementPolicyPermissionService : MarshalByRefObject, IDepartmentRequirementPolicyPermissionService
    {
        #region Private functions and declaration
        private DepartmentRequirementPolicyPermission MapObject(NullHandler oReader)
        {
            DepartmentRequirementPolicyPermission oDepartmentRequirementPolicyPermission = new DepartmentRequirementPolicyPermission();
            oDepartmentRequirementPolicyPermission.DRPPID = oReader.GetInt32("DRPPID");
            oDepartmentRequirementPolicyPermission.DRPID = oReader.GetInt32("DRPID");
            oDepartmentRequirementPolicyPermission.UserID = oReader.GetInt32("UserID");
            oDepartmentRequirementPolicyPermission.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oDepartmentRequirementPolicyPermission.LocationID = oReader.GetInt32("LocationID");
            oDepartmentRequirementPolicyPermission.DepartmentID = oReader.GetInt32("DepartmentID");
            oDepartmentRequirementPolicyPermission.InactiveDate = oReader.GetDateTime("InactiveDate");
            return oDepartmentRequirementPolicyPermission;
        }

        private DepartmentRequirementPolicyPermission CreateObject(NullHandler oReader)
        {
            DepartmentRequirementPolicyPermission oDepartmentRequirementPolicyPermission = MapObject(oReader);
            return oDepartmentRequirementPolicyPermission;
        }

        private List<DepartmentRequirementPolicyPermission> CreateObjects(IDataReader oReader)
        {
            List<DepartmentRequirementPolicyPermission> oDepartmentRequirementPolicyPermission = new List<DepartmentRequirementPolicyPermission>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DepartmentRequirementPolicyPermission oItem = CreateObject(oHandler);
                oDepartmentRequirementPolicyPermission.Add(oItem);
            }
            return oDepartmentRequirementPolicyPermission;
        }

        #endregion

        #region Interface implementation
        public DepartmentRequirementPolicyPermissionService() { }
        public List<DepartmentRequirementPolicyPermission> Gets(string sSql, Int64 nUserID)
        {
            List<DepartmentRequirementPolicyPermission> oDepartmentRequirementPolicyPermission = new List<DepartmentRequirementPolicyPermission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentRequirementPolicyPermissionDA.Gets(sSql,tc);
                oDepartmentRequirementPolicyPermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DepartmentRequirementPolicyPermission", e);
                #endregion
            }
            return oDepartmentRequirementPolicyPermission;
        }

        public bool ConfirmDRPPermission(int nUserID, string sSelectedMenuKeys, Int64 nCurrentUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DepartmentRequirementPolicyPermissionDA.ConfirmMenuPermission(tc, nUserID, sSelectedMenuKeys);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to User Menu configure. Because of " + e.Message, e);
                #endregion
            }
            return true;
        }
        #endregion
    }
}

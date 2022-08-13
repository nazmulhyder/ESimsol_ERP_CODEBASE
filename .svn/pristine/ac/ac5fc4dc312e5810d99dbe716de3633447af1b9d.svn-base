using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region DepartmentRequirementPolicyPermission

    public class DepartmentRequirementPolicyPermission : BusinessObject
    {
        public DepartmentRequirementPolicyPermission()
        {
            DRPPID = 0;
            DRPID = 0;
            UserID = 0;
            InactiveDate = DateTime.Now;
            BusinessUnitID= 0;
            LocationID = 0;
            DepartmentID = 0;
            ErrorMessage = "";
            Keys = "";
        }

        #region Properties
        public int DRPPID { get; set; }
        public int DRPID { get; set; }
        public int UserID { get; set; }
        public DateTime InactiveDate { get; set; }
        public int BusinessUnitID { get; set; }
		public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string Keys { get; set; }    
        #endregion

        #region Functions
        public static List<DepartmentRequirementPolicyPermission> Gets(string sSql, long nUserID)
        {
            return DepartmentRequirementPolicyPermission.Service.Gets(sSql,nUserID);
        }
        public bool ConfirmDRPPermission(int nUID, string sSelectedMenuKeys, long nUserID)
        {
            return DepartmentRequirementPolicyPermission.Service.ConfirmDRPPermission(nUID, sSelectedMenuKeys, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDepartmentRequirementPolicyPermissionService Service
        {
            get { return (IDepartmentRequirementPolicyPermissionService)Services.Factory.CreateService(typeof(IDepartmentRequirementPolicyPermissionService)); }
        }

        #endregion
    }
    #endregion

    #region IDepartmentRequirementPolicyPermission interface

    public interface IDepartmentRequirementPolicyPermissionService
    {
        List<DepartmentRequirementPolicyPermission> Gets(string sSql, Int64 nUserID);
        bool ConfirmDRPPermission(int nUserID, string sSelectedMenuKeys, Int64 nDBUserID);
    }
    #endregion
}

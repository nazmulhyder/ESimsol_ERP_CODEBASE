using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region DesignationResponsibility

    public class DesignationResponsibility : BusinessObject
    {
        public DesignationResponsibility()
        {
            DesignationResponsibilityID = 0;
            DRPID = 0;
            HRResponsibilityID = 0;
            DesignationID = 0;
            DesignationID = 0;
            HRResponsibilityCode = "";
            HRResponsibilityText = "";
            ErrorMessage = "";
            HRResponsibilityTextInBangla = "";
            DesignationResponsibilitys = new List<DesignationResponsibility>();
        }

        #region Properties
        public int DesignationResponsibilityID { get; set; }
        public int DRPID { get; set; }
        public int HRResponsibilityID { get; set; }
        public int DesignationID { get; set; }
        public string HRResponsibilityCode { get; set; }
        public string HRResponsibilityText { get; set; }
        public string HRResponsibilityTextInBangla { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public List<DesignationResponsibility> DesignationResponsibilitys { get; set; }

        #endregion

        #region Functions
        public static List<DesignationResponsibility> Gets(long nUserID)
        {
            return DesignationResponsibility.Service.Gets(nUserID);
        }
        public static List<DesignationResponsibility> GetsByPolicy(int nDepartmentRequirementPolicyID, long nUserID)
        {
            return DesignationResponsibility.Service.GetsByPolicy(nDepartmentRequirementPolicyID, nUserID);
        }
        public static List<DesignationResponsibility> Gets(int nDepartmentRequirementDesignationID, long nUserID)
        {
            return DesignationResponsibility.Service.Gets(nDepartmentRequirementDesignationID, nUserID);
        }
        public static List<DesignationResponsibility> Gets(string sSQL, long nUserID)
        {
            return DesignationResponsibility.Service.Gets(sSQL, nUserID);
        }
        public DesignationResponsibility Save(long nUserID)
        {
            return DesignationResponsibility.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDesignationResponsibilityService Service
        {
            get { return (IDesignationResponsibilityService)Services.Factory.CreateService(typeof(IDesignationResponsibilityService)); }
        }

        #endregion
    }
    #endregion

    #region IDesignationResponsibility interface

    public interface IDesignationResponsibilityService
    {
        List<DesignationResponsibility> Gets(Int64 nUserID);
        List<DesignationResponsibility> GetsByPolicy(int nDepartmentRequirementPolicyID, Int64 nUserID);
        List<DesignationResponsibility> Gets(int nDepartmentRequirementDesignationID, Int64 nUserID);
        List<DesignationResponsibility> Gets(string sSQL, Int64 nUserID);
        DesignationResponsibility Save(DesignationResponsibility oDesignationResponsibility, Int64 nUserID);
    }
    #endregion
}

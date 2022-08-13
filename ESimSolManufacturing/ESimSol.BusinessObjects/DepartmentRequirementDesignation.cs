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
    #region DepartmentRequirementDesignation

    public class DepartmentRequirementDesignation : BusinessObject
    {
        public DepartmentRequirementDesignation()
        {
            DepartmentRequirementDesignationID = 0;
            DepartmentRequirementPolicyID = 0;
            DesignationID = 0;
            HRResponsibilityID = 0;
            RequiredPerson = 0;
            StartTime = "";
            EndTime = "";
            NameOfShift = "";
            DesignationResponsibilitys = new List<DesignationResponsibility>();
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            Designation = "";
            Responsibility = "";
            ResponsibilityInBangla = "";
            ErrorMessage = "";
            //ShiftID = 0;
            //DesignationSequence = 0;
            //ShiftSequence = 0;
        }

        #region Properties
        public int DepartmentRequirementDesignationID { get; set; }
        public int DesignationID { get; set; }
        public int HRResponsibilityID { get; set; }
        public int RequiredPerson { get; set; }
        public int DepartmentRequirementPolicyID { get; set; }   
        public int DayID { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string Designation { get; set; }
        public string Responsibility { get; set; }
        public string ResponsibilityInBangla { get; set; }
        #endregion

        #region Derived Property
        public List<DesignationResponsibility> DesignationResponsibilitys { get; set; }
        public string DepartmentRequirementPolicyName { get; set; }
        public string DesignationName { get; set; }
        public string ShiftName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string NameOfShift { get; set; }

        #endregion

        #region Deleted Properties
        public int ShiftID { get; set; }
        public int DesignationSequence { get; set; }
        public int ShiftSequence { get; set; }

        #endregion

        #region Functions
        public static List<DepartmentRequirementDesignation> Gets(long nUserID)
        {
            return DepartmentRequirementDesignation.Service.Gets(nUserID);
        }
        public static List<DepartmentRequirementDesignation> Gets(int nDepartmentRequirementPolicyID, bool bIsShiftOrder, long nUserID)
        {
            return DepartmentRequirementDesignation.Service.Gets(nDepartmentRequirementPolicyID,bIsShiftOrder, nUserID);
        }

        public static List<DepartmentRequirementDesignation> GetsPolicy(int nDepartmentRequirementPolicyID, long nUserID)
        {
            return DepartmentRequirementDesignation.Service.GetsPolicy(nDepartmentRequirementPolicyID, nUserID);
        }
        public static List<DepartmentRequirementDesignation> Gets(string sSQL, long nUserID)
        {
            return DepartmentRequirementDesignation.Service.Gets(sSQL, nUserID);
        }

        //public DepartmentRequirementDesignation Get(int id, long nUserID)
        //{
        //    return DepartmentRequirementDesignation.Service.Get(id, nUserID);
        //}

        public DepartmentRequirementDesignation IUD(int nDBOperation, long nUserID)
        {
            return DepartmentRequirementDesignation.Service.IUD(this, nDBOperation, nUserID);
        }
        // Temp Work Start
        public string SaveDRPDesignations(List<DepartmentRequirementDesignation> oDRPDesigs, long nUserID)
        {
            return DepartmentRequirementDesignation.Service.SaveDRPDesignations(oDRPDesigs, nUserID);
        }
        // Temp Work End
        #endregion

        #region ServiceFactory
        internal static IDepartmentRequirementDesignationService Service
        {
            get { return (IDepartmentRequirementDesignationService)Services.Factory.CreateService(typeof(IDepartmentRequirementDesignationService)); }
        }

        #endregion
    }
    #endregion

    #region IDepartmentRequirementDesignation interface
    public interface IDepartmentRequirementDesignationService
    {
        List<DepartmentRequirementDesignation> Gets(Int64 nUserID);
        List<DepartmentRequirementDesignation> Gets(int nDepartmentRequirementPolicyID, bool bIsShiftOrder, Int64 nUserID);
        List<DepartmentRequirementDesignation> GetsPolicy(int nDepartmentRequirementPolicyID, Int64 nUserID);
        List<DepartmentRequirementDesignation> Gets(string sSQL, Int64 nUserID);
        DepartmentRequirementDesignation IUD(DepartmentRequirementDesignation oDepartmentRequirementDesignation, int nDBOperation, Int64 nUserID);
        // Temp Work Start
        string SaveDRPDesignations(List<DepartmentRequirementDesignation> oDRPDesigs, Int64 nUserID);
        // Temp Work End
    }
    #endregion

}

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
    #region DepartmentRequirementPolicy

    public class DepartmentRequirementPolicy : BusinessObject
    {
        public DepartmentRequirementPolicy()
        {
            DepartmentRequirementPolicyID = 0;
            CompanyID = 1;//This is a Hard Qoutted Value
            LocationID = 0;
            DepartmentID = 0;            
            Description = "";
            strDepartmentCloseDays = "";
            EmployeeCount = 0;
            ErrorMessage = "";
            HeadCount = 0;
            Budget = 0;
            BusinessUnitID = 0;
            Shifts = new List<HRMShift>();
            SelectedShifts = new List<HRMShift>();
            TempDesignations = new List<TempDesignation>();
            DepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
        }

        #region Properties
        public int DepartmentRequirementPolicyID { get; set; }        
        public int CompanyID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public string Description { get; set; }
        public int EmployeeCount { get; set; }
        public int HeadCount { get; set; }
        public double Budget { get; set; }
        public string ErrorMessage { get; set; }
        public int BusinessUnitID { get; set; }
        #endregion

        #region Derived Property

        public string CompanyName { get; set; }
        public string BUName { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public List<HRMShift> Shifts { get; set; }
        public List<HRMShift> SelectedShifts { get; set; }       
        //public List<Organogram> Organograms { get; set; }
        public List<Designation> Designations { get; set; }
        public List<DepartmentCloseDay> DepartmentCloseDays { get; set; }
        public List<DepartmentRequirementDesignation> DepartmentRequirementDesignations { get; set; }
        public string strDepartmentCloseDays { get; set; }
        public List<DepartmentRequirementPolicy> DepartmentRequirementPolicies { get; set; }
        public List<TempDesignation> TempDesignations { get; set; }


        #endregion

        #region Functions
        public static List<DepartmentRequirementPolicy> Gets(long nUserID)
        {
            return DepartmentRequirementPolicy.Service.Gets(nUserID);
        }
        public static List<DepartmentRequirementPolicy> Gets(string sSQL, long nUserID)
        {
            return DepartmentRequirementPolicy.Service.Gets(sSQL, nUserID);
        }
        public DepartmentRequirementPolicy Get(int id, long nUserID)
        {
            return DepartmentRequirementPolicy.Service.Get(id, nUserID);
        }
        public DepartmentRequirementPolicy Save(long nUserID)
        {
            return DepartmentRequirementPolicy.Service.Save(this, nUserID);
        }
        public string Delete(int nDeptReqPolicyID, long nUserID)
        {
            return DepartmentRequirementPolicy.Service.Delete(nDeptReqPolicyID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDepartmentRequirementPolicyService Service
        {
            get { return (IDepartmentRequirementPolicyService)Services.Factory.CreateService(typeof(IDepartmentRequirementPolicyService)); }
        }

        #endregion
    }
    #endregion

    #region IDepartmentRequirementPolicy interface

    public interface IDepartmentRequirementPolicyService
    {
        DepartmentRequirementPolicy Get(int id, Int64 nUserID);
        List<DepartmentRequirementPolicy> Gets(Int64 nUserID);
        List<DepartmentRequirementPolicy> Gets(string sSQL, Int64 nUserID);
        DepartmentRequirementPolicy Save(DepartmentRequirementPolicy oDepartmentRequirementPolicy, Int64 nUserID);
        string Delete(int nDeptReqPolicyID, Int64 nUserID);
    }
    #endregion

    public class DepartmentSetUp
    {
        public DepartmentSetUp() 
        {
            id = 0;
            text = "";
            state = "";
            attributes = "";
            parentid = 0;
            BusinessUnitID = 0;
            LocationID = 0;
            DetailsInfo = "";
            EmployeeCount = 0;
            DeptReqPolicyID = 0;
            DataType = 0;
            DepartmentID = 0;
        }

        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public int BusinessUnitID { get; set; }
        public int DepartmentID { get; set; }
        public int LocationID { get; set; }
        public string DetailsInfo { get; set; }
        public int EmployeeCount { get; set; }
        public int DeptReqPolicyID { get; set; }
        public int DataType { get; set; } // DataType 1=BU, 2=Location, 3= Department, 4= Designation
        public List<DepartmentSetUp> children { get; set; }//: an array nodes defines some children nodes
        public List<DepartmentRequirementPolicyPermission> DRPPs { get; set; }
    }
}

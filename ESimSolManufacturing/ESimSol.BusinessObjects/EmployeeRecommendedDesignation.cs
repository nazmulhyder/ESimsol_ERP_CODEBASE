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
    #region EmployeeRecommendedDesignation

    public class EmployeeRecommendedDesignation : BusinessObject
    {
        public EmployeeRecommendedDesignation()
        {

            ARDID = 0;
            EmployeeID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            ErrorMessage = "";
            EmployeeNameCode = "";
            DepartmentName = "";
            DesignationName = "";

        }

        #region Properties

        public int ARDID { get; set; }
        public int EmployeeID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string EmployeeNameCode { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public List<Department> Departments { get; set; }
        public List<Designation> Designations { get; set; }

        #endregion

        #region Functions
        public static EmployeeRecommendedDesignation Get(int Id, long nUserID)
        {
            return EmployeeRecommendedDesignation.Service.Get(Id, nUserID);
        }
        public static EmployeeRecommendedDesignation Get(string sSQL, long nUserID)
        {
            return EmployeeRecommendedDesignation.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeRecommendedDesignation> Gets(int Id, long nUserID)
        {
            return EmployeeRecommendedDesignation.Service.Gets(Id, nUserID);
        }
        public static List<EmployeeRecommendedDesignation> Gets(string sSQL, long nUserID)
        {
            return EmployeeRecommendedDesignation.Service.Gets(sSQL, nUserID);
        }
        public EmployeeRecommendedDesignation IUD(int nDBOperation, long nUserID)
        {
            return EmployeeRecommendedDesignation.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeRecommendedDesignationService Service
        {
            get { return (IEmployeeRecommendedDesignationService)Services.Factory.CreateService(typeof(IEmployeeRecommendedDesignationService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeRecommendedDesignation interface
    public interface IEmployeeRecommendedDesignationService
    {
        EmployeeRecommendedDesignation Get(int id, Int64 nUserID);
        EmployeeRecommendedDesignation Get(string sSQL, Int64 nUserID);
        List<EmployeeRecommendedDesignation> Gets(int Id, Int64 nUserID);
        List<EmployeeRecommendedDesignation> Gets(string sSQL, Int64 nUserID);
        EmployeeRecommendedDesignation IUD(EmployeeRecommendedDesignation oEmployeeRecommendedDesignation, int nDBOperation, Int64 nUserID);


    }
    #endregion
}

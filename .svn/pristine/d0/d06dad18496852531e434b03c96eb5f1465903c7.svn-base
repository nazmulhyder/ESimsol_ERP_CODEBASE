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
    #region EmployeeReportingPerson

    public class EmployeeReportingPerson : BusinessObject
    {
        public EmployeeReportingPerson()
        {
            ERPID = 0 ;
            RPID = 0;
            EmployeeID = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.MinValue;
            IsActive = true;
            ErrorMessage = "";
            Params = "";
        }

        #region Properties

        public int ERPID { get; set; }
        public int RPID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derived Property

        public string ReportingPersonName { get; set; }
        public string StartDateInStr { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateInStr { get { return (this.EndDate==DateTime.MinValue)? "--": this.EndDate.ToString("dd MMM yyyy"); } }

        public string ActiveStatus { get { return (this.IsActive)?"Active":"Inactive"; } }

        #endregion

        #region Functions

        public static EmployeeReportingPerson Get(int nERPID, long nUserID)
        {
            return EmployeeReportingPerson.Service.Get(nERPID, nUserID);
        }
        public static List<EmployeeReportingPerson> Gets(int nEmpID, long nUserID)
        {
            return EmployeeReportingPerson.Service.Gets(nEmpID, nUserID);
        }
        public static List<EmployeeReportingPerson> Gets(string sSQL, long nUserID)
        {
            return EmployeeReportingPerson.Service.Gets(sSQL, nUserID);
        }
        public EmployeeReportingPerson IUD(int nDBOperation, long nUserID)
        {
            return EmployeeReportingPerson.Service.IUD(this, nDBOperation, nUserID);
        }

        public static List<EmployeeReportingPerson> GetHierarchy(string sEmployeeIDs, long nUserID)
        {
            return EmployeeReportingPerson.Service.GetHierarchy(sEmployeeIDs, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeReportingPersonService Service
        {
            get { return (IEmployeeReportingPersonService)Services.Factory.CreateService(typeof(IEmployeeReportingPersonService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeReportingPerson interface

    public interface IEmployeeReportingPersonService
    {

        List<EmployeeReportingPerson> GetHierarchy(string sEmployeeIDs, Int64 nUserID);
        EmployeeReportingPerson Get(int nERPID, Int64 nUserID);
        List<EmployeeReportingPerson> Gets(int nEmpID, Int64 nUserID);
        List<EmployeeReportingPerson> Gets(string sSQL, Int64 nUserID);
        EmployeeReportingPerson IUD(EmployeeReportingPerson oEmployeeReportingPerson, int nDBOperation, Int64 nUserID);
    }
    #endregion
}

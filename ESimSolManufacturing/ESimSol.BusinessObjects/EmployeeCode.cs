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
    #region EmployeeCode

    public class EmployeeCode : BusinessObject
    {
        public EmployeeCode()
        {
            EmployeeCodeID = 0;
            DRPID = 0;
            DesignationID = 0;
            CompanyID = 0;
            ErrorMessage = "";
            EmployeeCodeDetails = new List<EmployeeCodeDetail>();
            EmployeeCodeDetailsInString = "";
            EncryptEmployeeCodeID = "";
            EmpCode = "";
            LocationName = "";
            DepartmentName = "";
            DesignationName = "";
            CompanyName = "";
            DepartmentID = 0;
            LocationID = 0;

        }



        #region Properties

        public int EmployeeCodeID { get; set; }
        public int LocationID { get; set; }
        public int DRPID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int CompanyID { get; set; }
        public string ErrorMessage { get; set; }
        public string EmployeeCodeDetailsInString { get; set; }

        #endregion

        #region Derived Property
        public int NumberMethodInInt { get; set; }
        public List<EmployeeCodeDetail> EmployeeCodeDetails { get; set; }
        public string EmpCode { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string CompanyName { get; set; }
        public string EncryptEmployeeCodeID { get; set; }
        
        #endregion

        #region Functions
        public static EmployeeCode Get(int id, long nUserID)
        {
            return EmployeeCode.Service.Get(id, nUserID);
        }

        public static List<EmployeeCode> Gets(long nUserID)
        {
            return EmployeeCode.Service.Gets(nUserID);
        }

        public static List<EmployeeCode> Gets(string sSQL, long nUserID)
        {
            return EmployeeCode.Service.Gets(sSQL, nUserID);
        }

        public EmployeeCode IUD(int nDBOperation ,long nUserID)
        {
            return EmployeeCode.Service.IUD(this,nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeCodeService Service
        {
            get { return (IEmployeeCodeService)Services.Factory.CreateService(typeof(IEmployeeCodeService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeCode interface
    public interface IEmployeeCodeService
    {
        EmployeeCode Get(int id, Int64 nUserID);
        List<EmployeeCode> Gets(Int64 nUserID);
        List<EmployeeCode> Gets(string sSQL, Int64 nUserID);
        EmployeeCode IUD(EmployeeCode oEmployeeCode,int nDBOperation, Int64 nUserID);
       
    }
    #endregion
}

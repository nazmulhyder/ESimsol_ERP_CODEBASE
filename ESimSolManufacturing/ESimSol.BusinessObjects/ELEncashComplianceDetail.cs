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
    #region ELEncashComplianceDetail
    public class ELEncashComplianceDetail
    {
        public ELEncashComplianceDetail()
        {
            ELECDID = 0;
            ELEncashCompID = 0;
            EmployeeID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            BlockID = 0;
            AttendanceSchemeID = 0;
            TotalPresent = 0;
            TotalAbsent = 0;
            TotalLeave = 0;
            TotalDayOff = 0;
            TotalHoliday = 0;
            TotalEarnLeave = 0;
            EncashELCount = 0;
            CompGrossSalary = 0;
            CompBasicAmount = 0;
            EncashAmount = 0;
            Stamp = 0;
            ApproveBy = 0;
            ApproveDate = DateTime.MinValue;
            PresencePerLeave = 0;
            LocationName = "";
            DepartmentName = "";
            BusinessUnitName = "";
            DesignationName = "";
            EmployeeName = "";
            ErrorMessage = "";
            Params = "";
            DeclarationDate = DateTime.MinValue;
            BusinessUnitAddress = "";
            EmployeeCode = "";
            JoiningDate = DateTime.MinValue;
            BUID = 0;
            ManPower = 0;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
        }

        #region Properties

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DeclarationDate { get; set; }
        public DateTime JoiningDate { get; set; }
        public string BusinessUnitAddress { get; set; }
        public string EmployeeCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public int ELECDID { get; set; }
        public int BUID { get; set; }
        public int ManPower { get; set; }

        public int ELEncashCompID { get; set; }

        public int EmployeeID { get; set; }

        public int DepartmentID { get; set; }

        public int DesignationID { get; set; }

        public int BlockID { get; set; }

        public int AttendanceSchemeID { get; set; }

        public int TotalPresent { get; set; }

        public int TotalAbsent { get; set; }

        public int TotalLeave { get; set; }

        public int TotalDayOff { get; set; }

        public int TotalHoliday { get; set; }

        public double TotalEarnLeave { get; set; }

        public double EncashELCount { get; set; }

        public double CompGrossSalary { get; set; }

        public double CompBasicAmount { get; set; }

        public double EncashAmount { get; set; }

        public double Stamp { get; set; }

        public int ApproveBy { get; set; }

        public DateTime ApproveDate { get; set; }

        public int PresencePerLeave { get; set; }

        public string LocationName { get; set; }

        public string DepartmentName { get; set; }

        public string BusinessUnitName { get; set; }

        public string DesignationName { get; set; }

        public string EmployeeName { get; set; }
        #endregion
        //public double NetPayable
        //{
        //    get
        //    {
        //        return (this.EncashAmount - this.Stamp);
        //    }
        //}
        public string DeclarationDateInStr
        {
            get
            {
                return DeclarationDate.ToString("dd MMM yyyy");
            }
        }
        public string StartDateInStr
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateInStr
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }
        #region Functions


        public static List<ELEncashComplianceDetail> Gets(string sSQL, long nUserID)
        {
            return ELEncashComplianceDetail.Service.Gets(sSQL, nUserID);
        }
        public static ELEncashComplianceDetail Get(string sSQL, long nUserID)
        {
            return ELEncashComplianceDetail.Service.Get(sSQL, nUserID);
        }
 
        #endregion

        #region ServiceFactory

        internal static IELEncashComplianceDetailService Service
        {
            get { return (IELEncashComplianceDetailService)Services.Factory.CreateService(typeof(IELEncashComplianceDetailService)); }
        }
        #endregion


    }
    #endregion

    #region IELEncashComplianceDetail interface

    public interface IELEncashComplianceDetailService
    {
        List<ELEncashComplianceDetail> Gets(string sSQL, Int64 nUserID);
        ELEncashComplianceDetail Get(string sSQL, Int64 nUserID);
       
      
    }
    #endregion
}


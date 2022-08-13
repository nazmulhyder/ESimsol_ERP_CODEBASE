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
    #region AbsentAmount_XL

    public class AbsentAmount_XL : BusinessObject
    {
        public AbsentAmount_XL()
        {
            SL = "";
            EmployeeCode = "";
            EmployeeName = "";
            DepartmentName = "";
            DesignationName = "";
            DOJ = DateTime.Now;
            Basic = 0;
            HRent = 0;
            Medical = 0;
            GrossAmount = 0;
            Sick = 0;
            LWP = 0;
            TotalAbsentAmount = 0;
            ErrorMessage = "";

        }

        #region Properties
        public string SL { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime DOJ { get; set; }
        public double Basic { get; set; }
        public double HRent { get; set; }
        public double Medical { get; set; }
        public double GrossAmount { get; set; }
        public double Sick { get; set; }
        public double LWP { get; set; }
        public double TotalAbsentAmount { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsActive { get; set; }

        #endregion

        #region Derived Property
        public string DOJInString
        {
            get
            {
                return DOJ.ToString("dd MMM yyyy");
            }
        }
        public string EmployeeWorkingStatus
        {
            get
            {
                return this.IsActive ? "Continued" : "Discontinued";
            }
        }
        #endregion

        #region Functions
        public static List<AbsentAmount_XL> Gets(DateTime StartDate, DateTime EndDate, long nUserID)
        {
            return AbsentAmount_XL.Service.Gets(StartDate, EndDate, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IAbsentAmount_XLService Service
        {
            get { return (IAbsentAmount_XLService)Services.Factory.CreateService(typeof(IAbsentAmount_XLService)); }
        }

        #endregion
    }
    #endregion

    #region IAbsentAmount_XL interface

    public interface IAbsentAmount_XLService
    {
        List<AbsentAmount_XL> Gets(DateTime StartDate, DateTime EndDate, Int64 nUserID);


    }
    #endregion
}

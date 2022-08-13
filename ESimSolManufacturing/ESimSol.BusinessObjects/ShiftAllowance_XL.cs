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
    #region ShiftAllowance_XL

    public class ShiftAllowance_XL : BusinessObject
    {
        public ShiftAllowance_XL()
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
            TotalShiftDay = 0;
            Value = 0;
            ShiftAmount = 0;
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
        public double TotalShiftDay { get; set; }
        public double Value { get; set; }
        public double ShiftAmount { get; set; }
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
        public static List<ShiftAllowance_XL> Gets(DateTime StartDate, DateTime EndDate, long nUserID)
        {
            return ShiftAllowance_XL.Service.Gets(StartDate, EndDate, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IShiftAllowance_XLService Service
        {
            get { return (IShiftAllowance_XLService)Services.Factory.CreateService(typeof(IShiftAllowance_XLService)); }
        }

        #endregion
    }
    #endregion

    #region IShiftAllowance_XL interface

    public interface IShiftAllowance_XLService
    {
        List<ShiftAllowance_XL> Gets(DateTime StartDate, DateTime EndDate, Int64 nUserID);


    }
    #endregion
}

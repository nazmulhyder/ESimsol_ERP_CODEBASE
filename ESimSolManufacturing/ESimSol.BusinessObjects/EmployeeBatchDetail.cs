using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    public class EmployeeBatchDetail : BusinessObject
    {
        public EmployeeBatchDetail()
        {
            EmployeeBatchDetailID = 0;
            EmployeeBatchID = 0;
            EmployeeID = 0;
            Location = "";
            Department = "";
            Designation = "";
            ShiftName = "";
            AttendanceScheme = "";
            SalaryScheme = "";
            DateOfJoin = DateTime.Now;
            GrossAmount = 0;
            ComplianceGross = 0;
            EmployeeName = "";
            EmployeeCode = "";
            ErrorMessage = "";
        }
        #region properties
        public int EmployeeBatchDetailID { get; set; }
        public int EmployeeBatchID { get; set; }
        public int EmployeeID { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ShiftName { get; set; }
        public string AttendanceScheme { get; set; }
        public string SalaryScheme { get; set; }
        public DateTime DateOfJoin { get; set; }
        public double GrossAmount { get; set; }
        public double ComplianceGross { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region derived properties
        public string DateOfJoinInString
        {
            get
            {
                return DateOfJoin.ToString("dd MMM yyyy");
            }
        }
        #endregion
        public static List<EmployeeBatchDetail> Gets(int id, long nUserID)
        {
            return EmployeeBatchDetail.Service.Gets(id, nUserID);
        }
        public static List<EmployeeBatchDetail> ArchiveSalaryChnage(ArchiveSalaryStruc oArchiveSalaryStruc, long nUserID)
        {
            return EmployeeBatchDetail.Service.ArchiveSalaryChnage(oArchiveSalaryStruc, nUserID);
        }
        #region ServiceFactory
        internal static IEmployeeBatchDetailService Service
        {
            get { return (IEmployeeBatchDetailService)Services.Factory.CreateService(typeof(IEmployeeBatchDetailService)); }
        }
        #endregion
    }
    #region IEmployeeBatchDetailService interface

    public interface IEmployeeBatchDetailService
    {
        List<EmployeeBatchDetail> Gets(int id, long nUserID);
        List<EmployeeBatchDetail> ArchiveSalaryChnage(ArchiveSalaryStruc oArchiveSalaryStruc, long nUserID);

    }
    #endregion
}

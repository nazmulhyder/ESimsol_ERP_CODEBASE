using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region EmployeeLoanInfo

    public class EmployeeLoanInfo : BusinessObject
    {

        #region  Constructor
        public EmployeeLoanInfo()
        {
            EmployeeID = 0;
            Code = string.Empty;
            Name = string.Empty;
            DepartmentName = string.Empty;
            DesignationName = string.Empty;
            LoanAmount = 0;
            NoOfInstallment = 0;
            InstallmentAmount = 0;
            InterestRate = 0;
            DisburseDate = DateTime.MinValue;
            InstallmentStartDate = DateTime.MinValue;
            InstallmentPaid = 0;
            InterestPaid = 0;
            RefundAmount = 0;
            ErrorMessage = string.Empty;
            Params = string.Empty;
            IsActive = false;
        }
        #endregion

        #region Properties
        public int EmployeeID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public double LoanAmount { get; set; }
        public short NoOfInstallment { get; set; }
        public double InstallmentAmount { get; set; }
        public double InterestRate { get; set; }
        public DateTime DisburseDate { get; set; }
        public DateTime InstallmentStartDate { get; set; }
        public double InstallmentPaid { get; set; }
        public double InterestPaid { get; set; }
        public double InstallmentPayable { get; set; }
        public double InterestPayable { get; set; }
        public double RefundAmount { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public bool IsActive { get; set; }
        #endregion

        #region Derived Properties
        public string EmployeeNameCode
        {
            get
            {
                return this.Name + " " + "["+ this.Code + "]";
            }
        }

        public double LoanBalance
        {
            get
            {
                return (this.LoanAmount - this.RefundAmount - this.InstallmentPaid - this.InstallmentPayable);
            }
        }

        public string DisburseDateStr
        {
            get
            {
                return (this.DisburseDate == DateTime.MinValue) ? "" : this.DisburseDate.ToString("dd MMM yyyy");
            }
        }
        public string InstallmentStartDateStr
        {
            get
            {
                return (InstallmentStartDate == DateTime.MinValue) ? "" : InstallmentStartDate.ToString("dd MMM yyyy");
            }
        }

        public string IsActiveInString
        {
            get
            {
                if (IsActive) return "Continued";
                else return "Discontinued";
            }
        }

        #endregion


        #region Functions

        public static List<EmployeeLoanInfo> Gets(DateTime dtFrom, DateTime dtTo, long nUserID)
        {
            return EmployeeLoanInfo.Service.Gets(dtFrom, dtTo, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeLoanInfoService Service
        {
            get { return (IEmployeeLoanInfoService)Services.Factory.CreateService(typeof(IEmployeeLoanInfoService)); }
        }
        #endregion
    }
    #endregion



    #region IEmployeeLoanInfo interface
    public interface IEmployeeLoanInfoService
    {
        List<EmployeeLoanInfo> Gets(DateTime dtFrom, DateTime dtTo, long nUserID);
    }
    #endregion
}
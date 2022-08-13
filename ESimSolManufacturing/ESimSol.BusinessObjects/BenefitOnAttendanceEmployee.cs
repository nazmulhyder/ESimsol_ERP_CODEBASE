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
    #region BenefitOnAttendanceEmployee

    public class BenefitOnAttendanceEmployee : BusinessObject
    {
        public BenefitOnAttendanceEmployee()
        {

            BOAEmployeeID = 0;
            BOAID = 0;
            EmployeeID = 0;
            InactiveDate = DateTime.Now;
            InactiveBy = 0;
            IsTemporaryAssign = false;
            ErrorMessage = "";
            IDs = "";
            EmployeeName = string.Empty;
		    EmployeeCode = string.Empty;
		    DepartmentName = string.Empty;
		    DesignationName= string.Empty;
            Params = string.Empty;
        }

        #region Properties
        public int BOAESID { get; set; }
        public int BOAEmployeeID { get; set; }
        public int BOAID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime InactiveDate { get; set; }
        public int InactiveBy { get; set; }
        public bool IsTemporaryAssign { get; set; }
        public string ErrorMessage { get; set; }
        public string IDs { get; set; }

        #endregion

        #region Derived Property
        public string Name { get; set; }
        public string SalaryHeadName { get; set; }
        public string LeaveHeadName { get; set; }
        public string EncryptedID { get; set; }
        public Company Company { get; set; }
        public EnumBenefitOnAttendance BenefitOn { get; set; }
        public int LeaveHeadID { get; set; }
        public int LeaveAmount { get; set; }
        public int OTInMinute { get; set; }
        public bool IsFullWorkingHourOT { get; set; }
        public int OTDistributePerPresence { get; set; }
        public int SalaryHeadID { get; set; }
        public bool IsPercent { get; set; }
        public int AllowanceOnInt { get; set; }
        public EnumPayrollApplyOn AllowanceOn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Value { get; set; }
   
        public string EmployeeName { get; set; }
		public string EmployeeCode { get; set; }
		public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Params { get; set; }

        public string AllowanceOnInString
        {
            get
            {
                return this.AllowanceOn.ToString();
            }
        }

        public string CurrencySymbol { get; set; }
        public string BenefitOnInString
        {
            get
            {
                return this.BenefitOn.ToString();
            }
        }
       
        public string OvertimeST
        {
            get
            {
                if (this.OTInMinute > 0 && !this.IsFullWorkingHourOT)
                    return "OT " + (this.OTInMinute / 60).ToString() + " hr and distribute " + (this.OTDistributePerPresence / 60).ToString() + " per day";
                else if (this.OTInMinute <= 0 && this.IsFullWorkingHourOT)
                    return "Full Working Hour";
                else
                    return "";
            }
        }
        public string SalaryST
        {
            get
            {
                string S = "";
                if (this.SalaryHeadID > 0)
                {
                    S = this.SalaryHeadName + ", ";

                    if (this.IsPercent)
                    {
                        S += this.Value + "% of" + this.AllowanceOnInString;
                    }
                    else
                    {
                        S += this.CurrencySymbol + this.Value + "(fixed)";
                    }
                    return S;
                }
                return S;
            }
        }
        public string LeaveST
        {
            get
            {
                if (this.LeaveHeadID > 0)
                    return this.LeaveAmount + " " + this.LeaveHeadName;
                else
                    return "";
            }
        }
        private string sBenifits = "";
        public string Benifits
        {
            get
            {
                if (this.OvertimeST.Length > 0)
                {
                    sBenifits =sBenifits+ this.OvertimeST;
                }
                if (this.SalaryST.Length > 0)
                {
                    sBenifits = sBenifits + this.SalaryST;
                }
                if (this.LeaveST.Length > 0)
                {
                    sBenifits = sBenifits + this.LeaveST;
                }
                    return sBenifits;
            }
        }
        public string StartDateSt
        {
            get
            {
                if (this.StartDate == DateTime.MinValue) return "--";

                else return this.StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateSt
        {
            get
            {
                if (this.EndDate == DateTime.MinValue) return "--";

                else return this.EndDate.ToString("dd MMM yyyy");
            }
        }
        private string sInactiveDateSt = "";
        public string InactiveDateSt
        {
            get
            {
                if (this.BOAEmployeeID > 0)
                {
                    if (this.InactiveDate == DateTime.MinValue)
                    {
                        if (this.StartDate == DateTime.MinValue)
                        {
                            sInactiveDateSt = "Active";
                        }
                        else
                        {
                            sInactiveDateSt = this.StartDate.ToString("dd MMM yyyy") + " To " + this.EndDate.ToString("dd MMM yyyy");
                        }
                    }

                    else sInactiveDateSt = this.InactiveDate.ToString("dd MMM yyyy");

                    return sInactiveDateSt;
                }
                else
                {
                    return "";
                }
                
            }
        }

        public string IsTemporaryAssignStr
        {
            get
            {
                return (this.BOAEmployeeID > 0) ? ((this.IsTemporaryAssign) ? "Temporary" : "Permanent") : "";
            }
        }
        #endregion

        #region Functions
        public static BenefitOnAttendanceEmployee Get(int Id, long nUserID)
        {
            return BenefitOnAttendanceEmployee.Service.Get(Id, nUserID);
        }
        public static BenefitOnAttendanceEmployee Get(string sSQL, long nUserID)
        {
            return BenefitOnAttendanceEmployee.Service.Get(sSQL, nUserID);
        }
        public static List<BenefitOnAttendanceEmployee> Gets(int nEmployeeID,long nUserID)
        {
            return BenefitOnAttendanceEmployee.Service.Gets(nEmployeeID,nUserID);
        }

        public static List<BenefitOnAttendanceEmployee> Gets(string sSQL, long nUserID)
        {
            return BenefitOnAttendanceEmployee.Service.Gets(sSQL, nUserID);
        }

        public BenefitOnAttendanceEmployee IUD(int nDBOperation, long nUserID)
        {
            return BenefitOnAttendanceEmployee.Service.IUD(this, nDBOperation, nUserID);
        }

        public static List<BenefitOnAttendanceEmployee> MultiAssign(List<BenefitOnAttendanceEmployee> oBOAEs, DateTime dtStartFrom, DateTime dtEndTo, long nUserID)
        {
            return BenefitOnAttendanceEmployee.Service.MultiAssign(oBOAEs, dtStartFrom, dtEndTo, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IBenefitOnAttendanceEmployeeService Service
        {
            get { return (IBenefitOnAttendanceEmployeeService)Services.Factory.CreateService(typeof(IBenefitOnAttendanceEmployeeService)); }
        }

        #endregion
    }
    #endregion

    #region IBenefitOnAttendanceEmployee interface

    public interface IBenefitOnAttendanceEmployeeService
    {
        BenefitOnAttendanceEmployee Get(int id, Int64 nUserID);
        BenefitOnAttendanceEmployee Get(string sSQL, Int64 nUserID);
        List<BenefitOnAttendanceEmployee> Gets(int nEmployeeID,Int64 nUserID);
        List<BenefitOnAttendanceEmployee> Gets(string sSQL, Int64 nUserID);
        BenefitOnAttendanceEmployee IUD(BenefitOnAttendanceEmployee oBenefitOnAttendanceEmployee, int nDBOperation, Int64 nUserID);
        List<BenefitOnAttendanceEmployee> MultiAssign(List<BenefitOnAttendanceEmployee> oBOAEs, DateTime dtStartFrom, DateTime dtEndTo, Int64 nUserID);
    }
    #endregion
}

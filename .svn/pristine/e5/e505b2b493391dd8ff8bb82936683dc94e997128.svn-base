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
    #region EmployeeDatabase
    [DataContract]
    public class EmployeeDatabase : BusinessObject
    {
        public EmployeeDatabase()
        {
            EmployeeID = 0;
            EmployeeStructureID = 0;
            EmployeeCode = "";
            EmployeeName = "";
            EmployeeID =0;
		    EmployeeCode ="";
		    EmployeeName ="";
		    FatherName ="";
		    MotherName ="";
		    PresentAddress ="";
            PermanentAddress = "";
            ContactNo = "";
		    DateOfBirth = DateTime.Now;
		    Gender ="";
            BloodGroup = "";
            Religion = "";
            EmployeeType = "";
            EmployeeCategory = EnumEmployeeCategory.None;
		    DepartmentName="";
		    DesignationName ="";
		    DateOfJoin = DateTime.Now;
		    DateOfConf = DateTime.Now;
		    AttendanceScheme="";
		    ShiftName ="";
		    CardNo ="";
		    salaryScheme ="";
		    Gross =0;
            CompGross = 0;
		    BankName ="";
		    AccountNo ="";
            ETINNumber = "";
            BUName = "";
		    PFScheme ="";
            PFMemberDate = DateTime.Now;
		    PIScheme ="";
		    TAXPerMonth =0;
		    LoanBalance =0;
            NomineeName ="";
		    NomineeContact ="";
            NomineeRelation = "";
            LastWorkingDate = DateTime.Now;
            LastEducationDegree = "";
            Email = "";
            NationalID = "";
            BirthID = "";

            Thana = "";
            District = "";
            Village = "";
            PostOffice = "";
            PostCode = "";
            GroupName = "";
            BlockName = "";
            ReportingPerson = "";

            BankAmount = 0;
            CashAmount = 0;
        }

        #region Properties
        public double BankAmount { get; set; }
        public double CashAmount { get; set; }
        public int EmployeeID { get; set; }
        public int EmployeeStructureID { get; set; }
        public string ReportingPerson { get; set; }
        public string EmployeeCode { get; set; }
        public string GroupName { get; set; }
        public string BlockName { get; set; }
        public string Thana { get; set; }
        public string District { get; set; }
        public string Village { get; set; }
        public string PostOffice { get; set; }
        public string PostCode { get; set; }
        public string Email { get; set; }
        public string NationalID { get; set; }
        public string BirthID { get; set; }
        public string EmployeeName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmployeeType { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Religion { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string LocationName { get; set; }
        public string BUName { get; set; }
        public string ContactNo { get; set; }
        public DateTime DateOfJoin { get; set; }
        public DateTime DateOfConf { get; set; }
        public string AttendanceScheme { get; set; }
        public string ShiftName { get; set; }
        public string CardNo { get; set; }
        public string salaryScheme { get; set; }
        public double Gross { get; set; }
        public double CompGross { get; set; }
        public string BankName { get; set; }
        public string Weight { get; set; }
        public string AccountNo { get; set; }
        public string ETINNumber { get; set; }        
        public string PFScheme { get; set; }
        public DateTime PFMemberDate { get; set; }
        public string PIScheme { get; set; }
        public double TAXPerMonth { get; set; }
        public double LoanBalance { get; set; }
        public string NomineeName { get; set; }
        public string NomineeContact { get; set; }
        public string NomineeRelation { get; set; }

        public string PFMemberDateInString
        {
            get
            {
                if (this.PFMemberDate < Convert.ToDateTime("01 JAN 1900")) return "-"; else return PFMemberDate.ToString("dd MMM yyyy");
            }
        }

        public string DateOfBirthInString
        {
            get
            {
                return DateOfBirth.ToString("dd MMM yyyy");
            }
        }
        public string DateOfJoinInString
        {
            get
            {
                return DateOfJoin.ToString("dd MMM yyyy");
            }
        }
        public string DateOfConfInString
        {
            get
            {
                return DateOfConf.ToString("dd MMM yyyy");
            }
        }
        public EnumEmployeeCategory EmployeeCategory { get; set; }
        public string EmployeeCategoryInString
        {
            get
            {
                return EmployeeCategory.ToString();
            }
        }
        public DateTime LastWorkingDate { get; set; }
        public string LastWorkingDateInString
        {
            get
            {
                return LastWorkingDate.ToString("dd MMM yyyy");
            }
        }
        public string LastEducationDegree { get; set; }
        #endregion

        #region Functions

        public static List<EmployeeDatabase> Gets(string sParam, long nUserID)
        {
            return EmployeeDatabase.Service.Gets(sParam, nUserID);
        }
       
        #endregion

        #region ServiceFactory
        internal static IEmployeeDatabaseService Service
        {
            get { return (IEmployeeDatabaseService)Services.Factory.CreateService(typeof(IEmployeeDatabaseService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeDatabase interface

    public interface IEmployeeDatabaseService
    {
        List<EmployeeDatabase> Gets(string sParam, Int64 nUserId);
    }
    #endregion
}

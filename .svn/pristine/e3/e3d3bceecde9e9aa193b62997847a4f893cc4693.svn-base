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
    #region Employee
    [DataContract]
    public class Employee : BusinessObject
    {
        public Employee()
        {
            EmployeeID = 0;
            CompanyID = 1;
            Code = "";
            Name = "";
            NickName = "";
            MiddleName = "";
            Gender = "";
            MaritalStatus = "";
            FatherName = "";
            SpouseName = "";
            MotherName = "";
            ParmanentAddress = "";
            PresentAddress = "";
            ContactNo = "";
            Email = "";
            DateOfBirth = DateTime.Now;
            BloodGroup = "N/A";
            Height = "";
            Weight = "";
            IdentificationMart = "";
            Chest = "";
            Waist = "";
            ShoeSize = "";
            Photo = null;
            Signature = null;
            Note = "";
            Attachment = null;
            IsActive = true;
            EmployeeDesignationType = EnumEmployeeDesignationType.None;
            EmployeeDesignationTypeInt = 0;
            RosterPlanID = 0;
            DisciplinaryActions = new List<DisciplinaryAction>();
            EmployeeEducations = new List<EmployeeEducation>();
            EmployeeSettlements = new List<EmployeeSettlement>();
            EmployeeSettlement = new EmployeeSettlement();
            EmployeeExperiences = new List<EmployeeExperience>();
            EmployeeTrainings = new List<EmployeeTraining>();
            EmployeeReferences = new List<EmployeeReference>();
            EmployeeBankAccounts = new List<EmployeeBankAccount>();
            EmployeeTypes = new List<EmployeeType>();
            EmployeeActiveInactiveHistorys = new List<EmployeeActiveInactiveHistory>();
            Shifts = new List<HRMShift>();
            RosterPlanDescription = "";
            EmployeeTypeID = 0;
            EmployeeTypeName = "";
            IsFather=true;
            BirthID="";
            BirthPlace = "";
            NationalID="";
            Religious=""; 
            Nationalism=""; 
            ChildrenInfo="";
            Village="";
            PostOffice="";
            Thana="";
            District="";
            PostCode = "";
            NameInBangla = "";
            PassportNo ="";
            CountryOfPassport = "";
            CurrentEmploymentStatus = EnumCurrentEmploymentStatus.Initialize;
            IsOwnEmployee = true;
            PassportIssueDate="";	
            PassportExpireDate="";	
            SeamanBook="";
            SemanBookIssueDate = "";
            SeamanBookExpireDate = "";
            ErrorMessage = "";
            Selected = false;
            LeaveLedgerReports = new List<LeaveLedgerReport>();
            EmployeeTINInformations = new EmployeeTINInformation();
            EmployeeReportingPersons = new List<EmployeeReportingPerson>();
            AttendanceSchemeHolidays = new List<AttendanceSchemeHoliday>();
            AttendanceSchemeLeaves = new List<AttendanceSchemeLeave>();
            EncryptEmpID = "";
            EmployeeOfficial = new EmployeeOfficial();
            HRResponsibility = new List<HRResponsibility>();
            Params = "";
            DateOfJoin = DateTime.Now;
            DRPID = 0;
            DesignationID = 0;
            ConfirmationDate = DateTime.Now;
            EmployeeConfirmations = new List<EmployeeConfirmation>();
            EmployeeCategory = EnumEmployeeCategory.None;
            BUName = "";
            CardNo = "";
            OtherPhoneNo = "";
            BusinessUnitID = 0;
            Male = 0;
            Female = 0;
            TotalManPower = 0;
            LocationID = 0;
            DepartmentID = 0;

            PermVillage = "";
			PermPostOffice = "";
			PermThana = "";
            PermDistrict = "";
            BankID = 0;

        }

        #region Properties

        public string PermVillage { get; set; }
        public string PermPostOffice { get; set; }
        public string PermThana { get; set; }
        public string PermDistrict { get; set; }

        public int BankID { get; set; }
        public int EmployeeID { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int BusinessUnitID { get; set; }
        
        public int CompanyID { get; set; }

        public string Code { get; set; }
        public string OtherPhoneNo { get; set; }
        
        public string Name { get; set; }
        
        public string NickName { get; set; }
        
        public string MiddleName { get; set; }
        
        public string Gender { get; set; }
        
        public string MaritalStatus { get; set; }
        
        public string FatherName { get; set; }
        public string SpouseName { get; set; }
        public string MotherName { get; set; }
        
        public string ParmanentAddress { get; set; }
        
        public string PresentAddress { get; set; }
        
        public string ContactNo { get; set; }
        
        public string Email { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public string BloodGroup { get; set; }
        
        public string Height { get; set; }
        
        public string Weight { get; set; }
        
        public string IdentificationMart { get; set; }
        
        public string Chest { get; set; }
        
        public string Waist { get; set; }
        
        public string ShoeSize { get; set; }
        
        public byte[] Photo { get; set; }
        
        public byte[] Signature { get; set; }
        
        public string Note { get; set; }
        
        public byte[] Attachment { get; set; }
        
        public bool IsActive { get; set; }
        
        public EnumEmployeeDesignationType EmployeeDesignationType { get; set; }
        public int EmployeeDesignationTypeInt { get; set; }
        
        public bool IsFather { get; set; }
        
        public string BirthID { get; set; }
        
        public string BirthPlace { get; set; }
        
        public string NationalID { get; set; }
        
        public string Religious { get; set; }
        
        public string Nationalism { get; set; }
        
        public string ChildrenInfo { get; set; }
        
        public string Village { get; set; }
        
        public string PostOffice { get; set; }
        
        public string Thana { get; set; }
        
        public string District { get; set; }
        
        public string PostCode { get; set; }
        
        
        public string PassportNo { get; set; }
        
        public string CountryOfPassport { get; set; }
        
        public EnumCurrentEmploymentStatus CurrentEmploymentStatus { get; set; }
        
        public bool IsOwnEmployee { get; set; }
        
        public string PassportIssueDate { get; set; }
        
        public string PassportExpireDate { get; set; }
        
        public string SeamanBook { get; set; }
        
        public string SemanBookIssueDate { get; set; }
	
        
        public string SeamanBookExpireDate { get; set; }	
	


        //// for Non DB Property  ///
        public string EncryptEmpID { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derive Property
        public bool Selected { get; set; }
        public string BUName { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int DRPID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameInBangla { get; set; }
        public string LocationNameInBangla { get; set; }
        public string BusinessUnitNameInBangla { get; set; }
        public string BusinessUnitName { get; set; }
        public string BUAddressInBangla { get; set; }
        public string BUAddress { get; set; }
        public string BUPhone { get; set; }
        public string BUFaxNo { get; set; }
        public int TotalManPower { get; set; }
        
        
        public string CardNo { get; set; }
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
        public string DesignationNameInBangla { get; set; }
        
        public EnumEmployeeWorkigStatus WorkingStatus { get; set; }
        public string WorkingStatusInString { get { return this.WorkingStatus.ToString(); } }
        public int CurrentEmploymentStatusInt { get; set; }
        public string CurrentEmploymentStatusInString
        {
            get
            {
                return CurrentEmploymentStatus.ToString();
            }
        }

        public string BloodGroupST
        {
            get
            {
                string SBG = "";
                if (this.BloodGroup == "1") { SBG = "A+"; }
                else if (this.BloodGroup == "2") { SBG = "A-"; }
                else if (this.BloodGroup == "3") { SBG = "B+"; }
                else if (this.BloodGroup == "4") { SBG = "B-"; }
                else if (this.BloodGroup == "5") { SBG = "O+"; }
                else if (this.BloodGroup == "6") { SBG = "O-"; }
                else if (this.BloodGroup == "7") { SBG = "AB+"; }
                else if (this.BloodGroup == "8") { SBG = "AB-"; }
                else { SBG = ""; }
                return SBG;
            }
        }
      

        public string EmployeeDesignationTypeInString
        {
            get
            {
                return this.EmployeeDesignationType.ToString();
            }
        }
        public string Activity
        {
            get
            {
                if (this.IsActive == true) return "Active";
                else return "InActive";
            }
        }
        public string DateOfBirthInString
        {
            get
            {
                return DateOfBirth.ToString("dd MMM yyyy");
            }
        }

        
        public DateTime DateOfJoin { get; set; }

        public string DateOfJoinInString
        {
            get
            {
                return DateOfJoin.ToString("dd MMM yyyy");
            }
        }

        public DateTime ConfirmationDate { get; set; }

        public string ConfirmationDateInString
        {
            get
            {
                return ConfirmationDate.ToString("dd MMM yyyy");
            }
        }
        public string YettoConfirmInString
        {
            get
            {
                if(DateTime.Now<this.ConfirmationDate)
                { DateDifference dateDifference = new DateDifference(DateTime.Now, this.ConfirmationDate); return dateDifference.ToString(); }
                else { return "-"; }
            }
        }
        public int EmployeeTypeID { get; set; }
        
        public string EmployeeTypeName { get; set; }
        public System.Drawing.Image EmployeePhoto { get; set; }
        public System.Drawing.Image EmployeeSiganture { get; set; }
        //public iTextSharp.text.Image EmployeePhotoItext { get; set; }

        
        public EnumEmployeeCardStatus EmployeeCardStatus { get; set; }
        public string EmployeeCardStatusInString
        {
            get
            {
                return EmployeeCardStatus.ToString();
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

        #region For bangla Tab
        public string CodeBangla { get; set; }
        public string NameInBangla { get; set; }
        public string FatherNameBangla { get; set; }
        public string MotherNameBangla { get; set; }
        public string NationalityBangla { get; set; }
        public string NationalIDBangla { get; set; }
        public string BloodGroupBangla { get; set; }
        public string HeightBangla { get; set; }
        public string WeightBangla { get; set; }
        public string DistrictBangla { get; set; }
        public string ThanaBangla { get; set; }
        public string PostOfficeBangla { get; set; }
        public string VillageBangla { get; set; }
        public string PresentAddressBangla { get; set; }
        public string PermDistrictBangla { get; set; }
        public string PermThanaBangla { get; set; }
        public string PermPostOfficeBangla { get; set; }
        public string PermVillageBangla { get; set; }
        public string PermanentAddressBangla { get; set; }
        public string ReligionBangla { get; set; }
        public string MaritalStatusBangla { get; set; }
        public string NomineeBangla { get; set; }
        public string AuthenticationBangla { get; set; }

        #endregion
        //Objects

        public List<EmployeeActiveInactiveHistory> EmployeeActiveInactiveHistorys { get; set; }
        public List<EmployeeEducation> EmployeeEducations { get; set; }
        public List<EmployeeSettlement> EmployeeSettlements { get; set; }
        public  EmployeeSettlement EmployeeSettlement { get; set; }
        public List<EmployeeExperience> EmployeeExperiences { get; set; }
        
        public List<EmployeeReference> EmployeeReferences { get; set; }

        public List<EmployeeBankAccount> EmployeeBankAccounts { get; set; }
        public List<EmployeeOfficial> EmployeeOfficials { get; set; }
        public List<EmployeeLeaveLedger> EmployeeLeaveLedgers { get; set; }
        public EmployeeTINInformation EmployeeTINInformations { get; set; }
        public List<EmployeeReportingPerson> EmployeeReportingPersons { get; set; }
        
        public List<EmployeeAuthentication> EmployeeAuthentications { get; set; }
        public List<EmployeeNominee> EmployeeNominees { get; set; }
        public List<BlockMachineMappingSupervisor> BlockMachineMappingSupervisors { get; set; }
        
        public List<EmployeeRecommendedDesignation> EmployeeRecommendedDesignations { get; set; }
        
        public List<EmployeeTraining> EmployeeTrainings { get; set; }
        
        public List<Employee> EmployeeHrms { get; set; }
        
        public AttendanceScheme AttendanceScheme { get; set; }
        
        //public  List<Organogram> Organograms { get; set; }
        
        public List<Designation> Designations { get; set; }
        
        public int  RosterPlanID { get; set; }
        
        public string RosterPlanDescription { get; set; }
        
        public string CurrentShift { get; set; }

        public List<RosterPlan> RosterPlans { get; set; } 
        public List<RosterPlanDetail> RosterPlanDetails { get; set; } 
        
        public List<AttendanceSchemeHoliday> AttendanceSchemeHolidays { get; set; } 
        
        public List<AttendanceSchemeLeave> AttendanceSchemeLeaves { get; set; }

        public List<EmployeeType> EmployeeTypes { get; set; }
        public List<EmployeeType> EmployeeGroups { get; set; }
        public List<EmployeeType> EmployeeBlocks { get; set; }
        public List<EmployeeGroup> EmpGroups { get; set; }
        public List<EmployeeGroup> EmpBlocks { get; set; }

        public List<HRMShift> Shifts { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public List<HRResponsibility> HRResponsibility { get; set; }
        public Company Company { get; set; }

        public List<SalaryScheme> SalarySchemes { get; set; }

        public List<SalaryHead> SalaryHeads { get; set; }

        public EmployeeOfficial EmployeeOfficial { get; set; }

        public DepartmentRequirementPolicy DRP { get; set; }
        
        public List<EmployeeSalaryStructureDetail> EmployeeSalaryStructureDetails { get; set; }
        public List<LeaveLedgerReport> LeaveLedgerReports { get; set; }
        
        public EmployeeSalaryStructure EmployeeSalaryStructure { get; set; }

        public List<EmployeeSalaryStructure> EmployeeSalaryStructures { get; set; }
        public List<LeaveHead> LeaveHeads { get; set; }

        public LeaveApplication LeaveApplication  { get; set; }
        public List<TransferPromotionIncrement> TPIs { get; set; }
        public List<TransferPromotionIncrement> IncrementHistorys { get; set; }
        public List<TransferPromotionIncrement> TransferHistorys { get; set; }
        public List<TransferPromotionIncrement> PromotionHistorys { get; set; }
        public List<DisciplinaryAction> DisciplinaryActions { get; set; }
        public List<EmployeeActivityNote> AcitivityNotes { get; set; }
        public List<Employee> Employees { get; set; }
        
        public List<Department> Departments { get; set; }

        public List<EmployeeConfirmation> EmployeeConfirmations { get; set; }
        

        public string FullName
        {
            get
            {
                return this.NickName +" "+ this.MiddleName +" "+ this.Name;
            }
        }
        public string NameCode
        {
            get
            {
                return this.Name +" ["+ this.Code +"] ";
            }
        }
        
        public string Params { get; set; }

        public string SignatureInBase64String { get; set; }
        #endregion

        #region Functions
        public Employee Get(int nEmployeeID, long nUserID)
        {
            return Employee.Service.Get(nEmployeeID, nUserID);
        }
        public Employee GetByCode(string sEmpCode, long nUserID)
        {
            return Employee.Service.GetByCode(sEmpCode, nUserID);
        }
        public static Employee Get(string sSQL, long nUserID)
        {
            return Employee.Service.Get(sSQL, nUserID);
        }
        public Employee IUD(int nDBOperation, long nUserID)
        {
            return Employee.Service.IUD(this, nDBOperation, nUserID);
        }
        public string DeleteImage(long nUserID)
        {
            return Employee.Service.DeleteImage(this, nUserID);
        }
        public static List<Employee> Gets(long nUserID)
        {
            return Employee.Service.Gets(nUserID);
        }
        public static List<Employee> Gets(string sSQL, long nUserID)
        {
            return Employee.Service.Gets(sSQL, nUserID);
        }
        public static List<Employee> GetsManPower(string sBUIDs, long nUserID)
        {
            return Employee.Service.GetsManPower(sBUIDs, nUserID);
        }

        public static List<Employee> GetsforPOP(long nUserID)
        {
            return Employee.Service.GetsforPOP(nUserID);
        }
        public static List<Employee> Gets(EnumEmployeeDesignationType eEmployeeType, long nUserID)
        {
            return Employee.Service.Gets((int)eEmployeeType, nUserID);
        }
        public static List<Employee> BUGets(EnumEmployeeDesignationType eEmployeeType, int BUID, long nUserID)
        {
            return Employee.Service.BUGets((int)eEmployeeType, BUID, nUserID);
        }
        public static List<Employee> Gets(EnumEmployeeDesignationType eEmployeeType, int nLocationID, long nUserID)
        {
            return Employee.Service.Gets((int)eEmployeeType, nLocationID, nUserID);
        }

        public static List<Employee> GetsByOperationEvent(int nLocationID, string sObjectName, string sOperaationEvent, long nUserID)
        {
            return Employee.Service.GetsByOperationEvent(nLocationID, sObjectName, sOperaationEvent, nUserID);
        }

        public static List<Employee> TransferShift(string sEmployeeIDs, int nCurrentShiftID, DateTime dDate, long nUserID)
        {
            return Employee.Service.TransferShift(sEmployeeIDs, nCurrentShiftID,dDate, nUserID);
        }
        public static Employee SwapShift( int nRosterPlanID,DateTime dDate, long nUserID)
        {
            return Employee.Service.SwapShift(nRosterPlanID,dDate, nUserID);
        }

        public static List<Employee> Activite(string sEmpIDs, long nUserID)
        {
            return Employee.Service.Activite(sEmpIDs, nUserID);
        }

        public static List<Employee> EmployeeWorkingStatusChange(string sEmpIDs, long nUserID)
        {
            return Employee.Service.EmployeeWorkingStatusChange(sEmpIDs, nUserID);
        }

        public static List<Employee> ContinuedEmployee(string sEmpIDs, long nUserID)
        {
            return Employee.Service.ContinuedEmployee(sEmpIDs, nUserID);
        }

        public Employee EmployeeBasicInformation_IUD(int nDBOperation, long nUserID)
        {
            return Employee.Service.EmployeeBasicInformation_IUD(this, nDBOperation, nUserID);
        }

        public string EmployeeImageIU(long nUserID)
        {
            return Employee.Service.EmployeeImageIU(this, nUserID);
        }

        public static string GetGeneratedEmpCode(int nDRPId, int nDesignationId, DateTime JoinningDate, int nCompanyId, long nUserID)
        {
            return Employee.Service.GetGeneratedEmpCode(nDRPId, nDesignationId,JoinningDate, nCompanyId, nUserID);
        }

        public static Employee SaveSignature(int nEmployeeID, byte[] imgSingnature, Int64 nUserID)
        {
            return Employee.Service.SaveSignature(nEmployeeID, imgSingnature, nUserID);
        }
        public static Employee RemoveSignature(int nEmployeeID, Int64 nUserID)
        {
            return Employee.Service.RemoveSignature(nEmployeeID, nUserID);
        }

        public static List<Employee> UploadXL(List<Employee_UploadXL> oEXLs, long nUserID)
        {
            return Employee.Service.UploadXL(oEXLs, nUserID);
        }

        public static List<Employee> UploadEmpBasicXL(List<Employee_UploadXL> oEXLs, long nUserID)
        {
            return Employee.Service.UploadEmpBasicXL(oEXLs, nUserID);
        }

        public static Employee UploadEmpBasicXLWithConfig(string sSql, long nUserID)
        {
            return Employee.Service.UploadEmpBasicXLWithConfig(sSql, nUserID);
        }
        
        public Employee EditDateOfJoin(long nUserID)
        {
            return Employee.Service.EditDateOfJoin(this, nUserID);
        }
        
        public static Employee EmployeeRecontract(int EmployeeID, DateTime StartDate, DateTime EndDate, string sNewCode, int nCategory, long nUserID)
        {
            return Employee.Service.EmployeeRecontract(EmployeeID, StartDate, EndDate, sNewCode,nCategory, nUserID);
        }

        #endregion

        #region Non DB Function
        public static string IDString(List<Employee> EmployeeList)
        {
            string sPIDs = "";
            foreach (Employee oItem in EmployeeList)
            {
                sPIDs = sPIDs + oItem.EmployeeID.ToString() + ",";
            }
            if (sPIDs.Length > 0)
                sPIDs = sPIDs.Remove(sPIDs.Length - 1, 1);
            return sPIDs;
        }
        public static List<Employee> CopyObject(List<Employee> oTempEmployees)
        {
            List<Employee> oEmployees = new List<Employee>();
            foreach (Employee oItem in oTempEmployees)
            {
                oEmployees.Add(oItem);
            }
            return oEmployees;
        }
        public static List<Employee> GetEmpBYIndex(List<Employee> oTempEmployees, int nEmpID)
        {
            List<Employee> oEmployees = new List<Employee>();
            foreach (Employee oItem in oTempEmployees)
            {
                if (oItem.EmployeeID == nEmpID)
                {
                    oEmployees.Add(oItem);
                }
            }
            return oEmployees;
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeService Service
        {
            get { return (IEmployeeService)Services.Factory.CreateService(typeof(IEmployeeService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployee interface
    
    public interface IEmployeeService
    {
        Employee Get(int nEmployeeID, Int64 nUserId);
        Employee GetByCode(string sEmpCode, Int64 nUserId);
        Employee Get(string sSQL, Int64 nUserId);
        List<Employee> Gets(Int64 nUserID);
        List<Employee> Gets(string sSQL, Int64 nUserID);
        List<Employee> GetsManPower(string sBUIDs, Int64 nUserID);
        Employee IUD(Employee oEmployee, int nDBOperation, Int64 nUserID);
        string DeleteImage(Employee oEmployee, Int64 nUserID);
        List<Employee> GetsforPOP(Int64 nUserID);
        List<Employee> Gets(int eEmployeeType, Int64 nUserID);
        List<Employee> BUGets(int eEmployeeType, int BUID, Int64 nUserID);
        List<Employee> Gets(int eEmployeeType, int nLocationID, Int64 nUserID);
        List<Employee> GetsByOperationEvent(int nLocationID, string sObjectName, string sOperaationEvent, Int64 nUserID);
        List<Employee> TransferShift(string sEmployeeIDs, int nCurrentShiftID,DateTime dDate, Int64 nUserID);
        Employee SwapShift(int nRosterPlanID,DateTime dDate, Int64 nUserID);
        List<Employee> Activite(string sEmpIDs, Int64 nUserID);
        List<Employee> EmployeeWorkingStatusChange(string sEmpIDs, Int64 nUserID);
        List<Employee> ContinuedEmployee(string sEmpIDs, Int64 nUserID);
        Employee EmployeeBasicInformation_IUD(Employee Employee, int nDBOperation, Int64 nUserID);
        string EmployeeImageIU(Employee oEmployee,  Int64 nUserID);
        string GetGeneratedEmpCode(int nDRPId, int nDesignationId, DateTime JoinningDate, int nCompanyId, Int64 nUserID);
        Employee SaveSignature(int nEmployeeID, byte[] imgSingnature, Int64 nUserID);
        Employee RemoveSignature(int nEmployeeID, Int64 nUserID);
        List<Employee> UploadXL(List<Employee_UploadXL> oEXLs, Int64 nUserID);
        List<Employee> UploadEmpBasicXL(List<Employee_UploadXL> oEXLs, Int64 nUserID);
        Employee UploadEmpBasicXLWithConfig(string sSql, Int64 nUserID);
        Employee EditDateOfJoin(Employee oEmployee, Int64 nUserID);
        Employee EmployeeRecontract(int EmployeeID, DateTime StartDate, DateTime EndDate, string sNewCode, int nCategory, Int64 nUserID);

    }
    #endregion
   
}

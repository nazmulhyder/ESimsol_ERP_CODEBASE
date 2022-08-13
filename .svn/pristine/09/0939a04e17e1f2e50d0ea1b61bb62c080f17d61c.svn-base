using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;

namespace ESimSol.BusinessObjects
{
    public class EmployeeSalaryV2 : BusinessObject
    {
        public EmployeeSalaryV2()
        {
            EmployeeSalaryID = 0;
            EmployeeID = 0;
            LocationID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            GrossAmount = 0;
            NetAmount = 0;
            CurrencyID = 0;
            SalaryDate = DateTime.Now;
            SalaryReceiveDate = DateTime.Now;
            PayrollProcessID = 0;
            IsManual = true;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            IsLock = true;
            ProductionAmount = 0;
            ProductionBonus = 0;
            OTHour = 0;
            OTRatePerHour = 0;
            TotalWorkingDay = 0;
            TotalAbsent = 0;
            TotalLate = 0;
            TotalEarlyLeaving = 0;
            TotalDayOff = 0;
            TotalHoliday = 0;
            TotalUpLeave = 0;
            TotalPLeave = 0;
            RevenueStemp = 0;
            TotalNoWorkDay = 0;
            TotalNoWorkDayAllowance = 0;
            AddShortFall = 0;
            IsProductionBase = true;
            IsAllowOverTime = true;
            ErrorMessage = "";
            EmployeeTypeName = "";
            Gender = "";
            CashAmount = 0;
            BankAmount = 0;
            BUID = 0;
            BUName = "";
            BMMID = 0;
            BlockName = "";
            LateInMin = 0;
            IsOutSheet = false;
            MonthID = 0;
            ETIN = "";
            EmployeeCode = "";
            EmployeeName = "";
            DepartmentName = "";
            DesignationName = "";
            JoiningDate = DateTime.Now;
            BankName = "";
            AccountNo = "";
            BUShortName = "";
            Grade = "";
            EmployeeCodeSL = 0;
            LastGross = 0;
            LastIncrement = 0;
            IncrementEffectDate = DateTime.Now;
            PresentSalary = 0;
            Photo = null;
            Signature = null;
            DefineOTHour = 0;
            ExtraOTHour = 0;
            PaymentType = "";
            EarlyOutInMin = 0;
            DayOfRegularOTHour = 0;
            LocationName = "";
            ContactNo = "";
            EmployeeSalaryBasics = new List<EmployeeSalaryDetailV2>();
            EmployeeSalaryAllowances = new List<EmployeeSalaryDetailV2>();
            EmployeeSalaryDeductions = new List<EmployeeSalaryDetailV2>();
            EmployeeWiseLeaveStatus = new List<LeaveStatus>();

            #region Non DB Property
            BasicAmount = 0;
            HouseRentAmount = 0;
            ConveyanceAmount = 0;
            FoodAmount = 0;
            MedicalAmount = 0;
            AbsentAmount= 0;
            StampAmount = 0;
            AttendanceBonus = 0;
            CL = 0;
            ML = 0;
            EL = 0;
            LWP = 0;
            #endregion
        }
        
        #region Properties
        public int EmployeeSalaryID { get; set; }
        public int EmployeeID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public double GrossAmount { get; set; }
        public double NetAmount { get; set; }
        public double TotalOT { get; set; }
        public double LastGross { get; set; }
        public double LastIncrement { get; set; }
        public double PresentSalary { get; set; }
        public DateTime IncrementEffectDate { get; set; }
        public int CurrencyID { get; set; }
        public int EmployeeCodeSL { get; set; }
        public DateTime SalaryDate { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime SalaryReceiveDate { get; set; }
        public int PayrollProcessID { get; set; }
        public int EmpCount { get; set; }
        public bool IsManual { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsLock { get; set; }
        public double ProductionAmount { get; set; }
        public double ProductionBonus { get; set; }
        public double OTHour { get; set; }
        public double OTRatePerHour { get; set; }
        public int TotalWorkingDay { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalLate { get; set; }
        public int TotalEarlyLeaving { get; set; }
        public int TotalDayOff { get; set; }
        public int TotalHoliday { get; set; }
        public int TotalUpLeave { get; set; }
        public int TotalPLeave { get; set; }
        public double RevenueStemp { get; set; }
        public int TotalNoWorkDay { get; set; }
        public double TotalNoWorkDayAllowance { get; set; }
        public double AddShortFall { get; set; }
        public bool IsProductionBase { get; set; }
        public bool IsAllowOverTime { get; set; }
        public string ErrorMessage { get; set; }
        public string EmployeeTypeName { get; set; }
        public string Gender { get; set; }
        public double CashAmount { get; set; }
        public double BankAmount { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public int BMMID { get; set; }
        public string BlockName { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public int LateInMin { get; set; }
        public bool IsOutSheet { get; set; }
        public int MonthID { get; set; }
        public string ETIN { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Grade { get; set; }
        public byte[] Photo { get; set; }
        public byte[] Signature { get; set; }
        public double DefineOTHour { get; set; }
        public double ExtraOTHour { get; set; }
        public string PaymentType { get; set; }
        public int EarlyOutInMin { get; set; }
        public double DayOfRegularOTHour { get; set; }
        public string LocationName { get; set; }
        public string ContactNo { get; set; }
        public System.Drawing.Image EmployeePhoto { get; set; }
        public System.Drawing.Image EmployeeSiganture { get; set; }
        public List<EmployeeSalaryDetailV2> EmployeeSalaryBasics { get; set; }
        public List<EmployeeSalaryDetailV2> EmployeeSalaryAllowances { get; set; }
        public List<EmployeeSalaryDetailV2> EmployeeSalaryDeductions { get; set; }
        public List<LeaveStatus> EmployeeWiseLeaveStatus { get; set; }
        #endregion

        #region Derived Properties
        public double DeriveNetAmount
        {
            get
            {
                if (this.NetAmount < 0) return 0; else return NetAmount;

            }
        }


        public string SalaryDateInString
        {
            get
            {
                return this.SalaryDate.ToString("dd MMM yyyy");
            }
        }

        public string SalaryForInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy") + " To " + EndDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Non DB Property
        public double BasicAmount { get; set; }
        public double HouseRentAmount { get; set; }
          public double ConveyanceAmount { get; set; }
          public double FoodAmount { get; set; }
          public double MedicalAmount { get; set; }
          public double AbsentAmount { get; set; }
          public double StampAmount { get; set; }
          public double AttendanceBonus { get; set; }

          public int CL { get; set; }
          public int ML { get; set; }
          public int EL { get; set; }
          public int LWP { get; set; }
        #endregion


        #region Function
        public static List<EmployeeSalaryV2> CompGets(string sSQL, DateTime SalaryStartDate, DateTime SalaryEndDate, long nUserID)
        {
            return EmployeeSalaryV2.Service.CompGets(sSQL, SalaryStartDate, SalaryEndDate,nUserID);
        }

        public static List<EmployeeSalaryV2> Gets(string sSQL, DateTime SalaryStartDate, DateTime SalaryEndDate, long nUserID)
        {
            return EmployeeSalaryV2.Service.Gets(sSQL, SalaryStartDate, SalaryEndDate,nUserID);
        }
        #endregion

        #region Non DB Functions
        public  static List<EmployeeSalaryV2> MapSalaryDetails(List<EmployeeSalaryV2> oEmployeeSalarys, List<EmployeeSalaryDetailV2> oEmployeeSalaryDetails)
        {
            foreach (EmployeeSalaryV2 oEmployeeSalary in oEmployeeSalarys)
            {
                oEmployeeSalary.EmployeeSalaryBasics = oEmployeeSalaryDetails.Where(X => X.EmployeeSalaryID == oEmployeeSalary.EmployeeSalaryID && X.SalaryHeadType==EnumSalaryHeadType.Basic).OrderBy(X => X.SalaryHeadSequence).ToList();
                oEmployeeSalary.EmployeeSalaryAllowances = oEmployeeSalaryDetails.Where(X => X.EmployeeSalaryID == oEmployeeSalary.EmployeeSalaryID && X.SalaryHeadType == EnumSalaryHeadType.Addition).OrderBy(X => X.SalaryHeadSequence).ToList();
                oEmployeeSalary.EmployeeSalaryDeductions = oEmployeeSalaryDetails.Where(X => X.EmployeeSalaryID == oEmployeeSalary.EmployeeSalaryID && X.SalaryHeadType == EnumSalaryHeadType.Deduction).OrderBy(X => X.SalaryHeadSequence).ToList();
            }
            return oEmployeeSalarys; 
        }

        public static List<EmployeeSalaryV2> MapAMGSalaryDetails(List<EmployeeSalaryV2> oEmployeeSalarys, List<EmployeeSalaryDetailV2> oEmployeeSalaryDetails)
        {
            List<EmployeeSalaryDetailV2> oTempEmployeeSalaryDetails = new List<EmployeeSalaryDetailV2>();
            foreach (EmployeeSalaryV2 oEmployeeSalary in oEmployeeSalarys)
            {
                oTempEmployeeSalaryDetails = new List<EmployeeSalaryDetailV2>();
                oTempEmployeeSalaryDetails = oEmployeeSalaryDetails.Where(X => X.EmployeeSalaryID == oEmployeeSalary.EmployeeSalaryID).OrderBy(X => X.SalaryHeadSequence).ToList();

                oEmployeeSalary.BasicAmount = oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 1).ToList().Count > 0 ? oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 1).FirstOrDefault().Amount : 0; //Here 1 Means BasicSalaryHeadID (Fixed)
                oEmployeeSalary.HouseRentAmount = oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 2).ToList().Count > 0 ? oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 2).FirstOrDefault().Amount : 0; //Here 2 Means HouseRentSalaryHeadID (Fixed)
                oEmployeeSalary.MedicalAmount = oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 5).ToList().Count > 0 ? oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 5).FirstOrDefault().Amount : 0; //Here 5 Means MedicalSalaryHeadID (Fixed)
                oEmployeeSalary.FoodAmount = oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 4).ToList().Count > 0 ? oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 4).FirstOrDefault().Amount : 0; //Here 4 Means FoodSalaryHeadID (Fixed)
                oEmployeeSalary.ConveyanceAmount = oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 3).ToList().Count > 0 ? oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 3).FirstOrDefault().Amount : 0; //Here 3 Means ConveyanceSalaryHeadID (Fixed)
                oEmployeeSalary.AbsentAmount = oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 8).ToList().Count > 0 ? oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 8).FirstOrDefault().Amount : 0; //Here 8 Means AbsentSalaryHeadID (Fixed)
                oEmployeeSalary.StampAmount = oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 10).ToList().Count > 0 ? oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 10).FirstOrDefault().Amount : 0; //Here 10 Means StampSalaryHeadID (Fixed)
                oEmployeeSalary.AttendanceBonus = oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 7).ToList().Count > 0 ? oTempEmployeeSalaryDetails.Where(X => X.SalaryHeadID == 7).FirstOrDefault().Amount : 0; //Here 7 Means AttdBonusSalaryHeadID (Fixed)
                
            }
            return oEmployeeSalarys;
        }

        public static List<EmployeeSalaryV2> MapLeaveStatus(List<EmployeeSalaryV2> oEmployeeSalarys, List<LeaveStatus> oLeaveStatus)
        {
            foreach (EmployeeSalaryV2 oEmployeeSalary in oEmployeeSalarys)
            {

                oEmployeeSalary.EmployeeWiseLeaveStatus = oLeaveStatus.Where(X => X.EmployeeID == oEmployeeSalary.EmployeeID ).ToList();
            }
            return oEmployeeSalarys;
        }

        public static List<EmployeeSalaryV2> MapAMGLeaveStatus(List<EmployeeSalaryV2> oEmployeeSalarys, List<LeaveStatus> oLeaveStatus)
        {
            List<LeaveStatus> oTempLeaveStatus = new List<LeaveStatus>();
            foreach (EmployeeSalaryV2 oEmployeeSalary in oEmployeeSalarys)
            {
                oTempLeaveStatus = new List<LeaveStatus>();
                oTempLeaveStatus = oLeaveStatus.Where(X => X.EmployeeID == oEmployeeSalary.EmployeeID).ToList();
                oEmployeeSalary.CL =oTempLeaveStatus.Where(x => x.LeaveHeadID == 1).ToList().Count>0? oTempLeaveStatus.Where(x => x.LeaveHeadID == 1).Select(x => x.LeaveDays).FirstOrDefault():0;//Here 1 Means CL (Fixed)
                oEmployeeSalary.ML = oTempLeaveStatus.Where(x => x.LeaveHeadID == 2).ToList().Count > 0 ? oTempLeaveStatus.Where(x => x.LeaveHeadID == 2).Select(x => x.LeaveDays).FirstOrDefault() : 0;//Here 2 Means SL (Fixed)
                oEmployeeSalary.EL = oTempLeaveStatus.Where(x => x.LeaveHeadID == 3).ToList().Count > 0 ? oTempLeaveStatus.Where(x => x.LeaveHeadID == 3).Select(x => x.LeaveDays).FirstOrDefault() : 0;//Here 3 Means EL (Fixed)
                oEmployeeSalary.LWP = oTempLeaveStatus.Where(x => x.LeaveHeadID == 4).ToList().Count > 0 ? oTempLeaveStatus.Where(x => x.LeaveHeadID == 4).Select(x => x.LeaveDays).FirstOrDefault() : 0;//Here 4 Means LWP (Fixed)
            }
            return oEmployeeSalarys;
        }

        public static List<EmployeeSalaryV2> MapEmployeeImages(List<EmployeeSalaryV2> oEmployeeSalarys, List<Employee> oEmployees)
        {
            foreach (EmployeeSalaryV2 oEmployeeSalary in oEmployeeSalarys)
            {
                oEmployeeSalary.Photo = oEmployees.Where(x => x.EmployeeID==oEmployeeSalary.EmployeeID).Select(x => x.Photo).FirstOrDefault();
                oEmployeeSalary.Signature = oEmployees.Where(x => x.EmployeeID == oEmployeeSalary.EmployeeID).Select(x => x.Signature).FirstOrDefault();
            }
            return oEmployeeSalarys;
        }

     
      
        #endregion

        #region ServiceFactory

        internal static IEmployeeSalaryV2Service Service
        {
            get { return (IEmployeeSalaryV2Service)Services.Factory.CreateService(typeof(IEmployeeSalaryV2Service)); }
        }
        #endregion
    }

    #region IEmployeeSalaryV2interface


    public interface IEmployeeSalaryV2Service
    {
        List<EmployeeSalaryV2> CompGets(string sSQL, DateTime SalaryStartDate, DateTime SalaryEndDate, Int64 nUserID);
        List<EmployeeSalaryV2> Gets(string sSQL, DateTime SalaryStartDate, DateTime SalaryEndDate, Int64 nUserID);
    }
    #endregion

}

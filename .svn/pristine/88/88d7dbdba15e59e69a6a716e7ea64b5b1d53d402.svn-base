using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    #region SalaryFieldSetup
    public class SalaryFieldSetup : BusinessObject
    {
        public SalaryFieldSetup()
        {
            SalaryFieldSetupID = 0;
            SetupNo = "";
            SalaryFieldSetupName = "";
            PageOrientation = EnumPageOrientation.None;
            PageOrientationInt = 0;
            Remarks = "";
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            UserName = "";
            LogInID = "";
            SalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();
            ErrorMessage = "";
        }

        #region Properties
        public int SalaryFieldSetupID { get; set; }
        public string SetupNo { get; set; }
        public string SalaryFieldSetupName { get; set; }
        public EnumPageOrientation PageOrientation { get; set; }
        public int PageOrientationInt { get; set; }
        public string Remarks { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string UserName { get; set; }
        public string LogInID { get; set; }
        public List<SalaryFieldSetupDetail> SalaryFieldSetupDetails { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string PageOrientationSt
        {
            get
            {
                return EnumObject.jGet(this.PageOrientation);
            }
        }
        #endregion

        #region Functions
        public static List<SalaryFieldSetup> Gets(long nUserID)
        {
            return SalaryFieldSetup.Service.Gets(nUserID);
        }

        public SalaryFieldSetup Get(int id, long nUserID)
        {
            return SalaryFieldSetup.Service.Get(id, nUserID);
        }

        public SalaryFieldSetup Save(long nUserID)
        {
            return SalaryFieldSetup.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return SalaryFieldSetup.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISalaryFieldSetupService Service
        {
            get { return (ISalaryFieldSetupService)Services.Factory.CreateService(typeof(ISalaryFieldSetupService)); }
        }
        #endregion
    }
    #endregion

    #region ISalaryFieldSetup interface
    public interface ISalaryFieldSetupService
    {
        List<SalaryFieldSetup> Gets(Int64 nUserID);
        SalaryFieldSetup Get(int id, Int64 nUserID);
        SalaryFieldSetup Save(SalaryFieldSetup oSalaryFieldSetup, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
    }
    #endregion

    #region SalaryFieldSetup
    public class SalaryField
    {
        public SalaryField()
        {
            EmployeeCode = false;
            EmployeeName = false;
            Designation = false;
            JoiningDate = false;
            EmployeeType = false;
            PaymentType = false;
            TotalDays = false;
            PresentDay = false;
            Day_off_Holidays = false;
            AbsentDays = false;
            LeaveDays = false;
            Employee_Working_Days = false;
            Early_Out_Days = false;
            Early_Out_Mins = false;
            LateDays = false;
            LateHrs = false;
            DefineOTHour = false;
            ExtraOTHour = false;
            OTHours = false;
            OTRate = false;
            LastGross = false;
            LastIncrement = false;
            Increment_Effect_Date = false;
            PresentSalary = false;
            OTAllowance = false;
            BankAmount = false;
            CashAmount = false;
            AccountNo = false;
            BankName = false;
            Grade = false;
            LeaveDetail = false;
            FixedColumn = 0;
            TotalEmpInfoCol = 0;
            TotalAttDetailColPrev = 0;
            TotalAttDetailColPost = 0;
            TotalIncrementDetailCol = 0;
            PresentSalaryCount = 0;
            OTAllowanceCount = 0;
            BankAmountCount = 0;
            CashAmountCount = 0;
            AccountNoCount = 0;
            BankNameCount = 0;
        }

        #region Properties
        public bool EmployeeCode { get; set; }
        public bool EmployeeName { get; set; }
        public bool Designation { get; set; }
        public bool JoiningDate { get; set; }
        public bool EmployeeType { get; set; }
        public bool PaymentType { get; set; }
        public bool TotalDays { get; set; }
        public bool PresentDay { get; set; }
        public bool Day_off_Holidays { get; set; }
        public bool AbsentDays { get; set; }
        public bool LeaveDays { get; set; }
        public bool Employee_Working_Days { get; set; }
        public bool Early_Out_Days { get; set; }
        public bool Early_Out_Mins { get; set; }
        public bool LateDays { get; set; }
        public bool LateHrs { get; set; }
        public bool DefineOTHour { get; set; }
        public bool ExtraOTHour { get; set; }
        public bool OTHours { get; set; }
        public bool OTRate { get; set; }
        public bool LastGross { get; set; }
        public bool LastIncrement { get; set; }
        public bool Increment_Effect_Date { get; set; }
        public bool PresentSalary { get; set; }
        public bool OTAllowance { get; set; }
        public bool BankAmount { get; set; }
        public bool CashAmount { get; set; }
        public bool AccountNo { get; set; }
        public bool BankName { get; set; }
        public bool Grade { get; set; }
        public bool LeaveDetail { get; set; }
        public int FixedColumn { get; set; }
        public int TotalEmpInfoCol { get; set; }
        public int TotalAttDetailColPrev { get; set; }
        public int TotalAttDetailColPost { get; set; }
        public int TotalIncrementDetailCol { get; set; }
        public int PresentSalaryCount { get; set; }
        public int OTAllowanceCount { get; set; }
        public int BankAmountCount { get; set; }
        public int CashAmountCount { get; set; }
        public int AccountNoCount { get; set; }
        public int BankNameCount { get; set; }

        #endregion


        public static SalaryField MapSalaryFieldExists(List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails)
        {
            SalaryField oSalaryField = new SalaryField();
            oSalaryField.EmployeeCode = IsFieldExists(EnumSalaryField.EmployeeCode, oSalaryFieldSetupDetails);
            oSalaryField.EmployeeName = IsFieldExists(EnumSalaryField.EmployeeName, oSalaryFieldSetupDetails);
            oSalaryField.Designation = IsFieldExists(EnumSalaryField.Designation, oSalaryFieldSetupDetails);
            oSalaryField.JoiningDate = IsFieldExists(EnumSalaryField.JoiningDate, oSalaryFieldSetupDetails);
            oSalaryField.EmployeeType = IsFieldExists(EnumSalaryField.EmployeeType, oSalaryFieldSetupDetails);
            oSalaryField.PaymentType = IsFieldExists(EnumSalaryField.PaymentType, oSalaryFieldSetupDetails);
            oSalaryField.TotalDays = IsFieldExists(EnumSalaryField.TotalDays, oSalaryFieldSetupDetails);
            oSalaryField.PresentDay = IsFieldExists(EnumSalaryField.PresentDay, oSalaryFieldSetupDetails);
            oSalaryField.Day_off_Holidays = IsFieldExists(EnumSalaryField.Day_off_Holidays, oSalaryFieldSetupDetails);
            oSalaryField.AbsentDays = IsFieldExists(EnumSalaryField.AbsentDays, oSalaryFieldSetupDetails);
            oSalaryField.LeaveDays = IsFieldExists(EnumSalaryField.LeaveDays, oSalaryFieldSetupDetails);
            oSalaryField.Employee_Working_Days = IsFieldExists(EnumSalaryField.Employee_Working_Days, oSalaryFieldSetupDetails);
            oSalaryField.Early_Out_Days = IsFieldExists(EnumSalaryField.Early_Out_Days, oSalaryFieldSetupDetails);
            oSalaryField.Early_Out_Mins = IsFieldExists(EnumSalaryField.Early_Out_Mins, oSalaryFieldSetupDetails);
            oSalaryField.LateDays = IsFieldExists(EnumSalaryField.LateDays, oSalaryFieldSetupDetails);
            oSalaryField.LateHrs = IsFieldExists(EnumSalaryField.LateHrs, oSalaryFieldSetupDetails);
            oSalaryField.DefineOTHour = IsFieldExists(EnumSalaryField.DefineOTHour, oSalaryFieldSetupDetails);
            oSalaryField.ExtraOTHour = IsFieldExists(EnumSalaryField.ExtraOTHour, oSalaryFieldSetupDetails);
            oSalaryField.OTHours = IsFieldExists(EnumSalaryField.OTHours, oSalaryFieldSetupDetails);
            oSalaryField.OTRate = IsFieldExists(EnumSalaryField.OTRate, oSalaryFieldSetupDetails);
            oSalaryField.LastGross = IsFieldExists(EnumSalaryField.LastGross, oSalaryFieldSetupDetails);
            oSalaryField.LastIncrement = IsFieldExists(EnumSalaryField.LastIncrement, oSalaryFieldSetupDetails);
            oSalaryField.Increment_Effect_Date = IsFieldExists(EnumSalaryField.Increment_Effect_Date, oSalaryFieldSetupDetails);
            oSalaryField.PresentSalary = IsFieldExists(EnumSalaryField.PresentSalary, oSalaryFieldSetupDetails);
            oSalaryField.OTAllowance = IsFieldExists(EnumSalaryField.OTAllowance, oSalaryFieldSetupDetails);
            oSalaryField.BankAmount = IsFieldExists(EnumSalaryField.BankAmount, oSalaryFieldSetupDetails);
            oSalaryField.CashAmount = IsFieldExists(EnumSalaryField.CashAmount, oSalaryFieldSetupDetails);
            oSalaryField.AccountNo = IsFieldExists(EnumSalaryField.AccountNo, oSalaryFieldSetupDetails);
            oSalaryField.BankName = IsFieldExists(EnumSalaryField.BankName, oSalaryFieldSetupDetails);
            oSalaryField.Grade = IsFieldExists(EnumSalaryField.Grade, oSalaryFieldSetupDetails);
            oSalaryField.LeaveDetail = IsFieldExists(EnumSalaryField.LeaveDetail, oSalaryFieldSetupDetails);
            SalaryField oSalaryFieldColCount = new SalaryField();
            oSalaryFieldColCount=ColCount(oSalaryField);
            oSalaryField.FixedColumn = oSalaryFieldColCount.FixedColumn;
            oSalaryField.TotalEmpInfoCol = oSalaryFieldColCount.TotalEmpInfoCol;
            oSalaryField.TotalAttDetailColPrev = oSalaryFieldColCount.TotalAttDetailColPrev;
            oSalaryField.TotalAttDetailColPost = oSalaryFieldColCount.TotalAttDetailColPost;
            oSalaryField.TotalIncrementDetailCol = oSalaryFieldColCount.TotalIncrementDetailCol;
            oSalaryField.PresentSalaryCount = oSalaryFieldColCount.PresentSalaryCount;
            oSalaryField.OTAllowanceCount = oSalaryFieldColCount.OTAllowanceCount;
            oSalaryField.BankAmountCount = oSalaryFieldColCount.BankAmountCount;
            oSalaryField.CashAmountCount = oSalaryFieldColCount.CashAmountCount;
            oSalaryField.AccountNoCount = oSalaryFieldColCount.AccountNoCount;
            oSalaryField.BankNameCount = oSalaryFieldColCount.BankNameCount;
            return oSalaryField;
            
        }

        private static bool IsFieldExists(EnumSalaryField eEnumSalaryField, List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails)
        {
            foreach (SalaryFieldSetupDetail oItem in oSalaryFieldSetupDetails)
            {
                if (oItem.SalaryField == eEnumSalaryField)
                {
                    return true;
                }
            }
            return false;
        }

        private static SalaryField ColCount(SalaryField oSalaryField)
        {
            SalaryField oSalaryFieldColCount = new SalaryField();
            int nFixedColumn = 0, nTotalAttDetailColPrev = 0, nTotalAttDetailColPost = 0, nTotalIncrementDetailCol=0;
            if (oSalaryField.EmployeeCode)
            {
                nFixedColumn++;
            }
            if (oSalaryField.EmployeeName)
            {
                nFixedColumn++;
            }
            if (oSalaryField.Designation)
            {
                nFixedColumn++;
            }
            if (oSalaryField.JoiningDate)
            {
                nFixedColumn++;
            }
            if (oSalaryField.EmployeeType)
            {
                nFixedColumn++;
            }
            if (oSalaryField.PaymentType)
            {
                nFixedColumn++;
            }
            if (oSalaryField.Grade)
            {
                nFixedColumn++;
            }
            oSalaryFieldColCount.TotalEmpInfoCol = nFixedColumn;

            if (oSalaryField.TotalDays)
            {
                nFixedColumn++;
                nTotalAttDetailColPrev++;
            }

            if (oSalaryField.PresentDay)
            {
                nFixedColumn++;
                nTotalAttDetailColPrev++;
            }

            if (oSalaryField.Day_off_Holidays)
            {
                nFixedColumn++;
                nTotalAttDetailColPrev++;
            }

            if (oSalaryField.AbsentDays)
            {
                nFixedColumn++;
                nTotalAttDetailColPrev++;
            }
       
            if (oSalaryField.LeaveDays)
            {
                nFixedColumn++;
                nTotalAttDetailColPost++;
            }

            if (oSalaryField.Employee_Working_Days)
            {
                nFixedColumn++;
                nTotalAttDetailColPost++;
            }

            if (oSalaryField.Early_Out_Days)
            {
                nFixedColumn++;
                nTotalAttDetailColPost++;
            }

            if (oSalaryField.Early_Out_Mins)
            {
                nFixedColumn++;
                nTotalAttDetailColPost++;
            }

            if (oSalaryField.LateDays)
            {
                nFixedColumn++;
                nTotalAttDetailColPost++;
            }

            if (oSalaryField.LateHrs)
            {
                nFixedColumn++;
                nTotalAttDetailColPost++;
            }

            if (oSalaryField.DefineOTHour)
            {
                nFixedColumn++;
                oSalaryFieldColCount.OTAllowanceCount++;
            }

            if (oSalaryField.ExtraOTHour)
            {
                nFixedColumn++;
                oSalaryFieldColCount.OTAllowanceCount++;
            }

            if (oSalaryField.OTHours)
            {
                nFixedColumn++;
                oSalaryFieldColCount.OTAllowanceCount++;
            }
            if (oSalaryField.OTRate)
            {
                nFixedColumn++;
                oSalaryFieldColCount.OTAllowanceCount++;
            }
            oSalaryFieldColCount.TotalAttDetailColPrev=nTotalAttDetailColPrev;
            oSalaryFieldColCount.TotalAttDetailColPost = nTotalAttDetailColPost;

            if (oSalaryField.LastGross)
            {
                nFixedColumn++;
                nTotalIncrementDetailCol++;
            }

            if (oSalaryField.LastIncrement)
            {
                nFixedColumn++;
                nTotalIncrementDetailCol++;
            }

            if (oSalaryField.Increment_Effect_Date)
            {
                nFixedColumn++;
                nTotalIncrementDetailCol++;
            }
            oSalaryFieldColCount.TotalIncrementDetailCol = nTotalIncrementDetailCol;
            if (oSalaryField.PresentSalary)
            {
                nFixedColumn++;
                oSalaryFieldColCount.PresentSalaryCount = 1;
            }
            if (oSalaryField.OTAllowance)
            {
                nFixedColumn++;
                oSalaryFieldColCount.OTAllowanceCount++;
            }
            if (oSalaryField.BankAmount)
            {
                nFixedColumn++;
                oSalaryFieldColCount.BankAmountCount = 1;
            }
            if (oSalaryField.CashAmount)
            {
                nFixedColumn++;
                oSalaryFieldColCount.CashAmountCount = 1;
            }
            if (oSalaryField.AccountNo)
            {
                nFixedColumn++;
                oSalaryFieldColCount.AccountNoCount = 1;
            }

            if (oSalaryField.BankName)
            {
                nFixedColumn++;
                oSalaryFieldColCount.BankNameCount = 1;
            }
            oSalaryFieldColCount.FixedColumn = nFixedColumn;
            return oSalaryFieldColCount;
        }


    }
    #endregion
}
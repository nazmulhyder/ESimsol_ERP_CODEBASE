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
    #region FinalSettlement

    public class FinalSettlement : BusinessObject
    {
        public FinalSettlement()
        {
            EmployeeSettlementID = 0;
            EmployeeID = 0;
            PaymentDate = DateTime.Now;
            EmployeeName = "";
            EmployeeNameBangla = "";
            EmployeeCode = "";
            EmployeeCodeBangla = "";
            BUID = 0;
            BUName = "";
            BUNameBangla = "";
            DesignationID = 0;
            DesignationName = "";
            DesignationNameBangla = "";
            DepartmentID = 0;
            DeptName = "";
            DrptNameBangla = "";
            DateOfJoin = DateTime.Now;
            DateOfSeparation = DateTime.Now;
            JobDurationYears = 0;
            JobDurationMonths = 0;
            JobDurationDays = 0;
            GrossSalary = 0;
            BasicSalary = 0;
            PerDayGross = 0;
            PerDayBasic = 0;
            OTRatePerHour = 0;
            PayableDays = 0;
            PayableAmount = 0;
            OTHour = 0;
            OTAmount = 0;
            AttendanceBonus = 0;
            FestivalBonus = 0;
            EarnLeaveDays = 0;
            EarnLeaveAmount = 0;
            ServiceBenefitDays = 0;
            ServiceBenefitAmount = 0;
            TerminationBenefitDays = 0;
            TerminationBenefitAmount = 0;
            TotalPayableAmount = 0;
            AbsentDays = 0;
            AbsentAmount = 0;
            NoticePeriodDays = 0;
            NoticePeriodAmount = 0;
            PaidEarnLeaveDays = 0;
            PaidEarnLeaveAmount = 0;
            StampAmount = 0;
            OthersAmount = 0;
            TotalDeductionAmount = 0;
            TotalDuesAmount = 0;
            AdvPaidAmount = 0;
            ApprovedAmount = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int EmployeeSettlementID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime PaymentDate { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameBangla { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeCodeBangla { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public string BUNameBangla { get; set; }
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
        public string DesignationNameBangla { get; set; }
        public int DepartmentID { get; set; }
        public string DeptName { get; set; }
        public string DrptNameBangla { get; set; }
        public DateTime DateOfJoin { get; set; }
        public DateTime DateOfSeparation { get; set; }
        public int JobDurationYears { get; set; }
        public int JobDurationMonths { get; set; }
        public int JobDurationDays { get; set; }
        public double GrossSalary { get; set; }
        public double BasicSalary { get; set; }
        public double PerDayGross { get; set; }
        public double PerDayBasic { get; set; }
        public double OTRatePerHour { get; set; }
        public int PayableDays { get; set; }
        public double PayableAmount { get; set; }
        public int OTHour { get; set; }
        public double OTAmount { get; set; }
        public double AttendanceBonus { get; set; }
        public double FestivalBonus { get; set; }
        public int EarnLeaveDays { get; set; }
        public double EarnLeaveAmount { get; set; }
        public int ServiceBenefitDays { get; set; }
        public double ServiceBenefitAmount { get; set; }
        public int TerminationBenefitDays { get; set; }
        public double TerminationBenefitAmount { get; set; }
        public double TotalPayableAmount { get; set; }
        public int AbsentDays { get; set; }
        public double AbsentAmount { get; set; }
        public int NoticePeriodDays { get; set; }
        public double NoticePeriodAmount { get; set; }
        public int PaidEarnLeaveDays { get; set; }
        public double PaidEarnLeaveAmount { get; set; }
        public double StampAmount { get; set; }
        public double OthersAmount { get; set; }
        public double TotalDeductionAmount { get; set; }
        public double TotalDuesAmount { get; set; }
        public double AdvPaidAmount { get; set; }
        public double ApprovedAmount { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string PaymentDateInSt
        {
            get
            {
                return this.PaymentDate.ToString("dd/MM/yyyy");
            }
        }
       public string DateOfJoinInSt
        {
            get
            {
                return this.DateOfJoin.ToString("dd/MM/yyyy");
            }
        }
       public string DateOfSeparationInSt
       {
           get
           {
               return this.DateOfSeparation.ToString("dd/MM/yyyy");
           }
       }

        public string GrossSalarySt
       {
           get
           {
               return this.GrossSalary.ToString("#,##0;(#,##0)");
           }
       }
        public string BasicSalarySt
        {
            get
            {
                return this.BasicSalary.ToString("#,##0;(#,##0)");
            }
        }
        public string OTRatePerHourSt
        {
            get
            {
                return this.OTRatePerHour.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string PayableAmountSt
        {
            get
            {
                return this.PayableAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string OTAmountSt
        {
            get
            {
                return this.OTAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string AttendanceBonusSt
        {
            get
            {
                return this.AttendanceBonus.ToString("#,##0;(#,##0)");
            }
        }
        public string FestivalBonusSt
        {
            get
            {
                return this.FestivalBonus.ToString("#,##0;(#,##0)");
            }
        }
        public string EarnLeaveAmountSt
        {
            get
            {
                return this.EarnLeaveAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string ServiceBenefitAmountSt
        {
            get
            {
                return this.ServiceBenefitAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string TerminationBenefitAmountSt
        {
            get
            {
                return this.TerminationBenefitAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string TotalPayableAmountSt
        {
            get
            {
                return this.TotalPayableAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string AbsentAmountSt
        {
            get
            {
                return this.AbsentAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string NoticePeriodAmountSt
        {
            get
            {
                return this.NoticePeriodAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string PaidEarnLeaveAmountSt
        {
            get
            {
                return this.PaidEarnLeaveAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string StampAmountSt
        {
            get
            {
                return this.StampAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string OthersAmountSt
        {
            get
            {
                return this.OthersAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string TotalDeductionAmountSt
        {
            get
            {
                return this.TotalDeductionAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string TotalDuesAmountSt
        {
            get
            {
                return this.TotalDuesAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string AdvPaidAmountSt
        {
            get
            {
                return this.AdvPaidAmount.ToString("#,##0;(#,##0)");
            }
        }
        public string ApprovedAmountSt
        {
            get
            {
                return this.ApprovedAmount.ToString("#,##0;(#,##0)");
            }
        }

        public string ApprovedAmountBangla
        {
            get
            {
                return Global.TakaWords(this.ApprovedAmount);
            }
        }
        #endregion

        #region Functions
        public static FinalSettlement Get(int Id, long nUserID)
        {
            return FinalSettlement.Service.Get(Id, nUserID);
        }
      
        #endregion

        #region ServiceFactory
        internal static IFinalSettlementService Service
        {
            get { return (IFinalSettlementService)Services.Factory.CreateService(typeof(IFinalSettlementService)); }
        }

        #endregion
    }
    #endregion

    #region IFinalSettlement interface

    public interface IFinalSettlementService
    {
        FinalSettlement Get(int id, Int64 nUserID);
    }
    #endregion
}

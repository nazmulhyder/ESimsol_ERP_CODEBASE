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
    #region SalaryScheme

    public class SalaryScheme : BusinessObject
    {
        public SalaryScheme()
        {
            SalarySchemeID = 0;
            Name = "";
            NatureOfEmployee = EnumEmployeeNature.None;
            PaymentCycle = EnumPaymentCycle.None;
            Description = "";
            IsAllowBankAccount = true;
            IsAllowOverTime = true;
            OverTimeON = EnumOverTimeON.None;
            DividedBy = 0;
            MultiplicationBy = 0;
            FixedOTRatePerHour = 0;
            CompOverTimeON = EnumOverTimeON.None;
            CompDividedBy = 0;
            CompMultiplicationBy = 0;
            CompFixedOTRatePerHour = 0;
            IsActive = false;
            IsAttendanceDependant = true;
            LateCount = 0;
            EarlyLeavingCount = 0;
            FixedLatePenalty = 0;
            FixedEarlyLeavePenalty = 0;
            IsProductionBase = true;
            StartDay = 0;    
            IsGratuity=false;
            GraturityMaturedInYear=0;
            NoOfMonthCountOneYear = 0;
            GratuityApplyOn = EnumPayrollApplyOn.None;
            ErrorMessage = "";
        }



        #region Properties

        public int SalarySchemeID { get; set; }
        public string Name { get; set; }
        public EnumEmployeeNature NatureOfEmployee { get; set; }
        public EnumPaymentCycle PaymentCycle { get; set; }
        public int PaymentCycleInt { get; set; }
        public string Description { get; set; }
        public bool IsAllowBankAccount { get; set; }
        public bool IsAllowOverTime { get; set; }
        public EnumOverTimeON OverTimeON { get; set; }
        public double DividedBy { get; set; }
        public double MultiplicationBy { get; set; }
        public double FixedOTRatePerHour { get; set; }
        public EnumOverTimeON CompOverTimeON { get; set; }
        public double CompDividedBy { get; set; }
        public double CompMultiplicationBy { get; set; }
        public double CompFixedOTRatePerHour { get; set; }
        public bool IsActive { get; set; }
        public bool IsAttendanceDependant { get; set; }
        public int LateCount { get; set; }
        public int EarlyLeavingCount { get; set; }
        public double FixedLatePenalty { get; set; }
        public double FixedEarlyLeavePenalty { get; set; }
        public bool IsProductionBase { get; set; }
        public int StartDay { get; set; }
        public bool IsGratuity { get; set; }
        public int GraturityMaturedInYear { get; set; }
        public int NoOfMonthCountOneYear { get; set; }
        public EnumPayrollApplyOn GratuityApplyOn { get; set; }        
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public string EncryptSalarySchemeID { get; set; }

        public List<SalarySchemeDetail> SalarySchemeDetails { get; set; }
        public List<ProductionBonus> ProductionBonuss { get; set; }

        public List<SalarySchemeDetailCalculation> SalarySchemeDetailCalculations { get; set; }        

        public int NatureOfEmployeeInt { get; set; }
        public string NatureOfEmployeeInString
        {
            get
            {
                return NatureOfEmployee.ToString();
            }
        }
       
        public string PaymentCycleInString
        {
            get
            {
                return PaymentCycle.ToString();
            }
        }

        public int OverTimeONInt { get; set; }
        public string OverTimeONInString
        {
            get
            {
                return OverTimeON.ToString();
            }
        }
        public int CompOverTimeONInt { get; set; }
        public string CompOverTimeONInString
        {
            get
            {
                return CompOverTimeON.ToString();
            }
        }
        public string GratuityApplyOnInStr
        {
            get
            {
                return GratuityApplyOn.ToString();
            }
        }


        #endregion

        #region Functions
        public static SalaryScheme Get(int id, long nUserID)
        {
            return SalaryScheme.Service.Get(id, nUserID);
        }

        public static List<SalaryScheme> Gets(long nUserID)
        {
            return SalaryScheme.Service.Gets(nUserID);
        }

        public static List<SalaryScheme> Gets(string sSQL, long nUserID)
        {
            return SalaryScheme.Service.Gets(sSQL, nUserID);
        }

        public SalaryScheme IUD(int nDBOperation, long nUserID)
        {
            return SalaryScheme.Service.IUD(this, nDBOperation, nUserID);
        }

        public SalaryScheme SalarySchemeSave(int nDBOperation, long nUserID)
        {
            return SalaryScheme.Service.SalarySchemeSave(this, nDBOperation, nUserID);
        }

        public static SalaryScheme Activite(int nId, bool Active, long nUserID)
        {
            return SalaryScheme.Service.Activite(nId, Active, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static ISalarySchemeService Service
        {
            get { return (ISalarySchemeService)Services.Factory.CreateService(typeof(ISalarySchemeService)); }
        }

        #endregion
    }
    #endregion

    #region ISalaryScheme interface
    public interface ISalarySchemeService
    {
        SalaryScheme Get(int id, Int64 nUserID);
        List<SalaryScheme> Gets(Int64 nUserID);//nUID=UserID
        List<SalaryScheme> Gets(string sSQL, Int64 nUserID);
        SalaryScheme IUD(SalaryScheme oSalaryScheme, int nDBOperation, Int64 nUserID);
        SalaryScheme SalarySchemeSave(SalaryScheme oSalaryScheme, int nDBOperation, Int64 nUserID);
        SalaryScheme Activite(int nId, bool Active, Int64 nUserID);

    }
    #endregion
}

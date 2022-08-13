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
    #region EmployeeSalaryStructureDetail
    [DataContract]
    public class EmployeeSalaryStructureDetail : BusinessObject
    {
        public EmployeeSalaryStructureDetail()
        {
            ESSSID = 0;
            ESSID = 0;
            SalaryHeadID = 0;
            Amount = 0;
            ErrorMessage = "";
            Equation = "";
            AddDate = DateTime.Now;
            UserNameCode = "";
            CompAmount = 0;
            SalaryType = EnumSalaryType.Both;
        }

        #region Properties

        public int ESSSID { get; set; }
        public int ESSID { get; set; }
        public int SalaryHeadID { get; set; }
        public double Amount { get; set; }
        public string ErrorMessage { get; set; }
        public string Equation { get; set; }
        public double CompAmount { get; set; }
        #endregion

        #region Derived Property
        public EnumSalaryType SalaryType { get; set; }
        public int SalaryTypeInt { get; set; }

        public string SalaryTypeInString
        {
            get
            {
                return SalaryType.ToString();
            }
        }
        public DateTime AddDate { get; set; }
        public string AddDateInString
        {
            get
            {
                return AddDate.ToString("dd MMM yyyy");
            }
        }
        public string UserNameCode { get; set; }
        public string Calculation { get; set; }
        public Company Company { get; set; }
        public List<SalaryHead> SalaryHeads { get; set; }
        public List<EmployeeSalaryStructureDetail> EmployeeSalaryStructureDetails { get; set; }
        public List<SalarySchemeDetailCalculation> SalarySchemeDetailCalculations { get; set; }
        public List<EmployeeSalaryStructure> EmployeeSalaryStructures { get; set; }
        public EnumSalaryCalculationOn AllowanceCalculationOn { get; set; }
        
        public int AllowanceCalculationOnInt { get; set; }
        public string AllowanceCalculationOnInString
        {
            get
            {
                return AllowanceCalculationOn.ToString();
            }
        }

        public EnumAllowanceCondition Condition { get; set; }
        public int ConditionInt { get; set; }
        public string ConditionInString
        {
            get
            {
                return Condition.ToString();
            }
        }
        
        public EnumPeriod Period { get; set; }
        
        public int Times { get; set; }
        public int PeriodInt { get; set; }
        public string PeriodInString
        {
            get
            {
                return Times + " Times" + " / " + Period;
            }
        }
        
        public int DeferredDay { get; set; }
        
        public EnumRecruitmentEvent ActivationAfter { get; set; }
        public int ActivationAfterInt { get; set; }
        public string ActivationAfterInString
        {
            get
            {
                return DeferredDay + " Days off " + ActivationAfter;
            }
        }

        public string SalaryHeadName { get; set; }
        
        public string SalaryHeadNameInBangla { get; set; }
        
        public EnumSalaryHeadType SalaryHeadType { get; set; }
        public string AllowanceName { get { if (this.SalaryHeadType == EnumSalaryHeadType.Addition)return SalaryHeadName + "(+)"; else return this.SalaryHeadName + "(-)"; } set { } }
        public string AllowanceType { get { return this.SalaryHeadType.ToString(); } }
        
        #endregion

        #region Functions
        public static EmployeeSalaryStructureDetail Get(int id, long nUserID)
        {
            return EmployeeSalaryStructureDetail.Service.Get(id, nUserID);
        }

        public static List<EmployeeSalaryStructureDetail> Gets(long nUserID)
        {
            return EmployeeSalaryStructureDetail.Service.Gets(nUserID);
        }

        public static List<EmployeeSalaryStructureDetail> Gets(string sSQL, long nUserID)
        {
            return EmployeeSalaryStructureDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<EmployeeSalaryStructureDetail> GetHistorys(string sSQL, long nUserID)
        {
            return EmployeeSalaryStructureDetail.Service.GetHistorys(sSQL, nUserID);
        }
        public EmployeeSalaryStructureDetail IUD(int nDBOperation, long nUserID)
        {
            return EmployeeSalaryStructureDetail.Service.IUD(this, nDBOperation, nUserID);
        }
        public string DeleteSingleSalaryStructureDetail(long nUserID)
        {
            return EmployeeSalaryStructureDetail.Service.DeleteSingleSalaryStructureDetail(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeSalaryStructureDetailService Service
        {
            get { return (IEmployeeSalaryStructureDetailService)Services.Factory.CreateService(typeof(IEmployeeSalaryStructureDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeSalaryStructureDetail interface
    
    public interface IEmployeeSalaryStructureDetailService
    {
        EmployeeSalaryStructureDetail Get(int id, Int64 nUserID);
        List<EmployeeSalaryStructureDetail> Gets(Int64 nUserID);
        List<EmployeeSalaryStructureDetail> Gets(string sSQL, Int64 nUserID);
        List<EmployeeSalaryStructureDetail> GetHistorys(string sSQL, Int64 nUserID);
        EmployeeSalaryStructureDetail IUD(EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail, int nDBOperation, Int64 nUserID);
        string DeleteSingleSalaryStructureDetail(EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail, Int64 nUserID);

    }
    #endregion
}

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
    #region SalarySummary_MAMIYA_NatureWise

    public class SalarySummary_MAMIYA_NatureWise : BusinessObject
    {
        public SalarySummary_MAMIYA_NatureWise()
        {
            DepartmentID = 0;
            DepartmentName = "";
            Wages = 0;
            Salary = 0;
            OTWages = 0;
            OTSalary = 0;
            TotalWages = 0;
            TotalSalary = 0;
            BonusWork = 0;
            BonusOth = 0;
            ErrorMessage = "";
        }

        #region Properties

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public double Wages { get; set; }
        public double Salary  { get; set; }
        public double OTWages { get; set; }
        public double OTSalary  { get; set; }
        public double TotalWages  { get; set; }
        public double TotalSalary  { get; set; }
        public double BonusWork { get; set; }
        public double BonusOth { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public List<SalarySummary_MAMIYA_NatureWise> SalarySummary_MAMIYA_NatureWises { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions
        public static List<SalarySummary_MAMIYA_NatureWise> Gets(DateTime StartDate, DateTime EndDate, string sEmpIDs, int LocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, bool bBankPay, long nUserID)
        {
            return SalarySummary_MAMIYA_NatureWise.Service.Gets(StartDate, EndDate, sEmpIDs, LocationID, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, bBankPay,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ISalarySummary_MAMIYA_NatureWiseService Service
        {
            get { return (ISalarySummary_MAMIYA_NatureWiseService)Services.Factory.CreateService(typeof(ISalarySummary_MAMIYA_NatureWiseService)); }
        }

        #endregion
    }
    #endregion

    #region ISalarySummary_MAMIYA_NatureWise interface

    public interface ISalarySummary_MAMIYA_NatureWiseService
    {
        List<SalarySummary_MAMIYA_NatureWise> Gets(DateTime StartDate, DateTime EndDate, string sEmpIDs, int LocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, bool bBankPay, Int64 nUserID);
    }
    #endregion
}

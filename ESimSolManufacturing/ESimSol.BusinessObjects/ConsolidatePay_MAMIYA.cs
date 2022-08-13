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
    #region ConsolidatePay_MAMIYA
    [DataContract]
    public class ConsolidatePay_MAMIYA : BusinessObject
    {
        public ConsolidatePay_MAMIYA()
        {
            DepartmentName = "";
            Wages = 0;
            Salary = 0;
            OTWages=0;
            OTSalary = 0;
            TotalWages = 0;
            TotalSalary = 0;
            BonusWages = 0;
            BonusSalary = 0;
            ErrorMessage = "";
        }


        #region Properties

        public string DepartmentName { get; set; }
        public double Wages { get; set; }
        public double Salary { get; set; }
        public double OTWages { get; set; }
        public double OTSalary { get; set; }
        public double TotalWages { get; set; }
        public double TotalSalary { get; set; }
        public double BonusWages { get; set; }
        public double BonusSalary { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        #endregion

        #region Functions
        public static List<ConsolidatePay_MAMIYA> Gets( DateTime dtDateFrom, DateTime dtDateTo,string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID, int nPayType, long nUserID)
        {
            return ConsolidatePay_MAMIYA.Service.Gets( dtDateFrom,  dtDateTo,sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nMonthID, nPayType, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IConsolidatePay_MAMIYAService Service
        {
            get { return (IConsolidatePay_MAMIYAService)Services.Factory.CreateService(typeof(IConsolidatePay_MAMIYAService)); }
        }
        #endregion
    }
    #endregion

    #region IConsolidatePay_MAMIYA interface

    public interface IConsolidatePay_MAMIYAService
    {
        List<ConsolidatePay_MAMIYA> Gets(DateTime dtDateFrom, DateTime dtDateTo,string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID, int nPayType, Int64 nUserID);
    }
    #endregion
}

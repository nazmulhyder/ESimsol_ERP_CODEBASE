using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region EmployeeLoanSetup

    public class EmployeeLoanSetup : BusinessObject
    {

        #region  Constructor
        public EmployeeLoanSetup()
        {
            ELSID = 0;
            DeferredDay = 0;
            ActivationAfter = 0;
            LimitInPercentOfPF = 0;
            ApproveBy = 0;
            ApproveDate = DateTime.Today;
            InactiveBy = 0;
            InactiveDate = DateTime.Today;
            IsEmployeeContribution = false;
            IsCompanyContribution = false;
            IsPFProfit = false;
            SalaryHeadID = 0;
            ErrorMessage = string.Empty;
            SalaryHeadName = string.Empty;
            Params = string.Empty;
        }
        #endregion

        #region Properties

        public int ELSID { get; set; }
        public int DeferredDay { get; set; }
        public EnumRecruitmentEvent ActivationAfter { get; set; }
        public double LimitInPercentOfPF { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public int InactiveBy { get; set; }
        public DateTime InactiveDate { get; set; }
        public bool IsEmployeeContribution { get; set; }
        public bool IsCompanyContribution { get; set; }
        public bool IsPFProfit { get; set; }
        public int SalaryHeadID { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derived Properties

        public string ApproveByName { get; set; }
        public string InactiveByName { get; set; }
        public string SalaryHeadName { get; set; }
    
        public string ActivationAfterStr
        {
            get
            {
                return this.ActivationAfter.ToString();
            }
        }
        public string ApproveDateStr
        {
            get
            {
                return (this.ApproveDate == DateTime.MinValue) ? "" : this.ApproveDate.ToString("dd MMM yyyy");
            }
        }

        public string InactiveDateStr
        {
            get
            {
                return (this.InactiveDate == DateTime.MinValue) ? "" : this.InactiveDate.ToString("dd MMM yyyy");
            }
        }

        public string IsEmployeeContributionStr
        {
            get
            {
                return (this.IsEmployeeContribution)?"Yes":"No";
            }
        }
        public string IsCompanyContributionStr
        {
            get
            {
                return (this.IsCompanyContribution)?"Yes":"No";
            }
        }
        public string IsPFProfitStr
        {
            get
            {
                return (this.IsPFProfit)?"Yes":"No";
            }
        }

        #endregion


        #region Functions

        public static EmployeeLoanSetup Get(int nELSID, long nUserID)
        {
            return EmployeeLoanSetup.Service.Get(nELSID, nUserID);
        }
        public static List<EmployeeLoanSetup> Gets(string sSQL, long nUserID)
        {
            return EmployeeLoanSetup.Service.Gets(sSQL, nUserID);
        }
        public EmployeeLoanSetup IUD(int nDBOperation, long nUserID)
        {
            return EmployeeLoanSetup.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeLoanSetupService Service
        {
            get { return (IEmployeeLoanSetupService)Services.Factory.CreateService(typeof(IEmployeeLoanSetupService)); }
        }
        #endregion
    }
    #endregion



    #region IEmployeeLoanSetup interface
    public interface IEmployeeLoanSetupService
    {
        EmployeeLoanSetup Get(int nELSID, long nUserID);

        List<EmployeeLoanSetup> Gets(string sSQL, long nUserID);

        EmployeeLoanSetup IUD(EmployeeLoanSetup oEmployeeLoanSetup, int nDBOperation, long nUserID);
    }
    #endregion
}
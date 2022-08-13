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
    #region ITaxHeadConfiguration

    public class ITaxHeadConfiguration : BusinessObject
    {
        public ITaxHeadConfiguration()
        {

           ITaxHeadConfigurationID = 0;
           SalaryHeadID = 0;
           IsExempted = false;
           MaxExemptAmount = 0;
           CalculationOn = EnumSalaryCalculationOn.None;
           CalculationSalaryHeadID = 0;
           PercentOfCalculation = 0;
           InactiveDate = DateTime.MinValue;
           ErrorMessage = "";
           SalaryHeadName = "";
        }

        #region Properties


        public int ITaxHeadConfigurationID { get; set; }

        public int SalaryHeadID { get; set; }

        public bool IsExempted { get; set; }

        public double MaxExemptAmount { get; set; }

        public EnumSalaryCalculationOn CalculationOn { get; set; }

        public int CalculationSalaryHeadID { get; set; }

        public double PercentOfCalculation { get; set; }

        public DateTime InactiveDate { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public string SalaryHeadName { get; set; }
        public string CalculationHead { get; set; }
        
        public string CalculationOnStr { get { return this.CalculationOn.ToString(); } }
        public string ExemptedStr { get { if (IsExempted == true) return "Exempted"; else return "Full Taxable"; } }
        public string InactiveDateStr { get { return (this.InactiveDate == DateTime.MinValue) ? "" : this.InactiveDate.ToString("dd MMM yyyy"); } }

        public string DescriptionStr
        {
            get
            {
                string sString = "";
                if (IsExempted == true)
                {
                    if (this.SalaryHeadName == "Other Allowance")
                    {
                        sString = "Yearly " + Global.MillionFormat(this.MaxExemptAmount) + " BDT or " + this.PercentOfCalculation + "% of " + this.CalculationHead + "  	whichever is higher will be exampted. Rest of the amount will be treated as taxable Income.";
                    }
                    else
                    {
                        sString = "Yearly " + Global.MillionFormat(this.MaxExemptAmount) + " BDT or " + this.PercentOfCalculation + "% of " + this.CalculationHead + "  	whichever is lower will be exampted. Rest of the amount will be treated as taxable Income.";
                    }
                }else{
                    sString="Full Taxable";
                }
                return sString;
            }
        }

        #endregion

        #region Functions
        public static ITaxHeadConfiguration Get(int Id, long nUserID)
        {
            return ITaxHeadConfiguration.Service.Get(Id, nUserID);
        }
        public static List<ITaxHeadConfiguration> Gets(string sSQL, long nUserID)
        {
            return ITaxHeadConfiguration.Service.Gets(sSQL, nUserID);
        }

        public ITaxHeadConfiguration IUD(int nDBOperation, long nUserID)
        {
            return ITaxHeadConfiguration.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IITaxHeadConfigurationService Service
        {
            get { return (IITaxHeadConfigurationService)Services.Factory.CreateService(typeof(IITaxHeadConfigurationService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxHeadConfiguration interface

    public interface IITaxHeadConfigurationService
    {
        ITaxHeadConfiguration Get(int id, Int64 nUserID);
        List<ITaxHeadConfiguration> Gets(string sSQL, Int64 nUserID);
        ITaxHeadConfiguration IUD(ITaxHeadConfiguration oITaxHeadConfiguration, int nDBOperation, Int64 nUserID);
    }
    #endregion
}

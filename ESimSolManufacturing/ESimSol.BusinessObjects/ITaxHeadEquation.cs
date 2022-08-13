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
    #region ITaxHeadEquation

    public class ITaxHeadEquation : BusinessObject
    {
        public ITaxHeadEquation()
        {

            ITaxHeadEquationID = 0;
            ITaxHeadConfigurationID = 0;
            CalculateOn = EnumSalaryCalculationOn.None;
            Value = 0;
            SalaryHeadID = 0;
            ErrorMessage = "";

        }

        #region Properties

        public int ITaxHeadEquationID { get; set; }
        public int ITaxHeadConfigurationID { get; set; }
        public EnumSalaryCalculationOn CalculateOn { get; set; }
        public double Value { get; set; }
        public int SalaryHeadID { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public int CalculateOnInt { get; set; }
        public string CalculateOnInString
        {
            get
            {
                return CalculateOn.ToString();
            }
        }

        public string SalaryHeadName { get; set; }
        public string Description
        {
            get
            {
                if (CalculateOn == EnumSalaryCalculationOn.Gross) return Value.ToString() + " % of " + CalculateOn.ToString();
                else if (CalculateOn == EnumSalaryCalculationOn.SalaryItem) return Value.ToString() + " % of " + SalaryHeadName;
                else return Value.ToString();

            }
        }
        #endregion

        #region Functions
        public static ITaxHeadEquation Get(int Id, long nUserID)
        {
            return ITaxHeadEquation.Service.Get(Id, nUserID);
        }
        public static ITaxHeadEquation Get(string sSQL, long nUserID)
        {
            return ITaxHeadEquation.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxHeadEquation> Gets(long nUserID)
        {
            return ITaxHeadEquation.Service.Gets(nUserID);
        }

        public static List<ITaxHeadEquation> Gets(string sSQL, long nUserID)
        {
            return ITaxHeadEquation.Service.Gets(sSQL, nUserID);
        }

        public ITaxHeadEquation IUD(int nDBOperation, long nUserID)
        {
            return ITaxHeadEquation.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IITaxHeadEquationService Service
        {
            get { return (IITaxHeadEquationService)Services.Factory.CreateService(typeof(IITaxHeadEquationService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxHeadEquation interface

    public interface IITaxHeadEquationService
    {
        ITaxHeadEquation Get(int id, Int64 nUserID);
        ITaxHeadEquation Get(string sSQL, Int64 nUserID);
        List<ITaxHeadEquation> Gets(Int64 nUserID);
        List<ITaxHeadEquation> Gets(string sSQL, Int64 nUserID);
        ITaxHeadEquation IUD(ITaxHeadEquation oITaxHeadEquation, int nDBOperation, Int64 nUserID);

    }
    #endregion
}

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
    #region ITaxRateScheme

    public class ITaxRateScheme : BusinessObject
    {
        public ITaxRateScheme()
        {

            ITaxRateSchemeID = 0;
            ITaxAssessmentYearID = 0;
            SalaryHeadID = 0;
            TaxPayerType = EnumTaxPayerType.None;
            TaxArea = EnumTaxArea.None;
            MinimumTax = 0;
            IsActive = true;
            ErrorMessage = "";

        }

        #region Properties

        public int ITaxRateSchemeID { get; set; }
        public int ITaxAssessmentYearID { get; set; }
        public int SalaryHeadID { get; set; }
        public EnumTaxPayerType TaxPayerType { get; set; }
        public EnumTaxArea TaxArea { get; set; }
        public double MinimumTax { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<ITaxRateSlab> ITaxRateSlabs { get; set; }
        public List<ITaxRebateScheme> ITaxRebateSchemes { get; set; }
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public int TaxPayerTypeInint { get; set; }
        public string TaxPayerTypeString
        {
            get
            {
                return TaxPayerType.ToString();
            }
        }
        public int TaxAreaInint { get; set; }
        public string TaxAreaString
        {
            get
            {
                return TaxArea.ToString();
            }
        }

        #endregion

        #region Functions
        public static ITaxRateScheme Get(int Id, long nUserID)
        {
            return ITaxRateScheme.Service.Get(Id, nUserID);
        }
        public static ITaxRateScheme Get(string sSQL, long nUserID)
        {
            return ITaxRateScheme.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxRateScheme> Gets(long nUserID)
        {
            return ITaxRateScheme.Service.Gets(nUserID);
        }

        public static List<ITaxRateScheme> Gets(string sSQL, long nUserID)
        {
            return ITaxRateScheme.Service.Gets(sSQL, nUserID);
        }

        public ITaxRateScheme IUD(int nDBOperation, long nUserID)
        {
            return ITaxRateScheme.Service.IUD(this, nDBOperation, nUserID);
        }

        public static ITaxRateScheme Activite(int nId, bool Active, long nUserID)
        {
            return ITaxRateScheme.Service.Activite(nId, Active, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IITaxRateSchemeService Service
        {
            get { return (IITaxRateSchemeService)Services.Factory.CreateService(typeof(IITaxRateSchemeService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxRateScheme interface

    public interface IITaxRateSchemeService
    {
        ITaxRateScheme Get(int id, Int64 nUserID);
        ITaxRateScheme Get(string sSQL, Int64 nUserID);
        List<ITaxRateScheme> Gets(Int64 nUserID);
        List<ITaxRateScheme> Gets(string sSQL, Int64 nUserID);
        ITaxRateScheme IUD(ITaxRateScheme oITaxRateScheme, int nDBOperation, Int64 nUserID);
        ITaxRateScheme Activite(int nId, bool Active, Int64 nUserID);
    }
    #endregion
}

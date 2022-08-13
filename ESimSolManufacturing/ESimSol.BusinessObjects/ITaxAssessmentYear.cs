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
    #region ITaxAssessmentYear

    public class ITaxAssessmentYear : BusinessObject
    {
        public ITaxAssessmentYear()
        {

            ITaxAssessmentYearID = 0;
            Description = "";
            IsActive = true;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            ErrorMessage = "";

        }

        #region Properties


        public int ITaxAssessmentYearID { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Freeze"; } }
        public string StartDateInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateInString
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }

        public string Session
        {
            get
            {
                return StartDateInString + "-" + EndDateInString;
            }
        }
        public string IncomeYear
        {
            get
            {
                return  (StartDate.Year-1) + "-" + (EndDate.Year-1);
            }
        }
        public string AssessmentYear
        {
            get
            {
                return StartDate.Year + "-" + EndDate.Year;
            }
        }


        #endregion

        #region Functions
        public static ITaxAssessmentYear Get(int Id, long nUserID)
        {
            return ITaxAssessmentYear.Service.Get(Id, nUserID);
        }
        public static ITaxAssessmentYear Get(string sSQL, long nUserID)
        {
            return ITaxAssessmentYear.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxAssessmentYear> Gets(long nUserID)
        {
            return ITaxAssessmentYear.Service.Gets(nUserID);
        }

        public static List<ITaxAssessmentYear> Gets(string sSQL, long nUserID)
        {
            return ITaxAssessmentYear.Service.Gets(sSQL, nUserID);
        }

        public ITaxAssessmentYear IUD(int nDBOperation, long nUserID)
        {
            return ITaxAssessmentYear.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IITaxAssessmentYearService Service
        {
            get { return (IITaxAssessmentYearService)Services.Factory.CreateService(typeof(IITaxAssessmentYearService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxAssessmentYear interface

    public interface IITaxAssessmentYearService
    {
        ITaxAssessmentYear Get(int id, Int64 nUserID);
        ITaxAssessmentYear Get(string sSQL, Int64 nUserID);
        List<ITaxAssessmentYear> Gets(Int64 nUserID);
        List<ITaxAssessmentYear> Gets(string sSQL, Int64 nUserID);
        ITaxAssessmentYear IUD(ITaxAssessmentYear oITaxAssessmentYear, int nDBOperation, Int64 nUserID);

    }
    #endregion
}

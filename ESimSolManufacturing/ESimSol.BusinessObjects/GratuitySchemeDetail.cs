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
    #region GratuitySchemeDetail

    public class GratuitySchemeDetail : BusinessObject
    {
        public GratuitySchemeDetail()
        {
            GSDID=0;
            GSID = 0;
            MaturityYearStart=0;
            MaturityYearEnd=0;
            ActivationAfter = EnumRecruitmentEvent.None;
            ValueInPercent=0;
            GratuityApplyOn=EnumPayrollApplyOn.None;
            NoOfMonthCountOneYear=0;
            ActiveDate = DateTime.Now;
            InactiveDate = DateTime.Now;
            ErrorMessage = "";
        }

        #region Properties
        public int GSDID { get; set; }
        public int GSID { get; set; }
        public int MaturityYearStart { get; set; }
        public int MaturityYearEnd { get; set; }
        public EnumRecruitmentEvent ActivationAfter { get; set; }
        public int ValueInPercent { get; set; }
        public EnumPayrollApplyOn GratuityApplyOn { get; set; }
        public int NoOfMonthCountOneYear { get; set; }
        public DateTime ActiveDate { get; set; }

        public DateTime InactiveDate { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string ActiveDateInString
        {
            get
            {
                return ActiveDate.ToString("dd MMM yyyy");
            }
        }
        public string InactiveDateInString
        {
            get
            {
                return InactiveDate.ToString("dd MMM yyyy");
            }
        }

        public string ActivationAfterInString
        {
            get
            {
                return ActivationAfter.ToString();
            }
        }
        public string GratuityApplyOnInString
        {
            get
            {
                return GratuityApplyOn.ToString();
            }
        }

        public string MaturityInString
        {
            get
            {
                return this.MaturityYearStart.ToString() + "-" + this.MaturityYearEnd.ToString() + " years of " + this.ActivationAfterInString;
            }
        }

        public string BenefitInString
        {
            get
            {
                return this.ValueInPercent.ToString() + " % of last " + this.GratuityApplyOnInString;
            }
        }

        public string FractionMonthInString
        {
            get
            {
                return this.NoOfMonthCountOneYear.ToString() + " month cross";
            }
        }

        public GratuityScheme GratuityScheme { get; set; }
        #endregion

        #region Functions
        public static GratuitySchemeDetail Get(int Id, long nUserID)
        {
            return GratuitySchemeDetail.Service.Get(Id, nUserID);
        }
        public static GratuitySchemeDetail Get(string sSQL, long nUserID)
        {
            return GratuitySchemeDetail.Service.Get(sSQL, nUserID);
        }
        public static List<GratuitySchemeDetail> Gets(long nUserID)
        {
            return GratuitySchemeDetail.Service.Gets(nUserID);
        }

        public static List<GratuitySchemeDetail> Gets(string sSQL, long nUserID)
        {
            return GratuitySchemeDetail.Service.Gets(sSQL, nUserID);
        }

        public GratuitySchemeDetail IUD(int nDBOperation, long nUserID)
        {
            return GratuitySchemeDetail.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IGratuitySchemeDetailService Service
        {
            get { return (IGratuitySchemeDetailService)Services.Factory.CreateService(typeof(IGratuitySchemeDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IGratuitySchemeDetail interface

    public interface IGratuitySchemeDetailService
    {
        GratuitySchemeDetail Get(int id, Int64 nUserID);
        GratuitySchemeDetail Get(string sSQL, Int64 nUserID);
        List<GratuitySchemeDetail> Gets(Int64 nUserID);
        List<GratuitySchemeDetail> Gets(string sSQL, Int64 nUserID);
        GratuitySchemeDetail IUD(GratuitySchemeDetail oGratuitySchemeDetail, int nDBOperation, Int64 nUserID);

    }
    #endregion
}

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
    #region ITaxRebatePayment

    public class ITaxRebatePayment : BusinessObject
    {
        public ITaxRebatePayment()
        {

            ITaxRebatePaymentID = 0;
            ITaxAssessmentYearID = 0;
            EmployeeID = 0;
            ITaxRebateItemID = 0;
            Amount = 0;
            Note = "";
            ErrorMessage = "";
            AssessmentYear = "";
            EmployeeNameCode = "";
            ITaxRebateType = EnumITaxRebateType.None;
        }

        #region Properties

        public int ITaxRebatePaymentID { get; set; }
        public int ITaxAssessmentYearID { get; set; }
        public int EmployeeID { get; set; }
        public int ITaxRebateItemID { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string AssessmentYear { get; set; }
        public string EmployeeNameCode { get; set; }
        public string EmployeeOfficial { get; set; }
        public string Description { get; set; }
        public EnumITaxRebateType ITaxRebateType { get; set; }
        public string ITaxRebateTypeString
        {
            get
            {
                return ITaxRebateType.ToString();
            }
        }
        #endregion

        #region Functions
        public static ITaxRebatePayment Get(int Id, long nUserID)
        {
            return ITaxRebatePayment.Service.Get(Id, nUserID);
        }
        public static ITaxRebatePayment Get(string sSQL, long nUserID)
        {
            return ITaxRebatePayment.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxRebatePayment> Gets(long nUserID)
        {
            return ITaxRebatePayment.Service.Gets(nUserID);
        }

        public static List<ITaxRebatePayment> Gets(string sSQL, long nUserID)
        {
            return ITaxRebatePayment.Service.Gets(sSQL, nUserID);
        }

        public ITaxRebatePayment IUD(int nDBOperation, long nUserID)
        {
            return ITaxRebatePayment.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IITaxRebatePaymentService Service
        {
            get { return (IITaxRebatePaymentService)Services.Factory.CreateService(typeof(IITaxRebatePaymentService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxRebatePayment interface

    public interface IITaxRebatePaymentService
    {
        ITaxRebatePayment Get(int id, Int64 nUserID);
        ITaxRebatePayment Get(string sSQL, Int64 nUserID);
        List<ITaxRebatePayment> Gets(Int64 nUserID);
        List<ITaxRebatePayment> Gets(string sSQL, Int64 nUserID);
        ITaxRebatePayment IUD(ITaxRebatePayment oITaxRebatePayment, int nDBOperation, Int64 nUserID);


    }
    #endregion
}

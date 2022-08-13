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
    #region ITaxAdvancePayment

    public class ITaxAdvancePayment : BusinessObject
    {
        public ITaxAdvancePayment()
        {

            ITaxAdvancePaymentID = 0;
            ITaxAssessmentYearID = 0;
            EmployeeID = 0;
            Amount = 0;
            Note = "";
            ErrorMessage = "";
            AssessmentYear = "";
            EmployeeNameCode = "";

        }

        #region Properties
        public int ITaxAdvancePaymentID { get; set; }
        public int ITaxAssessmentYearID { get; set; }
        public int EmployeeID { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string AssessmentYear { get; set; }
        public string EmployeeNameCode { get; set; }
        public string EmployeeOfficial { get; set; }
        #endregion

        #region Functions
        public static ITaxAdvancePayment Get(int Id, long nUserID)
        {
            return ITaxAdvancePayment.Service.Get(Id, nUserID);
        }
        public static ITaxAdvancePayment Get(string sSQL, long nUserID)
        {
            return ITaxAdvancePayment.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxAdvancePayment> Gets(long nUserID)
        {
            return ITaxAdvancePayment.Service.Gets(nUserID);
        }

        public static List<ITaxAdvancePayment> Gets(string sSQL, long nUserID)
        {
            return ITaxAdvancePayment.Service.Gets(sSQL, nUserID);
        }

        public ITaxAdvancePayment IUD(int nDBOperation, long nUserID)
        {
            return ITaxAdvancePayment.Service.IUD(this, nDBOperation, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IITaxAdvancePaymentService Service
        {
            get { return (IITaxAdvancePaymentService)Services.Factory.CreateService(typeof(IITaxAdvancePaymentService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxAdvancePayment interface

    public interface IITaxAdvancePaymentService
    {
        ITaxAdvancePayment Get(int id, Int64 nUserID);
        ITaxAdvancePayment Get(string sSQL, Int64 nUserID);
        List<ITaxAdvancePayment> Gets(Int64 nUserID);
        List<ITaxAdvancePayment> Gets(string sSQL, Int64 nUserID);
        ITaxAdvancePayment IUD(ITaxAdvancePayment oITaxAdvancePayment, int nDBOperation, Int64 nUserID);


    }
    #endregion
}

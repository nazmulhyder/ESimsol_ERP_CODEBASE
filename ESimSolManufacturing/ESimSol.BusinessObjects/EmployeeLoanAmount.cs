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
    #region EmployeeLoanAmount

    public class EmployeeLoanAmount : BusinessObject
    {

        #region  Constructor
        public EmployeeLoanAmount()
        {
            ELAID = 0;
            EmployeeLoanID = 0;
            LoanRequestID = 0;
            Amount = 0;
            Note = string.Empty;
            ApproveBy = 0;
            ApproveDate = DateTime.Today;
            LoanDisburseDate = DateTime.Today;
            ErrorMessage = string.Empty;
            Params = string.Empty;
        }
        #endregion

        #region Properties
        public int ELAID { get; set; }
        public int EmployeeLoanID { get; set; }
        public int LoanRequestID { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime LoanDisburseDate { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derived Properties

        public string ApproveDateStr
        {
            get
            {
                return this.ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public string LoanDisburseDateStr
        {
            get
            {
                return this.LoanDisburseDate.ToString("dd MMM yyyy");
            }
        }
        

        #endregion


        #region Functions

        public static EmployeeLoanAmount Get(int nEmployeeLoanAmountID, long nUserID)
        {
            return EmployeeLoanAmount.Service.Get(nEmployeeLoanAmountID, nUserID);
        }
        public static List<EmployeeLoanAmount> Gets(string sSQL, long nUserID)
        {
            return EmployeeLoanAmount.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeLoanAmountService Service
        {
            get { return (IEmployeeLoanAmountService)Services.Factory.CreateService(typeof(IEmployeeLoanAmountService)); }
        }
        #endregion
    }
    #endregion



    #region IEmployeeLoanAmount interface
    public interface IEmployeeLoanAmountService
    {
        EmployeeLoanAmount Get(int nEmployeeLoanAmountID, long nUserID);

        List<EmployeeLoanAmount> Gets(string sSQL, long nUserID);
    }
    #endregion
}
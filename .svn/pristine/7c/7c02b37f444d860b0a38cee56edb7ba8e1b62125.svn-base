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
    #region EmployeeLoanRefund

    public class EmployeeLoanRefund : BusinessObject
    {

        #region  Constructor
        public EmployeeLoanRefund()
        {
            ELRID = 0;
            EmployeeLoanID = 0;
            NoOfInstallmentRefund = 0;
            Amount = 0;
            ServiceCharge = 0;
            Note = "";
            RefundNo = string.Empty;
            RefundDate = DateTime.Today;
            ErrorMessage = string.Empty;
            Params = string.Empty;

        }
        #endregion

        #region Properties

        public int ELRID { get; set; }
        public int EmployeeLoanID { get; set; }
        public int NoOfInstallmentRefund { get; set; }
        public double Amount { get; set; }
        public double ServiceCharge { get; set; }
        public string Note { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string RefundNo { get; set; }
        public DateTime RefundDate { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derived Properties

        public string LoanCode { get; set; }
        public string ApproveByName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }

        public string RefundDateStr
        {
            get
            {
                return this.RefundDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproveDateStr
        {
            get
            {
                return (this.ApproveDate == DateTime.MinValue) ? "" : this.ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public string EmployeeNameCode
        {
            get
            {
                return this.EmployeeName + " [" + this.EmployeeCode + "]";
            }
        }

        #endregion


        #region Functions

        public static EmployeeLoanRefund Get(int nELRID, long nUserID)
        {
            return EmployeeLoanRefund.Service.Get(nELRID, nUserID);
        }
        public static List<EmployeeLoanRefund> Gets(string sSQL, long nUserID)
        {
            return EmployeeLoanRefund.Service.Gets(sSQL, nUserID);
        }
        public EmployeeLoanRefund IUD(int nDBOperation, long nUserID)
        {
            return EmployeeLoanRefund.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeLoanRefundService Service
        {
            get { return (IEmployeeLoanRefundService)Services.Factory.CreateService(typeof(IEmployeeLoanRefundService)); }
        }
        #endregion
    }
    #endregion



    #region IEmployeeLoanRefund interface
    public interface IEmployeeLoanRefundService
    {
        EmployeeLoanRefund Get(int nELRID, long nUserID);

        List<EmployeeLoanRefund> Gets(string sSQL, long nUserID);

        EmployeeLoanRefund IUD(EmployeeLoanRefund oEmployeeLoanRefund, int nDBOperation, long nUserID);
    }
    #endregion
}
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
    #region EmployeeLoanDisbursementPolicy

    public class EmployeeLoanDisbursementPolicy : BusinessObject
    {
        public EmployeeLoanDisbursementPolicy()
        {

            ELDPID = 0;
            EmployeeLoanID = 0;
            Amount = 0;
            ReceivableAmount = 0;
            IsDisbursed = false;
            ExpectedDisburseDate = DateTime.Now;
            ActualDisburseDate = DateTime.Now;
            ErrorMessage = "";

        }

        #region Properties

        public int ELDPID { get; set; }
        public int EmployeeLoanID { get; set; }
        public double Amount { get; set; }
        public double ReceivableAmount { get; set; }
        public bool IsDisbursed { get; set; }
        public DateTime ExpectedDisburseDate { get; set; }
        public DateTime ActualDisburseDate { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public string ExpectedDisburseDateInString
        {
            get
            {
                return ExpectedDisburseDate.ToString("dd MMM yyyy");
            }
        }
        public string ActualDisburseDateInString
        {
            get
            {
                return ActualDisburseDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions
        public static EmployeeLoanDisbursementPolicy Get(int Id, long nUserID)
        {
            return EmployeeLoanDisbursementPolicy.Service.Get(Id, nUserID);
        }
        public static EmployeeLoanDisbursementPolicy Get(string sSQL, long nUserID)
        {
            return EmployeeLoanDisbursementPolicy.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeLoanDisbursementPolicy> Gets(long nUserID)
        {
            return EmployeeLoanDisbursementPolicy.Service.Gets(nUserID);
        }

        public static List<EmployeeLoanDisbursementPolicy> Gets(string sSQL, long nUserID)
        {
            return EmployeeLoanDisbursementPolicy.Service.Gets(sSQL, nUserID);
        }

        public EmployeeLoanDisbursementPolicy IUD(int nDBOperation, long nUserID)
        {
            return EmployeeLoanDisbursementPolicy.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeLoanDisbursementPolicyService Service
        {
            get { return (IEmployeeLoanDisbursementPolicyService)Services.Factory.CreateService(typeof(IEmployeeLoanDisbursementPolicyService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeLoanDisbursementPolicy interface

    public interface IEmployeeLoanDisbursementPolicyService
    {
        EmployeeLoanDisbursementPolicy Get(int id, Int64 nUserID);
        EmployeeLoanDisbursementPolicy Get(string sSQL, Int64 nUserID);
        List<EmployeeLoanDisbursementPolicy> Gets(Int64 nUserID);
        List<EmployeeLoanDisbursementPolicy> Gets(string sSQL, Int64 nUserID);
        EmployeeLoanDisbursementPolicy IUD(EmployeeLoanDisbursementPolicy oEmployeeLoanDisbursementPolicy, int nDBOperation, Int64 nUserID);

    }
    #endregion
}

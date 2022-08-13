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
    #region EmployeeLoanInstallmentPolicy

    public class EmployeeLoanInstallmentPolicy : BusinessObject
    {
        public EmployeeLoanInstallmentPolicy()
        {

            ELIPID = 0;
            EmployeeLoanID = 0;
            Amount = 0;
            IsRealized = false;
            ExpectedRealizeDate = DateTime.Now;
            ActualRealizeDate = DateTime.Now;
            RealizeNote = "";
            ErrorMessage = "";

        }

        #region Properties

        public int ELIPID { get; set; }
        public int EmployeeLoanID { get; set; }
        public double Amount { get; set; }
        public bool IsRealized { get; set; }
        public DateTime ExpectedRealizeDate { get; set; }
        public DateTime ActualRealizeDate { get; set; }
        public string RealizeNote { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public string ExpectedRealizeDateInString
        {
            get
            {
                return ExpectedRealizeDate.ToString("dd MMM yyyy");
            }
        }
        public string ActualRealizeDateDateInString
        {
            get
            {
                return ActualRealizeDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static EmployeeLoanInstallmentPolicy Get(int Id, long nUserID)
        {
            return EmployeeLoanInstallmentPolicy.Service.Get(Id, nUserID);
        }
        public static EmployeeLoanInstallmentPolicy Get(string sSQL, long nUserID)
        {
            return EmployeeLoanInstallmentPolicy.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeLoanInstallmentPolicy> Gets(long nUserID)
        {
            return EmployeeLoanInstallmentPolicy.Service.Gets(nUserID);
        }

        public static List<EmployeeLoanInstallmentPolicy> Gets(string sSQL, long nUserID)
        {
            return EmployeeLoanInstallmentPolicy.Service.Gets(sSQL, nUserID);
        }

        public EmployeeLoanInstallmentPolicy IUD(int nDBOperation, long nUserID)
        {
            return EmployeeLoanInstallmentPolicy.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeLoanInstallmentPolicyService Service
        {
            get { return (IEmployeeLoanInstallmentPolicyService)Services.Factory.CreateService(typeof(IEmployeeLoanInstallmentPolicyService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeLoanInstallmentPolicy interface

    public interface IEmployeeLoanInstallmentPolicyService
    {
        EmployeeLoanInstallmentPolicy Get(int id, Int64 nUserID);
        EmployeeLoanInstallmentPolicy Get(string sSQL, Int64 nUserID);
        List<EmployeeLoanInstallmentPolicy> Gets(Int64 nUserID);
        List<EmployeeLoanInstallmentPolicy> Gets(string sSQL, Int64 nUserID);
        EmployeeLoanInstallmentPolicy IUD(EmployeeLoanInstallmentPolicy oEmployeeLoanInstallmentPolicy, int nDBOperation, Int64 nUserID);

    }
    #endregion
}

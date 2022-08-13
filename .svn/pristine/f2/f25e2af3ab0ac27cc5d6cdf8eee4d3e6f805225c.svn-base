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
    #region EmployeeLoanInstallment

    public class EmployeeLoanInstallment : BusinessObject
    {

        #region  Constructor
        public EmployeeLoanInstallment()
        {
            ELInstallmentID = 0;
            EmployeeLoanID = 0;
            InstallmentAmount = 0;
            InterestPerInstallment = 0;
            InstallmentDate = DateTime.Today;
            ESDetailID = 0;
            ErrorMessage = string.Empty;
            Params = string.Empty;
        }
        #endregion

        #region Properties
        public int ELInstallmentID { get; set; }
        public int EmployeeLoanID { get; set; }
        public double InstallmentAmount { get; set; }
        public double InterestPerInstallment { get; set; }
        public DateTime InstallmentDate { get; set; }
        public int ESDetailID { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derived Properties

        public double TotalAmount { get; set; }
        public double Balance { get; set; }
        public int Type { get; set; }
        public string Encash 
        {
            get
            {
                return (this.ESDetailID > 0) ? "Done" : "--";
            }
        }

        public string InstallmentDateStr
        {
            get
            {
                return this.InstallmentDate.ToString("dd MMM yyyy");
            }
        }

        

        #endregion


        #region Functions

        public EmployeeLoanInstallment IUD(int nDBOperation, long nUserID)
        {
            return EmployeeLoanInstallment.Service.IUD(this, nDBOperation, nUserID);
        }
        public static EmployeeLoanInstallment Get(int nEmployeeLoanInstallmentID, long nUserID)
        {
            return EmployeeLoanInstallment.Service.Get(nEmployeeLoanInstallmentID, nUserID);
        }
        public static List<EmployeeLoanInstallment> Gets(string sSQL, long nUserID)
        {
            return EmployeeLoanInstallment.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeLoanInstallmentService Service
        {
            get { return (IEmployeeLoanInstallmentService)Services.Factory.CreateService(typeof(IEmployeeLoanInstallmentService)); }
        }
        #endregion
    }
    #endregion



    #region IEmployeeLoanInstallment interface
    public interface IEmployeeLoanInstallmentService
    {
        EmployeeLoanInstallment IUD(EmployeeLoanInstallment oEmployeeLoanInstallment, int nDBOperation, long nUserID);

        EmployeeLoanInstallment Get(int nEmployeeLoanInstallmentID, long nUserID);

        List<EmployeeLoanInstallment> Gets(string sSQL, long nUserID);
    }
    #endregion
}